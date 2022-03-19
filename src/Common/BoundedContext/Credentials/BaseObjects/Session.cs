using System;
namespace WeeControl.Common.BoundedContext.Credentials.BaseObjects
{
    [Obsolete]
    public class Session
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? TerminatedOn { get; set; }
    }
}
