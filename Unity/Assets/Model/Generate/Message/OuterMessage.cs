using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(typeof(M2C_TestResponse))]
	[Message(OuterOpcode.C2M_TestRequest)]
	[ProtoContract]
	public partial class C2M_TestRequest: IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string request { get; set; }

	}

	[Message(OuterOpcode.M2C_TestResponse)]
	[ProtoContract]
	public partial class M2C_TestResponse: IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string response { get; set; }

	}

	[ResponseType(typeof(Actor_TransferResponse))]
	[Message(OuterOpcode.Actor_TransferRequest)]
	[ProtoContract]
	public partial class Actor_TransferRequest: IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MapIndex { get; set; }

	}

	[Message(OuterOpcode.Actor_TransferResponse)]
	[ProtoContract]
	public partial class Actor_TransferResponse: IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(typeof(G2C_EnterMap))]
	[Message(OuterOpcode.C2G_EnterMap)]
	[ProtoContract]
	public partial class C2G_EnterMap: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterMap)]
	[ProtoContract]
	public partial class G2C_EnterMap: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

// 自己的unit id
// 自己的unit id
		[ProtoMember(1)]
		public long UnitId { get; set; }

// 所有的unit
// 所有的unit
		[ProtoMember(2)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[Message(OuterOpcode.UnitInfo)]
	[ProtoContract]
	public partial class UnitInfo
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.M2C_CreateUnits)]
	[ProtoContract]
	public partial class M2C_CreateUnits: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[Message(OuterOpcode.Frame_ClickMap)]
	[ProtoContract]
	public partial class Frame_ClickMap: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long Id { get; set; }

		[ProtoMember(1)]
		public float X { get; set; }

		[ProtoMember(2)]
		public float Y { get; set; }

		[ProtoMember(3)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	[ProtoContract]
	public partial class M2C_PathfindingResult: IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

		[ProtoMember(5)]
		public List<float> Xs = new List<float>();

		[ProtoMember(6)]
		public List<float> Ys = new List<float>();

		[ProtoMember(7)]
		public List<float> Zs = new List<float>();

	}

	[ResponseType(typeof(G2C_Ping))]
	[Message(OuterOpcode.C2G_Ping)]
	[ProtoContract]
	public partial class C2G_Ping: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_Ping)]
	[ProtoContract]
	public partial class G2C_Ping: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Time { get; set; }

	}

	[Message(OuterOpcode.G2C_Test)]
	[ProtoContract]
	public partial class G2C_Test: IMessage
	{
	}

	[ResponseType(typeof(M2C_Reload))]
	[Message(OuterOpcode.C2M_Reload)]
	[ProtoContract]
	public partial class C2M_Reload: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.M2C_Reload)]
	[ProtoContract]
	public partial class M2C_Reload: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(typeof(R2C_Login))]
	[Message(OuterOpcode.C2R_Login)]
	[ProtoContract]
	public partial class C2R_Login: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Login)]
	[ProtoContract]
	public partial class R2C_Login: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Address { get; set; }

		[ProtoMember(2)]
		public long Key { get; set; }

		[ProtoMember(3)]
		public long GateId { get; set; }

	}

	[ResponseType(typeof(G2C_LoginGate))]
	[Message(OuterOpcode.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginGate)]
	[ProtoContract]
	public partial class G2C_LoginGate: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.G2C_TestHotfixMessage)]
	[ProtoContract]
	public partial class G2C_TestHotfixMessage: IMessage
	{
		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[ResponseType(typeof(M2C_TestActorResponse))]
	[Message(OuterOpcode.C2M_TestActorRequest)]
	[ProtoContract]
	public partial class C2M_TestActorRequest: IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[Message(OuterOpcode.M2C_TestActorResponse)]
	[ProtoContract]
	public partial class M2C_TestActorResponse: IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[Message(OuterOpcode.PlayerInfo)]
	[ProtoContract]
	public partial class PlayerInfo: IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[ResponseType(typeof(G2C_PlayerInfo))]
	[Message(OuterOpcode.C2G_PlayerInfo)]
	[ProtoContract]
	public partial class C2G_PlayerInfo: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_PlayerInfo)]
	[ProtoContract]
	public partial class G2C_PlayerInfo: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public PlayerInfo PlayerInfo { get; set; }

		[ProtoMember(2)]
		public List<PlayerInfo> PlayerInfos = new List<PlayerInfo>();

		[ProtoMember(3)]
		public List<string> TestRepeatedString = new List<string>();

		[ProtoMember(4)]
		public List<int> TestRepeatedInt32 = new List<int>();

		[ProtoMember(5)]
		public List<long> TestRepeatedInt64 = new List<long>();

	}

