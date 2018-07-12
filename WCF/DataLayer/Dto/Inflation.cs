using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    
    public class InflationBond : Instrument
    {
        public int InflationIndexId { get; set; }  //id of the inflation undex which is used for computation of coupons
        public DateTime IssueDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public string DateGeneration { get; set; }
        public string CouponFrequency { get; set; }
        public double Coupon { get; set; }
        public string CouponConvention { get; set; }
        public int CouponBasisId { get; set; }				// DayCounters table
        public double Redemption { get; set; }
        public double Nominal { get; set; }

        public InflationBond() : base() { }
    }

    public class InflationIndex : Instrument
    {
        public string ClassName { get; set; }

        public InflationIndex() : base() { }
    }
   
    public class InflationRate : Instrument
    {
        //--------- general definition ----
        public string ClassName { get; set; }
        public int InflationCurveId { get; set; }
        public int Duration { get; set; }
        public string TimeUnit { get; set; }
        public int Accuracy { get; set; }
        public int SettlementLag { get; set; }
        //--------- Index part ------
        public int InflationIndexId { get; set; }
        public int InflationLag { get; set; }
        public string InflationLagTimeUnit { get; set; }
        // -------- Rate part -------
        public int BasisId { get; set; }
        public int FrequencyId { get; set; }
        public string Frequency { get; set; }
        public int CompoundingId { get; set; }
        public string Compounding { get; set; }
        public double Spread { get; set; }
        
        public InflationRate() : base() { }
    }
    
    // -------- Inflation curve data -------------------
    //this class is to have a snapshot of the Inflation curve for a specific date with entry data relevant for that date and values for entry data
    public class InflationCurveSnapshot : ICloneable
    {
        public int Id;
		public List<HistoricValue> IndexHistory;
        public DateTime settlementDate { get; set; }  //date of the snapshot
        public string Name { get; set; }
        public string Family { get; set; }
        public int CurrencyId { get; set; }
        public int InflationIndexId { get; set; }
        public InflationIndex InflationIndex { get; set; }

        public InflationCurveSetting settings { get; set; }

        public List<InflationCurveEntryData> EntryList { get; set; }
        public List<double> ValueList { get; set; }   //number of elements in this list must be = number of elements in EntryList
        // these are last prices of entry instruments at a specific date of the snapsot

         
        public object Clone()  //to be done
        {
            InflationCurveSnapshot ycs = new InflationCurveSnapshot();

            ycs.Id=this.Id;
            ycs.Name=this.Name;
            ycs.Family=this.Family;
            ycs.CurrencyId =this.CurrencyId;
            ycs.InflationIndexId=this.InflationIndexId;
            ycs.settlementDate=this.settlementDate;
            ycs.InflationIndex=this.InflationIndex;
            ycs.settings=this.settings;

			foreach (HistoricValue vp in this.IndexHistory)
                ycs.IndexHistory.Add(vp);
            foreach(InflationCurveEntryData iced in this.EntryList)
                ycs.EntryList.Add(iced);  
            foreach(double vl in this.ValueList)
                ycs.ValueList.Add(vl);
                  
            return ycs;
        }
    }

    public class InflationCurveSetting : ICloneable, IComparable
    {
        public int ZcBasisId { get; set; }
        public string ZcCompounding { get; set; }
        public string ZcFrequency { get; set; }
        public bool ifZc { get; set; }
        public String ZCColor { get; set; }
        public bool ifIndex { get; set; }
        public String IndexColor { get; set; }

        //???    public Calendar Calendar { get; set; }

        public object Clone()
        {
            InflationCurveSetting ics = new InflationCurveSetting();
            ics.ZcBasisId = this.ZcBasisId;
            ics.ZcCompounding = this.ZcCompounding;
            ics.ZcFrequency = this.ZcFrequency;
            ics.ifZc = this.ifZc;
            ics.ZCColor = this.ZCColor;
            ics.ifIndex = this.ifIndex;
            ics.IndexColor = this.IndexColor;

            return ics;
        }

        int IComparable.CompareTo(object obj)
        {
            InflationCurveSetting ics = obj as InflationCurveSetting;
            if (ics != null)
            {
                if (
                    (this.ZcBasisId == ics.ZcBasisId) && (this.ZcCompounding == ics.ZcCompounding) &&
                    (this.ZcFrequency == ics.ZcFrequency) && (this.ifZc == ics.ifZc) && (this.ZCColor == ics.ZCColor) &&
                    (this.ifIndex == ics.ifIndex) && (this.IndexColor == ics.IndexColor)

                   // (this.Calendar == ycs.Calendar)
                    )
                    return 0;
                else
                    return -1;
            }
            else
                throw new ArgumentException("Parameter is not a InflationCurveSetting!");
        }
    }

    public class InflationCurveData : ICloneable
    {
        public int Id;
        public string Name { get; set; }
        public string Family { get; set; }
        public int CurrencyId { get; set; }
        public int InflationIndexId { get; set; }

        public InflationCurveSetting settings;

        public object Clone()
        {
            InflationCurveData ics = new InflationCurveData();
            ics.settings = (InflationCurveSetting)this.settings.Clone();

            ics.Id = this.Id;
            ics.Name = this.Name;
            ics.Family = this.Family;
            ics.CurrencyId = this.CurrencyId;
            ics.InflationIndexId = this.InflationIndexId;

            return ics;
        }
    }

    public class InflationCurveEntryData
    {
        public int Id { get; set; }					// id of the entry point
        public int InflationCurveId { get; set; }		// id of the yield curve for which entry point is set
        public bool Enabled { get; set; }			// entry rate can be disabled manually or automatically if it breaks callibration 
        public string Type { get; set; }            // type=deposit, swap or bond
        public Instrument Instrument { get; set; }	// each point is either the rate or the bond
        public DateTime ValidDateBegin { get; set; }  //if the yc definition has been changed it indicates from what date the entry rate is valid
        public DateTime ValidDateEnd { get; set; }
        
        private int duration;
        public int Duration
        {
            get
            {
                if (duration == 0)
                {
                    if (0 == String.Compare(Type, "inflationbond", StringComparison.OrdinalIgnoreCase))
                    {
                        DateTime dt1 = DateTime.Today;
                        DateTime dt2 = (Instrument as InflationBond).MaturityDate;
                        TimeSpan days = dt2.Subtract(dt1);
                        duration = days.Days;
                    }
                    else if(0 == String.Compare(Type, "swap", StringComparison.OrdinalIgnoreCase))
                    {
                        DateTime dt1 = DateTime.Today;
                        DateTime dt2 = DateTime.Today;

                        InflationRate ir = (Instrument as InflationRate);
                        if (ir.TimeUnit == "Days")
                            dt2 =dt1.AddDays(ir.Duration);
                        else if ((Instrument as InflationRate).TimeUnit == "Months")
                            dt2 = dt1.AddMonths(ir.Duration);
                        else if (ir.TimeUnit == "Years")
                            dt2 = dt1.AddYears(ir.Duration);

                        TimeSpan days = dt2.Subtract(dt1);
                        duration = days.Days;
                    }
                    else //not implemented
                    {
                        duration = 0;
                    }

                }
                return duration;
            }

            set { duration = Duration; }
        }


        public object Clone()
        {
            InflationCurveEntryData iced = new InflationCurveEntryData();
            iced.Id = this.Id;
            iced.InflationCurveId = this.InflationCurveId;
            iced.Enabled = this.Enabled;
            iced.Type = this.Type;
            iced.Instrument = this.Instrument;
            iced.ValidDateBegin = this.ValidDateBegin;
            iced.ValidDateEnd = this.ValidDateEnd;

            return iced;
        }
    }
  
    public class InflationCurveEntryDataCompare : System.Collections.Generic.IComparer<InflationCurveEntryData>
    {
        public int Compare(InflationCurveEntryData p1, InflationCurveEntryData p2)
        {
            return p1.Duration - p2.Duration;
        }
    }

    public class RateHistory
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int RateId { get; set; }
        public DateTime Date { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double Theoretical { get; set; }
    } 
}