using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent(typeof (Rigidbody))]

public class SC_Player : MonoBehaviour {

	//Animations
	private Animator _PlayerAnims;
	private int i_LookingDir;
	private string s_PlayerState;
	private float f_CurrentAnimTime;
	private string s_CurrentAnim;
	private int i_CurrentLookDir;
	public float f_AttackDuration;
	private float f_AngleDir;
	private float f_AttackTime;
	public AnimationCurve f_GainPerKill;

	public enum e_Player {Player1 = 1, Player2 = 2, Player3 = 3, Player4 = 4};
	public e_Player playerIndex;
	public Color s_Color;
	public Color s_brightColor;

	//[HideInInspector]
	private bool b_HasTouchedForDash;
	public bool b_HasTouchedForPU;

	public AnimationCurve _DashSpeed;
	private float f_DashDuration;
	private float f_InitialDistance;
	private float f_curBoost;

	//Variable pour accelaration / speed
	public float f_MaxSpeed;
	public float f_Accel;

	//Variable P-U
	public float f_DistanceProjectile;
	public float f_SpeedProjectile;
	public float f_DashStrengh;
	public float f_MvtSpeed;
	public int i_DashLimit;
	public float f_TrailfadeOutSpeed;
	public float f_ZoneDash;
	public float f_PushStrenght;

	//P-U
	[HideInInspector]
	public bool b_DashKill;

	[HideInInspector]
	public Vector3 _LookDir;
	public bool b_IsDashing;
	private bool b_CanDash;
	[HideInInspector]
	public GameObject _TriggerCaC;
	public GameObject _TriggerCaCChild;
	public bool b_IsCaCing;
	//	public GameObject _Projectile;
	GameObject _Viseur;
	private GameObject _Cible;
	private Collider[] a_CloseEnemies;
	private bool b_IsReallyDashing;
	private Animator _Animator;
	public float f_StepAttack;
	
	private Vector3 v_StartPos;

	//Variable pour trail
	SC_playerCombo SC_playerComboRef;
	SC_dashTrail SC_dashTrailRef;
	SC_playerDeath SC_playerDeathRef;
	SC_closeToEnnemiFB SC_closeToEnnemiFBRef;

	[HideInInspector]
	public SC_ScoreCounter SC_ScoreCounterRef;

	public SC_playerComboCounter SC_playerComboCounterRef;

	//rumble
//	bool b_isRumbling = false;
//	float f_rumbleTimer;
//	float f_rumbleTimerLimit;

	//ModePU
	public bool b_IsInPuMode;

	//Audio
	[HideInInspector]
	public SC_SoundManager _Sounds;
	public AudioSource _AttackAudioSource;
	public AudioSource _StateAudioSource;
	public AudioSource _DashPUModeAudioSource;
	private AudioClip _Clip_StateAudioSource;
	private string _AudioToPlay_State;

	// Use this for initialization
	private SC_mainLight SC_mainLightRef;

	public ParticleSystem p_particlePuff;
	public ParticleSystem p_particleGrass;
	public ParticleSystem p_PUmode;
	public Transform t_ParticleAbsorbDestination;
	[HideInInspector]
	public float f_speedref;


	//xINPUT
	public GamePadState _PreviousControllerState;
	public GamePadState _ControllerState;
	private PlayerIndex _PlayerIndex;

	//rumble
	private bool b_isRumbling = false;
	private float f_rumbleTimer;
	private float f_rumbleTimerLimit = 0.1f;

