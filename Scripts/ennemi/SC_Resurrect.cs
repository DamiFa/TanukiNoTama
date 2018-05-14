using UnityEngine;
using System.Collections;

public class SC_Resurrect : MonoBehaviour {

	public GameObject _EnemyResurrect;
	public float f_TimeToResurrect;
	private GameObject[] a_Parts;
	public float f_DistanceOfParts;
	private bool b_InResurrection;
	private bool b_StartedResurrected;
	private bool b_ReleaseTheParts;
	private GameObject[] a_ThingsAround;
	private Vector3 _Pos1;
	private Vector3 _Pos2;

	private int i_StateToBe;


	private SC_SoundManager _Sounds;
	private AudioSource _AudioSource_Resurrect;

	private LineRenderer _Line;

	public ParticleSystem p_particleResurect;
	public ParticleSystem p_particleResurect1;
	public ParticleSystem p_particleResurect2;

	SC_particleDirection SC_particleDirection1;
	SC_particleDirection SC_particleDirection2;

	// Use this for initialization
	void Start () 
	{

		_AudioSource_Resurrect = gameObject.AddComponent<AudioSource>();
		_Sounds = GameObject.FindGameObjectWithTag("SOUND_MANAGER").GetComponent<SC_SoundManager>();

		p_particleResurect1 = Instantiate( p_particleResurect, transform.position, Quaternion.identity) as ParticleSystem;
		p_particleResurect2 = Instantiate( p_particleResurect, transform.position, Quaternion.identity) as ParticleSystem;


		SC_particleDirection1 = p_particleResurect1.GetComponent<SC_particleDirection>();
		SC_particleDirection2 = p_particleResurect2.GetComponent<SC_particleDirection>();

		i_StateToBe = 6;

		_Line = GetComponent<LineRenderer>();

		a_Parts = new GameObject[2];

		b_InResurrection = false;
		b_StartedResurrected = false;

		float _signe;

		for(int i = 0; i < 2; i++)
		{
			if(i%2 == 0)
			{
				_signe = 1;
			}
			else
			{
				_signe = -1;
			}

			GameObject temp = Instantiate(_EnemyResurrect, new Vector3(transform.position.x +f_DistanceOfParts*0.5f*_signe,
			                                         					transform.position.y,
			                                                            transform.position.z), Quaternion.identity) as GameObject;
			temp.transform.parent = this.transform;

			a_Parts[i] = temp;

			a_Parts[i].GetComponent<SC_EnemyBehavior>().i_State = i_StateToBe;
		}

		SC_particleDirection1.t_destination = a_Parts[1].transform;
		SC_particleDirection2.t_destination = a_Parts[0].transform;

		//p_particleResurect1.startSize = a_Parts[0].transform.localScale.x/2;
		//p_particleResurect2.startSize = a_Parts[1].transform.localScale.x/2;

		//p_particleResurect1.transform.parent = a_Parts[0].transform;
		//p_particleResurect2.transform.parent = a_Parts[1].transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		a_ThingsAround = GameObject.FindGameObjectsWithTag("batiment");

		if(a_Parts[0] != null)
		{
			p_particleResurect1.transform.position = a_Parts[0].transform.position;
			if(p_particleResurect1.enableEmission != true) p_particleResurect1.enableEmission = true;

		}
		if(a_Parts[1] != null)
		{
			p_particleResurect2.transform.position = a_Parts[1].transform.position;
			if(p_particleResurect2.enableEmission != true) p_particleResurect2.enableEmission = true;

		}

//		if(a_Parts[0] == null)
//		{
//			p_particleResurect1.enableEmission = false;
//		}
//		if(a_Parts[1] == null)
//		{
//			p_particleResurect2.enableEmission = false;
//		}



		if(a_Parts[0] == null && a_Parts[1] == null)
		{
			b_ReleaseTheParts = false;
			b_InResurrection = false;

			Destroy(p_particleResurect1);
			Destroy(p_particleResurect2);

			DeathOfBoth ();
		}
		else if(a_Parts[1] == null && a_Parts[0] != null)
		{
			if(!b_StartedResurrected)
			{
				b_InResurrection = true;

				StartCoroutine(Resurrection(_Pos2, 1));
				_Pos1 = a_Parts[0].transform.position;
			}
		}
		else if(a_Parts[0] == null && a_Parts[1] != null)
		{
			if(!b_StartedResurrected)
			{
				b_InResurrection = true;
				
				StartCoroutine(Resurrection(_Pos1, 0));
				_Pos2 = a_Parts[1].transform.position;
			}
		}
		else
		{
			_Pos1 = a_Parts[0].transform.position;
			_Pos2 = a_Parts[1].transform.position;
		}


		if(b_InResurrection)
		{
			for(int i = 0; i < 2; i++)
			{
				float _signe;

				if(i%2 == 0)
				{
					_signe = 1;
				}
				else
				{
					_signe = -1;
				}

				if(a_Parts[i] != null)
				{
					a_Parts[i].GetComponent<SC_EnemyBehavior>().i_State = i_StateToBe;

					a_Parts[i].GetComponent<SC_EnemyBehavior>()._OutDestination = new Vector3(transform.position.x +f_DistanceOfParts*0.5f*_signe,
					                                                                          transform.position.y,
					                                                                          transform.position.z);
				}
			}
		}
		else
		{
			if(b_ReleaseTheParts)
			{
				for(int i = 0; i < 2; i++)
				{
					a_Parts[i].GetComponent<SC_EnemyBehavior>().i_State = 3;
				}
			}
			else
			{
				for(int i = 0; i < 2; i++)
				{
					float _signe;
					
					if(i%2 == 0)
					{
						_signe = 1;
					}
					else
					{
						_signe = -1;
					}
					
					if(a_Parts[i] != null)
					{
						a_Parts[i].GetComponent<SC_EnemyBehavior>().i_State = i_StateToBe;

						a_Parts[i].GetComponent<SC_EnemyBehavior>()._OutDestination = new Vector3(transform.position.x +f_DistanceOfParts*0.5f*_signe,
						                                                                          transform.position.y,
						                                                                          transform.position.z);
					}
				}
			}
		}

		ChangeTarget ();
		Signal ();
	}

