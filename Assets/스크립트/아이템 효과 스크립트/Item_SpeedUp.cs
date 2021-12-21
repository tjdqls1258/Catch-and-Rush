using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
using Photon.Realtime;
public class Item_SpeedUp : MonoBehaviour, IItem_use
{
    private float player_speed;

    // Start is called before the first frame update
    void Start()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(0.0f, 50.0f * Time.deltaTime, 0.0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        Item_use(other.gameObject);
    }

    public void Item_use(GameObject player)
    {
        StartCoroutine(SpeedUp(player));
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        StartCoroutine(Active_On());
    }
    IEnumerator Active_On()
    {
        yield return new WaitForSeconds(20.0f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    IEnumerator SpeedUp(GameObject player)
    {
        player_speed = player.GetComponent<PlayerCtrl>().speed;
        player.GetComponent<PlayerCtrl>().speed *= 1.5f;
        yield return new WaitForSeconds(3.0f);

        player.GetComponent<PlayerCtrl>().speed = player_speed;
    }
}
