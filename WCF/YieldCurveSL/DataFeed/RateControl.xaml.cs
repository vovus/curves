using System.Windows;
using System.Windows.Controls;

namespace YieldCurveSL
{
    public partial class RateControl : UserControl
    {
        public RateControl()
        {
            InitializeComponent();
            //initializeBasisCombo();
			//Dropdown.InitializeBasisCombo(this.comboBas);
            EnumConversion.InitializeComboFromEnum<basis>(this.comboBas);
			//Dropdown.InitializeComboFromEnum<basis>(this.comboBas);
            EnumConversion.InitializeComboFromEnum<compounding>(this.comboMod);
            EnumConversion.InitializeComboFromEnum<frequency>(this.comboFre);
            EnumConversion.InitializeComboFromEnum<ratetype>(this.comboTyp);
            EnumConversion.InitializeComboFromEnum<termbase>(this.comboTer);
            EnumConversion.InitializeComboFromEnum<BusinessDayConvention>(this.comboBus);
            CachedData.InitializeCurrencyComboFromCache(this.comboCur);

            EnumConversion.InitializeComboFromEnum<basis>(this.comboIBas);
            EnumConversion.InitializeComboFromEnum<compounding>(this.comboIMod);
            EnumConversion.InitializeComboFromEnum<frequency>(this.comboIFre);
		}

		public void Clear_RateGUI()
        {
            this.comboBus.SelectedItem = "";
        }

        public void makeDepositGUI()
        {
            comboFre.SelectedItem = frequency.Once.ToString();
            comboIBas.Visibility = Visibility.Collapsed;
            labelIBas.Visibility = Visibility.Collapsed;
            comboIFre.Visibility = Visibility.Collapsed;
            labelIFre.Visibility = Visibility.Collapsed;
            comboIMod.Visibility = Visibility.Collapsed;
            labelIMod.Visibility = Visibility.Collapsed;
            labelIndex.Visibility = Visibility.Collapsed;
            txtBoxIRef.Visibility = Visibility.Collapsed;
            labelIInd.Visibility = Visibility.Collapsed;
            borderIndex.Visibility = Visibility.Collapsed;

            labelFixed.Content = "";
            labelIndex.Content = "";
        }

        public void makeSwapGUI()
        {
            comboIBas.Visibility = Visibility.Visible;
            labelIBas.Visibility = Visibility.Visible;
            comboIFre.Visibility = Visibility.Visible;
            labelIFre.Visibility = Visibility.Visible;
            comboIMod.Visibility = Visibility.Visible;
            labelIMod.Visibility = Visibility.Visible;
            labelIndex.Visibility = Visibility.Visible;
            txtBoxIRef.Visibility = Visibility.Visible;
            labelIInd.Visibility = Visibility.Visible;
            borderIndex.Visibility = Visibility.Visible;

            labelFixed.Content = "Fixed Leg";
            labelIndex.Content = "Index Leg";
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

