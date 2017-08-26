﻿using System;

namespace Model
{
	public class OuterMessageDispatcher: IMessageDispatcher
	{
		public async void Dispatch(Session session, ushort opcode, int offset, byte[] messageBytes, object message)
		{
			// gate session收到actor消息直接转发给actor自己去处理
			if (message is AActorMessage aActorMessage)
			{
				long unitId = session.GetComponent<SessionPlayerComponent>().Player.UnitId;
				aActorMessage.Id = unitId;
				ActorProxy actorProxy = Game.Scene.GetComponent<ActorProxyComponent>().Get(aActorMessage.Id);
				actorProxy.Send(aActorMessage);
				return;
			}

			// gate session收到actor rpc消息，先向actor 发送rpc请求，再将请求结果返回客户端
			if (message is AActorRequest aActorRequest)
			{
				ActorProxy actorProxy = Game.Scene.GetComponent<ActorProxyComponent>().Get(aActorRequest.Id);
				aActorRequest.Id = session.GetComponent<SessionPlayerComponent>().Player.Id;
				uint rpcId = aActorRequest.RpcId;
				AActorResponse aActorResponse = await actorProxy.Call<AActorResponse>(aActorRequest);
				aActorResponse.RpcId = rpcId;
				session.Reply(aActorResponse);
				return;
			}

			if (message is AMessage)
			{
				Game.Scene.GetComponent<MessageDispatherComponent>().Handle(session, message);
				return;
			}

			throw new Exception($"message type error: {message.GetType().FullName}");
		}
	}
}