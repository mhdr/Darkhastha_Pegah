using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Darkhast.Lib
{
    public static class TraceToSql
    {
        public static void Log(string message, Lib.EventTraceType eventType = Lib.EventTraceType.Information)
        {
            DatabaseEntities entities = Lib.Global.Entities;

            EventLog eventLog = new EventLog();
            eventLog.EventGUID = Guid.NewGuid();
            eventLog.EventMessage = message;
            eventLog.MachineName = System.Environment.MachineName;
            eventLog.EventType = Convert.ToInt32(eventType);
            eventLog.Date = DateTime.Now;

            entities.EventLogs.AddObject(eventLog);
            entities.SaveChanges();
        }

        public static void Log(string message, string machineName, Lib.EventTraceType eventType = Lib.EventTraceType.Information)
        {
            DatabaseEntities entities = Lib.Global.Entities;

            EventLog eventLog = new EventLog();
            eventLog.EventGUID = Guid.NewGuid();
            eventLog.EventMessage = message;
            eventLog.MachineName = machineName;
            eventLog.EventType = Convert.ToInt32(eventType);
            eventLog.Date = DateTime.Now;

            entities.EventLogs.AddObject(eventLog);
            entities.SaveChanges();
        }
    }
}
