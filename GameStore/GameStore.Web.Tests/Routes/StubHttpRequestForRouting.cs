using System;
using System.Collections.Specialized;
using System.Web;

namespace GameStore.Web.Tests.Routes
{
    public class StubHttpRequestForRouting : HttpRequestBase
    {
        public StubHttpRequestForRouting(string appPath, string requestUrl, string httpMethod)
        {
            ApplicationPath = appPath;
            AppRelativeCurrentExecutionFilePath = requestUrl;
            HttpMethod = httpMethod;
        }

        public override string HttpMethod { get; }

        public override string ApplicationPath { get; }

        public override string AppRelativeCurrentExecutionFilePath { get; }

        public override string PathInfo => string.Empty;

        public override NameValueCollection ServerVariables => new NameValueCollection();
    }
}
