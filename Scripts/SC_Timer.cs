using UnityEngine;
using System.Collections;

public class SC_Timer : MonoBehaviour {

	private float f_StartTime;
	private float f_CurrentTime;
	private string s_Timer;

	// Use this for initialization
	void Start () 
	{
		f_StartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		f_CurrentTime = Mathf.Round(Time.time - f_StartTime);
		s_Timer = f_CurrentTime.ToString();
	}

	void OnGUI ()
	{
		GUI.Box(new Rect(0, 0, 100, 50), s_Timer);
	}
}
