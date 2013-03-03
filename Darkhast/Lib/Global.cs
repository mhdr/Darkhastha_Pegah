using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Darkhast.Lib
{
    public static class Global
    {
        private static Guid _currentUserGuid;
        private static int _currentUserRole ;
        private static Guid _LoginGUID;
        private static DatabaseEntities _entities=new DatabaseEntities();


        public static Guid CurrentUserGuid
        {
            get { return _currentUserGuid; }
            set { _currentUserGuid = value; }
        }

        public static int CurrentUserRole
        {
            get { return _currentUserRole; }
            set { _currentUserRole = value; }
        }

        /// <summary>
        /// table LoginsLog_LoginGUID
        /// </summary>
        public static Guid LoginGuid
        {
            get { return _LoginGUID; }
            set { _LoginGUID = value; }
        }

        public static DatabaseEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }
    }
}
