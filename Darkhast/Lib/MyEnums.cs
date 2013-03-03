using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Darkhast.Lib
{
    //class MyEnums
    //{
    //}

    public enum UserRole
    {
        Mamoli = 1,
        Admin = 2
    }

    public enum VaziatDarkhast
    {
        DarHaleBarresi = 0,
        TaeedNashode = 1,
        TaeedShode = 2
    }

    /// <summary>
    /// age dakhel satle ashghal bashe InTrash hast va age az satle ashghal ham hazf beshe OutOfTrash hast
    /// </summary>
    public enum Trash
    {
        NotATrash = 0,
        InTrash = 1,
        OutOfTrash = 2
    }

    public enum SearchType
    {
        Contains = 0,
        StarstWith = 1,
        EndsWith = 2,
        MatchCase = 3
    }

    public enum RevisionOperation
    {
        Insert = 1,
        Delete = 2,
        Update = 3
    }

    public enum MessageStatus
    {
        Draft = 1,
        Sent = 2
    }

    public enum EventTraceType
    {
        Information = 1,
        Warning = 2,
        Error = 3,
        Critical = 4
    }
}
