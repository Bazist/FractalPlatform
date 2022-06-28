using BigDoc.Client;
using BigDoc.Client.UI.DOM.Controls.Grid;
using System.Data;
using System.Text;

namespace FractalPlatform.Examples.Applications.SocialNetwork
{
    public class RenderForm: ExtendedRenderForm
    {
        public RenderForm(BaseApplication application) : base(application)
        {
        }

        public override string RenderGrid(GridDOMControl domControl)
        {
            if(domControl.Key == "ViewPosts" ||
               domControl.Key == "Posts")
            {
                var sb = new StringBuilder();

                sb.Append("<table border=1>");
                                
                foreach(DataRow dr in domControl.DataTable.Rows)
                {
                    sb.Append(@$"<tr>
                                    <td align='left' style='width:100%' nowrap>
                                       <div>{dr["Message"]}</div>
                                    </td>
                                    <td nowrap>
                                       <div>{dr["Who"]}</div>
                                    </td>
                                    <td nowrap>
                                       <div>{dr["OnDate"]}</div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td colspan=3>
                                       <img style='max-width:560px;max-height:560px' src='{GetFilesUrl()}{dr["Photo"]}'>
                                    </td>
                                 </tr>");
                }

                sb.Append("</table>");

                return sb.ToString();
            }
            else
            {
                return base.RenderGrid(domControl);
            }
        }
    }
}
