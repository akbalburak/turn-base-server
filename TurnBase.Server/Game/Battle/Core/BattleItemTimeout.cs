namespace TurnBase.Server.Game.Battle.Core
{
    public partial class BattleItem
    {
        const float TimeoutMins = .5f;

        private Timer _timer;

        private void StartTimeoutTimer()
        {
            StopTimeoutTimer();

            if (_disposed)
                return;

            _timer = new Timer(OnTimeoutTimerElapsed, 
                null, 
                TimeSpan.FromMinutes(TimeoutMins), 
                Timeout.InfiniteTimeSpan
             );
        }

        private void StopTimeoutTimer()
        {
            _timer?.Dispose();
            _timer = null;
        }

        private void OnTimeoutTimerElapsed(object? sender)
        {
            StopTimeoutTimer();
            FinalizeBattle(isVictory: false);
            Dispose();
        }
    }
}
