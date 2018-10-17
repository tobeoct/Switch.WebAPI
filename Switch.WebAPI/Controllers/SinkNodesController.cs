using System;
using System.Collections.Generic;
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
    public class SinkNodesController : ApiController
    {
        private ApplicationDbContext _context;
        public SinkNodesController()
        {
            _context=new ApplicationDbContext();
        }

        // GET api/<controller>
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/SinkNodes")]
        public HttpResponseMessage GetSinkNodes()
        {
            var sinkNodes = _context.SinkNodes.ToList();

            return Request.CreateResponse(HttpStatusCode.OK,sinkNodes);
        }

        //GET api/<controller>/5
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/SinkNode")]
        public HttpResponseMessage GetSinkNode([FromUri]int id)
        {
            var sinkNode = _context.SinkNodes.SingleOrDefault(c=>c.Id==id);

            return Request.CreateResponse(HttpStatusCode.OK, sinkNode);
        }
        

        // POST api/<controller>
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/CreateSinkNode")]
        public async Task<HttpResponseMessage> CreateSinkNode(SinkNode sinkNode)
        {
            var sinkName = _context.SinkNodes.SingleOrDefault(c => c.Name == sinkNode.Name);
            if (sinkName != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, StatusCodes.NAME_ALREADY_EXIST);
            }
            sinkNode.Id = 0;
            if (sinkNode.Status == "0")
            {
                sinkNode.Status = StatusCodes.ACTIVE;
            }
            else
            {
                sinkNode.Status = StatusCodes.INACTIVE;
            }
            Console.WriteLine("Posted SinkNode : "+sinkNode);
            var sinkNodeInDb = new SinkNode()
            {
                Name = sinkNode.Name,
                HostName = sinkNode.HostName,
                Port = sinkNode.Port,
                IPAddress = sinkNode.IPAddress,
                Status = sinkNode.Status

            };

            _context.SinkNodes.Add(sinkNodeInDb);
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

            var result = await pusher.TriggerAsync(
                "my-sinknodes",
                "new-sinknode",               
               data: new
                {
                    Id  = sinkNodeInDb.Id,
                    Name = sinkNode.Name,
                    HostName=sinkNode.HostName,
                    IPAddress = sinkNode.IPAddress,
                    Port = sinkNode.Port,
                    Status = sinkNode.Status
                });
           
            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.CREATED);

        }

        // PUT api/<controller>/5
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/UpdateSinkNode")]
        public async Task<HttpResponseMessage> UpdateSinkNode(SinkNode sinkNode)

        {
            var sinkNodeInDb = _context.SinkNodes.SingleOrDefault(c => c.Id == sinkNode.Id);
            if (sinkNode.Status == "0")
            {
                sinkNode.Status = StatusCodes.ACTIVE;
            }
            else
            {
                sinkNode.Status = StatusCodes.INACTIVE;
            }
            sinkNodeInDb.Name = sinkNode.Name;
            sinkNodeInDb.HostName = sinkNode.HostName;
            sinkNodeInDb.IPAddress = sinkNode.IPAddress;
            sinkNodeInDb.Port = sinkNode.Port;
            sinkNodeInDb.Status = sinkNode.Status;
            _context.SaveChanges();
//            var options = new PusherOptions
//            {
//                Cluster = "mt1",
//                Encrypted = true
//            };
//
//            var pusher = new Pusher(
//                "619556",
//                "1e8d9229f9b58c374f76",
//                "d3f1b6b70b528626fbef",
//                options);
//
//            var result = await pusher.TriggerAsync(
//                "my-sinknodes",
//                "new-sinknode",
//                data: new
//                {
//                    Id = sinkNode.Id,
//                    Name = sinkNode.Name,
//                    HostName = sinkNode.HostName,
//                    IPAddress = sinkNode.IPAddress,
//                    Port = sinkNode.Port,
//                    Status = sinkNode.Status
//                });
            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.UPDATED);
        }

        // DELETE api/<controller>/5
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/DeleteSinkNode")]
        public HttpResponseMessage DeleteSinkNode([FromUri]int id)
        {

            var sinkNode = _context.SinkNodes.SingleOrDefault(c => c.Id == id);
            _context.SinkNodes.Remove(sinkNode);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.DELETED);
        }
    }
}