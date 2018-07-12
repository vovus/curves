using System;
using System.Windows.Controls;

using YieldCurveSL.YieldCurveSrv;

namespace YieldCurveSL
{
    public partial class BondControl : UserControl
    {
		public BondControl()
        {
            InitializeComponent();
            EnumConversion.InitializeComboFromEnum<bondtype>(this.comboTyp);
            CachedData.InitializeCurrencyComboFromCache(this.comboCur);
            EnumConversion.InitializeComboFromEnum<frequency>(this.comboFre);
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

		public void Init(Bond b)
		{
			String CurCode = (CachedData.CachedCurrencyDic.ContainsKey(b.IdCcy)
								? CachedData.CachedCurrencyDic[b.IdCcy].Code
								: "");

			this.comboCur.SelectedItem = CurCode;
			this.txtBoxRef.Text = b.Name;
			this.comboFre.SelectedItem = EnumConversion.getStringFromEnum(EnumConversion.getFrequencyFromString(b.CouponFrequency));
			this.comboTyp.SelectedItem = EnumConversion.getStringFromEnum(bondtype.Fixed);

			this.txtBoxNom.Text = b.Nominal.ToString();
			this.txtBoxRed.Text = b.Redemption.ToString();
			this.txtBoxCoupon.Text = b.Coupon.ToString();
			this.datePickerIssue.SelectedDate = b.IssueDate;
			this.datePickerMaturity.SelectedDate = b.MaturityDate;
		}
    }
}