	void Start () 
	{	

		_PlayerIndex = (PlayerIndex)(int)playerIndex-1;
		_ControllerState = GamePad.GetState(_PlayerIndex);

		_PlayerAnims = GetComponent<Animator>();
		s_PlayerState = "Idle";
		s_CurrentAnim = "idle_1";
		i_LookingDir = 1;
		i_CurrentLookDir = 1;
		f_AttackTime = 0;
		b_IsInPuMode = false;

		_Sounds = GameObject.FindGameObjectWithTag("SOUND_MANAGER").GetComponent<SC_SoundManager>();

		SC_mainLightRef = GameObject.Find("flashLight_root").GetComponent<SC_mainLight>();
		SC_ScoreCounterRef = GameObject.FindGameObjectWithTag("scoreCounter").GetComponent<SC_ScoreCounter>();
		_TriggerCaC = this.transform.FindChild("TriggerCac").gameObject;
		SC_playerComboCounterRef = GameObject.Find("scoreCounterGUI"+(int)playerIndex).GetComponent<SC_playerComboCounter>();
		SC_playerComboRef = this.GetComponent<SC_playerCombo>();
		SC_playerDeathRef = this.GetComponent<SC_playerDeath>();
		SC_closeToEnnemiFBRef = this.GetComponent<SC_closeToEnnemiFB>();
		SC_dashTrailRef = GetComponent<SC_dashTrail>();

		_Viseur = GameObject.Find("Viseur"+(int)playerIndex).gameObject;
		_Viseur.renderer.material.color = s_Color;

		b_HasTouchedForDash = false;

		b_DashKill = false;

		Physics.IgnoreLayerCollision(8,8);
	
		this.tag = "Player";

		b_IsCaCing = false;
		b_IsDashing = false;
		b_CanDash = true;
		CaCOFF();

		p_PUmode.startColor = s_brightColor;

		_TriggerCaCChild.SetActive(false);

		stopRumble();
	}
	
	// Update is called once per frame
	void Update () 
	{
		_PreviousControllerState = _ControllerState;
		_ControllerState = GamePad.GetState(_PlayerIndex);

		if(b_isRumbling)
		{
			f_rumbleTimer += Time.deltaTime;
			if(f_rumbleTimer >= f_rumbleTimerLimit)
			{
				stopRumble();
			}
		}

		///PU MODE RAPH's STUFF
		if(b_IsInPuMode != true && p_PUmode.enableEmission == true)
		{
			p_PUmode.enableEmission = false;
		}

		if(!b_IsInPuMode && this.GetComponent<SpriteRenderer>().color != Color.white) this.GetComponent<SpriteRenderer>().color = Color.white;
		///////////////////////////////////////////////

		if(!b_IsDashing)
		{
			Move ();
		}

		Dash ();

		if(b_DashKill)
		{
			CaCON();
		}
		else
		{
			if (_ControllerState.Buttons.X == ButtonState.Pressed && _PreviousControllerState.Buttons.X == ButtonState.Released)
			{
				CaC ();
			}
		}

		if(_LookDir != Vector3.zero)
		{
			if(_LookDir.x >= 0)
			{
				f_AngleDir = Vector3.Angle(_LookDir, -Vector3.up);
			}
			else
			{
				f_AngleDir =(360 -Vector3.Angle(_LookDir, -Vector3.up));
			}
		}

		if(b_IsCaCing)
		{
			rigidbody.velocity = new Vector3(rigidbody.velocity.x + _LookDir.normalized.x*f_StepAttack,
			                                 rigidbody.velocity.y + _LookDir.normalized.y*f_StepAttack,
			                                 0);

			if(f_AttackTime < f_AttackDuration)
			{
				f_AttackTime += Time.deltaTime;
				CaCON();
//				if(f_AttackTime > 3)
//				{
//					CaCON();
//				}
			}
			else
			{
				CaCOFF();
				f_AttackTime = 0;
				AnimationHandeler("Idle");
			}
		}
		else
		{
			if(f_AngleDir < 22.5f || f_AngleDir >= 337.5f)
			{
				i_CurrentLookDir = 2;
				_TriggerCaC.transform.eulerAngles = new Vector3(0, 0, 0);
			}
			else if(f_AngleDir < 337.5f && f_AngleDir >= 292.5f)
			{
				i_CurrentLookDir = 1;
				_TriggerCaC.transform.eulerAngles = new Vector3(0, 0, 315);
			}
			else if(f_AngleDir < 292.5f && f_AngleDir >= 247.5f)
			{
				i_CurrentLookDir = 4;
				_TriggerCaC.transform.eulerAngles = new Vector3(0, 0, 270);
			}
			else if(f_AngleDir < 247.5f && f_AngleDir >= 202.5f)
			{
				i_CurrentLookDir = 7;
				_TriggerCaC.transform.eulerAngles = new Vector3(0, 0, 225);
			}
			else if(f_AngleDir < 202.5f && f_AngleDir >= 157.5f)
			{
				i_CurrentLookDir = 8;
				_TriggerCaC.transform.eulerAngles = new Vector3(0, 0, 180);
			}
			else if(f_AngleDir < 157.5f && f_AngleDir >= 112.5f)
			{
				i_CurrentLookDir = 9;
				_TriggerCaC.transform.eulerAngles = new Vector3(0, 0, 135);
			}
			else if(f_AngleDir < 112.5f && f_AngleDir >= 67.5f)
			{
				i_CurrentLookDir = 6;
				_TriggerCaC.transform.eulerAngles = new Vector3(0, 0, 90);
			}
			else if(f_AngleDir < 67.5f && f_AngleDir >= 22.5f)
			{
				i_CurrentLookDir = 3;
				_TriggerCaC.transform.eulerAngles = new Vector3(0, 0, 45);
			}
		}



		if((Mathf.Abs(_ControllerState.ThumbSticks.Left.X) > 0.2f) || (Mathf.Abs(_ControllerState.ThumbSticks.Left.Y) > 0.2f))
		{
			_LookDir = new Vector3(_ControllerState.ThumbSticks.Left.X, _ControllerState.ThumbSticks.Left.Y, 0.0f);
		}
	}

