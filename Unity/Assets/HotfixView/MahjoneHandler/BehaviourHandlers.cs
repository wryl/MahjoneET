using System;
using System.Collections.Generic;

namespace ET
{
    public class BehaviourHandlers
    {

    }
	/// <summary>
	/// 推送消息.表示准备
	/// </summary>
	[MessageHandler]
	public class M2C_GamePrepareHandler : AMHandler<M2C_GamePrepare>
	{
		protected override async ETTask Run(Session session, M2C_GamePrepare message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcGamePrepare(message);
		}
	}
	/// <summary>
	/// 推送消息.表示开始游戏
	/// </summary>
	[MessageHandler]
	public class M2C_RoundStartInfoHandler : AMHandler<M2C_RoundStartInfo>
	{
		protected override async ETTask Run(Session session, M2C_RoundStartInfo message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcRoundStart(message);
		}
	}
	/// <summary>
	/// 推送消息.表示开始游戏
	/// </summary>
	[MessageHandler]
	public class M2C_DrawTileInfoHandler : AMHandler<M2C_DrawTileInfo>
	{
		protected override async ETTask Run(Session session, M2C_DrawTileInfo message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcDrawTile(message);
		}
	}
	
   [MessageHandler]
	public class Event_DiscardTileInfoHandler : AMHandler<Event_DiscardTileInfo>
	{
		protected override async ETTask Run(Session session, Event_DiscardTileInfo message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcDiscardOperation(message);
		}
	}
	
   [MessageHandler]
	public class M2C_TurnEndInfoHandler : AMHandler<M2C_TurnEndInfo>
	{
		protected override async ETTask Run(Session session, M2C_TurnEndInfo message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcTurnEnd(message);
		}
	}
	
   [MessageHandler]
	public class M2C_RoundDrawInfoHandler : AMHandler<M2C_RoundDrawInfo>
	{
		protected override async ETTask Run(Session session, M2C_RoundDrawInfo message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcRoundDraw(message);
		}
	}
	[MessageHandler]
	public class M2C_GameEndInfoHandler : AMHandler<M2C_GameEndInfo>
	{
		protected override async ETTask Run(Session session, M2C_GameEndInfo message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcGameEnd(message);
		}
	}
	
   [MessageHandler]
	public class M2C_PointTransferInfoHandler : AMHandler<M2C_PointTransferInfo>
	{
		protected override async ETTask Run(Session session, M2C_PointTransferInfo message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcPointTransfer(message);
		}
	}
	
   [MessageHandler]
	public class M2C_OperationPerformInfoHandler : AMHandler<M2C_OperationPerformInfo>
	{
		protected override async ETTask Run(Session session, M2C_OperationPerformInfo message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcOperationPerform(message);
		}
	}
	[MessageHandler]
	public class M2C_RongInfoHandler : AMHandler<M2C_RongInfo>
	{
		protected override async ETTask Run(Session session, M2C_RongInfo message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcRong(message);
		}
	}
	[MessageHandler]
	public class M2C_TsumoInfoHandler : AMHandler<M2C_TsumoInfo>
	{
		protected override async ETTask Run(Session session, M2C_TsumoInfo message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcTsumo(message);
		}
	}
	[MessageHandler]
	public class M2C_KongInfoHandler : AMHandler<M2C_KongInfo>
	{
		protected override async ETTask Run(Session session, M2C_KongInfo message)
		{
			session.DomainScene().GetComponent<GamePlay.Client.Controller.ClientBehaviour>().RpcKong(message);
		}
	}
}
