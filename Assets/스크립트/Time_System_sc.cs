using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_System_sc : MonoBehaviour
{
    float time;
    float start_time=60;//���� ���� �ð�
    // Start is called before the first frame update
    void Start()
    {   //�ð��� ���� ���� �ð����� �ʱ�ȭ
        time = start_time;
    }

    // Update is called once per frame
    void Update()
    {
        //�ð��� 0�� �ƴ� ��쿡�� �ð��� ����
        if (time > 0)
        {
            //�ð��� �������� time�� ���ҽ�Ŵ
            time -= Time.deltaTime;
        }
       
    }
    public float get_time()
    {
        return ((int)time);
    }
}
