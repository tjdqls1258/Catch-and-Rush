using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_System_cs : MonoBehaviour
{   //시간
    float time;
    //초기 시간 값
    float start_Time=99999;

    // Start is called before the first frame update
    void Start()
    {
        time = start_Time;
        
    }

    // Update is called once per frame
    void Update()
    {
        //타임이 0보다 크면 시간 감소
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
