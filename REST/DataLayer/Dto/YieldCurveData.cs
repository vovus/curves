using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
	//
	// descriptor for whole set of points of given YieldCurveData 
	//
	public class YieldCurveSetting : IComparable
	{
		public int ZcBasisId { get; set; }
		public string ZcCompounding { get; set; }
		public string ZcFrequency { get; set; }
		public bool ifZc { get; set; }
		public String ZCColor { get; set; }

		public int FrwBasisId { get; set; }
		public string FrwCompounding { get; set; }
		public string FrwFrequency { get; set; }
		public bool ifFrw { get; set; }
		public String FrwColor { get; set; }

		public int FrwTerm { get; set; }
		public string FrwTermBase { get; set; }  // "Days", "Months", "Years"

		public int SpreadType { get; set; }
		public double SpreadSize { get; set; }
		public int SpreadFamily { get; set; }
		public Calendar Calendar { get; set; }

		public object Clone()
		{
			YieldCurveSetting ycs = new YieldCurveSetting();
			ycs.ZcBasisId = this.ZcBasisId;
			ycs.ZcCompounding = this.ZcCompounding;
			ycs.ZcFrequency = this.ZcFrequency;
			ycs.ifZc = this.ifZc;
			ycs.ZCColor = this.ZCColor;
			ycs.FrwBasisId = this.FrwBasisId;
			ycs.FrwCompounding = this.FrwCompounding;
			ycs.FrwFrequency = this.FrwFrequency;
			ycs.ifFrw = this.ifFrw;
			ycs.FrwColor = this.FrwColor;
			ycs.FrwTerm = this.FrwTerm;
			ycs.FrwTermBase = this.FrwTermBase;
			ycs.SpreadType = this.SpreadType;
			ycs.SpreadSize = this.SpreadSize;
			ycs.SpreadFamily = this.SpreadFamily;
			ycs.Calendar = this.Calendar;
			return ycs;
		}

		int IComparable.CompareTo(object obj)
		{
			YieldCurveSetting ycs = obj as YieldCurveSetting;
			if (ycs != null)
			{
				if (
					(this.ZcBasisId == ycs.ZcBasisId) && (this.ZcCompounding == ycs.ZcCompounding) &&
					(this.ZcFrequency == ycs.ZcFrequency) && (this.ifZc == ycs.ifZc) &&
					(this.ZCColor == ycs.ZCColor) && (this.FrwBasisId == ycs.FrwBasisId) &&
					(this.FrwCompounding == ycs.FrwCompounding) && (this.FrwFrequency == ycs.FrwFrequency) &&
					(this.ifFrw == ycs.ifFrw) && (this.FrwColor == ycs.FrwColor) && (this.FrwTerm == ycs.FrwTerm) &&
					(this.FrwTermBase == ycs.FrwTermBase) && (this.SpreadType == ycs.SpreadType) &&
					(this.SpreadSize == ycs.SpreadSize) && (this.SpreadFamily == ycs.SpreadFamily) &&
					(this.Calendar == ycs.Calendar)
					)
					return 0;
				else
					return -1;
			}
			else
				throw new ArgumentException("Parameter is not a YieldCurveSetting!");
		}
	}

	public class YieldCurveData
	{
		public int Id;
		public string Name { get; set; }
		public string Family { get; set; }
		public int CurrencyId { get; set; }

		public YieldCurveSetting settings;

		public DateTime settlementDate = DateTime.Now;		// List bellow are all entry points/data are on/before this date (default is TODAY)
		// if settlementDate is null - all available dates will be taken

		// these are selected/calculated according to settlementDate
		public List<EntryPoint> entryPointList;
		public List<DiscountPoint> discountPointList;
		//

		public object Clone()
		{
			YieldCurveData ycs = new YieldCurveData();
			ycs.settings = (YieldCurveSetting)this.settings.Clone();

			return ycs;
		}
	}

	public class YieldCurveFamily
	{
		public int Id { get; set; }
		public int CurrencyId { get; set; }
		public string Name { get; set; }
	}

	public class DiscountPoint
	{
		public DateTime discountDate;
		public double discount;
		public double zcRate;
		public double fwdRate;
	}

	public class EntryPoint
	{
		public int Id { get; set; }					// id of the entry point
		public int YieldCurveId { get; set; }		// id of the yield curve for which entry point is set
		public bool Enabled { get; set; }			// entry rate can be disabled manually or automatically if it breaks callibration 
		public string Type { get; set; }            // type=deposit, swap or bond
		public int Length { get; set; }             // Rate only, not for bonds. Length + timeUnit = rate maturity
		public string TimeUnit { get; set; }        // Rate only, not for bonds. timeUnit = {"Days", "Weeks", "Months" ,"Years"}
		public Instrument Instrument { get; set; }	// each point is either the rate or the bond
		public int DataProviderId { get; set; }
		public string DataReference { get; set; }
		public DateTime ValidDateBegin { get; set; }  //if the yc definition has been changed it indicates from what date the entry rate is valid
		public DateTime ValidDateEnd { get; set; }

		public HistoricValue epValue = null;

		private int duration;
		public int Duration
		{
			get
			{
				if (duration == 0)
				{
					if (0 == String.Compare(Type, "bond", StringComparison.OrdinalIgnoreCase))
					{
						DateTime dt1 = DateTime.Today;
						DateTime dt2 = (Instrument as Bond).MaturityDate;
						TimeSpan days = dt2.Subtract(dt1);
						duration = days.Days;
					}
					else
					{
						if (0 == String.Compare(TimeUnit, "Days", StringComparison.OrdinalIgnoreCase))
						{
							duration = Length;
						}
						else if (0 == String.Compare(TimeUnit, "Weeks", StringComparison.OrdinalIgnoreCase))
						{
							duration = Length * 7;
						}
						else if (0 == String.Compare(TimeUnit, "Months", StringComparison.OrdinalIgnoreCase))
						{
							DateTime dt1 = DateTime.Today;
							DateTime dt2 = dt1.AddMonths(Length);
							TimeSpan days = dt2.Subtract(dt1);
							duration = days.Days;
							//duration = Length * 30;
						}
						else if (0 == String.Compare(TimeUnit, "Years", StringComparison.OrdinalIgnoreCase))
						{
							DateTime dt1 = DateTime.Today;
							DateTime dt2 = dt1.AddYears(Length);
							TimeSpan days = dt2.Subtract(dt1);
							duration = days.Days;
							//  duration = Length * 365;
						}
					}
				}
				return duration;
			}

			set { duration = Duration; }
		}
	}

	public class EntryPointCompare : System.Collections.Generic.IComparer<EntryPoint>
	{
		public int Compare(EntryPoint p1, EntryPoint p2)
		{
			return p1.Duration - p2.Duration;
		}
	}

	//
	// the same but with whole History
	//

	public class HistoricValue
	{
		public DateTime Date { get; set; }
		public double Value { get; set; }
	}

	public class HistoricValueComparer : IEqualityComparer<HistoricValue>
	{
		public bool Equals(HistoricValue b1, HistoricValue b2)
		{
			return DateTime.Equals(b1.Date, b2.Date);
		}
		public int GetHashCode(HistoricValue item)
		{
			return item.Date.GetHashCode();
		}
	}

	public static class Extensions		// used by ToHashSet() in linq
	{
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
		{
			return new HashSet<T>(source);
		}
	}

	public class EntryPointHistory : EntryPoint
	{
		public HashSet<HistoricValue> epValueHistory;
	}

	public class EntryPointHistoryCompare : System.Collections.Generic.IComparer<EntryPointHistory>
	{
		public int Compare(EntryPointHistory p1, EntryPointHistory p2)
		{
			return p1.Duration - p2.Duration;
		}
	}

