using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Photon.Pun;
using Photon.Realtime;
public class Item_SpeedUp : MonoBehaviour
{
    private float player_speed;

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
        player.GetComponent<PlayerCtrl>().speed = player.GetComponent<PlayerCtrl>().Basic_speed * 1.5f;
        yield return new WaitForSeconds(3.0f);

        player.GetComponent<PlayerCtrl>().speed = player.GetComponent<PlayerCtrl>().Basic_speed;
    }
}
