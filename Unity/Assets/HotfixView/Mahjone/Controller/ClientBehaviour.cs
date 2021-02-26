using System.Linq;
using Common.StateMachine;
using Common.StateMachine.Interfaces;
using GamePlay.Client.Controller.GameState;
using GamePlay.Client.Model;
using GamePlay.Server.Model;
using Mahjong.Model;
using UnityEngine;
using ET;
namespace GamePlay.Client.Controller
{
	public class ClientBehaviourAwakeSystem : AwakeSystem<ClientBehaviour>
	{
		public override void Awake(ClientBehaviour self)
		{
			self.Awake();
		}
	}
	public class ClientBehaviour:Entity
	{
		public static ClientBehaviour Instance
		{
			get;
			set;
		}

		private ClientRoundStatus CurrentRoundStatus;
		private ViewController controller;

		public IStateMachine StateMachine
		{
			get;
			private set;
		}
		public void Awake()
		{
			Debug.Log("ClientBehaviour.OnEnable() is called");
			Instance = this;
			controller = ViewController.Instance;
			StateMachine = new StateMachine();
		}
		//public override void OnEnable()
		//{
		//	base.OnEnable();
		//	Debug.Log("ClientBehaviour.OnEnable() is called");
		//	Instance = this;
		//	StateMachine = new StateMachine();
		//}

		//private void Start()
		//{
		//	controller = ViewController.Instance;
		//}

		
		public void RpcGamePrepare(M2C_GamePrepare info)
		{
			CurrentRoundStatus = new ClientRoundStatus(info.PlayerIndex, info.Settings);
			var prepareState = new GamePrepareState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				Points = info.Points.ToArray(),
				Names = info.PlayerNames.ToArray()
			};
			StateMachine.ChangeState(prepareState);
		}

