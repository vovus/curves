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

using YieldCurveSL.YieldCurveSrv;


namespace YieldCurveSL
{
    public partial class DiscountSettingsGUI : ChildWindow
    {
        public DiscountSettingsGUI()
        {
            InitializeComponent();

            EnumConversion.InitializeComboFromEnum<compounding>(this.comboBox1);
            EnumConversion.InitializeComboFromEnum<basis>(this.comboBox2);
            EnumConversion.InitializeComboFromEnum<compounding>(this.comboBox3);
            EnumConversion.InitializeComboFromEnum<basis>(this.comboBox4);
            EnumConversion.InitializeComboFromEnum<frequency>(this.comboBox5);
            EnumConversion.InitializeComboFromEnum<frequency>(this.comboBox6);
            EnumConversion.InitializeComboFromEnum<termbase>(this.comboBox7);

            if (CurrentElements.CurrentYCId != -1)
                SetControlsFromSettingsForYC(CurrentElements.CurrentYCId);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ifNumericString(textBox1.Text))
            {
                MessageBox.Show("Term should be a numeric value");
            }
            else
            {
                this.DialogResult = true;
                
                if (CurrentElements.CurrentYCId != -1)
                {
                    SetYCSettingsFromGUI(CurrentElements.CurrentYCId);
                }
                else
                    MessageBox.Show("no YcId is chosen");
            }
        }

        private bool ifNumericString(string textboxText)
        {
            bool result = false;
            for (int i = 0; i < textboxText.Length; i++)
            {
                if (!char.IsNumber(textboxText[i]))
                {
                    result = true;
                }
            }

            return result;

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
			this.comboBox1.SelectedItem = s.ycd.ZcCompounding;
			this.comboBox2.SelectedItem = CachedData.CachedDayCounterDic[s.ycd.ZcBasisId].Name;
			this.comboBox5.SelectedItem = s.ycd.ZcFrequency;
			this.comboBox3.SelectedItem = s.ycd.FrwCompounding;
			this.comboBox4.SelectedItem = CachedData.CachedDayCounterDic[s.ycd.FrwBasisId].Name;
			this.comboBox6.SelectedItem = s.ycd.FrwFrequency;
			this.comboBox7.SelectedItem = s.ycd.FrwTermBase;
			this.textBox1.Text = s.ycd.FrwTerm.ToString();
			this.colorPicker1.Color = s.ZCColor;
			this.colorPicker2.Color = s.FrwColor;
        }

        public void SetYCSettingsFromGUI(int YcId)
        {
			YcSettings s = YcSettingsDic.GetYcSett(YcId);  // will return existing one (to overwrite here) 
                                                                // or create new default one with Id = YcId
			s.ycd.ZcBasisId = -1;
			s.ycd.FrwBasisId = -1;
			foreach (var i in CachedData.CachedDayCounterDic.Values)
			{
				if (i.Name == (String)comboBox2.SelectedItem)
				{
					s.ycd.ZcBasisId = i.Id;
				}
				if (i.Name == (String)comboBox4.SelectedItem)
				{
					s.ycd.FrwBasisId = i.Id;
				}
			}

            s.ycd.ZcCompounding = (String)comboBox1.SelectedItem;
			//s.ycd.ZcBasisId = CachedData.CachedDayCounterDic[(String)comboBox2.SelectedItem];
			s.ycd.ZcFrequency = (String)comboBox5.SelectedItem;
			s.ycd.FrwCompounding = (String)comboBox3.SelectedItem;
			//s.ycd.FrwBasisId = CachedData.CachedDayCounterDic[(String)comboBox4.SelectedItem];
			s.ycd.FrwFrequency = (String)comboBox6.SelectedItem;
			s.ycd.FrwTerm = int.Parse(textBox1.Text);
			s.ycd.FrwTermBase = (String)comboBox7.SelectedItem;
            s.ifZCCurve = checkBox1.IsChecked.Value;
            s.ifForwardCurve = checkBox2.IsChecked.Value;
			s.ZCColor = this.colorPicker1.Color;
			s.FrwColor = this.colorPicker2.Color;

			//YcSettingsDic.SetYcSett(CurrentElements.CurrentYCId, s);
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == compounding.Compounded.ToString())
            {
                comboBox5.IsEnabled = true;
           //     comboBox5.SelectedItem = frequency.Once;
            }
            else
            {
                comboBox5.IsEnabled = false;
                comboBox5.SelectedItem = frequency.Once;
            }
        }

        private void comboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == compounding.Compounded.ToString())
            {
                comboBox6.IsEnabled = true;
                //     comboBox5.SelectedItem = frequency.Once;
            }
            else
            {
                comboBox6.IsEnabled = false;
                comboBox6.SelectedItem = frequency.Once;
            }
        }

		private void colorPicker1_ColorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (colorPicker1 != null)
			{
				// MessageBox.Show(colorPicker1.Color.ToString());
				// (ellipse00.Fill as SolidColorBrush).Color = colorPicker1.Color;
			}
		}
    }
}

