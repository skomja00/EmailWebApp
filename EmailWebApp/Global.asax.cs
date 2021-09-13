using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace EmailWebApp
{
    public class Global : System.Web.HttpApplication
    {
        private ArrayList arrayListAvatar = new ArrayList
            (
                new string[] {
                        "adult-man-avatar-with-short-curly-hair-and-mustache.svg",
                        "female-avatar-image.svg",
                        "male-profile-avatar-without-face.svg",
                        "man-avatar-with-bald-head-sunglasses-and-mustache.svg",
                        "man-with-mustache-avatar.svg",
                        "man-with-short-hair-profile-avatar.svg",
                        "manager-avatar.svg",
                        "nerd-male-profile-avatar.svg",
                        "nerd-man-with-curly-hair-and-circular-eyeglasses-avatar.svg",
                        "profiles-avatar.svg",
                        "woman-avatar.svg",
                        "young-man-avatar.svg"}
            );

        protected void Application_Start(object sender, EventArgs e)
        {
            Application["arrayListAvatar"] = arrayListAvatar;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}