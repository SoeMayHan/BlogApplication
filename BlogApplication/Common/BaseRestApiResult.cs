using BlogApplication.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace BlogApplication.Common
{
    public class BaseRestApiResult : BaseRestApiInterface
    {
        public BaseRestApiResult()
        {
            Results = new ArrayList();
        }
        public string Message
        {
            get; set;
        }

        public IList Results
        {
            get; set;
        }

        public StatusType Status
        {
            get; set;
        }

        public void process(Func<ApplicationDbContext, IList> functionality)
        {
            if (functionality != null)
            {
                Status = StatusType.SUCCESS;
                try
                {
                    using (var ctx = new ApplicationDbContext())
                    {
                        IList retval = functionality.Invoke(ctx);
                        if (retval != null)
                        {
                            Results = retval;
                        }
                    }
                }
                catch (DbEntityValidationException entityValidationError)
                {
                    Status = StatusType.FAILED;
                    Message = String.Join(";", entityValidationError.EntityValidationErrors.SelectMany((x) => x.ValidationErrors).Select((x) => x.ErrorMessage));
                }
                catch (Exception randomException)
                {
                    Status = StatusType.FAILED;
                    Message = randomException.Message;
                }
            }
            else
            {
                throw new Exception("Processing action cannot be null");
            }
        }

        public void process(Func<ApplicationDbContext, Object> functionality)
        {
            if (functionality != null)
            {
                Status = StatusType.SUCCESS;
                try
                {
                    using (var ctx = new ApplicationDbContext())
                    {

                        Object retval = functionality.Invoke(ctx);
                        if (retval != null)
                        {
                            Results.Add(retval);
                        }
                    }
                }
                catch (DbEntityValidationException entityValidationError)
                {
                    Status = StatusType.FAILED;
                    Message = String.Join(";", entityValidationError.EntityValidationErrors.SelectMany((x) => x.ValidationErrors).Select((x) => x.ErrorMessage));
                }
                catch (Exception randomException)
                {
                    Status = StatusType.FAILED;
                    Message = randomException.Message;
                }
            }
            else
            {
                throw new Exception("Processing action cannot be null");
            }
        }

        public void process(Action<ApplicationDbContext> functionality)
        {
            if (functionality != null)
            {
                Status = StatusType.SUCCESS;
                try
                {
                    using (var ctx = new ApplicationDbContext())
                    {
                        functionality.Invoke(ctx);
                    }
                }
                catch (DbEntityValidationException entityValidationError)
                {
                    Status = StatusType.FAILED;
                    Message = String.Join(";", entityValidationError.EntityValidationErrors.SelectMany((x) => x.ValidationErrors).Select((x) => x.ErrorMessage));
                }
                catch (Exception randomException)
                {
                    Status = StatusType.FAILED;
                    Message = randomException.Message;
                }
            }
            else
            {
                throw new Exception("Processing action cannot be null");
            }
        }
    }
}