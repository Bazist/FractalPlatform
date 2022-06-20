using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using BigDoc.Database.Storages;
using System;

namespace FractalPlatform.Examples
{
    public class TaxiApplication : BaseApplication
    {
        public TaxiApplication(string workingFolder,
                               IFormFactory formFactory) : base(workingFolder,
                                                                "Taxi",
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
                    Client.SetDefaultCollection("Drivers")
                          .GetWhere(doc)
                          .SetUserContext();
                }
            });
        }

        private void Logout()
        {
            UserContext.ResetToGuest();
        }

        public void MyDashboard()
        {
            Client.SetDefaultCollection("Drivers")
                  .GetWhere(new { Name = UserContext.User.Name })
                  .OpenForm();
        }

        public void NewOrder()
        {
            Client.SetDefaultCollection("NewOrder")
                  .WantCreateNewDocumentForArray("Orders", 1, "{'Orders':[$]}")
                  .OpenForm();
        }

        public void Register()
        {
            Client.SetDefaultCollection("NewDriver")
                  .WantCreateNewDocumentFor("Drivers")
                  .OpenForm();
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
                case "MyDashboard":
                    MyDashboard();
                    break;
                case "NewOrder":
                    NewOrder();
                    break;
                default:
                    throw new NotImplementedException();
            }

            return true;
        }


        public override bool OnMenuDimension(Context context,
                                             Collection collection,
                                             KeyMap key,
                                             uint docID,
                                             string action)
        {
            switch (action)
            {
                case "Take":
                    {
                        Client.SetDefaultCollection("Drivers")
                              .GetWhere(key)
                              .Update(DQL("{'NewOrders':[{'Who':@Who}]}", UserContext.User.Name));

                        break;
                    }
                case "Complete":
                    {
                        Client.SetDefaultCollection("Drivers")
                              .GetWhere(key)
                              .Update("{'ActiveOrders':[{'IsCompleted':true}]}");

                        break;
                    }
                default:
                    throw new InvalidOperationException();
            }

            return true;
        }
    }
}
