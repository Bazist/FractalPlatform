using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using BigDoc.Database.Storages;
using System;

namespace FractalPlatform.Examples.Applications.Forum
{
    public class ForumApplication : DashboardApplication
    {
        public ForumApplication(Guid sessionId,
                                BigDocInstance instance,
                                IFormFactory formFactory) : base(sessionId,
                                                                instance,
                                                                formFactory,
                                                                "Forum")
        {
        }

        public override void OnStart(Context context)
        {
            Client.SetDefaultCollection("Dashboard")
                  .OpenForm(Constants.FIRST_DOC_ID);
        }

        private void SendMessage(FormResult result)
        {
            if (result.Result)
            {
                //get message
                var message = result.Collection
                                    .DocumentStorage
                                    .GetDoc(UserContext, result.DocID)
                                    .Value("{'Message':$}");

                //get user avatar
                var avatar = Client.SetDefaultCollection("Users")
                                   .GetWhere(DQL("{'Name':@Name}", UserContext.User.Name))
                                   .Value("{'Avatar':$}");

                //add new message
                Client.SetDefaultCollection("Topics")
                      .GetDoc(result.DocID)
                      .Update(DQL(@"{'Messages':[Add,{'OnDate':@OnDate,
                                                      'Who':@Who,
                                                      'Avatar':@Avatar,
                                                      'Message':@Message}],
                                     'Message':'Put your message here'}",
                                  DateTime.Now,
                                  UserContext.User.Name,
                                  avatar,
                                  message));

                //show topic form
                Client.SetDefaultCollection("Topics")
                      .GetDoc(result.DocID)
                      .OpenForm(SendMessage);
            }
        }

        public void Topics()
        {
            Client.SetDefaultCollection("Topics")
                  .GetAll()
                  .WantModifyExistingDocuments()
                  .OpenForm(SendMessage);
        }

        public void NewTopic()
        {
            Client.SetDefaultCollection("NewTopic")
                  .WantCreateNewDocumentFor("Topics")
                  .OpenForm();
        }

        public override bool OnOpenForm(Context context, Collection collection, KeyMap key, uint docID)
        {
            if (collection.Name == "Topics" &&
               docID != Constants.ANY_DOC_ID)
            {
                var countViews = Client.SetDefaultCollection("Topics")
                                       .GetDoc(docID)
                                       .Value("{'CountViews':$}");

                Client.SetDefaultCollection("Topics")
                      .GetDoc(docID)
                      .Update(new { CountViews = int.Parse(countViews) + 1 });

                collection.ReloadData();
            }

            return true;
        }

        public override object OnComputedDimension(Context context, Storage storage, KeyMap key, AttrValue value, uint docID, string variable)
        {
            switch (variable)
            {
                case "CountMessages":
                    {
                        var count = Client.SetDefaultCollection("Topics")
                                          .GetDoc(docID)
                                          .Count("{'Messages':[{'Who':$}]}");

                        return count;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public override bool OnEventDimension(Context context,
                                              Collection collection,
                                              KeyMap key,
                                              AttrValue attrValue,
                                              uint docID,
                                              string eventType,
                                              string action)
        {
            switch (action)
            {
                case "Login":
                    Login("Bob", "Bob");
                    break;
                case "Logout":
                    Logout();
                    break;
                case "Register":
                    Register();
                    break;
                case "Topics":
                    Topics();
                    break;
                case "NewTopic":
                    NewTopic();
                    break;
                default:
                    throw new NotImplementedException();
            }

            return true;
        }

        public override BaseRenderForm CreateRenderForm(string formName) => new RenderForm(this);
    }
}
