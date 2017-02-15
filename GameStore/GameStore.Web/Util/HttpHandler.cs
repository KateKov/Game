using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GameStore.BLL.Interfaces;

namespace GameStore.Web.Util
{
    public class HttpHandler: HttpTaskAsyncHandler
        {
        public override async Task ProcessRequestAsync(HttpContext context)
        {
            string url = context.Request.Url.Segments.Last();
            var service = DependencyResolver.Current.GetService<IGameService>();
            var picturePath = await Task.Run(() => service.GetByKey(url.Split('.')[0]).FilePath);
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var imagePath = $"{path}Content\\images\\{picturePath}";
            var picture= Image.FromFile(imagePath);
            var stream = new MemoryStream();
            picture.Save(stream, ImageFormat.Jpeg);
            var bytearr = stream.GetBuffer();
            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(bytearr);
        }

        public override bool IsReusable
        {
            get { return true; }
        }

        public override void ProcessRequest(HttpContext context)
        {
            throw new Exception("The ProcessRequest method has no implementation.");
        }
    }
}
    
