using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_System_sc : MonoBehaviour
{
    float time;
    float start_time=60;//게임 진행 시간
    // Start is called before the first frame update
    void Start()
    {   //시간을 게임 진행 시간으로 초기화
        time = start_time;
    }

    // Update is called once per frame
    void Update()
    {
        //시간이 0이 아닐 경우에만 시간을 감소
        if (time > 0)
        {
            //시간이 지날수록 time을 감소시킴
            time -= Time.deltaTime;
        }
       
    }
    public float get_time()
    {
        return ((int)time);
    }
}
