using System;
using System.Windows;
using System.Windows.Controls;

namespace YieldCurveSL
{
    using YieldCurveSrv;
    public partial class RateDepositControl : UserControl
    {
		public RateDepositControl()
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

