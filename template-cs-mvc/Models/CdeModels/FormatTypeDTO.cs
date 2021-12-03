using Newtonsoft.Json;
using System;

namespace Bimswarm.Models.CdeModels
{
    public class FormatTypeDTO : IDataTransferObject
    {
        public long id;
        public string mime;
        public string extension;
        public string schema;
        public string schemaVersion;
        public string schemaSubset;

        public FormatTypeDTO() { }

        public FormatTypeDTO(FormatType format) {
            id = format.id;
            mime = format.mime;
            extension = format.extension;
            schema = format.schema;
            schemaVersion = format.schemaVersion;
            schemaSubset = format.schemaSubset;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
