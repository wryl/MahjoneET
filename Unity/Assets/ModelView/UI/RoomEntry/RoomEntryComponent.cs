using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class RoomEntryComponent : Entity
    {
        public long _roomid;
        public GameObject MQTJ;
        public Button MCheckRuleButton;
        public Button MJoinButton;
        public Text MPlayerStatusText;
        public Text MRoomName;
        public long Roomid { get => _roomid; set => _roomid = value; }
        public void SetRoom(MJ_RoomInfoInList mJ_RoomInfo)
        {
            Debug.Log(mJ_RoomInfo.ToString());
            Roomid = mJ_RoomInfo.RoomId;
            MRoomName.text = mJ_RoomInfo.RoomName;
            MQTJ.gameObject.SetActive(mJ_RoomInfo.GameMode == Mahjong.Model.GameMode.QTJ);
            MPlayerStatusText.text = $"{mJ_RoomInfo.NowPlayerNum}/{GetPlayerMax(mJ_RoomInfo.GamePlayers)}";
            MJoinButton.onClick.AddListener(JoinRoom);
            MCheckRuleButton.onClick.AddListener(CheckRule);
        }
        int GetPlayerMax(Mahjong.Model.GamePlayers layers)
        {
            switch (layers)
            {
                case Mahjong.Model.GamePlayers.Two:
                    return 2;
                    break;
                case Mahjong.Model.GamePlayers.Three:
                    return 3;
                    break;
                case Mahjong.Model.GamePlayers.Four:
                    return 4;
                    break;
                default:
                    return 4;
                    break;
            }
        }
        private void CheckRule()
        {

        }

        private void JoinRoom()
        {
            Log.Debug(Roomid.ToString());
        }
        public void Awake(MJ_RoomInfoInList mJ_RoomInfo)
        {
            ReferenceCollector rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            MQTJ = rc.Get<GameObject>("QTJ");
            MCheckRuleButton = rc.Get<GameObject>("CheckRuleButton").GetComponent<Button>();
            MJoinButton = rc.Get<GameObject>("JoinButton").GetComponent<Button>();
            MPlayerStatusText = rc.Get<GameObject>("PlayerStatusText").GetComponent<Text>();
            MRoomName = rc.Get<GameObject>("RoomName").GetComponent<Text>();
            SetRoom(mJ_RoomInfo);
        }
    }
}

