using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Tests.Helpers
{
    public static class ControllerExtensions
    {
        public static void SetUserIsAuthenticated(this Controller controller, bool isAuthenticated)
        {
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(c => c.User.Identity!.IsAuthenticated).Returns(isAuthenticated);

            controller.ControllerContext = new ControllerContext { HttpContext= mockHttpContext.Object };

        }
        
        public static void SetAjaxRequest(this Controller controller, bool isAjax)
        {
            var mockHttpContext = new Mock<HttpContext>();

            if(isAjax)
                  mockHttpContext.SetupGet(c => c.Request.Headers["X-Requested-With"]).Returns("XMLHttpRequest");
            else
                  mockHttpContext.SetupGet(c => c.Request.Headers["X-Requested-With"]).Returns("");

            controller.ControllerContext = new ControllerContext { HttpContext= mockHttpContext.Object };
        }
    }
}
