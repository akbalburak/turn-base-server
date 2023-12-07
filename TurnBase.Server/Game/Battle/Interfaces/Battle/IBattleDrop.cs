namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattleDrop
    {
        IBattleUnit KilledUnit { get; }
        IBattleDropItem[] Drops { get; }
    }
}