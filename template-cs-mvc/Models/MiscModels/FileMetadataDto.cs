using Newtonsoft.Json;
using Bimswarm.Models.CdeModels;
using Bimswarm.Models.ToolchainModels;

namespace Bimswarm.Models.MiscModels
{
    public class FileMetadataDto : IDataTransferObject
    {
        public string guid;
        public string productid;
        public string user;
        public Toolchain toolchain;
        public FormatType filetype;
        public string filename;
        public string date;

        public FileMetadataDto(string guid, string productid, string user, string filename, Toolchain toolchain, FormatType filetype, string date)
        {
            this.guid = guid;
            this.productid = productid;
            this.user = user;
            this.toolchain = toolchain;
            this.filetype = filetype;
            this.filename = filename;
            this.date = date;
        }

        public string GetFileName()
        {
            return guid + "_" + filename;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
