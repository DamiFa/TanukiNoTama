using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_Ennemi : MonoBehaviour {

	private GameObject[] a_Players;

	public int i_LifeAmount = 1;
	public int i_ScoreValue = 10;

	public float f_explosionTime;
	private float f_exploderTimer;
	private bool b_exploding = false;

	public Transform _deathMarkPrefab;

	public GameObject _bloodSplat;
	public GameObject _ExplosionSplat;
	public GameObject _PUabsorbParticle;

	public AnimationCurve AC_PushStrengh;
	private float f_PushDuration;

	private float f_pushStrenght;
	private Vector3 v_HitPushDir;

	private bool b_isActive = true;	

	private SC_ScoreCounter SC_ScoreCounterRef;
	private SC_EnemyBehavior SC_EnemyBehaviorRef;
	private SC_Damage SC_DamageRef;

	public GameObject g_scoreValueGUI;
	private float f_angle;
	private GameObject _scoreValueGUIdisplay;


	private Vector3 _LookDir;
	private float f_AngleDir;
	private int i_CurrentLookDir;
	private string s_EnemyState;
	private int i_LookingDir;
	private Animator _EnemyAnim;
	private string s_CurrentAnim;


	private GameObject _deathMarkRoot;


	private SC_SoundManager _Sounds;
	private AudioSource _AudioSource_Ennemi;

	public GameObject[] g_walls;

	
	void Start () 
	{
		g_walls = GameObject.FindGameObjectsWithTag("Wall");

		_AudioSource_Ennemi = gameObject.AddComponent<AudioSource>();
		_Sounds = GameObject.FindGameObjectWithTag("SOUND_MANAGER").GetComponent<SC_SoundManager>();



		_deathMarkRoot = GameObject.Find("[deathMarkRoot]").gameObject;
		_EnemyAnim = GetComponent<Animator>();

		SC_DamageRef = this.GetComponent<SC_Damage>();
		SC_EnemyBehaviorRef = this.GetComponent<SC_EnemyBehavior>();
		SC_ScoreCounterRef = GameObject.Find("guiSCOREcounter").GetComponent<SC_ScoreCounter>();			
		this.tag = null;

		s_EnemyState = "Walk";
//		AnimationHandeler("Walk");

		f_PushDuration = 0;

		transform.FindChild("corps").GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
	}	


	void Update () 
	{

		if(transform.position.x < g_walls[3].transform.position.x && transform.position.x > g_walls[1].transform.position.x && transform.position.y > g_walls[0].transform.position.y && transform.position.y < g_walls[2].transform.position.y)
		{
		   this.tag = "Ennemi";
		}else
		{
			this.tag = null;
		}

		if(b_exploding==true)
		{	
			f_exploderTimer += Time.deltaTime;
			
			collider.transform.localScale = Vector3.Lerp(collider.transform.localScale, new Vector3(4, 4, 4), 3*Time.deltaTime);

			if(f_exploderTimer >= 0.2)
			{
				rigidbody.velocity = Vector3.zero;
			}

			if(f_exploderTimer >= f_explosionTime)
			{
				b_exploding = false;
				StartCoroutine(deathFlash());
			}
		}

		//Set _LookDir
		_LookDir =  SC_EnemyBehaviorRef._Destination - transform.position;

		//Set f_AngleDir
		if(_LookDir.x >= 0)
		{
			f_AngleDir = Vector3.Angle(_LookDir, -Vector3.up);
		}
		else
		{
			f_AngleDir =(360 - Vector3.Angle(_LookDir, -Vector3.up));
		}

		//Set i_CurrentLookDir
		if(f_AngleDir < 22.5f || f_AngleDir >= 337.5f)
		{
			i_CurrentLookDir = 2;
		}
		else if(f_AngleDir < 337.5f && f_AngleDir >= 292.5f)
		{
			i_CurrentLookDir = 1;
		}
		else if(f_AngleDir < 292.5f && f_AngleDir >= 247.5f)
		{
			i_CurrentLookDir = 4;
		}
		else if(f_AngleDir < 247.5f && f_AngleDir >= 202.5f)
		{
			i_CurrentLookDir = 7;
		}
		else if(f_AngleDir < 202.5f && f_AngleDir >= 157.5f)
		{
			i_CurrentLookDir = 8;
		}
		else if(f_AngleDir < 157.5f && f_AngleDir >= 112.5f)
		{
			i_CurrentLookDir = 9;
		}
		else if(f_AngleDir < 112.5f && f_AngleDir >= 67.5f)
		{
			i_CurrentLookDir = 6;
		}
		else if(f_AngleDir < 67.5f && f_AngleDir >= 22.5f)
		{
			i_CurrentLookDir = 3;
		}
	}

	void LateUpdate ()
	{
		AnimationHandeler(s_EnemyState);
	}

	public void death(Vector3 v_HitPushDirRef, float f_pushStrenghtRef, int i_TypeOfDeath, int i_dashMultiplicator, SC_Player SC_PlayerRef)
	{
		//NOTE to get v_HitPushDirRef, use SC_Trigger.getHitDir()
		// to get strenghtRef use SC_Player.f_PushStrenght


		v_HitPushDirRef = v_HitPushDirRef.normalized;
		if(v_HitPushDirRef != Vector3.zero)
		{
			if(v_HitPushDirRef.x >= 0)
			{
				f_angle = (180-Vector3.Angle(new Vector3(0,1,0), new Vector3(v_HitPushDirRef.x, v_HitPushDirRef.y,0)))+180;
			}
			else
			{
				f_angle = Vector3.Angle(new Vector3(0,1,0), new Vector3(v_HitPushDirRef.x, v_HitPushDirRef.y,0));
			}
		}
		if(i_TypeOfDeath == 1)
		{
			GameObject _deathBlood = Instantiate(_bloodSplat, new Vector3(transform.position.x,  transform.position.y, transform.position.z+0.05f), Quaternion.Euler(new Vector3(270,0,0))) as GameObject;
			_deathBlood.transform.eulerAngles = new Vector3(0,0,f_angle);
			_deathBlood.transform.parent = this.transform;

			_deathBlood.transform.GetChild(0).particleSystem.emissionRate += i_dashMultiplicator*100;
			//_deathBlood.transform.GetChild(0).particleSystem.startLifetime += Mathf.Clamp(i_dashMultiplicator/5,0,1);
			_deathBlood.transform.GetChild(0).particleSystem.startSpeed += i_dashMultiplicator/10;
			_deathBlood.transform.GetChild(0).particleSystem.startLifetime += Mathf.Clamp(i_dashMultiplicator/5,0,1);
			_deathBlood.transform.GetChild(0).particleSystem.startColor = this.GetComponent<SpriteRenderer>().color;




//			if(SC_PlayerRef.b_IsInPuMode != true && SC_PlayerRef.b_IsDashing)
//			{
//				GameObject _PUabsorbTEMP = Instantiate(_PUabsorbParticle, new Vector3(transform.position.x,  transform.position.y, transform.position.z+0.05f), Quaternion.Euler(new Vector3(270,0,0))) as GameObject;
//				_PUabsorbTEMP.transform.eulerAngles = new Vector3(0,0,-f_angle);
//				//_PUabsorbTEMP.transform.parent = this.transform;
//				_PUabsorbTEMP.transform.GetChild(0).particleSystem.startColor = this.GetComponent<SpriteRenderer>().color;
//
//				SC_particleSetRotation SC_ParticleSetRotationRef = _PUabsorbTEMP.GetComponent<SC_particleSetRotation>();
//				SC_ParticleSetRotationRef.t_destination = SC_PlayerRef.t_ParticleAbsorbDestination;
//				SC_ParticleSetRotationRef.c_PlayerColor = SC_PlayerRef.s_brightColor;
//				//SC_ParticleSetRotationRef.f_speed = SC_PlayerRef.f_speedref*3f;
//
//				//Debug.Log("ABSORB");
//			}


		}else
		{
			GameObject _deathBloodExplo = Instantiate(_ExplosionSplat, new Vector3(transform.position.x,  transform.position.y, transform.position.z+0.05f), Quaternion.Euler(new Vector3(270,0,0))) as GameObject;
			_deathBloodExplo.transform.eulerAngles = new Vector3(0,0,f_angle);
			_deathBloodExplo.transform.parent = this.transform;
			_deathBloodExplo.renderer.material.color = SC_PlayerRef.s_brightColor;
		}

		GameObject g_scoreValueGUITEMP = Instantiate(g_scoreValueGUI, new Vector3(0,  0.03f, 0), Quaternion.Euler(0,0,0)) as GameObject;

		Color c_guiColor = SC_PlayerRef.s_Color;
		c_guiColor.a = 0.5f;
		g_scoreValueGUITEMP.guiText.color = c_guiColor;

		SC_EnnemiScoreGUI SC_EnnemiScoreGUIRef = g_scoreValueGUITEMP.GetComponent<SC_EnnemiScoreGUI>();
		SC_EnnemiScoreGUIRef.i_scoreValue = Mathf.Clamp(i_dashMultiplicator,1,999)*i_ScoreValue;
		SC_EnnemiScoreGUIRef.g_ennemi = this.transform;

		_scoreValueGUIdisplay = g_scoreValueGUITEMP;



		v_HitPushDir = v_HitPushDirRef;
		f_pushStrenght = f_pushStrenghtRef;
		
		this.collider.enabled = false;
		SC_EnemyBehaviorRef.enabled = false;
		StartCoroutine(IsPushed (v_HitPushDirRef, f_pushStrenghtRef, 0.5f));

		b_isActive = false;

		if(SC_EnemyBehaviorRef.i_EnemyType == 1)
		{
			b_exploding = true;		
			SC_DamageRef.DamageDealth = 1200;
			SC_EnemyBehaviorRef.enabled = false;
		}
		else
		{
			//StartCoroutine(deathFlash());
		}

		if(i_TypeOfDeath == 0)
		{
			//mort par PU
			AnimationHandeler("DeathPU");
			_AudioSource_Ennemi.PlayOneShot(_Sounds.GetSound("MortParPu"));
		}
		else
		{
			AnimationHandeler("DeathCaC");
		}
	}

	IEnumerator deathFlash()
	{	
		b_isActive = false;

		yield return new WaitForSeconds(0.5f);
		
		Destroy(_scoreValueGUIdisplay);


		yield return new WaitForSeconds(1.5f);



//		Transform t_deathMark = Instantiate(_deathMarkPrefab, new Vector3(transform.position.x,  transform.position.y, -0.06f), Quaternion.identity) as Transform;
//		t_deathMark.renderer.material.color = this.GetComponent<SpriteRenderer>().color;
//		t_deathMark.parent = _deathMarkRoot.transform;
//		StaticBatchingUtility.Combine(_deathMarkRoot);

		Destroy(this.gameObject);

		yield break;
	}

	public void deathDestroy()
	{			
		Destroy(_scoreValueGUIdisplay);		
		Destroy(this.gameObject);
	}



	public void Push (Vector3 v_HitPushDirRef, float f_pushStrenghtRef, float f_Duration)
	{
		StartCoroutine(IsPushed (v_HitPushDirRef, f_pushStrenghtRef, f_Duration));
	}

	IEnumerator IsPushed (Vector3 v_HitPushDirRef, float f_pushStrenghtRef, float f_Duration)
	{
		while(f_PushDuration <= 1)
		{
			rigidbody.velocity = v_HitPushDirRef*f_pushStrenghtRef*AC_PushStrengh.Evaluate(f_PushDuration);

			f_PushDuration += (1/f_Duration) * Time.deltaTime;

			yield return new WaitForEndOfFrame();
		}

		rigidbody.velocity = Vector3.zero;
		f_PushDuration = 0;
		yield break;
	}

	public void AnimationHandeler(string s_NewAnim)
	{
		//gérer les changements de direction
		//doit etre dans le fixed update pour ne pas changer lui meme les states

		//(s_CurrentAnim);
		
		if(s_NewAnim == "DeathPU")
		{
			s_EnemyState = "DeathPU";
			_EnemyAnim.Play("pu");
		}
		else if(s_NewAnim == "DeathCaC")
		{
			s_EnemyState = "DeathCaC";
			_EnemyAnim.Play("cac");
		}
		else if(i_CurrentLookDir != i_LookingDir || s_NewAnim != s_EnemyState)
		{
			if(s_NewAnim != s_EnemyState)
			{
				if(s_NewAnim == "Walk")
				{
					s_CurrentAnim = "walk_" + i_CurrentLookDir;
					s_EnemyState = "Walk";
				}
				
				_EnemyAnim.Play(s_CurrentAnim);
			}
			else if(i_CurrentLookDir != i_LookingDir)
			{
				if(s_EnemyState == "Walk")
				{
					_EnemyAnim.Play("walk_" + i_CurrentLookDir);
					s_CurrentAnim = "walk_" + i_CurrentLookDir;
				}
				
				i_LookingDir = i_CurrentLookDir;
			}
		}
	}
}
