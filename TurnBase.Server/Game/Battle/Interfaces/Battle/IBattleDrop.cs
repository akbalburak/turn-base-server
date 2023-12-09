namespace TurnBase.Server.Game.Battle.Interfaces.Battle
{
    public interface IBattleDrop
    {
        IBattleUser DropOwner { get; }
        IBattleUnit KilledUnit { get; }
        IBattleDropItem[] Drops { get; }

        void Claim(int dropItemId);
    }
}