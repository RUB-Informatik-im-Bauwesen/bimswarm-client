using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using Bimswarm.Models.DataTransferObjects;

namespace Bimswarm.Models.ToolchainModels
{
    public class ToolchainItem : ISwarmModel
    {
        public long? Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemStatus? Status { get; set; }
        public long? TemplateId { get; set; }
        public long? RelInstance { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ItemType? Type { get; set; }
        public long? ProductId { get; set; }
        public string CdeContainer { get; set; }
        public List<long> InputContentId { get; set; }
        public List<ToolchainOutput> OutputContentIds { get; set; } = new List<ToolchainOutput>();
        public List<long> Successors { get; set; } = new List<long>();

        public ToolchainItem() { }

        public ToolchainItem(ToolchainItemDTO toolchainItemDto, ToolchainDTO toolchain)
        {
            Id = toolchainItemDto.id;
            Status = toolchainItemDto.status;
            switch (toolchain.type)
            {
                case ToolchainType.TEMPLATE:
                    TemplateId = toolchain.id;
                    break;
                case ToolchainType.TOOLCHAIN:
                    RelInstance = toolchain.id;
                    CdeContainer = toolchain.container.externalId;
                    break;
            }
            Type = toolchainItemDto.type;
            ProductId = toolchainItemDto.productId.HasValue ? toolchainItemDto.productId.Value : -1;
            if (toolchainItemDto.inputContentId != null)
                InputContentId = toolchainItemDto.inputContentId;

            if (toolchainItemDto.outputFormatTypeDtos != null)
            {
                foreach (var elem in toolchainItemDto.outputFormatTypeDtos)
                {
                    OutputContentIds.Add(new ToolchainOutput(elem));
                }
            }
            
            Successors = toolchainItemDto.successors;
        }

        public ToolchainItem(long? id, long? templateId, ItemType type, long? productId, List<long> inputContentId, List<ToolchainOutput> outputContentIds, List<long> successors)
        {
            Id = id;
            Status = ItemStatus.PENDING;
            TemplateId = templateId;
            RelInstance = null;
            Type = type;
            ProductId = productId;
            InputContentId = inputContentId;
            OutputContentIds = outputContentIds;
            Successors = successors;
        }
        public override string ToString()
        {
            return Type.ToString() + " [ID=" + Id + "]";
        }

        public override IDataTransferObject ToDto()
        {
            return new ToolchainItemDTO(this);
        }
    }

}
