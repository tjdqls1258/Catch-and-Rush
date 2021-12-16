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
    }

    public int get_Team_Score()
    {
        return this.Team_Score;
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<PlayerCtrl>().get_flag == true &&
            player_Team == coll.gameObject.GetComponent<PlayerCtrl>().team)
        {
            GameObject Player = coll.gameObject;
            Add_Score(Player);
            PV.RPC("Add_Score", RpcTarget.Others, Player);
        }
    }

    [PunRPC]
    void Add_Score(GameObject Player)
    {
        //���� ����
        Team_Score++;
        //��� ���� ��.
        Player.gameObject.GetComponent<PlayerCtrl>().get_flag = false;
        //��� Ȱ��ȭ ���
        flag.GetComponent<FlagCatch>().Iscatched = false;
        //FollwFlaf ��Ȱ��ȭ
        flag.GetComponent<FollowFlag>().enabled = false;
        //��� �߾����� ��ġ ����
        flag.transform.position = new Vector3(70.0f, 5.0f, 40.0f);
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
}
