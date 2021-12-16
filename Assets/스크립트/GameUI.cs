using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject Blue_Goal;
    public GameObject Red_Goal;
    public GameObject Time_object;

    public Text Blue_Score;
    public Text Red_Score;
    public Text Timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Blue_Score.text = Blue_Goal.GetComponent<Score_System>().get_Team_Score().ToString();
        Red_Score.text = Red_Goal.GetComponent<Score_System>().get_Team_Score().ToString();
        Timer.text = Time_object.GetComponent<Time_System_cs>().get_Time().ToString();
    }
}
