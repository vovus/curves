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
    public partial class DecimalsGUI : ChildWindow
    {
        public DecimalsGUI()
        {
            InitializeComponent();
            this.textBox1.Text = AppSettings.Decimals.ToString();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ifNumericString(textBox1.Text))
            {
                MessageBox.Show("Decimals should be a numeric value");
            }
            else
            {
                this.DialogResult = true;
                AppSettings.Decimals = Convert.ToInt32(this.textBox1.Text);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
          
            this.DialogResult = false;
            this.textBox1.Text = AppSettings.Decimals.ToString();
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

        


    }
}
