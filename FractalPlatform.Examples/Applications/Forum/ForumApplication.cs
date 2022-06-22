using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using BigDoc.Database.Storages;
using System;

namespace FractalPlatform.Examples.Applications.Forum
{
    public class ForumApplication : DashboardApplication
    {
        public ForumApplication(string workingFolder,
                               IFormFactory formFactory) : base(workingFolder,
                                                                "Forum",
                                                                formFactory)
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
                Client.SetDefaultCollection("Articles")
                      .GetDoc(result.DocID)
                      .Update(DQL("{'Messages':[Add,{'OnDate':@OnDate,'Who':@Who,'Avatar':@Avatar,'Message':@Message}]}",
                                  DateTime.Now,
                                  UserContext.User.Name,
                                  avatar,
                                  message));

                //show article form
                Client.SetDefaultCollection("Articles")
                      .GetDoc(result.DocID)
                      .OpenForm(SendMessage);
            }
        }

        public void Articles()
        {
            Client.SetDefaultCollection("Articles")
                  .GetAll()
                  .WantModifyExistingDocuments()
                  .OpenForm(SendMessage);
        }

        public void NewArticle()
        {
            Client.SetDefaultCollection("NewArticle")
                  .WantCreateNewDocumentFor("Articles")
                  .OpenForm();
        }

        public override bool OnOpenForm(Context context, Collection collection, KeyMap key, uint docID)
        {
            if (collection.Name == "Articles" &&
               docID != Constants.ANY_DOC_ID)
            {
                var countViews = Client.SetDefaultCollection("Articles")
                                       .GetDoc(docID)
                                       .Value("{'CountViews':$}");

                Client.SetDefaultCollection("Articles")
                      .GetDoc(docID)
                      .Update(new { CountViews = int.Parse(countViews) + 1 });

                collection.ReloadData();
            }

            return true;
        }

        public override string OnComputedDimension(Context context, Storage storage, KeyMap key, AttrValue value, uint docID, string variable)
        {
            switch (variable)
            {
                case "CountMessages":
                    {
                        var count = Client.SetDefaultCollection("Articles")
                                          .GetDoc(docID)
                                          .Count("{'Messages':[{'Who':$}]}");

                        return count.ToString();
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
                    Login();
                    break;
                case "Logout":
                    Logout();
                    break;
                case "Register":
                    Register();
                    break;
                case "Articles":
                    Articles();
                    break;
                case "NewArticle":
                    NewArticle();
                    break;
                default:
                    throw new NotImplementedException();
            }

            return true;
        }

        public override BaseRenderForm CreateRenderForm(string formName) => new RenderForm(this);
    }
}
