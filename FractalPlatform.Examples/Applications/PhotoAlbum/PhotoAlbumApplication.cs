using BigDoc.Client;
using BigDoc.Client.UI;
using BigDoc.Database.Engine;

namespace FractalPlatform.Examples.Applications.PhotoAlbum
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
                      Client.SetDefaultCollection("Photos")
                            .GetDoc(Constants.FIRST_DOC_ID)
                            .OpenForm();
                  });
        }

        public override BaseRenderForm CreateRenderForm(string renderFormName) => new RenderForm(this);
    }
}
