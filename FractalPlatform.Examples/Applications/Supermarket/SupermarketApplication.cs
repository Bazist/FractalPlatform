using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using System;
using System.Linq;
using System.Collections.Generic;
using BigDoc.Common.Enums;
using BigDoc.Database.Storages;
using BigDoc.Client;

namespace FractalPlatform.Examples.Applications.Supermarket
{
    public class SupermarketApplication : BaseApplication
    {
        public SupermarketApplication(Guid sessionId,
                                      BigDocInstance instance,
                                      IFormFactory formFactory) : base(sessionId,
                                                                        instance,
                                                                        formFactory,
                                                                        "Supermarket")
        {
        }

        public override void OnStart(Context context)
        {
            Client.SetDefaultCollection("Dashboard")
                  .OpenForm(Constants.FIRST_DOC_ID);
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

        public void ViewStock()
        {
            Client.SetDefaultCollection("Stock")
                  .GetAll()
                  .OpenForm();
        }

        public void NewOrder()
        {
            Client.SetDefaultCollection("NewOrder")
                  .WantCreateNewDocumentFor("Orders")
                  .OpenForm(result => 
                  {
                      if (result.Result)
                      {
                          var storage = result.Collection.DocumentStorage.GetNativeStorage();

                          var cart = storage.GetDoc(UserContext, result.DocID)
                                            .SelectOne<Cart>();

                          Client.BeginTran(TranType.RepeatableRead);

                          foreach(var product in cart.Products)
                          {
                              var count = Client.SetDefaultCollection("Stock")
                                                .GetDoc(Constants.FIRST_DOC_ID)
                                                .AndWhere(DQL("{'Products':[{'Product':@Product}]}", product.Product))
                                                .Value(DQL("{'Products':[{'Count':$}]}"));

                              var newCount = int.Parse(count) - product.Count;

                              Client.SetDefaultCollection("Stock")
                                                .GetDoc(Constants.FIRST_DOC_ID)
                                                .AndWhere(DQL("{'Products':[{'Product':@Product}]}", product.Product))
                                                .Update(DQL("{'Products':[{'Count':@Count}]}", newCount));

                          }

                          Client.CommitTran();
                      }
                  });
        }

        public void Register()
        {
            Client.SetDefaultCollection("NewUser")
                  .WantCreateNewDocumentFor("Users")
                  .OpenForm();
        }

        public class ProductItem
        {
            public string Product { get; set; }

            public int Count { get; set; }

            public int Price { get; set; }
        }

        public class Stock
        {
            public List<ProductItem> StockProducts { get; set; }
        }

        public class Cart
        {
            public List<ProductItem> Products { get; set; }
        }

        public override bool OnMenuDimension(Context context, Collection collection, KeyMap key, uint docID, string action)
        {
            switch (action)
            {
                case "AddToCart":
                    {
                        var storage = collection.DocumentStorage;

                        //read entities
                        var stock = storage.GetWhere(context, key).SelectOne<Stock>();

                        var cart = storage.GetDoc(context, docID).SelectOne<Cart>();

                        var stockProduct = stock.StockProducts[0];

                        //add to cart
                        var product = cart.Products.FirstOrDefault(x => x.Product == stockProduct.Product);

                        if (product != null)
                        {
                            product.Count++;

                            product.Price = product.Count * stockProduct.Price;
                        }
                        else
                        {
                            stockProduct.Count = 1;

                            cart.Products.Add(stockProduct);
                        }

                        //save
                        storage.GetDoc(context, docID).Update(cart);

                        return false; //do not reload data
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
                case "Stock":
                    ViewStock();
                    break;
                case "NewOrder":
                    NewOrder();
                    break;
                default:
                    throw new NotImplementedException();
            }

            return true;
        }
    }
}
