using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Threading;
using Lite.ExcelLibrary.SpreadSheet;
using System.IO;

namespace YieldCurveSL
{
    using YieldCurveSrv;
    public partial class Page1 : Page
    {
        YieldCurveSrvClient _service;

		public Dictionary<long, ObservableCollection<EntryPointChart>> entryLineDic = new Dictionary<long, ObservableCollection<EntryPointChart>>();
		public Dictionary<long, ObservableCollection<EntryPointChart>> frwLineDic = new Dictionary<long, ObservableCollection<EntryPointChart>>();
		public Dictionary<long, ObservableCollection<EntryPointChart>> zCLineDic = new Dictionary<long, ObservableCollection<EntryPointChart>>();
		public Dictionary<KeyValuePair<long, long>, ObservableCollection<EntryPointChart>> diffLineDic =
			new Dictionary<KeyValuePair<long, long>, ObservableCollection<EntryPointChart>>(); // map of {curveId1, curveId2} -> {set of DataPoints}

        private RateGUI childRateWindow; 
        private BondGUI childBondWindow; 
		private NewSettings newSettings;
        private InflationRateGUI childInflationRateWindow;
     
        public Page1()
        {
            InitializeComponent();

			newSettings = new NewSettings();
      
            CurrentElements.CurrentYCId = -1;
            CurrentElements.CurrentICId = -1;
            CurrentElements.CurrentInflationCurveSnapshot = null;
			CurrentElements.CurrentDate = DateTime.Today;
            
			this.datePicker1.SelectedDate = CurrentElements.CurrentDate;

            //
            EventRangeSlider += new EventHandler(OnEventRangeSlider);
            //

            _service = new YieldCurveSrvClient();       

            // registering event handlers only once here
			_service.InitCompleted +=
				new EventHandler<InitCompletedEventArgs>(proxy_InitCompleted);

            _service.InflationInitCompleted +=
                new EventHandler<InflationInitCompletedEventArgs>(proxy_InflationInitCompleted);

            _service.GetRateDataDicCompleted +=
                new EventHandler<GetRateDataDicCompletedEventArgs>(proxy_GetRateDataDicCompleted);

            _service.GetBondDataDicCompleted +=
                new EventHandler<GetBondDataDicCompletedEventArgs>(proxy_GetBondDataDicCompleted);

            _service.GetEntryPointHistoryListCompleted +=
                new EventHandler<GetEntryPointHistoryListCompletedEventArgs>(proxy_GetEntryPointHistoryListCompleted);

            _service.CalculateDiscountedRateListCompleted +=
                new EventHandler<CalculateDiscountedRateListCompletedEventArgs>(proxy_CalculateDiscountedRateListCompleted);

            _service.CalculateInflationRateListCompleted +=
                new EventHandler<CalculateInflationRateListCompletedEventArgs>(proxy_CalculateInflationRateListCompleted);

            _service.GetMaturityDatesListCompleted +=
               new EventHandler<GetMaturityDatesListCompletedEventArgs>(proxy_GetMaturityDatesListCompleted);

			// TEST
			this.button4.IsEnabled = false;
			this.tabItem5.IsEnabled = false;
			this.button5.IsEnabled = false;
        }


        #region --------------- Old treeview control ----------------------------

        /*

        /// <summary>
        /// Tree View (left pannel)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_Loaded(object sender, RoutedEventArgs e)
        {
			_service.InitAsync(CurrentElements.CurrentDate);
		}

        /// <summary>
        /// Chart: Blue Line 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void treeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //??? make a check if entry rates were already cached

            CcyItem.YcItem tmp = (e.NewValue as CcyItem.YcItem);
            if (tmp == null)
                return;

            if (CurrentElements.CurrentYCId == tmp.idYc)
                return;

            DrawEntryPointChartFromCache(tmp.idYc, null);

            CurrentElements.CurrentYCId = tmp.idYc;
            int curve_id = tmp.idYc;

            if (
                CachedData.CachedYieldCurvesDic.ContainsKey(curve_id)				//everything is already cached
                && CachedData.CachedYieldCurvesDic[curve_id].Points.Count != 0
                )
            {
                if (YcSettingsDic.GetYcSett(curve_id).ifZCCurve)
                    DrawResultCurveFromCache(curve_id, true, 1, YcSettingsDic.GetYcSett(curve_id).ZCColor, 1);

                if (YcSettingsDic.GetYcSett(curve_id).ifForwardCurve)
                    DrawResultCurveFromCache(curve_id, false, 2, YcSettingsDic.GetYcSett(curve_id).FrwColor, 1);

                FillOutputDataGridFromCache(null);
                DrawEntryPointGridFromCache(tmp.idYc, null);
            }

            ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();
           
      //      foreach (YieldCurveEntryDataHistory y in CachedData.CachedEntryPointHistoryList)
      //      {
	//			if (y.YieldCurveId != curve_id)
	//				continue;
        //
       //         dList.Add(CurrentElements.CurrentDate.AddDays(y.Duration));
       //     }
			
#if __VC10__
            foreach (DateTime d in CachedData.CommonDates)
#else
			foreach ( DateTime d in CachedData.CommonDates.Keys)
#endif
                dList.Add(d);

            YieldCurveData ycd = YcSettingsDic.GetYcSett(curve_id).ycd;
            _service.CalculateDiscountedRateListAsync(ycd, CurrentElements.CurrentDate, dList, true);
        }

        */

        #endregion


        void proxy_InitCompleted(object sender, InitCompletedEventArgs e)
        {
            if (e.Result.Error != null)
            {
                MessageBox.Show(e.Result.Error.Message);
                return;
            }

			if (e.Result.ccy != null && e.Result.ccy.CurrencyDic != null)
				CachedData.CachedCurrencyDic = e.Result.ccy.CurrencyDic.ToDictionary(i => i.Key, i => i.Value);

			if (e.Result.r != null && e.Result.r.RateDic != null)
				CachedData.CachedRatesDic = e.Result.r.RateDic.ToDictionary(i => i.Key, i => i.Value);

			if (e.Result.b != null && e.Result.b.BondDic != null)
				CachedData.CachedBondsDic = e.Result.b.BondDic.ToDictionary(i => i.Key, i => i.Value);

			if (e.Result.dc != null && e.Result.dc.DayCounterDic != null)
				CachedData.CachedDayCounterDic = e.Result.dc.DayCounterDic.ToDictionary(i => i.Key, i => i.Value);

			if (e.Result.xr != null && e.Result.xr.XRateDic != null)
				CachedData.CachedExchangeRatesDic = e.Result.xr.XRateDic.ToDictionary(i => i.Key, i => i.Value);

			if (e.Result.edh != null && e.Result.edh.EntryPointHistoryList != null)
			{
				CachedData.CachedEntryPointHistoryList = e.Result.edh.EntryPointHistoryList.ToList(); // bond is taken care of maturity already, rates are there too

                // populate the common calendar in cache
                CachedData.CommonDates.Clear();
				foreach (EntryPointHistory yedh in CachedData.CachedEntryPointHistoryList)
				{
					//yedh.Instrument = (yedh.Type == "bond" 
					//	? (Instrument)CachedData.CachedBondsDic[yedh.Instrument.Id]
					//	: (Instrument)CachedData.CachedRatesDic[yedh.Instrument.Id]);
#if __VC10__
                    CachedData.CommonDates.Add(CurrentElements.CurrentDate.AddDays(yedh.Duration));
#else
					CachedData.CommonDates[CurrentElements.CurrentDate.AddDays(yedh.Duration)] = true;
#endif
				}
			}

			if (e.Result.xrh != null && e.Result.xrh.EntryPointHistoryList != null)
				CachedData.CachedExchangeRateHistory = e.Result.xrh.EntryPointHistoryList.ToList();

			if (e.Result.yc == null
				|| e.Result.ycf == null
				|| e.Result.yc.YieldCurveDataDic == null
				|| e.Result.ycf.YcFamilyList == null)
				return;

			//
			// TEST
			//
			CachedData.YieldCurveDataDic = e.Result.yc.YieldCurveDataDic.ToDictionary(i => i.Key, i => i.Value);
			CachedData.YcFamilyList = e.Result.ycf.YcFamilyList.ToList();

			InitCcyList();
			//
			//
			//

            // drawing the currency tree here

			/* --Have replaced by Expander control
			 * 
			ObservableCollection<CcyItem> ccyList = new ObservableCollection<CcyItem>();
			
			YcSettingsDic.ycdDic.Clear();

            foreach (Currency c in CachedData.CachedCurrencyDic.Values)
			{
                CcyItem tmp = new CcyItem { name = c.ClassName };

                tmp.SubTree = new List<CcyItem.YcItem>();

                foreach (YieldCurveFamily ycf in e.Result.ycf.YcFamilyList)
                {
                    if (ycf.CurrencyId != c.Id)
                        continue;

                    foreach (YieldCurveData yc in e.Result.yc.YieldCurveDataDic.Values)
                    {
                        if (yc.Family != ycf.Name || yc.CurrencyId != c.Id)
                            continue;

                        tmp.SubTree.Add(new CcyItem.YcItem { name = yc.Name, idYc = (int)yc.Id });
						
						// and to YcSettings cache
                        YcSettingsDic.SetYcSett(yc);
						//
                    }
                }

                if (tmp.SubTree.Count != 0)
                    ccyList.Add(tmp);
            }

            this.treeView1.ItemsSource = ccyList;
			*/


			//currency list is populated and cache is populated
			childRateWindow = new RateGUI();  //cache should be populated before
			childBondWindow = new BondGUI();
            childInflationRateWindow = new InflationRateGUI(); 
		
            FillExchangeGridFromCache(null);

			CachedData.InitializeYieldCurveComboFromCache(this.comboBox1);
			CachedData.InitializeYieldCurveComboFromCache(this.comboBox2);

			// TEST
			this.button4.IsEnabled = true;
			this.tabItem5.IsEnabled = true;
			this.dataFeedControl1.InitData();
        }

        void proxy_InflationInitCompleted(object sender, InflationInitCompletedEventArgs e)
        {
            if (e.Result.Error != null)
            {
                MessageBox.Show(e.Result.Error.Message);
                return;
            }

           
            if (e.Result.ib != null && e.Result.ib.BondDic != null)
                CachedData.CachedInflationBondsDic = e.Result.ib.BondDic.ToDictionary(i => i.Key, i => i.Value);

            if (e.Result.ii != null && e.Result.ii.InflationDic != null)
                CachedData.CachedInflationIndexDic = e.Result.ii.InflationDic.ToDictionary(i => i.Key, i => i.Value);

            if (e.Result.ir != null && e.Result.ir.InflationRateDic != null)
                CachedData.CachedInflationRateDic = e.Result.ir.InflationRateDic.ToDictionary(i => i.Key, i => i.Value);

            if (e.Result.rh != null)
            {
                CachedData.CachedInflationBondsHistoryDic = e.Result.rh.InflationBondsHistoryDic.ToDictionary(i => i.Key, i => i.Value);
                CachedData.CachedInflationIndexHistoryDic = e.Result.rh.InflationIndexHistoryDic.ToDictionary(i => i.Key, i => i.Value);
                CachedData.CachedInflationRateHistoryDic = e.Result.rh.InflationRateHistoryDic.ToDictionary(i => i.Key, i => i.Value); 
            
            }
          

            if (e.Result.icf == null
                || e.Result.ic == null
                || e.Result.iced == null)
                return;

            CachedData.CachedInflationCurveFamily = e.Result.icf.InflationFamilyList.ToList();
            CachedData.CachedInflationCurveDataDic = e.Result.ic.InflationCurveDataDic.ToDictionary(i => i.Key, i => i.Value);
            CachedData.CachedInflationCurveEntryDataDic = e.Result.iced.InflationCurveEntryDataDic.ToDictionary(i => i.Key, i => i.Value);

            if (CachedData.CachedCurrencyDic!=null)
            {
                InitInflationList();
            }
            else
            {
                StartTimerInflationInit();
            }
        }


		void InitCcyList()
		{
			YcSettingsDic.ycdDic.Clear();

			foreach (Currency c in CachedData.CachedCurrencyDic.Values)
			{
				CcyItem tmp = new CcyItem { name = c.ClassName };

				tmp.SubTree = new List<CcyItem.YcItem>();

				foreach (YieldCurveFamily ycf in CachedData.YcFamilyList)
				{
					if (ycf.CurrencyId != c.Id)
						continue;

					foreach (YieldCurveData yc in CachedData.YieldCurveDataDic.Values)
					{
						if (yc.Family != ycf.Name || yc.CurrencyId != c.Id)
							continue;

						tmp.SubTree.Add(new CcyItem.YcItem { name = yc.Name, idYc = (int)yc.Id });

						// and to YcSettings cache
						YcSettingsDic.SetYcSett(yc);
						//
					}
				}

				if (tmp.SubTree.Count != 0)
				{
					//ccyList.Add(tmp);

					// custom header (to include ccy icon)
					string expXaml = string.Format(
						"<toolkitEx:Expander Height=\"Auto\" " +
							"xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
							"xmlns:toolkitEx=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Toolkit\">" + 
							"<toolkitEx:Expander.Header>" + 
								"<StackPanel Orientation=\"Horizontal\">" +
									"<Image Source=\"/YieldCurveSL;component/Images/Ccy/{0}3.png\" Height=\"24\" Width=\"24\" HorizontalAlignment=\"Left\"/>" +
									"<TextBlock Grid.Column=\"1\" Margin=\"4,0,0,0\"  VerticalAlignment=\"Center\" FontSize=\"12\" " +
										"FontFamily=\"Arial\" FontWeight=\"Normal\" Foreground=\"Black\" Text=\"{1}\"/>" +
								"</StackPanel>" +
							"</toolkitEx:Expander.Header>" +
						"</toolkitEx:Expander>",
						tmp.name,
						tmp.name
						);

					Expander e = XamlReader.Load(expXaml) as Expander;
					//Expander e = new Expander();
					if (e == null) { e = new Expander(); e.Header = tmp.name; }
					e.IsExpanded = false;

					//e.Expanded += new RoutedEventHandler(Expander_Expanded);
					//e.Collapsed += new RoutedEventHandler(Expander_Collapsed);
					e.Name = tmp.name;
					//e.ExpandDirection = ExpandDirection.Right;

					//
					//lb.SetBinding(ListBox.ItemsSourceProperty, new Binding("name"));	// prop of CcyItem.YcItem
					string lbXaml =
									"<ListBox xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
											"Name=\"listBox1\" HorizontalAlignment=\"Left\">" +
										"<ListBox.ItemTemplate>" +
											"<DataTemplate>" +
												"<StackPanel Orientation=\"Horizontal\">" +
													"<TextBlock Text=\"{Binding name}\"/>" +
												"</StackPanel>" +
											"</DataTemplate>" +
										"</ListBox.ItemTemplate>" +
									"</ListBox>";

					ListBox lb = XamlReader.Load(lbXaml) as ListBox;
					if (lb == null) lb = new ListBox();
					lb.SelectionChanged += new SelectionChangedEventHandler(lb_SelectionChanged);
					//

					lb.ItemsSource = tmp.SubTree;
                    lb.Background = new SolidColorBrush(Colors.Transparent);
                    lb.BorderBrush = new SolidColorBrush(Colors.Transparent);

					e.Content = lb;
					this.ExpanderStackParent.Children.Add(e);

					if (tmp.name == "USD")
					{
						lb.SelectedItem = lb.Items[0];
						e.IsExpanded = true;
					} 
				}
			}
		}

