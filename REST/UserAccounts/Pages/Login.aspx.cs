using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace UserAccounts
{
	public partial class Login : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
		{
		}

		protected void Button2_Click(object sender, EventArgs e)
		{
			/*
			FCUser tmp = new FCUser 
			{
				id = -1,
				name = UserName.Text,
				pass = Password.Text,
				g = Guid.Empty,
				status = FCUser.eStatus.eDisabled
			};
			*/
			// verify the user
			FCUser user = Repository.LoginUser(UserName.Text, Password.Text);
			
			if (user == null
				|| user.status != FCUser.eStatus.eEnabled)
			{ 
				// can't login
				return;
			}

			Session["user"] = user;

			if (CheckBox1.Enabled)
				FormsAuthentication.SetAuthCookie(UserName.Text, true);

			Repository.sIsLogedIn = true;
		}

		public void Logout()
		{
			String user = (String)Session["user"];

			if (CheckBox1.Enabled)
				FormsAuthentication.SetAuthCookie(user, false);

			Session.Abandon();

			Repository.sIsLogedIn = false;
		}
	}
}