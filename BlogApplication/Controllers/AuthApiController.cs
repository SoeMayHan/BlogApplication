using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BlogApplication.Common;
using BlogApplication.Data;
using BlogApplication.Models;
using BlogApplication.IAM;
using System.Collections;
namespace BlogApplication.Controllers
{
    public class AuthApiController : ApiController
    {
        JWTAuthenication jwtAuth = new JWTAuthenication();
        [RestAuthorization(Roles = "USER")]
        [HttpGet]
        [Route("user")]
        public BaseRestApiInterface getLoggedOnUser()
        {
            //deprecated with JWT flow
            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) => {
                string userID = this.RequestContext.Principal.Identity.Name;
                var query = from u in ctx.User where u.UserId == userID select u;
                if (query.Count() == 0)
                {
                    throw new BlogException("userNotFound", new string[] { "" });
                }
                return query.First();
            });
            return result;
        }
        [HttpPost]
        [Route("user/login")]
        public BaseRestApiInterface doLogin([FromBody] User oldUser)
        {
            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) => {

                var query = (from u in ctx.User where u.UserId.ToLower() == oldUser.UserId.ToLower() && u.Password == oldUser.Password select u).ToList();
                if (query.Count == 0)
                {
                    throw new BlogException("userNotFound", new string[] { oldUser.UserId });
                }
                else
                {
                    User u = query.First();
                    string token = jwtAuth.CreateAccessToken(u.UserName, u.UserId, u.UserRole);
                    string refresh_token = jwtAuth.CreateRefreshToken(u.UserId);
                    JWTAuthenticationToken tokens = new JWTAuthenticationToken();
                    tokens.Token = token;
                    tokens.RefreshToken = refresh_token;
                    tokens.User = u;
                    return tokens;
                }
            });
            return result;
        }
        [HttpPost]
        [Route("user/validate_login")]
        public BaseRestApiInterface validateLogin([FromBody] JWTAuthenticationToken oldTokens)
        {
            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) => {
                var token = jwtAuth.ValidateToken(oldTokens.Token);
                if (token == null)
                {
                    return jwtAuth.GetNewAccessTokenFromRefreshToken(oldTokens.RefreshToken, ctx);
                }
                else
                {
                    oldTokens.User = jwtAuth.GetUserFromAccessToken(token, true);
                }
                return oldTokens;
            });
            return result;
        }

       
    }
}