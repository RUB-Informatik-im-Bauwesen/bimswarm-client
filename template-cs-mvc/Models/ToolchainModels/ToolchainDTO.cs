using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Bimswarm.Models.CdeModels;
using Bimswarm.Models.ToolchainModels;

namespace Bimswarm.Models.DataTransferObjects
{
    public class ToolchainDTO : IDataTransferObject
    {
        [DefaultValue(-1)]
        public long id { get; set; }

        [DefaultValue(-1)]
        public long? relTemplate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ToolchainType type { get; set; }

        public List<ToolchainItemDTO> items { get; set; } = new List<ToolchainItemDTO>();
        public string toolchainMetadataId { get; set; }
        [DefaultValue("")]
        public string userId { get; set; }
        [DefaultValue(false)]
        public bool deleted { get; set; }
        public CdeContainerDTO container { get; set; }

        public ToolchainDTO() { }

        public ToolchainDTO(Toolchain toolchain)
        {
            id = toolchain.Id;
            relTemplate = toolchain.RelTemplate;
            type = toolchain.Type;

            foreach (var item in toolchain.Items)
            {
                items.Add((ToolchainItemDTO)item.ToDto());
            }

            toolchainMetadataId = toolchain.Metadata.Id.ToString();
            userId = toolchain.UserId;
            deleted = toolchain.Deleted;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
