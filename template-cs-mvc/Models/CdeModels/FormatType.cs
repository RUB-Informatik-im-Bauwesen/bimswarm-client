using Newtonsoft.Json;
using System;

namespace Bimswarm.Models.CdeModels
{
    public class FormatType : ISwarmModel
    {
        public long id;
        public string mime;
        public string extension;
        public string schema;
        public string schemaVersion;
        public string schemaSubset;


        public FormatType() { }

        public FormatType(FormatTypeDTO formatTypeDto)
        {
            id = formatTypeDto.id;
            mime = formatTypeDto.mime;
            extension = formatTypeDto.extension;
            schema = formatTypeDto.schema;
            schemaVersion = formatTypeDto.schemaVersion;
            schemaSubset = formatTypeDto.schemaSubset;
        }

        public FormatType(string mime, string extension, string schema, string schemaVersion, string schemaSubset)
        {
            this.mime = mime;
            this.extension = extension;
            this.schema = schema;
            this.schemaVersion = schemaVersion;
            this.schemaSubset = schemaSubset;
        }

        public override bool Equals(object obj)
        {
            FormatType o = (FormatType)obj;
            if (o.id == this.id)
            {
                return true;
            }
            if (o.schema == this.schema && o.schemaVersion == this.schemaVersion && o.schemaSubset == this.schemaSubset)
            {
                return true;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, mime, extension, schema, schemaVersion, schemaSubset);
        }

        public string GetFormattedString()
        {
            string res = schema;
            if (!string.IsNullOrEmpty(schemaVersion) && !schemaVersion.Equals("-") && !schemaVersion.Equals("null"))
            {
                res += " " + schemaVersion;
            }
            if (!string.IsNullOrEmpty(schemaSubset) && !schemaSubset.Equals("-") && !schemaSubset.Equals("null"))
            {
                res += " " + schemaSubset;
            }
            res += " (*." + extension + ")";
            return res;
        }

        public override IDataTransferObject ToDto()
        {
            return new FormatTypeDTO(this);
        }
        public override string ToString()
        {
            return GetFormattedString();
        }
    }
}
