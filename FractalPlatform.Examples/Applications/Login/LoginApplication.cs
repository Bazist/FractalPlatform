using BigDoc.Client;
using BigDoc.Database.Engine;

namespace FractalPlatform.Examples.Applications.Login
{
    public class LoginApplication : DashboardApplication
    {
        public LoginApplication(string workingFolder,
                               IFormFactory formFactory) : base(workingFolder,
                                                                "Login",
                                                                formFactory)
        {
        }

        public override void OnStart(Context context)
        {
            Login();
        }

        protected override void OnLogin(FormResult result)
        {
            MessageBox("You logged in as: " + UserContext.User.Name);
        }
    }
}
