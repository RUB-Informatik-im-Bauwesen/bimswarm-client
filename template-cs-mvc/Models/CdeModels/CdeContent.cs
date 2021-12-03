using Newtonsoft.Json;
using Bimswarm.Models.ToolchainModels;
using System;

namespace Bimswarm.Models.CdeModels
{
    public class CdeContent : ISwarmModel
    {
        public long id;
        public string externalId;
        public string filename;
        public FormatType format;

        public CdeContent() { }

        public CdeContent(CdeContentDTO cdeContentDto)
        {
            id = cdeContentDto.id;
            externalId = cdeContentDto.externalId;
            filename = cdeContentDto.filename;
            if (cdeContentDto.format != null)
            {
                format = new FormatType(cdeContentDto.format);
            }
        }
        public CdeContent(long id, string externalId, string filename)
        {
            this.id = id;
            this.externalId = externalId;
            this.filename = filename;
            this.format = new FormatType("application/png", "png", "png", "1", "1");
        }

        public override IDataTransferObject ToDto()
        {
            return new CdeContentDTO(this);
        }
    }
}