        void InitInflationList()
        {
       //     YcSettingsDic.icdDic.Clear();

            foreach (Currency c in CachedData.CachedCurrencyDic.Values)
            {
                CcyItem tmp = new CcyItem { name = c.ClassName };

                tmp.SubTree = new List<CcyItem.YcItem>();

                foreach (YieldCurveFamily icf in CachedData.CachedInflationCurveFamily)
                {
                    if (icf.CurrencyId != c.Id)
                        continue;

                    foreach (InflationCurveData ic in CachedData.CachedInflationCurveDataDic.Values)
                    {
                        if (ic.Family != icf.Name || ic.CurrencyId != c.Id)
                            continue;

                        tmp.SubTree.Add(new CcyItem.YcItem { name = ic.Name, idYc = (int)ic.Id });
                        
                        InflationCurveSnapshot ics = new InflationCurveSnapshot();
                        ics.Id = ic.Id; //not correct
                        ics.settlementDate = DateTime.Now;
                        ics.Name = ic.Name;
                        ics.Family = ic.Family;
                        ics.CurrencyId = icf.CurrencyId;
                        ics.InflationIndexId = ic.InflationIndexId;
                        ics.InflationIndex = CachedData.CachedInflationIndexDic[ics.InflationIndexId];
                        ics.settings = CachedData.CachedInflationCurveDataDic[ic.Id].settings;

                        ics.EntryList = new ObservableCollection<InflationCurveEntryData>();
                        ics.ValueList = new ObservableCollection<double>();

                        DateTime d = ics.settlementDate;

                        foreach (InflationCurveEntryData iced in CachedData.CachedInflationCurveEntryDataDic.Values.ToList())
                        {

                            if ((iced.InflationCurveId == ic.Id) &&
                                (iced.ValidDateBegin <= ics.settlementDate) &&
                                (iced.ValidDateEnd >= ics.settlementDate))
                            {;
                                
                                ics.EntryList.Add(iced);
                                
                                //now for that entry we need to find the most recent historical value
                                if (iced.Type == "inflationbond")
                                {
                                    List<RateHistory> bh = CachedData.getBondsHistoryById(iced.Instrument.Id, d);
                                    RateHistory rh = bh[bh.Count() - 1];
                                    if (rh.RateId != iced.Instrument.Id)
                                        MessageBox.Show("something is wrong in GetInflationCurveSnapshot");
                                    else
                                        ics.ValueList.Add(bh[bh.Count() - 1].Close);
                                }
                                else
                                {//only bond is implemented for a moment
                                    List<RateHistory> bh = CachedData.getRateHistoryById(iced.Instrument.Id, d);
                                    RateHistory rh = bh[bh.Count() - 1];
                                    if (rh.RateId != iced.Instrument.Id)
                                        MessageBox.Show("something is wrong in GetInflationCurveSnapshot");
                                    else
                                        ics.ValueList.Add(bh[bh.Count() - 1].Close);
                                }

                            }
                        }

                        List<RateHistory> iihistory = CachedData.getIndexHistoryById(ics.InflationIndexId, d);
                        ics.IndexHistory = new ObservableCollection<HistoricValue>();
                        foreach (RateHistory iirh in iihistory)
                        {
                            HistoricValue vp = new HistoricValue();
                            vp.Date = iirh.Date;
                            vp.Value = iirh.Close;
                            ics.IndexHistory.Add(vp);
                        }

                        // and to YcSettings cache
                 //       YcSettingsDic.SetWrICSnapshot(ics);
                        //
                    }
                }

                if (tmp.SubTree.Count != 0)
                {
                    string expXaml = string.Format(
                        "<toolkitEx:Expander Height=\"Auto\" " +
                            "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
                            "xmlns:toolkitEx=\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Toolkit\">" +
                            "<toolkitEx:Expander.Header>" +
                                "<StackPanel Orientation=\"Horizontal\">" +
                                    "<Image Source=\"/YieldCurveSL;component/Images/Ccy/{0}3.png\" Height=\"24\" Width=\"24\" HorizontalAlignment=\"Left\"/>" +
                                    "<TextBlock Grid.Column=\"1\" Margin=\"4,0,0,0\"  VerticalAlignment=\"Center\" FontSize=\"12\" " +
                                        "FontFamily=\"Arial\" FontWeight=\"Normal\" Foreground=\"Black\" Text=\"{1}\"/>" +
                                "</StackPanel>" +
                            "</toolkitEx:Expander.Header>" +
                        "</toolkitEx:Expander>",
                        tmp.name,
                        tmp.name
                        );

                    Expander e = XamlReader.Load(expXaml) as Expander;
                    if (e == null) { e = new Expander(); e.Header = tmp.name; }
                    e.IsExpanded = false;
                    e.Name = tmp.name;
                   
                    string lbXaml =
                                    "<ListBox xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
                                            "Name=\"listBox1\" HorizontalAlignment=\"Left\">" +
                                        "<ListBox.ItemTemplate>" +
                                            "<DataTemplate>" +
                                                "<StackPanel Orientation=\"Horizontal\">" +
                                                    "<TextBlock Text=\"{Binding name}\"/>" +
                                                "</StackPanel>" +
                                            "</DataTemplate>" +
                                        "</ListBox.ItemTemplate>" +
                                    "</ListBox>";

                    ListBox lb = XamlReader.Load(lbXaml) as ListBox;
                    if (lb == null) lb = new ListBox();
                    //testButton_click
                    lb.SelectionChanged += new SelectionChangedEventHandler(lb_InflationSelectionChanged);
                   

                    lb.ItemsSource = tmp.SubTree;
                    lb.Background = new SolidColorBrush(Colors.Transparent);
                    lb.BorderBrush = new SolidColorBrush(Colors.Transparent);

                    e.Content = lb;
                    this.ExpanderStackInflationParent.Children.Add(e);

                    if (tmp.name == "USD")
                    {
                  //      lb.SelectedItem = lb.Items[0];
                  //      e.IsExpanded = true;
                    }
                }
            }
        }


		void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 0)
				return;

			CcyItem.YcItem tmp = (e.AddedItems[0] as CcyItem.YcItem);
			if (tmp == null)
				return;

			if (CurrentElements.CurrentYCId == tmp.idYc)
				return;

			((LineSeries)this.chart1.Series[0]).ItemsSource = null;
            ((LineSeries)this.chart1.Series[1]).ItemsSource = null;
            ((LineSeries)this.chart1.Series[2]).ItemsSource = null;
            this.dataGridOutput.ItemsSource = null;
            this.dataGridEntryRates.ItemsSource = null;
			
			// close other open expanders / deselect their curve selected
			foreach (var i in this.ExpanderStackParent.Children)
			{
				Expander exp = i as Expander;
				if (exp != null)
				{
					ListBox lb = exp.Content as ListBox;
					if (lb != null
						&& lb.SelectedItem != null
						&& lb.SelectedItem is CcyItem.YcItem
						&& ((lb.SelectedItem as CcyItem.YcItem).idYc != tmp.idYc)
						)
						lb.SelectedIndex = -1;
				}
			}
			//

			DrawEntryPointChartFromCache(tmp.idYc, null);
			//    ((LineSeries)this.chart1.Series[0]).ItemsSource = null;
			((LineSeries)this.chart1.Series[0]).Refresh();

			CurrentElements.CurrentYCId = tmp.idYc;
			int curve_id = tmp.idYc;

			if (
				CachedData.CachedYieldCurvesDic.ContainsKey(curve_id)				//everything is already cached
				&& CachedData.CachedYieldCurvesDic[curve_id].Points.Count != 0
				)
			{
				if (YcSettingsDic.GetYcSett(curve_id).ifZCCurve)
					DrawResultCurveFromCache(curve_id, true, 1, YcSettingsDic.GetYcSett(curve_id).ZCColor, 1);

				if (YcSettingsDic.GetYcSett(curve_id).ifForwardCurve)
					DrawResultCurveFromCache(curve_id, false, 2, YcSettingsDic.GetYcSett(curve_id).FrwColor, 1);

				FillOutputDataGridFromCache(null);
				DrawEntryPointGridFromCache(tmp.idYc, null);
			}

			ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();

			//		foreach (YieldCurveEntryDataHistory y in CachedData.CachedEntryPointHistoryList)
			//		{
			//			if (y.YieldCurveId != curve_id)
			//				continue;
			//
			//			dList.Add(CurrentElements.CurrentDate.AddDays(y.Duration));
			//		}

#if __VC10__
			foreach (DateTime d in CachedData.CommonDates)
#else
			foreach ( DateTime d in CachedData.CommonDates.Keys)
#endif
				dList.Add(d);

