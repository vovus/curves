<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UserAccounts.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
  <form id="form1" runat="server">
	<table border="0" style="font-size: 100%; font-family: Verdana">
            
            <tr>
                <td align="right">
                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">
                        E-mail:</asp:Label></td>
                <td>
                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="EmailRequired" 
                        runat="server" 
                        ControlToValidate="UserName"
                        ErrorMessage="E-mail is required." 
                        ToolTip="E-mail is required." 
                        ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">
                        Password:</asp:Label></td>
                <td>
                    <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" 
                            runat="server" 
                            ControlToValidate="Password"
                            ErrorMessage="Password is required." 
                            ToolTip="Password is required." 
                            ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                </td>
            </tr>

			<tr>
                <td align="center" colspan="2" >
                   <asp:CheckBox ID="CheckBox1" 
						runat="server" Text="Remember me" oncheckedchanged="CheckBox1_CheckedChanged" />
				</td>
            </tr>
			<tr>
                <td align="left">
					<asp:HyperLink ID="HyperLink2" runat="server" 
						NavigateUrl="~/Pages/Register.aspx">Register</asp:HyperLink>
				</td>
				<td align="right">
					<asp:Button ID="Button2" runat="server" Text="Button" onclick="Button2_Click" />
				</td>
            </tr>
		</table>
	</form>
</body>
</html>

