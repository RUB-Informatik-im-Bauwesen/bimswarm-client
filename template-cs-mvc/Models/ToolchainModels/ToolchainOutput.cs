using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bimswarm.Models.DataTransferObjects;

namespace Bimswarm.Models.ToolchainModels
{
    public class ToolchainOutput
    {
        public long outputContentId { get; set; }
        public long formatTypeId { get; set; }

        public ToolchainOutput()
        {
        }

        public ToolchainOutput(ToolchainOutputDTO dto)
        {
            outputContentId = dto.outputContentId;
            formatTypeId = dto.formatTypeId;
        }

        public ToolchainOutput(long outputContentId, long formatTypeId)
        {
            this.outputContentId = outputContentId;
            this.formatTypeId = formatTypeId;
        }
    }
}
