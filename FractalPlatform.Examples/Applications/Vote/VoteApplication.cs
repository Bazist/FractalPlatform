using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using System;

namespace FractalPlatform.Examples
{
    public class VoteApplication : BaseApplication
    {
        public VoteApplication(Guid sessionId,
                               BigDocInstance instance,
                               IFormFactory formFactory) : base(sessionId,
                                                                instance,
                                                                formFactory,
                                                                "Vote")
        {
        }

        public override void OnStart(Context context)
        {
            Client.SetDefaultCollection("Questionary")
                  .WantMergeDocumentFor("Report", 1)
                  .OpenForm(result => 
                  {
                      if(result.Result)
                      {
                          Client.SetDefaultCollection("Report")
                                .OpenForm(1);
                      }
                  });
        }
    }
}
