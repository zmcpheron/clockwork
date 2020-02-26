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
        public IActionResult GetCurrentTime(string timeZoneId)
        {            
            var utcTime = DateTime.UtcNow;
            var serverTime = DateTime.Now;          
            var requestedTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, timeZoneId);
            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();          

            var returnVal = new CurrentTimeQuery
            {
                UTCTime = utcTime,
                ClientIp = ip,
                Time = requestedTime,
                TimeZoneId = timeZoneId
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

            //Leverage the ID field to hold user-friendly display name
            returnVal.SetTimeZoneIdToDisplayName();

            return Ok(returnVal);
        }

        [HttpGet("getAllRequestEntries")]
        public IActionResult GetAllRequestEntries()
        {
            List<CurrentTimeQuery> returnVal;

            using (var db = new ClockworkContext())
            {
                returnVal = db.CurrentTimeQueries.ToList();

                //Leverage the ID field to hold user-friendly display name
                foreach (CurrentTimeQuery ctq in returnVal)
                {
                    ctq.SetTimeZoneIdToDisplayName();
                }
            }

            return Ok(returnVal);
        }

        [HttpGet("getAvailableTimeZones")]
        public IActionResult GetAvailableTimeZones()
        {
            //Return a list of all timezones available to the server (Id and DisplayName)
            IReadOnlyCollection<TimeZoneInfo> availableTimeZones = TimeZoneInfo.GetSystemTimeZones();
            TimeZoneInfo localTZI = TimeZoneInfo.Local;

            List<TimeZoneOption> returnVal = new List<TimeZoneOption>();

            //Set isLocal to true for local timezone
            foreach(TimeZoneInfo tzi in availableTimeZones)
            {
                returnVal.Add(new TimeZoneOption(tzi.Id, tzi.DisplayName, localTZI.Id == tzi.Id ? true : false));
            }

            return Ok(returnVal);
        }
    }
}
