using UnityEngine;
using System.Collections;

public class SC_ParticleGetParentColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		particleSystem.startColor = this.transform.parent.gameObject.renderer.material.color;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
