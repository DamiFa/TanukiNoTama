using UnityEngine;
using System.Collections;

public class SC_Projectile : MonoBehaviour {

	private float f_Speed = 30f;
	private float f_Distance = 100f;
	private Vector3 _StartPos;

	// Use this for initialization
	void Start () 
	{
		_StartPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Move ();

		if(Vector3.Distance(_StartPos, transform.position) > f_Distance)
		{
			Destroy(this.gameObject);
		}
	}

	private void Move ()
	{
		rigidbody.velocity = transform.forward * f_Speed;
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.CompareTag("Ennemi"))
		{
			col.gameObject.SendMessage("TouchedProjectile");
			Destroy(this.gameObject);
		}

		if(col.gameObject.CompareTag("Wall"))
		{
			Destroy(this.gameObject);
		}
	}

	private void SetSpeedProjectile (float f_SpeedToSet)
	{
		f_Speed = f_SpeedToSet;
	}

	private	void SetDistanceProjectile (float f_DistanceToSet)
	{
		f_Distance = f_DistanceToSet;
	}
}
