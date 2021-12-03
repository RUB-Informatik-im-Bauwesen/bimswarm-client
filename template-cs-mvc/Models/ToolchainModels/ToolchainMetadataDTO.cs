using Newtonsoft.Json;
using Bimswarm.Models.ToolchainModels;

namespace Bimswarm.Models.DataTransferObjects
{
    public class ToolchainMetadataDTO : IDataTransferObject
    {
        public long id { get; set; }
        public string name { get; set; }

        public string description { get; set; }

        public ToolchainMetadataDTO() { }

        public ToolchainMetadataDTO(ToolchainMetadata metadata) {
            id = metadata.Id;
            name = metadata.Name;
            description = metadata.Description;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
