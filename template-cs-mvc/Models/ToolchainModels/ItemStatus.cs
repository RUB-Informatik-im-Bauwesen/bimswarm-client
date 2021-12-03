using System.Runtime.Serialization;

namespace Bimswarm.Models.ToolchainModels
{
    public enum ItemStatus
    {
        [EnumMember(Value = "PENDING")]
        PENDING,
        [EnumMember(Value = "SUCCESS")]
        SUCCESS,
        [EnumMember(Value = "WORKING")]
        WORKING,
        [EnumMember(Value = "FAILED")]
        FAILED
    }
}
