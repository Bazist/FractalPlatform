using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using System;

namespace FractalPlatform.Examples
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
            Client.SetDefaultCollection("ToDo")
                  .GetDoc(Constants.FIRST_DOC_ID)
                  .WantModifyExistingDocuments()
                  .OpenForm();
        }
    }
}
