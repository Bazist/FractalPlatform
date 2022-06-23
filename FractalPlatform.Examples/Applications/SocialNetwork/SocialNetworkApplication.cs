using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using BigDoc.Database.Storages;
using System;

namespace FractalPlatform.Examples.Applications.SocialNetwork
{
    public class SocialNetworkApplication : DashboardApplication
    {
        public SocialNetworkApplication(string workingFolder,
                               IFormFactory formFactory) : base(workingFolder,
                                                                "SocialNetwork",
                                                                formFactory)
        {
        }

        public override void OnStart(Context context)
        {
            Client.SetDefaultCollection("Dashboard")
                  .OpenForm(Constants.FIRST_DOC_ID);
        }

        public void MyUser()
        {
            Client.SetDefaultCollection("Users")
                  .GetWhere(DQL("{'Name':@Name}", UserContext.User.Name))
                  .OpenForm();
        }

        public void Friends()
        {
            Client.SetDefaultCollection("Users")
                  .GetWhere(DQL("{'Name':@Name,'Friends':[{'Approved':true}]}", UserContext.User.Name))
                  .OpenForm("{'Friends':[{'Name':$}]}");
        }

        public void Users()
        {
            Client.SetDefaultCollection("Users")
                  .GetAll()
                  .OpenForm();
        }

        public void Posts()
        {
            Client.SetDefaultCollection("Users")
                  .GetWhere(DQL("{'Name':@Name}", UserContext.User.Name))
                  .OpenForm("{'Posts':[{'Who':$,'OnDate':$,'Message':$,'Picture':$}]}");
        }

        public void NewPost()
        {
            var docID = Client.SetDefaultCollection("Users")
                              .GetWhere(DQL("{'Name':@Name}", UserContext.User.Name))
                              .GetFirstID();

            Client.SetDefaultCollection("NewPost")
                 .WantCreateNewDocumentForArray("Users", docID, "{'MyPosts':[$]}")
                 .OpenForm();
        }

        public void RequestFriend(uint docID)
        {
            Client.SetDefaultCollection("Users")
                  .GetDoc(docID)
                  .Update(DQL("{'Friends':[Add,{'Name':@Name,'Approved':false}]}", UserContext.User.Name));
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
                case "MyUser":
                    MyUser();
                    break;
                case "Friends":
                    Friends();
                    break;
                case "Users":
                    Users();
                    break;
                case "Posts":
                    Posts();
                    break;
                case "NewPost":
                    NewPost();
                    break;
                case "RequestFriend":
                    RequestFriend(docID);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return true;
        }

        private void Friend(KeyMap key)
        {
            Client.SetDefaultCollection("Users")
                  .GetWhere(key)
                  .Update("{'Friends':[{'Approved':true}]}");
        }

        public override bool OnMenuDimension(Context context, Collection collection, KeyMap key, uint docID, string action)
        {
            switch (action)
            {
                case "Friend":
                    Friend(key);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return true;
        }

        public override BaseRenderForm CreateRenderForm(string formName) => new RenderForm(this);
    }
}