//房间内玩家
	[Message(OuterOpcode.PlayerInRoom)]
	[ProtoContract]
	public partial class PlayerInRoom: IDataWithInsId
	{
		[ProtoMember(95)]
		public long InsId { get; set; }

		[ProtoMember(96)]
		public long GateSessionId { get; set; }

		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public string NickName { get; set; }

		[ProtoMember(3)]
		public string Icon { get; set; }

		[ProtoMember(4)]
		public bool IsOnline { get; set; }

		[ProtoMember(5)]
		public bool IsMaster { get; set; }

		[ProtoMember(6)]
		public bool IsReady { get; set; }

	}

//	房间列表
	[ResponseType(typeof(G2C_RoomList))]
	[Message(OuterOpcode.C2G_RoomList)]
	[ProtoContract]
	public partial class C2G_RoomList: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_RoomList)]
	[ProtoContract]
	public partial class G2C_RoomList: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<MJ_RoomInfoInList> rooms = new List<MJ_RoomInfoInList>();

	}

//	创建房间
	[ResponseType(typeof(G2C_CreateRoom))]
	[Message(OuterOpcode.C2G_CreateRoom)]
	[ProtoContract]
	public partial class C2G_CreateRoom: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public Mahjong.Model.GameSetting Setting { get; set; }

		[ProtoMember(2)]
		public string RoomName { get; set; }

	}

//	创建房间返回
	[Message(OuterOpcode.G2C_CreateRoom)]
	[ProtoContract]
	public partial class G2C_CreateRoom: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public PlayerInRoom myplayer { get; set; }

		[ProtoMember(2)]
		public MJ_RoomInfo RoomInfo { get; set; }

	}

//	加入房间
	[ResponseType(typeof(G2C_JoinRoom))]
	[Message(OuterOpcode.C2G_JoinRoom)]
	[ProtoContract]
	public partial class C2G_JoinRoom: IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long RoomId { get; set; }

	}

	[Message(OuterOpcode.G2C_JoinRoom)]
	[ProtoContract]
	public partial class G2C_JoinRoom: IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public PlayerInRoom myplayer { get; set; }

		[ProtoMember(2)]
		public MJ_RoomInfo RoomInfo { get; set; }

	}

	[Message(OuterOpcode.MJ_NormalSetting)]
	[ProtoContract]
	public partial class MJ_NormalSetting
	{
		[ProtoMember(1)]
		public int testid { get; set; }

	}

//部分房间数据,用于列表显示
	[Message(OuterOpcode.MJ_RoomInfoInList)]
	[ProtoContract]
	public partial class MJ_RoomInfoInList: IDataWithInsId
	{
		[ProtoMember(95)]
		public long InsId { get; set; }

		[ProtoMember(1)]
		public long RoomId { get; set; }

		[ProtoMember(2)]
		public Mahjong.Model.GameMode GameMode { get; set; }

		[ProtoMember(3)]
		public Mahjong.Model.GamePlayers GamePlayers { get; set; }

		[ProtoMember(4)]
		public Mahjong.Model.RoundCount RoundCount { get; set; }

		[ProtoMember(5)]
		public int NowPlayerNum { get; set; }

		[ProtoMember(6)]
		public string RoomName { get; set; }

	}

