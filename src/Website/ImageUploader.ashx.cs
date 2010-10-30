using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website
{
    /// <summary>
    /// Summary description for ImageUploader
    /// </summary>
    public class ImageUploader : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                HttpPostedFile file = context.Request.Files["Filedata"];

                string planId = context.Request["PlanId"].ToString();
                if (!System.IO.Directory.Exists(context.Request.PhysicalApplicationPath + "\\Resources\\Images\\SitePhotos\\PlanId_" + planId))
                    System.IO.Directory.CreateDirectory(context.Request.PhysicalApplicationPath +
                                                        "\\Resources\\Images\\SitePhotos\\PlanId_" + planId);

                if(context.Request["sessionId"] == null)
                    file.SaveAs(context.Request.PhysicalApplicationPath + "\\Resources\\Images\\SitePhotos\\PlanId_" + planId + "\\UA_" + Guid.NewGuid() + "_" +  file.FileName);
                else
                {
                    file.SaveAs(context.Request.PhysicalApplicationPath + "\\Resources\\Images\\SitePhotos\\PlanId_" + planId + "\\CA_" + context.Request["sessionId"].ToString() + "_" + Guid.NewGuid() + "_" + file.FileName);
                }
                context.Response.Write("1");
                
            }
            catch (Exception ex)
            {
                context.Response.Write("0");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}