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
            if(domControl.Key == "Messages")
            {
                var sb = new StringBuilder();
                                
                foreach(DataRow dr in domControl.DataTable.Rows)
                {
                    sb.Append(@$"<tr>
                                    <td style='border:1px solid white'>
                                       <img style='max-width:50px;max-height:50px' src='{GetFilesUrl()}{dr["Avatar"]}'>
                                       <div>{dr["Who"]}</div>
                                       <div>{dr["OnDate"]}</div>
                                    </td>
                                    <td  style='border:1px solid white' align='left'>
                                        <div>{dr["Message"]}</div>
                                    </td>
                                 </tr>");
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
