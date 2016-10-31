using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GameStore.Tests.Routes
{
    public class StubHttpContextForRouting : HttpContextBase
    {
        readonly StubHttpRequestForRouting _request;
        readonly StubHttpResponseForRouting _response;

        public StubHttpContextForRouting(string appPath = "/", string requestUrl = "~/", string httpMethod = "get")
        {
            _request = new StubHttpRequestForRouting(appPath, requestUrl, httpMethod);
            _response = new StubHttpResponseForRouting();
        }

        public override HttpRequestBase Request => _request;

        public override HttpResponseBase Response => _response;
    }
}
