using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyInit : MonoBehaviourPunCallbacks
{
    //�̱��� ������ ����
    public static PhotonInit instance;

    public InputField playerInput;
    public Button chattingBtn;
    bool isGameStart = false;
    bool isLoggin = false;
    bool isReady = false;
    string playerName = "";
    string connectionState = "";

    public string chatMessage;
    Text chatText;
    ScrollRect scroll_rect = null;

    PhotonView pv;

    Text connectionInfoText;

    [Header("LobbyCanvas")] public GameObject LobbyCanvas; //canvas
    public GameObject LobbyPanel; //login
    public GameObject RoomPanel; // lobby
    public GameObject MakeRoomPanel; //�� ����
    public InputField RoomInput; //�� ���� �κ� �̸� ��ǲ�ʵ�
    public InputField RoomPwInput; //�� ���� �κ� ��й�ȣ ��ǲ�ʵ�
    public Toggle PwToggle; //�� ���� �κ� ��й�ȣ ��� ��ǲ�ʵ�
    public GameObject PwPanel; //�� ����(��� �� ������ ����)(��� �Է�â)
    public GameObject PwErrorLog; //�� ����(��� �Է� ����)(�Ⱦ���)
    public GameObject PwConfirmBtn; //�� ����(��� �Է�â ��ư)
    public GameObject PwPanelCloseBtn;//�� ����(��� �Է�â â�ݱ� ��ư)
    public InputField PwCheckIF; //�� ����
    //ĵ���� �ȿ� ĵ������ �־�� ������ ���� �� ��, ����� �ȵǸ� �ϳ��� ĵ������ ���� �гα����� ��ü�ؾ���

    public bool LockState = false;
    public string privateroom;
    public Button[] CellBtn;
    public Button PreviousBtn;//lobby ���� ȭ��ǥ �߰�
    public Button NextBtn; //lobby ���� ȭ��ǥ �߰�
    public Button CreateRoomBtn;
    public int hashtablecount;

    //�������� �� ����Ʈ�� �����ϴ� ����
    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple, roomnumber;

    public static PhotonInit Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(PhotonInit)) as PhotonInit;

                if (instance == null)
                {
                    Debug.Log("no singleton obj");
                }
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //�����ʿ�
        PhotonNetwork.GameVersion = "MyFps 1.0";
        PhotonNetwork.ConnectUsingSettings();

        if (GameObject.Find("ChatText") != null)
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

        connectionState = "������ ������ ���� ��...";

        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Login", 0);
    }

    private void OnConnectedToServer()
    {
        Debug.Log("OnConnectedToServer");
        isReady = true;
    }

    public void Connect()
    {
        Debug.Log(PhotonNetwork.IsConnected);
        if (PhotonNetwork.IsConnected)
        {
            connectionState = "�뿡 ����....";
            if (connectionInfoText)
            {
                connectionInfoText.text = connectionState;
            }

            LobbyPanel.SetActive(false);
            RoomPanel.SetActive(true);

            PhotonNetwork.JoinLobby();
        }
        else
        {
            connectionState = "�������� : ������ ������ ������� ����\n���� ��õ� ��....";
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
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        connectionState = "Finish make a room";
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
        Debug.Log("Finish make a room");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        connectionState = "Joined Room";
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
        Debug.Log("Joined room");
        isLoggin = false;
        PlayerPrefs.SetInt("Login", 1);

        PhotonNetwork.LoadLevel("MainScene");
    }

    void OnGUI()
    {
        GUILayout.Label(connectionState);
    }

    IEnumerator CreatePlayer()
    {
        while (!isGameStart)
        {
            yield return new WaitForSeconds(0.5f);
        }

        GameObject tempPlayer = PhotonNetwork.Instantiate("Player", new Vector3(2, 1, 11), Quaternion.identity, 0);
        tempPlayer.GetComponent<PlayerCtrl>().SetPlayerName(playerName);
        pv = GetComponent<PhotonView>();

        yield return null;
    }

    public void SetPlayerName()
    {
        Debug.Log(playerInput.text + "�� �Է� �ϼ̽��ϴ�!");

        if (isGameStart == false && isLoggin == false)
        {
            playerName = playerInput.text;
            playerInput.text = string.Empty;
            Debug.Log("connect �õ� " + isGameStart + ", " + isLoggin);
            Connect();
        }
        else if (isGameStart == true && isLoggin == true)
        {
            chatMessage = playerInput.text;
            playerInput.text = string.Empty;
            pv.RPC("ChatInfo", RpcTarget.All, chatMessage);
        }
    }

    public void ShowChat(string chat)
    {
        chatText.text += chat + "\n";
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }

    [PunRPC]
    public void ChatInfo(string sChat, PhotonMessageInfo info)
    {
        ShowChat(sChat);
    }

    #region �� ���� �� ���� ���� �ż���
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
        PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 80 });
        LobbyPanel.SetActive(false);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        connectionState = "������ ������ ���� ��...";
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }

        isGameStart = false;
        isLoggin = false;

        PlayerPrefs.SetInt("Login", 0);
    }

    //���ο� �� �����
    public void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 80;
        roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"password", RoomPwInput.text}
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        if (PwToggle.isOn)
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : "*" + RoomInput.text, roomOptions);
        }
        else
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 80 });
        }

        MakeRoomPanel.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate : " + roomList.Count);
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

            MyListRenewal();
        }
    }

    void MyListRenewal()
    {
        //�ִ� ������
        maxPage = (myList.Count % CellBtn.Length == 0)
            ? myList.Count / CellBtn.Length
            : myList.Count / CellBtn.Length + 1;

        //����, ������ư
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        //�������� �´� ����Ʈ ����
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + 1 < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count)
                ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public void MyListClick(int num)
    {
        if (num == -2)
        {
            --currentPage;
            MyListRenewal();
        }
        else if (num == -1)
        {
            ++currentPage;
            MyListRenewal();
        }
        else if (myList[multiple + num].CustomProperties["password"] != null)
        {
            PwPanel.SetActive(true);
        }
        else
        {
            PhotonNetwork.JoinRoom(myList[multiple + num].Name);
            MyListRenewal();
        }
    }

    public void RoomPw(int number)
    {
        switch (number)
        {
            case 0:
                roomnumber = 0;
                break;
            case 1:
                roomnumber = 1;
                break;
            case 2:
                roomnumber = 2;
                break;
            case 3:
                roomnumber = 3;
                break;
            case 4:
                roomnumber = 4;
                break;
            case 5:
                roomnumber = 5;
                break;
            case 6:
                roomnumber = 6;
                break;
            case 7:
                roomnumber = 7;
                break;
            default:
                break;
        }
    }

    public void EnterRoomWithPW()
    {
        if ((string)myList[multiple + roomnumber].CustomProperties["password"] == PwCheckIF.text)
        {
            PhotonNetwork.JoinRoom(myList[multiple + roomnumber].Name);
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
        if (!PwErrorLog.activeSelf)
        {
            PwErrorLog.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            PwErrorLog.SetActive(false);
        }
    }

    #endregion

    private void Update()
    {
        if (PlayerPrefs.GetInt("Login") == 1)
        {
            isLoggin = true;
        }

        if (isGameStart == false && SceneManager.GetActiveScene().name == "StartScene" && isLoggin == true)
        {
            Debug.Log("Update : " + isGameStart + ", " + isLoggin);
            isGameStart = true;

            if (GameObject.Find("ChatText") != null)
            {
                chatText = GameObject.Find("ChatText").GetComponent<Text>();
            }

            if (GameObject.Find("Scroll View") != null)
            {
                scroll_rect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
            }

            //���� �÷��̾� ��ǲ �ʵ带 ��ü
            if (GameObject.Find("InputFieldChat") != null)
            {
                playerInput = GameObject.Find("InputFieldChat").GetComponent<InputField>();
            }

            if (GameObject.Find("ChattingButton") != null)
            {
                chattingBtn = GameObject.Find("ChattingButton").GetComponent<Button>();
                chattingBtn.onClick.AddListener(SetPlayerName);
            }

            StartCoroutine(CreatePlayer());
        }
    }
}
