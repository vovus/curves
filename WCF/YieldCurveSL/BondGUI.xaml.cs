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
    public partial class BondGUI : ChildWindow
    {
        public BondGUI()
        {
            InitializeComponent();
            EnumConversion.InitializeComboFromEnum<bondtype>(this.comboTyp);
            CachedData.InitializeCurrencyComboFromCache(this.comboCur);
            EnumConversion.InitializeComboFromEnum<frequency>(this.comboFre);
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
