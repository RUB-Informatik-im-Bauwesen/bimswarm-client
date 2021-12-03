using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Bimswarm.Models.CdeModels
{
    public class CdeContainerDTO : IDataTransferObject
    {
        public long id;
        public string externalId;
        public string baseUrl;
        public List<CdeContentDTO> content = new List<CdeContentDTO>();

        public CdeContainerDTO() { }

        public CdeContainerDTO(CdeContainer cdeContainer)
        {
            id = cdeContainer.Id;
            externalId = cdeContainer.ExternalId;
            baseUrl = cdeContainer.BaseUrl;
            foreach(var elem in cdeContainer.Content)
            {
                content.Add((CdeContentDTO)elem.ToDto());
            }
        }

        public CdeContainerDTO(long id, string externalId, string baseUrl)
        {
            this.id = id;
            this.externalId = externalId;
            this.baseUrl = baseUrl;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}