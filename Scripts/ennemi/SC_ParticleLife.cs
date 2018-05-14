using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_ParticleLife : MonoBehaviour {

	public bool b_BatimentAggro = true;
	public bool b_NotPermanentParticule = false;
	public bool b_DestructibleParticule = false;



	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {		
		
	}

	void FixedUpdate()
	{
	}
	
	
	void OnTriggerEnter(Collider collision)
	{		
		if(b_BatimentAggro == true)
		{
			if(collision.transform.tag == "batiment")
			{
//				SC_batimentVie Sc_batimentVieRef = collision.GetComponent<SC_batimentVie>();
//				Sc_batimentVieRef.Damage();
//				Death();
			}		
		}

		if(collision.transform.tag == "Player")
		{
			if(b_DestructibleParticule == true)Death();
		}	
		
	}


	void OnTriggerExit(Collider collision)
	{

	}



	public void removeEnnemyInList()
	{
		collider.enabled = false;
	}

	public void Death()
	{
		Destroy(gameObject);
	}
	
}
