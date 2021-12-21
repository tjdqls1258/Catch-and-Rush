using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Score_System : MonoBehaviourPun
{   //플레이어팀은 외부에서 바꿔서 골 1과 2를 만들게끔.

    public string player_Team;
    public GameObject flag;
    public UI_IN_Game texts;

    //팀별 점수
    public int Team_Score = 0;
    //깃발을 넣었을 시 얻을 수 있는 점수(피버 타임일 경우에는 2배)
    public int Plus_Score = 1;
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        texts = GameObject.Find("InGame_UI").GetComponent<UI_IN_Game>();
        texts.Blue_Score.text = Team_Score.ToString();
        texts.Red_Score.text = Team_Score.ToString();
    }
    public void Reset_Score()
    {
        Team_Score = 0;
        Plus_Score = 1;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if ((coll.gameObject.GetComponent<PlayerCtrl>().get_flag == true) &&
                (player_Team == coll.gameObject.GetComponent<PlayerCtrl>().team))
            {
                Add_Score();
                flag.transform.position = new Vector3(70.0f, 5.0f, 40.0f);
            }
        }
    }

    [PunRPC]
    void Add_Score()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //점수 증가
            Debug.Log("isMaster");
            Team_Score+= Plus_Score;
            photonView.RPC("ApplyAdd_Score", RpcTarget.Others, Team_Score);
            photonView.RPC("Add_Score", RpcTarget.Others);
        }

        //깃발 활성화 취소
        flag.GetComponent<FlagCatch>().Iscatched = false;
        //FollwFlaf 비활성화
        flag.GetComponent<FollowFlag>().enabled = false;
        //깃발 캡슐콜라이더 활성화
        flag.GetComponent<CapsuleCollider>().enabled = true;
        //깃발 리지드바디.중력 사용
        flag.GetComponent<Rigidbody>().useGravity = true;

        if (player_Team == "Blue")
        {
            texts.Blue_Score.text = Team_Score.ToString();
        }
        else if (player_Team == "Red")
        {
            texts.Red_Score.text = Team_Score.ToString();
        }
    }

    [PunRPC]
    public void ApplyAdd_Score(int score)
    {
        Team_Score = score;
    }
}
