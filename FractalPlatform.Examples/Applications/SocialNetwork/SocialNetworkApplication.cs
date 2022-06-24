﻿using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Common.Enums;
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

        public void Users()
        {
            Client.SetDefaultCollection("Users")
                  .GetAll()
                  .OpenForm();
        }

        public void ViewPosts()
        {
            Client.SetDefaultCollection("Users")
                  .GetWhere(DQL("{'Name':@Name}", UserContext.User.Name))
                  .ToCollection("{'ViewPosts':[$]}", true)
                  .ChangeDimension(UserContext,
                                   DimensionType.UI,
                                   "{'Style':'Save:false','ViewPosts':[{'Visible':true}]}")
                  .OpenForm(UserContext);
        }

        public void NewPost()
        {
            var docID = Client.SetDefaultCollection("Users")
                              .GetWhere(DQL("{'Name':@Name}", UserContext.User.Name))
                              .GetFirstID();

            Client.SetDefaultCollection("NewPost")
                 .WantCreateNewDocumentForArray("Users", docID, "{'Posts':[$]}")
                 .OpenForm();
        }

        public void RequestFriend(uint docID)
        {
            if (!Client.SetDefaultCollection("Users")
                      .GetDoc(docID)
                      .AndWhere(DQL("{'Friends':[{'Name':@Name}]}", UserContext.User.Name))
                      .Any())
            {
                Client.SetDefaultCollection("Users")
                      .GetDoc(docID)
                      .Update(DQL("{'Friends':[Add,{'Name':@Name,'Approved':false}]}", UserContext.User.Name));

                MessageBox("You have sent friend request.");
            }
            else
            {
                MessageBox("Your have already sent friend request.");
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
                case "MyUser":
                    MyUser();
                    break;
                case "Users":
                    Users();
                    break;
                case "ViewPosts":
                    ViewPosts();
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

        private void Friend(KeyMap key, bool approve)
        {
            if(Client.SetDefaultCollection("Users")
                     .GetWhere(key)
                     .AndWhere(DQL("{'Friends':[{'Approved':@Approve}]}", approve))
                     .Any())
            {
                MessageBox("You have already friend/unfriend user.");

                return;
            }

            //my friend
            Client.SetDefaultCollection("Users")
                  .GetWhere(key)
                  .Update(DQL("{'Friends':[{'Approved':@Approve}]}", approve));

            //and his friend
            var friend = Client.SetDefaultCollection("Users")
                               .GetWhere(key)
                               .Value("{'Friends':[{'Name':$}]}");

            if (approve)
            {
                Client.SetDefaultCollection("Users")
                      .GetWhere(DQL("{'Name':@Name}", friend))
                      .Update(DQL("{'Friends':[Add,{'Name':@Name,'Approved':true}]}", UserContext.User.Name));
            }
            else
            {
                Client.SetDefaultCollection("Users")
                      .GetWhere(DQL("{'Name':@Friend,'Friends':[{'Name':@User}]}", friend, UserContext.User.Name))
                      .Delete(DQL("{'Friends':[{'Name':$,'Approved':$}]}", UserContext.User.Name, approve));
            }
        }

        public override bool OnMenuDimension(Context context, Collection collection, KeyMap key, uint docID, string action)
        {
            switch (action)
            {
                case "Friend":
                    Friend(key, true);
                    break;
                case "Unfriend":
                    Friend(key, false);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return true;
        }

        public override BaseRenderForm CreateRenderForm(string formName) => new RenderForm(this);
    }
}
