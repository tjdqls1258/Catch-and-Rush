using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public static PhotonInit instance;

    public InputField PlayerInput;

    bool isGameStart = false;
    bool isLoggIn = false;
    string playerName;
    string connectionState = "";
    public string chatMessage;
    // Start is called before the first frame update

    Text chatText;
    Text connectionInfoText;

    ScrollRect scroll_rect = null;
    PhotonView pv;

    //UI
    [Header("LobbyCanveas")] public GameObject LobbyCanvas;
    public GameObject LobbyPanel;
    public GameObject MakeRoomPanel;
    public GameObject RoomPanel;
    public GameObject PwPanel;
    public GameObject PwErrorLog;
    public GameObject PwConfirmBtn;
    public GameObject PwPanelCloseBtn;
    public InputField RoomInput;
    public InputField RoomPwInput;
    public InputField PwCheckIF;
    public Toggle PwToggle;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;
    public Button CreateRoomBtn;
    public int hanhtablecount;
    public bool LockState = false;
    public string privateroom;

    //방 리스트 관리
    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, mutiple, roomnuber;

    void Awake()
    {
        PhotonNetwork.GameVersion = "MyFps 1.0";
        PhotonNetwork.ConnectUsingSettings();

        if(GameObject.Find("ChatText")!= null)
        {
            chatText = GameObject.Find("ChatText").GetComponent<Text>();
        }
        if (GameObject.Find("Scroll View") != null)
        {
            scroll_rect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        }
        if (GameObject.Find("ConnectionInfoText") != null)
        {
            connectionInfoText = GameObject.Find("ConnectionInfoText").GetComponent<Text>();
        }

        connectionState = "마스터 서버에 접속 중...";

        if(connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }       
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("LogIn", 0);
    }

    private void Update()
    {
        if ((PlayerPrefs.GetInt("LogIn") == 1))
        {
            isLoggIn = true;
        }
        if ((isGameStart == false) &&(SceneManager.GetActiveScene().name == "SampleScene") && (isLoggIn == true))
        {
            isGameStart = true;
            if(GameObject.Find("ChatText") != null)
            {
                chatText = GameObject.Find("ChatText").GetComponent<Text>();
            }
            if(GameObject.Find("Scroll View") != null)
            {
                scroll_rect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
            }
            StartCoroutine(CreatPlayer());
        }
    }

    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //    Debug.Log("로비 입장");
    //    PhotonNetwork.JoinRandomRoom();
    //}

    public static PhotonInit Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType(typeof(PhotonInit)) as PhotonInit;

                if(instance == null)
                {
                    Debug.Log("no singleton obj");
                }
            }
            return instance;
        }
    }

    public void Connect()
    {
        if(PhotonNetwork.IsConnected)
        {
            connectionState = "룸에 접속 중 ...";
            if(connectionInfoText)
            {
                connectionInfoText.text = connectionState;
            }
            LobbyPanel.SetActive(false);
            RoomPanel.SetActive(true);

            PhotonNetwork.JoinLobby();
        }
        else
        {
            connectionState = "오프라인 : 마스터 서버와 연결되지 않음 \n접속 재시도중 ...";
            if (connectionInfoText)
            {
                connectionInfoText.text = connectionState;
            }
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionState = "No Room";
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
        Debug.Log("No Room");
        //PhotonNetwork.CreateRoom("MyRoom");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        connectionState = "Finish make a room";
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
        
        Debug.Log("방 생성 완료");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        connectionState = "Joined Room";
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
        Debug.Log("방 입장");
        isLoggIn = true;
        PlayerPrefs.SetInt("LogIn", 1);

        //SceneManager.LoadScene("SampleScene");
        PhotonNetwork.LoadLevel("SampleScene");
        
    }
    IEnumerator CreatPlayer()
    {
        while(!isGameStart)
        {
            yield return new WaitForSeconds(0.5f);
        }
        GameObject tempPlayer = PhotonNetwork.Instantiate("PlayerDagger", 
            new Vector3(0, 0, 0), 
            Quaternion.identity, 
            0);
        Debug.Log("이름 : " + playerName);
        //tempPlayer.GetComponent<PlayerCtrl>().SetPlayerName(PlayerPrefs.GetString("PlayerName"));
        pv = GetComponent<PhotonView>();
        yield return null;
    }
    private void OnGUI()
    {
        GUILayout.Label(connectionState);
    }

    public void SetPlayerName()
    {
        Debug.Log(PlayerInput.text + " 를(을) 입력 하셨습니다!");
        if (isGameStart == false && isLoggIn == false)
        {
            playerName = PlayerInput.text;
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerInput.text = string.Empty;
            Debug.Log("연결 이름 : " + playerName);
            Debug.Log("Connect 시도" + isGameStart + ", " + isLoggIn);
            Connect();
        }
        else
        {
            chatMessage = PlayerInput.text;
            PlayerInput.text = string.Empty;
            pv.RPC("ChatInfo", RpcTarget.All, chatMessage);
        } 
    }
 
    public void ShowChat(string chat)
    {
        chatText.text +=  chat + "\n";

        scroll_rect.verticalNormalizedPosition = 0.0f;
    }

    [PunRPC]
    public void ChatInfo(string sChat, PhotonMessageInfo info)
    {
        ShowChat(sChat);
    }

    public void CreateRoomBtnOnClick()
    {
        MakeRoomPanel.SetActive(true);
    }

    public void OKBtnOnClick()
    {
        MakeRoomPanel.SetActive(false);
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text,
            new RoomOptions { MaxPlayers = 100 });
        LobbyPanel.SetActive(false);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        connectionState = "마스터 서버에 접속중...";
        if(connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
        isGameStart = false;
        isLoggIn = false;
        PlayerPrefs.SetInt("LogIn", 0);

    }

    //방만들기
    public void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 80;
        roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"password", RoomPwInput.text}
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };
        
        if(PwToggle.isOn)
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : "*" + RoomInput.text,
                roomOptions);
        }
        else
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text,
                new RoomOptions { MaxPlayers = 80 });
        }

        MakeRoomPanel.SetActive(false);
    }

    public void MyListClick(int num)
    {
        if(num == -2)
        {
            --currentPage;
            MyListRenewal();
        }
        else if(num == -1)
        {
            ++currentPage;
            MyListRenewal();
        }

        else if(myList[mutiple + num].CustomProperties["password"] != null)
        {
            PwPanel.SetActive(true);
        }
        else 
        {
            PhotonNetwork.JoinRoom(myList[mutiple + num].Name);
            MyListRenewal();
        }
    }

    public void RoomPw(int number)
    {
        switch(number)
        {
            case 0:
                roomnuber = 0;
                break;
            case 1:
                roomnuber = 1;
                break;
            case 2:
                roomnuber = 2;
                break;
            case 3:
                roomnuber = 3;
                break;
            default:
                break;
        }
    }

    public void EnterRoomWothPW()
    {
        if((string)myList[mutiple + roomnuber].CustomProperties["password"] == PwCheckIF.text)
        {
            PhotonNetwork.JoinRoom(myList[mutiple + roomnuber].Name);
            MyListRenewal();
            PwPanel.SetActive(false);
        }
        else
        {
            StartCoroutine("ShowPwWrongMsg");
        }
    }

    IEnumerator ShowPwWrongMsg()
    {
        if(!PwErrorLog.activeSelf)
        {
            PwErrorLog.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            PwErrorLog.SetActive(false);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i]))
                {
                    myList.Add(roomList[i]);
                }
                else
                {
                    myList[myList.IndexOf(roomList[i])] = roomList[i];
                }
            }
            else if (myList.IndexOf(roomList[i]) != -1)
            {
                myList.RemoveAt(myList.IndexOf(roomList[i]));
            }
        }
        MyListRenewal();
    }

    void MyListRenewal()
    {
        maxPage = (myList.Count % CellBtn.Length == 0) ? 
            myList.Count / CellBtn.Length : 
            myList.Count / CellBtn.Length + 1;

        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        mutiple = (currentPage - 1) * CellBtn.Length;
        for(int i = 0; i< CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (mutiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text =
                (mutiple + i < myList.Count) ? myList[mutiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (mutiple + i < myList.Count)
                ? myList[mutiple + i].PlayerCount + "/" + myList[mutiple + i].MaxPlayers : "";
        }
    }

}