		public void RpcRoundStart(M2C_RoundStartInfo info)
		{
			var startState = new RoundStartState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				LocalPlayerHandTiles = info.InitialHandTiles,
				OyaPlayerIndex = info.OyaPlayerIndex,
				Dice = info.Dice,
				Field = info.Field,
				Extra = info.Extra,
				RichiSticks = info.RichiSticks,
				MahjongSetData = info.MahjongSetData,
				Points = info.Points.ToArray()
			};
			StateMachine.ChangeState(startState);
		}

		public void RpcDrawTile(M2C_DrawTileInfo info)
		{
			var drawState = new PlayerDrawState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				PlayerIndex = info.DrawPlayerIndex,
				Tile = info.Tile,
				BonusTurnTime = info.BonusTurnTime,
                Zhenting = info.Zhenting,
                MahjongSetData = info.MahjongSetData,
                Operations = info.Operations.ToArray()
            };
			StateMachine.ChangeState(drawState);
		}
		public void RpcKong(M2C_KongInfo message)
		{
			var kongState = new PlayerKongState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				KongPlayerIndex = message.KongPlayerIndex,
				HandData = message.HandData,
				BonusTurnTime = message.BonusTurnTime,
				Operations = message.Operations.ToArray(),
				MahjongSetData = message.MahjongSetData
			};
			StateMachine.ChangeState(kongState);
		}

		public void RpcBeiDora(M2C_BeiDoraInfo message)
		{
			var beiDoraState = new PlayerBeiDoraState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				BeiDoraPlayerIndex = message.BeiDoraPlayerIndex,
				BeiDoras = message.BeiDoras.ToArray(),
				HandData = message.HandData,
				BonusTurnTime = message.BonusTurnTime,
				Operations = message.Operations.ToArray(),
				MahjongSetData = message.MahjongSetData
			};
			StateMachine.ChangeState(beiDoraState);
		}

		public void RpcDiscardOperation(Event_DiscardTileInfo info)
		{
			var discardOperationState = new PlayerDiscardOperationState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				CurrentPlayerIndex = info.PlayerIndex,
				IsRichiing = info.IsRichiing,
				DiscardingLastDraw = info.DiscardingLastDraw,
				Tile = info.Tile,
				BonusTurnTime = info.BonusTurnTime,
                Zhenting = info.Zhenting,
                Operations = info.Operations.ToArray(),
                HandTiles = info.HandTiles.ToArray(),
                Rivers = info.Rivers.ToArray()
			};
			StateMachine.ChangeState(discardOperationState);
		}

		public void RpcTurnEnd(M2C_TurnEndInfo info)
		{
			var turnEndState = new PlayerTurnEndState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				//PlayerIndex = info.PlayerIndex,
				ChosenOperationType = info.ChosenOperationType,
				Operations = info.Operations.ToArray(),
				Points = info.Points.ToArray(),
				RichiStatus = info.RichiStatus.ToArray(),
				RichiSticks = info.RichiSticks,
				//Zhenting = info.Zhenting,
				MahjongSetData = info.MahjongSetData
			};
			StateMachine.ChangeState(turnEndState);
		}

		public void RpcOperationPerform(M2C_OperationPerformInfo info)
		{
			var operationState = new PlayerOperationPerformState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				//PlayerIndex = info.PlayerIndex,
				OperationPlayerIndex = info.OperationPlayerIndex,
				Operation = info.Operation,
				HandData = info.HandData,
				BonusTurnTime = info.BonusTurnTime,
				Rivers = info.Rivers.ToArray(),
				MahjongSetData = info.MahjongSetData
			};
			StateMachine.ChangeState(operationState);
		}

		public void RpcTsumo(M2C_TsumoInfo info)
		{
			var tsumoState = new PlayerTsumoState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				TsumoPlayerIndex = info.TsumoPlayerIndex,
				TsumoPlayerName = info.TsumoPlayerName,
				TsumoHandData = info.TsumoHandData,
				WinningTile = info.WinningTile,
				DoraIndicators = info.DoraIndicators.ToArray(),
				UraDoraIndicators = info.UraDoraIndicators.ToArray(),
				IsRichi = info.IsRichi,
				TsumoPointInfo = info.TsumoPointInfo,
				TotalPoints = info.TotalPoints
			};
			StateMachine.ChangeState(tsumoState);
		}

		public void RpcRong(M2C_RongInfo message)
		{
			var rongState = new PlayerRongState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				RongPlayerIndices = message.RongPlayerIndices.ToArray(),
				RongPlayerNames = message.RongPlayerNames.ToArray(),
				HandData = message.HandData.ToArray(),
				WinningTile = message.WinningTile,
				DoraIndicators = message.DoraIndicators.ToArray(),
				UraDoraIndicators = message.UraDoraIndicators.ToArray(),
				RongPlayerRichiStatus = message.RongPlayerRichiStatus.ToArray(),
				RongPointInfos = message.RongPointInfos.ToArray(),
				TotalPoints = message.TotalPoints.ToArray()
			};
			StateMachine.ChangeState(rongState);
		}

		public void RpcRoundDraw(M2C_RoundDrawInfo info)
		{
			var roundDrawState = new RoundDrawState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				RoundDrawType = info.RoundDrawType,
				WaitingData = info.WaitingData.ToArray()
			};
			StateMachine.ChangeState(roundDrawState);
		}

		public void RpcPointTransfer(M2C_PointTransferInfo message)
		{
			var transferState = new PointTransferState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				PlayerNames = message.PlayerNames.ToArray(),
				Points = message.Points.ToArray(),
				PointTransfers = message.PointTransfers.ToArray()
			};
			StateMachine.ChangeState(transferState);
		}

		public void RpcGameEnd(M2C_GameEndInfo message)
		{
			var endState = new GameEndState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				PlayerNames = message.PlayerNames.ToArray(),
				Points = message.Points.ToArray(),
				Places = message.Places.ToArray()
			};
			StateMachine.ChangeState(endState);
		}

		public void ClientReady()
		{
			this.DomainScene().GetComponent<SessionComponent>().Session.Send(new ClientReadyEvent());
		}

		public void NextRound()
		{
			this.DomainScene().GetComponent<SessionComponent>().Session.Send(new NextRoundEvent());
		}

		public void OnDiscardTile(Tile tile, bool isLastDraw)
		{
			int bonusTimeLeft = controller.TurnTimeController.StopCountDown();
			OnDiscardTile(tile, isLastDraw, bonusTimeLeft);
		}

		public void OnDiscardTile(Tile tile, bool isLastDraw, int bonusTimeLeft)
		{
			Debug.Log($"Sending request of discarding tile {tile}");
			var info = new Event_DiscardTileInfo
			{
				PlayerIndex = CurrentRoundStatus.LocalPlayerIndex,
				IsRichiing = CurrentRoundStatus.IsRichiing,
				DiscardingLastDraw = isLastDraw,
				Tile = tile,
				BonusTurnTime = bonusTimeLeft
			};
			MahjoneHelper.OnDiscardTile(this.DomainScene(),info);
			var localDiscardState = new LocalDiscardState
			{
				CurrentRoundStatus = CurrentRoundStatus,
				CurrentPlayerIndex = CurrentRoundStatus.LocalPlayerIndex,
				IsRichiing = CurrentRoundStatus.IsRichiing,
				DiscardingLastDraw = isLastDraw,
				Tile = tile
			};
			StateMachine.ChangeState(localDiscardState);
		}

		private void OnInTurnOperationTaken(InTurnOperation operation, int bonusTurnTime)
		{
			var info = new Event_InTurnOperationInfo
			{
				PlayerIndex = CurrentRoundStatus.LocalPlayerIndex,
				Operation = operation,
				BonusTurnTime = bonusTurnTime
			};
			this.DomainScene().GetComponent<SessionComponent>().Session.Send(info);
		}

		public void OnSkipOutTurnOperation(int bonusTurnTime)
		{
			OnOutTurnOperationTaken(new OutTurnOperation {Type = OutTurnOperationType.Skip}, bonusTurnTime);
		}

		public void OnOutTurnOperationTaken(OutTurnOperation operation, int bonusTurnTime)
		{
			var info = new Event_OutTurnOperationInfo
			{
				PlayerIndex = CurrentRoundStatus.LocalPlayerIndex,
				Operation = operation,
				BonusTurnTime = bonusTurnTime
			};
			this.DomainScene().GetComponent<SessionComponent>().Session.Send(info);
		}

		public void OnInTurnSkipButtonClicked()
		{
			Debug.Log("In turn skip button clicked, hide buttons");
			controller.InTurnPanelManager.Close();
		}

		public void OnTsumoButtonClicked(InTurnOperation operation)
		{
			if (operation.Type != InTurnOperationType.Tsumo)
			{
				Debug.LogError(
					$"Cannot send a operation with type {operation.Type} within OnTsumoButtonClicked method");
				return;
			}

			int bonusTimeLeft = controller.TurnTimeController.StopCountDown();
			Debug.Log($"Sending request of tsumo operation with bonus turn time {bonusTimeLeft}");
			OnInTurnOperationTaken(operation, bonusTimeLeft);
			controller.InTurnPanelManager.Close();
		}

		public void OnRichiButtonClicked(InTurnOperation operation)
		{
			if (operation.Type != InTurnOperationType.Richi)
			{
				Debug.LogError(
					$"Cannot send a operation with type {operation.Type} within OnRichiButtonClicked method");
				return;
			}

			// show richi selection panel
			Debug.Log($"Showing richi selection panel, candidates: {string.Join(",", operation.RichiAvailableTiles)}");
			CurrentRoundStatus.SetRichiing(true);
			controller.HandPanelManager.SetCandidates(operation.RichiAvailableTiles);
		}

		public void OnInTurnKongButtonClicked(InTurnOperation[] operationOptions)
		{
			if (operationOptions == null || operationOptions.Length == 0)
			{
				Debug.LogError(
					"The operations are null or empty in OnInTurnKongButtonClicked method, this should not happen.");
				return;
			}

			if (!operationOptions.All(op => op.Type == InTurnOperationType.Kong))
			{
				Debug.LogError("There are incompatible type within OnInTurnKongButtonClicked method");
				return;
			}

			if (operationOptions.Length == 1)
			{
				int bonusTimeLeft = controller.TurnTimeController.StopCountDown();
				Debug.Log($"Sending request of in turn kong operation with bonus turn time {bonusTimeLeft}");
				OnInTurnOperationTaken(operationOptions[0], bonusTimeLeft);
				controller.InTurnPanelManager.Close();
				return;
			}

			// show kong selection panel here
			controller.InTurnPanelManager.ShowBackButton();
			var meldOptions = operationOptions.Select(op => op.Meld);
			controller.MeldSelectionManager.SetMeldOptions(meldOptions.ToArray(), meld =>
			{
				int bonusTimeLeft = controller.TurnTimeController.StopCountDown();
				Debug.Log($"Sending request of in turn kong operation with bonus turn time {bonusTimeLeft}");
				OnInTurnOperationTaken(new InTurnOperation
				{
					Type = InTurnOperationType.Kong,
					Meld = meld
				}, bonusTimeLeft);
				controller.InTurnPanelManager.Close();
				controller.MeldSelectionManager.Close();
			});
		}

		public void OnInTurnButtonClicked(InTurnOperation operation)
		{
			Debug.Log($"Requesting to proceed operation: {operation}");
			int bonusTimeLeft = controller.TurnTimeController.StopCountDown();
			OnInTurnOperationTaken(operation, bonusTimeLeft);
			controller.InTurnPanelManager.Close();
		}

		public void OnInTurnBackButtonClicked(InTurnOperation[] operations)
		{
			controller.InTurnPanelManager.SetOperations(operations);
			controller.MeldSelectionManager.Close();
			controller.HandPanelManager.RemoveCandidates();
			CurrentRoundStatus.SetRichiing(false);
		}

		public void OnOutTurnBackButtonClicked(OutTurnOperation[] operations)
		{
			controller.OutTurnPanelManager.SetOperations(operations);
			controller.MeldSelectionManager.Close();
		}

		public void OnOutTurnButtonClicked(OutTurnOperation operation)
		{
			int bonusTimeLeft = controller.TurnTimeController.StopCountDown();
			Debug.Log($"Sending request of operation {operation} with bonus turn time {bonusTimeLeft}");
			OnOutTurnOperationTaken(operation, bonusTimeLeft);
			controller.OutTurnPanelManager.Close();
		}

		public void OnChowButtonClicked(OutTurnOperation[] operationOptions, OutTurnOperation[] originalOperations)
		{
			if (operationOptions == null || operationOptions.Length == 0)
			{
				Debug.LogError(
					"The operations are null or empty in OnChowButtonClicked method, this should not happen.");
				return;
			}

			if (!operationOptions.All(op => op.Type == OutTurnOperationType.Chow))
			{
				Debug.LogError("There are incompatible type within OnChowButtonClicked method");
				return;
			}

			if (operationOptions.Length == 1)
			{
				int bonusTimeLeft = controller.TurnTimeController.StopCountDown();
				Debug.Log($"Sending request of chow operation with bonus turn time {bonusTimeLeft}");
				OnOutTurnOperationTaken(operationOptions[0], bonusTimeLeft);
				controller.OutTurnPanelManager.Close();
				return;
			}

			// chow selection logic here
			controller.OutTurnPanelManager.ShowBackButton();
			var meldOptions = operationOptions.Select(op => op.Meld);
			controller.MeldSelectionManager.SetMeldOptions(meldOptions.ToArray(), meld =>
			{
				int bonusTimeLeft = controller.TurnTimeController.StopCountDown();
				Debug.Log($"Sending request of in turn kong operation with bonus turn time {bonusTimeLeft}");
				OnOutTurnOperationTaken(new OutTurnOperation
				{
					Type = OutTurnOperationType.Chow,
					Meld = meld
				}, bonusTimeLeft);
				controller.InTurnPanelManager.Close();
				controller.MeldSelectionManager.Close();
			});
		}

		public void OnPongButtonClicked(OutTurnOperation[] operationOptions, OutTurnOperation[] originalOperations)
		{
			if (operationOptions == null || operationOptions.Length == 0)
			{
				Debug.LogError(
					"The operations are null or empty in OnPongButtonClicked method, this should not happen.");
				return;
			}

			if (!operationOptions.All(op => op.Type == OutTurnOperationType.Pong))
			{
				Debug.LogError("There are incompatible type within OnPongButtonClicked method");
				return;
			}

			if (operationOptions.Length == 1)
			{
				int bonusTimeLeft = controller.TurnTimeController.StopCountDown();
				Debug.Log($"Sending request of kong operation with bonus turn time {bonusTimeLeft}");
				OnOutTurnOperationTaken(operationOptions[0], bonusTimeLeft);
				controller.OutTurnPanelManager.Close();
				return;
			}

			// pong selection logic here
			controller.OutTurnPanelManager.ShowBackButton();
			var meldOptions = operationOptions.Select(op => op.Meld);
			controller.MeldSelectionManager.SetMeldOptions(meldOptions.ToArray(), meld =>
			{
				int bonusTimeLeft = controller.TurnTimeController.StopCountDown();
				Debug.Log($"Sending request of in turn kong operation with bonus turn time {bonusTimeLeft}");
				OnOutTurnOperationTaken(new OutTurnOperation
				{
					Type = OutTurnOperationType.Pong,
					Meld = meld
				}, bonusTimeLeft);
				controller.InTurnPanelManager.Close();
				controller.MeldSelectionManager.Close();
			});
		}

		//public override void OnLeftRoom()
		//{
		//	// todo
		//}

		//public override void OnLeftLobby()
		//{
		//	// todo
		//}

		//public override void OnPlayerLeftRoom(Player otherPlayer)
		//{
		//	// todo
		//}

		//public override void OnPlayerEnteredRoom(Player newPlayer)
		//{
		//	// todo
		//}
	}
}