using Bimswarm.Models.CdeModels;
using System.Collections.Generic;
using System.Linq;
using Bimswarm.Models.DataTransferObjects;

namespace Bimswarm.Models.ToolchainModels
{
    public class Toolchain : ISwarmModel
    {
        public long Id { get; set; }
        public long? RelTemplate { get; set; }
        public ToolchainType Type { get; set; }
        public string Name => Metadata.Name;
        public string Description => Metadata.Description;
        public List<ToolchainItem> Items { get; set; } = new List<ToolchainItem>();
        public CdeContainer Container { get; set; } = new CdeContainer();
        public ToolchainMetadata Metadata { get; set; } = new ToolchainMetadata();
        public string UserId { get; set; }
        public bool Deleted { get; set; }
        public List<Toolchain> Toolchains { get; set; } = new List<Toolchain>();

        public Toolchain() { }

        public Toolchain(ToolchainDTO toolchainDto, ToolchainMetadataDTO metadatenDto)
        {
            Id = toolchainDto.id;

            if (toolchainDto.relTemplate.HasValue)
                RelTemplate = toolchainDto.relTemplate.Value;

            Type = toolchainDto.type;
            foreach (var item in toolchainDto.items)
            {
                Items.Add(new ToolchainItem(item, toolchainDto));
            }
            if (toolchainDto.container != null)
                Container = new CdeContainer(toolchainDto.container);
            Metadata = new ToolchainMetadata(metadatenDto);
            UserId = toolchainDto.userId;
            Deleted = toolchainDto.deleted;
        }

        public Toolchain(string name, string description, string userId)
        {
            this.Metadata = new ToolchainMetadata();
            this.Id = 99;
            this.Metadata.Name = name;
            this.Metadata.Description = description;
            this.UserId = userId;
            this.Type = ToolchainType.TEMPLATE;
            this.Container = new CdeContainer("http://localhost:8099");
            this.Items = new List<ToolchainItem>();

            Container.Content.Add(new CdeContent(0, "0", "testinput.png"));
            Container.Content.Add(new CdeContent(1, "1", "testoutput.png"));
            long pid = long.Parse(ConfigurationManager.AppSettings["swarm:productID"]);
            var startItem = new ToolchainItem(0, 99, ItemType.STARTITEM, null, null, null, new List<long> { 1 });
            var activityItem = new ToolchainItem(1, 99, ItemType.ACTIVITYITEM, pid, null, new List<ToolchainOutput>() { new ToolchainOutput(1, 1) }, new List<long> { 1 });
            var forkItem = new ToolchainItem(2, 99, ItemType.FORKITEM, null, null, null, new List<long> { 3, 4 });
            var activityItem1 = new ToolchainItem(3, 99, ItemType.ACTIVITYITEM, pid, new List<long> { 1 }, new List<ToolchainOutput>() { new ToolchainOutput(2, 2) }, new List<long> { 5 });
            var activityItem2 = new ToolchainItem(4, 99, ItemType.ACTIVITYITEM, pid, new List<long> { 1 }, new List<ToolchainOutput>() { new ToolchainOutput(3, 1) }, new List<long> { 5 });
            var joinItem = new ToolchainItem(5, 99, ItemType.JOINITEM, null, null, null, new List<long> { 6 });
            var endItem = new ToolchainItem(6, 99, ItemType.ENDITEM, null, null, null, null);

            Items.Add(startItem);
            Items.Add(activityItem);
            Items.Add(forkItem);
            Items.Add(activityItem1);
            Items.Add(activityItem2);
            Items.Add(joinItem);
            Items.Add(endItem);

        }

        public ItemStatus GetStatus()
        {

            ToolchainItem start = GetStartItem();
            if (start == null)
            {
                return ItemStatus.PENDING;
            }

            List<ToolchainItem> successors = GetSuccessors(start);
            ItemStatus? status = start.Status;
            while (successors.Any())
            {
                ToolchainItem next = successors.First();
                switch (next.Status)
                {
                    case ItemStatus.FAILED:
                        return ItemStatus.FAILED;
                    case ItemStatus.WORKING:
                        return ItemStatus.WORKING;
                    case ItemStatus.PENDING:
                        return ItemStatus.PENDING;
                    case ItemStatus.SUCCESS:
                        break;
                    case null:
                        break;
                    default:
                        break;
                }
                successors.Remove(next);
                successors.AddRange(GetSuccessors(next));
            }
            return ItemStatus.SUCCESS;
        }

        public ItemStatus GetStatusByProduct(long productId)
        {
            return GetItemByProduct(productId)?.Status ?? ItemStatus.FAILED;
        }

        public ToolchainItem GetStartItem()
        {
            return Items.Find(item => item.Type.Equals(ItemType.STARTITEM));
        }

        public List<ToolchainItem> GetSuccessors(ToolchainItem forItem)
        {
            var result = new List<ToolchainItem>();
            if (forItem != null && forItem.Successors != null)
            {
                foreach (var id in forItem.Successors)
                {
                    result.Add(GetItem(id));
                }
            }
            return result;
        }

        public bool HasSuccessor(ToolchainItem forItem)
        {
            return forItem.Successors.Any();
        }

        public ToolchainItem GetItem(long itemId)
        {
            return Items.Find(item => item.Id.Value.Equals(itemId));
        }
        public ToolchainItem GetItemByProduct(long productId)
        {
            return Items.Find(item => item.ProductId.Equals(productId));
        }
        public override string ToString()
        {
            return Type.ToString() + " [ID=" + Id + "]";
        }

        public override IDataTransferObject ToDto()
        {
            return new ToolchainDTO(this);
        }
    }
}
