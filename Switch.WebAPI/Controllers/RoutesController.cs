using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            _entityLogic=new EntityLogic<Route>();
        }

        // GET api/<controller>
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Routes")]
        public HttpResponseMessage GetRoutes()
        {
            var routes = _entityLogic.GetAll(c=>c.SinkNode);

            return Request.CreateResponse(HttpStatusCode.OK, routes);
        }

        // GET api/<controller>/5
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Route")]
        public HttpResponseMessage GetRoute([FromUri]int id)
        {
            var route = _entityLogic.GetSingle(c=>c.Id==id,c=>c.SinkNode);

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

            
            var routeInDb = new Route()
            {
                Name = route.Name,
                CardPan = route.CardPan,
                SinkNodeId = route.SinkNodeId,
                Description = route.Description

            };

            _context.Routes.Add(routeInDb);
            _context.SaveChanges();

            var options = new PusherOptions
            {
                Cluster = "mt1",
                Encrypted = true
            };

            var getRoutes = _entityLogic.GetSingle(c => c.Id == routeInDb.Id, c => c.SinkNode);
            var pusher = new Pusher(
                "619556",
                "1e8d9229f9b58c374f76",
                "d3f1b6b70b528626fbef",
                options);

            if (getRoutes != null)
            {
                var result = await pusher.TriggerAsync(
                    "my-routes",
                    "new-route",
                    data: new
                    {
                        Id = getRoutes.Id,
                        Name = getRoutes.Name,
                        CardPan = getRoutes.CardPan,
                        SinkNodeName = getRoutes.SinkNode.Name,
                        Description = getRoutes.Description
                    });
            }

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
                Console.WriteLine(e);

            }

            _context.SaveChanges();
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