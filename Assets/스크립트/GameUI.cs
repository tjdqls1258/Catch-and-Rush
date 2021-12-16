using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class GameUI : MonoBehaviourPun, IPunObservable
{
    public GameObject Blue_Goal;
    public GameObject Red_Goal;
    public GameObject Time_object;

    public Text Blue_Score;
    public Text Red_Score;
    public Text Timer;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Blue_Score.text);
            stream.SendNext(Red_Score.text);
            stream.SendNext(Timer.text);
        }
        else
        {
            Blue_Score.text = (string)stream.ReceiveNext();
            Red_Score.text = (string)stream.ReceiveNext();
            Timer.text = (string)stream.ReceiveNext();
        }
    }
    // Update is called once per frame
    void Update()
    {
        Blue_Score.text = Blue_Goal.GetComponent<Score_System>().get_Team_Score().ToString();
        Red_Score.text = Red_Goal.GetComponent<Score_System>().get_Team_Score().ToString();
        Timer.text = Time_object.GetComponent<Time_System_cs>().get_Time().ToString();
    }
}
