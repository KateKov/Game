using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces.Services;
using GameStore.Web.Infrastructure.AuthorizeAttribute;
using GameStore.Web.Interfaces;
using GameStore.Web.ViewModels.Orders;

namespace GameStore.Web.Controllers.api
{
    [RoutePrefix("api/orders")]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service, IAuthenticationManager authentication) 
            : base(authentication)
        {
            _service = service;
        }

        [Route("{id}")]
        [HttpGet]
        public OrderViewModel Get(string id)
        {
            OrderDTO orderDto = _service.GetById(id);
            var orderViewModel = Mapper.Map<OrderViewModel>(orderDto);
            return orderViewModel;
        }

        [ApiUserAuthorize]
        [Route]
        [HttpPost]
        public HttpResponseMessage Post([FromBody]OrderDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var orderDetails = Mapper.Map<OrderDetailDTO>(model);
            _service.AddOrder(orderDetails, User.Identity.Name, false);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [ApiUserAuthorize]
        [Route("{id}")]
        [HttpPut]
        public HttpResponseMessage Put(string id, [FromBody]OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var orderDetail = Mapper.Map<OrderDetailDTO>(model);
            _service.UpdateOrder(User.Identity.Name, orderDetail, false);
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}
