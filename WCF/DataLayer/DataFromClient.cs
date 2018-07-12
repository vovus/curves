using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    class DataFromClient
    {
    }

    public class YCSettings
    {
        public  string ZcCompounding { get; set; }
        public string ZcBasis { get; set; }
        public string ZcFrequency { get; set; }
        public string FrwCompounding { get; set; }
        public string FrwBasis { get; set; }
        public string FrwFrequency { get; set; }
        public int FrwTerm { get; set; }
        public string FrwTermBase { get; set; }  //can be only "Days", "Months", "Years"
    }
}
