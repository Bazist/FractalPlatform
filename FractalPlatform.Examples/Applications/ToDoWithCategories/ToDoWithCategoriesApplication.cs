using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using System;

namespace FractalPlatform.Examples
{
    public class ToDoWithCategoriesApplication : BaseApplication
    {
        public ToDoWithCategoriesApplication(string workingFolder,
                               IFormFactory formFactory) : base(workingFolder,
                                                                "ToDoWithCategories",
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
