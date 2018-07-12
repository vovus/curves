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

namespace YieldCurveSL
{
    public partial class InflationRateGUI : ChildWindow
    {
        public InflationRateGUI()
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

      //      EnumConversion.InitializeComboFromEnum<basis>(this.comboIBas);
      //      EnumConversion.InitializeComboFromEnum<compounding>(this.comboIMod);
            EnumConversion.InitializeComboFromEnum<frequency>(this.comboIFre);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

