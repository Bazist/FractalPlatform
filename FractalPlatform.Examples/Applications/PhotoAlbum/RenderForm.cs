using BigDoc.Client;
using BigDoc.Client.UI.DOM.Controls.Grid;
using System.Data;
using System.Text;

namespace FractalPlatform.Examples.Applications.PhotoAlbum
{
    public class RenderForm: BaseRenderForm
    {
        public RenderForm(BaseApplication application) : base(application)
        {

        }

        public override string RenderGrid(GridDOMControl domControl)
        {
            if(domControl.Key == "Photos")
            {
                var sb = new StringBuilder();

                foreach(DataRow dr in domControl.DataTable.Rows)
                {
                    sb.Append("<div align='left'>").Append(dr["Title"]).Append("</div>")
                      .Append("<div><img style='max-width:480px;max-height:480px' src='")
                      .Append(GetFilesUrl()).Append(dr["Photo"]).Append("'></div><br><br>");
                }

                return sb.ToString();
            }
            else
            {
                return base.RenderGrid(domControl);
            }
        }
    }
}
