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

            var sourceNodes = _entityLogic.GetAll(c => c.Scheme);
            return Request.CreateResponse(HttpStatusCode.OK, sourceNodes);
        }

        // GET api/<controller>/5
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/SourceNode")]
        public HttpResponseMessage GetSourceNode([FromUri]int id)
        {

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

            _entityLogic.Insert(sourceNode);
            _entityLogic.Save();

            var getSourceNode = _entityLogic.GetSingle(c => c.Id == sourceNode.Id, c => c.Scheme);
            await _entityLogic.Pusher(getSourceNode, "sourcenode");

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