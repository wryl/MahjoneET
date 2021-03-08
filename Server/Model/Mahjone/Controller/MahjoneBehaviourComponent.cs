using Common.StateMachine;
using Common.StateMachine.Interfaces;
using GamePlay.Server.Model;
using Mahjong.Model;
using System.Collections.Generic;
namespace ET
{
	public class MahjoneBehaviourComponentAwakeSystem : AwakeSystem<MahjoneBehaviourComponent,GameSetting>
	{
		public override void Awake(MahjoneBehaviourComponent self, GameSetting setting)
		{
			self.Awake(setting);
		}
	}
	/// <summary>
	/// This class only takes effect on server
	/// </summary>
	public class MahjoneBehaviourComponent :Entity
	{
		public GameSetting GameSettings;

		public StateMachine StateMachine
		{
			get;
			set;
		}

		public MahjongSet mahjongSet;
		public ServerRoundStatus CurrentRoundStatus = null;

		public void Awake(GameSetting setting)
		{
			GameSettings = setting;
			InitAddState();
			ET.Log.Debug("[Server] ServerBehaviour.OnEnable() is called");
			StateMachine = new StateMachine();
			ReadSetting();
			CurrentRoundStatus = new ServerRoundStatus(GameSettings, GetParent<MJRoomComponent>().AllPlayer);
			//mahjongSet = new MahjongSet(GameSettings, GameSettings.GetAllTiles());
			WaitForOthersLoading();
			Game.EventSystem.Publish(new EventType.MessageBroadCast() {actorIds= GetParent<MJRoomComponent>().AllPlayerActorids, actorMessage = new M2C_EnterMahjoneGame() }).Coroutine();
		}
		/// <summary>
		/// 初始化的状态机行为
		/// </summary>
		private void InitAddState() 
		{
			AddComponent<WaitForLoadingState>();
			AddComponent<GamePrepareState>();
			AddComponent<RoundStartState>();
			
			AddComponent<PlayerDrawTileState>();
			AddComponent<WaitForDiscardTileState>();
			AddComponent<PlayerBeiDoraState>();
			AddComponent<PlayerDiscardTileState>();
			AddComponent<PlayerKongState>();
			AddComponent<PlayerRongState>();
			AddComponent<PlayerTsumoState>();
			AddComponent<OperationPerformState>();
			AddComponent<PointTransferState>();
			AddComponent<RoundDrawState>();
			AddComponent<TurnEndState>();
			AddComponent<GameEndState>();
			AddComponent<PrePlayerDrawTileState>();
			AddComponent<InitHandAfterRongState>();
			AddComponent<BattleRoundDrawState>();
			AddComponent<SelectTilesState>();

		}
		private void ReadSetting()
		{
			var room = GetParent<MJRoomComponent>();
			GameSettings = room.GameSettings;
		}

		private void WaitForOthersLoading()
		{
			StateMachine.ChangeState(GetComponent<WaitForLoadingState>());
		}

		public void GamePrepare()
		{
			GetComponent<GamePrepareState>().Init();
			StateMachine.ChangeState(GetComponent<GamePrepareState>());
		}

		public void GameAbort()
		{
			// todo -- implement abort logic here: at least one of the players cannot load into game, back to lobby scene
			//Debug.LogError("The game aborted, this part is still under construction");
		}

