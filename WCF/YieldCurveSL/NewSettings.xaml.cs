using System;
using System.Windows;
using System.Windows.Controls;


namespace YieldCurveSL
{
    public partial class NewSettings : ChildWindow
    {
        public NewSettings()
        {
            InitializeComponent();

            EnumConversion.InitializeComboFromEnum<compounding>(this.comboBox_ZCcomp);
            EnumConversion.InitializeComboFromEnum<basis>(this.comboBox_ZCbas);
            EnumConversion.InitializeComboFromEnum<compounding>(this.comboBox_FRWcomp);
            EnumConversion.InitializeComboFromEnum<basis>(this.comboBox_FRWbas);
            EnumConversion.InitializeComboFromEnum<frequency>(this.comboBox_ZCfre);
            EnumConversion.InitializeComboFromEnum<frequency>(this.comboBox_FRWfre);
            EnumConversion.InitializeComboFromEnum<termbase>(this.comboBox_FRWterm);

            if (CurrentElements.CurrentYCId != -1)
                SetControlsFromSettingsForYC(CurrentElements.CurrentYCId);

            this.textBox_decimal.Text = AppSettings.Decimals.ToString();
            this.colorPicker_curve1.Color = AppSettings.FirstCurveColor;
            this.colorPicker_curve2.Color = AppSettings.SecondCurveColor;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            bool ifOneCurveSettingsChanged = false;

            if (ifNumericString(textBoxTerm.Text))
                 MessageBox.Show("Term should be a numeric value");
            else
            {
                if (ifNumericString(this.textBox_decimal.Text))
                    MessageBox.Show("Decimals should be a numeric value");
                else
                {
                    this.DialogResult = true;
                    AppSettings.Decimals = Convert.ToInt32(this.textBox_decimal.Text);
                }

                this.DialogResult = true;

                if (CurrentElements.CurrentYCId != -1)
                    ifOneCurveSettingsChanged=SetYCSettingsFromGUI(CurrentElements.CurrentYCId);
                else
                    MessageBox.Show("no YcId is choosen");

                 AppSettings.FirstCurveColor = this.colorPicker_curve1.Color;
                 AppSettings.SecondCurveColor = this.colorPicker_curve2.Color;

                if (AppSettings.ActiveWindow == 1)
                {
                    if (ifOneCurveSettingsChanged)
                    {
                        int curve_id = CurrentElements.CurrentYCId;
                 //       Page1 p = this.Parent as Page1;

                                          
                //       bool res = p.DrawEntryCurveFromCache(curve_id, null); 
                    //    res = FillInputDataGridFromCache(curve_id, null); in case if settings were changed the data table is stil the same

             //           ObservableCollection<DateTime> dList = new ObservableCollection<DateTime>();

               //         foreach (DateTime d in CachedData.CommonDates)
               //             dList.Add(d);


               //         _service.CalculateDiscountedRateListAsync(YcSettingsDic.GetYcSett(curve_id).ycd, CurrentElements.CurrentDate, dList);
                    }

                }
                else
                {

                }

               
                
                
             }
        }

        private bool ifNumericString(string textboxText)
        {
            bool result = false;
            for (int i = 0; i < textboxText.Length; i++)
            {
                if (!char.IsNumber(textboxText[i]))
                    result = true;
            }

            return result;

        }

