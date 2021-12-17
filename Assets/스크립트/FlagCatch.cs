using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class FlagCatch : MonoBehaviourPun, IPunObservable
{
    public bool Iscatched = false;
    public GameObject Flag;

    public Transform FollowPlayer;
    public string FollowPlayer_Name;
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
            RPC_Get_Flag(coll.gameObject.name);
            Debug.Log("플레이어 충돌");
        }
    }
    private void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "DAED_ZONE")
        {
            RPC_Drop_Flag();
            tr.position = new Vector3(70.0f, 5.0f, 40.0f);
        }
    }
    private void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag == "DAED_ZONE")
        {
            RPC_Drop_Flag();
            tr.position = new Vector3(70.0f, 5.0f, 40.0f);
        }
    }
    private void Update()
    {

    }

    [PunRPC]
    public void RPC_Get_Flag(string PlayerName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            FollowPlayer_Name = PlayerName;
            photonView.RPC("RPC_Get_Flag", RpcTarget.Others, FollowPlayer_Name);
            photonView.RPC("Rpc_Set_Target", RpcTarget.Others, FollowPlayer_Name);
        }
        Iscatched = true;
        Flag.GetComponent<CapsuleCollider>().enabled = false;
        Flag.GetComponent<FollowFlag>().enabled = true;
        Flag.GetComponent<Rigidbody>().useGravity = false;

    }
    [PunRPC]
    void Rpc_Set_Target(string target)
    {
        FollowPlayer_Name = target;
    }

    public void Drop_Flag()
    {
        RPC_Drop_Flag();
    }

    [PunRPC]
    void RPC_Drop_Flag()
    {
        Flag.GetComponent<FollowFlag>().enabled = false;
        Flag.GetComponent<Rigidbody>().useGravity = true;
        Flag.GetComponent<CapsuleCollider>().enabled = true;
        FollowPlayer_Name = null;
        Iscatched = false;
        photonView.RPC("RPC_Drop_Flag", RpcTarget.Others, FollowPlayer_Name);
        photonView.RPC("Rpc_Set_Target", RpcTarget.Others, FollowPlayer_Name);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(FollowPlayer_Name);
        }
        else
        {
            this.FollowPlayer_Name = (string)stream.ReceiveNext();
        }
    }
}
