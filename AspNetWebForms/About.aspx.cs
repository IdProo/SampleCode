using System;
using System.Web.UI;
using System.Web.UI.WebControls; 
using System.Linq;
using IdentityModel.Client;
using System.Configuration;
using System.Threading.Tasks;

namespace AspNetWebForms
{
    public partial class About : Page
    { 
        protected void Page_Load(object sender, EventArgs e)
        {
	        string labelText =""; 
	        if (Request.IsAuthenticated)
            {
                var identity = (System.Security.Claims.ClaimsIdentity)Context.User.Identity;
                int no = 0;
                labelText = "[";
                foreach (var c in identity.Claims)
                {
                    labelText += (no > 0) ? ",{\"" + c.Type + "\":\"" + c.Value + "\"}" : "{\"" + c.Type + "\":\"" + c.Value + "\"}";
                    no++;
                }

                labelText += "]"; 
               // labelText = identity.Claims.FirstOrDefault(x => x.Type.Equals("access_token")).Value;
            }
	        else
	        {
		        labelText = "You are not authenticated!";
	        }


            var label = new Label
	        {
				Text = labelText
            };

	        var mainContent = (ContentPlaceHolder)Page.Form.FindControl("MainContent");
			mainContent.Controls.Add(label);
        }
    }
}