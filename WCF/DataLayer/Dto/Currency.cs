using System;
using System.Collections.Generic;

namespace DataLayer
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ClassName { get; set; }
        public int NumericCode { get; set; }
        public int FractionsPerUnit { get; set; }
        public String FractionSymbol { get; set; }
        public String Symbol { get; set; }
        //public Currency1() { }
        //for a moment just leaving last two fileds out of the scope
        //public static ??? Rounding { get; set; }
        // public static String TriangulationCurrencyClassName { get; set; }
        //public List<YieldCurveFamily> YieldCurveFamilyList { get; set; }
    }
}