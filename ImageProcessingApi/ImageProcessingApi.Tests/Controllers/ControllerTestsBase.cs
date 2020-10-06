using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace ImageProcessingApi.Tests.Controllers
{
    public class ControllerTestsBase<T> where T : ControllerBase
    {
        protected T _controller;
        protected HttpResponse _response;

        public void SetupContext()
        {
            _response = Substitute.For<HttpResponse>();
            var context = Substitute.For<HttpContext>();
            context.Response.Returns(_response);

            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = context;

            _controller.ControllerContext = controllerContext;
        }
    }
}
