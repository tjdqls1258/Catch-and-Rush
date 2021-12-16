using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Score_System : MonoBehaviourPun
{   //�÷��̾����� �ܺο��� �ٲ㼭 �� 1�� 2�� ����Բ�.

    public string player_Team;
    public GameObject flag;

    private PhotonView PV;

    //���� ����
    private int Team_Score = 0;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
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
            PV.RPC("Add_Score", RpcTarget.All, Player);
        }
    }

    [PunRPC]
    void Add_Score(GameObject Player)
    {
        StartCoroutine(Add_Score_With_Trigger(Player));
    }
    IEnumerator Add_Score_With_Trigger(GameObject Player)
    {
        yield return null;
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
    }
}