		public void RoundStart(bool next, bool extra, bool keepSticks)
		{
			GetComponent<RoundStartState>().Init(next, extra, keepSticks);
			StateMachine.ChangeState(GetComponent<RoundStartState>());
		}
		public void PreDrawTile(int playerIndex, bool isLingShang = false, bool turnDoraAfterDiscard = false)
		{
			GetComponent<PrePlayerDrawTileState>().Init(playerIndex, isLingShang, turnDoraAfterDiscard);
			StateMachine.ChangeState(GetComponent<PrePlayerDrawTileState>());
		}
		public void InitHandAfterRong(int playerIndex)
		{
			GetComponent<InitHandAfterRongState>().Init(playerIndex);
			StateMachine.ChangeState(GetComponent<InitHandAfterRongState>());
		}
		public void DrawTile(int playerIndex, bool isLingShang = false, bool turnDoraAfterDiscard = false)
		{
			GetComponent<PlayerDrawTileState>().Init(playerIndex, isLingShang,turnDoraAfterDiscard);
			StateMachine.ChangeState(GetComponent<PlayerDrawTileState>());
		}
		public void WaitingDiscardTile(int playerIndex, Tile tile,bool turnDoraAfterDiscard)
		{
			GetComponent<WaitForDiscardTileState>().Init(playerIndex, tile, turnDoraAfterDiscard);
			StateMachine.ChangeState(GetComponent<WaitForDiscardTileState>());
		}
		public void DiscardTile(int playerIndex, Tile tile, bool isRichiing, bool discardLastDraw, int bonusTurnTime,
			bool turnDoraAfterDiscard)
		{
			GetComponent<PlayerDiscardTileState>().Init(tile, isRichiing, discardLastDraw, turnDoraAfterDiscard, bonusTurnTime);
			StateMachine.ChangeState(GetComponent<PlayerDiscardTileState>());
		}

		public void TurnEnd(int playerIndex, Tile discardingTile, bool isRichiing, OutTurnOperation[] operations,
			bool isRobKong, bool turnDoraAfterDiscard)
		{
			GetComponent<TurnEndState>().Init(discardingTile, operations, isRichiing, isRobKong, turnDoraAfterDiscard);
			StateMachine.ChangeState(GetComponent<TurnEndState>());
		}

		public void PerformOutTurnOperation(int newPlayerIndex, OutTurnOperation operation)
		{
			GetComponent<OperationPerformState>().Init(newPlayerIndex, CurrentRoundStatus.CurrentPlayerIndex, operation);
			StateMachine.ChangeState(GetComponent<OperationPerformState>());
		}

		public void Tsumo(int currentPlayerIndex, Tile winningTile, PointInfo pointInfo)
		{
			GetComponent<PlayerTsumoState>().Init(currentPlayerIndex,winningTile,pointInfo);
			StateMachine.ChangeState(GetComponent<PlayerTsumoState>());
		}

		public void Rong(int currentPlayerIndex, Tile winningTile, int[] rongPlayerIndices, PointInfo[] rongPointInfos)
		{
			GetComponent<PlayerRongState>().Init(currentPlayerIndex, winningTile, rongPlayerIndices, rongPointInfos);
			StateMachine.ChangeState(GetComponent<PlayerRongState>());
		}

		public void Kong(int playerIndex, OpenMeld kong)
		{
			GetComponent<PlayerKongState>().Init(playerIndex, kong);
			StateMachine.ChangeState(GetComponent<PlayerKongState>());
		}

		public void RoundDraw(RoundDrawType type)
		{
			GetComponent<RoundDrawState>().Init(type);
			StateMachine.ChangeState(GetComponent<RoundDrawState>());
		}
		public void BattleRoundDraw(int playerindex)
		{
			GetComponent<BattleRoundDrawState>().Init(playerindex);
			StateMachine.ChangeState(GetComponent<BattleRoundDrawState>());
		}
		public void BeiDora(int playerIndex)
		{
			GetComponent<PlayerBeiDoraState>().Init(playerIndex);
			StateMachine.ChangeState(GetComponent<PlayerBeiDoraState>());
		}

		public void PointTransfer(List<PointTransfer> transfers, bool next, bool extra, bool keepSticks)
		{
			GetComponent<PointTransferState>().Init(transfers,next,extra,keepSticks);
			StateMachine.ChangeState(GetComponent<PointTransferState>());
		}

		public void GameEnd()
		{
			StateMachine.ChangeState(GetComponent<GameEndState>());
		}
		public void SelectTiles()
		{
			GetComponent<SelectTilesState>().Init();
			StateMachine.ChangeState(GetComponent<SelectTilesState>());
		}
	}
}