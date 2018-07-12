using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

namespace Utilities
{
	public class Security
	{
		public static void ProtectSection(string sectionName, string provider)
		{
			//Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

			ConfigurationSection section = config.GetSection(sectionName);

			if (section != null && !section.SectionInformation.IsProtected)
			{
				section.SectionInformation.ProtectSection(provider);
				config.Save();
			}
		}
		public static void UnProtectSection(string sectionName)
		{
			//Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
			Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

			ConfigurationSection section = config.GetSection(sectionName);

			if (section != null && section.SectionInformation.IsProtected)
			{
				section.SectionInformation.UnprotectSection();
				config.Save();
			}
		}
	}
}
