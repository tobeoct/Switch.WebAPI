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
    public class SchemesController : ApiController
    {
      
        private EntityLogic<Scheme> _entityLogic;
        public SchemesController()
        {
           
            _entityLogic = new EntityLogic<Scheme>();
        }

        // GET api/<controller>
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Schemes")]
        public HttpResponseMessage GetSchemes()
        {
            var schemes = _entityLogic.GetList();

            return Request.CreateResponse(HttpStatusCode.OK, schemes);
        }

        // GET api/<controller>/5
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Scheme")]
        public HttpResponseMessage GetScheme([FromUri]int id)
        {
            var scheme = _entityLogic.GetSingle(c => c.Id == id, c => c.TransactionType, c => c.Channel, c => c.Route, c => c.Fee);

            return Request.CreateResponse(HttpStatusCode.OK, scheme);
        }

        // POST api/<controller>
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/CreateScheme")]
        public async Task<HttpResponseMessage> CreateScheme(Scheme scheme)
        {
            var schemeName = _entityLogic.GetSingle(c => c.Name == scheme.Name);
            if (schemeName != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, StatusCodes.NAME_ALREADY_EXIST);
            }
            scheme.Id = 0;

            _entityLogic.Insert(scheme);
            _entityLogic.Save();



            var getSchemes = _entityLogic.GetSingle(c => c.Id == scheme.Id, c => c.TransactionType, c => c.Channel, c => c.Route, c => c.Fee);
            await _entityLogic.Pusher(getSchemes, "scheme");

            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.CREATED);

        }

        // PUT api/<controller>/5
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/UpdateScheme")]
        public HttpResponseMessage UpdateScheme(Scheme scheme)

        {
            try
            {

                _entityLogic.Update(scheme);
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
        [Route("api/DeleteScheme")]
        public HttpResponseMessage DeleteScheme([FromUri]int id)
        {

            var scheme = _entityLogic.GetSingle(c => c.Id == id);
            _entityLogic.Delete(scheme);
            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.DELETED);
        }
    }
}