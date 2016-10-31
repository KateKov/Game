using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GameStore.Tests.Routes
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

        public override string PathInfo => "";

        public override NameValueCollection ServerVariables => new NameValueCollection();
    }
}
