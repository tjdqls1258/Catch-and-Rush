using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_SpeedUp : MonoBehaviour, IItem_use
{
    private GameObject Player;

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

    public void Item_use()
    {
        StartCoroutine(SpeedUp());
    }

    IEnumerator SpeedUp()
    {
        player_speed = Player.GetComponent<PlayerCtrl>().speed;
        Player.GetComponent<PlayerCtrl>().speed *= 1.2f;
        yield return new WaitForSeconds(3.0f);

        Player.GetComponent<PlayerCtrl>().speed = player_speed;
    }
}