        public bool SetYCSettingsFromGUI(int YcId) //this function will return true if settings were changed otherwise false
        {
            bool ifChanged = false;
            YcSettings s = YcSettingsDic.GetYcSett(YcId);  // will return existing one (to overwrite here) 
            YcSettings s1 = new YcSettings();
            // or create new default one with Id = YcId
         //   s.ycd.ZcBasisId = -1;
         //   s.ycd.FrwBasisId = -1;
            foreach (var i in CachedData.CachedDayCounterDic.Values)
            {
                if (i.Name == (String)comboBox_ZCbas.SelectedItem)
                    s1.ycd.settings.ZcBasisId = i.Id;
                    
                if (i.Name == (String)comboBox_FRWbas.SelectedItem)
                    s1.ycd.settings.FrwBasisId = i.Id;
            }


            s1.ycd.settings.ZcCompounding = (String)comboBox_ZCcomp.SelectedItem;
            //s.ycd.ZcBasisId = CachedData.CachedDayCounterDic[(String)comboBox2.SelectedItem];
            s1.ycd.settings.ZcFrequency = (String)comboBox_ZCfre.SelectedItem;
            s1.ycd.settings.FrwCompounding = (String)comboBox_FRWcomp.SelectedItem;
            //s.ycd.FrwBasisId = CachedData.CachedDayCounterDic[(String)comboBox4.SelectedItem];
            s1.ycd.settings.FrwFrequency = (String)comboBox_FRWfre.SelectedItem;
            s1.ycd.settings.FrwTerm = int.Parse(textBoxTerm.Text);
            s1.ycd.settings.FrwTermBase = (String)comboBox_FRWterm.SelectedItem;
            s1.ifZCCurve = checkBox1.IsChecked.Value;
            s1.ifForwardCurve = checkBox2.IsChecked.Value;
            s1.ZCColor = this.colorPicker_ZC.Color;
            s1.FrwColor = this.colorPicker_FRW.Color;
            s1.ycd.settings.ZCColor = this.colorPicker_ZC.Color.ToString();
            s1.ycd.settings.FrwColor = this.colorPicker_FRW.Color.ToString();
           


            if ((s1.ycd.settings.ZcBasisId != s.ycd.settings.ZcBasisId) || (s1.ycd.settings.FrwBasisId != s.ycd.settings.FrwBasisId) ||
                (s1.ycd.settings.ZcCompounding != s.ycd.settings.ZcCompounding) || (s1.ycd.settings.ZcBasisId != s.ycd.settings.ZcBasisId) ||
                (s1.ycd.settings.ZcFrequency != s.ycd.settings.ZcFrequency) || (s1.ycd.settings.FrwCompounding != s.ycd.settings.FrwCompounding) ||
                (s1.ycd.settings.FrwBasisId != s.ycd.settings.FrwBasisId) || (s1.ycd.settings.FrwFrequency != s.ycd.settings.FrwFrequency) ||
                (s1.ycd.settings.FrwTerm != s.ycd.settings.FrwTerm) || (s1.ycd.settings.FrwTermBase != s.ycd.settings.FrwTermBase) ||  
                (s1.ifZCCurve!=s.ifZCCurve)||(s1.ifForwardCurve!=s.ifForwardCurve) ||
                (s1.ZCColor!=s.ZCColor) ||(s1.FrwColor!=s.FrwColor))
            {
                ifChanged = true;
                s = s1;
                s1.ycd.Id = CurrentElements.CurrentYCId;
                YcSettingsDic.SetYcSett(s1.ycd);
            }

            return ifChanged;
        }


        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_ZCcomp.SelectedItem.ToString() == compounding.Compounded.ToString())
            {
                comboBox_ZCfre.IsEnabled = true;
                //     comboBox5.SelectedItem = frequency.Once;
            }
            else
            {
                comboBox_ZCfre.IsEnabled = false;
                comboBox_ZCfre.SelectedItem = frequency.Once;
            }
        }

        private void comboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_FRWcomp.SelectedItem.ToString() == compounding.Compounded.ToString())
            {
                comboBox_FRWfre.IsEnabled = true;
                //     comboBox5.SelectedItem = frequency.Once;
            }
            else
            {
                comboBox_FRWfre.IsEnabled = false;
                comboBox_FRWfre.SelectedItem = frequency.Once;
            }
        }

        private void colorPicker1_ColorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (colorPicker_ZC != null)
            {
                // MessageBox.Show(colorPicker1.Color.ToString());
                // (ellipse00.Fill as SolidColorBrush).Color = colorPicker1.Color;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            SetControlsFromSettingsForYC(CurrentElements.CurrentYCId);
        }

        // this is a method to display GUI from SettingsList for corresponding YC
        public void SetControlsFromSettingsForYC(int YcId)
        {
            YcSettings s = YcSettingsDic.GetYcSett(CurrentElements.CurrentYCId);

            this.Title = "Settings: " + s.ycd.Name;
            this.checkBox1.IsChecked = s.ifZCCurve;
            this.checkBox2.IsChecked = s.ifForwardCurve;
            this.comboBox_ZCcomp.SelectedItem = s.ycd.settings.ZcCompounding;
            this.comboBox_ZCbas.SelectedItem = CachedData.CachedDayCounterDic[s.ycd.settings.ZcBasisId].Name;
            this.comboBox_ZCfre.SelectedItem = s.ycd.settings.ZcFrequency;
            this.comboBox_FRWcomp.SelectedItem = s.ycd.settings.FrwCompounding;
            this.comboBox_FRWbas.SelectedItem = CachedData.CachedDayCounterDic[s.ycd.settings.FrwBasisId].Name;
            this.comboBox_FRWfre.SelectedItem = s.ycd.settings.FrwFrequency;
            this.comboBox_FRWterm.SelectedItem = s.ycd.settings.FrwTermBase;
            this.textBoxTerm.Text = s.ycd.settings.FrwTerm.ToString();
            this.colorPicker_ZC.Color = s.ZCColor;
            this.colorPicker_FRW.Color = s.FrwColor;
        }

    }
}
