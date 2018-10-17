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
using Switch.WebAPI.Models;

namespace Switch.WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class SourceNodesController : ApiController
    {
        private ApplicationDbContext _context;
        public SourceNodesController()
        {
            _context = new ApplicationDbContext();
            
        }

        // GET api/<controller>
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/SourceNodes")]
        public HttpResponseMessage GetSourceNodes()
        {
            var sourceNodes = _context.SourceNodes.Include(c=>c.Scheme).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, sourceNodes);
        }

        // GET api/<controller>/5
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/SourceNode")]
        public HttpResponseMessage GetSourceNode([FromUri]int id)
        {
            var sourceNode = _context.SourceNodes.Include(c=>c.Scheme).SingleOrDefault(c => c.Id == id);

            return Request.CreateResponse(HttpStatusCode.OK, sourceNode);
        }

       
        // POST api/<controller>
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/CreateSourceNode")]
        public async Task<HttpResponseMessage> CreateSourceNode(SourceNode sourceNode)
        {
            var sourceName = _context.SourceNodes.SingleOrDefault(c => c.Name == sourceNode.Name);
            if (sourceName != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, StatusCodes.NAME_ALREADY_EXIST);
            }
            sourceNode.Id = 0;
            if (sourceNode.Status == "0")
            {
                sourceNode.Status = "Active";
            }
            else
            {
                sourceNode.Status = "In-active";
            }
            Console.WriteLine("Posted SourceNode : " + sourceNode);
            var sourceNodeInDb = new SourceNode()
            {
                Name = sourceNode.Name,
                HostName = sourceNode.HostName,
                Port = sourceNode.Port,
                IPAddress = sourceNode.IPAddress,
                Status = sourceNode.Status,
                SchemeId = sourceNode.SchemeId
            };

            _context.SourceNodes.Add(sourceNodeInDb);
            _context.SaveChanges();

            var options = new PusherOptions
            {
                Cluster = "mt1",
                Encrypted = true
            };

            var pusher = new Pusher(
                "619556",
                "1e8d9229f9b58c374f76",
                "d3f1b6b70b528626fbef",
                options);
            var getSourceNode = _context.SourceNodes.Include(c => c.Scheme)
                .SingleOrDefault(c => c.Id == sourceNodeInDb.Id);
            var schemeName = getSourceNode.Scheme.Name;
            if (getSourceNode != null)
            {
                var result = await pusher.TriggerAsync(
                    "my-sourcenodes",
                    "new-sourcenode",
                    data: new
                    {
                        Id = getSourceNode.Id,
                        Name = getSourceNode.Name,
                        HostName = getSourceNode.HostName,
                        IPAddress = getSourceNode.IPAddress,
                        Port = getSourceNode.Port,
                        Status = getSourceNode.Status,
                        
                        SchemeName = schemeName
                    });
            }
           
            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.CREATED);

        }

        // PUT api/<controller>/5
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/UpdateSourceNode")]
        public HttpResponseMessage UpdateSourceNode(SourceNode sourceNode)

        {
            var sourceInDb = _context.SourceNodes.SingleOrDefault(c => c.Id == sourceNode.Id);

            if (sourceInDb != null)
            {
                sourceInDb.Name = sourceNode.Name;
                sourceInDb.HostName = sourceNode.HostName;
                sourceInDb.IPAddress = sourceNode.IPAddress;
                sourceInDb.Port = sourceNode.Port;
                sourceInDb.Status = sourceNode.Status;
                sourceInDb.SchemeId = sourceNode.SchemeId;
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.UPDATED);
            }


            return Request.CreateErrorResponse(HttpStatusCode.OK, StatusCodes.ERROR_DOESNT_EXIST);

        }

        // DELETE api/<controller>/5
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/DeleteSourceNode")]
        public HttpResponseMessage DeleteSourceNode([FromUri]int id)
        {

            var sourceNode = _context.SourceNodes.SingleOrDefault(c => c.Id == id);
            _context.SourceNodes.Remove(sourceNode);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.DELETED);
        }
    }
}