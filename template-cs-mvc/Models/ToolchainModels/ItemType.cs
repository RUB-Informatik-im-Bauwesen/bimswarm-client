using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Bimswarm.Models.ToolchainModels
{
    public enum ItemType
    {
        [EnumMember(Value = "ACTIVITYITEM")]
        ACTIVITYITEM,
        [EnumMember(Value = "ENDITEM")]
        ENDITEM,
        [EnumMember(Value = "FORKITEM")]
        FORKITEM,
        [EnumMember(Value = "JOINITEM")]
        JOINITEM,
        [EnumMember(Value = "STARTITEM")]
        STARTITEM
    }
}
