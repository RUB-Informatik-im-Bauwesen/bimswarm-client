using Newtonsoft.Json;
using Bimswarm.Models.ToolchainModels;
using System;

namespace Bimswarm.Models.CdeModels
{
    public class CdeContentDTO : IDataTransferObject
    {
        public long id;
        public string externalId;
        public string filename;
        public FormatTypeDTO format;

        public CdeContentDTO() { }

        public CdeContentDTO(CdeContent cdeContent) 
        {
            id = cdeContent.id;
            externalId = cdeContent.externalId;
            filename = cdeContent.filename;
            format = (FormatTypeDTO)cdeContent.format.ToDto();
        }

        public CdeContentDTO(long id, string externalId, string filename, FormatTypeDTO formatTypeDto)
        {
            this.id = id;
            this.externalId = externalId;
            this.filename = filename;
            this.format = formatTypeDto;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