//完整房间数据.用于数据初始化
	[Message(OuterOpcode.MJ_RoomInfo)]
	[ProtoContract]
	public partial class MJ_RoomInfo
	{
		[ProtoMember(1)]
		public long RoomId { get; set; }

		[ProtoMember(2)]
		public Mahjong.Model.GameSetting Setting { get; set; }

		[ProtoMember(3)]
		public List<PlayerInRoom> PlayerInRooms = new List<PlayerInRoom>();

		[ProtoMember(4)]
		public string RoomName { get; set; }

	}

	[Message(OuterOpcode.MJ_Setting)]
	[ProtoContract]
	public partial class MJ_Setting
	{
		[ProtoMember(80)]
		public string RoomName { get; set; }

		[ProtoMember(81)]
		public long RoomId { get; set; }

		[ProtoMember(1)]
		public Mahjong.Model.GameSetting Setting { get; set; }

	}

	[Message(OuterOpcode.MJ_RoomPlayerUpdate)]
	[ProtoContract]
	public partial class MJ_RoomPlayerUpdate: IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<PlayerInRoom> players = new List<PlayerInRoom>();

	}

	[Message(OuterOpcode.G2C_RoomPlayerChange)]
	[ProtoContract]
	public partial class G2C_RoomPlayerChange: IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PlayerInRoom player { get; set; }

		[ProtoMember(2)]
		public Mahjong.Model.PlayerChangeState ChangeState { get; set; }

	}

	[Message(OuterOpcode.MJ_RoomPlayerChangeRequest)]
	[ProtoContract]
	public partial class MJ_RoomPlayerChangeRequest: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PlayerInRoom player { get; set; }

	}

	[ResponseType(typeof(MJ_QuitRoomResponse))]
	[Message(OuterOpcode.MJ_QuitRoomRequest)]
	[ProtoContract]
	public partial class MJ_QuitRoomRequest: IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.MJ_QuitRoomResponse)]
	[ProtoContract]
	public partial class MJ_QuitRoomResponse: IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.MJ_KickPlayer)]
	[ProtoContract]
	public partial class MJ_KickPlayer: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long KickId { get; set; }

	}

//客户端请求开始游戏
	[ResponseType(typeof(MJ_StartGameResponse))]
	[Message(OuterOpcode.MJ_StartGameRequest)]
	[ProtoContract]
	public partial class MJ_StartGameRequest: IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.MJ_StartGameResponse)]
	[ProtoContract]
	public partial class MJ_StartGameResponse: IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.M2C_RoomClose)]
	[ProtoContract]
	public partial class M2C_RoomClose: IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

//通知客户端加载游戏资源
	[Message(OuterOpcode.M2C_EnterMahjoneGame)]
	[ProtoContract]
	public partial class M2C_EnterMahjoneGame: IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

//玩家loading完毕 WaitForLoading=>GamePrepare
	[Message(OuterOpcode.MJ_LoadComplete)]
	[ProtoContract]
	public partial class MJ_LoadComplete: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

