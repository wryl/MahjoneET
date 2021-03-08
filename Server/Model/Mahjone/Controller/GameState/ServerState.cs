using Common.StateMachine.Interfaces;
using GamePlay.Server.Model;
using Mahjong.Model;
using System.Collections.Generic;
namespace ET
{
    public abstract class ServerState:Entity,IState
	{
		public ServerRoundStatus CurrentRoundStatus=>GetParent<MahjoneBehaviourComponent>().CurrentRoundStatus;
		public GameSetting gameSettings=> GetParent<MahjoneBehaviourComponent>().CurrentRoundStatus.GameSettings;
		public List<long> players=> GetParent<MahjoneBehaviourComponent>().CurrentRoundStatus.PlayerActorNumbers;
		public int totalPlayers => GetParent<MahjoneBehaviourComponent>().CurrentRoundStatus.TotalPlayers;
		public MahjoneBehaviourComponent ParentBehaviour => GetParent<MahjoneBehaviourComponent>();
		public void OnStateEnter()
		{
			OnServerStateEnter();
		}

		public void OnStateExit()
		{
			ET.Log.Debug($"Server exits {GetType().Name}");
			OnServerStateExit();
			//Dispose();
		}
		public abstract void OnServerStateEnter();
		public abstract void OnServerStateExit();
	}
}