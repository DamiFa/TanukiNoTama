using UnityEngine;
using System.Collections;

public class SC_particleSystemDestroyer : MonoBehaviour {


	public float f_duration = 4;
	private float f_timer;



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
		f_timer += Time.deltaTime;
		if(f_timer>f_duration) Destroy(this.gameObject);
	
	}
}
