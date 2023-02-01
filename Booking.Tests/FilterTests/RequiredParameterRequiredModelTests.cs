using Booking.Web.Controllers;
using Booking.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Tests.FilterTests
{
    
    public class RequiredParameterRequiredModelTests
    {

        [Fact]
        public void OnActionExecuting_ParameterHasNoValue_ShouldReturnNotFound()
        {
            ActionExecutingContext context = GetContext<object>("id", null);
            var filter = new RequiredParameterRequiredModel("id");

            filter.OnActionExecuting(context);

            var result = context.Result;

            Assert.IsType<NotFoundResult>(result);
        }

        private ActionExecutingContext GetContext<T>(string key, T? value)
        {
            var routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add(key, value);

            var routeData = new RouteData(routeValueDictionary);


            var actionContext = new ActionContext(Mock.Of<HttpContext>(),
                                                  routeData,
                                                  Mock.Of<ActionDescriptor>());

            var mockController = new Mock<GymClassesController>();

            var actionExcecutingContext = new ActionExecutingContext(actionContext,
                                                                    new List<IFilterMetadata>(),
                                                                    routeValueDictionary,
                                                                    mockController);

            return actionExcecutingContext;
                              
        }
    }
}
