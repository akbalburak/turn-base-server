using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Models;
using TurnBase.DBLayer.Repositories;
using TurnBase.Server.Enums;
using TurnBase.Server.Models;

namespace TurnBase.Server.Core.Services
{
    public static class ItemService
    {
        private static ItemDTO[] _items = new ItemDTO[0];

        public static ItemDTO[] Items => _items;

        public static void Initialize()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                _items = uow.GetRepository<TblItem>().Select(y => new ItemDTO
                {
                    Id = y.Id,
                    TypeId = (ItemTypes)y.TypeId,
                    Properties = y.TblItemProperties.Select(p => new ItemPropertyDTO
                    {
                        PropertyId = (ItemProperties)p.PropertyId,
                        MaxValue = p.MaxValue,
                        MinValue = p.MinValue
                    }).ToArray(),
                    Contents = y.TblItemContents.Select(c => new ItemContentDTO
                    {
                        ContentId = (ItemContents)c.ContentId,
                        IndexId = c.IndexId,
                        Value = c.Value
                    }).ToArray(),
                    Skills = y.TblItemSkills.Select(i => new ItemSkillDTO
                    {
                        SkillId = i.SkillId,
                        SlotIndex = i.SlotIndex,
                    }).ToArray()
                }).ToArray();
            }
        }

        public static ItemDTO GetItem(int itemId)
        {
            return _items.FirstOrDefault(y => y.Id == itemId);
        }
    }
}
