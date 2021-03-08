using GamePlay.Server.Model;
using Mahjong.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GamePlay.Client.Controller.GameState
{
	public class PrePlayerDrawState : ClientState
	{
		public int PlayerIndex;
		public List<Tile> Tiles;
		public int BonusTurnTime;

		public override void OnClientStateEnter()
		{
			if (CurrentRoundStatus.LocalPlayerIndex == PlayerIndex)
				HandleLocalPlayerDraw();
			//else
			//	HandleOtherPlayerDraw();
		}

		private void HandleLocalPlayerDraw()
		{

			CurrentRoundStatus.SetCurrentPlaceIndex(PlayerIndex);
			var placeIndex = CurrentRoundStatus.CurrentPlaceIndex;
			Assert.IsTrue(placeIndex == 0);
			CurrentRoundStatus.PreDrawTile(Tiles);
			controller.TurnTimeController.StartCountDown(CurrentRoundStatus.GameSetting.BaseTurnTime, BonusTurnTime, () => {
				Debug.Log("≥¨ ±¡À");

			});
		}

		private void HandleOtherPlayerDraw()
		{
			CurrentRoundStatus.SetCurrentPlaceIndex(PlayerIndex);
			int placeIndex = CurrentRoundStatus.CurrentPlaceIndex;

			CurrentRoundStatus.PreDrawTile(Tiles);
			Debug.Log($"LastDraws: {string.Join(",", CurrentRoundStatus.LastDraws)}");
		}

		public override void OnClientStateExit()
		{
			CurrentRoundStatus.ClearSelectTiles();
			controller.TurnTimeController.StopCountDown();
		}

		public override void OnStateUpdate()
		{
		}
	}
}