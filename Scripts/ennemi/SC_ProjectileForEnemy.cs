using UnityEngine;
using System.Collections;

public class SC_ProjectileForEnemy : MonoBehaviour {

	private GameObject[] a_Batiments;
	private Vector3 _Destination;
	public float f_Speed;

	// Use this for initialization
	void Start () 
	{
		a_Batiments = GameObject.FindGameObjectsWithTag("batiment");

	}
	
	// Update is called once per frame
	void Update () 
	{
		_Destination = FollowVillage ();

		if(Vector3.Distance(transform.position, _Destination) < 0.2f)
		{
			Destroy(this.gameObject);
		}
	}

	void FixedUpdate()
	{
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(_Destination.x, _Destination.y, transform.position.z), f_Speed * Time.deltaTime);
	}

	private Vector3 FollowVillage ()
	{
		Vector3 _ToGoTo;
		
		if(a_Batiments[FindClosestBat ()] != null)
		{
			_ToGoTo = a_Batiments[FindClosestBat ()].transform.position;
		}
		else
		{
			_ToGoTo = transform.position;
			Destroy(this.gameObject);
		}
		
		return _ToGoTo;
	}

	private int FindClosestBat ()
	{
		int _ClosestPoint = 0;
		
		if(a_Batiments.Length > 0)
		{
			for(int i = 1; i < a_Batiments.Length; i++)
			{
				if(a_Batiments[i] != null)
				{
					if(Vector3.Distance(transform.position, a_Batiments[i].transform.position) < Vector3.Distance(transform.position, a_Batiments[_ClosestPoint].transform.position))
					{
						_ClosestPoint = i;
					}
				}
			}
		}
		
		return _ClosestPoint;
	}
}
