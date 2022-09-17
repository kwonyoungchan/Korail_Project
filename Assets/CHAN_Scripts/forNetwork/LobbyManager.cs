using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        //  방이름
        public InputField roomName;
        //비밀번호 InputField
        public InputField InputPassword;
        //  총인원
        public InputField totalNum;
        //  방참가
        public Button btnCreate;
        //  방입장
        public Button btnJoin;
        //접속 Button

        //방의 정보들
        Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
        //룸 리스트 content
        public Transform trListContent;

        public GameObject[] mapThumbs;
        public void OnRoomNameChanged(string s)
        {
            //참가
            btnJoin.interactable = s.Length > 0;
            //생성
            btnCreate.interactable = s.Length > 0 && totalNum.text.Length > 0;
        }
        public void OnTotalPlayerChanged(string s)
        {
            //생성
            btnCreate.interactable = s.Length > 0 && totalNum.text.Length > 0;
        }
        void Start()
        {

            //방이름(InputField) 이 변경될 때 호출되는 함수 등록
            roomName.onValueChanged.AddListener(OnRoomNameChanged);
            totalNum.onValueChanged.AddListener(OnTotalPlayerChanged);
            //CreateRoom();

        }
        //방생성
        public void CreateRoom()
        {
            // 갈축
            //방 정보 셋팅
            RoomOptions roomOptions = new RoomOptions();
            //최대인원
            //where '0' means "no limit"
            roomOptions.MaxPlayers = byte.Parse(totalNum.text);
            //룸 목록에 모이나? 보이지 않느냐?
            roomOptions.IsVisible = true;
            // 커스텀 정보를 셋팅
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable(); ;
            hash["desc"] = "여긴 초보방" + Random.Range(1, 1000);
            hash["map_id"] = Random.Range(0, mapThumbs.Length);
            hash["room_name"] = roomName.text;
            hash["password"] = InputPassword.text;
            roomOptions.CustomRoomProperties = hash;
            //print((string)hash["desc"]+ ", " + (float)hash[1]);
            //custom 정보를 공개하는 설정
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "desc", "map_id", "room_name", "password" };
            // 방을 만든다.
            PhotonNetwork.CreateRoom(roomName.text + InputPassword.text, roomOptions, TypedLobby.Default);
        }
        //방입장
        //방 생성 완료
        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        // 만약 방 생성이 실패할 경우 
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            print(System.Reflection.MethodBase.GetCurrentMethod().Name + returnCode + message);
        }
        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(roomName.text + InputPassword.text);
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print(System.Reflection.MethodBase.GetCurrentMethod().Name);
            PhotonNetwork.LoadLevel("ProtoType");

        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            print(System.Reflection.MethodBase.GetCurrentMethod().Name + ", " + returnCode + ", " + message);
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            //룸리스트 UI를 전체삭제
            DeleteRoomListUI();
            //룸리스트 정보를 업데이트
            UpdateRoomCache(roomList);
            //룸리스트UI 전체 생성
            //CreateRoomListUI();
        }
        void DeleteRoomListUI()
        {
            foreach (Transform tr in trListContent)
            {
                Destroy(tr.gameObject);
            }
        }
        void UpdateRoomCache(List<RoomInfo> roomList)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                //수정, 삭제
                if (roomCache.ContainsKey(roomList[i].Name))
                {
                    //만약 해당 룸이 삭제된것이라면
                    if (roomList[i].RemovedFromList)
                    {
                        //roomCache에서 해당 정보를 삭제
                        roomCache.Remove(roomList[i].Name);
                    }
                    //그렇지 않다면
                    else
                    {
                        //정보 수정
                        roomCache[roomList[i].Name] = roomList[i];
                    }
                }
                //추가 
                else
                {
                    roomCache[roomList[i].Name] = roomList[i];
                }
            }
        }

        public GameObject roomItemFac;
        //void CreateRoomListUI()
        //{
        //    foreach (RoomInfo info in roomCache.Values)
        //    {
        //        //instantiate 두번째 파라메터 의 자식으로 샐성된다.
        //        GameObject go = Instantiate(roomItemFac, trListContent);
        //        // 룸아이템 정보를 셋팅(방제목)
        //        RoomItem item = go.GetComponent<RoomItem>();
        //        item.SetInfo(info);
        //        //roomItem 버튼이 클릭되면 호출되는 함수 등록
        //        item.OnclickAction = SetRoomName;
        //        //or 람다식
        //        //item.OnclickAction = (room) => { roomName.text = room; };

        //        string desc = (string)info.CustomProperties["desc"];
        //        int mapId = (int)info.CustomProperties["map_id"];
        //        print(desc + " , " + mapId);
        //    }
        //}
        //이전 썸네일 Id
        //초기에는 이전의 맵 id가 없으므로 -1
        int prevMap_id = -1;
        void SetRoomName(string room, int map_id)
        {
            //룸네일 설정
            roomName.text = room;
            // 이전 맵 썸네일을 모두 비활성화
            // 만약에 이전 맵 썸네일이 활성화되어 있다면
            //맵 썸네일 설정
            if (prevMap_id > -1)
            {
                mapThumbs[prevMap_id].SetActive(false);
            }
            mapThumbs[map_id].SetActive(true);
            //이전의 맵 id 저장
            prevMap_id = map_id;
        }
    }
}
