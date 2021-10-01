using Microsoft.EntityFrameworkCore;
using WeeControl.SharedKernel.Authorization.Bases;

namespace WeeControl.Server.Domain.Authorization.ValueObjects
{
    [Owned]
    public class SessionLog : SessionLogBase
    {
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
        
        public SessionLog()
        {
        }

        public SessionLog(string value) : base()
        {
            LogValue = value;
        }

        /// <summary>
        /// Store User Activity in Session Log.
        /// </summary>
        /// <param name="logType">Optional brief string less than 10 letters.</param>
        /// <param name="logValue">Optional string less than 255 letters.</param>
        public SessionLog(string logType, string logValue) : this(logValue)
        {
            LogType = logType;
        }
    }
}