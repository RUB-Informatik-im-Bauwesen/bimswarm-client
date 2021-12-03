using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bimswarm.Models.CdeModels
{
    public class CdeContainer : ISwarmModel
    {
        public long Id;
        public string ExternalId;
        public string BaseUrl;
        public List<CdeContent> Content { get; set; } = new List<CdeContent>();

        public CdeContainer() { }

        public CdeContainer(CdeContainerDTO cdeContainerDto)
        {
            if (cdeContainerDto != null)
            {
                Id = cdeContainerDto.id;
                ExternalId = cdeContainerDto.externalId;
                BaseUrl = cdeContainerDto.baseUrl;
                foreach (var elem in cdeContainerDto.content)
                {
                    Content.Add(new CdeContent(elem));
                }
            }
        }
        public CdeContainer(string baseUrl)
        {
            this.Id = 0;
            this.ExternalId = Guid.NewGuid().ToString();
            this.BaseUrl = baseUrl;
            Content = new List<CdeContent>(); ;
        }

        public override IDataTransferObject ToDto()
        {
            return new CdeContainerDTO(this);
        }
    }
}
