using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Time_System_cs : MonoBehaviourPun, IPunObservable
{   //�ð�
    float Min;
    float Sec;
    float time;
    //�ʱ� �ð� ��
    float start_Time = 20;
    //�ǹ����� Ȯ���ϱ� ���� ����
    bool piver = false;
    //�¸� �� ������ string
    string Winner = "";
    [SerializeField]
    private GameObject player;
    private GameObject blue_team;
    private GameObject red_team;

    public GameObject Play_UI;
    public GameObject End_UI;

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
    }

    // Update is called once per frame
    void Update()
    {
        Min = Mathf.Floor(time / 60);
        Sec = Mathf.Floor(time % 60);
        //Ÿ���� 0�� �� �� ���� �ð� ����
        if (time >= 0)
        {
            AddTimer(-Time.deltaTime);
            texts.Time_min.text = Min.ToString();
            texts.Time_sec.text = Sec.ToString();
        }
        else //�ð��� 0���Ϸ� ��, ���� ��
        {
            time = 0;

            //������� ������ ������ �����Ǻ� �� �������� ���� string�� �߰�
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

        if ((time <= 60) && (time >= 0) && (piver==false) && (player))
        {
            piver = true;
            //�÷��� �̼� 2��
            player.GetComponent<PlayerCtrl>().Basic_speed *= 2.0f;
            //��� ���� 2��
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

    public void SetPlayer(GameObject Player)
    {
        player = Player;
    }
}