using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    public float Rotate_Speed;

    // Update is called once per frame
    void Update()
    {
        if (transform)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * Rotate_Speed);
        }
    }
}
