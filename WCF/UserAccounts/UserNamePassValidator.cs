using System;
using System.ServiceModel;
using System.IdentityModel.Selectors;

namespace UserAccounts
{
	class CustomValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if(  userName == null ||  password == null)
            {
                throw new ArgumentNullException();
            }

            if (!(userName == "admin1" && password == "4dm1n1") )
            {
                throw new FaultException("Incorrect Username or Password");
            }
        }
    }
}
