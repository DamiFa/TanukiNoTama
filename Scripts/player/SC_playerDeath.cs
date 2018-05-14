using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_playerDeath : MonoBehaviour {

	public Transform death_swordPrefab;

	public float f_DeathRadius;
	public float f_LifeAmount;
	public float f_LifeRegenSpeed;


	public GameObject lifeCounter;
//	public GameObject _charactRenderer;	
	public SC_templeReincarnate SC_templeReincarnateRef;

	Collider[] a_CloseEnemies;

	float f_damageReceived;
	Color _deathColor;
	bool b_Alive = true;
	bool b_inEnnemyZone = false;

	Light l_LifeLamp;
	float f_LifeAmountMax;
	float f_LifeLampMax;

	//GameObject _Player;
	Vector3 v_StartPos;

	Vector3 v_respawnPoint;

	SC_Player SC_PlayerRef;
	SC_mainLight SC_mainLightRef;
	SC_Damage SC_DamageRef;
	SC_ScoreCounter SC_ScoreCounterRef;
	SC_Ennemi SC_EnnemiRef;


	//NOUVELLE VARIABLE QUI SONT PAS AJOUTER AUTOMATIQUEMENT (DESOLER)
	public SC_playerComboCounter 	SC_playerComboCounterRef;
	public SC_Trigger SC_TriggerRef;
	public SC_SpawnerManager SC_SpawnerManagerRef;
	public bool b_isActive = true;
	[HideInInspector]
	public int i_storedKillCount;

	// Use this for initialization

	SC_BatimentChoice SC_BatimentChoiceRef;

	void Start () 
	{
		//_Player = this.gameObject;

		SC_BatimentChoiceRef = this.GetComponent<SC_BatimentChoice>();
		l_LifeLamp = transform.Find("LifeLamp").light;
		f_LifeAmountMax = f_LifeAmount;
		f_LifeLampMax = l_LifeLamp.spotAngle;
		//v_StartPos = transform.position;
		v_respawnPoint = GameObject.FindGameObjectWithTag("respawnPoint").transform.position;
		v_respawnPoint = new Vector3(v_respawnPoint.x, v_respawnPoint.y, transform.position.z);

		//SC_playerComboCounterRef = this.GetComponent<SC_playerComboCounter>();
		SC_ScoreCounterRef = GameObject.FindGameObjectWithTag("scoreCounter").GetComponent<SC_ScoreCounter>();
		SC_PlayerRef = this.GetComponent<SC_Player>();
		SC_mainLightRef = GameObject.Find("flashLight_root").GetComponent<SC_mainLight>();

		_deathColor = SC_PlayerRef.s_Color;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(b_isActive)
		{
			a_CloseEnemies = Physics.OverlapSphere(transform.position, f_DeathRadius);

			if(a_CloseEnemies.Length > 0)
			{	
				foreach (Collider EnnemyInZone in a_CloseEnemies)
				{
					if(EnnemyInZone.transform.tag == "Ennemi" || EnnemyInZone.transform.tag == "particule" )
					{			
						b_inEnnemyZone = true;
						SC_DamageRef = EnnemyInZone.gameObject.GetComponent<SC_Damage>();
						f_damageReceived += SC_DamageRef.Damage();
					}else
					{
						b_inEnnemyZone = false;
					}
				}		
				f_LifeAmount -= f_damageReceived * Time.deltaTime;
				f_damageReceived = 0;
			}

			if(b_inEnnemyZone == false && f_LifeAmount < f_LifeAmountMax)
			{
				f_damageReceived = 0;
				f_LifeAmount += f_LifeRegenSpeed * Time.deltaTime;
				if (f_LifeAmount>f_LifeAmountMax)f_LifeAmount = f_LifeAmountMax;
			}

			if(f_LifeAmount <= 0 && b_Alive == true)
			{
				foreach (Collider EnnemyInZone in a_CloseEnemies)
				{
					if(EnnemyInZone.transform.tag == "Ennemi")
					{
						SC_EnnemiRef = EnnemyInZone.gameObject.GetComponent<SC_Ennemi>();
						SC_EnnemiRef.death(SC_TriggerRef.getHitDir(EnnemyInZone.transform), 10, 1, 0, SC_PlayerRef);
					}
				}

				Death();
			}
			l_LifeLamp.spotAngle = (f_LifeAmount*f_LifeLampMax)/f_LifeAmountMax;
		}
	}

	public void SetLifeAmount(float lifePerc)
	{
		f_LifeAmount += f_LifeAmountMax*lifePerc;
		if(f_LifeAmount > f_LifeAmountMax) f_LifeAmount = f_LifeAmountMax;	
	}

	public void Death()
	{
		Transform death_Sword = Instantiate(death_swordPrefab, new Vector3(transform.position.x,  transform.position.y, -0.03f), transform.rotation) as Transform;

		//THIS LINE GETS IF THE PLAYER WAS IN COMBO AND AT WICH MULTIPLICATOR HE WAS (USED FOR TEMPLE REGEN TIMER)
		if(SC_playerComboCounterRef.b_isActive == true && SC_playerComboCounterRef.i_killCount > 0 )
		{
			i_storedKillCount = SC_playerComboCounterRef.i_killCount;
		}

		//ESSAI SPAWN ENNEMI
//		if(i_storedKillCount/4 > 0)
//		{
//			for(int i = 0;i<(i_storedKillCount/4); i++)
//			{
//				GameObject ennemiTemp = Instantiate(SC_SpawnerManagerRef.ChooseEnemy(), new Vector3(transform.position.x+i,  transform.position.y+i, -0.04f), Quaternion.identity) as GameObject;
//			}
//		}


//		SC_PlayerRef.rumble(0.4f);

		//SC_BatimentChoiceRef.instantDestroy();
		b_isActive = false;
		b_Alive = false;
		SC_PlayerRef.resetKillCount();	
		SC_PlayerRef.AnimationHandeler("Death");		//Animation de mort
//		l_LifeLamp.enabled = false;

		transform.tag = null;
		transform.position = v_respawnPoint;
		SC_PlayerRef.stopRumble();
		SC_PlayerRef.StopMvt();
		SC_PlayerRef.DashEnd();
		SC_PlayerRef.enabled = false;
		renderer.enabled = false;
//		_charactRenderer.SetActive(false);

		//Audio Mort
		SC_PlayerRef._StateAudioSource.Stop();
		SC_PlayerRef._StateAudioSource.PlayOneShot(SC_PlayerRef._Sounds.GetSound("Death"));

		SC_templeReincarnateRef.addDeadPlayer(this.gameObject);
	}

	public void Reincarnate()
	{
		f_damageReceived = 0;
		
		f_LifeAmount = f_LifeAmountMax;
		l_LifeLamp.spotAngle = f_LifeLampMax;
		l_LifeLamp.enabled = true;
		transform.renderer.enabled = true;	
		SC_PlayerRef.AnimationHandeler("Idle");
//		_charactRenderer.SetActive(true);		
		SC_PlayerRef.enabled = true;
		transform.tag = "Player"; 		
		b_Alive = true;
		SC_PlayerRef._AttackAudioSource.PlayOneShot(SC_PlayerRef._Sounds.GetSound("Respawn"));
		StartCoroutine(respawnProtection());
	}

	IEnumerator respawnProtection()
	{
		yield return new WaitForSeconds(3f);
		b_isActive = true;
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;			
		Gizmos.DrawWireSphere(transform.position, f_DeathRadius);
	}

	
}
