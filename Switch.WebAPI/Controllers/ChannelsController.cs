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
    public class ChannelsController : ApiController
    {
        private ApplicationDbContext _context;

        public ChannelsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET api/<controller>
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Channels")]
        public HttpResponseMessage GetChannels()
        {
            var channels = _context.Channels.ToList();

            return Request.CreateResponse(HttpStatusCode.OK, channels);
        }

        // GET api/<controller>/5
        [AcceptVerbs("GET")]
        [HttpGet]
        [Route("api/Channel")]
        public HttpResponseMessage GetChannel([FromUri]int id)
        {
            var channel = _context.Channels.SingleOrDefault(c => c.Id == id);

            return Request.CreateResponse(HttpStatusCode.OK, channel);
        }

        // POST api/<controller>
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/CreateChannel")]
        public async Task<HttpResponseMessage> CreateChannel(Channel channel)
        {
            channel.Id = 0;


            var channelInDb = new Channel()
            {
                Name = channel.Name,
                Code = channel.Code,
                Description = channel.Description

            };

            _context.Channels.Add(channelInDb);
            _context.SaveChanges();

            var options = new PusherOptions
            {
                Cluster = "mt1",
                Encrypted = true
            };

            var getChannels = _context.Channels.SingleOrDefault(c => c.Id == channelInDb.Id);
            var pusher = new Pusher(
                "619556",
                "1e8d9229f9b58c374f76",
                "d3f1b6b70b528626fbef",
                options);

            if (getChannels != null)
            {
                var result = await pusher.TriggerAsync(
                    "my-channels",
                    "new-channel",
                    data: new
                    {
                        Id = getChannels.Id,
                        Name = getChannels.Name,
                        Code = getChannels.Code,
                        Description = getChannels.Description
                    });
            }

            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.CREATED);

        }

        // PUT api/<controller>/5
        [AcceptVerbs("POST")]
        [HttpPost]
        [Route("api/UpdateChannel")]
        public HttpResponseMessage UpdateChannel(Channel channel)

        {
            var channelInDb = _context.Channels.SingleOrDefault(c => c.Id == channel.Id);

            if (channelInDb != null)
            {
                channelInDb.Name = channel.Name;
                channelInDb.Code = channel.Code;
                channelInDb.Description = channel.Description;
                
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.UPDATED);
            }


            return Request.CreateErrorResponse(HttpStatusCode.OK, StatusCodes.ERROR_DOESNT_EXIST);

        }

        

        // DELETE api/<controller>/5
        [AcceptVerbs("DELETE")]
        [HttpDelete]
        [Route("api/DeleteChannel")]
        public HttpResponseMessage DeleteChannel([FromUri]int id)
        {
          
            var channel = _context.Channels.SingleOrDefault(c => c.Id == id);
            _context.Channels.Remove(channel);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, StatusCodes.DELETED);
        }
    }
}