			YieldCurveData ycd = YcSettingsDic.GetYcSett(curve_id).ycd;
			_service.CalculateDiscountedRateListAsync(ycd, CurrentElements.CurrentDate, dList, true);   
		}

        void proxy_GetRateDataDicCompleted(object sender, GetRateDataDicCompletedEventArgs e)
        {
            if (e.Result.Error != null)
            {
                MessageBox.Show(e.Result.Error.Message);
                return;
            }

            if (e.Result.RateDic == null)
                return;

			CachedData.CachedRatesDic = e.Result.RateDic.ToDictionary(i => i.Key, i => i.Value);
        }

        void proxy_GetBondDataDicCompleted(object sender, GetBondDataDicCompletedEventArgs e)
        {
            if (e.Result.Error != null)
            {
                MessageBox.Show(e.Result.Error.Message);
                return;
            }

            if (e.Result.BondDic == null)
                return;

			CachedData.CachedBondsDic = e.Result.BondDic.ToDictionary(i => i.Key, i => i.Value);
        }

        void proxy_GetEntryPointHistoryListCompleted(object sender, GetEntryPointHistoryListCompletedEventArgs e)
        {
            if (e.Result.Error != null)
            {
                MessageBox.Show(e.Result.Error.Message);
                return;
            }

            if (e.Result.EntryPointHistoryList == null)
                return;

            CachedData.CachedEntryPointHistoryList.Clear();
            CachedData.CachedEntryPointHistoryList = e.Result.EntryPointHistoryList.ToList(); // bond is taken care of maturity already, rates are there too
        }

        //
        // this will populate the cache for computed yieldcurve
        // if this method was called the yield curve needs to be drawn and output datagrid needs to be filled
        void proxy_CalculateInflationRateListCompleted(object sender, CalculateInflationRateListCompletedEventArgs e)
        {
            if (e.Result.Error != null)
            {
                MessageBox.Show(e.Result.Error.Message);
                return;
            }

     //       if (e.Result.entryPoints.Count() == 0)
       //         return;

            //after callibration was done the information about disabled entry rates is saved to the cache
            /*	foreach (YieldCurveEntryDataHistory yceh in CachedData.CachedEntryPointHistoryList)
                {
                    if (yceh.YieldCurveId != e.Result.YcId)
                        continue;

                    foreach (YieldCurveEntryData yce in e.Result.entryPoints)
                        if (yceh.Id == yce.Id)
                        {
                            yceh.Enabled = yce.Enabled;

                            if (maxDay < yce.Duration)
                                maxDay = yce.Duration;

                        }
                } */

            if (e.Result.discountPoints.Count() == 0)
                return;

            YieldCurveComputed ycc = new YieldCurveComputed()
            {
                YieldCurveID = e.Result.YcId,
                Points = new List<DicountPointGrid>()
            };

            //  populate CachedYieldCurvesList with results
            foreach (DiscountPoint rd in e.Result.discountPoints)
            {
                ycc.Points.Add(new DicountPointGrid()
                {
                    dateToDiscount = rd.discountDate,
                    Length = rd.discountDate.Subtract(CurrentElements.CurrentDate).Days,
                    TimeUnit = "Days",
                    zcRate = rd.zcRate,
                    discount = rd.discount,
                    frwRate = rd.fwdRate
                });
            }

            if (CachedData.CachedInflationCurvesDic.ContainsKey(e.Result.YcId))
                CachedData.CachedInflationCurvesDic[e.Result.YcId].Points.Clear();
            CachedData.CachedInflationCurvesDic[e.Result.YcId] = ycc;

              if (e.Result.ifToDraw)
              {
                   FillInflationInputDataGridFromCache();

            //     if (YcSettingsDic.GetYcSett(e.Result.YcId).ifZCCurve)
            //         DrawResultCurveFromCache(e.Result.YcId, true, 1, ColorFromString.ToColor(YcSettingsDic.GetYcSett(e.Result.YcId).ycd.settings.ZCColor), 1);

             //      if (YcSettingsDic.GetYcSett(e.Result.YcId).ifForwardCurve)
             //          DrawResultCurveFromCache(e.Result.YcId, false, 2, ColorFromString.ToColor(YcSettingsDic.GetYcSett(e.Result.YcId).ycd.settings.FrwColor), 1);

                  // FillOutputDataGridFromCache(null);
                   FillInflationOutputDataGridFromCache(null);

            //       this.rangeSlider.Maximum = maxDay;
               }

            // this.button5.IsEnabled = true;
        }


        void proxy_CalculateDiscountedRateListCompleted(object sender, CalculateDiscountedRateListCompletedEventArgs e)
        {
            if (e.Result.Error != null)
            {
                MessageBox.Show(e.Result.Error.Message);
                return;
            }
			/*
			if (e.Result.entryPoints.Count() == 0)
				return;

            long maxDay = 0; //maximum date form entry table - used for chart

            //after callibration was done the information about disabled entry rates is saved to the cache
			foreach (EntryPointHistory yceh in CachedData.CachedEntryPointHistoryList)
			{
				if (yceh.YieldCurveId != e.Result.YcId)
					continue;

				foreach (EntryPoint yce in e.Result.entryPoints)
                    if (yceh.Id == yce.Id)
                    {
                        yceh.Enabled = yce.Enabled;

                        if (maxDay < yce.Duration)
                            maxDay = yce.Duration;

                    }
			}
			*/
			if (e.Result.discountPoints.Count() == 0)
				return;

			YieldCurveComputed ycc = new YieldCurveComputed()
			{
				YieldCurveID = e.Result.YcId,
				Points = new List<DicountPointGrid>()
			};

			long maxDay = 0; //maximum date form entry table - used for chart

			//  populate CachedYieldCurvesList with results
			foreach (DiscountPoint rd in e.Result.discountPoints)
			{
				int len = rd.discountDate.Subtract(CurrentElements.CurrentDate).Days;

				ycc.Points.Add(new DicountPointGrid()
				{
					dateToDiscount = rd.discountDate,
					Length = len,
					TimeUnit = "Days",
					zcRate = rd.zcRate,
					discount = rd.discount,
					frwRate = rd.fwdRate
				});

				if (maxDay < len)
					maxDay = len;
			}

            //if( CachedData.CachedYieldCurvesDic.ContainsKey(e.Result.YcId))
            //    CachedData.CachedYieldCurvesDic[e.Result.YcId].Points.Clear();
			
			CachedData.CachedYieldCurvesDic[e.Result.YcId] = ycc;

            if (e.Result.ifToDraw)
            {
                DrawEntryPointGridFromCache(e.Result.YcId, null);

                if (YcSettingsDic.GetYcSett(e.Result.YcId).ifZCCurve)
                    DrawResultCurveFromCache(e.Result.YcId, true, 1, ColorFromString.ToColor(YcSettingsDic.GetYcSett(e.Result.YcId).ycd.settings.ZCColor), 1);

                if (YcSettingsDic.GetYcSett(e.Result.YcId).ifForwardCurve)
                    DrawResultCurveFromCache(e.Result.YcId, false, 2, ColorFromString.ToColor(YcSettingsDic.GetYcSett(e.Result.YcId).ycd.settings.FrwColor), 1);

				FillOutputDataGridFromCache(e.Result.YcId);
                this.rangeSlider.Maximum = maxDay;
            }

			this.button5.IsEnabled = true;
        }

        void proxy_GetMaturityDatesListCompleted(object sender, GetMaturityDatesListCompletedEventArgs e)
        { 
            if (e.Result.Error != null)
            {
                MessageBox.Show(e.Result.Error.Message);
                return;
            }

            if (e.Result.dates.Count() == 0)
				return;

            CurrentElements.InflationDates.Clear();

            for (int i = 0; i < e.Result.dates.Count(); i++)
            {
                CurrentElements.InflationDates.Add(e.Result.dates[i]);
            }
        }

           
        /// <summary>
        /// Chart: Red Line 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        // clicking refresh button - in fact it is recompute button forcing the recomputation and disregaring the cache
        // normally it means that some entry parameters are changes including settings for zc yc or even entry rates (in the future)
        //hence both entry curve and zc and frw curve have to be redrawn
        // for a moment we will just make a recomputation though considerng that entry rates were not changed

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
			if (AppSettings.ActiveWindow == 1) //two curve mode
            {
                int curve_id = CurrentElements.CurrentYCId;

                DrawEntryPointChartFromCache(curve_id, null);
                DrawEntryPointGridFromCache(curve_id, null);

                ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();
#if __VC10__
                foreach (DateTime d in CachedData.CommonDates)
#else
				foreach (DateTime d in CachedData.CommonDates.Keys)
#endif
                    dList.Add(d);

                _service.CalculateDiscountedRateListAsync(YcSettingsDic.GetYcSett(curve_id).ycd, CurrentElements.CurrentDate, dList,true);
            }
            else  //the assumption is that refresi is clicked only after curves were computed and settings for color was changed
            {
                if ((((String)comboBox1.SelectedItem) == "") || (((String)comboBox2.SelectedItem) == ""))
				{
					MessageBox.Show("Two curves must be selected");
					return;
				}

				int CurveId1 = CachedData.GetYieldCurveIDbyName((String)comboBox1.SelectedItem);
				int CurveId2 = CachedData.GetYieldCurveIDbyName((String)comboBox2.SelectedItem);

				// to chart4a draw two line series, zC curves
			    DrawResultCurveFromCache(CurveId1, true, 1, AppSettings.FirstCurveColor /*this.colorPicker1.Color*/, 2);
				DrawResultCurveFromCache(CurveId2, true, 2, AppSettings.SecondCurveColor /*this.colorPicker2.Color*/, 2);
				
				// to chart3a draw one line series, zC curve1 - zC curve2 (the diff of two above) 
				DrawCompareDiffCurves(CurveId1, CurveId2);
			}
        }

        // the hyperlink from the upper datagrid containing the list of entry rates
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton btn = (HyperlinkButton)sender;
            string RateRef = btn.Content.ToString();

            EntryPointGrid edg = (EntryPointGrid)(dataGridEntryRates.SelectedItems[0]);
            
			if(edg.type != ratetype.bond.ToString())
            {
                childRateWindow.comboTyp.SelectedItem = edg.type;

                childRateWindow.Clear_RateGUI();

                if (childRateWindow.comboTyp.SelectedItem.ToString() == ratetype.deposit.ToString())
					childRateWindow.makeDepositGUI();
                else
                    childRateWindow.makeSwapGUI();

                // ------------------ from cache ---------------
                Rate r = new Rate();

                foreach (Rate r0 in CachedData.CachedRatesDic.Values)
                {
					if (r0.Name == RateRef)
					{
						r = r0;
						break;
					}
                }

                childRateWindow.comboBas.SelectedItem = CachedData.CachedDayCounterDic[r.BasisId].Name;
                childRateWindow.comboMod.SelectedItem = r.Compounding;
                childRateWindow.comboTer.SelectedItem = r.TimeUnit;
                childRateWindow.txtTer.Text = r.Duration.ToString();
                childRateWindow.txtBoxRef.Text = RateRef;

                //??? it seems there is no Find method for the list in Silverlight
                // so I need to use the loopfor that
                //int id = r.IdCcy;
            
				String CurCode = (CachedData.CachedCurrencyDic.ContainsKey(r.IdCcy) 
									? CachedData.CachedCurrencyDic[r.IdCcy].Code
									: "");
				/*
                foreach (Currency c1 in CachedData.CachedCurrencyList)
                {
					if (c1.Id == r.IdCcy)
					{
						CurCode = c1.Code;
						break;
					}
                }
				*/

                childRateWindow.comboCur.SelectedItem = CurCode;
                childRateWindow.comboCur.SelectedItem = r.IdCcy;
                childRateWindow.txtBoxFda.Text = r.SettlementDays.ToString();
                childRateWindow.comboBus.SelectedItem = r.BusinessDayConvention.ToString();

                //-----------  swap rate part-----------
                if (childRateWindow.comboTyp.SelectedItem.ToString() == ratetype.swap.ToString())
                {
                    childRateWindow.txtBoxIRef.Text = r.IndexName.ToString();

                    Rate r1 = new Rate();

                    foreach (Rate r0 in CachedData.CachedRatesDic.Values)
                    {
						if (r0.Name == r.IndexName)
						{
							r1 = r0;
							break;
						}
                    }

                    childRateWindow.comboFre.SelectedItem = EnumConversion.getStringFromEnum(frequency.Annual);

					childRateWindow.comboIBas.SelectedItem = CachedData.CachedDayCounterDic[r1.BasisId].Name;
                    int l = r1.Duration;  
                    termbase t = (termbase)Enum.Parse(typeof(termbase), r1.TimeUnit, true);
                    childRateWindow.comboIFre.SelectedItem = EnumConversion.getStringFromEnum(EnumConversion.GetFrequencyFromMaturity(l, t));
                    childRateWindow.comboIMod.SelectedItem = r1.Compounding;
                }

                childRateWindow.Show();
            }
            else //bond
            {
                // ------------------ from cache ---------------
                Bond b = new Bond();
                
                foreach (Bond b1 in CachedData.CachedBondsDic.Values)
                {
					if (b1.Name == RateRef)
					{
						b = b1;
						break;
					}
                }
                				
				//int id = b.IdCcy;
                String CurCode = (CachedData.CachedCurrencyDic.ContainsKey(b.IdCcy) 
								? CachedData.CachedCurrencyDic[b.IdCcy].Code
								: "");
				/*
                foreach (Currency c1 in CachedData.CachedCurrencyList)
                {
					if (c1.Id == id)
					{
						CurCode = c1.Code;
						break;
					}
                }
				*/
				
                childBondWindow.comboCur.SelectedItem = CurCode;
                childBondWindow.txtBoxRef.Text = b.Name;
                childBondWindow.comboFre.SelectedItem = EnumConversion.getStringFromEnum(EnumConversion.getFrequencyFromString(b.CouponFrequency));
                childBondWindow.comboTyp.SelectedItem = EnumConversion.getStringFromEnum(bondtype.Fixed);

                childBondWindow.txtBoxNom.Text = b.Nominal.ToString();
                childBondWindow.txtBoxRed.Text = b.Redemption.ToString();
                childBondWindow.txtBoxCoupon.Text = b.Coupon.ToString();
				childBondWindow.datePickerIssue.SelectedDate = b.IssueDate;
                childBondWindow.datePickerMaturity.SelectedDate = b.MaturityDate;
               
                childBondWindow.Show();
            }
        }

        //button_EntryRateHyperlink_Click
        private void button_EntryRateHyperlink_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton btn = (HyperlinkButton)sender;
            string RateRef = btn.Content.ToString();

            EntryPointGrid edg = (EntryPointGrid)(dataGridInflationEntryRates.SelectedItems[0]);

            if (edg.type != ratetype.bond.ToString())
            {
                childInflationRateWindow.comboTyp.SelectedItem = edg.type;
            }
               //childRateWindow.Clear_RateGUI();

              /*  if (childRateWindow.comboTyp.SelectedItem.ToString() == ratetype.deposit.ToString())
					childRateWindow.makeDepositGUI();
                else
                    childRateWindow.makeSwapGUI(); */

                // ------------------ from cache ---------------
                InflationRate ir = new InflationRate();

                foreach (InflationRate r0 in CachedData.CachedInflationRateDic.Values)
                {
                    if (r0.Name == RateRef)
                    {
                        ir = r0;
                        break;
                    }
                }

             //   Rate r = new Rate();

               
                childInflationRateWindow.comboBas.SelectedItem = CachedData.CachedDayCounterDic[ir.BasisId].Name;
                childInflationRateWindow.comboMod.SelectedItem = ir.Compounding;
                childInflationRateWindow.comboTer.SelectedItem = ir.TimeUnit;
                childInflationRateWindow.txtTer.Text = ir.Duration.ToString();
                childInflationRateWindow.txtBoxRef.Text = RateRef;

              	String CurCode = (CachedData.CachedCurrencyDic.ContainsKey(ir.IdCcy) 
									? CachedData.CachedCurrencyDic[ir.IdCcy].Code
									: "");
                childInflationRateWindow.comboCur.SelectedItem = CurCode; // ir.IdCcy;
                childInflationRateWindow.txtBoxFda.Text = ir.SettlementDays.ToString();
                childInflationRateWindow.comboBus.SelectedItem = BusinessDayConvention.Following.ToString(); //ir..BusinessDayConvention.ToString();

                //-----------  swap rate part-----------
                if (childInflationRateWindow.comboTyp.SelectedItem.ToString() == ratetype.swap.ToString())
                {
                     childInflationRateWindow.txtBoxIRef.Text =  CachedData.CachedInflationIndexDic[ir.InflationIndexId].Name.ToString();
                     childInflationRateWindow.comboFre.SelectedItem = EnumConversion.getStringFromEnum(EnumConversion.getFrequencyFromString(ir.Frequency));
                     childInflationRateWindow.comboIFre.SelectedItem = EnumConversion.getStringFromEnum(EnumConversion.getFrequencyFromString(ir.Frequency));
                
                  //   childInflationRateWindow.comboFre.SelectedItem = EnumConversion.getStringFromEnum(frequency.Monthly);
                  //   childInflationRateWindow.comboIFre.SelectedItem = EnumConversion.getStringFromEnum(frequency.Monthly);
                     childInflationRateWindow.Show();
                }
                else //bond
                {
                    #region commented
                    /*
                    Bond b = new Bond();
                
                    foreach (Bond b1 in CachedData.CachedBondsDic.Values)
                    {
					    if (b1.Name == RateRef)
					    {
						    b = b1;
						    break;
					    }
                    }
                				
				    //int id = b.IdCcy;
                    String CurCode1 = (CachedData.CachedCurrencyDic.ContainsKey(b.IdCcy) 
								    ? CachedData.CachedCurrencyDic[b.IdCcy].Code
								    : "");
			
                    childBondWindow.comboCur.SelectedItem = CurCode1;
                    childBondWindow.txtBoxRef.Text = b.Name;
                    childBondWindow.comboFre.SelectedItem = EnumConversion.getStringFromEnum(EnumConversion.getFrequencyFromString(b.CouponFrequency));
                    childBondWindow.comboTyp.SelectedItem = EnumConversion.getStringFromEnum(bondtype.Fixed);

                    childBondWindow.txtBoxNom.Text = b.Nominal.ToString();
                    childBondWindow.txtBoxRed.Text = b.Redemption.ToString();
                    childBondWindow.txtBoxCoupon.Text = b.Coupon.ToString();
				    childBondWindow.datePickerIssue.SelectedDate = b.IssueDate;
                    childBondWindow.datePickerMaturity.SelectedDate = b.MaturityDate;
               
                    childBondWindow.Show(); */

                    #endregion
                }  
        }

		//  the button to display settings
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentElements.CurrentYCId != -1)
            {
				//     discountSettings.SetControlsFromSettingsForYC(CurrentElements.CurrentYCId);
				//     discountSettings.Show();
                newSettings.SetControlsFromSettingsForYC(CurrentElements.CurrentYCId);
				newSettings.Show();   
            }
        }

		// Save ... calculated discount data into excel file
        private void button5_Click(object sender, RoutedEventArgs e)
        {
			// open file dialog to select an export file.   
			SaveFileDialog sDialog = new SaveFileDialog();
			sDialog.Filter = "Excel Files(*.xls)|*.xls";

			if (sDialog.ShowDialog() == true)
			{

				// create an instance of excel workbook
				Workbook workbook = new Workbook();
				// create a worksheet object
				Worksheet worksheet = new Worksheet(String.Format("{0}", CurrentElements.CurrentDate.ToString()));

				int ColumnCount = 0;
				int RowCount = 0;

				// Writing Column Names 
				foreach (DataGridColumn dgcol in this.dataGridOutput.Columns)
				{
					worksheet.Cells[0, ColumnCount] = new Cell(dgcol.Header.ToString());
					ColumnCount++;
				}

				// Extracting values from grid and writing to excell sheet

				foreach (object data in this.dataGridOutput.ItemsSource)
				{
					ColumnCount = 0;
					RowCount++;
					foreach (DataGridColumn col in this.dataGridOutput.Columns)
					{
						string strValue = "";
						Binding objBinding = null;

						if (col is DataGridBoundColumn)
							objBinding = (col as DataGridBoundColumn).Binding;

						if (col is DataGridTemplateColumn)
						{
							//This is a template column... let us see the underlying dependency object
							DependencyObject objDO = (col as DataGridTemplateColumn).CellTemplate.LoadContent();
							FrameworkElement oFE = (FrameworkElement)objDO;
							FieldInfo oFI = oFE.GetType().GetField("TextProperty");
							if (oFI != null)
							{
								if (oFI.GetValue(null) != null)
								{
									if (oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)) != null)
										objBinding = oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)).ParentBinding;
								}
							}
						}

						if (objBinding != null)
						{
							if (objBinding.Path.Path != "")
							{
								PropertyInfo pi = data.GetType().GetProperty(objBinding.Path.Path);
								if (pi != null) strValue = Convert.ToString(pi.GetValue(data, null));
							}
							/*
							if (objBinding.Converter != null)
							{
								if (strValue != "")
									strValue = objBinding.Converter.Convert(
										strValue, 
										typeof(string), objBinding.ConverterParameter, 
										objBinding.ConverterCulture).ToString();
								//else
								//    strValue = objBinding.Converter.Convert(data, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
							}
							*/
						}
						// writing extracted value in excell cell
						worksheet.Cells[RowCount, ColumnCount] = new Cell(strValue);
						ColumnCount++;
					}
				}

				//add worksheet to workbook
				workbook.Worksheets.Add(worksheet);
				// get the selected file's stream
				Stream sFile = sDialog.OpenFile();
				workbook.Save(sFile);
			}
        }

        private void datePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
			if (CurrentElements.CurrentYCId == -1)
				return;

            int curve_id = CurrentElements.CurrentYCId;
            CurrentElements.CurrentDate = this.datePicker1.SelectedDate.Value;

            DrawEntryPointChartFromCache(CurrentElements.CurrentYCId, CurrentElements.CurrentDate);
            DrawEntryPointGridFromCache(CurrentElements.CurrentYCId, CurrentElements.CurrentDate);

            ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();
            /*
            foreach (YieldCurveEntryDataHistory y in CachedData.CachedEntryPointHistoryList)
            {
				if (y.YieldCurveId != curve_id)
					continue;

                dList.Add(CurrentElements.CurrentDate.AddDays(y.Duration));
            }
			*/
