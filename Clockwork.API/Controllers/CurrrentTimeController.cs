using System;
using Microsoft.AspNetCore.Mvc;
using Clockwork.API.Models;
using System.Linq;
using System.Collections.Generic;

namespace Clockwork.API.Controllers
{
    [Route("api/[controller]")]
    public class CurrentTimeController : Controller
    {    
        [HttpGet("getCurrentTime")]        
        public IActionResult GetCurrentTime()
        {
            var utcTime = DateTime.UtcNow;
            var serverTime = DateTime.Now;
            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();

            var returnVal = new CurrentTimeQuery
            {
                UTCTime = utcTime,
                ClientIp = ip,
                Time = serverTime
            };

            using (var db = new ClockworkContext())
            {
                db.CurrentTimeQueries.Add(returnVal);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                foreach (var CurrentTimeQuery in db.CurrentTimeQueries)
                {
                    Console.WriteLine(" - {0}", CurrentTimeQuery.UTCTime);
                }
            }

            return Ok(returnVal);
        }

        [HttpGet("getAllRequestEntries")]
        public IActionResult GetAllRequestEntries()
        {
            List<CurrentTimeQuery> returnVal;

            using (var db = new ClockworkContext())
            {
                returnVal = db.CurrentTimeQueries.ToList();
            }

            return Ok(returnVal);
        }
    }
}
