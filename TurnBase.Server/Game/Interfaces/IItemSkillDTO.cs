namespace TurnBase.Server.Game.Interfaces
{
    public interface IItemSkillDTO
    {
        int TurnCooldown { get; }
        bool FinalizeTurnInUse { get; }
    }
}
