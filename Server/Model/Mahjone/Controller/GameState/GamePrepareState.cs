using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// When the server is in this state, the server make preparation that the game needs.
    /// The playerIndex is arranged in this state, so is the settings. Messages will be sent to clients to inform the information.
    /// Transfers to RoundStartState. The state transfer will be done regardless whether enough client responds received.
    /// </summary>
    public class GamePrepareState : ServerState
    {
        private HashSet<long> responds;
        private long timerId;
        public void Init()
        {
            responds = new HashSet<long>();
        }
        public void Destroy()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
            responds = null;
        }
        public override void OnServerStateEnter()
        {
            timerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 5000, TimeOutRoundStart);
            
            CurrentRoundStatus.ShufflePlayers();
            AssignInitialPoints();
            
            for (int index = 0; index < players.Count; index++)
            {
                //var tiles = CurrentRoundStatus.HandTiles(index);
                //Log.Debug($"[Server] Hand tiles of player {index}: {string.Join("", tiles)}");
                var info = new M2C_GamePrepare
                {
                    PlayerIndex = index,
                    Points = CurrentRoundStatus.Points,
                    PlayerNames = CurrentRoundStatus.PlayerNames,
                    Settings=CurrentRoundStatus.GameSettings
                };
                Game.EventSystem.Publish(new EventType.ActorMessage() { actorId=players[index],actorMessage=info }).Coroutine();
            }
        }
        public override void OnServerStateExit()
        {
            responds.Clear();
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }
        private void AssignInitialPoints()
        {
            for (int i = 0; i < players.Count; i++)
            {
                CurrentRoundStatus.SetPoints(CurrentRoundStatus.GameSettings.InitialPoints);
            }
        }




        public void TimeOutRoundStart()
        {
            Log.Debug("[Server] Prepare state time out");
            if (GetParent<MahjoneBehaviourComponent>().IsBattleMod)
            {
                GetParent<MahjoneBehaviourComponent>().SelectTiles();
            }
            else
            {
                GetParent<MahjoneBehaviourComponent>().RoundStart(true, false, false);
            }
        }

        public void OnEvent(long playerid)
        {
            responds.Add(playerid);
            if (responds.Count == totalPlayers)
            {
                Log.Debug("[Server] Prepare state MaxPlayer");
                TimeOutRoundStart();
            }
        }
    }
}