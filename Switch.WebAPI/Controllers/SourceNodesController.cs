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
    public class SourceNodesController : ApiController
    {
        private ApplicationDbContext _context;
        private EntityLogic<SourceNode> _entityLogic;
        public SourceNodesController()
        {
            _context = new ApplicationDbContext();
            _entityLogic = new EntityLogic<SourceNode>();
        }

        // GET api/<controller>
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/SourceNodes")]
        public HttpResponseMessage GetSourceNodes()
        {
            //            var sourceNodes = _context.SourceNodes.Include(c=>c.Scheme).ToList();
            //            var sourceNodes = new EntityLogic<SourceNode>().GetList();
            var sourceNodes = _entityLogic.GetAll(c => c.Scheme);
            return Request.CreateResponse(HttpStatusCode.OK, sourceNodes);
        }

        // GET api/<controller>/5
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/SourceNode")]
        public HttpResponseMessage GetSourceNode([FromUri]int id)
        {
            //            var sourceNode = _context.SourceNodes.Include(c=>c.Scheme).SingleOrDefault(c => c.Id == id);
            var sourceNode = _entityLogic.GetSingle(c => c.Id == id);
            return Request.CreateResponse(HttpStatusCode.OK, sourceNode);
        }


        // POST api/<controller>
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/CreateSourceNode")]
        public async Task<HttpResponseMessage> CreateSourceNode(SourceNode sourceNode)
        {
            SourceNode sourceName = _entityLogic.GetSingle(c => c.Name == sourceNode.Name);
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

            _entityLogic.Insert(sourceNode);
            _entityLogic.Save();
           

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
            var getSourceNode = new EntityLogic<SourceNode>().GetSingle(c => c.Id == sourceNodeInDb.Id, c => c.Scheme);
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
                        SchemeName = schemeName,

                        Status = getSourceNode.Status


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

            try
            {
                _entityLogic.Update(sourceNode);
                return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.UPDATED);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, StatusCodes.ERROR_DOESNT_EXIST);

        }

        // DELETE api/<controller>/5
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/DeleteSourceNode")]
        public HttpResponseMessage DeleteSourceNode([FromUri]int id)
        {

            var sourceNode = _entityLogic.GetSingle(c => c.Id == id);
            _entityLogic.Delete(sourceNode);
            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.DELETED);
        }
    }
}