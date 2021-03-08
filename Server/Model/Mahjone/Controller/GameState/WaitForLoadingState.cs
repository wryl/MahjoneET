using GamePlay.Server.Model;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// When server is in this state, the server waits for ReadinessMessage from every player.
    /// When the server gets enough ReadinessMessages, the server transfers to GamePrepareState.
    /// Otherwise the server will resend the messages to not-responding clients until get enough responds or time out.
    /// When time out, the server transfers to GameAbortState.
    /// </summary>
    public class WaitForLoadingState : ServerState
    {
        private HashSet<long> responds;
        private long timerId;
        public void Awake()
        {

        }
        public void Destory()
        {
            OnServerStateExit();
        }
        public override void OnServerStateEnter()
        {
            responds = new HashSet<long>();
            timerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + ServerConstants.ServerWaitForLoadingTimeOut, TimeOutGameAbort);
        }

        public override void OnServerStateExit()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
            responds.Clear();
        }
        private void TimeOutGameAbort()
        {
            GetParent<MahjoneBehaviourComponent>().GameAbort();
        }

        public void OnEvent(long playerid)
        {
            responds.Add(playerid);
            ET.Log.Debug($"WaitForLoadingState id: {playerid}");
            Log.Debug($"WaitForLoadingState count: {responds.Count},max :{totalPlayers}");
            if (responds.Count == totalPlayers)
            {
                GetParent<MahjoneBehaviourComponent>().GamePrepare();
            }
        }
    }
}