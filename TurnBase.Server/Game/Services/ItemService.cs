using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Models;
using TurnBase.DBLayer.Repositories;
using TurnBase.Server.Game.Battle.Enums;
using TurnBase.Server.Game.Interfaces;
using TurnBase.Server.Game.DTO;
using TurnBase.Server.Game.Enums;

namespace TurnBase.Server.Game.Services
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
                    Properties = y.TblItemPropertyMappings.Select(p => new ItemPropertyMappingDTO
                    {
                        PropertyId = (ItemProperties)p.PropertyId,
                        MaxValue = p.MaxValue,
                        MinValue = p.MinValue
                    }).ToArray(),
                    Contents = y.TblItemContentMappings.Select(c => new ItemContentMappingDTO
                    {
                        ContentId = (ItemContents)c.ContentId,
                        IndexId = c.IndexId,
                        Value = c.Value
                    }).ToArray(),
                    Skills = y.TblItemSkillMappings.Select(i => new ItemSkillMappingDTO
                    {
                        SkillId = (BattleSkills)i.SkillId,
                        SlotIndex = i.SlotIndex,
                    }).ToArray()
                }).ToArray();
            }
        }

        public static IItemDTO GetItem(int itemId)
        {
            return _items.FirstOrDefault(y => y.Id == itemId);
        }
    }
}
