using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;
using BigDoc.Database.Storages;
using System;

namespace FractalPlatform.Examples
{
    public class PhotoAlbumApplication : BaseApplication
    {
        public PhotoAlbumApplication(string workingFolder,
                               IFormFactory formFactory) : base(workingFolder,
                                                                "PhotoAlbum",
                                                                formFactory)
        {
        }

        public override void OnStart(Context context)
        {
            Client.SetDefaultCollection("NewPhoto")
                  .WantCreateNewDocumentForArray("Photos", Constants.FIRST_DOC_ID, "{'Photos':[$]}")
                  .OpenForm(result =>
                  {
                      if (result.Result)
                      {
                          Client.SetDefaultCollection("Photos")
                                .GetDoc(Constants.FIRST_DOC_ID)
                                .OpenForm();
                      }
                  });
        }
    }
}
