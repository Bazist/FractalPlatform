using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using System;

namespace FractalPlatform.Examples
{
    public class ToDoApplication : BaseApplication
    {
        public ToDoApplication(string workingFolder,
                               IFormFactory formFactory) : base(workingFolder,
                                                                "ToDo",
                                                                formFactory)
        {
        }

        public override void OnStart(Context context)
        {
            Client.SetDefaultCollection("ToDoColl")
                  .GetDoc(Constants.FIRST_DOC_ID)
                  .WantModifyExistingDocuments()
                  .OpenForm();
        }
    }
}
