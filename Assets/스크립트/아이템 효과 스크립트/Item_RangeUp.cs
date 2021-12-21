using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_RangeUp : MonoBehaviour, IItem_use
{
    public GameObject bullet;

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
    }

    IEnumerator RangeUp(GameObject player)
    {
        bullet.GetComponent<Bullet>().fireRange *= 2;
        yield return new WaitForSeconds(3.0f);

        bullet.GetComponent<Bullet>().fireRange /= 2;
    }
}
