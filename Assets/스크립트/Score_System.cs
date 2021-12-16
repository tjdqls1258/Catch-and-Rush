using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Score_System : MonoBehaviourPun
{   //�÷��̾����� �ܺο��� �ٲ㼭 �� 1�� 2�� ����Բ�.

    public string player_Team;
    public GameObject flag;
    public UI_IN_Game texts;

    //���� ����
    private int Team_Score = 0;

    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        texts = GameObject.Find("Canvas").GetComponent<UI_IN_Game>();
        texts.Blue_Score.text = Team_Score.ToString();
        texts.Red_Score.text = Team_Score.ToString();
    }

    public int get_Team_Score()
    {
        return this.Team_Score;
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if ((coll.gameObject.GetComponent<PlayerCtrl>().get_flag == true) &&
                (player_Team == coll.gameObject.GetComponent<PlayerCtrl>().team))
            {
                Add_Score();
                PV.RPC("Add_Score", RpcTarget.Others);
                flag.transform.position = new Vector3(70.0f, 5.0f, 40.0f);
            }
        }
    }

    [PunRPC]
    void Add_Score()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //���� ����
            Team_Score++;
            photonView.RPC("Add_Score", RpcTarget.Others);
            photonView.RPC("ApplyAdd_Score", RpcTarget.Others, Team_Score);
        }

        //��� Ȱ��ȭ ���
        flag.GetComponent<FlagCatch>().Iscatched = false;
        //FollwFlaf ��Ȱ��ȭ
        flag.GetComponent<FollowFlag>().enabled = false;
        //��� ĸ���ݶ��̴� Ȱ��ȭ
        flag.GetComponent<CapsuleCollider>().enabled = true;
        //��� ������ٵ�.�߷� ���
        flag.GetComponent<Rigidbody>().useGravity = true;

        if(player_Team == "Blue")
        {
            texts.Blue_Score.text = Team_Score.ToString();
        }
        else if(player_Team == "Red")
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
