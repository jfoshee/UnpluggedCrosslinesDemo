using System;
using System.Web.Management;

namespace BluwareDemo
{
    public class LogEvent : WebRequestErrorEvent
    {
        public LogEvent(Exception ex)
            : base(null, null, 100001, ex)
        {
        }

        public LogEvent(string message)
            : base(null, null, 100001, new Exception(message))
        {
        }
    }
}