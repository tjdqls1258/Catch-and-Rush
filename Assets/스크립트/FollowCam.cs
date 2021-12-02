using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform Target;
    public float dist = 10.0f;
    public float hight = 3.0f;
    public float dampTrace = 20.0f;

    private Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (Target == null) return;
        tr.position = Vector3.Lerp(tr.position, Target.position - (Target.forward * dist) + (Vector3.up * hight), Time.deltaTime * dampTrace);
        tr.LookAt(Target.position);
    }
}