#if __VC10__
			foreach (DateTime d in CachedData.CommonDates)
#else
			foreach (DateTime d in CachedData.CommonDates.Keys)
#endif
				dList.Add(d);

            _service.CalculateDiscountedRateListAsync(YcSettingsDic.GetYcSett(curve_id).ycd, CurrentElements.CurrentDate, dList, true);
        }
        
        public bool DrawEntryPointChartFromCache(int? curveId, DateTime? dt)
        {
			DateTime sd = (dt == null ? CurrentElements.CurrentDate : dt.Value);
			int curve_id = (curveId == null ? CurrentElements.CurrentYCId : curveId.Value);

			ObservableCollection<EntryPointChart> EntryCurveDataPoints = new ObservableCollection<EntryPointChart>(); //in%
            EntryCurveDataPoints.Clear();

			long maxDay = 0;
			foreach (EntryPointHistory y in CachedData.CachedEntryPointHistoryList)
			{
                if ((y.ValidDateBegin > sd) || (y.ValidDateEnd < sd))
                    continue;

				if (y.YieldCurveId != curve_id)
					continue;

				double? val = null;	// here EntryDataHistory assumed be sorted by Date descending (latest at the begining)
				for (int i = 0; i < y.epValueHistory.Count; i++)
				{
					if (y.epValueHistory[i].Date <= sd)
					{
						val = y.epValueHistory[i].Value;
						break;
					}
				}

				EntryCurveDataPoints.Add(
					new EntryPointChart
					{
						type = y.Type,
						rate = (val == null ? 0 : (y.Type == "bond" ? (double)val : (double)val * 100)),
						length = y.Duration,
						maturity = (y.Type == "bond"
									? (y.Instrument as Bond).MaturityDate.ToShortDateString()
									: y.Length.ToString() + y.TimeUnit),
						termStr = String.Format("{0} {1}", y.Length, y.TimeUnit)
					});

				maxDay = Math.Max(maxDay, y.Duration + 2);
			}

            ((LineSeries)this.chart1.Series[0]).ItemsSource = null;
            ((LineSeries)this.chart1.Series[0]).ItemsSource = EntryCurveDataPoints;
            if(entryLineDic.ContainsKey(curve_id))
                entryLineDic[curve_id].Clear();
			entryLineDic[curve_id] = EntryCurveDataPoints;

            this.rangeSlider.Minimum = this.rangeSlider.RangeStart = 0;
            this.rangeSlider.Maximum = maxDay;
            this.rangeSlider.RangeEnd = maxDay;
            ((LineSeries)this.chart1.Series[0]).Refresh();
            
            return true;
        }

        private bool DrawEntryPointGridFromCache(int? curveId, DateTime? dt)
        {
			DateTime sd = (dt == null ? CurrentElements.CurrentDate : dt.Value);
			int curve_id = (curveId == null ? CurrentElements.CurrentYCId : curveId.Value);

			ObservableCollection<EntryPointGrid> EntrySource = new ObservableCollection<EntryPointGrid>();

			foreach (EntryPointHistory y in CachedData.CachedEntryPointHistoryList)
			{
                if ((y.ValidDateBegin > sd) || (y.ValidDateEnd < sd))
                    continue;

				if (y.YieldCurveId != curve_id)
					continue;

				double? val = null;	// here EntryDataHistory assumed be sorted by Date descending (latest at the begining)
				for (int i = 0; i < y.epValueHistory.Count; i++)
				{
					if (y.epValueHistory[i].Date <= sd)
					{
						val = y.epValueHistory[i].Value;
						break;
					}
				}

				EntrySource.Add(new EntryPointGrid()
				{
					enabled = y.Enabled,
					type = y.Type,
					value = (val == null ? 0 : (y.Type == "bond" ? (double)val : (double)val * 100)),
					reference = y.Instrument.Name,
					maturity = (y.Type == "bond"
								? (y.Instrument as Bond).MaturityDate.ToShortDateString()
								: y.Length.ToString() + y.TimeUnit)
				});
			}

            this.dataGridEntryRates.ItemsSource = EntrySource;
			/*
            // TODO: this part of the code doesnt work
            // it changes the font of the entry line if it is disabled
            foreach (EntryPointGrid edg in EntrySource)
            {
				if (edg.enabled)
					continue;

                // FrameworkElement el = this.dataGridEntryRates.Columns[1].GetCellContent(j);

                this.dataGridEntryRates.SelectedIndex = 5;
                FrameworkElement el = this.dataGridEntryRates.Columns.Last().GetCellContent(this.dataGridEntryRates.SelectedItem);
                
				if (el == null) return true;

                DataGridRow row = DataGridRow.GetRowContainingElement(el.Parent as FrameworkElement);
                if (row == null) return true;

                row.FontSize = 5; 

                    //     DataGridCell changeCell =(DataGridCell) this.dataGridEntryRates.Columns[1].GetCellContent(this.dataGridEntryRates.ItemsSource.Cast<object>().ElementAt(1));
                    //   DataGridCell changeCell = (DataGridCell)el.Parent;
                    //   changeCell.FontSize = 4;
                    // SolidColorBrush brush = new SolidColorBrush(Colors.Black);
            }
            */
            return true;
        }

        private SolidColorBrush highlightBrush = new SolidColorBrush(Colors.LightGray);
		private SolidColorBrush normalBrush = new SolidColorBrush(Colors.White);
		
		private void dataGridEntryRates_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			// Check the data object for this row.
			EntryPointGrid edg = (EntryPointGrid)e.Row.DataContext;
			if (edg.enabled == false)
			{
				e.Row.FontStyle = FontStyles.Italic; //.Background = highlightBrush;
           	}
			else
			{
				e.Row.FontStyle = FontStyles.Normal;
			}
		}
#if FALSE
        //before calling this function user should make sure that the YC is computed and cached
        //ifZC and ifFrw overwrite preferences - so user should check global prefs
        // the reason is that it can be used for curves comparison which will be independent of preferences
        private bool DrawZCandFrwCurveFromCache(int? curveId, bool? ifZC, bool? ifFrw, bool? ifZCLine)
        {
            int curve_id = (curveId == null ? CurrentElements.CurrentYCId : curveId.Value);
            
            //??? why such global collections exist at all?
            // clear Red Line if any
            /*
			if (redLine != null)
                redLine.Clear();

            if (frwLine != null)
                frwLine.Clear();
			*/

			if (ifZCLine == null) //normal case - we will draw both ZC and frw curves
			{
				if (ZCLine != null)
					ZCLine.Clear();

				if (frwLine != null)
					frwLine.Clear();
			}
			else if (ifZCLine == true)
			{
				if (ZCLine != null)
					ZCLine.Clear();
			}
			else if (ifZCLine == false)
			{
				if (frwLine != null)
					frwLine.Clear();
			}

			YieldCurveComputed yc = null;
			try
			{
				yc = CachedData.CachedYieldCurvesDic[curve_id];
			}
			catch (KeyNotFoundException)
			{
				MessageBox.Show("not yet computed");
				return false;
			}

			List<EntryPointChart> ZC_curve = new List<EntryPointChart>();
			List<EntryPointChart> Forward_curve = new List<EntryPointChart>();

            long maxDay = 0;

            // ifZC overwrites prefs
            if ((ifZC == null) || (ifZC == true))
            {
                foreach (DicountPointGrid d in yc.Points)
                {
                    TimeSpan diff = d.dateToDiscount - CurrentElements.CurrentDate;
                    ZC_curve.Add(
						new EntryPointChart 
						{ 
							rate = d.zcRate * 100, 
							length = diff.Days, 
							termStr = String.Format("{0} {1}", 
							d.Length, 
							d.TimeUnit) 
						});

                    if (diff.Days > maxDay)
	                    maxDay = diff.Days;
                }
				/*
                ((LineSeries)this.chart1.Series[1]).ItemsSource = ZC_curve;
                redLine = ZC_curve;
				*/
				if ((ifZCLine == null) || (ifZCLine == true))
				{
					((LineSeries)this.chart1.Series[1]).ItemsSource = ZC_curve;
					Color color = YcSettingsDic.GetYcSett(curve_id).ZCColor;
					((LineSeries)this.chart1.Series[1]).Background = new SolidColorBrush(color);
					ZCLine = ZC_curve;
				}
				else // this should indicate that we will use frwLine for zc curve (comparing zc yc)
				{
					((LineSeries)this.chart1.Series[2]).ItemsSource = ZC_curve;
					Color color = YcSettingsDic.GetYcSett(curve_id).ZCColor;
					((LineSeries)this.chart1.Series[2]).Background = new SolidColorBrush(color);
					frwLine = ZC_curve;
				}
            }

            if ((ifFrw == null) || (ifFrw == true))
            {
                foreach (DicountPointGrid d in yc.Points)
                {
                    TimeSpan diff = d.dateToDiscount - CurrentElements.CurrentDate;
                    Forward_curve.Add(
						new EntryPointChart 
						{ 
							rate = d.frwRate * 100, 
							length = diff.Days, 
							termStr = String.Format("{0} {1}", 
							d.Length, d.TimeUnit) 
						}
					);

                    if (diff.Days > maxDay)
                        maxDay = diff.Days;
                }

                ((LineSeries)this.chart1.Series[2]).ItemsSource = Forward_curve;
				
				Color color = YcSettingsDic.GetYcSett(curve_id).FrwColor;
				((LineSeries)this.chart1.Series[2]).Background = new SolidColorBrush(color);
                
                frwLine = Forward_curve;
            }

            // reset slider
            this.rangeSlider.Minimum = this.rangeSlider.RangeStart = 0;
            this.rangeSlider.Maximum = maxDay;
            this.rangeSlider.RangeEnd = maxDay;

            return true;
		}
