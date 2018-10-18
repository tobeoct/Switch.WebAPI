using System;
using System.Collections.Generic;
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
    public class FeesController : ApiController
    {
        private ApplicationDbContext _context;

        private EntityLogic<Fee> _entityLogic;
        public FeesController()
        {
            _context = new ApplicationDbContext();
            _entityLogic = new EntityLogic<Fee>();
        }

        // GET api/<controller>
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Fees")]
        public HttpResponseMessage GetFees()
        {
            var fees = _entityLogic.GetList();

            return Request.CreateResponse(HttpStatusCode.OK, fees);
        }

        // GET api/<controller>/5
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Fee")]
        public HttpResponseMessage GetFee([FromUri]int id)
        {
            var fee = _entityLogic.GetSingle(c => c.Id == id);
            return Request.CreateResponse(HttpStatusCode.OK, fee);
        }

        // POST api/<controller>
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/CreateFee")]
        public async Task<HttpResponseMessage> CreateFee(Fee fee)
        {
            var feeName = _entityLogic.GetSingle(c => c.Name == fee.Name);
            if (feeName != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, StatusCodes.NAME_ALREADY_EXIST);
            }
            fee.Id = 0;
            if (fee.FlatAmount == 0)
            {
                fee.FlatAmount = null;
            }

            if (fee.PercentOfTrx == 0)
            {
                fee.PercentOfTrx = null;
            }
            if (fee.Maximum == 0)
            {
                fee.Maximum = null;
            }
            if (fee.Minimum == 0)
            {
                fee.Minimum = null;
            }
          
            _entityLogic.Insert(fee);
            _entityLogic.Save();

            var getFees = _entityLogic.GetSingle(c => c.Id == fee.Id);
            await _entityLogic.Pusher(getFees, "fee");

            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.CREATED);

        }

        // PUT api/<controller>/5
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/UpdateFee")]
        public HttpResponseMessage UpdateFee(Fee fee)

        {
           
            try
            { 
                _entityLogic.Update(fee);
                return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.UPDATED);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);

            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, StatusCodes.ERROR_DOESNT_EXIST);


        }

        // DELETE api/<controller>/5
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/DeleteFee")]
        public HttpResponseMessage DeleteFee([FromUri]int id)
        {

            var fee = _entityLogic.GetSingle(c=>c.Id==id);
            _entityLogic.Delete(fee);
            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.DELETED);
        }
    }
}