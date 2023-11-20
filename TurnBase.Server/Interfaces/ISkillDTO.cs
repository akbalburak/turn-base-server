namespace TurnBase.Server.Interfaces
{
    public interface ISkillDTO
    {
        int TurnCooldown { get; }
        bool FinalizeTurnInUse { get; }
    }
}
