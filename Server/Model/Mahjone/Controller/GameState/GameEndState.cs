using Mahjong.Logic;
using System.Linq;
namespace ET
{
    public class GameEndState : ServerState
	{
		public override void OnServerStateEnter()
		{
			var pointsAndPlaces = MahjongLogic.SortPointsAndPlaces(CurrentRoundStatus.Points);
			var names = CurrentRoundStatus.PlayerNames;
			var points = CurrentRoundStatus.Points;
			var places = pointsAndPlaces.Select(v => v.Value).ToList();
			var info = new M2C_GameEndInfo
			{
				PlayerNames = names,
				Points = points,
				Places = places
			};
			Game.EventSystem.Publish(new ET.EventType.GameEnd() { actorIds=players,msg=info}).Coroutine();
			this.Domain.GetComponent<MJRoomManagerComponent>().RemoveRoom(this.Parent.Parent.InstanceId);
		}
		public override void OnServerStateExit()
        {
        }

    }
}