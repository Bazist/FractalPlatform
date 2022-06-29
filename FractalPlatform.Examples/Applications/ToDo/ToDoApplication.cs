using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using System;

namespace FractalPlatform.Examples.Applications.ToDo
{
    public class ToDoApplication : BaseApplication
    {
        public ToDoApplication(Guid sessionId,
                               BigDocInstance instance,
                               IFormFactory formFactory) : base(sessionId,
                                                               instance,
                                                               formFactory,
                                                               "ToDo")
        {
        }

        public override void OnStart(Context context)
        {
            Client.SetDefaultCollection("ToDoColl")
                  .GetFirstDoc()
                  .WantModifyExistingDocuments()
                  .OpenForm();
        }
    }
}
