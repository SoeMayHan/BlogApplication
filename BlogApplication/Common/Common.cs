using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogApplication.Common
{
    public enum PostStatus
    {
        Draft = 0,
        ReadToPublish = 1,
        Reject = 2,
        Published = 3,
        Archived = 4
    }

    public enum UserRole
    {
        ADMIN,
        EDITOR,
        USER
    }

    public static class Common
    {
        public static String GetMessage(String type, object[] values)
        {
            try
            {
                String message = "";

                switch (type)
                {
                    case "tokenExpired":
                        message = "User token has expired";
                        break;
                    case "tokenInvalid":
                        message = "Invalid Refresh Token {0} ";
                        break;
                    case "allowedFileExtensions":
                        message = "Please Upload image of type .jpg,.gif,.png.";
                        break;
                    case "allowedFileSize":
                        message = "Please Upload a file upto 5 MB.";
                        break;
                    case "userNotAccessRight":
                        message = "User {0} do not have to perform for the action.";
                        break;
                    case "userNotFound":
                        message = "User {0} was not found";
                        break;
                    case "userExists":
                        message = "User {0} already exists.";
                        break;
                    case "blogpostExists":
                        message = "Blogpost {0} already exists.";
                        break;
                    case "blogpostnotExists":
                        message = "Blogpost {0} don't have exist.";
                        break;
                    default:
                        message = "";
                        break;
                }
                return String.Format(message, values);
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
    
}