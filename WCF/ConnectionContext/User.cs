using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserAccounts
{
	public class FCUser
	{
		public enum eStatus 
		{ 
			eDisabled = 0,
			eEnabled = 1
		};

		//public static Dictionary<Guid, FCUser> sActiveUsersDic = new Dictionary<Guid, FCUser>();
		
		public Guid g = Guid.Empty;
		public string name = string.Empty;
		public string pass = string.Empty;
		public eStatus status = eStatus.eDisabled;
		public long id;
	}
}