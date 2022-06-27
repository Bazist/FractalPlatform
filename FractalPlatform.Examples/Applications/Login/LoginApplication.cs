using BigDoc.Client;
using BigDoc.Database.Engine;
using System;

namespace FractalPlatform.Examples.Applications.Login
{
    public class LoginApplication : DashboardApplication
    {
        public LoginApplication(Guid sessionId,
                                 BigDocInstance instance,
                                 IFormFactory formFactory) : base(sessionId,
                                                                 instance,
                                                                 formFactory,
                                                                 "Login")
        {
        }

        public override void OnStart(Context context)
        {
            Login();
        }

        protected override void OnLogin(FormResult result)
        {
            MessageBox($"You logged in as: {UserContext.User.Name} with role {UserContext.User.Roles[0]}");
        }
    }
}
