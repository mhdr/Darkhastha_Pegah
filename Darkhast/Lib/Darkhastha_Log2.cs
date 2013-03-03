using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Darkhast.Lib
{
    public static class Darkhastha_Log2
    {
        public static void InsertLog(Darkhastha darkhast, RevisionOperation operation)
        {
            Darkhastha_Log log = new Darkhastha_Log();
            DatabaseEntities entities = Lib.Global.Entities;

            log.DarkhastLogGUID = Guid.NewGuid();
            log.Date = DateTime.Now;
            log.Operation = Convert.ToInt32(operation);

            log.BarghkarGUID = darkhast.BarghkarGUID;
            log.DarkhastGUID = darkhast.DarkhastGUID;
            log.DarkhastName = darkhast.DarkhastName;
            log.DastgahGUID = darkhast.DastgahGUID;
            log.IsTrash = darkhast.IsTrash;
            log.LoginGUID = Lib.Global.LoginGuid;
            log.ShomareDarkhastTadbir = darkhast.ShomareDarkhastTadbir;
            log.ShomareFani = darkhast.ShomareFani;
            log.Tarikh = darkhast.Tarikh;
            log.TarikhDaryaftKala = darkhast.TarikhDaryaftKala;
            log.TarikhVorodKalaBeAnbar = darkhast.TarikhVorodKalaBeAnbar;
            log.TedadDarkhast = darkhast.TedadDarkhast;
            log.Tozihat = darkhast.Tozihat;
            log.VahedShomaresh = darkhast.VahedShomaresh;
            log.Vaziat = darkhast.Vaziat;

            entities.Darkhastha_Log.AddObject(log);
            entities.SaveChanges();
        }
    }
}
