namespace ET
{
    namespace EventType
    {
        public struct AppStart
        {
        }
        
        public struct AfterCreateZoneScene
        {
            public Scene ZoneScene;
        }
        
        public struct AfterCreateLoginScene
        {
            public Scene LoginScene;
        }

        public struct AppStartInitFinish
        {
            public Scene ZoneScene;
        }

        public struct LoginFinish
        {
            public Scene ZoneScene;
        }

        public struct LoadingBegin
        {
            public Scene Scene;
        }

        public struct LoadingFinish
        {
            public Scene Scene;
        }

        public struct EnterMapFinish
        {
            public Scene ZoneScene;
        }

        public struct AfterUnitCreate
        {
            public Unit Unit;
        }
        public struct CreateRoomPanel
        {
            public Scene Scene;
        }
        public struct LeaveCreateRoomPanel
        {
            public Scene Scene;
        }
        public struct CreateRoomCanvas
        {
            public Scene Scene;
            public MJ_RoomInfo MJ_RoomInfo;
            public PlayerInRoom myplayer;
        }
        public struct JoinRoomCanvas
        {
            public Scene Scene;
            public MJ_RoomInfo MJ_RoomInfo;
            public PlayerInRoom myplayer;
        }
        public struct RemoveRoomCanvas
        {
            public Scene Scene;
        }
        public struct RoomPlayerUpdate
        {
            public MJ_RoomPlayerUpdate PlayerChange;
        }
        public struct RoomPlayerChange
        {
            public Scene Scene;
            public PlayerInRoom PlayerInfo;
            public bool IsMaster;
            public Mahjong.Model.PlayerChangeState ChangeState;
        }
        public struct StartGame
        {
            public Scene Scene;
        }
        public struct GameEnd
        {
            public Scene Scene;
        }
    }
}