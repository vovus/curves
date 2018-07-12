using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserAccounts
{
	public partial class EmailConfirmation1 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Guid newUserGuid = new Guid(Request.QueryString["ID"]);

			string newUserGuidStr = newUserGuid.ToString("B");
			if (Repository.ConfirmUser(newUserGuidStr))
			{
				lblMessage.Text = "Account Approved, please <a href=\"Login.aspx\"> Login</a> to continue";
			}
			else
			{
				lblMessage.Text = "Account failed to be confirmed, please contact our team <a href=\"contact.aspx\"></a>";
			}
		}
	}
}