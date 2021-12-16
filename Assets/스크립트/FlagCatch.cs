using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class FlagCatch : MonoBehaviourPun, IPunObservable
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

}
