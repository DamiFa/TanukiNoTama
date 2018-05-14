using UnityEngine;
using System.Collections;

public class SC_startParticle : MonoBehaviour {


	private SC_Player SC_PlayerRef;
	private float f_timer;

	// Use this for initialization
	void Start () {
		SC_PlayerRef = this.gameObject.transform.parent.gameObject.GetComponent<SC_Player>();
		particleSystem.startColor = SC_PlayerRef.s_brightColor;
	}
	
	// Update is called once per frame
	void Update () 
	{
		f_timer += Time.deltaTime;
		if(f_timer> particleSystem.duration) Destroy(gameObject);
	}
}
