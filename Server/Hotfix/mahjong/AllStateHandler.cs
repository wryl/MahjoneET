using System;

/// <summary>
/// 所有状态机的handler
/// </summary>
namespace ET
{
    [ActorMessageHandler]
    public class MJ_LoadCompleteGameHandler : AMActorLocationHandler<MJRoomPlayerComponent, MJ_LoadComplete>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, MJ_LoadComplete message)
        {
            if (entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().StateMachine.CurrentStateType == typeof(WaitForLoadingState))
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<WaitForLoadingState>().OnEvent(entity.PlayerInfo.UserId);
            await ETTask.CompletedTask;
        }
    }

    [ActorMessageHandler]
    public class ClientReadyEventHandler : AMActorLocationHandler<MJRoomPlayerComponent, ClientReadyEvent>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, ClientReadyEvent message)
        {
            var currstate = entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().StateMachine.CurrentStateType;
            Log.Debug($"ClientReadyEventHandler:{currstate.FullName}");
            if (currstate == typeof(GamePrepareState))
            {
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<GamePrepareState>().OnEvent(entity.PlayerInfo.UserId);
            }
            else if (currstate == typeof(PlayerRongState))
            {
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<PlayerRongState>().OnEvent(entity.PlayerInfo.UserId);
            }

            else if (currstate == typeof(PlayerTsumoState))
            {
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<PlayerTsumoState>().OnEvent(entity.PlayerInfo.UserId);
            }
            else if (currstate == typeof(RoundStartState))
            {
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<RoundStartState>().OnEvent(entity.PlayerInfo.UserId);
            }
            await ETTask.CompletedTask;

        }
    }
    [ActorMessageHandler]
    public class Event_SelectTileInfoHandler : AMActorLocationHandler<MJRoomPlayerComponent, Event_SelectTileInfo>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, Event_SelectTileInfo message)
        {
            if (entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().StateMachine.CurrentStateType == typeof(PrePlayerDrawTileState))
            {
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<PrePlayerDrawTileState>().OnSelectTileEvent(message);
            }
            await ETTask.CompletedTask;
        }
    }
    [ActorMessageHandler]
    public class Event_DiscardTileInfoHandler : AMActorLocationHandler<MJRoomPlayerComponent, Event_DiscardTileInfo>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, Event_DiscardTileInfo message)
        {
            if (entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().StateMachine.CurrentStateType == typeof(WaitForDiscardTileState))
            {
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<WaitForDiscardTileState>().OnDiscardTileEvent(message);
            }
            else if (entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().StateMachine.CurrentStateType == typeof(PlayerDrawTileState))
            {
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<PlayerDrawTileState>().OnDiscardTileEvent(message);
            }
            await ETTask.CompletedTask;
        }
    }
    [ActorMessageHandler]
    public class Event_InTurnOperationInfoHandler : AMActorLocationHandler<MJRoomPlayerComponent, Event_InTurnOperationInfo>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, Event_InTurnOperationInfo message)
        {
            if (entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().StateMachine.CurrentStateType == typeof(PlayerDrawTileState))
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<PlayerDrawTileState>().OnInTurnOperationEvent(message);
            await ETTask.CompletedTask;
        }
    }
    [ActorMessageHandler]
    public class Event_OutTurnOperationInfoHandler : AMActorLocationHandler<MJRoomPlayerComponent, Event_OutTurnOperationInfo>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, Event_OutTurnOperationInfo message)
        {
            var currstate = entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().StateMachine.CurrentStateType;
            if (currstate == typeof(PlayerDiscardTileState))
            {
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<PlayerDiscardTileState>().OnOutTurnOperationEvent(message.PlayerIndex, message.Operation, message.BonusTurnTime);
            }
            else if (currstate == typeof(PlayerKongState))
            {
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<PlayerKongState>().OnOutTurnOperationEvent(message);

            }
            await ETTask.CompletedTask;
        }
    }
    [ActorMessageHandler]
    public class NextRoundEventHandler : AMActorLocationHandler<MJRoomPlayerComponent, NextRoundEvent>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, NextRoundEvent message)
        {
            if (entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().StateMachine.CurrentStateType == typeof(PointTransferState))
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<PointTransferState>().OnNextRoundEvent(entity.PlayerInfo.UserId);
            await ETTask.CompletedTask;
        }
    }
    [ActorMessageHandler]
    public class ClientSelectTilesReadyHandler : AMActorLocationHandler<MJRoomPlayerComponent, ClientSelectTilesReady>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, ClientSelectTilesReady message)
        {
            if (entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().StateMachine.CurrentStateType == typeof(SelectTilesState))
                entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().GetComponent<SelectTilesState>().OnEvent(entity.PlayerInfo.UserId, message.InitialTiles);
            await ETTask.CompletedTask;
        }
    }
    [ActorMessageHandler]
    public class ClientChangeTileConfHandler : AMActorLocationHandler<MJRoomPlayerComponent, ClientChangeTileConf>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, ClientChangeTileConf message)
        {
            var x = entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>();
            var playindex = x.CurrentRoundStatus.PlayerActorNumbers.FindIndex(e => e == entity.PlayerInfo.GateSessionId);
            entity.GetParent<MJRoomComponent>().GetComponent<MahjoneBehaviourComponent>().CurrentRoundStatus.ChangeHandAfterRong[playindex] = message.IsChangeTileAfterRong;
            await ETTask.CompletedTask;
        }
    }
}