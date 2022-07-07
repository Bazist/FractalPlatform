﻿using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using System;

namespace FractalPlatform.Examples.Applications.Vote
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
                  .WantMergeDocumentFor("Report", Constants.FIRST_DOC_ID)
                  .OpenForm(result => 
                  {
                      if(result.Result)
                      {
                          Client.SetDefaultCollection("Report")
                                .OpenForm(Constants.FIRST_DOC_ID);
                      }
                  });
        }
    }
}
