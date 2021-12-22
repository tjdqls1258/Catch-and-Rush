using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_RangeUp : MonoBehaviour
{
    public GameObject bullet;
    float fireRange_Up;
    float BulletSpeed_Up;
    // Start is called before the first frame update
    void Start()
    {
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
        StartCoroutine(RangeUp(player));
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

    IEnumerator RangeUp(GameObject player)
    {
        player.GetComponent<PlayerCtrl>().fireRange = player.GetComponent<PlayerCtrl>().base_fireRange * 2;
        player.GetComponent<PlayerCtrl>().BulletSpeed = player.GetComponent<PlayerCtrl>().base_BulletSpeed * 1.2f;
        yield return new WaitForSeconds(3.0f);

        player.GetComponent<PlayerCtrl>().fireRange = player.GetComponent<PlayerCtrl>().base_fireRange;
        player.GetComponent<PlayerCtrl>().BulletSpeed = player.GetComponent<PlayerCtrl>().base_BulletSpeed;
    }
}
