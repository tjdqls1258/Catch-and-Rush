using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class Change_Text : MonoBehaviourPun, IPunObservable
{
    Text self;
    PhotonView pv;
    bool Is_Join = false;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        self = GetComponent<Text>();
    }
    public bool setText(string text)
    {
        if (self.text == "비어있음")
        {
            Debug.Log(self.text);
            pv.RPC("RPC_setText", RpcTarget.AllBuffered, text, true);
            return true;
        }
        return false;
    }
    [PunRPC]
    public void RPC_setText(string text, bool isNull)
    {
        self.text = text;
        Is_Join = isNull;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(self.text);
            stream.SendNext(Is_Join);
        }
        else
        {
            self.text = (string)stream.ReceiveNext();
            Is_Join = (bool)stream.ReceiveNext();
        }
    }
}
