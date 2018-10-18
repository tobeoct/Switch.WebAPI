using System;
using System.Collections.Generic;
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
//            var fee = _context.Fees.SingleOrDefault(c => c.Id == id);
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


            var feeInDb = new Fee()
            {
                Name = fee.Name,
               FlatAmount = fee.FlatAmount,
                PercentOfTrx = fee.PercentOfTrx,
                Maximum = fee.Maximum,
                Minimum = fee.Minimum

            };

           var feeLogic = new EntityLogic<Fee>();
            feeLogic.Insert(feeInDb);
            feeLogic.Save();

            var options = new PusherOptions
            {
                Cluster = "mt1",
                Encrypted = true
            };

            var getFees = _entityLogic.GetSingle(c => c.Id == feeInDb.Id);
            var pusher = new Pusher(
                "619556",
                "1e8d9229f9b58c374f76",
                "d3f1b6b70b528626fbef",
                options);

            if (getFees != null)
            {
                var result = await pusher.TriggerAsync(
                    "my-fees",
                    "new-fee",
                    data: new
                    {
                        Id = getFees.Id,
                        Name = getFees.Name,
                        FlatAmount = getFees.FlatAmount,
                        PercentOfTrx = getFees.PercentOfTrx,
                        Maximum = getFees.Maximum,
                        Minimum = getFees.Minimum
                    });
            }

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
                Console.WriteLine(e);

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