//通知玩家准备开始
	[Message(OuterOpcode.M2C_GamePrepare)]
	[ProtoContract]
	public partial class M2C_GamePrepare: IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int PlayerIndex { get; set; }

		[ProtoMember(2)]
		public List<int> Points = new List<int>();

		[ProtoMember(3)]
		public List<string> PlayerNames = new List<string>();

		[ProtoMember(4)]
		public Mahjong.Model.GameSetting Settings { get; set; }

	}

	[Message(OuterOpcode.ClientReadyEvent)]
	[ProtoContract]
	public partial class ClientReadyEvent: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.NextRoundEvent)]
	[ProtoContract]
	public partial class NextRoundEvent: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_RoundStartInfo)]
	[ProtoContract]
	public partial class M2C_RoundStartInfo: IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int PlayerIndex { get; set; }

		[ProtoMember(2)]
		public int Field { get; set; }

		[ProtoMember(3)]
		public int Dice { get; set; }

		[ProtoMember(4)]
		public int Extra { get; set; }

		[ProtoMember(5)]
		public int RichiSticks { get; set; }

		[ProtoMember(6)]
		public int OyaPlayerIndex { get; set; }

		[ProtoMember(7)]
		public List<int> Points = new List<int>();

		[ProtoMember(8)]
		public List<Mahjong.Model.Tile> InitialHandTiles = new List<Mahjong.Model.Tile>();

		[ProtoMember(9)]
		public Mahjong.Model.MahjongSetData MahjongSetData { get; set; }

	}

	[Message(OuterOpcode.M2C_DrawTileInfo)]
	[ProtoContract]
	public partial class M2C_DrawTileInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public int DrawPlayerIndex { get; set; }

		[ProtoMember(3)]
		public Mahjong.Model.Tile Tile { get; set; }

		[ProtoMember(4)]
		public int BonusTurnTime { get; set; }

		[ProtoMember(6)]
		public bool Zhenting { get; set; }

		[ProtoMember(7)]
		public List<GamePlay.Server.Model.InTurnOperation> Operations = new List<GamePlay.Server.Model.InTurnOperation>();

		[ProtoMember(9)]
		public Mahjong.Model.MahjongSetData MahjongSetData { get; set; }

	}

	[Message(OuterOpcode.M2C_PreDrawTileInfo)]
	[ProtoContract]
	public partial class M2C_PreDrawTileInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public int DrawPlayerIndex { get; set; }

		[ProtoMember(3)]
		public List<Mahjong.Model.Tile> Tiles = new List<Mahjong.Model.Tile>();

		[ProtoMember(4)]
		public int BonusTurnTime { get; set; }

	}