	IEnumerator Resurrection (Vector3 _WhereToResurrect, int i)
	{
		b_StartedResurrected = true;

		float f_StartTime = Time.time;
		float f_CurrentTime;

		while(b_InResurrection)
		{
			GetComponent<SC_EnemyBehavior>().i_State = 5;

			f_CurrentTime = Time.time - f_StartTime;

			if(f_CurrentTime > f_TimeToResurrect)
			{
				GameObject temp = Instantiate(_EnemyResurrect, _WhereToResurrect, Quaternion.identity) as GameObject;
				
				temp.transform.parent = this.transform;
				a_Parts[i] = temp;

				_AudioSource_Resurrect.PlayOneShot(_Sounds.GetSound("Resurrect"));

				GetComponent<SC_EnemyBehavior>().i_State = (int)GetComponent<SC_EnemyBehavior>().i_StateStart;

				b_InResurrection = false;

				SC_particleDirection1.t_destination = a_Parts[1].transform;
				SC_particleDirection2.t_destination = a_Parts[0].transform;

				//p_particleResurect1.transform.parent = a_Parts[0].transform;
				//p_particleResurect2.transform.parent = a_Parts[1].transform;

				yield return null;
			}
			else
			{
				yield return null;
			}
		}

		b_StartedResurrected = false;
		
		yield break;
	}

	void Signal ()
	{
//		_Line.SetPosition(0, _Pos1);
//		_Line.SetPosition(1, _Pos2);
	}

	private void ChangeTarget ()
	{
		for(int i = 0; i < a_ThingsAround.Length; i++)
		{
			if(Vector3.Distance(a_ThingsAround[i].transform.position, transform.position) < f_DistanceOfParts*1.5f)
			{
				b_ReleaseTheParts = true;

				i = a_ThingsAround.Length;
			}
			else if(i == a_ThingsAround.Length -1)
			{
				b_ReleaseTheParts = false;
			}
		}
	}

	void DeathOfBoth ()
	{
		Destroy(this.gameObject);
	}
}
