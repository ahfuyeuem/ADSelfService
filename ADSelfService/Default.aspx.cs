using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace ADSelfService
{
    public partial class Default : System.Web.UI.Page
    {
        private const string DefaultPassword = "!QAZ2wsx"; // Default Passowrd

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUnlock_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            if (UnlockUserAccount(username))
            {
                lblMessage.Text = "Account unlock successful.";
            }
            else
            {
                
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            if (ResetUserPassword(username, DefaultPassword))
            {
                lblMessage.Text = "Password reset successful.";
            }
            else
            {
                lblMessage.Text = "Password reset failed.";
            }
        }

        private bool UnlockUserAccount(string username)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                {
                    UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
                    if (user != null)
                    {
                        if (user.IsAccountLockedOut())
                        {
                            user.UnlockAccount();
                            user.Save();
                            lblMessage.Text = "Account unlock successful.";
                            return true;
                        }
                        else
                        {
                            lblMessage.Text = "Account is not locked.";
                            return false;
                        }
                    }
                    else
                    {
                        lblMessage.Text = "User not found.";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error (not shown).
                lblMessage.Text = "Account unlock failed.";
                return false;
            }
        }

        private bool ResetUserPassword(string username, string newPassword)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                {
                    UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
                    if (user != null)
                    {
                        user.SetPassword(newPassword);
                        user.ExpirePasswordNow(); // Change password next time
                        user.Save();
                        return true;
                    }
                    else
                    {
                        lblMessage.Text = "User not found.";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error (not shown).
                lblMessage.Text = "Password reset failed.";
                return false;
            }
        }
    }
}
