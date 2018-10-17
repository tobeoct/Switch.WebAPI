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
    public class TransactionTypesController : ApiController
    {
        private ApplicationDbContext _context;

        public TransactionTypesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET api/<controller>
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/TransactionTypes")]
        public HttpResponseMessage GetTransactionTypes()
        {
            var transactionTypes = _context.TransactionTypes.ToList();

            return Request.CreateResponse(HttpStatusCode.OK, transactionTypes);
        }

        // GET api/<controller>/5
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/TransactionType")]
        public HttpResponseMessage GetTransactionType([FromUri]int id)
        {
            var transactionType = _context.TransactionTypes.SingleOrDefault(c => c.Id == id);

            return Request.CreateResponse(HttpStatusCode.OK, transactionType);
        }
        // POST api/<controller>
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/CreateTransactionType")]
        public async Task<HttpResponseMessage> CreateTransactionType(TransactionType transactionType)
        {
            var trxTypeName = _context.TransactionTypes.SingleOrDefault(c => c.Name == transactionType.Name);
            if (trxTypeName != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, StatusCodes.NAME_ALREADY_EXIST);
            }
            transactionType.Id = 0;


            var transactionTypeInDb = new TransactionType()
            {
                Name = transactionType.Name,
                Code = transactionType.Code,
                Description = transactionType.Description

            };

            _context.TransactionTypes.Add(transactionTypeInDb);
            _context.SaveChanges();

            var options = new PusherOptions
            {
                Cluster = "mt1",
                Encrypted = true
            };

            var getTransactionTypes = _context.TransactionTypes.SingleOrDefault(c => c.Id == transactionTypeInDb.Id);
            var pusher = new Pusher(
                "619556",
                "1e8d9229f9b58c374f76",
                "d3f1b6b70b528626fbef",
                options);

            if (getTransactionTypes != null)
            {
                var result = await pusher.TriggerAsync(
                    "my-transactionTypes",
                    "new-transactionType",
                    data: new
                    {
                        Id = getTransactionTypes.Id,
                        Name = getTransactionTypes.Name,
                        Code = getTransactionTypes.Code,
                        Description = getTransactionTypes.Description
                    });
            }

            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.CREATED);

        }

        // PUT api/<controller>/5
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/UpdateTransactionType")]
        public HttpResponseMessage UpdateTransactionType(TransactionType transactionType)

        {
            var transactionInDb = _context.TransactionTypes.SingleOrDefault(c => c.Id == transactionType.Id);

            if (transactionInDb != null)
            {
                transactionInDb.Name = transactionType.Name;
                transactionInDb.Code = transactionType.Code;
                transactionInDb.Description = transactionType.Description;
               
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.UPDATED);
            }


            return Request.CreateErrorResponse(HttpStatusCode.OK, StatusCodes.ERROR_DOESNT_EXIST);

        }

        // DELETE api/<controller>/5
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/DeleteTransactionType")]
        public HttpResponseMessage DeleteTransactionType([FromUri]int id)
        {

            var transactionType = _context.TransactionTypes.SingleOrDefault(c => c.Id == id);
            _context.TransactionTypes.Remove(transactionType);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.DELETED);
        }
    }
}