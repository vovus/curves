using System;
using System.Collections.Generic;

namespace DataLayer
{
    public class Bond : Instrument
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Type { get; set; }   //victor added:  type=fixed or floating
        //public int DataProviderId { get; set; }
        //public string DataReference { get; set; }
        //public int IdCcy { get; set; }
        //public string CcyName { get; set; }
		//public int SettlementDays { get; set; }
		//public int YieldCurveId { get; set; }

		public DateTime IssueDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public string DateGeneration { get; set; }  
        public string CouponFrequency { get; set; }  
        public double Coupon { get; set; }
        public string CouponConvention { get; set; }  
        public int CouponBasisId { get; set; }				// DayCounters table
        public double Redemption { get; set; }
        public double Nominal { get; set; }
      
		//    `CalendarId` INT NULL, 
		// `YieldCurveId` INT NULL,   for a moment not important
               
        public Bond() : base() {}
    }

   
}
