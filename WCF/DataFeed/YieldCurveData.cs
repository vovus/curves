using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;

// used by API
public class YieldCurveDefinition : YieldCurveSetting
{
	public int Id;
	public int CurrencyId { get; set; }
	public string Name { get; set; }
	public int FamilyId { get; set; }
}
//