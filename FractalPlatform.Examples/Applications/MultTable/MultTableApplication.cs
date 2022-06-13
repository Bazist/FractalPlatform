using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Common.Enums;
using BigDoc.Database.Engine;
using BigDoc.Database.Storages.BUF;
using System;
using System.Linq;
using System.Text;

namespace FractalPlatform.Examples
{
    public class MultTableApplication : BaseApplication
    {
        public MultTableApplication(Guid sessionId,
                                    BigDocInstance instance,
                                    IFormFactory formFactory) : base(sessionId,
                                                                    instance,
                                                                    formFactory,
                                                                    "MultTable")
        {
        }

        private class Setting
        {
            public uint From { get; set; }

            public uint To { get; set; }

            public bool Shuffle { get; set; }

            public uint PagePortion { get; set; }
        }

        private uint[] GetArray(uint from, uint to, bool shuffle)
        {
            var arr = new uint[to - from + 1];

            for (uint i = from, j = 0; i <= to; i++, j++)
            {
                arr[j] = i;
            }

            if (shuffle)
            {
                Random rnd = new Random();

                arr = arr.OrderBy(x => rnd.Next())
                         .ToArray();
            }

            return arr;

        }

        public override bool OnCloseForm(Context context, Collection collection, KeyMap key, uint docID)
        {
            return collection.Name != "Doc";
        }

        public override void OnStart(Context context)
        {
            Client.SetDefaultCollection("Setting")
                  .GetDoc(1)
                  .OpenForm(null, null, result =>
                  {
                      if (result.Result)
                      {
                          var setting = result.Collection
                                              .DocumentStorage
                                              .GetDoc(UserContext, 1)
                                              .SelectOne<Setting>();

                          var sbDoc = new StringBuilder();
                          var sbVal = new StringBuilder();

                          sbDoc.Append('{');
                          sbVal.Append('{');

                          foreach (var i in GetArray(setting.From, setting.To, setting.Shuffle))
                          {
                              foreach (var j in GetArray(0, 9, setting.Shuffle))
                              {
                                  if (j > 0 && i % j == 0)
                                  {
                                      if (sbDoc.Length > 1)
                                      {
                                          sbDoc.Append(",\n");
                                          sbVal.Append(",\n");
                                      }

                                      sbDoc.Append($"\"{i} : {j} =\":\"\"");
                                      sbVal.Append($"\"{i} : {j} =\":").Append("{\"Formula\":\"@Value = " + (i / j).ToString() + "\"}");
                                  }
                              }
                          }

                          sbDoc.Append('}');
                          sbVal.Append('}');

                          var doc = new BUFStorage("Doc");
                          doc.Init();
                          doc.FromJson(sbDoc.ToString(), Constants.FIRST_DOC_ID);

                          var val = new BUFStorage("Val");
                          val.Init();
                          val.FromJson(sbVal.ToString(), Constants.BASE_DOC_ID);

                          var coll = new Collection(doc);
                          coll.SetDimension(UserContext, DimensionType.Validation, val);
                          coll.SetDimension(UserContext, DimensionType.Wizard);

                          FormBuilder.OpenForm(UserContext, coll, Constants.FIRST_DOC_ID);
                      }
                  });
        }
    }
}
