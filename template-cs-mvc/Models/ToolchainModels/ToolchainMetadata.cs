using Newtonsoft.Json;
using Bimswarm.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bimswarm.Models.ToolchainModels
{
    public class ToolchainMetadata : ISwarmModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ToolchainMetadata() { }

        public ToolchainMetadata(ToolchainMetadataDTO toolchainMetadataDto)
        {
            if (toolchainMetadataDto == null)
            {
                Name = string.Empty;
                Description = string.Empty;
            }
            else
            {
                Id = toolchainMetadataDto.id;
                Name = toolchainMetadataDto.name;
                Description = toolchainMetadataDto.description;
            }
        }

        public override IDataTransferObject ToDto()
        {
            return new ToolchainMetadataDTO(this);
        }
    }
}
