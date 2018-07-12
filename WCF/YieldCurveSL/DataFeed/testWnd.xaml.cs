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
using System.Collections.ObjectModel;
using YieldCurveSL.YieldCurveSrv;
using System.Windows.Data;
using System.Windows.Markup;

using Lite.ExcelLibrary.CompoundDocumentFormat;
using Lite.ExcelLibrary.BinaryFileFormat;
using Lite.ExcelLibrary.SpreadSheet;
using System.IO;
using System.Reflection;

namespace YieldCurveSL
{
	public partial class CTestWnd : ChildWindow
	{
		ObservableCollection<Rate> rs = new ObservableCollection<Rate>();
		ObservableCollection<Rate> rd = new ObservableCollection<Rate>();

		public CTestWnd()
		{
			InitializeComponent();

			EnumConversion.InitializeComboFromEnum<ratetype>(this.comboBox1);

			foreach (Rate i in CachedData.CachedRatesDic.Values)
			{
				if (i.Type == ratetype.deposit.ToString())
					rd.Add(i);
				else if (i.Type == ratetype.swap.ToString())
					rs.Add(i);
			}

			this.comboBox1.SelectedIndex = 0; // rate deposit
			this.rateDepositControl1.Visibility = System.Windows.Visibility.Visible;

			this.rateSwapControl1.Visibility = System.Windows.Visibility.Collapsed;
			this.bondControl1.Visibility = System.Windows.Visibility.Collapsed;
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}
		
