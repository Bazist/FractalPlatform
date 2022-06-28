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
            if(domControl.Key == "ViewPosts")
            {
                var sb = new StringBuilder();

                int number = 0;

                foreach(DataRow dr in domControl.DataTable.Rows)
                {
                    var html = @"<a href='javascript:editGridRow(@GridName,@Number)'>
                                 <table border=1>
                                 <tr>
                                    <td>
                                        <img style='max-width:50px;max-height:50px' src='@Avatar'>
                                    </td>
                                    <td align='left' style='width:100%' nowrap>
                                       <div>@Message</div>
                                    </td>
                                    <td nowrap>
                                       <div>@Who</div>
                                    </td>
                                    <td nowrap>
                                       <div>@OnDate</div>
                                    </td>
                                 </tr>
                                 <tr>
                                    <td colspan=4>
                                       <img style='max-width:560px;max-height:560px' src='@Photo'>
                                    </td>
                                 </tr>
                                 </table>
                                 </a>";

                    html = html.Replace("@GridName", $"\"{domControl.GetEscapedName()}\"");
                    html = html.Replace("@Number", number.ToString());
                    html = html.Replace("@Message", dr["Message"].ToString());
                    html = html.Replace("@Who", dr["Who"].ToString());
                    html = html.Replace("@OnDate", dr["OnDate"].ToString());
                    html = html.Replace("@Photo", GetFilesUrl() + dr["Photo"]);
                    html = html.Replace("@Avatar", GetFilesUrl() + dr["Avatar"]);

                    sb.Append(html);

                    number++;
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
