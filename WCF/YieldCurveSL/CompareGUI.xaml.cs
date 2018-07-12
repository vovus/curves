using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using System.Windows.Navigation;
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ComponentModel;
using System.Text;
using YieldCurveSL.YieldCurveSrv;


namespace YieldCurveSL
{
    public partial class CompareGUI : ChildWindow
    {
        public CompareGUI()
        {
            InitializeComponent();
            CachedData.InitializeYieldCurveComboFromCache(this.comboBox1);
            CachedData.InitializeYieldCurveComboFromCache(this.comboBox5);
        }

        public Page1 my_parent = null;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            Page1 p = my_parent;// this.Parent as Page1;

            if (p.EntryLine != null)
                p.EntryLine.Clear();

            ((LineSeries)p.chart1.Series[0]).ItemsSource = null;

            this.DialogResult = true;
            if ((((String)comboBox1.SelectedItem) == "") || (((String)comboBox5.SelectedItem) == ""))
            {
                MessageBox.Show("Two curves must be selected");
                return;
            }

            int CurveId1 = CachedData.GetYieldCurveIDbyName((String)comboBox1.SelectedItem);
            int CurveId2 = CachedData.GetYieldCurveIDbyName((String)comboBox5.SelectedItem);

			bool ifComputed1 = CachedData.CachedYieldCurvesDic.ContainsKey(CurveId1)				//everything is already cached
								&& CachedData.CachedYieldCurvesDic[CurveId1].Points.Count != 0;
			bool ifComputed2 = CachedData.CachedYieldCurvesDic.ContainsKey(CurveId2)				//everything is already cached
								&& CachedData.CachedYieldCurvesDic[CurveId2].Points.Count != 0;
			
            if (!ifComputed1)
            {
                YcSettings s = YcSettingsDic.GetYcSett(CurveId1);
               
                ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();

				foreach (YieldCurveEntryDataHistory y in CachedData.CachedEntryDataHistoryList)
				{
					if (y.YieldCurveId != CurveId1)
						continue;

					dList.Add(CurrentElements.CurrentDate.AddDays(y.Duration));
				}
				//_service.CalculateDiscountedRateListAsync(YcSettingsDic.GetYcSett(CurveId1).ycd, settlementDate, dList);
            }
            else //everything is already cached
            {
				if (p.ZCLine != null)
                   p.ZCLine.Clear();

				//bool res = p.DrawZCandFrwCurveFromCache(CurveId1, true, false, true);
				bool res = p.DrawResultCurveFromCache(CurveId1, true, 1, this.colorPicker1.Color,2);
            }

            if (!ifComputed2)
            {
				YcSettings s = YcSettingsDic.GetYcSett(CurveId2);

				ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();

				foreach (YieldCurveEntryDataHistory y in CachedData.CachedEntryDataHistoryList)
				{
					if (y.YieldCurveId != CurveId2)
						continue;

					dList.Add(CurrentElements.CurrentDate.AddDays(y.Duration));
				}
				//_service.CalculateDiscountedRateListAsync(YcSettingsDic.GetYcSett(CurveId2).ycd, settlementDate, dList);
            }
            else //everything is already cached
            {
				if (p.frwLine != null)
                    p.frwLine.Clear();

				//bool res = p.DrawZCandFrwCurveFromCache(CurveId2, true, false, false);
				bool res = p.DrawResultCurveFromCache(CurveId2, true, 2, this.colorPicker2.Color,2);
            }


            bool res1;
            if (ifComputed1 & ifComputed2)
                res1 = p.DrawCompareDiffCurves(CurveId1, CurveId2);

            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void colorPicker1_ColorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (colorPicker1 != null)
            {
                // MessageBox.Show(colorPicker1.Color.ToString());
                // (ellipse00.Fill as SolidColorBrush).Color = colorPicker1.Color;
            }
        }

        private void colorPicker2_ColorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (colorPicker2 != null)
            {
                // MessageBox.Show(colorPicker1.Color.ToString());
                // (ellipse00.Fill as SolidColorBrush).Color = colorPicker1.Color;
            }
        }
    }
}

