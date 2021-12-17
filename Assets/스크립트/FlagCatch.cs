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
    private Transform tr;

    private Vector3 currPos;
    private Quaternion currRot;

    private PhotonView PV;
    
    private void Start()
    {
        tr = GetComponent<Transform>();
        PV = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter(Collider coll)
    {
        if ((coll.gameObject.tag == "Player") && (!Iscatched))
        {
            RPC_Get_Flag();
            FollowPlayer = coll.gameObject;
            PV.RPC("RPC_Get_Flag", RpcTarget.Others);
            Debug.Log("플레이어 충돌");
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
            RPC_Drop_Flag();
            PV.RPC("RPC_Drop_Flag", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void RPC_Get_Flag()
    {
        Iscatched = true;
        Flag.GetComponent<CapsuleCollider>().enabled = false;
        Flag.GetComponent<FollowFlag>().enabled = true;
        Flag.GetComponent<Rigidbody>().useGravity = false;
    }

    [PunRPC]
    public void RPC_Drop_Flag()
    {
        Iscatched = false;
        Flag.GetComponent<FollowFlag>().enabled = false;
        Flag.GetComponent<FlagCatch>().Iscatched = false;
        Flag.GetComponent<Rigidbody>().useGravity = true;
        Flag.GetComponent<CapsuleCollider>().enabled = true;
    }
}
