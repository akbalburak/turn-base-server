namespace TurnBase.Server.Game.Battle.Enums
{
    public enum BattleActions
    {
        LoadAll = 1,
        IamReady = 2,
        TurnUpdated = 3,
        FinalizeTurn = 4,
        UnitUseSkill = 5,
        BattleEnd = 6,
        EffectStarted = 7,
        EffectExecutionTurn = 8,
        EffectOver = 9,
        TurnOrderChanged = 10,
        YouHaveDrop = 11,
        ClaimADrop = 12,
        LeaveBattle = 13,
        ReconnectBattle = 14,
        CombatStateChanged = 15
    }
}
