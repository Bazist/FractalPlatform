using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using System;

namespace FractalPlatform.Examples.Applications.ToDoWithCategories
{
    public class ToDoWithCategoriesApplication : BaseApplication
    {
        public ToDoWithCategoriesApplication(Guid sessionId,
                                        BigDocInstance instance,
                                        IFormFactory formFactory) : base(sessionId,
                                                                        instance,
                                                                        formFactory,
                                                                        "ToDoWithCategories")
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
