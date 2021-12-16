using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Time_System_cs : MonoBehaviour
{   //�ð�
    float time;
    //�ʱ� �ð� ��
    float start_Time=99999;

    public UI_IN_Game texts;

    // Start is called before the first frame update
    void Start()
    {
        time = start_Time;
        texts = GameObject.Find("Canvas").GetComponent<UI_IN_Game>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ÿ���� 0���� ũ�� �ð� ����
        if (time > 0)
        {
            texts.Timer.text = time.ToString();
            time -= Time.deltaTime;
        }
    }

    public int get_Time()
    {
        return ((int)time);
    }
}
