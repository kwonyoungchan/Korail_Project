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
        [Header("����� ����� ��ư���� ���� ����")]
        //  �����
        public Button btnCreate;
        //  ���̸�
        public InputField roomName;
        //��й�ȣ InputField
        public InputField inputPassword;
        //  ���ο�
        public InputField totalNum;
        // ���� 
        public Button create;
        int flag = 1;
        [Header("����� ������ ��ư���� ���� ����")]
        //
        //  ������
        public Button SelectSection;
        public GameObject SelectRooms;
        public Button btnJoin;
        public Button btnJoinRandomly;
        public Button btnBack;
        //���� Button

        //���� ������
        Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();
        //�� ����Ʈ content
        public Transform trListContent;

        public GameObject[] mapThumbs;
        public void OnRoomNameChanged(string s)
        {
            //����
            btnJoin.interactable = s.Length > 0;
            //����
            btnCreate.interactable = s.Length > 0 && totalNum.text.Length > 0;
        }
        public void OnTotalPlayerChanged(string s)
        {
            //����
            btnCreate.interactable = s.Length > 0 && totalNum.text.Length > 0;
        }
        void Start()
        {

            //���̸�(InputField) �� ����� �� ȣ��Ǵ� �Լ� ���
            roomName.onValueChanged.AddListener(OnRoomNameChanged);
            totalNum.onValueChanged.AddListener(OnTotalPlayerChanged);
            // ���ۺ��� ����� �ɼǵ��� ��� ����. 
            TurnCreateBtn(false);
            SelectRooms.SetActive(false);
            btnBack.transform.gameObject.SetActive(false);

        }
        //�����
        public void CreateRoom()
        {
            // ����
            //�� ���� ����
            RoomOptions roomOptions = new RoomOptions();
            //�ִ��ο�
            //where '0' means "no limit"
            roomOptions.MaxPlayers = 4;//byte.Parse(totalNum.text);
            //�� ��Ͽ� ���̳�? ������ �ʴ���?
            roomOptions.IsVisible = true;
            // Ŀ���� ������ ����
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable(); ;
            //hash["map_id"] = Random.Range(0, mapThumbs.Length);
            hash["room_name"] = roomName.text;
            hash["password"] = inputPassword.text;
            roomOptions.CustomRoomProperties = hash;
            //print((string)hash["desc"]+ ", " + (float)hash[1]);
            //custom ������ �����ϴ� ����
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "room_name", "password" };
            // ���� �����.
            PhotonNetwork.CreateRoom(roomName.text + inputPassword.text, roomOptions, TypedLobby.Default);
        }
        //������
        //�� ���� �Ϸ�
        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        // ���� �� ������ ������ ��� 
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
            //�븮��Ʈ UI�� ��ü����
            DeleteRoomListUI();
            //�븮��Ʈ ������ ������Ʈ
            UpdateRoomCache(roomList);
            //�븮��ƮUI ��ü ����
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
                //����, ����
                if (roomCache.ContainsKey(roomList[i].Name))
                {
                    //���� �ش� ���� �����Ȱ��̶��
                    if (roomList[i].RemovedFromList)
                    {
                        //roomCache���� �ش� ������ ����
                        roomCache.Remove(roomList[i].Name);
                    }
                    //�׷��� �ʴٸ�
                    else
                    {
                        //���� ����
                        roomCache[roomList[i].Name] = roomList[i];
                    }
                }
                //�߰� 
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
                //instantiate �ι�° �Ķ���� �� �ڽ����� �����ȴ�.
                GameObject go = Instantiate(roomItemFac, trListContent);
                // ������� ������ ����(������)
                RoomItem item = go.GetComponent<RoomItem>();
                item.SetInfo(info);
                //roomItem ��ư�� Ŭ���Ǹ� ȣ��Ǵ� �Լ� ���
                item.OnclickAction = SetRoomName;
                //or ���ٽ�
                //item.OnclickAction = (room) => { roomName.text = room; };

                string desc = (string)info.CustomProperties["desc"];
                //int mapId = (int)info.CustomProperties["map_id"];
                //print(desc + " , " + mapId);
            }
        }
        //���� ����� Id
        //�ʱ⿡�� ������ �� id�� �����Ƿ� -1
        //int prevMap_id = -1;
        void SetRoomName(string room /*,int map_id*/)
        {
            //����� ����
            roomName.text = room;
            // ���� �� ������� ��� ��Ȱ��ȭ
            // ���࿡ ���� �� ������� Ȱ��ȭ�Ǿ� �ִٸ�
            //�� ����� ����
            //if (prevMap_id > -1)
            //{
            //    mapThumbs[prevMap_id].SetActive(false);
            //}
            //mapThumbs[map_id].SetActive(true);
            ////������ �� id ����
            //prevMap_id = map_id;
        }
        //����� ����â �������� �ϴ� �Լ�
        public void OnClickedCreateRoom()
        {
            flag *= -1;
            if (flag == -1)
            {
                TurnCreateBtn(true);
            }
            else { TurnCreateBtn(false); }
            //�� ���� ��ư�� ������
            //���̸�, ��й�ȣ, �ο��� ���� UI, �׸��� ���� ��ư�� ���������Ѵ�. 
            //�ٽ� ����� ��ư�� ������ �ش� ��ư�� ���������� �����.
        }
        void TurnCreateBtn(bool B)
        {
            roomName.transform.gameObject.SetActive(B);
            totalNum.transform.gameObject.SetActive(B);
            create.transform.gameObject.SetActive(B);
            
        }
        public void OnClickedSelectSection()
        {
            // ���� �˻��ϱ� ��ư�� ������ ���� ���� ��ư�� ��� ����
            TurnCreateBtn(false);
            SelectRooms.SetActive(true);
            btnJoinRandomly.transform.gameObject.SetActive(false);
            btnCreate.transform.gameObject.SetActive(false);
            btnBack.transform.gameObject.SetActive(true);
            // ��ũ�� â�� �ڷΰ��⸸ ���̵����Ѵ�.
        }
        public void OnClickedBackBtn()
        {
            // �ڷΰ��� ��ư�� ������ �� �����Ѵ�.
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
