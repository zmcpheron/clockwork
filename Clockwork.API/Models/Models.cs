using System;
using Microsoft.EntityFrameworkCore;

namespace Clockwork.API.Models
{
    public class ClockworkContext : DbContext
    {        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=clockwork.db");
        }

        public DbSet<CurrentTimeQuery> CurrentTimeQueries { get; set; }
    }

    public class CurrentTimeQuery
    {
        public int CurrentTimeQueryId { get; set; }
        public DateTime Time { get; set; }
        public string ClientIp { get; set; }
        public DateTime UTCTime { get; set; }
        public string TimeZoneId { get; set; }

        /// <summary>
        /// Sets the Id field to the user-friendly display name for output
        /// </summary>
        public void SetTimeZoneIdToDisplayName()
        {
            TimeZoneInfo thisTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(this.TimeZoneId);
            this.TimeZoneId = thisTimeZoneInfo.DisplayName;
        }
    }

    public class TimeZoneOption
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public bool IsLocal { get; set; }

        public TimeZoneOption(string id, string displayName, bool isLocal)
        {
            Id = id;
            DisplayName = displayName;
            IsLocal = isLocal;
        }
    }
}
