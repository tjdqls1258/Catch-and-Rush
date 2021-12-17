using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFlag : MonoBehaviour
{
    public Transform Player;
    public float hight = 5.0f;
    public float dampTrace = 20.0f;

    private Transform flagtr;

    // Start is called before the first frame update
    void Start()
    {
        flagtr = GetComponent<Transform>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (gameObject.GetComponent<FlagCatch>().Iscatched)
        {
            Player = gameObject.GetComponent<FlagCatch>().FollowPlayer;
            if (Player == null)
            {
                return;
            }
            flagtr.position = Vector3.Lerp(flagtr.position, Player.position + (Vector3.up * hight), Time.deltaTime * dampTrace);
        }
    }
}
