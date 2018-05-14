using UnityEngine;
using System.Collections;

public class SC_batimentVie : MonoBehaviour {


	public float f_DeathRadius;
	public float f_LifeAmount;
	public float f_LifeRegenSpeed;

	GameObject g_CrestRoot;
	Collider[] a_CloseEnemies;

	bool b_Alive = true;
	bool b_inEnnemyZone = false;

	float f_LifeAmountMax;
	float f_StartSize;
	float f_damageReceived;
	float f_CurSize;
	SC_Damage SC_DamageRef;


	private Color c_crestColor;
	float f_currentAlpha;


	private ParticleSystem p_damageParticle;
	private GameObject g_backgroundColor;

	//Pour le PU MAnager
	public string s_BatType;
	public GameObject _Player;
	SC_Player SC_PlayerRef;

	private Vector3 v_startSize;


	private bool b_beingAttacked = false;


	private SC_SoundManager _Sounds;
	private AudioSource _AudioSource_bat;

	// Use this for initialization
	void Start () 
	{
		_AudioSource_bat = gameObject.AddComponent<AudioSource>();
		_Sounds = GameObject.FindGameObjectWithTag("SOUND_MANAGER").GetComponent<SC_SoundManager>();

		g_backgroundColor = this.transform.FindChild("bat_Background").gameObject;
		p_damageParticle = this.transform.FindChild("damageParticle").particleSystem;
		SC_PlayerRef = _Player.GetComponent<SC_Player>();
		renderer.material.color = SC_PlayerRef.s_brightColor;
		g_CrestRoot = this.gameObject;
		f_StartSize = transform.localScale.x;
		f_LifeAmountMax = f_LifeAmount;
		c_crestColor = renderer.material.color;

		p_damageParticle.startColor = c_crestColor;
		g_backgroundColor.renderer.material.color = SC_PlayerRef.s_brightColor;
		g_backgroundColor.SetActive(false);

		v_startSize = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(b_Alive){

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
			}else
			{
				b_inEnnemyZone = false;
			}
			
			if(b_inEnnemyZone == false && f_LifeAmount < f_LifeAmountMax)
			{
				f_damageReceived = 0;
				f_LifeAmount += f_LifeRegenSpeed * Time.deltaTime;
				if (f_LifeAmount>f_LifeAmountMax)f_LifeAmount = f_LifeAmountMax;
			}
			
			if(f_LifeAmount <= 0 && b_Alive == true)StartCoroutine(Death());
			
			if(b_inEnnemyZone)
			{


				if(b_beingAttacked != true)
				{
					p_damageParticle.enableEmission = true;
					StartCoroutine(beingDamagedVisual());
					b_beingAttacked = true;
				}
			}else{
				p_damageParticle.enableEmission = false;
				b_beingAttacked = false;
				gameObject.renderer.material.color = c_crestColor;
			}

			f_currentAlpha = (f_LifeAmount*1)/f_LifeAmountMax; 
			c_crestColor.a = f_currentAlpha;
			gameObject.renderer.material.color = c_crestColor;

		}
	}

	IEnumerator beingDamagedVisual()
	{

		renderer.material.color = Color.white;
		transform.localScale = v_startSize*1.5f;

		yield return new WaitForSeconds(0.1f);
		renderer.material.color = SC_PlayerRef.s_brightColor;
		transform.localScale = v_startSize;

		if(b_beingAttacked) StartCoroutine(beingDamagedVisual());
	}

	
	public void SetPlayer (GameObject _player)
	{
		_Player = _player;
		_Player.GetComponent<SC_PUManager>().AddOrRemovePU(s_BatType, 1, this.gameObject);
	}
	
	public void SetLifeAmount(float lifePerc)
	{
		f_LifeAmount += f_LifeAmountMax*lifePerc;
		if(f_LifeAmount > f_LifeAmountMax) f_LifeAmount = f_LifeAmountMax;	
	}

	IEnumerator Death()
	{
		_Player.GetComponent<SC_PUManager>().AddOrRemovePU(s_BatType, -1, this.gameObject);
		b_Alive = false;
		yield return new WaitForSeconds(0.5f);
		_AudioSource_bat.PlayOneShot(_Sounds.GetSound("PuDisparition"));

		Destroy(g_CrestRoot.gameObject);
	}


	public void activate()
	{
		c_crestColor = SC_PlayerRef.s_brightColor;
		g_backgroundColor.SetActive(true);
	}

	public void deactivate()
	{
		c_crestColor = SC_PlayerRef.s_Color;
		g_backgroundColor.SetActive(false);
	}


	/*
	public void Damage()
	{
		i_lifeCount --;
		
		if (i_lifeCount == 2) renderer.material.color = Color.yellow;
		if (i_lifeCount == 1) renderer.material.color = Color.red;
		
		if (i_lifeCount <= 0)		{
			// send info to player to loose his skill (if batiment is type ''incremente batiment'')
			Destroy(gameObject);
		}
	}
	*/
}
