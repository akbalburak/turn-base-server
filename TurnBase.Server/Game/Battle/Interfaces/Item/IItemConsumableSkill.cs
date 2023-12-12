namespace TurnBase.Server.Game.Battle.Interfaces.Item
{
    public interface IItemConsumableSkill : IItemSkill
    {
        public int LeftUseCount { get; }
    }
}
