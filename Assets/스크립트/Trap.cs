using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            //coll.gameObject.transform.Translate(coll.transform.position.x,coll.transform.position.y-2,coll.transform.position.z);
            coll.gameObject.transform.Translate(0f, -2.0f,0f) ;
        }
    }
}
