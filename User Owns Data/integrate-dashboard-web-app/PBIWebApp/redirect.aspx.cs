﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace PBIWebApp
{
    /* NOTE: This sample is to illustrate how to authenticate a Power BI web app. 
    * In a production application, you should provide appropriate exception handling and refactor authentication settings into 
    * a configuration. Authentication settings are hard-coded in the sample to make it easier to follow the flow of authentication. */

    public partial class Redirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect uri must match the redirect_uri used when requesting Authorization code.
            string redirectUri = String.Format("{0}Redirect", Properties.Settings.Default.RedirectUrl);
            string authorityUri = Properties.Settings.Default.AADAuthorityUri;

            // Get the auth code
            string code = Request.Params["code"];

            if (code != null)
            {
                // Get auth token from auth code       
                TokenCache TC = new TokenCache();

                AuthenticationContext AC = new AuthenticationContext(authorityUri, TC);
                ClientCredential cc = new ClientCredential
                    (Properties.Settings.Default.ClientID,
                    Properties.Settings.Default.ClientSecret);

                AuthenticationResult AR = AC.AcquireTokenByAuthorizationCodeAsync(code, new Uri(redirectUri), cc).Result;

                //Set Session "authResult" index string to the AuthenticationResult
                Session[_Default.authResultString] = AR;
            }
            else
            {
                //Remove Session "authResult"
                Session[_Default.authResultString] = null;
            }
            //Redirect back to Default.aspx
            Response.Redirect("/Default.aspx");
        }
    }
}