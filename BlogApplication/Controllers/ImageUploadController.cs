using BlogApplication.Common;
using BlogApplication.IAM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BlogApplication.Controllers
{
    [RoutePrefix("api/v1/imageupload")]
    public class ImageUploadController : ApiController
    {
        JWTAuthenication jwtAuth = new JWTAuthenication();
        //user admin only
        //add user 
        [RestAuthorization(Roles = "ADMIN")]
        [HttpPost]
        [Route("PostUserImage")]
        public BaseRestApiInterface PostUserImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            BaseRestApiResult result = new BaseRestApiResult();
            
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count < 1)
                {
                    result.Message = string.Format("Please Upload a image.");
                    result.Status = StatusType.FAILED;
                }
                else
                {
                    foreach (string file in httpRequest.Files)
                    {
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                        var postedFile = httpRequest.Files[file];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB  

                            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                            var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {
                                throw new BlogException("allowedFileExtensions", new String[] { extension.ToString() });
                            }
                            else if (postedFile.ContentLength > MaxContentLength)
                            {
                                throw new BlogException("allowedFileSize", new String[] { postedFile.ContentLength.ToString() });
                            }
                            else
                            {
                                var filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + postedFile.FileName);
                                postedFile.SaveAs(filePath);
                                result.Message = string.Format(filePath);
                                result.Status = StatusType.SUCCESS;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = string.Format("Error " + ex.Message);
                result.Status = StatusType.FAILED;
            }

            return result;
        }
    }
}