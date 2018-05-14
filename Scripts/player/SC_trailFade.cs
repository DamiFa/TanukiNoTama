using UnityEngine;
using System.Collections;

public class SC_trailFade : MonoBehaviour {
	
	
	public float f_fadeOutSpeed;
	
	public float f_alphaValue = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Color C_color = renderer.material.color;
		f_alphaValue -= f_fadeOutSpeed * Time.deltaTime;
		C_color.a = f_alphaValue;
		renderer.material.color = C_color;


		if(f_alphaValue < 0.01f) Destroy(gameObject);	
	}
	
	public void setFadeSpeed(float fadeSpeed){
		f_fadeOutSpeed = fadeSpeed;
	}
	
}
