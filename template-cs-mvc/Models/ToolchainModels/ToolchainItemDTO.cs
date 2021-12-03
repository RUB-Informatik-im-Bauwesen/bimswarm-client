using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Bimswarm.Models.DataTransferObjects;
using System.Collections.Generic;

namespace Bimswarm.Models.ToolchainModels
{
    public class ToolchainItemDTO : IDataTransferObject
    {

        public long? id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemStatus? status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemType? type { get; set; }
        public long? productId { get; set; }

       [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<long> inputContentId { get; set; } 
        public List<ToolchainOutputDTO> outputFormatTypeDtos { get; set; } = new List<ToolchainOutputDTO>();
        public List<long> successors { get; set; } = new List<long>();

        public ToolchainItemDTO() { }

        public ToolchainItemDTO(ToolchainItem toolchainItem) {
            id = toolchainItem.Id;
            status = toolchainItem.Status;
            type = toolchainItem.Type;
            productId = toolchainItem.ProductId;
            inputContentId = toolchainItem.InputContentId;
            //outputOcdeContentIds = toolchainItem.OutputContentIds;

            foreach(var item in toolchainItem.OutputContentIds)
            {
                outputFormatTypeDtos.Add(new ToolchainOutputDTO(item));
            }
            successors = toolchainItem.Successors;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
