using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_System_cs : MonoBehaviour
{   //�ð�
    float time;
    //�ʱ� �ð� ��
    float start_Time=99999;

    // Start is called before the first frame update
    void Start()
    {
        time = start_Time;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Ÿ���� 0���� ũ�� �ð� ����
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
    }

    public int get_Time()
    {
        return ((int)time);
    }
}
