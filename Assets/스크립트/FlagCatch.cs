using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class FlagCatch : MonoBehaviourPun
{
    public bool Iscatched = false;
    public GameObject FollowPlayer;
    public GameObject Flag;

    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

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
