using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net.Mail;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;

namespace UserAccounts
{
	public partial class Register : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				if (CreateUser(UserName.Text, Password.Text))
					lblMessage.Text = "Thanks for registration, verification e-mail has been sent to " + UserName.Text;
				else
					lblMessage.Text = "User already exists";
			}
		}

		public bool CreateUser(string name, string pass)
		{
			FCUser newUser = Repository.CreateUser(name, pass);
			if (newUser == null)
				return false;

			string newUserGuidStr = newUser.g.ToString("B");	// 38 chars, e.g. {12345678-1234-1234-1234-123456789abc}

			// send confirmation email
			string domainName = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
			string confirmationPage = "Pages/EmailConfirmation.aspx?ID=" + newUserGuidStr;
			string url = domainName + confirmationPage;

			MailAddress to = new MailAddress(newUser.name);
			MailAddress from = new MailAddress("admin@fincurve.com");
			
			var message = new MailMessage(from, to);

			try
			{
				string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\user_data\\";

				using (StreamReader sr = new StreamReader(path + "NewUserMail.txt"))
				{
					message.Body = sr.ReadToEnd();
				}
			}
			catch (Exception e)
			{
				Utilities.Logger.WriteError(String.Format("CreateUser() failed - {0}", e.Message));
				//Console.WriteLine("The file could not be read:");
				//Console.WriteLine(e.Message);

				return false;
			}

			message.Subject = "[fincurve.com] Registration confirmation";
			message.Body = message.Body.Replace("<%UserName%>", newUser.name);
			message.Body = message.Body.Replace("<%VerificationUrl%>", url);

			SmtpClient smtp = new SmtpClient();

			smtp.Host = "smtp.gmail.com";
			smtp.Port = 587;

			NameValueCollection tmp = ConfigurationManager.AppSettings;
			string adminEmailName = tmp["AdminEmailName"];
			string adminEmailPass = tmp["AdminEmailPass"];

			smtp.UseDefaultCredentials = false;
			smtp.Credentials = new System.Net.NetworkCredential(adminEmailName, adminEmailPass);
			smtp.EnableSsl = true;

			smtp.Send(message);
			//e.Cancel = true;

			return true;
		}

		public string GetErrorMessage(MembershipCreateStatus status)
		{
			switch (status)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "Username already exists. Please enter a different user name.";
				case MembershipCreateStatus.DuplicateEmail:
					return "A username for that e-mail address already exists. Please enter a different e-mail address.";
				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";
				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";
				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";
				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";
				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";
				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact Support.";
				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact Support.";
				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact Support.";
			}
		}
	}
}