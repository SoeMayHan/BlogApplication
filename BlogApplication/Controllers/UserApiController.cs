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
        [Route("addUser")]
        public BaseRestApiInterface addUser([FromBody] User newUser)
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Scheme), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });

            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) =>
            {
                var oldUser = ctx.User.Where(u => u.UserId == newUser.UserId).SingleOrDefault();
                if (oldUser == null)
                {
                    var nu = ctx.User.Add(newUser);
                    ctx.SaveChanges();
                    return new ArrayList() { nu };
                }
                else
                {
                    throw new BlogException("userExists", new string[] { newUser.UserId });
                }
            });
            return result;
        }
        
        //update user
        [RestAuthorization(Roles = "ADMIN")]
        [HttpPost]
        [Route("updateUser")]
        public BaseRestApiInterface updateUser([FromBody] User updateUser)
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Scheme), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });

            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) =>
            {
                var oldUser = ctx.User.Where(u => u.UserId == updateUser.UserId).SingleOrDefault();
                if (oldUser != null)
                {
                    oldUser.UserEmail = updateUser.UserEmail;
                    oldUser.UserName = updateUser.UserName;
                    oldUser.UserRole = updateUser.UserRole;
                    ctx.SaveChanges();
                }
                else
                {
                    throw new BlogException("userNotFound", new string[] { updateUser.UserId });
                }

            });
            return result;
        }

        [RestAuthorization(Roles = "EDITOR")]
        [HttpGet]
        [Route("getUser/{id}")]
        public BaseRestApiInterface getUsers(string id)
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Scheme), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });

            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) =>
            {
                var query = from u in ctx.User
                            where (u.UserId == id)
                            select u;
                return query.OrderBy((item) => item.UserId).ToList();
            });
            return result;
        }
        
        [RestAuthorization(Roles = "ADMIN")]
        [HttpGet]
        [Route("getAllUsers")]
        public BaseRestApiInterface getAllUsers()
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Scheme), false);
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