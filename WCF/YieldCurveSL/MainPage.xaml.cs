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
using System.Windows.Controls.DataVisualization.Charting;
using YieldCurveSL.ServiceReference1;

namespace YieldCurveSL
{
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			InitializeComponent();
		}

		void proxy_GetYieldCurveDataListCompleted(object sender, GetYieldCurveDataListCompletedEventArgs e)
		{
			KeyValuePair<string, double>[] tmp = new KeyValuePair<string,double>[e.Result.Count()];

			for (int i = 0; i < e.Result.Count(); i++)
			{
				TermRateData trd = e.Result[i];
				tmp[i] = new KeyValuePair<string, double>(trd.term, trd.rate);
			}
			
			((LineSeries)this.chart1.Series[0]).ItemsSource = tmp;				
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			YieldCurveSrvSoapClient proxy = new YieldCurveSrvSoapClient();

			proxy.GetYieldCurveDataListCompleted +=
				new EventHandler<GetYieldCurveDataListCompletedEventArgs>(proxy_GetYieldCurveDataListCompleted);

			//proxy.GetYieldCurveDataListAsync(10000);	//TODO pass proper idYc here
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		
		private void button2_Click(object sender, RoutedEventArgs e)
		{
			DateTime d1 = this.datePicker1.SelectedDate.Value;
			DateTime d2 = this.datePicker2.SelectedDate.Value;
			
			YieldCurveSrvSoapClient proxy = new YieldCurveSrvSoapClient();

			proxy.CalculateDiscountedRateCompleted +=
				new EventHandler<CalculateDiscountedRateCompletedEventArgs>(proxy_CalculateDiscountedRateCompleted);

			proxy.CalculateDiscountedRateAsync(d1, d2, 10000);	//TODO pass proper idYc here
			
		}
		
		void proxy_CalculateDiscountedRateCompleted(object sender, CalculateDiscountedRateCompletedEventArgs e)
		{
			this.label3.Content += e.Result.rate.ToString();
			this.label4.Content += e.Result.discount.ToString(); 
		}
	}
}
