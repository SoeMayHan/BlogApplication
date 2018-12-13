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
    [RoutePrefix("api/v1/user")]
    public class UserApiController : ApiController
    {
        JWTAuthenication jwtAuth = new JWTAuthenication();
        //user admin only
        //add user 
        [RestAuthorization(Roles = "ADMIN")]
        [HttpPost]
        [Route("")]
        public BaseRestApiInterface addUser([FromBody] User newUser)
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Parameter), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });

            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) =>
            {
                var oldUser = ctx.User.Find(new string[] { newUser.UserId });
                if (oldUser == null)
                {
                    var nu = ctx.User.Add(newUser);
                    ctx.SaveChanges();
                    return new ArrayList() { nu };
                }
                else
                {
                    throw new BlogException("userExists", new string[] { user.UserId });
                }
            });
            return result;
        }
        
        //update user
        [RestAuthorization(Roles = "ADMIN")]
        [HttpPut]
        [Route("")]
        public BaseRestApiInterface updateUser([FromBody] User updateUser)
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Parameter), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });

            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) =>
            {
                var oldUser = ctx.User.Find(new string[] { updateUser.UserId });
                if (oldUser != null)
                {
                    oldUser.UserEmail = updateUser.UserEmail;
                    oldUser.UserName = updateUser.UserName;
                    oldUser.UserRole = updateUser.UserRole;
                    ctx.SaveChanges();
                }
                else
                {
                    throw new BlogException("userNotFound", new string[] { user.UserId });
                }

            });
            return result;
        }

        //get list of users
        [RestAuthorization(Roles = "EDITOR")]
        [HttpPost]
        [Route("getUser")]
        public BaseRestApiInterface getUsers([FromBody] User tempUser)
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Parameter), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });

            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) =>
            {
                var query = from u in ctx.User
                            where (u.UserName.Contains(tempUser.UserName)) || (u.UserId.Contains(tempUser.UserId))
                            select u;
                return query.OrderBy((item) => item.UserId).ToList();
            });
            return result;
        }
        
        [RestAuthorization(Roles = "ADMIN")]
        [HttpGet]
        [Route("")]
        public BaseRestApiInterface getAllUsers()
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Parameter), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });

            string s = this.RequestContext.Principal.Identity.Name;

            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) =>
            {
                return ctx.User.OrderBy((item) => item.UserId).ToList();
            });
            return result;
        }
    }
}