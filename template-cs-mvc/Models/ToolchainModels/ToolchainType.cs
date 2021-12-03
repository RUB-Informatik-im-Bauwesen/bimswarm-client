using System.Runtime.Serialization;

namespace Bimswarm.Models.ToolchainModels
{
    public enum ToolchainType
    {
        [EnumMember(Value = "TOOLCHAIN")]
        TOOLCHAIN,
        [EnumMember(Value = "TEMPLATE")]
        TEMPLATE
    }
}
