using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_System : MonoBehaviour
{   //�÷��̾����� �ܺο��� �ٲ㼭 �� 1�� 2�� ����Բ�.
    public string player_Team;
    public GameObject flag;

    //���� ����
    private int Team_Score = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int get_Team_Score()
    {
        return this.Team_Score;
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<PlayerCtrl>().get_flag == true)
        {   //���� ����
            Team_Score++;
            //��� ���� ��.
            coll.gameObject.GetComponent<PlayerCtrl>().get_flag = false;
            //��� Ȱ��ȭ ���
            flag.GetComponent<FlagCatch>().Iscatched = false;
            //FollwFlaf ��Ȱ��ȭ
            flag.GetComponent<FollowFlag>().enabled = false;
            //��� �߾����� ��ġ ����
            flag.transform.Translate(new Vector3(70.0f,5.0f,40.0f));
            //��� ĸ���ݶ��̴� Ȱ��ȭ
            flag.GetComponent<CapsuleCollider>().enabled = true;
            //��� ������ٵ�.�߷� ���
            flag.GetComponent<Rigidbody>().useGravity = true;
            //��� �߾����� ��ġ ����

        }
    }
}
