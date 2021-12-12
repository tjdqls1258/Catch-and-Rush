using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string team = "";
    public float speed = 10;
    public float damage = 300;
    public float fireRange = 10;

    private Transform tr;
    private Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        spawnPoint = tr.position;
    }

    // Update is called once per frame
    void Update()
    {
        tr.Translate(Vector3.forward * Time.deltaTime * speed);
        if ((spawnPoint - tr.position).sqrMagnitude > fireRange)
        {
            StartCoroutine(this.DestoyBullet());
        }
    }

    IEnumerator DestoyBullet()
    {
        yield return null;
        Destroy(gameObject);
    }
}
