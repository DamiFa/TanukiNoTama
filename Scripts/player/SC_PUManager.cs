using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SC_PUManager : MonoBehaviour {

	private SC_Player SC_PlayerRef;
	private SC_playerCombo SC_PlayerComboRef;
	private SC_playerComboCounter SC_playerComboCounterRef;
	private SC_ScoreCounter  SC_ScoreCounterRef;


	//POUR LE LASER
	public GameObject _CaCLaser;
	public float f_Laser;
	private bool b_IsLasering;

	//POUR LES MISSILES
	public GameObject g_MissilePrefab;
	public float f_numberOfMissile;
	private bool b_missileLaunched;
	private Collider[] _ThingsAround;
	private List<GameObject> a_EnemiesAround;

	//DASH TRAIL
	public Transform g_fireDashTrailPrefab;
	public ParticleSystem p_trailParticle;
	public float f_dashFire;
	public float f_dashFade;
	public bool b_dashFire = false;
	SC_dashTrail SC_dashTrailRef;
	float f_initialDashFade;
	float f_initialTrailSpeed;

	//Audio
	private SC_SoundManager _Sounds;
	public AudioSource _PuAudioSource;


	//ONDE DE CHOC
	public float f_ODC = 10f;
	public bool b_IsODCing;
	public GameObject _ODCanim;

	//list des batiments
	private List<GameObject> l_batimentList = new List<GameObject>();
	bool b_batimentActivated = false;

	// Use this for initialization
	void Start () 
	{
		_Sounds = GameObject.FindGameObjectWithTag("SOUND_MANAGER").GetComponent<SC_SoundManager>();

		SC_PlayerRef = this.GetComponent<SC_Player>();
		SC_playerComboCounterRef = SC_PlayerRef.SC_playerComboCounterRef;

		SC_ScoreCounterRef = GameObject.Find("guiSCOREcounter").GetComponent<SC_ScoreCounter>();	
		SC_PlayerRef = GetComponent<SC_Player>();
		SC_PlayerComboRef = GetComponent<SC_playerCombo>();

		//POUR LE DASHFIRE
		SC_dashTrailRef = GetComponent<SC_dashTrail>();
		f_initialDashFade = SC_PlayerRef.f_TrailfadeOutSpeed;
		f_initialTrailSpeed = SC_dashTrailRef.f_TrailSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(SC_PlayerRef.b_IsInPuMode != true)
		{
			if(b_batimentActivated)
			{
				deactiveVisualBatiment();
				b_batimentActivated = false;
			}

			if(b_dashFire) deactivateFireTrail();

		}

		if(SC_PlayerRef.b_IsInPuMode)
		{
			if(b_batimentActivated != true)
			{
				activeVisualBatiment();
				b_batimentActivated = true;
			}


			//POUR LE LASER
			if(f_Laser > 0)
			{	
				if(SC_PlayerRef.b_HasTouchedForPU == true)b_IsLasering = true;
				if(b_IsLasering)
				{
					Laser();
				}
			}

			//POUR LES MISSILES
			if(f_numberOfMissile > 0)
			{
				if(SC_PlayerRef.b_HasTouchedForPU == true)b_missileLaunched = true;

				if(b_missileLaunched)
				{
					_ThingsAround = Physics.OverlapSphere(transform.position, 25f);
					a_EnemiesAround = new List<GameObject>();
					Missile();
					b_missileLaunched = false;
				}
			}

			//FIRE TRAIL
			if(f_dashFire>0)
			{			
				if(b_dashFire != true) fireTrail();		
			}
			//ONDE DE CHOC
			if(f_ODC > 0)
			{		
				if(SC_PlayerRef.b_HasTouchedForPU == true)b_IsODCing = true;

				if(b_IsODCing)
				{
					ODC ();
				}		
			}
		}

		SC_PlayerRef.b_HasTouchedForPU = false;
	}


	public void fireTrail()
	{

		if(b_dashFire != true) StartCoroutine(firetrailSpawner());
		b_dashFire = true;

		p_trailParticle.startLifetime = f_dashFire *0.7f;
		p_trailParticle.startColor = SC_PlayerRef.s_brightColor;
		p_trailParticle.enableEmission = true;
		/*
		SC_dashTrailRef._TrailPrefab = g_fireDashTrailPrefab;	
		SC_PlayerRef.f_TrailfadeOutSpeed = f_initialDashFade - (f_dashFire/5);
		if(f_initialDashFade -(f_dashFire/5) < 0.5f) SC_PlayerRef.f_TrailfadeOutSpeed = 0.5f;
		SC_dashTrailRef.f_TrailSpeed = 0.008f;
		*/
	}

	public void deactivateFireTrail()
	{
		b_dashFire = false;
		p_trailParticle.enableEmission = false;
	}

	IEnumerator firetrailSpawner()
	{		
		Transform Trail = Instantiate(g_fireDashTrailPrefab, new Vector3(transform.position.x,  transform.position.y, -0.04f), transform.rotation) as Transform;

		SC_trailFire SC_trailFireRef =  Trail.GetComponent<SC_trailFire>();

		SC_trailFireRef.f_timeDuration = f_dashFire *0.9f;
		SC_trailFireRef.SC_playerComboCounterRef = SC_playerComboCounterRef;
		SC_trailFireRef.SC_ScoreCounterRef = SC_ScoreCounterRef;

		yield return new WaitForSeconds(0.015f);
		if(b_dashFire) StartCoroutine(firetrailSpawner());
	}

	public void Missile()
	{
		for(int i = 0; i < _ThingsAround.Length; i++)
		{
			if(_ThingsAround[i].CompareTag("Ennemi"))
			{
				a_EnemiesAround.Add(_ThingsAround[i].gameObject);
			}
		}

		int i_MissilesToLaunch;

		if(f_numberOfMissile*2 > a_EnemiesAround.Count)
		{
			i_MissilesToLaunch = a_EnemiesAround.Count;
		}
		else
		{
			i_MissilesToLaunch = (int)f_numberOfMissile*2;
		}



		for(int i = 0; i < i_MissilesToLaunch; i++)
		{
			GameObject _tempM = Instantiate(g_MissilePrefab, transform.position, transform.rotation) as GameObject;
			_tempM.renderer.material.color = SC_PlayerRef.s_brightColor;
			if(a_EnemiesAround[i] != null) _tempM.GetComponent<SC_followingStars>()._Destination = a_EnemiesAround[i];

			_tempM.GetComponent<SC_followingStars>().SC_playerComboCounterRef = SC_playerComboCounterRef;
		}
	}


	public void Laser ()
	{
		Quaternion _QuaTemp;

		if(SC_PlayerRef._LookDir.x >= 0)
		{
			_QuaTemp = Quaternion.Euler(new Vector3( 0, 0, Vector3.Angle(SC_PlayerRef._LookDir, -Vector3.up)));
		}
		else
		{
			_QuaTemp = Quaternion.Euler(new Vector3( 0, 0, - Vector3.Angle(SC_PlayerRef._LookDir, -Vector3.up)));
		}


		for(int i = 0; i < (f_Laser * 2); i++)
		{
			GameObject _temp = Instantiate(_CaCLaser, transform.position,  _QuaTemp) as GameObject;			
			_temp.transform.GetChild(0).GetComponent<SC_LaserTrigger>().SC_playerComboCounterRef = SC_playerComboCounterRef;			
			_temp.transform.GetChild(0).GetComponent<SC_LaserTrigger>().f_Laser = f_Laser + 5;
			_temp.transform.GetChild(0).GetComponent<SpriteRenderer>().color = SC_PlayerRef.s_brightColor;
			_temp.transform.rotation = Quaternion.Euler(transform.rotation.x+_QuaTemp.eulerAngles.x, transform.rotation.y + _QuaTemp.eulerAngles.y, transform.rotation.z + _QuaTemp.eulerAngles.z+i*(360/(f_Laser * 2)));
		}


		b_IsLasering = false;

	}
	
	public void ODC ()
	{
		Collider[] a_ToRepulse = Physics.OverlapSphere(transform.position, 8f+f_ODC);
		Vector3 _DirRepulse;
		GameObject _tempAnimODC = Instantiate(_ODCanim, new Vector3(transform.position.x,transform.position.y,transform.position.z-0.01f) , Quaternion.identity) as GameObject;
		_tempAnimODC.transform.localScale = new Vector3(_tempAnimODC.transform.localScale.x+0.5f*f_ODC,_tempAnimODC.transform.localScale.y+0.5f*f_ODC,_tempAnimODC.transform.localScale.z+0.5f*f_ODC);
		_tempAnimODC.renderer.material.color = SC_PlayerRef.s_brightColor;
		SC_ODCanim SC_ODCanimRef = _tempAnimODC.GetComponent<SC_ODCanim>();
		SC_ODCanimRef.b_isMaster = true;
		SC_ODCanimRef.f_numberOfODCtoLaunch = f_ODC;
		SC_ODCanimRef.f_numberOfODCtoLaunchRef = f_ODC;

		for(int i = 0; i < a_ToRepulse.Length; i++)
		{
			if(a_ToRepulse[i].CompareTag("Ennemi"))
			{
				_DirRepulse = a_ToRepulse[i].transform.position - transform.position;
				a_ToRepulse[i].GetComponent<SC_Ennemi>().Push(_DirRepulse.normalized, f_ODC*10f, 1/f_ODC);
			}
		}

		b_IsODCing = false;
	}

	public void AddOrRemovePU (string s_BatName, int f_AddOrRemove, GameObject g_batiment)
	{
		if(s_BatName == "Laser")
		{
			f_Laser = f_Laser + (1 * Mathf.Sign(f_AddOrRemove));
		}

		if(s_BatName == "followingStars")
		{
			f_numberOfMissile = f_numberOfMissile + (1 * Mathf.Sign(f_AddOrRemove));
		}

		if(s_BatName == "trailFire")
		{
			f_dashFire = f_dashFire + (1 * Mathf.Sign(f_AddOrRemove));
		}

		if(s_BatName == "ODC")
		{
			f_ODC = f_ODC + (1 * Mathf.Sign(f_AddOrRemove));
		}


		if(f_AddOrRemove == 1)
		{
			l_batimentList.Add(g_batiment);
		}
		
		if(f_AddOrRemove == -1)
		{
			l_batimentList.Remove(g_batiment);
		}
	}

	public void activeVisualBatiment()
	{
		_PuAudioSource.PlayOneShot(_Sounds.GetSound("PuActive"));

		foreach(GameObject g_batimentInList in l_batimentList)
		{
			SC_batimentVie SC_batimentVieRef;
			SC_batimentVieRef = g_batimentInList.GetComponent<SC_batimentVie>();
			SC_batimentVieRef.activate();
		}
	}

	public void deactiveVisualBatiment()
	{
		_PuAudioSource.PlayOneShot(_Sounds.GetSound("PuInactive"));

		foreach(GameObject g_batimentInList in l_batimentList)
		{
			SC_batimentVie SC_batimentVieRef;
			SC_batimentVieRef = g_batimentInList.GetComponent<SC_batimentVie>();
			SC_batimentVieRef.deactivate();		
		}
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;			
		Gizmos.DrawWireSphere(transform.position, 12);
	}	
	
}
