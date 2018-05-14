using UnityEngine;
using System.Collections;

public class SC_Bodyguard : MonoBehaviour {

	public float f_RayonDetect;
	public GameObject _Bodyguard;
	private GameObject _TheRealBodyGuard;
	private Collider[] a_PlayersAround;
	private bool b_IsPlayerAround;
	private GameObject[] a_Players;
	private Vector3 _Destination;
	
	private float f_TimeForPositionChange = 0.5f;
	private float f_StartTime;
	private float f_Timer;

	// Use this for initialization
	void Start () 
	{
		_Destination = new Vector3(((Random.value * f_RayonDetect * Random.Range(-1,1)) + transform.position.x +0.5f),
		                           ((Random.value * f_RayonDetect * Random.Range(-1,1)) + transform.position.y +0.5f),
		                           transform.position.z);

		_TheRealBodyGuard = Instantiate(_Bodyguard, _Destination, Quaternion.identity) as GameObject;
		_TheRealBodyGuard.transform.parent = this.transform;

		a_Players = GameObject.FindGameObjectsWithTag("Player");

		f_StartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		a_PlayersAround = Physics.OverlapSphere(transform.position, f_RayonDetect);

		for(int i = 0; i < a_PlayersAround.Length; i++)
		{
			if(a_PlayersAround[i].CompareTag("Player"))
			{
				b_IsPlayerAround = true;
				
				i = a_PlayersAround.Length;
			}
			else if(i == a_PlayersAround.Length -1)
			{
				b_IsPlayerAround = false;
			}
		}
	}

	void FixedUpdate ()
	{
		if(_TheRealBodyGuard != null)
		{
			if(b_IsPlayerAround)
			{
				_TheRealBodyGuard.GetComponent<SC_EnemyBehavior>().i_State = 2;
			}
			else
			{
				_TheRealBodyGuard.GetComponent<SC_EnemyBehavior>()._OutDestination = GetBGDestination ();
				_TheRealBodyGuard.GetComponent<SC_EnemyBehavior>().i_State = (int)_TheRealBodyGuard.GetComponent<SC_EnemyBehavior>().i_StateStart;
			}
		}
	}

	private Vector3 GetBGDestination ()
	{
		f_Timer = Time.time - f_StartTime;
		
		if(f_Timer > f_TimeForPositionChange)
		{
			_Destination = new Vector3(((Random.value * f_RayonDetect * Random.Range(-1,1)) + transform.position.x +0.5f),
			                           ((Random.value * f_RayonDetect * Random.Range(-1,1)) + transform.position.y +0.5f),
			                           transform.position.z);

			f_StartTime = Time.time;
		}
		else if(_Destination == null)
		{
			_Destination = new Vector3(((Random.value * f_RayonDetect * Random.Range(-1,1)) + transform.position.x +0.5f),
			                           ((Random.value * f_RayonDetect * Random.Range(-1,1)) + transform.position.y +0.5f),
			                           transform.position.z);
		}

		return _Destination;
	}

	private int FindClosestPlayer ()
	{
		int _ClosestPoint = 0;
		
		if(a_Players.Length > 0)
		{
			for(int i = 1; i < a_Players.Length; i++)
			{	
				if(a_Players[i] != null)
				{
					if(Vector3.Distance(transform.position, a_Players[i].transform.position) < Vector3.Distance(transform.position, a_Players[_ClosestPoint].transform.position))
					{
						
						_ClosestPoint = i;
					}
				}
			}
		}
		
		return _ClosestPoint;
	}
}