// 玩家选牌
	[Message(OuterOpcode.Event_SelectTileInfo)]
	[ProtoContract]
	public partial class Event_SelectTileInfo: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int PlayerIndex { get; set; }

		[ProtoMember(2)]
		public int SelectIndex { get; set; }

		[ProtoMember(3)]
		public int BonusTurnTime { get; set; }

	}

	[Message(OuterOpcode.M2C_DiscardTileInfo)]
	[ProtoContract]
	public partial class M2C_DiscardTileInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int PlayerIndex { get; set; }

		[ProtoMember(2)]
		public bool IsRichiing { get; set; }

		[ProtoMember(3)]
		public bool DiscardingLastDraw { get; set; }

		[ProtoMember(4)]
		public Mahjong.Model.Tile Tile { get; set; }

		[ProtoMember(5)]
		public int BonusTurnTime { get; set; }

		[ProtoMember(6)]
		public bool Zhenting { get; set; }

		[ProtoMember(7)]
		public List<GamePlay.Server.Model.OutTurnOperation> Operations = new List<GamePlay.Server.Model.OutTurnOperation>();

		[ProtoMember(8)]
		public List<Mahjong.Model.Tile> HandTiles = new List<Mahjong.Model.Tile>();

		[ProtoMember(9)]
		public List<Mahjong.Model.RiverData> Rivers = new List<Mahjong.Model.RiverData>();

	}

	[Message(OuterOpcode.Event_DiscardTileInfo)]
	[ProtoContract]
	public partial class Event_DiscardTileInfo: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int PlayerIndex { get; set; }

		[ProtoMember(2)]
		public bool IsRichiing { get; set; }

		[ProtoMember(3)]
		public bool DiscardingLastDraw { get; set; }

		[ProtoMember(4)]
		public Mahjong.Model.Tile Tile { get; set; }

		[ProtoMember(5)]
		public int BonusTurnTime { get; set; }

	}

	[Message(OuterOpcode.M2C_TurnEndInfo)]
	[ProtoContract]
	public partial class M2C_TurnEndInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int PlayerIndex { get; set; }

		[ProtoMember(2)]
		public GamePlay.Server.Model.OutTurnOperationType ChosenOperationType { get; set; }

		[ProtoMember(3)]
		public List<GamePlay.Server.Model.OutTurnOperation> Operations = new List<GamePlay.Server.Model.OutTurnOperation>();

		[ProtoMember(4)]
		public List<int> Points = new List<int>();

		[ProtoMember(5)]
		public List<bool> RichiStatus = new List<bool>();

		[ProtoMember(6)]
		public int RichiSticks { get; set; }

		[ProtoMember(7)]
		public bool Zhenting { get; set; }

		[ProtoMember(9)]
		public Mahjong.Model.MahjongSetData MahjongSetData { get; set; }

	}

	[Message(OuterOpcode.M2C_OperationPerformInfo)]
	[ProtoContract]
	public partial class M2C_OperationPerformInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int OperationPlayerIndex { get; set; }

		[ProtoMember(2)]
		public Mahjong.Model.PlayerHandData HandData { get; set; }

		[ProtoMember(3)]
		public GamePlay.Server.Model.OutTurnOperation Operation { get; set; }

		[ProtoMember(4)]
		public int BonusTurnTime { get; set; }

		[ProtoMember(5)]
		public List<Mahjong.Model.RiverData> Rivers = new List<Mahjong.Model.RiverData>();

		[ProtoMember(9)]
		public Mahjong.Model.MahjongSetData MahjongSetData { get; set; }

	}

	[Message(OuterOpcode.M2C_GameEndInfo)]
	[ProtoContract]
	public partial class M2C_GameEndInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<string> PlayerNames = new List<string>();

		[ProtoMember(2)]
		public List<int> Points = new List<int>();

		[ProtoMember(3)]
		public List<int> Places = new List<int>();

	}

	[Message(OuterOpcode.Event_OutTurnOperationInfo)]
	[ProtoContract]
	public partial class Event_OutTurnOperationInfo: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int PlayerIndex { get; set; }

		[ProtoMember(2)]
		public GamePlay.Server.Model.OutTurnOperation Operation { get; set; }

		[ProtoMember(3)]
		public int BonusTurnTime { get; set; }

	}

	[Message(OuterOpcode.Event_InTurnOperationInfo)]
	[ProtoContract]
	public partial class Event_InTurnOperationInfo: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int PlayerIndex { get; set; }

		[ProtoMember(2)]
		public GamePlay.Server.Model.InTurnOperation Operation { get; set; }

		[ProtoMember(3)]
		public int BonusTurnTime { get; set; }

	}

	[Message(OuterOpcode.M2C_KongInfo)]
	[ProtoContract]
	public partial class M2C_KongInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int KongPlayerIndex { get; set; }

		[ProtoMember(2)]
		public Mahjong.Model.PlayerHandData HandData { get; set; }

		[ProtoMember(3)]
		public List<GamePlay.Server.Model.OutTurnOperation> Operations = new List<GamePlay.Server.Model.OutTurnOperation>();

		[ProtoMember(4)]
		public int BonusTurnTime { get; set; }

		[ProtoMember(9)]
		public Mahjong.Model.MahjongSetData MahjongSetData { get; set; }

	}

	[Message(OuterOpcode.M2C_BeiDoraInfo)]
	[ProtoContract]
	public partial class M2C_BeiDoraInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int BeiDoraPlayerIndex { get; set; }

		[ProtoMember(2)]
		public List<int> BeiDoras = new List<int>();

		[ProtoMember(3)]
		public Mahjong.Model.PlayerHandData HandData { get; set; }

		[ProtoMember(4)]
		public List<GamePlay.Server.Model.OutTurnOperation> Operations = new List<GamePlay.Server.Model.OutTurnOperation>();

		[ProtoMember(5)]
		public int BonusTurnTime { get; set; }

		[ProtoMember(9)]
		public Mahjong.Model.MahjongSetData MahjongSetData { get; set; }

	}

	[Message(OuterOpcode.M2C_RongInfo)]
	[ProtoContract]
	public partial class M2C_RongInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<int> RongPlayerIndices = new List<int>();

		[ProtoMember(2)]
		public List<string> RongPlayerNames = new List<string>();

		[ProtoMember(3)]
		public List<Mahjong.Model.PlayerHandData> HandData = new List<Mahjong.Model.PlayerHandData>();

		[ProtoMember(4)]
		public Mahjong.Model.Tile WinningTile { get; set; }

		[ProtoMember(5)]
		public List<Mahjong.Model.Tile> DoraIndicators = new List<Mahjong.Model.Tile>();

		[ProtoMember(6)]
		public List<Mahjong.Model.Tile> UraDoraIndicators = new List<Mahjong.Model.Tile>();

		[ProtoMember(7)]
		public List<bool> RongPlayerRichiStatus = new List<bool>();

		[ProtoMember(8)]
		public List<GamePlay.Server.Model.NetworkPointInfo> RongPointInfos = new List<GamePlay.Server.Model.NetworkPointInfo>();

		[ProtoMember(9)]
		public List<int> TotalPoints = new List<int>();

	}

	[Message(OuterOpcode.M2C_TsumoInfo)]
	[ProtoContract]
	public partial class M2C_TsumoInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int TsumoPlayerIndex { get; set; }

		[ProtoMember(2)]
		public string TsumoPlayerName { get; set; }

		[ProtoMember(3)]
		public Mahjong.Model.PlayerHandData TsumoHandData { get; set; }

		[ProtoMember(4)]
		public Mahjong.Model.Tile WinningTile { get; set; }

		[ProtoMember(5)]
		public List<Mahjong.Model.Tile> DoraIndicators = new List<Mahjong.Model.Tile>();

		[ProtoMember(6)]
		public List<Mahjong.Model.Tile> UraDoraIndicators = new List<Mahjong.Model.Tile>();

		[ProtoMember(7)]
		public bool IsRichi { get; set; }

		[ProtoMember(8)]
		public GamePlay.Server.Model.NetworkPointInfo TsumoPointInfo { get; set; }

		[ProtoMember(9)]
		public int TotalPoints { get; set; }

	}

	[Message(OuterOpcode.M2C_PointTransferInfo)]
	[ProtoContract]
	public partial class M2C_PointTransferInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public List<string> PlayerNames = new List<string>();

		[ProtoMember(8)]
		public List<GamePlay.Server.Model.PointTransfer> PointTransfers = new List<GamePlay.Server.Model.PointTransfer>();

		[ProtoMember(9)]
		public List<int> Points = new List<int>();

	}

	[Message(OuterOpcode.M2C_RoundDrawInfo)]
	[ProtoContract]
	public partial class M2C_RoundDrawInfo: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public GamePlay.Server.Model.RoundDrawType RoundDrawType { get; set; }

		[ProtoMember(8)]
		public List<Mahjong.Model.WaitingData> WaitingData = new List<Mahjong.Model.WaitingData>();

	}

	[Message(OuterOpcode.Event_GamePrepare)]
	[ProtoContract]
	public partial class Event_GamePrepare: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.Event_OutTurnOperation)]
	[ProtoContract]
	public partial class Event_OutTurnOperation: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public GamePlay.Server.Model.OutTurnOperation Operation { get; set; }

	}

	[Message(OuterOpcode.M2C_InitHandAfterRong)]
	[ProtoContract]
	public partial class M2C_InitHandAfterRong: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int PlayerIndex { get; set; }

		[ProtoMember(5)]
		public List<bool> RichiStatus = new List<bool>();

		[ProtoMember(8)]
		public List<Mahjong.Model.Tile> InitialHandTiles = new List<Mahjong.Model.Tile>();

		[ProtoMember(9)]
		public Mahjong.Model.MahjongSetData MahjongSetData { get; set; }

	}

	[Message(OuterOpcode.M2C_BattleRoundDraw)]
	[ProtoContract]
	public partial class M2C_BattleRoundDraw: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(9)]
		public Mahjong.Model.MahjongSetData MahjongSetData { get; set; }

	}

	[Message(OuterOpcode.M2C_SelectTiles)]
	[ProtoContract]
	public partial class M2C_SelectTiles: IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<Mahjong.Model.Tile> AllTiles = new List<Mahjong.Model.Tile>();

	}

	[Message(OuterOpcode.ClientSelectTilesReady)]
	[ProtoContract]
	public partial class ClientSelectTilesReady: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<Mahjong.Model.Tile> InitialTiles = new List<Mahjong.Model.Tile>();

	}

	[Message(OuterOpcode.ClientChangeTileConf)]
	[ProtoContract]
	public partial class ClientChangeTileConf: IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public bool IsChangeTileAfterRong { get; set; }

	}

}
