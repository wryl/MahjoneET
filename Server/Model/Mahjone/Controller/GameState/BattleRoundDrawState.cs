using GamePlay.Server.Model;
using Mahjong.Logic;
using Mahjong.Model;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class BattleRoundDrawState : ServerState
	{
		private int playerIndex;
		public RoundDrawType RoundDrawType;
		public MahjongSet MahjongSet => GetParent<MahjoneBehaviourComponent>().mahjongSet;
		public void Init(int playerindex)
		{
			playerIndex = playerindex;
		}
		public override void OnServerStateEnter()
		{
			Log.Debug("÷ÿ–¬œ¥≈∆¡À:" + MahjongSet.TilesRemain.ToString());
			List<Tile> newTiles = CurrentRoundStatus.GetTileFromRiver();
			if (MahjongSet.TilesRemain > 0) newTiles.AddRange(MahjongSet.PeekTiles(MahjongSet.TilesRemain));
			MahjongSet.ResetByTiles(newTiles);

			var info = new M2C_BattleRoundDraw() { MahjongSetData=MahjongSet.Data};
			Game.EventSystem.Publish(new EventType.MessageBroadCast() { actorIds = players, actorMessage = info }).Coroutine();
			TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow()+ServerConstants.ServerRoundDrawTimeOut, TimeOutFunc);
		}


		public void TimeOutFunc()
		{
			ParentBehaviour.PreDrawTile(playerIndex);
        }

        public override void OnServerStateExit()
        {
			//ParentBehaviour.PointTransfer(transfers, next, extra, true);
		}
    }
}