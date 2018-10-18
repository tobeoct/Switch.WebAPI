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
    public class SchemesController : ApiController
    {
        private ApplicationDbContext _context;
        public SchemesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET api/<controller>
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Schemes")]
        public HttpResponseMessage GetSchemes()
        {
            var schemes = _context.Schemes.ToList();

            return Request.CreateResponse(HttpStatusCode.OK, schemes);
        }

        // GET api/<controller>/5
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Scheme")]
        public HttpResponseMessage GetScheme([FromUri]int id)
        {
            var scheme = _context.Schemes.Include(c=>c.TransactionType).Include(c=>c.Channel).Include(c=>c.Route).Include(c=>c.Fee).SingleOrDefault(c => c.Id == id);

            return Request.CreateResponse(HttpStatusCode.OK, scheme);
        }
        
        // POST api/<controller>
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/CreateScheme")]
        public async Task<HttpResponseMessage> CreateScheme(Scheme scheme)
        {
            var schemeName = _context.Schemes.SingleOrDefault(c => c.Name == scheme.Name);
            if (schemeName != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, StatusCodes.NAME_ALREADY_EXIST);
            }
            scheme.Id = 0;

            Console.WriteLine("Posted Scheme : " + scheme);
            var schemeInDb = new Scheme()
            {
                Name = scheme.Name,
                RouteId = scheme.RouteId,
                ChannelId = scheme.ChannelId,
                TransactionTypeId = scheme.TransactionTypeId,
                FeeId = scheme.FeeId,
                Description = scheme.Description

            };

            _context.Schemes.Add(schemeInDb);
            _context.SaveChanges();

            var options = new PusherOptions
            {
                Cluster = "mt1",
                Encrypted = true
            };

            var getSchemes = _context.Schemes.Include(c => c.Route).Include(c => c.TransactionType)
                .Include(c => c.Channel).Include(c => c.Fee).SingleOrDefault(c => c.Id == schemeInDb.Id);
            var pusher = new Pusher(
                "619556",
                "1e8d9229f9b58c374f76",
                "d3f1b6b70b528626fbef",
                options);

            var result = await pusher.TriggerAsync(
                "my-schemes",
                "new-scheme",
               data: new
               {
                   Id = getSchemes.Id,
                   Name = getSchemes.Name,
                   RouteId = getSchemes.RouteId,
                   Route = getSchemes.Route,
                   ChannelId = getSchemes.ChannelId,
                   Channel= getSchemes.Channel,
                   TransactionTypeId = getSchemes.TransactionTypeId,
                   TransactionType = getSchemes.TransactionType,
                   FeeId= getSchemes.FeeId,
                   Fee = getSchemes.Fee,
                   Description= getSchemes.Description

               });

            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.CREATED);

        }

        // PUT api/<controller>/5
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/UpdateScheme")]
        public HttpResponseMessage UpdateScheme(Scheme scheme)

        {
            var schemeInDb = _context.Schemes.SingleOrDefault(c => c.Id == scheme.Id);
           
            schemeInDb.Name = scheme.Name;
            schemeInDb.ChannelId = scheme.ChannelId;
            schemeInDb.FeeId = scheme.FeeId;
            schemeInDb.RouteId = scheme.RouteId;
            schemeInDb.TransactionTypeId = scheme.TransactionTypeId;
            schemeInDb.Description = scheme.Description;
            
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.UPDATED);
        }

        // DELETE api/<controller>/5
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/DeleteScheme")]
        public HttpResponseMessage DeleteScheme([FromUri]int id)
        {

            var scheme = _context.Schemes.SingleOrDefault(c => c.Id == id);
          
            var deleteScheme = new EntityLogic<Scheme>();
            deleteScheme.Delete(scheme);
            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.DELETED);
        }
    }
}