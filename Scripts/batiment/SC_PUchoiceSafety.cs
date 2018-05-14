using UnityEngine;
using System.Collections;

public class SC_PUchoiceSafety : MonoBehaviour {
	
	private float f_timer;

	public bool b_timerOn = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//f_timer += ;

		if(b_timerOn) timerOn();

	}


	public void timerOn()
	{
		f_timer += Time.deltaTime;
		if(f_timer >= 1.5f)
		{
			Destroy(this.gameObject); 
		}
	}
}
