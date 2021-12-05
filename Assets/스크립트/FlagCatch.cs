using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCatch : MonoBehaviour
{
    bool Iscatched = false;
    public GameObject Flag;

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
        if(coll.gameObject.tag == "Player")
        {
            Iscatched = true;
            Flag.GetComponent<CapsuleCollider>().enabled = false;
            Flag.GetComponent<FollowFlag>().enabled = true;
        }
    }
}
