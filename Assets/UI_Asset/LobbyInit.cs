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
    public static LobbyInit instance;

    public InputField PlayerInput;
    public Button chattingBtn;
    bool isGameStart = false;
    bool isLoggIn = false;
    bool isReady = false;
    string playerName = "";
    string playerTeam = "";
    string connectionState = "";

    public string chatMessage;
    Text chatText;
    ScrollRect scroll_rect = null;

    PhotonView pv;

    Text connectionInfoText;

    [Header("LobbyCanvas")] public GameObject LobbyCanvas; //canvas
    public GameObject LobbyPanel; //login
    public GameObject RoomPanel; // lobby
    public GameObject WaitRoom; // ����
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
    public Button StartGame;
    public Button ChangeTeam;
    public int hashtablecount;

    //�� �ؽ�Ʈ
    [SerializeField] public Text Playe_1;
    [SerializeField] public Text Playe_2;
    [SerializeField] public Text Playe_3;
    [SerializeField] public Text Playe_4;

    //�������� �� ����Ʈ�� �����ϴ� ����
    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, mutiple, roomnuber;
    void Awake()
    {
        PhotonNetwork.GameVersion = "MyFps 1.0";
        PhotonNetwork.ConnectUsingSettings();
        pv = GetComponent<PhotonView>();
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
        if ((isGameStart == false) && (SceneManager.GetActiveScene().name == "MainScene") && (isLoggIn == true))
        {
            isGameStart = true;
            if (GameObject.Find("ChatText") != null)
            {
                chatText = GameObject.Find("ChatText").GetComponent<Text>();
            }
            if (GameObject.Find("Scroll View") != null)
            {
                scroll_rect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
            }

            if (GameObject.Find("InputFieldChat") != null)
            {
                PlayerInput = GameObject.Find("InputFieldChat").GetComponent<InputField>();
            }
            if (GameObject.Find("ChattingButton") != null)
            {
                chattingBtn = GameObject.Find("ChattingButton").GetComponent<Button>();
                chattingBtn.onClick.AddListener(SetPlayerName);
            }
            StartCoroutine(CreatPlayer());
        }
    }

    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //    Debug.Log("�κ� ����");
    //    PhotonNetwork.JoinRandomRoom();
    //}

    public static LobbyInit Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(LobbyInit)) as LobbyInit;

                if (instance == null)
                {
                    Debug.Log("no singleton obj");
                }
            }
            return instance;
        }
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            connectionState = "�뿡 ���� �� ...";
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
            connectionState = "�������� : ������ ������ ������� ���� \n���� ��õ��� ...";
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

        Debug.Log("�� ���� �Ϸ�");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        connectionState = "Joined Room";
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
        Debug.Log("�� ����");
        isLoggIn = true;
        PlayerPrefs.SetInt("LogIn", 1);

        //SceneManager.LoadScene("SampleScene");
        //PhotonNetwork.LoadLevel("MainScene");
        WaitRoom.SetActive(true);
        StartCoroutine(Set_waitName(playerName));
    }

    IEnumerator CreatPlayer()
    {
        while (!isGameStart)
        {
            yield return new WaitForSeconds(0.5f);
        }
        GameObject tempPlayer = PhotonNetwork.Instantiate("Player_01",
            new Vector3(2, 1, 11),
            Quaternion.identity,
            0);
        Debug.Log("�̸� : " + playerName);
        tempPlayer.GetComponent<PlayerCtrl>().SetPlayerName(PlayerPrefs.GetString("PlayerName"));
        tempPlayer.GetComponent<PlayerCtrl>().SetPlayerTeam(playerTeam);
        yield return null;
    }
    private void OnGUI()
    {
        GUILayout.Label(connectionState);
    }

    public void SetPlayerName()
    {
        Debug.Log(PlayerInput.text + " ��(��) �Է� �ϼ̽��ϴ�!");
        if (isGameStart == false && isLoggIn == false)
        {
            playerName = PlayerInput.text;
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerInput.text = string.Empty;
            Debug.Log("���� �̸� : " + playerName);
            Debug.Log("Connect �õ�" + isGameStart + ", " + isLoggIn);
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
        chatText.text += chat + "\n";

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
            new RoomOptions { MaxPlayers = 4 });
        LobbyPanel.SetActive(false);
        WaitRoom.SetActive(true);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        connectionState = "������ ������ ������...";
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
        isGameStart = false;
        isLoggIn = false;
        PlayerPrefs.SetInt("LogIn", 0);
    }

    //�游���
    public void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"password", RoomPwInput.text}
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        if (PwToggle.isOn)
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : "*" + RoomInput.text,
                roomOptions);
        }
        else
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text,
                new RoomOptions { MaxPlayers = 4 });
        }

        MakeRoomPanel.SetActive(false);
        WaitRoom.SetActive(true);
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

        else if (myList[mutiple + num].CustomProperties["password"] != null)
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
        switch (number)
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
            case 4:
                roomnuber = 4;
                break;
            case 5:
                roomnuber = 5;
                break;
            case 6:
                roomnuber = 6;
                break;
            case 7:
                roomnuber = 7;
                break;
            default:
                break;
        }
    }

    public void EnterRoomWothPW()
    {
        if ((string)myList[mutiple + roomnuber].CustomProperties["password"] == PwCheckIF.text)
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
        if (!PwErrorLog.activeSelf)
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
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (mutiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text =
                (mutiple + i < myList.Count) ? myList[mutiple + i].Name : "";
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text += (mutiple + i < myList.Count)
                ? " " + myList[mutiple + i].PlayerCount + "/" + myList[mutiple + i].MaxPlayers : "";
        }
    }

    private void OnConnectedToServer()
    {
        Debug.Log("OnConnectedToServer");
        isReady = true;
    }
    IEnumerator Set_waitName(string name)
    {
        yield return null;
        if (Playe_1.GetComponent<Change_Text>().setText(name))
        {
            Playe_1.GetComponent<Change_Text>().Set_Team(true);
            playerTeam = "Red";
        }
        else if (Playe_2.GetComponent<Change_Text>().setText(name))
        {
            Playe_2.GetComponent<Change_Text>().Set_Team(true);
            playerTeam = "Red";
        }
        else if (Playe_3.GetComponent<Change_Text>().setText(name))
        {
            Playe_3.GetComponent<Change_Text>().Set_Team(false);
            playerTeam = "Blue";
        }
        else if (Playe_4.GetComponent<Change_Text>().setText(name))
        {
            Playe_4.GetComponent<Change_Text>().Set_Team(false);
            playerTeam = "Blue";
        }
    }

    public void Change_Team()
    {
        Set_Change_Team();
    }
    void Set_Change_Team()
    {
        if (playerName == Playe_1.GetComponent<Change_Text>().self.text)
        {
            if (playerTeam == "Red")
            {
                Playe_1.GetComponent<Change_Text>().Set_Team(false);
                playerTeam = "Blue";
            }
            else
            {
                Playe_1.GetComponent<Change_Text>().Set_Team(true);
                playerTeam = "Red";
            }
        }
        else if (playerName == Playe_2.GetComponent<Change_Text>().self.text)
        {
            if (playerTeam == "Red")
            {
                Playe_2.GetComponent<Change_Text>().Set_Team(false);
                playerTeam = "Blue";
            }
            else
            {
                Playe_2.GetComponent<Change_Text>().Set_Team(true);
                playerTeam = "Red";
            }
        }
        else if (playerName == Playe_3.GetComponent<Change_Text>().self.text)
        {
            if (playerTeam == "Red")
            {
                Playe_3.GetComponent<Change_Text>().Set_Team(false);
                playerTeam = "Blue";
            }
            else
            {
                Playe_3.GetComponent<Change_Text>().Set_Team(true);
                playerTeam = "Red";
            }
        }
        else
        {
            if (playerTeam == "Red")
            {
                Playe_4.GetComponent<Change_Text>().Set_Team(false);
                playerTeam = "Blue";
            }
            else
            {
                Playe_4.GetComponent<Change_Text>().Set_Team(true);
                playerTeam = "Red";
            }
        }
    }
    [PunRPC]
    public void Start_Game()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("InMain", RpcTarget.All);
        }
    }
    [PunRPC]
    void InMain()
    {
        PhotonNetwork.LoadLevel("MainScene");
    }
    [PunRPC]
    void DisConnect_waitName()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Playe_1.text == playerName)
            {
                Playe_1.text = "�������";
                photonView.RPC("DisConnect_waitName", RpcTarget.Others, Playe_1.text);
            }
            else if (Playe_2.text == playerName)
            {
                Playe_2.text = "�������";
                photonView.RPC("DisConnect_waitName", RpcTarget.Others, Playe_2.text);
            }
            else if (Playe_3.text == playerName)
            {
                Playe_3.text = "�������";
                photonView.RPC("DisConnect_waitName", RpcTarget.Others, Playe_3.text);
            }
            else if (Playe_4.text == playerName)
            {
                Playe_4.text = "�������";
                photonView.RPC("DisConnect_waitName", RpcTarget.Others, Playe_4.text);
            }
        }
        else
        {

        }
    }
}