	void LateUpdate()
	{
		if(!b_IsCaCing)
		{
			AnimationHandeler(s_PlayerState);
		}
	}

	private void Move ()
	{
		if((Mathf.Abs(_ControllerState.ThumbSticks.Left.X) > 0.2f) || (Mathf.Abs(_ControllerState.ThumbSticks.Left.Y) > 0.2f))
		{
			if(f_MvtSpeed<f_MaxSpeed)f_MvtSpeed += f_Accel*Time.deltaTime;
			if(f_MvtSpeed>f_MaxSpeed) f_MvtSpeed = f_MaxSpeed;


			rigidbody.velocity = new Vector3(_ControllerState.ThumbSticks.Left.X *  f_MvtSpeed, 
			                                 _ControllerState.ThumbSticks.Left.Y * f_MvtSpeed,
			                                 0.0f);

			if(!b_IsCaCing && !b_IsDashing)
			{
				AnimationHandeler("Walk");

				//Audio Marche
				if(_AudioToPlay_State != "Walk" || !_StateAudioSource.isPlaying)
				{
					_AudioToPlay_State = "Walk";
					_Clip_StateAudioSource = _Sounds.GetSound(_AudioToPlay_State);
					_StateAudioSource.clip = _Clip_StateAudioSource;
					_StateAudioSource.Play();
				}
			}
		}
		else
		{
			rigidbody.velocity = Vector3.zero;
			f_MvtSpeed = 0;

			if(!b_IsCaCing && !b_IsDashing)
			{
				AnimationHandeler("Idle");
				_StateAudioSource.Stop();
			}
		}
	}

