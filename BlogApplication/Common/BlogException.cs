using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogApplication.Common
{
    public class BlogException : Exception
    {
        public BlogException(String templateName, String[] formatObjects) 
            : base(Common.GetMessage(templateName, formatObjects))
        {

        }
    }
}