using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using System;

namespace FractalPlatform.Examples.Applications.ToDo2
{
    public class ToDo2Application : BaseApplication
    {
        public ToDo2Application(Guid sessionId,
                                        BigDocInstance instance,
                                        IFormFactory formFactory) : base(sessionId,
                                                                        instance,
                                                                        formFactory,
                                                                        "ToDo2")
        {
        }

        public override void OnStart(Context context)
        {
            Client.SetDefaultCollection("Categories")
                  .GetAll()
                  .WantModifyExistingDocuments()
                  .OpenForm();
        }
    }
}