	private void Dash ()
	{
		if(_ControllerState.Buttons.A == ButtonState.Pressed && _PreviousControllerState.Buttons.A == ButtonState.Released)
		{
			if(b_CanDash && !b_IsInPuMode)
			{
				_Viseur.SetActive(true);
				b_IsDashing = true;
				b_CanDash = false;
				rigidbody.velocity = Vector3.zero;

				ParticleSystem t_particlePuff = Instantiate(p_particlePuff, transform.position,  Quaternion.Euler(new Vector3(270,0,0))) as ParticleSystem;	
				t_particlePuff.transform.parent = this.transform;

				ParticleSystem t_particleGrass = Instantiate(p_particleGrass, transform.position, Quaternion.Euler(new Vector3(270,0,0))) as ParticleSystem;
				t_particlePuff.transform.parent = this.transform;
				_DashPUModeAudioSource.PlayOneShot(_Sounds.GetSound("DashStart"));
			}
			else if(!b_IsInPuMode && SC_playerComboCounterRef.i_killCount > 0)
			{
//				b_IsInPuMode = true;
//				p_PUmode.enableEmission = true;
//				SC_playerComboCounterRef.Set_inPuMode(b_IsInPuMode);
//
//				SC_mainLightRef.addPlayerInPU(this.GetComponent<SC_Player>());
//				this.GetComponent<SpriteRenderer>().color = s_brightColor;
//			
//				_DashPUModeAudioSource.PlayOneShot(_Sounds.GetSound("PuModeStart"));
			}
			else if(b_IsInPuMode)
			{
//				b_IsInPuMode = false;
//				p_PUmode.enableEmission = false;
//
//				SC_playerComboCounterRef.Set_inPuMode(b_IsInPuMode);
//
//				SC_mainLightRef.stopFlash(this.GetComponent<SC_Player>());
//			
//
//				this.GetComponent<SpriteRenderer>().color = Color.white;
//
//				_DashPUModeAudioSource.PlayOneShot(_Sounds.GetSound("PuModeEnd"));
			}
		}

		if(_ControllerState.Triggers.Right >= 0.5f && _PreviousControllerState.Triggers.Right < 0.5f && SC_playerComboCounterRef.i_killCount > 0) 
		{
			b_IsInPuMode = true;
			p_PUmode.enableEmission = true;
			SC_dashTrailRef.deactivateTrail();
			SC_playerComboCounterRef.Set_inPuMode(b_IsInPuMode);
			
			SC_mainLightRef.addPlayerInPU(this.GetComponent<SC_Player>());
			this.GetComponent<SpriteRenderer>().color = s_brightColor;
			
			_DashPUModeAudioSource.PlayOneShot(_Sounds.GetSound("PuModeStart"));
		}else if(b_IsInPuMode && _ControllerState.Triggers.Right < 0.5f && _PreviousControllerState.Triggers.Right >= 0.5f)
		{
			b_IsInPuMode = false;
			p_PUmode.enableEmission = false;
			
			SC_playerComboCounterRef.Set_inPuMode(b_IsInPuMode);
			
			SC_mainLightRef.stopFlash(this.GetComponent<SC_Player>());
			
			
			this.GetComponent<SpriteRenderer>().color = Color.white;
			
			_DashPUModeAudioSource.PlayOneShot(_Sounds.GetSound("PuModeEnd"));
		}


		
		if(b_IsDashing)
		{
			if(FindClosestEnemy() != null)
			{
				_Cible = FindClosestEnemy();
				Vector3 v3 = _Cible.transform.position - transform.position;
				_Viseur.transform.position = new Vector3 (_Cible.transform.position.x, _Cible.transform.position.y-0.8f, _Viseur.transform.position.z) ;
				
				if(_ControllerState.Buttons.A == ButtonState.Pressed && _PreviousControllerState.Buttons.A == ButtonState.Released)
				{
					b_IsReallyDashing = true;
					f_InitialDistance = Vector3.Distance(_Cible.transform.position, transform.position);
				}
				
				if(b_IsReallyDashing)
				{
					SC_dashTrailRef.activateTrail(f_TrailfadeOutSpeed);
					
					f_DashDuration = 1 - (Vector3.Distance(_Cible.transform.position, transform.position) / f_InitialDistance);
					
					_LookDir = v3;



					rigidbody.velocity = v3.normalized * _DashSpeed.Evaluate(f_DashDuration) * f_DashStrengh * ((1+ ( 5* f_GainPerKill.Evaluate((float)SC_playerComboCounterRef.i_killCount/30))));
					f_speedref = _DashSpeed.Evaluate(f_DashDuration) * f_DashStrengh * ((1+ ( 5* f_GainPerKill.Evaluate((float)SC_playerComboCounterRef.i_killCount/30))));

					//Feedback ennemy zone attack
					SC_closeToEnnemiFBRef.inEnnemyZone();

					//Audio Dash
					if(_AudioToPlay_State != "Dash")
					{
						_AudioToPlay_State = "Dash";
						_Clip_StateAudioSource = _Sounds.GetSound(_AudioToPlay_State);
						_StateAudioSource.clip = _Clip_StateAudioSource;
						_StateAudioSource.Play();
					}

					if(Vector3.Distance(_Cible.transform.position, transform.position) < 0.45f)
					{
						DashEnd ();
					}

					if(!b_IsCaCing)
					{
						AnimationHandeler("Dash");
					}
				}
			}
			else
			{
				DashEnd ();
			}
		}
	}

	private void CaC ()
	{
		if(!b_IsCaCing)
		{
			AnimationHandeler("Attaque");
			b_IsCaCing = true;
			_AttackAudioSource.PlayOneShot(_Sounds.GetSound("Attack"));
		}
	}

	public void CaCON()
	{
		b_IsCaCing = true;

		_TriggerCaCChild.SetActive(true);
		//_TriggerCaC.SetActive(true);
	}