#endif // if FALSE

		// ifZC - if zc should be drawn, otherwise it will be forward 
		public bool DrawResultCurveFromCache(int? curveId /*= null*/, bool? ifZC /*= true*/, int? series /*= 1*/, Color? color /*= null*/, int? chartId /*= 1*/)
        {
            int curve_id = (curveId == null ? CurrentElements.CurrentYCId : curveId.Value);
            if (chartId == 1)
                ((LineSeries)this.chart1.Series[(int)series]).ItemsSource = null;
            else
                ((LineSeries)this.chart4a.Series[(int)series]).ItemsSource = null;
           
			if (color == null) 
                color = Colors.Black;

            YieldCurveComputed yc = new YieldCurveComputed();
            yc=null;

			try
			{
				yc = CachedData.CachedYieldCurvesDic[curve_id];
			}
			catch (KeyNotFoundException)
			{
				MessageBox.Show("not yet computed");
				return false;
			}

			ObservableCollection<EntryPointChart> CurvePoints = new ObservableCollection<EntryPointChart>();
           
            long maxDay = 0;

            foreach (DicountPointGrid d in yc.Points)
            {
                TimeSpan diff = d.dateToDiscount - CurrentElements.CurrentDate;
                double r = (ifZC == true) ? d.zcRate : d.frwRate;
                EntryPointChart dp = new EntryPointChart 
                {   rate = r * 100, 
                    length = diff.Days, 
                    termStr = String.Format("{0} {1}", d.Length, d.TimeUnit) 
                };
                 
                CurvePoints.Add(dp);

                if (diff.Days > maxDay)
                    maxDay = diff.Days;
            }
			/*
            ((LineSeries)this.chart1.Series[(int)series]).ItemsSource = CurvePoints;
            ((LineSeries)this.chart1.Series[(int)series]).Background = new SolidColorBrush((Color)color);
            */
			if (chartId == 1)
			{
                ((LineSeries)this.chart1.Series[(int)series]).ItemsSource = CurvePoints;
				((LineSeries)this.chart1.Series[(int)series]).Background = new SolidColorBrush((Color)color);
            }
			else
			{
               	((LineSeries)this.chart4a.Series[(int)series]).ItemsSource = CurvePoints;
				((LineSeries)this.chart4a.Series[(int)series]).Background = new SolidColorBrush((Color)color);
            }

			if (ifZC == true)	// series[1]
			{
                if(zCLineDic.ContainsKey(curve_id))
                    zCLineDic[curve_id].Clear();
				zCLineDic[curve_id] = CurvePoints;
			}
			else				// series[2]
			{
                if (frwLineDic.ContainsKey(curve_id))
                    frwLineDic[curve_id].Clear();
				frwLineDic[curve_id] = CurvePoints;
			}

            if (chartId == 1)
            {
                this.rangeSlider.Minimum = this.rangeSlider.RangeStart = 0;
                this.rangeSlider.Maximum = maxDay;
                this.rangeSlider.RangeEnd = maxDay;
            }
 
            return true;
        }

        private bool FillOutputDataGridFromCache(int? curveId)
        {
			int curve_id = (curveId == null ? CurrentElements.CurrentYCId : (int)curveId);
            
			YieldCurveComputed yc = null;
			try
			{
				yc = CachedData.CachedYieldCurvesDic[curve_id];
			}
			catch (KeyNotFoundException)
			{
				MessageBox.Show("not yet computed");
				return false;
			}
		
			this.dataGridOutput.ItemsSource = yc.Points;
			/*
			ObservableCollection<DicountPointGrid> dataGridOutput = new ObservableCollection<DicountPointGrid>();
			foreach (EntryPointHistory y in CachedData.CachedEntryPointHistoryList)
			{
				if (y.YieldCurveId != curve_id)
					continue;

				foreach (DicountPointGrid ycdg in yc.Points)
					if (ycdg.dateToDiscount == CurrentElements.CurrentDate.AddDays(y.Duration))
						dataGridOutput.Add(ycdg);
			}

			this.dataGridOutput.ItemsSource = dataGridOutput;
			*/
            this.dataGridOutput.InvalidateMeasure();
            this.dataGridOutput.UpdateLayout();
          
            return true;
        }

        //
        // DataGrid: right table
        //
        private void dataGrid_NormalizedByBlueLine()
        {
			ObservableCollection<DicountPointGrid> source = new ObservableCollection<DicountPointGrid>();

            int itemsCount = 10;

            for (int i = 1; i <= itemsCount; i++)
            {
                if (this.rangeSlider.Maximum < i * 2)
                    break;

                source.Add(
					new DicountPointGrid()
					{
						dateToDiscount = CurrentElements.CurrentDate.AddDays(i * 2),
						Length = i * 2,
						TimeUnit = "Days"
					}
				);
            }

            for (int i = 1; i <= itemsCount; i++)
            {
                if (this.rangeSlider.Maximum < i * 2 * 30)
                    break;

                source.Add(
					new DicountPointGrid()
					{
						dateToDiscount = CurrentElements.CurrentDate.AddMonths(i * 2),
						Length = i * 2,
						TimeUnit = "Months"
					}
				);
            }

            //for (int i = 1; i <= itemsCount; i++)
            for (int i = 1; this.rangeSlider.Maximum >= i * 2 * 365; i++)
            {
                //if (this.rangeSlider.Maximum < i * 2 * 365)
                //	break;

                source.Add(
					new DicountPointGrid()
					{
						dateToDiscount = CurrentElements.CurrentDate.AddYears(i * 2),
						Length = i * 2,
						TimeUnit = "Years"
					}
				);
            }

            this.dataGridOutput.ItemsSource = source;
        }
        
        //
        // Event handler for RangeSlider
        //
        private EventHandler _EventRangeSlider;
        public event EventHandler EventRangeSlider
        {
			add
			{ 
				// only subscribe when we have a subscriber ourselves
				bool first = _EventRangeSlider == null;
				_EventRangeSlider += value;

				if (first && _EventRangeSlider != null)
				{
					this.rangeSlider.RangeChanged += OnEventRangeSlider;
					this.rangeSlider3.RangeChanged += OnEventRangeSlider;
				}
			}
			remove
			{ 
				// unsubscribe if we have no more subscribers
				_EventRangeSlider -= value;

				if (_EventRangeSlider == null)
				{
					this.rangeSlider.RangeChanged -= OnEventRangeSlider;
					this.rangeSlider3.RangeChanged -= OnEventRangeSlider;
				}
			}
        }

		public class Curve
		{
			public LineSeries ls;
			public RangeSlider slider;
			public ObservableCollection<EntryPointChart> allPoints;
		}
        protected void OnEventRangeSlider(object sender, EventArgs args)
        {
			RangeSlider slider = sender as RangeSlider;
			if (slider == null)
				return;

			List<Curve> tmp = new List<Curve>();

			try
			{
                // which slider ?
                if (slider.Name == "rangeSlider")       // YC/ZC/FW curve
                {
                    int curveId = CurrentElements.CurrentYCId;
                    if (curveId == -1)
                        return;

                    tmp.Add(new Curve { allPoints = entryLineDic[curveId], ls = (LineSeries)(chart1.Series[0]), slider = rangeSlider });
                    tmp.Add(new Curve { allPoints = zCLineDic[curveId], ls = (LineSeries)(chart1.Series[1]), slider = rangeSlider });
                    tmp.Add(new Curve { allPoints = frwLineDic[curveId], ls = (LineSeries)(chart1.Series[2]), slider = rangeSlider });
                }
                else if (slider.Name == "rangeSlider3") // Compare curve
                {
                    int curveId1 = CachedData.GetYieldCurveIDbyName((String)comboBox1.SelectedItem);
                    int curveId2 = CachedData.GetYieldCurveIDbyName((String)comboBox2.SelectedItem);
                    if (curveId1 == -1 || curveId2 == -1)
                        return;

                    // zC curve1 and zC curve2
                    tmp.Add(new Curve { allPoints = zCLineDic[curveId1], ls = (LineSeries)(chart4a.Series[1]), slider = rangeSlider3 });
                    tmp.Add(new Curve { allPoints = zCLineDic[curveId2], ls = (LineSeries)(chart4a.Series[2]), slider = rangeSlider3 });
                    // diff zC curve
                    tmp.Add(new Curve
                    {
                        allPoints = diffLineDic[new KeyValuePair<long, long>(curveId1, curveId2)],
                        ls = (LineSeries)(chart3a.Series[0]),
                        slider = rangeSlider3
                    });
                }
                else if (slider.Name == "")             // FX curve
                { }
                else
                { }

				ProcessSlider(tmp);
			}
			catch (KeyNotFoundException)
			{ }
        }

		private void ProcessSlider(List<Curve> curves)
		{
			foreach (Curve c in curves)
			{
				ObservableCollection<EntryPointChart> oldLine = c.allPoints;

				if (oldLine == null)
					continue;

				ObservableCollection<EntryPointChart> newLine = new ObservableCollection<EntryPointChart>();
				foreach (EntryPointChart dp in oldLine)
				{
					if (dp.length < c.slider.RangeStart)		// haven't get into range yet
						continue;

					if (dp.length > c.slider.RangeEnd)			// all other points are out of range
						break;

					newLine.Add(dp);
				}

				c.ls.ItemsSource = newLine;
			}
		}
		/*
		// chart -> list of series -> each selies has Collecton of EntryPointChart .. one slider for all series in chart 
		private void ProcessSlider(List<Chart> charts, RangeSlider slider)
		{
			foreach (Chart c in charts)
			{
				foreach (LineSeries ls in c.Series)
				{
					ObservableCollection<EntryPointChart> oldLine = ls.ItemsSource as ObservableCollection<EntryPointChart>;

					if (oldLine == null)
						continue;

					ObservableCollection<EntryPointChart> newLine = new ObservableCollection<EntryPointChart>();
					foreach (EntryPointChart dp in oldLine)
					{
						if (dp.length < slider.RangeStart)		// haven't get into range yet
							continue;

						if (dp.length > slider.RangeEnd)			// all other points are out of range
							break;

						newLine.Add(dp);
					}

					ls.ItemsSource = newLine;
				}
			}
		}
		*/

        private void dataGridOutput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			int curve_id = CurrentElements.CurrentYCId;
            

			/*  ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();

            foreach (DateTime d in CachedData.CommonDates)
                dList.Add(d);

            YieldCurveData ycd = YcSettingsDic.GetYcSett(curve_id).ycd;
            _service.CalculateDiscountedRateListAsync(ycd, CurrentElements.CurrentDate, dList); */
        }

        private void image1_MouseEnter(object sender, MouseEventArgs e)
        {

        }

		private void buttonCompare_Click(object sender, RoutedEventArgs e)
        {
			//  compareGUI.my_parent = this;
			//  compareGUI.Show();
            /*
			CcyItem.YcItem ci = (CcyItem.YcItem)this.treeView1.SelectedItem;
            if (ci != null)
			    this.comboBox1.SelectedItem = ci.name; */

            if (CurrentElements.CurrentYCId != -1)
                this.comboBox1.SelectedItem = YcSettingsDic.GetYcSett(CurrentElements.CurrentYCId).ycd.Name;
			
			this.LayoutChart1.Visibility = Visibility.Collapsed;
			this.LayoutChart2.Visibility = Visibility.Visible;

          //  AppSettings.Decimals = 2;
		}

		private void buttonCompareBack_Click(object sender, RoutedEventArgs e)
		{
			//  compareGUI.my_parent = this;
			//  compareGUI.Show();
			//   CcyItem.YcItem ci = (CcyItem.YcItem)this.treeView1.SelectedItem;
			//   this.comboBox1.SelectedItem = ci.name;
            ((LineSeries)this.chart1.Series[0]).Refresh();
            ((LineSeries)this.chart1.Series[1]).Refresh();
            ((LineSeries)this.chart1.Series[2]).Refresh();
            
       //     DrawEntryPointChartFromCache(CurrentElements.CurrentYCId, null); //redrawing it as there is some weird behav

			this.LayoutChart1.Visibility = Visibility.Visible;
			this.LayoutChart2.Visibility = Visibility.Collapsed;


            AppSettings.ActiveWindow = 1;
        }

        //========

        public class ExchangeGrid
        {
			public string reference { get; set; }
            public string Currency { get; set; }

			public string col0 { get; set; }
			public string col1 { get; set; }
			public string col2 { get; set; }
			public string col3 { get; set; }
			public string col4 { get; set; }
			public string col5 { get; set; }
			public string col6 { get; set; }
			public string col7 { get; set; }
			public string col8 { get; set; }
			public string col9 { get; set; }
			public string col10 { get; set; }
        }

        private bool FillExchangeGridFromCache(DateTime? dt)
        {
			DateTime SettlementDate;

			if (dt == null)
				SettlementDate = CurrentElements.CurrentDate; //YcSettingsList.CurrentDate; //settlementDate;
			else
				SettlementDate = (DateTime)dt;

			/* 
            List<DataGridColumn> ldgc = new List<DataGridColumn>();

            foreach (DataGridColumn dgc in this.dataGridCrossCurrency.Columns)
            {
                if (dgc.Header.ToString() != "reference")
                    ldgc.Add(dgc);
                     
            }

            foreach (DataGridColumn dgc in ldgc)
                this.dataGridCrossCurrency.Columns.Remove(dgc); */

			//     this.dataGridCrossCurrency.Columns.Clear();
			//         this.dataGridCrossCurrency.Columns.Add(CreateTextColumn("Currency", ""));
        
			string colBase = "col";
			int j = 0;
			string colName;

			foreach (String c in YcSettingsDic.NotEmptyCurrency())
			{
				colName = colBase + j.ToString();
				j = j + 1;

				//this.dataGridCrossCurrency.Columns.Add(CreateTextColumn(colName, c));
				this.dataGridCrossCurrency.Columns[j].Header = c; //..Add(CreateTextColumn(colName, c));
			}

			//  int i = 0;
			for (int i = j + 1; i < this.dataGridCrossCurrency.Columns.Count(); i++)
				this.dataGridCrossCurrency.Columns.Remove(this.dataGridCrossCurrency.Columns[i]);

			#region ---------------  hyperlink  ----------------------------------

			/*      DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
            templateColumn.Header = "reference";
                                 
            StringBuilder sb = new StringBuilder();
            sb.Append("<DataTemplate ");
            sb.Append("xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' >");
            sb.Append("<Button Content={Binding col6} >");
         //   sb.Append("Click='button3_Click' ");
            //   sb.Append("NavigateUri='http://forums.silverlight.net/t/197254.aspx/1?Programmatically+create+a+HyperlinkButton+in+a+DataGrid' ");
        //    sb.Append(" />");
            sb.Append("</DataTemplate>");  

       //     StringBuilder sb = new StringBuilder();

      /*      sb.Append("<DataTemplate> ");
            sb.Append("xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' ");
            sb.Append("xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' ");
            sb.Append("xmlns:navigation='clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Navigation' ");
            sb.Append("xmlns:controls='clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls' ");
            sb.Append("xmlns:data='clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data' ");
            sb.Append("xmlns:src='clr-namespace:Silverlight1.Classes;assembly=Silverlight1.Classes'>" );

            sb.Append("<DataGridTemplateColumn Header='reference'> ");
            sb.Append("<data:DataGridTemplateColumn.CellTemplate> ");
            
            sb.Append("<HyperlinkButton Click='button5_Click' Margin='4' Content='{Binding reference}' x:Name='aaa' /> ");
            
            sb.Append("</data:DataGridTemplateColumn.CellTemplate> ");
            sb.Append("</data:DataGridTemplateColumn> ");
            sb.Append("</DataTemplate> "); */

			//       templateColumn.CellTemplate = (DataTemplate)XamlReader.Load(sb.ToString());

			//      templateColumn.IsReadOnly = false;
			//       this.dataGridCrossCurrency.Columns.Add(templateColumn);

			DataGridTemplateColumn templateColumn1 = new DataGridTemplateColumn();
			templateColumn1.Header = "test2";
			// templateColumn1.CellTemplate = (System.Windows.DataTemplate)
			/*   templateColumn1.CellTemplate = (DataTemplate)XamlReader.Load(@"<DataTemplate 
					xmlns='http://schemas.microsoft.com/client/2007'   
					xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>  
					<Button Content='Button' Width='50' x:Name='aaa'></Button> </DataTemplate>");*/

			/* templateColumn1.CellTemplate = Resources(datatemplateaccount);
			 this.dataGridCrossCurrency.Columns.Add(templateColumn1); */

			#endregion

			ObservableCollection<ExchangeGrid> OutputDataGridSource = new ObservableCollection<ExchangeGrid>();
			List<String> ColumnValues = new List<String>();

			foreach (DataGridColumn d in this.dataGridCrossCurrency.Columns)
			{
				ColumnValues.Clear();
				if (d.Header.ToString() != "")
				{
					String curCode = d.Header.ToString();
					foreach (DataGridColumn d1 in this.dataGridCrossCurrency.Columns)
					{
						double val = 0;
						if (d1.Header.ToString() != "")
							val = CachedData.GetExchangeRateValue(curCode, d1.Header.ToString(), CurrentElements.CurrentDate);
						if (val != 0)
							ColumnValues.Add(val.ToString());
						else
							ColumnValues.Add("");
					}

                    OutputDataGridSource.Add(new ExchangeGrid()
					{
						//   reference="ref",
						Currency = d.Header.ToString(),
						col0 = ColumnValues[1],
						col1 = ColumnValues[2],
						col2 = ColumnValues[3],
						col3 = ColumnValues[4],
						col4 = ColumnValues[5],
						col5 = ColumnValues[6]
					//	col6 = ColumnValues[6]
					});
				}
			}

			this.dataGridCrossCurrency.ItemsSource = OutputDataGridSource;

			this.dataGridCrossCurrency.InvalidateMeasure();
			this.dataGridCrossCurrency.UpdateLayout();

			return true;
        }

		public bool DrawCompareDiffCurves(int CurveId1, int CurveId2)
		{
			chart3a.Visibility = System.Windows.Visibility.Visible;
			chart4a.Visibility = System.Windows.Visibility.Visible;
#if __VC10__
			HashSet<DateTime> DatesToDiscount = CachedData.GetCommonCalendarFromCachedCurves(CurveId1, CurveId2);
#else
			Dictionary<DateTime, bool> DatesToDiscount = CachedData.GetCommonCalendarFromCachedCurves(CurveId1, CurveId2);
#endif
			ObservableCollection<DicountPointGrid> dataGridOutput = new ObservableCollection<DicountPointGrid>();  //this list will contain only results for DatesToDiscount dates

			Color color = Colors.Black;

			YieldCurveComputed yc1 = null;
			YieldCurveComputed yc2 = null;

			try
			{
				yc1 = CachedData.CachedYieldCurvesDic[CurveId1];
				yc2 = CachedData.CachedYieldCurvesDic[CurveId2];
			}
			catch (KeyNotFoundException)
			{
				MessageBox.Show("not yet computed");
				return false;
			}

			ObservableCollection<EntryPointChart> CurvePoints = new ObservableCollection<EntryPointChart>();

			long maxDay = 0;
			int i = -1;

            foreach (DicountPointGrid d in yc1.Points)
			{
				i = i + 1;
				TimeSpan diff = d.dateToDiscount - CurrentElements.CurrentDate;
				double r = d.zcRate - yc2.Points[i].zcRate;
				EntryPointChart dp = new EntryPointChart
				{
					rate = r * 100,
					length = diff.Days,
					termStr = String.Format("{0} {1}", d.Length, d.TimeUnit)
				};

				CurvePoints.Add(dp);

				if (diff.Days > maxDay)
					maxDay = diff.Days;
#if __VC10__
				if (DatesToDiscount.Contains(d.dateToDiscount))
#else
				if (DatesToDiscount.ContainsKey(d.dateToDiscount))
#endif
				{
					DicountPointGrid ycdg = new DicountPointGrid();
					ycdg.dateToDiscount = d.dateToDiscount;
					ycdg.zcRate = d.zcRate;
					ycdg.discount = yc2.Points[i].zcRate;
					ycdg.frwRate = d.zcRate - yc2.Points[i].zcRate;//ycdg.zcRate - ycdg.frwRate;

        			dataGridOutput.Add(ycdg);
				}
			}

			this.dataGridDiff.ItemsSource = dataGridOutput;
			((LineSeries)this.chart3a.Series[0]).ItemsSource = CurvePoints;

			diffLineDic.Add(new KeyValuePair<long, long>(CurveId1, CurveId2), CurvePoints);

			this.rangeSlider3.Minimum = this.rangeSlider.RangeStart = 0;
			this.rangeSlider3.Maximum = maxDay;
			this.rangeSlider3.RangeEnd = maxDay;

			return true;
		}

		//this function will draw the fx curve and fill the datagarid with forward fx rates
		public bool DrawFXCurve(int CurveId1, int CurveId2)
		{
			string curCode1 = "";
			string curCode2 = "";

			foreach (var ycsd in YcSettingsDic.ycdDic)
			{
				if (ycsd.Value.ycd.Id == CurveId1)
					curCode1 = CachedData.CachedCurrencyDic[ycsd.Value.ycd.CurrencyId].Code;
				if (ycsd.Value.ycd.Id == CurveId2)
					curCode2 = CachedData.CachedCurrencyDic[ycsd.Value.ycd.CurrencyId].Code;
			}
			double val = CachedData.GetExchangeRateValue(curCode1, curCode2, CurrentElements.CurrentDate);
#if __VC10__
			HashSet<DateTime> DatesToDiscount = CachedData.GetCommonCalendarFromCachedCurves(CurveId1, CurveId2);
#else
			Dictionary<DateTime, bool> DatesToDiscount = CachedData.GetCommonCalendarFromCachedCurves(CurveId1, CurveId2);
#endif
			ObservableCollection<DicountPointGrid> dataGridOutput = new ObservableCollection<DicountPointGrid>();
			Color color = Colors.Black;

			YieldCurveComputed yc1 = null;
			YieldCurveComputed yc2 = null;

			try
			{
				yc1 = CachedData.CachedYieldCurvesDic[CurveId1];
				yc2 = CachedData.CachedYieldCurvesDic[CurveId2];
			}
			catch (KeyNotFoundException)
			{
				MessageBox.Show("not yet computed");
				return false;
			}

			ObservableCollection<EntryPointChart> CurvePoints = new ObservableCollection<EntryPointChart>();

			long maxDay = 0;
			int i = -1;

			EntryPointChart dp = new EntryPointChart
			{
				rate = val,
				length = 0,
				termStr = String.Format("{0} {1}", 0, "Days")
			};

			CurvePoints.Add(dp);

			foreach (DicountPointGrid d in yc1.Points)
			{
				i++;
#if __VC10__
				if (DatesToDiscount.Contains(d.dateToDiscount))
#else
				if (DatesToDiscount.ContainsKey(d.dateToDiscount))
#endif
				{
					TimeSpan diff = d.dateToDiscount - CurrentElements.CurrentDate;
					double compound = d.discount / yc2.Points[i].discount;

					dp = new EntryPointChart
					{
						rate = val * compound,
						length = diff.Days,
						termStr = String.Format("{0} {1}", d.Length, d.TimeUnit)
					};

					CurvePoints.Add(dp);

					DicountPointGrid ycdg = new DicountPointGrid();
					ycdg.dateToDiscount = d.dateToDiscount;
					ycdg.frwRate = val * compound;

					dataGridOutput.Add(ycdg);

					if (diff.Days > maxDay)
						maxDay = diff.Days;
				}
			}

			this.dataGridOutput.InvalidateMeasure();
			this.dataGridOutput.UpdateLayout();

			((LineSeries)this.chart2.Series[0]).ItemsSource = CurvePoints;
			this.dataGridForwardRate.ItemsSource = dataGridOutput;// OutputDataGridSource;

			/* TODO: no slider yet - create one in GUI
			this.rangeSlider.Minimum = this.rangeSlider.RangeStart = 0;
			this.rangeSlider.Maximum = maxDay;
			this.rangeSlider.RangeEnd = maxDay;
			*/
			return true;
		}

        private static DataGridTextColumn CreateTextColumn(string fieldName, string title)
        {
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = title;
            column.Binding = new System.Windows.Data.Binding(fieldName);
            return column;
        }

		private void LineSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{}

        private void buttonCompare_Click1(object sender, RoutedEventArgs e)
		{
            int series1 = 1;
            int series2 = 2;
		    ((LineSeries)this.chart4a.Series[series1]).ItemsSource = null;  //first curve
            ((LineSeries)this.chart4a.Series[series2]).ItemsSource = null;  //second curve
            ((LineSeries)this.chart3a.Series[0]).ItemsSource = null;        //chart with spread curve
            this.dataGridDiff.ItemsSource = null;

			if ((((String)comboBox1.SelectedItem) == "") || (((String)comboBox2.SelectedItem) == ""))
			{
				MessageBox.Show("Two curves must be selected");
				return;
			}

			int CurveId1 = CachedData.GetYieldCurveIDbyName((String)comboBox1.SelectedItem);
			int CurveId2 = CachedData.GetYieldCurveIDbyName((String)comboBox2.SelectedItem);

			bool ifComputed1 = CachedData.CachedYieldCurvesDic.ContainsKey(CurveId1)				//everything is already cached
								&& CachedData.CachedYieldCurvesDic[CurveId1].Points.Count != 0;
			bool ifComputed2 = CachedData.CachedYieldCurvesDic.ContainsKey(CurveId2)				//everything is already cached
								&& CachedData.CachedYieldCurvesDic[CurveId2].Points.Count != 0;

			if (!ifComputed1)
			{
				YcSettings s = YcSettingsDic.GetYcSett(CurveId1);
				ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();

                foreach (DateTime d in CachedData.CommonDates)
                    dList.Add(d);

    	         _service.CalculateDiscountedRateListAsync(YcSettingsDic.GetYcSett(CurveId1).ycd, CurrentElements.CurrentDate, dList, false);
			}
		
			if (!ifComputed2)
			{
				YcSettings s = YcSettingsDic.GetYcSett(CurveId2);
        		ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();

                foreach (DateTime d in CachedData.CommonDates)
                    dList.Add(d);

			    _service.CalculateDiscountedRateListAsync(YcSettingsDic.GetYcSett(CurveId2).ycd, CurrentElements.CurrentDate, dList, false);
			}
	
            if (ifComputed1 && ifComputed2)
            {
                DrawResultCurveFromCache(CurveId1, true, series1, AppSettings.FirstCurveColor, 2);
                DrawResultCurveFromCache(CurveId2, true, 2, AppSettings.SecondCurveColor, 2);
                DrawCompareDiffCurves(CurveId1, CurveId2);
            }
            else
            {
                FirstYCId_gl = CurveId1;
                SecondYCId_gl = CurveId2;
                StartTimerCompareCurves();
            }
		}

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
		{}

		private void fx_loaded(object sender, RoutedEventArgs e)
		{
            ((LineSeries)this.chart2.Series[0]).ItemsSource = null;
            this.dataGridForwardRate.ItemsSource = null;
            		
			SolidColorBrush highlightBrush = new SolidColorBrush(Colors.Blue);
            SolidColorBrush defaultBrush = new SolidColorBrush(Colors.Transparent);
            SolidColorBrush newcolor = new SolidColorBrush(Color.FromArgb(144, 144, 144, 144));

			Button btn = sender as Button;
            int row = dataGridCrossCurrency.SelectedIndex;
			int col = dataGridCrossCurrency.CurrentColumn.DisplayIndex;
            ExchangeGrid eg = (ExchangeGrid)dataGridCrossCurrency.SelectedItem;
            string FirstCur = eg.Currency;
            string SecondCur = dataGridCrossCurrency.Columns[col].Header.ToString();

            if (CurrentElements.FXElement.btn != null)
                CurrentElements.FXElement.btn.Background = defaultBrush;

            CurrentElements.FXElement.btn = btn;
            CurrentElements.FXElement.ColId = col;
            CurrentElements.FXElement.RowId = row;
            CurrentElements.FXElement.FirstCurrency = FirstCur;
            CurrentElements.FXElement.SecondCurrency = SecondCur;

            btn.Background = newcolor; // highlightBrush;
			var entity = btn.DataContext;
			dataGridCrossCurrency.SelectedItem = entity;
			
			int FirstYCId = CachedData.GetDefaultYieldCurveIDbyCurrencyName(FirstCur);
			int SecondYCId = CachedData.GetDefaultYieldCurveIDbyCurrencyName(SecondCur);
            SecondYCId_gl = SecondYCId;
            FirstYCId_gl = FirstYCId;

            if (!CachedData.CachedYieldCurvesDic.ContainsKey(FirstYCId))
            {
                ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();
			    foreach (DateTime d in CachedData.CommonDates)
    				dList.Add(d);

                YieldCurveData ycd = YcSettingsDic.GetYcSett(FirstYCId).ycd;
			    _service.CalculateDiscountedRateListAsync(ycd, CurrentElements.CurrentDate, dList, false);
            }

            if (!CachedData.CachedYieldCurvesDic.ContainsKey(SecondYCId))
            {
                ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();
                foreach (DateTime d in CachedData.CommonDates)
                    dList.Add(d);

                YieldCurveData ycd = YcSettingsDic.GetYcSett(SecondYCId).ycd;
                _service.CalculateDiscountedRateListAsync(ycd, CurrentElements.CurrentDate, dList, false);
            }

            if ((!CachedData.CachedYieldCurvesDic.ContainsKey(FirstYCId)) ||
            (!CachedData.CachedYieldCurvesDic.ContainsKey(SecondYCId)))
            {
                StartTimerButton_Click();
            }
            else
            {
                bool res = DrawFXCurve(FirstYCId_gl, SecondYCId_gl);
            }

 		}

        private void dataGridOutput_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			//_service.InitAsync(CurrentElements.CurrentDate);
            //_service.InflationInitAsync(CurrentElements.CurrentDate);
		}


        #region --------------------- Inflation ----------------------------


        void lb_InflationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            CcyItem.YcItem tmp = (e.AddedItems[0] as CcyItem.YcItem);
            if (tmp == null)
                return;

            if (CurrentElements.CurrentICId == tmp.idYc)
                return;



           
            // close other open expanders / deselect their curve selected
            foreach (var i in this.ExpanderStackInflationParent.Children)
            {
                Expander exp = i as Expander;
                if (exp != null)
                {
                    ListBox lb = exp.Content as ListBox;
                    if (lb != null
                        && lb.SelectedItem != null
                        && lb.SelectedItem is CcyItem.YcItem
                        && ((lb.SelectedItem as CcyItem.YcItem).idYc != tmp.idYc)
                        )
                        lb.SelectedIndex = -1;
                }
            }
            //

        //    DrawInflationEntryCurveFromCache(null,null);


            int series1 = 1;
            int series2 = 2;
            ((LineSeries)this.chartInflation1.Series[series1]).ItemsSource = null;
            ((LineSeries)this.chartInflation1.Series[series2]).ItemsSource = null;
            this.dataGridInflationOutput.ItemsSource = null;
            this.dataGridInflationEntryRates.ItemsSource = null;

            int CurveId1 = tmp.idYc;
            CurrentElements.CurrentICId = tmp.idYc;
            
       //     DrawInflationResultCurveFromCache();
       //     DrawEntryPointChartFromCache(tmp.idYc, null);
            //    ((LineSeries)this.chart1.Series[0]).ItemsSource = null;
            ((LineSeries)this.chart1.Series[0]).Refresh();

            
        //    int curve_id = tmp.idYc;

            bool ifComputed1 = CachedData.CachedInflationCurvesDic.ContainsKey(CurveId1)				//everything is already cached
                                && CachedData.CachedInflationCurvesDic[CurveId1].Points.Count != 0;
           
            if (!ifComputed1)
            {
                InflationCurveSnapshot ics = CachedData.GetInflationCurveSnapshot(CurrentElements.CurrentICId, CurrentElements.CurrentDate);

                ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();
                List<DateTime> dList1 = new List<DateTime>();


                // --- this to be changed as dates should be computed using a calendar
                // so in the future to be replaced by GetMaturityDatesList
                
                foreach (InflationCurveEntryData iced in ics.EntryList)
                {
                    DateTime d=DateTime.Today;
                    if (iced.Instrument.Type == "swap") //inflation rate
                    {
                        InflationRate ir = ((InflationRate)iced.Instrument);
                        if(ir.TimeUnit=="Days")
                            d = CurrentElements.CurrentDate.AddDays(ir.Duration);
                        else if(ir.TimeUnit=="Months")
                            d = CurrentElements.CurrentDate.AddMonths(ir.Duration);
                        else if (ir.TimeUnit == "Years")
                            d = CurrentElements.CurrentDate.AddYears(ir.Duration);

                        dList1.Add(d);
                    }

                }

                dList1.Sort();
                foreach (DateTime dt in dList1)
                    dList.Add(dt);
                //to be fixed
                //for a moment finlayer crashes is the date to discount > entry point maturity
              
                if (dList.Count > 0)
                    dList.Remove(dList[dList.Count() - 1]);
              

                //CurrentElements.InflationDates.Clear();
                // foreach (DateTime d in CachedData.CommonDates)
                //    dList.Add(d);

                CurrentElements.CurrentInflationCurveSnapshot = ics;
                if (ics != null)
                    DrawInflationEntryCurveFromCache(null, null);
               _service.CalculateInflationRateListAsync(ics, CurrentElements.CurrentDate, dList, true);
              
             
                InflationCurveId_gl = CurveId1;
                StartTimerInflationCurve();
            }
            else
            {
                DrawInflationResultCurveFromCache(CurveId1);
            }

            /*
            

           

            if (
                CachedData.CachedYieldCurvesDic.ContainsKey(curve_id)				//everything is already cached
                && CachedData.CachedYieldCurvesDic[curve_id].Points.Count != 0
                )
            {
                if (YcSettingsDic.GetYcSett(curve_id).ifZCCurve)
                    DrawResultCurveFromCache(curve_id, true, 1, YcSettingsDic.GetYcSett(curve_id).ZCColor, 1);

                if (YcSettingsDic.GetYcSett(curve_id).ifForwardCurve)
                    DrawResultCurveFromCache(curve_id, false, 2, YcSettingsDic.GetYcSett(curve_id).FrwColor, 1);

                FillOutputDataGridFromCache(null);
                DrawEntryPointGridFromCache(tmp.idYc, null);
            }

            ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();

           
#if __VC10__
            foreach (DateTime d in CachedData.CommonDates)
#else
			foreach ( DateTime d in CachedData.CommonDates.Keys)
#endif
                dList.Add(d);

            YieldCurveData ycd = YcSettingsDic.GetYcSett(curve_id).ycd;
            _service.CalculateDiscountedRateListAsync(ycd, CurrentElements.CurrentDate, dList, true);

            */
        }

        public bool DrawInflationResultCurveFromCache(int curveId)
        {
                     
           long maxDay = 0;

            #region ---------------using cache-----------------

 
            //------------after cache is filled
           YieldCurveComputed yc = new YieldCurveComputed();
           yc = null;

           try
           {
               yc = CachedData.CachedInflationCurvesDic[curveId];
           }
           catch (KeyNotFoundException)
           {
               MessageBox.Show("not yet computed");
               return false;
           }

           ObservableCollection<EntryPointChart> RatePoints = new ObservableCollection<EntryPointChart>();
           ObservableCollection<EntryPointChart> IndexPoints = new ObservableCollection<EntryPointChart>();

           foreach (DicountPointGrid d in yc.Points)
           {
               TimeSpan diff = d.dateToDiscount - CurrentElements.CurrentDate;
            
               RatePoints.Add(new EntryPointChart
               {
                   rate = d.zcRate,
                   length = diff.Days,
                   termStr = String.Format("{0} {1}", d.Length, d.TimeUnit)
               });

               IndexPoints.Add(new EntryPointChart
               {
                   rate = d.frwRate,
                   length = diff.Days,
                   termStr = String.Format("{0} {1}", d.Length, d.TimeUnit)
               });

               if (diff.Days > maxDay)
                   maxDay = diff.Days;
           }

           ((LineSeries)this.chartInflation1.Series[2]).ItemsSource = RatePoints; //rate should be drawn against left Yaxis along with entry rates
           ((LineSeries)this.chartInflation1.Series[1]).ItemsSource = IndexPoints; //index should be drawn against right Y-axis
       //    this.LeftInflationAxis.Maximum = ;
                         
           #endregion

            this.rangeSlider.Minimum = this.rangeSlider.RangeStart = 0;
            this.rangeSlider.Maximum = maxDay;
            this.rangeSlider.RangeEnd = maxDay;
          //  ((LineSeries)this.chartInflation1.Series[1]).Refresh();


            return true;
        }

        public bool DrawInflationEntryCurveFromCache(int? curveId, DateTime? dt)
        {
            DateTime sd = (dt == null ? CurrentElements.CurrentDate : dt.Value);
            int curve_id = (curveId == null ? CurrentElements.CurrentICId : curveId.Value);

            ObservableCollection<EntryPointChart> EntryCurveDataPoints = new ObservableCollection<EntryPointChart>(); //in%
            EntryCurveDataPoints.Clear();

            long maxDay = 0;
            // foreach (YieldCurveEntryDataHistory y in CachedData.CachedEntryPointHistoryList)

         //    ObservableCollection<EntryPointGrid> EntrySource = new ObservableCollection<EntryPointGrid>();

            for (int i = 0; i < CurrentElements.CurrentInflationCurveSnapshot.EntryList.Count(); i++)
            {
                InflationCurveEntryData y = CurrentElements.CurrentInflationCurveSnapshot.EntryList[i];
                double val = CurrentElements.CurrentInflationCurveSnapshot.ValueList[i];

               
                 EntryCurveDataPoints.Add(
                    new EntryPointChart
                    {
                        type = y.Type,
                        rate = (y.Type == "inflationbond" ? (double)val : (double)val),
                        length = y.Duration,
                        maturity = (y.Type == "inflationbond"
                                ? (y.Instrument as InflationBond).MaturityDate.ToShortDateString()
                                : (y.Instrument as InflationRate).Duration.ToString() + (y.Instrument as InflationRate).TimeUnit),
                        termStr = (y.Type == "inflationbond"
                                ? (y.Instrument as InflationBond).MaturityDate.ToShortDateString()
                                : String.Format("{0} {1}", (y.Instrument as InflationRate).Duration.ToString(), (y.Instrument as InflationRate).TimeUnit))
                               
                    });

                maxDay = Math.Max(maxDay, y.Duration + 2);
            }

      

            ((LineSeries)this.chartInflation1.Series[0]).ItemsSource = null;
            ((LineSeries)this.chartInflation1.Series[0]).ItemsSource = EntryCurveDataPoints;
        /*    if (entryLineDic.ContainsKey(curve_id))
                entryLineDic[curve_id].Clear();
            entryLineDic[curve_id] = EntryCurveDataPoints;*/

            this.rangeSlider.Minimum = this.rangeSlider.RangeStart = 0;
            this.rangeSlider.Maximum = maxDay;
            this.rangeSlider.RangeEnd = maxDay;
            ((LineSeries)this.chartInflation1.Series[0]).Refresh();


            return true;
        }

        private bool FillInflationOutputDataGridFromCache(int? curveId)
        {
            int curve_id = (curveId == null ? CurrentElements.CurrentICId : (int)curveId);

            YieldCurveComputed yc = null;
            try
            {
                //   yc = CachedData.CachedYieldCurvesDic[curve_id];
                yc = CachedData.CachedInflationCurvesDic[curve_id];
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("not yet computed");
                return false;
            }

            ObservableCollection<DicountPointGrid> OutputDataGridSource = new ObservableCollection<DicountPointGrid>();

            foreach (DicountPointGrid d in yc.Points)
            {
                OutputDataGridSource.Add(
                    new DicountPointGrid()
                    {
                        dateToDiscount = d.dateToDiscount,
                        Length = d.Length,
                        TimeUnit = "Days",
                        zcRate = d.zcRate,
                        discount = d.frwRate, //it is not an error
                        frwRate = d.discount //it is not an error
                    }
                );
            }


            //this.dataGridOutput.ItemsSource = yc.Points;// OutputDataGridSource;

            //         ObservableCollection<DicountPointGrid> dataGridOutput = new ObservableCollection<DicountPointGrid>();
            /*        foreach (YieldCurveEntryDataHistory y in CachedData.CachedEntryPointHistoryList)
                    {
                        if (y.YieldCurveId != curve_id)
                            continue;

                        foreach (DicountPointGrid ycdg in yc.Points)
                            if (ycdg.dateToDiscount == CurrentElements.CurrentDate.AddDays(y.Duration))
                                dataGridOutput.Add(ycdg);
                    } */

            this.dataGridInflationOutput.ItemsSource = OutputDataGridSource; // OutputDataGridSource;

            this.dataGridInflationOutput.InvalidateMeasure();
            this.dataGridInflationOutput.UpdateLayout();

            return true;
        }

        //it fills it from current inflation snapshot
        private bool FillInflationInputDataGridFromCache()
        {
            ObservableCollection<EntryPointGrid> EntrySource = new ObservableCollection<EntryPointGrid>();

            for (int i = 0; i < CurrentElements.CurrentInflationCurveSnapshot.EntryList.Count(); i++)
            {
                InflationCurveEntryData y = CurrentElements.CurrentInflationCurveSnapshot.EntryList[i];
                double val = CurrentElements.CurrentInflationCurveSnapshot.ValueList[i];

                EntrySource.Add(new EntryPointGrid()
                {
                    enabled = y.Enabled,
                    type = y.Type,
                    //   value = (val == null ? 0 : (y.Type == "inflationbond" ? (double)val : (double)val * 100)),
                    value = val,
                    reference = y.Instrument.Name,
                    maturity = (y.Type == "inflationbond"
                                ? (y.Instrument as InflationBond).MaturityDate.ToShortDateString()
                                : (y.Instrument as InflationRate).Duration.ToString() + (y.Instrument as InflationRate).TimeUnit)
                });
            }

            this.dataGridInflationEntryRates.ItemsSource = EntrySource;
            /*
            // TODO: this part of the code doesnt work
            // it changes the font of the entry line if it is disabled
            foreach (EntryPointGrid edg in EntrySource)
            {
                if (edg.enabled)
                    continue;

                // FrameworkElement el = this.dataGridEntryRates.Columns[1].GetCellContent(j);

                this.dataGridEntryRates.SelectedIndex = 5;
                FrameworkElement el = this.dataGridEntryRates.Columns.Last().GetCellContent(this.dataGridEntryRates.SelectedItem);
                
                if (el == null) return true;

                DataGridRow row = DataGridRow.GetRowContainingElement(el.Parent as FrameworkElement);
                if (row == null) return true;

                row.FontSize = 5; 

                    //     DataGridCell changeCell =(DataGridCell) this.dataGridEntryRates.Columns[1].GetCellContent(this.dataGridEntryRates.ItemsSource.Cast<object>().ElementAt(1));
                    //   DataGridCell changeCell = (DataGridCell)el.Parent;
                    //   changeCell.FontSize = 4;
                    // SolidColorBrush brush = new SolidColorBrush(Colors.Black);
            }
            */
            return true;
        }


       


        private void testButton_click(object sender, RoutedEventArgs e)
        {
            int series1 = 1;
            int series2 = 2;
            ((LineSeries)this.chartInflation1.Series[series1]).ItemsSource = null;
            ((LineSeries)this.chartInflation1.Series[series2]).ItemsSource = null;
            this.dataGridInflationOutput.ItemsSource = null;
            this.dataGridInflationEntryRates.ItemsSource = null;

            int CurveId1 = 1;
         
            bool ifComputed1 = CachedData.CachedInflationCurvesDic.ContainsKey(CurveId1)				//everything is already cached
                                && CachedData.CachedInflationCurvesDic[CurveId1].Points.Count != 0;

            if (!ifComputed1)
            {
                YcSettings s = YcSettingsDic.GetYcSett(CurveId1);
                ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();

                foreach (DateTime d in CachedData.CommonDates)
                    dList.Add(d);

                InflationCurveSnapshot ics = CachedData.GetInflationCurveSnapshot(CurrentElements.CurrentICId, CurrentElements.CurrentDate);
                _service.CalculateInflationRateListAsync(ics, CurrentElements.CurrentDate, dList, false);



                InflationCurveId_gl = CurveId1;
                StartTimerInflationCurve();
            }
            else
            {
                DrawInflationResultCurveFromCache(CurveId1);
            }

        }

        #endregion

        #region --------------- dispatchers ---------------------------------
        //dispatcher for fx computations
        private DispatcherTimer _uiTimer;
        private int SecondYCId_gl;
        private int FirstYCId_gl;

        private void StartTimerButton_Click()
        {
            _uiTimer = new DispatcherTimer();
            _uiTimer.Interval = TimeSpan.FromSeconds(2.0);
            _uiTimer.Tick += new EventHandler(UITimer_Tick);
            _uiTimer.Start();
        }

        private void UITimer_Tick(object sender, EventArgs e)
        {
      //      string message = string.Format("{0} - {1}", DateTime.Now, "UI Timer tick");
            if ((CachedData.CachedYieldCurvesDic.ContainsKey(SecondYCId_gl)) &&
                (CachedData.CachedYieldCurvesDic.ContainsKey(FirstYCId_gl)))
            {
                bool res = DrawFXCurve(FirstYCId_gl, SecondYCId_gl);
                StopAll();
            }


        }

        private void StopAll()
        {
            if (_uiTimer != null)
            {
                _uiTimer.Stop();
            }
        }

        //dispatcher for curve comparison computation
        private DispatcherTimer _uiTimerCompareCurves;
        private void StartTimerCompareCurves()
        {
            _uiTimerCompareCurves = new DispatcherTimer();
            _uiTimerCompareCurves.Interval = TimeSpan.FromSeconds(2.0);
            _uiTimerCompareCurves.Tick += new EventHandler(UITimerCompareCurves_Tick);
            _uiTimerCompareCurves.Start();
        }
        private void UITimerCompareCurves_Tick(object sender, EventArgs e)
        {
            bool ifComputed1 = CachedData.CachedYieldCurvesDic.ContainsKey(FirstYCId_gl)				//everything is already cached
                                && CachedData.CachedYieldCurvesDic[FirstYCId_gl].Points.Count != 0;
            bool ifComputed2 = CachedData.CachedYieldCurvesDic.ContainsKey(SecondYCId_gl)				//everything is already cached
                                && CachedData.CachedYieldCurvesDic[SecondYCId_gl].Points.Count != 0;

            if (ifComputed1 && ifComputed2)
            {
                int series1 = 1;
                int series2 = 2;
                ((LineSeries)this.chart4a.Series[series1]).ItemsSource = null;  //first curve
                ((LineSeries)this.chart4a.Series[series2]).ItemsSource = null;  //second curve
                ((LineSeries)this.chart3a.Series[0]).ItemsSource = null;        //chart with spread curve
                DrawCompareDiffCurves(FirstYCId_gl, SecondYCId_gl);
                DrawResultCurveFromCache(FirstYCId_gl, true, series1, AppSettings.FirstCurveColor, 2);
                DrawResultCurveFromCache(SecondYCId_gl, true, series2, AppSettings.SecondCurveColor, 2);
                StopCompare();
            }
        }
        private void StopCompare()
        {
            if (_uiTimerCompareCurves != null)
            {
                _uiTimerCompareCurves.Stop();

            }
        }

        //dispatcher for Inflation curve computation
        private int InflationCurveId_gl;
        private DispatcherTimer _uiTimerInflationCurve;
        private void StartTimerInflationCurve()
        {
            _uiTimerInflationCurve = new DispatcherTimer();
            _uiTimerInflationCurve.Interval = TimeSpan.FromSeconds(2.0);
            _uiTimerInflationCurve.Tick += new EventHandler(UITimerInflationCurve_Tick);
            _uiTimerInflationCurve.Start();
        }
        private void UITimerInflationCurve_Tick(object sender, EventArgs e)
        {
            bool ifComputed1 = CachedData.CachedInflationCurvesDic.ContainsKey(InflationCurveId_gl)				//everything is already cached
                                && CachedData.CachedInflationCurvesDic[InflationCurveId_gl].Points.Count != 0;

            if (ifComputed1)
            {
                int series1 = 1;
                int series2 = 2;
                ((LineSeries)this.chartInflation1.Series[series1]).ItemsSource = null;  //first curve
                ((LineSeries)this.chartInflation1.Series[series2]).ItemsSource = null;  //second curve
                DrawInflationResultCurveFromCache(InflationCurveId_gl);
                StopInflation();
            }
        }
        private void StopInflation()
        {
            if (_uiTimerInflationCurve != null)
            {
                _uiTimerInflationCurve.Stop();

            }
        }

        //dispatcher of inflation initialization
        //initialization of inflation depends on initialization of currency list
        // private bool Currency_Init;
        private DispatcherTimer _uiTimerInflationInit;
        private void StartTimerInflationInit()
        {
            _uiTimerInflationInit = new DispatcherTimer();
            _uiTimerInflationInit.Interval = TimeSpan.FromSeconds(2.0);
            _uiTimerInflationInit.Tick += new EventHandler(UITimerInflationInit_Tick);
            _uiTimerInflationInit.Start();
        }
        private void UITimerInflationInit_Tick(object sender, EventArgs e)
        {

            if (CachedData.CachedCurrencyDic != null 
				&& CachedData.CachedCurrencyDic.Count != 0
				)
            {
                InitInflationList();
                //DrawInflationResultCurveFromCache(InflationCurveId_gl);
                StopInflationInit();
            }
        }
        private void StopInflationInit()
        {
            if (_uiTimerInflationInit != null)
            {
                _uiTimerInflationInit.Stop();

            }
        }

        // ----inflation dates
        private DispatcherTimer _uiTimerInflationDates;
        private void StartTimerInflationDates()
        {
            _uiTimerInflationDates = new DispatcherTimer();
            _uiTimerInflationDates.Interval = TimeSpan.FromSeconds(2.0);
            _uiTimerInflationDates.Tick += new EventHandler(UITimerInflationDates_Tick);
            _uiTimerInflationDates.Start();
        }
        private void UITimerInflationDates_Tick(object sender, EventArgs e)
        {
            bool ifComputed1 = CachedData.CachedInflationCurvesDic.ContainsKey(InflationCurveId_gl)				//everything is already cached
                                && CachedData.CachedInflationCurvesDic[InflationCurveId_gl].Points.Count != 0;

            if (ifComputed1)
            {
                int series1 = 1;
                int series2 = 2;
                ((LineSeries)this.chartInflation1.Series[series1]).ItemsSource = null;  //first curve
                ((LineSeries)this.chartInflation1.Series[series2]).ItemsSource = null;  //second curve
                DrawInflationResultCurveFromCache(InflationCurveId_gl);
                StopInflationDates();
            }
        }
        private void StopInflationDates()
        {
            if (_uiTimerInflationDates != null)
            {
                _uiTimerInflationDates.Stop();

            }
        }

        #endregion

		private void tabItem3_Loaded(object sender, RoutedEventArgs e)
		{
			_service.InflationInitAsync(CurrentElements.CurrentDate);
		}

		private void tabItem1_Loaded(object sender, RoutedEventArgs e)
		{
			_service.InitAsync(CurrentElements.CurrentDate);
		}
    }

    /// <summary>
    /// Chart controll data binding 
    /// </summary>
    public class EntryPointChart
    {
        public string type { get; set; } //bond, rate
        public double rate { get; set; }
        public string termStr { get; set; }
        public long length { get; set; }
        public string maturity { get; set; }

        public object EntryPointChartTooltip
        {
            get
            {
                TextBlock tb = new TextBlock();
                if (type == "bond")
                {
                    tb.Inlines.Add(new Run { Text = "Bond: " + (100 * rate).ToString("#0.0000"), FontWeight = FontWeights.Bold });
                    tb.Inlines.Add(new LineBreak());
                    tb.Inlines.Add(new Run { Text = "Maturity: " + maturity });
                }
                else
                {
                    tb.Inlines.Add(new Run { Text = "Rate: " + rate.ToString("#0.0000"), FontWeight = FontWeights.Bold });
                    tb.Inlines.Add(new LineBreak());
                    tb.Inlines.Add(new Run { Text = "Term: " + termStr });
                }
 
                tb.Inlines.Add(new LineBreak());

                //if (Growth >= 0)
                //	tb.Inlines.Add(new Run { Text = "Growth: " + Growth.ToString(), Foreground = new SolidColorBrush(Colors.Green) });
                //else
                //	tb.Inlines.Add(new Run { Text = "Neg Growth: " + Growth.ToString(), Foreground = new SolidColorBrush(Colors.Red) });

                return tb;
            }
        }
    }

    public class YieldCurveComputed
    {
        public int YieldCurveID;
        public List<DicountPointGrid> Points;
    }

    public class CcyItem
    {
        string _name = string.Empty;
        public string name
        {
            get { return _name.Replace("Currency", ""); }
            set { _name = value; }
        }
        public List<YcItem> SubTree { get; set; }

        public class YcItem
        {
            public int idYc;
            public string name { get; set; }
        }
    }

    // 
    // data grid data binding
    //
    public class DicountPointGrid : INotifyPropertyChanged
    {
        public int Length;
        public string TimeUnit;

        DateTime _dateToDiscount;
        public DateTime dateToDiscount
        {
            get
            {
                return _dateToDiscount;
            }
            set
            {
                _dateToDiscount = value;
                // Call NotifyPropertyChanged when the property is updated
                NotifyPropertyChanged("dateToDiscount");
            }
        }

        double _zcRate;
        public double zcRate
        {
            get
            {
                return _zcRate;
            }
            set
            {
                _zcRate = value;
                // Call NotifyPropertyChanged when the property is updated
                NotifyPropertyChanged("zcRate");
            }
        }

        double _discount;
        public double discount
        {
            get
            {
                return _discount;
            }
            set
            {
                _discount = value;
                // Call NotifyPropertyChanged when the property is updated
                NotifyPropertyChanged("discount");
            }
        }

        double _frwRate;
        public double frwRate
        {
            get
            {
                return _frwRate;
            }
            set
            {
                _frwRate = value;
                // Call NotifyPropertyChanged when the property is updated
                NotifyPropertyChanged("frwRate");
            }
        }

        // Declare the PropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;
        // NotifyPropertyChanged will raise the PropertyChanged event passing the
        // source property that is being updated.
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    //------------------
    public class EntryPointGrid : INotifyPropertyChanged
    {
		public bool enabled = true;

        public int Length;
        public string TimeUnit;

        String _type;
        public String type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                // Call NotifyPropertyChanged when the property is updated
                NotifyPropertyChanged("type");
            }
        }

        String _maturity;
        public String maturity
        {
            get
            {
                return _maturity;
            }
            set
            {
                _maturity = value;
                // Call NotifyPropertyChanged when the property is updated
                NotifyPropertyChanged("maturity");
            }
        }

        double _value;
        public double value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                // Call NotifyPropertyChanged when the property is updated
                NotifyPropertyChanged("value");
            }
        }

        String _reference;
        public String reference
        {
            get
            {
                return _reference;
            }
            set
            {
                _reference = value;
                // Call NotifyPropertyChanged when the property is updated
                NotifyPropertyChanged("reference");
            }
        }

        // Declare the PropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;
        // NotifyPropertyChanged will raise the PropertyChanged event passing the
        // source property that is being updated.
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

	//
    // Term to String Converter
    //

    public class TermToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int v = -1;
            int v2 = -1;

            if (!Int32.TryParse(value.ToString(), out v))
                return "";

            if (v / 365 >= 3)		// years
            {
                v2 = v / 365;
				return v2.ToString() + "y";
            }
            else if (v / 30 >= 3)	// months
            {
                v2 = v / 30;
				return v2.ToString() + "m";
            }
            else if (v / 7 >= 3)	// weeks
			{
                v2 = v / 7;
                return v2.ToString() + "w";
            }
            else						// days
                return v.ToString() + "d";
        }
        
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //
    // grid date formater
    //

    public class DateTimeFormatConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			DateTime dt = (DateTime)value;
            return dt.ToString("dd/MM/yyyy");
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //
    // rate formater
    //

    public class RateFormatConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			double r = -1;

            if (!Double.TryParse(value.ToString(), out r))
                return "";

            //return r.ToString("#0.00000");
            String format = AppSettings.DecimalsString();
            return r.ToString(format);
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
