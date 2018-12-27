using System;
using System.Collections;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BlogApplication.Common;
using BlogApplication.Data;
using BlogApplication.Models;
using BlogApplication.IAM;

namespace BlogApplication.Controllers
{
    [RestAuthorization(Roles = "EDITOR")]
    [RoutePrefix("api/v1/blogposts")]
    public class BlogPostsApiController : ApiController
    {
        JWTAuthenication jwtAuth = new JWTAuthenication();

        [HttpGet]
        [Route("getBlogPosts")]
        public BaseRestApiInterface GetBlogPosts()
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Scheme), false);
            if(user == null)
                    throw new BlogException("userNotAccessRight", new String[] { user.UserId });
            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) =>
            {
                return ctx.BlogPosts.OrderBy((item) => item.BlogId).ToList();
            });

            return result;
        }

        [HttpGet]
        [Route("getBlogPostsById/{id}")]
        public BaseRestApiInterface GetBlogPostById(int id)
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Scheme), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });
            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) =>
            {
                return (from blog in ctx.BlogPosts where blog.BlogId.Equals(id) select blog).FirstOrDefault();
            });

            return result;
        }

        [HttpPost]
        [Route("addBlogPost")]
        public BaseRestApiInterface AddBlogPosts([FromBody] BlogPost newblogPost)
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Scheme), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });
            BaseRestApiResult result = new BaseRestApiResult();
            newblogPost.CreatedDate = DateTime.Now;
            newblogPost.CreatedBy = user.UserId;
            newblogPost.ModifiedBy = user.UserId;
            newblogPost.ModifiedDate = DateTime.Now;
            newblogPost.Status = (int)PostStatus.Draft;
            result.process((ctx) =>
            {
                var oldblogPost = ctx.BlogPosts.Where(a => a.Title == newblogPost.Title).SingleOrDefault();
                if (oldblogPost == null)
                {
                    var newBlog = ctx.BlogPosts.Add(newblogPost);
                    ctx.SaveChanges();
                    return new ArrayList() { newBlog };
                }
                else
                {
                    throw new BlogException("blogpostExists", new string[] { newblogPost.Title });
                }
            });

            return result;
        }

        [HttpPost]
        [Route("updateBlogPost")]
        public BaseRestApiInterface EditBlogPosts([FromBody] BlogPost blogPost)
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Scheme), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });
            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) =>
            {
                var oldblogPost = ctx.BlogPosts.Where(a => a.Title == blogPost.Title).SingleOrDefault();

                if (oldblogPost == null)
                {
                    throw new BlogException("blogpostnotExists", new string[] { blogPost.Title });
                }
                else
                {
                    oldblogPost.Content = blogPost.Content;
                    oldblogPost.ImageUrl = blogPost.ImageUrl;
                    oldblogPost.ModifiedBy = user.UserName;
                    oldblogPost.ModifiedDate = DateTime.Now;
                    ctx.SaveChanges();

                }
            });

            return result;
        }

        [RestAuthorization(Roles = "ADMIN")]
        [HttpPost]
        [Route("publictoready/{blogId}")]
        public BaseRestApiInterface PublicToReady(int blogId)
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Scheme), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });
            BaseRestApiResult result = new BaseRestApiResult();
            result.process((ctx) =>
            {
                var blogPost = ctx.BlogPosts.Where(b => b.BlogId == blogId).SingleOrDefault();

                if (blogPost == null)
                {
                    throw new BlogException("blogpostnotExists", new string[] { blogPost.Title });
                }
                else
                {
                    blogPost.PublishToDate = DateTime.Now;
                    blogPost.Status = (int)PostStatus.ReadToPublish;
                    blogPost.ModifiedDate = DateTime.Now;
                    blogPost.ModifiedBy = user.UserName;
                    ctx.SaveChanges();
                }
            });

            return result;
        }

        [HttpPost]
        [Route("appliedstatus")]
        public BaseRestApiInterface AppliedStatus(int blogId, PostStatus pt)
        {
            User user = jwtAuth.GetUserFromAccessToken(jwtAuth.ValidateToken(Request.Headers.Authorization.Scheme), false);
            if (user == null)
                throw new BlogException("userNotAccessRight", new String[] { user.UserId });
            BaseRestApiResult result = new BaseRestApiResult();

            result.process((ctx) =>
            {
                var blogPost = ctx.BlogPosts.Find(new int[] { blogId });

                if (blogPost == null)
                {
                    throw new BlogException("blogpostnotExists", new string[] { blogPost.Title });
                }
                else
                {
                    switch (pt)
                    {
                        case PostStatus.Draft:
                            blogPost.Status = (int)PostStatus.Draft;
                            break;
                        case PostStatus.ReadToPublish:
                            blogPost.Status = (int)PostStatus.ReadToPublish;
                            break;
                        case PostStatus.Reject:
                            blogPost.Status = (int)PostStatus.Reject;
                            break;
                        case PostStatus.Published:
                            blogPost.Status = (int)PostStatus.Published;
                            break;
                        case PostStatus.Archived:
                            blogPost.Status = (int)PostStatus.Archived;
                            break;
                        default:
                            break;
                    }

                    blogPost.ModifiedDate = DateTime.Now;
                    blogPost.ModifiedBy = user.UserName;
                    ctx.SaveChanges();
                }
            });

            return result;
        }

    }
}
