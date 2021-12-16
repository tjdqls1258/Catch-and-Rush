using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class Change_Text : MonoBehaviourPun, IPunObservable
{
    public Text self;
    PhotonView pv;
    bool Is_Join = false;
    bool Team_Set = true; // true = Red, false = Blue
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
            self.text = text;
            pv.RPC("RPC_setText", RpcTarget.AllBuffered, text, true);
            return true;
        }
        return false;
    }
    public void Set_Team(bool Team)
    {
        if (Team)
        {
            RPC_Set_Team_Color(Team);
            pv.RPC("RPC_Set_Team_Color", RpcTarget.AllBuffered, Team);
        }
        else
        {
            RPC_Set_Team_Color(Team);
            pv.RPC("RPC_Set_Team_Color", RpcTarget.AllBuffered, Team);
        }
    }

    [PunRPC]
    public void RPC_Set_Team_Color(bool Team)
    {
        if (Team)
        {
            self.color = new Color(1.0f, 0.0f, 0.0f);
        }
        else 
        {
            self.color = new Color(0.0f, 0.0f, 1.0f);
        }
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
