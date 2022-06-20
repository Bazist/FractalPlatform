using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using BigDoc.Database.Storages;
using System;

namespace FractalPlatform.Examples
{
    public class ForumApplication : BaseApplication
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
                  .OpenForm(1);
        }

        private class LoginInfo
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }

        private void Login()
        {
            var doc = new LoginInfo();
            doc.Name = "Bob";
            doc.Password = "Bob";

            FormBuilder.OpenForm(UserContext, doc, null, result =>
            {
                if (result.Result)
                {
                    Client.SetDefaultCollection("Users")
                          .GetWhere(doc)
                          .SetUserContext();
                }
            });
        }

        private void Logout()
        {
            UserContext.ResetToGuest();
        }

        public void Articles()
        {
            Client.SetDefaultCollection("Articles")
                  .GetAll()
                  .WantModifyExistingDocuments()
                  .OpenForm();
        }

        public void NewArticle()
        {
            Client.SetDefaultCollection("NewArticle")
                  .WantCreateNewDocumentFor("Articles")
                  .OpenForm();
        }

        public void Register()
        {
            Client.SetDefaultCollection("NewUser")
                  .WantCreateNewDocumentFor("Users")
                  .OpenForm();
        }

        public override bool OnOpenForm(Context context, Collection collection, KeyMap key, uint docID)
        {
            if(collection.Name == "Articles" &&
               docID != Constants.ANY_DOC_ID &&
               key.IsEmpty)
            {
                var countViews = Client.SetDefaultCollection("Articles")
                                       .GetDoc(docID)
                                       .Value("{'CountViews':$}");

                Client.SetDefaultCollection("Articles")
                      .GetDoc(docID)
                      .Update(new { CountViews = int.Parse(countViews) + 1 });
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
    }
}