		private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems[0].ToString() == ratetype.deposit.ToString())
			{
				this.bondControl1.Visibility = Visibility.Collapsed;
				this.rateSwapControl1.Visibility = Visibility.Collapsed;

				this.listBox1.ItemsSource = rd;
				this.rateDepositControl1.Visibility = Visibility.Visible;
			}
			if (e.AddedItems[0].ToString() == ratetype.swap.ToString())
			{
				this.bondControl1.Visibility = Visibility.Collapsed;
				this.rateDepositControl1.Visibility = Visibility.Collapsed;

				this.listBox1.ItemsSource = rs;
				this.rateSwapControl1.Visibility = Visibility.Visible;
			}
			if (e.AddedItems[0].ToString() == ratetype.bond.ToString())
			{   
				this.rateSwapControl1.Visibility = Visibility.Collapsed;
				this.rateDepositControl1.Visibility = Visibility.Collapsed;

				this.listBox1.ItemsSource = CachedData.CachedBondsDic.Values;
				this.bondControl1.Visibility = Visibility.Visible;
			}
		}

		//
		//
		//

		private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 0)
				return;

			Rate r = e.AddedItems[0] as Rate;
			Bond b = e.AddedItems[0] as Bond;

			int instrumentId = 0;

			// deposit rate
			if (r != null
				&& this.comboBox1.SelectedItem.ToString() == ratetype.deposit.ToString()
				)
			{
				this.rateDepositControl1.Init(r);
				instrumentId = r.Id;
			}
			// swap rate
			else if (r != null
				&& this.comboBox1.SelectedItem.ToString() == ratetype.swap.ToString()
				)
			{
				this.rateSwapControl1.Init(r);
				instrumentId = r.Id;
			}
			else if (b != null
				&& this.comboBox1.SelectedItem.ToString() == ratetype.bond.ToString()
				)
			{
				this.bondControl1.Init(b);
				instrumentId = b.Id;
			}

			// data grid - entry rates history - actually here we assume 1-to-1 relationship between YcEntryId and RateId (or BondId), e.g.
			// type (rate, bond or xh-rate) + rateId (or bondId or xhRateId) = YcEntryId
			// just to remind that YcEntryId is unique in the YieldCurveEntryDataHistory

			if (instrumentId != 0)
				foreach (YieldCurveEntryDataHistory yceh in CachedData.CachedEntryDataHistoryList)
				{
					if (yceh.Instrument.Id == instrumentId)
					{
						this.dataGrid1.ItemsSource = yceh.EntryDataHistory;
						return;
					}
				}
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			// open file dialog to select an export file.   
			SaveFileDialog sDialog = new SaveFileDialog();
			sDialog.Filter = "Excel Files(*.xls)|*.xls";

			if (sDialog.ShowDialog() == true)
			{

				// create an instance of excel workbook
				Workbook workbook = new Workbook();
				// create a worksheet object
				Worksheet worksheet = new Worksheet(String.Format("{0}", (this.listBox1.SelectedItem as Instrument).Name));

				int ColumnCount = 0;
				int RowCount = 0;

				// Writing Column Names 
				foreach (DataGridColumn dgcol in this.dataGrid1.Columns)
				{
					worksheet.Cells[0, ColumnCount] = new Cell(dgcol.Header.ToString());
					ColumnCount++;
				}

				// Extracting values from grid and writing to excell sheet
				
				foreach (object data in this.dataGrid1.ItemsSource)
				{
					ColumnCount = 0;
					RowCount++;
					foreach (DataGridColumn col in this.dataGrid1.Columns)
					{
						string strValue = "";
						Binding objBinding = null;

						if (col is DataGridBoundColumn)
							objBinding = (col as DataGridBoundColumn).Binding;

						if (col is DataGridTemplateColumn)
						{
							//This is a template column... let us see the underlying dependency object
							DependencyObject objDO = (col as DataGridTemplateColumn).CellTemplate.LoadContent();
							FrameworkElement oFE = (FrameworkElement)objDO;
							FieldInfo oFI = oFE.GetType().GetField("TextProperty");
							if (oFI != null)
							{
								if (oFI.GetValue(null) != null)
								{
									if (oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)) != null)
										objBinding = oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)).ParentBinding;
								}
							}
						}

						if (objBinding != null)
						{
							if (objBinding.Path.Path != "")
							{
								PropertyInfo pi = data.GetType().GetProperty(objBinding.Path.Path);
								if (pi != null) strValue = Convert.ToString(pi.GetValue(data, null));
							}
							/*
							if (objBinding.Converter != null)
							{
								if (strValue != "")
									strValue = objBinding.Converter.Convert(
										strValue, 
										typeof(string), objBinding.ConverterParameter, 
										objBinding.ConverterCulture).ToString();
								//else
								//    strValue = objBinding.Converter.Convert(data, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
							}
							*/
						}
						// writing extracted value in excell cell
						worksheet.Cells[RowCount, ColumnCount] = new Cell(strValue);
						ColumnCount++;
					}
				}

				//add worksheet to workbook
				workbook.Worksheets.Add(worksheet);
				// get the selected file's stream
				Stream sFile = sDialog.OpenFile();
				workbook.Save(sFile);
			}
		}

		private void button2_Click(object sender, RoutedEventArgs e)
		{
			Instrument instr = this.listBox1.SelectedItem as Instrument;
			if (instr == null)
				return;
			
			//To access file system we need to use Silverlight file dialogs
			OpenFileDialog oFile = new OpenFileDialog();
			// .xls filter specified to select only .xls file.
			oFile.Filter = "Excel (*.xls)|*.xls";

			if (oFile.ShowDialog() == true)
			{
				// Get the stream of the selected file
				FileStream fs = oFile.File.OpenRead();
				// Simply call the Open method of Workbook and you are done
				Workbook book = Workbook.Open(fs);
				// All of the worksheet will be populated with data 
				// currently we will read only first for this sample
				Worksheet sheet = book.Worksheets[0];

				/// itrating through worksheet object to get values
				/// Worksheet.Cells.FirstRowIndex tells the First row index of data
				/// Worksheet.Cells.LastRowIndex tells the last of data
				/// Worksheet.Cells.FirstColIndex has first index of column value
				/// Worksheet.Cells.LastColIndex has last index of column
				/// So itrating using these properties will traverse all data of the sheet
				/// 

				ObservableCollection<ValuePair> yceh = new ObservableCollection<ValuePair>();

				for (int i = sheet.Cells.FirstRowIndex + 1; i < sheet.Cells.LastRowIndex; i++)	// +1 cos of Header in Excel
				{
					if (String.IsNullOrEmpty(sheet.Cells[i, sheet.Cells.FirstColIndex].StringValue)
						&& String.IsNullOrEmpty(sheet.Cells[i, sheet.Cells.FirstColIndex + 1].StringValue)
						)
						continue;

					DateTime d;
					double v;
					if (!DateTime.TryParse(sheet.Cells[i, sheet.Cells.FirstColIndex].StringValue, out d))
					{
						MessageBox.Show(String.Format("Line {0} has incorrect date format (DateTime C# compatable format expected)", i));
						return;
					}
					if (!Double.TryParse(sheet.Cells[i, sheet.Cells.FirstColIndex + 1].StringValue, out v))
					{
						MessageBox.Show(String.Format("Line {0} has incorrect value format (Double C# compatable format expected", i));
						return;
					}
					yceh.Add(new ValuePair 
					{
						Date = d, //Convert.ToDateTime(sheet.Cells[i, sheet.Cells.FirstColIndex].StringValue),
						Value = v //Convert.ToDouble(sheet.Cells[i, sheet.Cells.FirstColIndex + 1].StringValue)
					});
				}

				if (instr.Id != 0)
					foreach (YieldCurveEntryDataHistory i in CachedData.CachedEntryDataHistoryList)
					{
						if (i.Instrument.Id == instr.Id)
						{
							i.EntryDataHistory = yceh;
							//i.EntryDataHistory.Sort();

							// new data into datagrid
							this.dataGrid1.ItemsSource = i.EntryDataHistory;
							return;
						}
					}
			}
		}
	}
}

