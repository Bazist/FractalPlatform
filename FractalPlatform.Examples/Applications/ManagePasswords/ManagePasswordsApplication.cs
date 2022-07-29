using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using System;

namespace FractalPlatform.Examples.Applications.ToDo
{
    public class ManagePasswordsApplication : BaseApplication
    {
        public ManagePasswordsApplication(Guid sessionId,
                               BigDocInstance instance,
                               IFormFactory formFactory) : base(sessionId,
                                                               instance,
                                                               formFactory,
                                                               "ManagePasswords")
        {
        }

        public override void OnStart(Context context)
        {
            Client.SetDefaultCollection("ManagePasswordsColl")
                  .GetFirstDoc()
                  .WantModifyExistingDocuments()
                  .OpenForm();
        }
    }
}
