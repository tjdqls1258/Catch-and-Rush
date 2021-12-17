using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Time_System_cs : MonoBehaviour
{   //시간
    float Min;
    float Sec;
    float time;
    //초기 시간 값
    float start_Time=20;

    //승리 팀 저장할 string
    string Winner = "";

    private GameObject player;
    private GameObject blue_team;
    private GameObject red_team;

    public GameObject Play_UI;
    public GameObject End_UI;

    public UI_IN_Game texts;

    // Start is called before the first frame update
    void Start()
    {
        time = start_Time;
        texts = GameObject.Find("InGame_UI").GetComponent<UI_IN_Game>();
        player = GameObject.FindGameObjectWithTag("Player");
        blue_team = GameObject.FindGameObjectWithTag("GOAL_BLUE");
        red_team = GameObject.FindGameObjectWithTag("GOAL_RED");
    }

    // Update is called once per frame
    void Update()
    {
        Min = Mathf.Floor(time / 60);
        Sec = Mathf.Floor(time % 60);
        //타임이 0이 될 때 까지 시간 감소
        if (time >= 0)
        {
            texts.Time_min.text = Min.ToString();
            texts.Time_sec.text = Sec.ToString();
            time -= Time.deltaTime;
        }
        else //시간이 0이하로 즉, 게임 끝
        {
            time = 0;

            //어느쪽의 점수가 높은지 점수판별 후 높은쪽의 팀을 string에 추가
            if(blue_team.GetComponent<Score_System>().Team_Score >
                red_team.GetComponent<Score_System>().Team_Score)
            {
                Winner = "Blue team Win!";
            }
            else if(blue_team.GetComponent<Score_System>().Team_Score <
                red_team.GetComponent<Score_System>().Team_Score)
            {
                Winner = "Red team Win!";
            }
            else
            {
                Winner = "Draw!";
            }


            Play_UI.SetActive(false);
            End_UI.SetActive(true);

            texts.Winnerteam.text = Winner;
        }

        if(time <= 60 && time>=0)
        {
            //피버타임
            //player.GetComponent<PlayerCtrl>().speed *= 2.0f; //이동속도 증가
            //공속증가 => 코루틴 건드려야할듯
        }
    }

    public int get_Time()
    {
        return ((int)time);
    }
}
