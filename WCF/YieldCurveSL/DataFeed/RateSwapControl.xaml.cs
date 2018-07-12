using System;
using System.Windows;
using System.Windows.Controls;

namespace YieldCurveSL
{
    using YieldCurveSrv;
    public partial class RateSwapControl : UserControl
    {
		public RateSwapControl()
        {
            InitializeComponent();

			EnumConversion.InitializeComboFromEnum<ratetype>(this.comboTyp);
			EnumConversion.InitializeComboFromEnum<termbase>(this.comboTer);
			CachedData.InitializeCurrencyComboFromCache(this.comboCur);
			//CachedData.InitializeCalendarComboFromCache(this.comboMar);

            EnumConversion.InitializeComboFromEnum<basis>(this.comboBas);
			EnumConversion.InitializeComboFromEnum<compounding>(this.comboMod);
            EnumConversion.InitializeComboFromEnum<frequency>(this.comboFre);
            EnumConversion.InitializeComboFromEnum<BusinessDayConvention>(this.comboBus);
            
            EnumConversion.InitializeComboFromEnum<basis>(this.comboIBas);
            EnumConversion.InitializeComboFromEnum<compounding>(this.comboIMod);
            EnumConversion.InitializeComboFromEnum<frequency>(this.comboIFre);
		}

		public void Init(Rate r)
		{
			this.comboTyp.SelectedItem = r.Type;
			this.txtBoxRef.Text = r.Name;
			this.txtTer.Text = r.Duration.ToString();
			this.comboTer.SelectedItem = r.TimeUnit;
			String CurCode = (CachedData.CachedCurrencyDic.ContainsKey(r.IdCcy)
								? CachedData.CachedCurrencyDic[r.IdCcy].Code
								: "");
			this.comboCur.SelectedItem = CurCode;
			//this.comboMar.SelectedItem = r.FixingPlace;

			this.txtBoxFda.Text = r.SettlementDays.ToString();
			this.comboBas.SelectedItem = (CachedData.CachedDayCounterDic.ContainsKey(r.BasisId) 
								? CachedData.CachedDayCounterDic[r.BasisId].Name
								: "");
			this.comboMod.SelectedItem = r.Compounding;
			this.comboFre.SelectedItem = r.Frequency;
			this.comboBus.SelectedItem = r.BusinessDayConvention.ToString();
			this.textBoxSpread.Text = r.Spread.ToString();

			//this.comboFre.SelectedItem = EnumConversion.getStringFromEnum(frequency.Annual);
			this.txtBoxIRef.Text = r.IndexName.ToString();
			this.comboIBas.SelectedItem = (CachedData.CachedDayCounterDic.ContainsKey(r.BasisIndexId) 
											? CachedData.CachedDayCounterDic[r.BasisIndexId].Name
											: "");
			this.comboIMod.SelectedItem = r.CompoundingIndex;
			this.comboIFre.SelectedItem = r.FrequencyIndex;
			this.textBoxISpread.Text = r.SpreadIndex.ToString();
			//int l = r.Duration;
			//termbase t = (termbase)Enum.Parse(typeof(termbase), r.TimeUnit, true);
			//this.comboIFre.SelectedItem = EnumConversion.getStringFromEnum(EnumConversion.GetFrequencyFromMaturity(l, t));
			
		}

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;
        }
    }
}