	public void CaCOFF()
	{
		b_IsCaCing = false;
		//_TriggerCaC.SetActive(false);
		_TriggerCaCChild.SetActive(false);
		if(b_IsDashing)
		{
			if(!b_HasTouchedForDash && !b_IsInPuMode)
			{
				DashEnd ();
			}
			else
			{
				b_HasTouchedForDash = false;
			}
		}
	}

	public void CaCHasTouched (GameObject _touchedEnnemy)
	{
		_AttackAudioSource.PlayOneShot(_Sounds.GetSound("HasTouched"));

		if(b_isRumbling == false)
		{
			rumble (0.2f);
		}
		b_HasTouchedForDash = true;
	
		if(b_IsReallyDashing == true)
		{
			//TEST AUDIO//
			_AttackAudioSource.pitch = 1 + (SC_playerComboCounterRef.i_killCount*0.01f);
			/////////////

			b_HasTouchedForPU = true;	
			SC_playerComboCounterRef.addKill(_touchedEnnemy.GetComponent<SC_Ennemi>().i_ScoreValue, true);
		}else
		{
			SC_playerComboCounterRef.addKill(_touchedEnnemy.GetComponent<SC_Ennemi>().i_ScoreValue, false);
		}

		// SERT POUR LA COULEUR DE L'ANNEAU AU MILIEU
		SC_ScoreCounterRef.IncreaseScoreForPlayer((int)playerIndex);

	}

	public void resetKillCount(){
		SC_ScoreCounterRef.ResetScoreForPlayer((int)playerIndex);
	}

	public void StopMvt(){
		rigidbody.velocity = Vector3.zero;
	}

	private GameObject FindClosestEnemy ()
	{
		a_CloseEnemies = Physics.OverlapSphere(transform.position, f_ZoneDash);

		Vector3 _V3Temp;
		Vector3 _V3Temp2;

		GameObject _ClosestEnemy = null;

		for(int i = 1; i < a_CloseEnemies.Length; i++)
		{
			if(a_CloseEnemies[i].gameObject.CompareTag("Ennemi"))
			{
				if(_ClosestEnemy == null)
				{
					_ClosestEnemy = a_CloseEnemies[i].gameObject;
				}
				else
				{
					_V3Temp = transform.position - a_CloseEnemies[i].transform.position;
					_V3Temp2 = transform.position - _ClosestEnemy.transform.position;

					if(Vector3.Angle(-_LookDir, _V3Temp) < Vector3.Angle(-_LookDir, _V3Temp2))
					{
						_ClosestEnemy = a_CloseEnemies[i].gameObject;
					}
				}
			}
		}

		return _ClosestEnemy;
	}

	public void DashEnd ()
	{
		//TEST AUDIO//
		_AttackAudioSource.pitch = 1;
		/////////////

		SC_playerComboCounterRef.endCount();

		//SC_playerComboRef.resetKillCount();

		SC_closeToEnnemiFBRef.noFB();
		SC_closeToEnnemiFBRef.b_inEnnemyZone = false;	

		_Viseur.SetActive(false);

		SC_dashTrailRef.deactivateTrail();
		
		removeComboBoost(f_curBoost);
		f_curBoost = 0;

		b_IsReallyDashing = false;
		b_IsDashing = false;
		b_CanDash = true;
		b_IsInPuMode = false;
		SC_playerComboCounterRef.Set_inPuMode(b_IsInPuMode);

		rigidbody.velocity = Vector3.zero;
		AnimationHandeler("Idle");	
	}


	public Color GetSpriteColor(){
		return s_Color;
	}

	public float GetPushStrenght(){
		return f_PushStrenght;
	}

//	public void comboBoost(float _Boost)
//	{
//		f_curBoost = _Boost;
//		f_DashStrengh = f_DashStrengh +_Boost;
//		f_PushStrenght = f_PushStrenght +_Boost;
//	}

	public void removeComboBoost(float _BoostToRemove)
	{
		f_DashStrengh = f_DashStrengh- _BoostToRemove;
		f_PushStrenght = f_PushStrenght- _BoostToRemove;
	}

