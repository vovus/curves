using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;

#if false // not used here

namespace DataLayer
{
	//[DataContract]
	public class Basis
	{
		public enum eBasis
		{
			None = -1,
			Actual360,
			Actual365Fixed,
			ActualActual,
			Business252,
			Thirty360
		}
		//[DataMember]
		public eBasis _basis;

		public Basis() { _basis = eBasis.None; }
		public Basis(eBasis b) { _basis = b; }
		public Basis(string s)
		{
			switch (s)
			{
				case "Actual/360":
					_basis = eBasis.Actual360;
					break;
				case "Actual/365 (Fixed)":
					_basis = eBasis.Actual365Fixed;
					break;
				case "Actual/Actual (ISDA)":
					_basis = eBasis.ActualActual;
					break;
				case "Business/252":
					_basis = eBasis.Business252;
					break;
				case "30/360 (Bond Basis)":
					_basis = eBasis.Thirty360;
					break;
				default:
					_basis = eBasis.None;
					break;
			}
		}

		public override string ToString()
		{
			//return _basis.ToString();
			String Result = "";
			switch (_basis)
			{
				case eBasis.Actual360:
					Result = "Actual/360";
					break;
				case eBasis.Actual365Fixed:
					Result = "Actual/365 (Fixed)";
					break;
				case eBasis.ActualActual:
					Result = "Actual/Actual (ISDA)";
					break;
				case eBasis.Business252:
					Result = "Business/252";
					break;
				case eBasis.Thirty360:
					Result = "30/360 (Bond Basis)";
					break;
				default:
					throw new NotImplementedException();
			}
			return Result;
		}
		*/
	}

	//[DataContract]
	public class Compounding
	{
		public enum eCompounding
		{
			None = -1,
			Simple,
			Compounded,
			Continuous
		}
		//[DataMember]
		public eCompounding _compounding;

		public Compounding() { _compounding = eCompounding.None; }
		public Compounding(eCompounding c) { _compounding = c; }
		public Compounding(string s)
		{
			try
			{
				_compounding = (eCompounding)Enum.Parse(typeof(eCompounding), s, true);
			}
			catch
			{
				_compounding = eCompounding.None;
			}
		}

		public override string ToString()
		{
			return _compounding.ToString();
		}
	}

	//[DataContract]
	public class Frequency
	{
		public enum eFrequency
		{
			None = -1,
			Once,
			Annual,
			Semiannual,
			Quarterly,
			EveryFourthMonth,
			Bimonthly,
			Monthly,
			EveryFourthWeek,
			Biweekly,
			Weekly,
			Daily
		}
		//[DataMember]
		public eFrequency _frequency;

		public Frequency() { _frequency = eFrequency.None; }
		public Frequency(eFrequency f) { _frequency = f; }
		public Frequency(string s)
		{
			switch (s)
			{
				case "Every-Fourth-Month":
					_frequency = eFrequency.EveryFourthMonth;
					break;
				case "Every-fourth-week":
					_frequency = eFrequency.EveryFourthWeek;
					break;
				case "Once":
				case "Annual":
				case "Semiannual":
				case "Quarterly":
				case "Bimonthly":
				case "Monthly":
				case "Biweekly":
				case "Weekly":
				case "Daily":
					try
					{
						_frequency = (eFrequency)Enum.Parse(typeof(eFrequency), s, true);
					}
					catch
					{
						_frequency = eFrequency.None;
					}
					break;
				default:
					_frequency = eFrequency.None;
					break;
			}
		}

		public Frequency(int l, TermBase t)
		{
			eFrequency result = eFrequency.Annual;
			switch (t._termbase)
			{
				case TermBase.eTermBase.Days:
					result = eFrequency.Daily;
					break;
				case TermBase.eTermBase.Months:
					switch (l)
					{
						case 1:
							result = eFrequency.Monthly;
							break;
						case 2:
							result = eFrequency.Bimonthly;
							break;
						case 3:
							result = eFrequency.Quarterly;
							break;
						case 4:
							result = eFrequency.EveryFourthMonth;
							break;
						case 6:
							result = eFrequency.Semiannual;
							break;
						default:
							result = eFrequency.Semiannual;
							break;
					}
					break;
				case TermBase.eTermBase.Weeks:
					switch (l)
					{
						case 2:
							result = eFrequency.Biweekly;
							break;
						case 4:
							result = eFrequency.EveryFourthWeek;
							break;
						case 1:
							result = eFrequency.Weekly;
							break;
						default:
							result = eFrequency.Weekly;
							break;
					}
					break;
				case TermBase.eTermBase.Years:
					result = eFrequency.Annual;
					break;
				default:
					result = eFrequency.Once;
					break;
			}
			_frequency = result;
		}

