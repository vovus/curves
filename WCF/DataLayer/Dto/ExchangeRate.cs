using System;
using System.Collections.Generic;

namespace DataLayer
{
    public class ExchangeRate : Instrument
    {
        public string ClassName { get; set; }
        public string FixingPlace { get; set; }			
        public int PrimaryCurrencyId { get; set; }
        public int SecondaryCurrencyId { get; set; }
        
        public ExchangeRate() : base() { }
    }
}



        