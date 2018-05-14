using UnityEngine;
using System.Collections;

public class SC_trailFire : MonoBehaviour {

	SC_Ennemi SC_EnnemiRef;

	public SC_ScoreCounter SC_ScoreCounterRef;

	public float f_timeDuration;

	private float f_timer;

	private ParticleSystem p_fireParticle;
	public Color c_trailColor;

	public SC_playerComboCounter SC_playerComboCounterRef;

	// Use this for initialization
	void Start ()
	{
		//c_trailColor.a = 0.5f;
		//p_fireParticle.startColor = c_trailColor;
	 	
		f_timer = f_timeDuration;

	}
	
	// Update is called once per frame
	void Update ()
	{

		f_timer -= Time.deltaTime;
		if(f_timer <= 0)
		{
			if(collider.enabled == true) collider.enabled = false;
		}

		if(f_timer <= -f_timeDuration)
		{
			Destroy(gameObject);
		}
	
	}



	private void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.CompareTag("Ennemi"))
		{
			SC_EnnemiRef = col.gameObject.GetComponent<SC_Ennemi>();
			SC_EnnemiRef.death(new Vector3(0,0,0), 1,0,SC_playerComboCounterRef.i_killCount, SC_playerComboCounterRef.SC_PlayerRef);
			SC_ScoreCounterRef.IncreaseScore(SC_EnnemiRef.i_ScoreValue* Mathf.Clamp(SC_playerComboCounterRef.i_killCount, 1,999));

			SC_EnnemiRef.tag = null;
		}
	}



}
