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
        [Header("여기는 방생성 버튼관련 설정 모음")]
        //  방생성
        public Button btnCreate;
        //  방이름
        public InputField roomName;
        //비밀번호 InputField
        public InputField inputPassword;
        //  총인원
        public InputField totalNum;
        // 생성 
        public Button create;
        int flag = 1;
        [Header("여기는 방입장 버튼관련 설정 모음")]
        //
        //  방입장
        public Button SelectSection;
        public GameObject SelectRooms;
        public Button btnJoin;
        public Button btnJoinRandomly;
        public Button btnBack;
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
            // 시작부터 방생성 옵션들을 모두 끈다. 
            TurnCreateBtn(false);
            SelectRooms.SetActive(false);
            btnBack.transform.gameObject.SetActive(false);

        }
        //방생성
        public void CreateRoom()
        {
            // 갈축
            //방 정보 셋팅
            RoomOptions roomOptions = new RoomOptions();
            //최대인원
            //where '0' means "no limit"
            roomOptions.MaxPlayers = 4;//byte.Parse(totalNum.text);
            //룸 목록에 모이나? 보이지 않느냐?
            roomOptions.IsVisible = true;
            // 커스텀 정보를 셋팅
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable(); ;
            //hash["map_id"] = Random.Range(0, mapThumbs.Length);
            hash["room_name"] = roomName.text;
            hash["password"] = inputPassword.text;
            roomOptions.CustomRoomProperties = hash;
            //print((string)hash["desc"]+ ", " + (float)hash[1]);
            //custom 정보를 공개하는 설정
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "room_name", "password" };
            // 방을 만든다.
            PhotonNetwork.CreateRoom(roomName.text + inputPassword.text, roomOptions, TypedLobby.Default);
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
            PhotonNetwork.JoinRoom(roomName.text);
        }
        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print(System.Reflection.MethodBase.GetCurrentMethod().Name);
            Destroyplayer();  
            PhotonNetwork.LoadLevel("WaitingRoom");

        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
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
            CreateRoomListUI();
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
        void CreateRoomListUI()
        {
            foreach (RoomInfo info in roomCache.Values)
            {
                //instantiate 두번째 파라메터 의 자식으로 샐성된다.
                GameObject go = Instantiate(roomItemFac, trListContent);
                // 룸아이템 정보를 셋팅(방제목)
                RoomItem item = go.GetComponent<RoomItem>();
                item.SetInfo(info);
                //roomItem 버튼이 클릭되면 호출되는 함수 등록
                item.OnclickAction = SetRoomName;
                //or 람다식
                //item.OnclickAction = (room) => { roomName.text = room; };

                string desc = (string)info.CustomProperties["desc"];
                //int mapId = (int)info.CustomProperties["map_id"];
                //print(desc + " , " + mapId);
            }
        }
        //이전 썸네일 Id
        //초기에는 이전의 맵 id가 없으므로 -1
        //int prevMap_id = -1;
        void SetRoomName(string room /*,int map_id*/)
        {
            //룸네일 설정
            roomName.text = room;
            // 이전 맵 썸네일을 모두 비활성화
            // 만약에 이전 맵 썸네일이 활성화되어 있다면
            //맵 썸네일 설정
            //if (prevMap_id > -1)
            //{
            //    mapThumbs[prevMap_id].SetActive(false);
            //}
            //mapThumbs[map_id].SetActive(true);
            ////이전의 맵 id 저장
            //prevMap_id = map_id;
        }
        //방생성 설정창 나오도록 하는 함수
        public void OnClickedCreateRoom()
        {
            flag *= -1;
            if (flag == -1)
            {
                TurnCreateBtn(true);
            }
            else { TurnCreateBtn(false); }
            //방 생성 버튼을 누르면
            //방이름, 비밀번호, 인원수 설정 UI, 그리고 생성 버튼이 나오도록한다. 
            //다시 방생성 버튼을 누르면 해당 버튼이 없어지도록 만든다.
        }
        void TurnCreateBtn(bool B)
        {
            roomName.transform.gameObject.SetActive(B);
            totalNum.transform.gameObject.SetActive(B);
            create.transform.gameObject.SetActive(B);
            
        }
        public void OnClickedSelectSection()
        {
            // 섹션 검색하기 버튼을 누르면 게임 생성 버튼을 모두 끄고
            TurnCreateBtn(false);
            SelectRooms.SetActive(true);
            btnJoinRandomly.transform.gameObject.SetActive(false);
            btnCreate.transform.gameObject.SetActive(false);
            btnBack.transform.gameObject.SetActive(true);
            // 스크롤 창과 뒤로가기만 보이도록한다.
        }
        public void OnClickedBackBtn()
        {
            // 뒤로가기 버튼을 눌렀을 때 원복한다.
            SelectRooms.SetActive(false);
            btnJoinRandomly.transform.gameObject.SetActive(true);
            btnCreate.transform.gameObject.SetActive(true);
            btnBack.transform.gameObject.SetActive(false);
        }
        public void Destroyplayer()
        {
            photonView.RPC("RpcDestroyplayer", RpcTarget.All);
        }
        [PunRPC]
        void RpcDestroyplayer()
        {
            Destroy(GameObject.FindGameObjectWithTag("check"));
        }

    }
}
