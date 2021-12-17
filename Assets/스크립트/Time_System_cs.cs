using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Time_System_cs : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        time = start_Time;
        texts = GameObject.Find("InGame_UI").GetComponent<UI_IN_Game>();
        blue_team = GameObject.FindGameObjectWithTag("GOAL_BLUE");
        red_team = GameObject.FindGameObjectWithTag("GOAL_RED");
    }

    // Update is called once per frame
    void Update()
    {
        Min = Mathf.Floor(time / 60);
        Sec = Mathf.Floor(time % 60);
        //Ÿ���� 0�� �� �� ���� �ð� ����
        if (time >= 0)
        {
            texts.Time_min.text = Min.ToString();
            texts.Time_sec.text = Sec.ToString();
            time -= Time.deltaTime;
        }
        else //�ð��� 0���Ϸ� ��, ���� ��
        {
            time = 0;

            //������� ������ ������ �����Ǻ� �� �������� ���� string�� �߰�
            if (blue_team.GetComponent<Score_System>().Team_Score >
                red_team.GetComponent<Score_System>().Team_Score)
            {
                Winner = "Blue team Win!";
            }
            else if (blue_team.GetComponent<Score_System>().Team_Score <
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

        if (time <= 60 && time >= 0&& piver==false)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            piver = true;
            //�÷��� �̼� 2��
            player.GetComponent<PlayerCtrl>().speed *= 2.0f;
            //��� ���� 2��
            GameObject.Find("Team2_Score_Zone").GetComponent<Score_System>().Plus_Score = 2;
            GameObject.Find("Team1_Score_Zone").GetComponent<Score_System>().Plus_Score = 2;
        }
    }

    public int get_Time()
    {
        return ((int)time);
    }
}