#if FALSE
	public class DayCounter
	{
		public int Id;
		public string Name;			// user friendly name, used in GUI, gotten from QuantLib's name() method of the class
		public string ClassName;	// used by ObjectFactory, shell be the same as in QuantLib 

        public DayCounter(string cn, string n) { Id = -1; ClassName = cn; Name = n; }
        public DayCounter() { Id = -1; ClassName = string.Empty; Name = string.Empty; }
	}

	//
	// the single point on the plot
	//

	public class HistoricValue : IComparable<HistoricValue>
	{
		public DateTime Date { get; set; }
		public double Value { get; set; }

		int IComparable<HistoricValue>.CompareTo(HistoricValue other)
		{
			if (other.Date > Date) // this date is less the other
				return -1;
			else if (other.Date == Date)
				return 0;
			else
				return 1;
		}
	}
	
	public class RateDiscount
	{
		public DateTime discountDate;
		public double discount;
		public double zcRate;
		public double fwdRate;
	}

    /// <summary>
    /// 
    /// </summary>

    public class YieldCurveFamily
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public string Name { get; set; }
    }

	//
	// descriptor for whole set of points of given YieldCurveData 
	//
    public class YieldCurveSetting : ICloneable, IComparable
    {
        //public DayCounter ZcBasis { get; set; }           
        public int ZcBasisId { get; set; }            
        public string ZcCompounding { get; set; }       
        public string ZcFrequency { get; set; }       
        public bool ifZc { get; set; }
     //   public Color ZCColor { get; set; }
        public String ZCColor { get; set; }

        //public DayCounter FrwBasis { get; set; }           
        public int FrwBasisId { get; set; }          
        public string FrwCompounding { get; set; }       
        public string FrwFrequency { get; set; }       
        public bool ifFrw { get; set; }
      //  public Color FrwColor { get; set; }
        public String FrwColor { get; set; }

        public int FrwTerm { get; set; }
        public string FrwTermBase { get; set; }  // "Days", "Months", "Years"

        public int SpreadType { get; set; }
        public double SpreadSize { get; set; }
        public int SpreadFamily { get; set; }
        public Calendar Calendar { get; set; }	

        public object Clone()
        {
            YieldCurveSetting ycs = new YieldCurveSetting();
            ycs.ZcBasisId = this.ZcBasisId;
            ycs.ZcCompounding = this.ZcCompounding;
            ycs.ZcFrequency = this.ZcFrequency;
            ycs.ifZc = this.ifZc;
            ycs.ZCColor = this.ZCColor;
            ycs.FrwBasisId = this.FrwBasisId;
            ycs.FrwCompounding = this.FrwCompounding;
            ycs.FrwFrequency = this.FrwFrequency;
            ycs.ifFrw = this.ifFrw;
            ycs.FrwColor = this.FrwColor;
            ycs.FrwTerm = this.FrwTerm;
            ycs.FrwTermBase = this.FrwTermBase;
            ycs.SpreadType = this.SpreadType;
            ycs.SpreadSize = this.SpreadSize;
            ycs.SpreadFamily = this.SpreadFamily;
            ycs.Calendar = this.Calendar;
            return ycs;
        }

        int IComparable.CompareTo(object obj)
        {
            YieldCurveSetting ycs = obj as YieldCurveSetting;
            if (ycs != null)
            {
                if (
                    (this.ZcBasisId==	ycs.ZcBasisId) && (this.ZcCompounding== ycs.ZcCompounding) &&       
                    (this.ZcFrequency== ycs.ZcFrequency) && (this.ifZc==ycs.ifZc) &&
                    (this.ZCColor==	ycs.ZCColor) && (this.FrwBasisId== ycs.FrwBasisId) &&  
                    (this.FrwCompounding==ycs.FrwCompounding) && (this.FrwFrequency== ycs.FrwFrequency) &&       
                    (this.ifFrw==ycs.ifFrw) && (this.FrwColor==	ycs.FrwColor) && (this.FrwTerm==ycs.FrwTerm) &&
                    (this.FrwTermBase== ycs.FrwTermBase) && (this.SpreadType==ycs.SpreadType) &&
                    (this.SpreadSize==	ycs.SpreadSize) && (this.SpreadFamily==	ycs.SpreadFamily) &&
                    (this.Calendar == ycs.Calendar)
                    )
                    return 0;
                else
                    return -1;
            }
            else
                throw new ArgumentException("Parameter is not a YieldCurveSetting!");
        }
    }

    public class YieldCurveData : ICloneable
	{
		public int Id;
		public string Name { get; set; }
        public string Family { get; set; }
        public int CurrencyId { get; set; }

        public YieldCurveSetting settings;

		public DateTime settlementDate = DateTime.Now;		// List bellow are all entry points/data are on/before this date (default is TODAY)
		public List<EntryPoint> entryPoints;		// if settlementDate is null - all available dates will be taken

        public object Clone()
        {
            YieldCurveData ycs = new YieldCurveData();
            ycs.settings=(YieldCurveSetting)this.settings.Clone();
            
            return ycs;
        }
    }
	
	//
	// the single point on the plot
	//

    public class EntryPoint
    {
        public int Id { get; set; }					// id of the entry point
        public int YieldCurveId { get; set; }		// id of the yield curve for which entry point is set
        public bool Enabled { get; set; }			// entry rate can be disabled manually or automatically if it breaks callibration 
        public string Type { get; set; }            // type=deposit, swap or bond
        public int Length { get; set; }             // Rate only, not for bonds. Length + timeUnit = rate maturity
        public string TimeUnit { get; set; }        // Rate only, not for bonds. timeUnit = {"Days", "Weeks", "Months" ,"Years"}
        public Instrument Instrument { get; set; }	// each point is either the rate or the bond
        public int DataProviderId { get; set; }
        public string DataReference { get; set; }
        public DateTime ValidDateBegin { get; set; }  //if the yc definition has been changed it indicates from what date the entry rate is valid
        public DateTime ValidDateEnd { get; set; }


        public HistoricValue EntryPoint { get; set; }           // value of the rate/bond

		private int duration;
        public int Duration
        {
            get
            {
                if (duration == 0)
                {
					if (0 == String.Compare(Type, "bond", StringComparison.OrdinalIgnoreCase))
                    {
                        DateTime dt1 = DateTime.Today;
                        DateTime dt2 = (Instrument as Bond).MaturityDate;
                        TimeSpan days = dt2.Subtract(dt1);
                        duration = days.Days;
                    }
                    else
                    {
                        if (0 == String.Compare(TimeUnit, "Days", StringComparison.OrdinalIgnoreCase))
                        {
                            duration = Length;
                        }
                        else if (0 == String.Compare(TimeUnit, "Weeks", StringComparison.OrdinalIgnoreCase))
                        {
                            duration = Length * 7;
                        }
                        else if (0 == String.Compare(TimeUnit, "Months", StringComparison.OrdinalIgnoreCase))
                        {
                            DateTime dt1 = DateTime.Today;
                            DateTime dt2 = dt1.AddMonths(Length);
                            TimeSpan days = dt2.Subtract(dt1);
                            duration = days.Days;
                            //duration = Length * 30;
                        }
                        else if (0 == String.Compare(TimeUnit, "Years", StringComparison.OrdinalIgnoreCase))
                        {
                            DateTime dt1 = DateTime.Today;
                            DateTime dt2 = dt1.AddYears(Length);
                            TimeSpan days = dt2.Subtract(dt1);
                            duration = days.Days;
                            //  duration = Length * 365;
                        }
                    }
                }
                return duration;
            }

            set { duration = Duration; }
        }
    }

    public class YieldCurveEntryDataCompare : System.Collections.Generic.IComparer<EntryPoint>
    {
        public int Compare(EntryPoint p1, EntryPoint p2)
        {
            return p1.Duration - p2.Duration;
        }
    }

	//
	// the same but with whole History
	//

	public class EntryPointHistory : EntryPoint
    {
		public List<HistoricValue> EntryDataHistory;
		/*
        private DateTime setDate;
		public DateTime SetDate
        {
            get { return setDate; }
            set { setDate = value; }
        }
		*/
		public double ValueLast
		{
			get { return EntryDataHistory.Max().Value; }
		}

		public double ValueByDate(DateTime? settlementDate)
		{
			if (settlementDate == null || EntryDataHistory.Max().Date <= settlementDate.Value)
				return EntryDataHistory.Max().Value;

			EntryDataHistory.Sort();
			int i = ~(EntryDataHistory.BinarySearch(new HistoricValue { Date = settlementDate.Value }));
			return EntryDataHistory[i - 1].Value;
		}
    }

	public class EntryPointHistoryCompare : System.Collections.Generic.IComparer<EntryPointHistory>
	{
		public int Compare(EntryPointHistory p1, EntryPointHistory p2)
		{
			return p1.Duration - p2.Duration;
		}
	}
#endif // FALSE
}
