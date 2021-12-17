using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : MonoBehaviour//아이템 기본형 스크립트, 아이템 타입 = 태그로 받아와볼 예정
{
    private GameObject Item;
    private GameObject Player;
    public GameObject bullet;

    private float player_speed;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider coll)//임시로 만들어본 것이므로 오류가 많이 날 것
    {
        if((coll.gameObject.tag == "Player") && (Item.gameObject.tag == "SpeedUp"))//이동속도 증가 아이템 부분
        {
            StartCoroutine(SpeedUp());
        }

        if((coll.gameObject.tag == "Player") && (Item.gameObject.tag == "RangeUp"))//공격 사거리 증가 아이템 부분
        {
            StartCoroutine(RangeUp());
        }
    }

    IEnumerator SpeedUp()
    {
        player_speed = Player.GetComponent<PlayerCtrl>().speed;
        Player.GetComponent<PlayerCtrl>().speed *= 1.5f;
        yield return new WaitForSeconds(3.0f);

        Player.GetComponent<PlayerCtrl>().speed = player_speed;
    }

    IEnumerator RangeUp()
    {
        bullet.GetComponent<Bullet>().fireRange *= 2.0f;
        yield return new WaitForSeconds(3.0f);

        bullet.GetComponent<Bullet>().fireRange /= 2.0f;
    }
}
