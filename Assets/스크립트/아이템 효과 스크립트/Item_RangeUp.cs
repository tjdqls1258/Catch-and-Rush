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
        
    }

    public void Item_use()
    {
        StartCoroutine(RangeUp());
    }

    IEnumerator RangeUp()
    {
        bullet.GetComponent<Bullet>().fireRange *= 2;
        yield return new WaitForSeconds(3.0f);

        bullet.GetComponent<Bullet>().fireRange /= 2;
    }
}
