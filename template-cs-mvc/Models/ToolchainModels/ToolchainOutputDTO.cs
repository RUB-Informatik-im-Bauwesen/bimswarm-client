using Bimswarm.Models.ToolchainModels;

namespace Bimswarm.Models.DataTransferObjects
{
    public class ToolchainOutputDTO
    {
        public long outputContentId { get; set; }
        public long formatTypeId { get; set; }

        public ToolchainOutputDTO():base() { }

        public ToolchainOutputDTO(ToolchainOutput model)
        {
            outputContentId = model.outputContentId;
            formatTypeId = model.formatTypeId;
        }


        public ToolchainOutputDTO(long outputcontentid, long formattypeid)
        {
            outputContentId = outputcontentid;
            formatTypeId = formattypeid;
        }
    }
}
