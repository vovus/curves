using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
	public class DayCounter
	{
		public int Id;
		public string Name;			// user friendly name, used in GUI, gotten from QuantLib's name() method of the class
		public string ClassName;	// used by ObjectFactory, shell be the same as in QuantLib 

		public DayCounter(string cn, string n) { Id = -1; ClassName = cn; Name = n; }
		public DayCounter() { Id = -1; ClassName = string.Empty; Name = string.Empty; }
	}
}