		public override string ToString()
		{
			// return _frequency.ToString();
			string Result = "";
			switch (_frequency)
			{
				case eFrequency.Once:
					Result = "Once";
					break;
				case eFrequency.Annual:
					Result = "Annual";
					break;
				case eFrequency.Semiannual:
					Result = "Semiannual";
					break;
				case eFrequency.EveryFourthMonth:
					Result = "Every-Fourth-Month";
					break;
				case eFrequency.Quarterly:
					Result = "Quarterly";
					break;
				case eFrequency.Bimonthly:
					Result = "Bimonthly";
					break;
				case eFrequency.Monthly:
					Result = "Monthly";
					break;
				case eFrequency.EveryFourthWeek:
					Result = "Every-fourth-week";
					break;
				case eFrequency.Biweekly:
					Result = "Biweekly";
					break;
				case eFrequency.Weekly:
					Result = "Weekly";
					break;
				case eFrequency.Daily:
					Result = "Daily";
					break;
				default:
					throw new NotImplementedException();
			}
			return Result;
		}
	}

	//[DataContract]
	public class TermBase
	{
		public enum eTermBase
		{
			None = -1,
			Days,
			Weeks,
			Months,
			Years
		}
		//[DataMember]
		public eTermBase _termbase;

		public TermBase() { _termbase = eTermBase.None; }
		public TermBase(eTermBase t) { _termbase = t; }
		public TermBase(string s)
		{
			try
			{
				_termbase = (eTermBase)Enum.Parse(typeof(eTermBase), s, true);
			}
			catch
			{
				_termbase = eTermBase.None;
			}
		}

		public override string ToString()
		{
			return _termbase.ToString();
		}
	}

	//[DataContract]
	public class RateType
	{
		public enum eRateType
		{
			None = -1,
			deposit,
			swap,
			bond
		}
		//[DataMember]
		public eRateType _ratetype;

		public RateType() { _ratetype = eRateType.None; }
		public RateType(eRateType r) { _ratetype = r; }
		public RateType(string s)
		{
			try
			{
				_ratetype = (eRateType)Enum.Parse(typeof(eRateType), s, true);
			}
			catch
			{
				_ratetype = eRateType.None;
			}
		}

		public override string ToString()
		{
			return _ratetype.ToString();
		}
	}

	//[DataContract]
	public class BondType
	{
		public enum eBondType
		{
			None = -1,
			Fixed,
			Floating
		}
		//[DataMember]
		public eBondType _bondtype;

		public BondType() { _bondtype = eBondType.None; }
		public BondType(eBondType b) { _bondtype = b; }
		public BondType(string s)
		{
			try
			{
				_bondtype = (eBondType)Enum.Parse(typeof(eBondType), s, true);
			}
			catch
			{
				_bondtype = eBondType.None;
			}
		}

		public override string ToString()
		{
			return _bondtype.ToString();
		}
	}

	//[DataContract]
	public class BusinessDayConvention
	{
		public enum eBusinessDayConvention
		{
			None = -1,
			Following,
			ModifiedFollowing,
			Preceding,
			ModifiedPreceding,
			Unadjusted
		}
		//[DataMember]
		public eBusinessDayConvention _bdc;

		public BusinessDayConvention() { _bdc = eBusinessDayConvention.None; }
		public BusinessDayConvention(eBusinessDayConvention b) { _bdc = b; }
		public BusinessDayConvention(string s)
		{
			switch (s)
			{
				case "Following":
				case "Preceding":
				case "Unadjusted":
					try
					{
						_bdc = (eBusinessDayConvention)Enum.Parse(typeof(eBusinessDayConvention), s, true);
					}
					catch
					{
						_bdc = eBusinessDayConvention.None;
					}
					break;
				case "Modified Following":
					_bdc = eBusinessDayConvention.ModifiedFollowing;
					break;
				case "Modified Preceding":
					_bdc = eBusinessDayConvention.ModifiedPreceding;
					break;
				default:
					_bdc = eBusinessDayConvention.None;
					break;
			}
		}

		public override string ToString()
		{
			// return _bdc.ToString();
			String Result = "";
			switch (_bdc)
			{
				case eBusinessDayConvention.Following:
					Result = "Following";
					break;
				case eBusinessDayConvention.ModifiedFollowing:
					Result = "Modified Following";
					break;
				case eBusinessDayConvention.Preceding:
					Result = "Preceding";
					break;
				case eBusinessDayConvention.ModifiedPreceding:
					Result = "Modified Preceding";
					break;
				case eBusinessDayConvention.Unadjusted:
					Result = "Unadjusted";
					break;
				default:
					throw new NotImplementedException();
			}
			return Result;
		}
	}
}

#endif // if false