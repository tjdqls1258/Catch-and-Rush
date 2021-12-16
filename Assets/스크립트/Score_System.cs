using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_System : MonoBehaviour
{   //플레이어팀은 외부에서 바꿔서 골 1과 2를 만들게끔.
    public string player_Team;
    public GameObject flag;

    //팀별 점수
    private int Team_Score = 0;

    public int get_Team_Score()
    {
        return this.Team_Score;
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<PlayerCtrl>().get_flag == true)
        {   //점수 증사
            if (player_Team == coll.gameObject.GetComponent<PlayerCtrl>().team)
            {
                Team_Score++;
                //깃발 없애 줌.
                coll.gameObject.GetComponent<PlayerCtrl>().get_flag = false;
                //깃발 활성화 취소
                flag.GetComponent<FlagCatch>().Iscatched = false;
                //FollwFlaf 비활성화
                flag.GetComponent<FollowFlag>().enabled = false;
                //깃발 중앙으로 위치 변경
                flag.transform.position = new Vector3(70.0f, 5.0f, 40.0f);
                //깃발 캡슐콜라이더 활성화
                flag.GetComponent<CapsuleCollider>().enabled = true;
                //깃발 리지드바디.중력 사용
                flag.GetComponent<Rigidbody>().useGravity = true;
                //깃발 중앙으로 위치 변경
            }
        }
    }
}
