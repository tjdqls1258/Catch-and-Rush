using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : MonoBehaviour
{
    public GameObject Item;
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

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player" && Item.gameObject.tag == "SpeedUp")
        {
            StartCoroutine(SpeedUp());
        }

        if (coll.gameObject.tag == "Player" && Item.gameObject.tag == "RangeUp")
        {
            StartCoroutine(RangeUp());
        }
    }

    IEnumerator SpeedUp()
    {
        player_speed = Player.GetComponent<PlayerCtrl>().speed;
        Player.GetComponent<PlayerCtrl>().speed *= 1.2f;
        yield return new WaitForSeconds(3.0f);

        Player.GetComponent<PlayerCtrl>().speed = player_speed;
    }

    IEnumerator RangeUp()
    {
        bullet.GetComponent<Bullet>().fireRange *= 2;
        yield return new WaitForSeconds(3.0f);

        bullet.GetComponent<Bullet>().fireRange /= 2;
    }
}
