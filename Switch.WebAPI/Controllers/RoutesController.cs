using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using PusherServer;
using Switch.WebAPI.Logics;
using Switch.WebAPI.Models;

namespace Switch.WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class RoutesController : ApiController
    {
        private ApplicationDbContext _context;
        private EntityLogic<Route> _entityLogic;
        public RoutesController()
        {
            _context = new ApplicationDbContext();
            _entityLogic = new EntityLogic<Route>();
        }

        // GET api/<controller>
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Routes")]
        public HttpResponseMessage GetRoutes()
        {
            var routes = _entityLogic.GetAll(c => c.SinkNode);

            return Request.CreateResponse(HttpStatusCode.OK, routes);
        }

        // GET api/<controller>/5
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Route")]
        public HttpResponseMessage GetRoute([FromUri]int id)
        {
            var route = _entityLogic.GetSingle(c => c.Id == id, c => c.SinkNode);

            return Request.CreateResponse(HttpStatusCode.OK, route);
        }

        // POST api/<controller>
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/CreateRoute")]
        public async Task<HttpResponseMessage> CreateScheme(Route route)
        {
            var routeName = _entityLogic.GetSingle(c => c.Name == route.Name);
            if (routeName != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, StatusCodes.NAME_ALREADY_EXIST);
            }
            route.Id = 0;
            _entityLogic.Insert(route);
            _entityLogic.Save();

            var getRoutes = _entityLogic.GetSingle(c => c.Id == route.Id, c => c.SinkNode);
            await _entityLogic.Pusher(getRoutes, "route");

            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.CREATED);

        }

        // PUT api/<controller>/5
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/UpdateRoute")]
        public HttpResponseMessage UpdateRoute(Route route)

        {
            try
            {

                _entityLogic.Update(route);
                return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.UPDATED);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);

            }

           
            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.UPDATED);
        }
        // DELETE api/<controller>/5
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/DeleteRoute")]
        public HttpResponseMessage DeleteRoute([FromUri]int id)
        {

            var route = _entityLogic.GetSingle(c => c.Id == id);
            _entityLogic.Delete(route);
            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.DELETED);
        }
    }
}