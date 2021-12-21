using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class Time_System_cs : MonoBehaviourPun, IPunObservable
{   //시간
    float Min;
    float Sec;
    public float time;
    //초기 시간 값
    public float start_Time = 20;
    //피버인지 확인하기 위한 변수
    bool piver = false;
    public bool start_Game = false;

    //승리 팀 저장할 string
    string Winner = "";
    [SerializeField]
    private GameObject[] player;
    private GameObject blue_team;
    private GameObject red_team;

    public GameObject Play_UI;
    public GameObject End_UI;

    public Button Back_To_Lobby;
    public Button Exit_Game;

    public UI_IN_Game texts;

    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        time = start_Time;
        texts = GameObject.Find("InGame_UI").GetComponent<UI_IN_Game>();
        blue_team = GameObject.FindGameObjectWithTag("GOAL_BLUE");
        red_team = GameObject.FindGameObjectWithTag("GOAL_RED");
        PV = GetComponent<PhotonView>();

        Back_To_Lobby.onClick.AddListener(Back_Lobby_Event);
        Exit_Game.onClick.AddListener(Exit_Game_Event);
    }

    public void Exit_Game_Event()
    {
        Application.Quit();
    }

    public void Reset_Time(float startTime)
    {
        time = startTime;
        start_Game = true;
    }

    public void Back_Lobby_Event()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Min = Mathf.Floor(time / 60);
        Sec = Mathf.Floor(time % 60);
        //타임이 0이 될 때 까지 시간 감소
        if (time >= 0 && start_Game)
        {
            AddTimer(-Time.deltaTime);
        }
        else if(start_Game)//시간이 0이하로 즉, 게임 끝
        {
            start_Game = false;
            //어느쪽의 점수가 높은지 점수판별 후 높은쪽의 팀을 string에 추가
            if (blue_team.GetComponent<Score_System>().Team_Score >
                red_team.GetComponent<Score_System>().Team_Score)
            {
                Winner = "Blue team Win!";
                texts.Winnerteam.color = new Color(0.0f, 0f, 1.0f);
            }
            else if (blue_team.GetComponent<Score_System>().Team_Score <
                red_team.GetComponent<Score_System>().Team_Score)
            {
                Winner = "Red team Win!";
                texts.Winnerteam.color = new Color(1.0f, 0f, 0f);
            }
            else
            {
                Winner = "Draw!";
                texts.Winnerteam.color = new Color(0.0f, 1.0f, 0f);
            }


            Play_UI.SetActive(false);
            End_UI.SetActive(true);

            texts.Winnerteam.text = Winner;
        }

        if ((time <= 60) && (time >= 0) && (piver==false))
        {
            player = GameObject.FindGameObjectsWithTag("Player");
            piver = true;
            for (int i = 0; i < player.Length; i++)
            {
                //플레이 이속 2배
                player[i].GetComponent<PlayerCtrl>().Basic_speed *= 2.0f;
            }
            //깃발 점수 2배
            GameObject.Find("Team2_Score_Zone").GetComponent<Score_System>().Plus_Score = 2;
            GameObject.Find("Team1_Score_Zone").GetComponent<Score_System>().Plus_Score = 2;
        }
    }
    
    public int get_Time()
    {
        return ((int)time);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(time);
        }
        else
        {
            Set_Time((float)stream.ReceiveNext());
        }
    }
    [PunRPC]
    public void AddTimer(float deleteTime)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            time += deleteTime;
            String_Text();
            
            photonView.RPC("Set_Time", RpcTarget.Others, time);
            photonView.RPC("String_Text", RpcTarget.Others);
        }
    }
    [PunRPC]
    public void Set_Time(float timer)
    {
        time = timer;
    }
    [PunRPC]
    public void String_Text()
    {
         texts.Time_min.text = Min.ToString();
         texts.Time_sec.text = Sec.ToString();
    }
}