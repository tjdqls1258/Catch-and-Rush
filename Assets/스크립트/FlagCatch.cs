using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCatch : MonoBehaviour
{
    public bool Iscatched = false;
    public GameObject FollowPlayer;
    public GameObject Flag;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Iscatched = true;
            FollowPlayer = coll.gameObject;
            Flag.GetComponent<CapsuleCollider>().enabled = false;
            Flag.GetComponent<FollowFlag>().enabled = true;
            Flag.GetComponent<Rigidbody>().useGravity = false;
        }
    }
    private void Update()
    {
        if (FollowPlayer)
        {
            if (!FollowPlayer.GetComponent<PlayerCtrl>().get_flag)
            {
                Flag.GetComponent<FollowFlag>().enabled = false;
                Flag.GetComponent<FlagCatch>().Iscatched = false;
                Flag.GetComponent<Rigidbody>().useGravity = true;
                Flag.GetComponent<CapsuleCollider>().enabled = true;
            }
        }
        else
        {
            Flag.GetComponent<FollowFlag>().enabled = false;
            Flag.GetComponent<FlagCatch>().Iscatched = false;
            Flag.GetComponent<Rigidbody>().useGravity = true;
            Flag.GetComponent<CapsuleCollider>().enabled = true;
        }
    }
}
