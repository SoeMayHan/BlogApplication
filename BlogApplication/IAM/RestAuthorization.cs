using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using BlogApplication.Models;
using BlogApplication.Common;

namespace BlogApplication.IAM
{
    public class RestAuthorization : AuthorizeAttribute
    {
        JWTAuthenication jwtAuth = new JWTAuthenication();
        bool isAuthError = false;
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
            string err = isAuthError ? "Authentication failed. Please login again and try performing the requested operation." : "You are not authorized to perform this operation";
            var response = actionContext.Request.CreateErrorResponse(isAuthError ? System.Net.HttpStatusCode.Unauthorized : System.Net.HttpStatusCode.Forbidden, err);
            actionContext.Response = response;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            isAuthError = false;

            User validUser = null;

            if (actionContext.Request.Headers.Contains("Authorization"))
            {
                string jwt = actionContext.Request.Headers.Authorization.Scheme;
                var token = jwtAuth.ValidateToken(jwt);
                if (token != null)
                {
                    validUser = jwtAuth.GetUserFromAccessToken(token, false);
                }
            }
            if (validUser == null)
            {
                isAuthError = true;
                return false;
            }
            foreach (string role in this.Roles.Split(",".ToCharArray()))
            {
                if (hasAccess(role, validUser.UserRole.ToUpper()))
                    return true;
            }
            return false;
        }
        public bool hasAccess(string role, string UserRole)
        {
            bool retval = false;
            switch (role)
            {
                case "ADMIN":
                    {
                        retval = (UserRole == role);//only ADMIN can access
                    }
                    break;
                case "EDITOR":
                    {
                        retval = (UserRole == "ADMIN" || UserRole == "EDITOR");//non User roles can access
                    }
                    break;
                case "USER":
                    {
                        retval = (UserRole == "USER" || UserRole == "ADMIN" || UserRole == "EDITOR");//any and every type of User can access
                    }
                    break;
            }
            return retval;
        }
    }
}