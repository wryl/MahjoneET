using System.Collections.Generic;
using Mahjong.Model;

namespace GamePlay.Client.Controller.GameState
{
	public class InitHandAfterRongState : ClientState
	{
		public IList<Tile> LocalPlayerHandTiles;
		public MahjongSetData MahjongSetData;
		public int[] Points;
		public bool[] richiStatus;
		public int playerIndex;
		public override void OnClientStateEnter()
		{
			var controller = ViewController.Instance;

			CurrentRoundStatus.UpdateRichiStatus(richiStatus);
			// update local tiles
			
            if (CurrentRoundStatus.GetPlayerIndex(CurrentRoundStatus.CurrentPlaceIndex)==playerIndex)
            {
				CurrentRoundStatus.SetMahjongSetData(MahjongSetData);
				// clear claimed open melds
				controller.TableTilesManager.ClearMelds(CurrentRoundStatus.CurrentPlaceIndex);
				CurrentRoundStatus.ClearLastDraws();
				CurrentRoundStatus.CheckLocalHandTiles(LocalPlayerHandTiles);
				CurrentRoundStatus.SetZhenting(false);
			}
			// update other player's hand tile count
			//for (int placeIndex = 0; placeIndex < 4; placeIndex++)
			//{
			//	int playerIndex = CurrentRoundStatus.GetPlayerIndex(placeIndex);
			//	if (playerIndex < CurrentRoundStatus.TotalPlayers)
			//		CurrentRoundStatus.SetHandTiles(placeIndex, LocalPlayerHandTiles.Count);
			//	else
			//		CurrentRoundStatus.SetHandTiles(placeIndex, 0);
			//}

			// sync points
			//CurrentRoundStatus.UpdatePoints(Points);
			// update ui statement

			// reset yama
			controller.YamaManager.ResetAllTiles();
			// stand hand tiles
			controller.TableTilesManager.StandUp();

			// send ready message
			ClientBehaviour.Instance.ClientReady();
		}

		public override void OnClientStateExit()
		{
			controller.HandPanelManager.Show();
			controller.HandPanelManager.UnlockTiles();
		}

		public override void OnStateUpdate()
		{
		}
	}
}