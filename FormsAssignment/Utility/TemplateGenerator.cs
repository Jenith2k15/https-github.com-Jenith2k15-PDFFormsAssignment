using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsAssignment.Models;

namespace FormsAssignment.Utility
{
    public static class TemplateGenerator
    {
        public static string GetHTMLString(User user, string link)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                                <table align='center'>
                                    ");
            {
                sb.AppendFormat(@"<tr>
                                        <th>FullName</th>
                                        <th><input type='text' value='{0}'></th>
                                    </tr>
                                    <tr>
                                        <th>Proof</th>
                                        <th><a href='{1}' target='_blank'>{2}</a></th>
                                     </tr>", user.Fullname, link, user.Proof.FileName);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