	public void rumble(float f_rumbleTime)
	{
		f_rumbleTimerLimit = f_rumbleTime;

		if(!b_IsDashing && !b_IsInPuMode)
		{
			f_rumbleTimerLimit = 0.15f;
			GamePad.SetVibration(_PlayerIndex,0.3f,0.3f);
		}
		if(b_IsDashing && !b_IsInPuMode)
		{
			f_rumbleTimerLimit = 0.2f;
			GamePad.SetVibration(_PlayerIndex,0.7f,0.7f);
		}
		if(b_IsDashing && b_IsInPuMode)
		{
			f_rumbleTimerLimit = 0.25f;
			GamePad.SetVibration(_PlayerIndex,1f,1f);
		}
		b_isRumbling = true;
	}

	public void stopRumble()
	{

		GamePad.SetVibration(_PlayerIndex,0f,0f);
		f_rumbleTimer = 0;
		b_isRumbling = false;

	}

	public void AnimationHandeler(string s_NewAnim)
	{
		//gérer les changements de direction
		//doit etre dans le fixed update pour ne pas changer lui meme les states

//		f_CurrentAnimTime = _PlayerAnims[s_CurrentAnim].time;

		if(s_NewAnim == "Death")
		{
			//_PlayerAnims.Play("death");
		}
		if(i_CurrentLookDir == 1)
		{
			if(s_NewAnim != s_PlayerState)
			{
				if(s_NewAnim == "Idle")
				{
					//Play animation d'idle
					//changer s_PlayerState
					//changer la variable current animation
					s_CurrentAnim = "idle_1";
					s_PlayerState = "Idle";
				}
				else if(s_NewAnim == "Dash")
				{
					s_CurrentAnim = "dash_1";
					s_PlayerState = "Dash";
				}
				else if(s_NewAnim == "Attaque")
				{
					s_CurrentAnim = "attack_1";
					s_PlayerState = "Attaque";
				}
				else if(s_NewAnim == "Walk")
				{
					s_CurrentAnim = "walk_1";
					s_PlayerState = "Walk";
				}
				
				_PlayerAnims.Play(s_CurrentAnim);
			}
			else if(i_CurrentLookDir != i_LookingDir)
			{
				if(s_PlayerState == "Idle")
				{
					_PlayerAnims.Play("idle_1");
//					animation["idle_1"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Dash")
				{
					_PlayerAnims.Play("dash_1");
//					animation["dash_1"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Attaque")
				{
					_PlayerAnims.Play("attack_1");
//					animation["attack_1"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Walk")
				{
					_PlayerAnims.Play("walk_1");
//					animation["walk_1"].time = f_CurrentAnimTime;
				}
				i_LookingDir = 1;
			}
		}
		else if(i_CurrentLookDir == 2)
		{
			if(s_NewAnim != s_PlayerState)
			{
				if(s_NewAnim == "Idle")
				{
					//Play animation d'idle
					//changer s_PlayerState
					//changer la variable current animation
					s_CurrentAnim = "idle_2";
					s_PlayerState = "Idle";
				}
				else if(s_NewAnim == "Dash")
				{
					s_CurrentAnim = "dash_2";
					s_PlayerState = "Dash";
				}
				else if(s_NewAnim == "Attaque")
				{
					s_CurrentAnim = "attack_2";
					s_PlayerState = "Attaque";
				}
				else if(s_NewAnim == "Walk")
				{
					s_CurrentAnim = "walk_2";
					s_PlayerState = "Walk";
				}
				
				_PlayerAnims.Play(s_CurrentAnim);
			}
			else if(i_CurrentLookDir != i_LookingDir)
			{
				if(s_PlayerState == "Idle")
				{
					_PlayerAnims.Play("idle_2");
//					animation["idle_2"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Dash")
				{
					_PlayerAnims.Play("dash_2");
//					animation["dash_2"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Attaque")
				{
					_PlayerAnims.Play("attack_2");
//					animation["attack_2"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Walk")
				{
					_PlayerAnims.Play("walk_2");
//					animation["walk_2"].time = f_CurrentAnimTime;
				}
				i_LookingDir = 2;
			}
		}
		else if(i_CurrentLookDir == 3)
		{
			if(s_NewAnim != s_PlayerState)
			{
				if(s_NewAnim == "Idle")
				{
					//Play animation d'idle
					//changer s_PlayerState
					//changer la variable current animation
					s_CurrentAnim = "idle_3";
					s_PlayerState = "Idle";
				}
				else if(s_NewAnim == "Dash")
				{
					s_CurrentAnim = "dash_3";
					s_PlayerState = "Dash";
				}
				else if(s_NewAnim == "Attaque")
				{
					s_CurrentAnim = "attack_3";
					s_PlayerState = "Attaque";
				}
				else if(s_NewAnim == "Walk")
				{
					s_CurrentAnim = "walk_3";
					s_PlayerState = "Walk";
				}
				
				_PlayerAnims.Play(s_CurrentAnim);
			}
			else if(i_CurrentLookDir != i_LookingDir)
			{
				if(s_PlayerState == "Idle")
				{
					_PlayerAnims.Play("idle_3");
//					animation["idle_3"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Dash")
				{
					_PlayerAnims.Play("dash_3");
//					animation["dash_3"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Attaque")
				{
					_PlayerAnims.Play("attack_3");
//					animation["attack_3"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Walk")
				{
					_PlayerAnims.Play("walk_3");
//					animation["walk_3"].time = f_CurrentAnimTime;
				}
				i_LookingDir = 3;
			}
		}
		else if(i_CurrentLookDir == 6)
		{
			if(s_NewAnim != s_PlayerState)
			{
				if(s_NewAnim == "Idle")
				{
					//Play animation d'idle
					//changer s_PlayerState
					//changer la variable current animation
					s_CurrentAnim = "idle_6";
					s_PlayerState = "Idle";
				}
				else if(s_NewAnim == "Dash")
				{
					s_CurrentAnim = "dash_6";
					s_PlayerState = "Dash";
				}
				else if(s_NewAnim == "Attaque")
				{
					s_CurrentAnim = "attack_6";
					s_PlayerState = "Attaque";
				}
				else if(s_NewAnim == "Walk")
				{
					s_CurrentAnim = "walk_6";
					s_PlayerState = "Walk";
				}
				
				_PlayerAnims.Play(s_CurrentAnim, 0);
			}
			else if(i_CurrentLookDir != i_LookingDir)
			{
				if(s_PlayerState == "Idle")
				{
					_PlayerAnims.Play("idle_6");
//					animation["idle_6"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Dash")
				{
					_PlayerAnims.Play("dash_6");
//					animation["dash_6"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Attaque")
				{
					_PlayerAnims.Play("attack_6");
//					animation["attack_6"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Walk")
				{
					_PlayerAnims.Play("walk_6");
//					animation["walk_6"].time = f_CurrentAnimTime;
				}
				i_LookingDir = 6;
			}
		}
		else if(i_CurrentLookDir == 9)
		{
			if(s_NewAnim != s_PlayerState)
			{
				if(s_NewAnim == "Idle")
				{
					//Play animation d'idle
					//changer s_PlayerState
					//changer la variable current animation
					s_CurrentAnim = "idle_9";
					s_PlayerState = "Idle";
				}
				else if(s_NewAnim == "Dash")
				{
					s_CurrentAnim = "dash_9";
					s_PlayerState = "Dash";
				}
				else if(s_NewAnim == "Attaque")
				{
					s_CurrentAnim = "attack_9";
					s_PlayerState = "Attaque";
				}
				else if(s_NewAnim == "Walk")
				{
					s_CurrentAnim = "walk_9";
					s_PlayerState = "Walk";
				}
				
				_PlayerAnims.Play(s_CurrentAnim);
			}
			else if(i_CurrentLookDir != i_LookingDir)
			{
				if(s_PlayerState == "Idle")
				{
					_PlayerAnims.Play("idle_9");
//					animation["idle_9"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Dash")
				{
					_PlayerAnims.Play("dash_9");
//					animation["dash_9"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Attaque")
				{
					_PlayerAnims.Play("attack_9");
//					animation["attack_9"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Walk")
				{
					_PlayerAnims.Play("walk_9");
//					animation["walk_9"].time = f_CurrentAnimTime;
				}
				i_LookingDir = 9;
			}
		}
		else if(i_CurrentLookDir == 8)
		{
			if(s_NewAnim != s_PlayerState)
			{
				if(s_NewAnim == "Idle")
				{
					//Play animation d'idle
					//changer s_PlayerState
					//changer la variable current animation
					s_CurrentAnim = "idle_8";
					s_PlayerState = "Idle";
				}
				else if(s_NewAnim == "Dash")
				{
					s_CurrentAnim = "dash_8";
					s_PlayerState = "Dash";
				}
				else if(s_NewAnim == "Attaque")
				{
					s_CurrentAnim = "attack_8";
					s_PlayerState = "Attaque";
				}
				else if(s_NewAnim == "Walk")
				{
					s_CurrentAnim = "walk_8";
					s_PlayerState = "Walk";
				}
				
				_PlayerAnims.Play(s_CurrentAnim);
			}
			else if(i_CurrentLookDir != i_LookingDir)
			{
				if(s_PlayerState == "Idle")
				{
					_PlayerAnims.Play("idle_8");
//					animation["idle_8"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Dash")
				{
					_PlayerAnims.Play("dash_8");
//					animation["dash_8"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Attaque")
				{
					_PlayerAnims.Play("attack_8");
//					animation["attack_8"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Walk")
				{
					_PlayerAnims.Play("walk_8");
//					animation["walk_8"].time = f_CurrentAnimTime;
				}
				i_LookingDir = 8;
			}
		}
		else if(i_CurrentLookDir == 7)
		{
			if(s_NewAnim != s_PlayerState)
			{
				if(s_NewAnim == "Idle")
				{
					//Play animation d'idle
					//changer s_PlayerState
					//changer la variable current animation
					s_CurrentAnim = "idle_7";
					s_PlayerState = "Idle";
				}
				else if(s_NewAnim == "Dash")
				{
					s_CurrentAnim = "dash_7";
					s_PlayerState = "Dash";
				}
				else if(s_NewAnim == "Attaque")
				{
					s_CurrentAnim = "attack_7";
					s_PlayerState = "Attaque";
				}
				else if(s_NewAnim == "Walk")
				{
					s_CurrentAnim = "walk_7";
					s_PlayerState = "Walk";
				}
				
				_PlayerAnims.Play(s_CurrentAnim);
			}
			else if(i_CurrentLookDir != i_LookingDir)
			{
				if(s_PlayerState == "Idle")
				{
					_PlayerAnims.Play("idle_7");
//					animation["idle_7"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Dash")
				{
					_PlayerAnims.Play("dash_7");
//					animation["dash_7"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Attaque")
				{
					_PlayerAnims.Play("attack_7");
//					animation["attack_7"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Walk")
				{
					_PlayerAnims.Play("walk_7");
//					animation["walk_7"].time = f_CurrentAnimTime;
				}
				i_LookingDir = 7;
			}
		}
		else if(i_CurrentLookDir == 4)
		{
			if(s_NewAnim != s_PlayerState)
			{
				if(s_NewAnim == "Idle")
				{
					//Play animation d'idle
					//changer s_PlayerState
					//changer la variable current animation
					s_CurrentAnim = "idle_4";
					s_PlayerState = "Idle";
				}
				else if(s_NewAnim == "Dash")
				{
					s_CurrentAnim = "dash_4";
					s_PlayerState = "Dash";
				}
				else if(s_NewAnim == "Attaque")
				{
					s_CurrentAnim = "attack_4";
					s_PlayerState = "Attaque";
				}
				else if(s_NewAnim == "Walk")
				{
					s_CurrentAnim = "walk_4";
					s_PlayerState = "Walk";
				}
				
				_PlayerAnims.Play(s_CurrentAnim);
			}
			else if(i_CurrentLookDir != i_LookingDir)
			{
				if(s_PlayerState == "Idle")
				{
					_PlayerAnims.Play("idle_4");
//					animation["idle_4"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Dash")
				{
					_PlayerAnims.Play("dash_4");
//					animation["dash_4"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Attaque")
				{
					_PlayerAnims.Play("attack_4");
//					animation["attack_4"].time = f_CurrentAnimTime;
				}
				else if(s_PlayerState == "Walk")
				{
					_PlayerAnims.Play("walk_4");
//					animation["walk_4"].time = f_CurrentAnimTime;
				}
				i_LookingDir = 4;
			}
		}
	}


//	IEnumerator flashColor()
//	{
//		this.GetComponent<SpriteRenderer>().color = s_brightColor;
//		yield return new WaitForSeconds(0.2f);
//		this.GetComponent<SpriteRenderer>().color = Color.white;
//		if(b_IsInPuMode) StartCoroutine(flashColor());
//	}

//	private void OnDrawGizmosSelected() {
//		Gizmos.color = Color.blue;			
//		Gizmos.DrawWireSphere(transform.position, f_ZoneDash);
//	}	
}
