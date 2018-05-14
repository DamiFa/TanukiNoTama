using UnityEngine;
using System.Collections;

public class SC_EnemyBehavior : MonoBehaviour {

	public enum e_State {FollowPath = 1, FollowPlayer = 2, FollowVillage = 3, Rien = 5, OutDestination = 6};
	public e_State i_StateStart = e_State.FollowPlayer;

	private AudioSource _AudioSource_RunAway;
	private SC_SoundManager _Sounds;
	private bool b_SoundRunAway;

	public bool b_RunFromPlayer = false;
	public float f_DistanceRunFromPlayer = 0f;
	public float f_TimeRunning;
	private float f_Timer;
	
	private SC_PathManager _PathManager;
	private Transform[] a_Path;
	private float f_Percent;
	public float f_Speed;
	public int i_State;
	public string s_PathName;
	private float f_goodSpeed;
	private bool b_IsOnPath;
	private GameObject[] a_Players;
	private GameObject[] a_Batiments;

	private Vector3 _PointToFollow;

	public Vector3 _OutDestination;
	
	public Vector3 _Destination;
	private float f_Signe;

	//Pour l'Enemy Manager
	public int i_EnemyType;
	public int i_FormationId;
	
	// Use this for initialization
	private void Start () 
	{
		b_SoundRunAway = true;
		_AudioSource_RunAway = gameObject.AddComponent<AudioSource>();
		_Sounds = GameObject.FindGameObjectWithTag("SOUND_MANAGER").GetComponent<SC_SoundManager>();


		a_Players = GameObject.FindGameObjectsWithTag("Player");
		a_Batiments = GameObject.FindGameObjectsWithTag("batiment");
		_PathManager = GameObject.FindGameObjectWithTag("PATH_MANAGER").GetComponent<SC_PathManager>();
		
		a_Path = _PathManager.GetEnemyPath(s_PathName);

		i_FormationId = 0;

		i_State = (int)i_StateStart;

		b_IsOnPath = false;
		
		f_goodSpeed = f_Speed / iTween.PathLength(a_Path);

		f_Timer = 0;
	}
	
	// Update is called once per frame
	private void Update ()
	{
		switch(i_State)
		{
			case 1:
			{
				f_Signe = 1;
				_Destination = FollowPath ();
			}break;
				
			case 2:
			{
				f_Signe = 1;
				_Destination = FollowPlayer ();
			}break;

			case 3:
			{
				f_Signe = 1;
				_Destination = FollowVillage ();
			}break;

			case 4:
			{
				f_Signe = -1.5f;
				_Destination = FollowPlayer ();
			}break;

			case 5:
			{
				f_Signe = 1;
				_Destination = transform.position;
			}break;

			case 6:
			{
				if(_OutDestination == null)
				{
					_OutDestination = transform.position;
				}
				else
				{
					f_Signe = 1;
					_Destination = _OutDestination;
				}

			}break;
				
			default:
			{

			}break;
		}

		if(b_RunFromPlayer)
		{
			if(a_Players[FindClosestPlayer()] != null)
			{
				if(Vector3.Distance(transform.position, a_Players[FindClosestPlayer()].transform.position) < f_DistanceRunFromPlayer)
				{
					if(b_SoundRunAway)
					{
						_AudioSource_RunAway.PlayOneShot(_Sounds.GetSound("RunAway"));
						b_SoundRunAway = false;
					}

					i_State = 4;
				}
				else
				{
					if(f_Timer > f_TimeRunning)
					{
						b_SoundRunAway = true;


						i_State = (int)i_StateStart;
						f_Timer = 0;
					}
					else
					{
						f_Timer ++;
					}

				}
			}
		}
	}
	
	void FixedUpdate()
	{
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(_Destination.x, _Destination.y, transform.position.z), f_Speed * Time.deltaTime * f_Signe);
	}
	
	private float RectifiedSpeed ()
	{
		Vector3 _PointAhead = iTween.PointOnPath(a_Path, f_Percent + 0.01f);
		float f_RealDistance = Vector3.Distance(transform.position, _PointAhead);
		
		float f_PathLenght = iTween.PathLength(a_Path);
		
		float f_Distortion = (f_PathLenght * 0.01f) / f_RealDistance;
		
		return f_Distortion;
	}

	private Vector3 FollowPath ()
	{
		if(!b_IsOnPath)
		{
			float a = FindColsestPointOnPath ();
			float b = a_Path.Length - 1;
			
			f_Percent = a/b;

			if(Vector3.Distance(transform.position, a_Path[FindColsestPointOnPath ()].position) < 3)
			{
				b_IsOnPath = true;
			}
		}
		else
		{
			f_Percent = (f_Percent + f_goodSpeed * 0.1f * RectifiedSpeed ()) % 1.0f;
		}

		_PointToFollow = iTween.PointOnPath(a_Path, f_Percent);
		
		return _PointToFollow;
	}
	
	private Vector3 FollowPlayer ()
	{
		Vector3 _ToGoTo = transform.position;

		if(a_Players.Length > 0)
		{
			_ToGoTo = a_Players[FindClosestPlayer ()].transform.position;
		}
		else
		{
			_ToGoTo = FollowVillage();
		}

		return _ToGoTo;
	}

	private Vector3 FollowVillage ()
	{
		Vector3 _ToGoTo;

		if(a_Batiments.Length > 0)
		{
			_ToGoTo = a_Batiments[FindClosestBat ()].transform.position;
		}
		else
		{
			_ToGoTo = FollowPath();
		}
		
		return _ToGoTo;
	}
	
	private int FindColsestPointOnPath ()
	{
		int _ClosestPoint = 0;
		
		for(int i = 1; i < a_Path.Length; i++)
		{
			if(Vector3.Distance(transform.position, a_Path[i].transform.position) < Vector3.Distance(transform.position, a_Path[_ClosestPoint].transform.position))
			{
				_ClosestPoint = i;
			}
		}
		
		return _ClosestPoint;
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
