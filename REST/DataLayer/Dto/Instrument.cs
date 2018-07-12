using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
	public class Instrument
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int DataProviderId { get; set; }
		public string DataReference { get; set; }
		public string Type { get; set; }
		public int IdCcy { get; set; }
		//public string CcyName { get; set; }
		public int SettlementDays { get; set; }
		//public int YieldCurveId { get; set; }
       // public int IdCalendar { get; set; }
        public Calendar Calendar { get; set; }
	}
}
