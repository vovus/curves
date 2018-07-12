using System;
using System.Collections.Generic;

namespace DataLayer
{
    public class Rate : Instrument
	{
		public string ClassName { get; set; }
        public int Duration { get; set; }
        public string TimeUnit { get; set; }
        public int Accuracy { get; set; }
        //public DayCounter Basis { get; set; }				// DayCounters table
		public int BasisId { get; set; }
        public string Frequency { get; set; }
        public string Compounding { get; set; }
        public double Spread { get; set; }
       
        public string FixingPlace { get; set; }			// ??? should be the name of the calendar from quantlib. should add a custom class for calendar and use data from the database if quantlib class name is not passed
        public string BusinessDayConvention { get; set; }
        // swap part of the rate
        public int IdIndex { get; set; }
        public string IndexName { get; set; }
        public string ClassNameIndex { get; set; }
        public int IndexDuration { get; set; }
        public string IndexTimeUnit { get; set; }
        //public DayCounter BasisIndex { get; set; }				// DayCounters table
		public int BasisIndexId { get; set; }
        public string FrequencyIndex { get; set; }
        public string CompoundingIndex { get; set; }
        public double SpreadIndex { get; set; }
        // public string reference;

        public Rate() : base() {}
    }

  }
