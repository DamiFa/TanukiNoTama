using UnityEngine;
using System.Collections;

public class SC_templeLife : MonoBehaviour {

	public float f_DeathRadius;
	public float f_LifeAmount;
	public float f_LifeRegenSpeed;
	
	GameObject g_CrestRoot;
	Collider[] a_CloseEnemies;
	
	public bool b_Alive = true;
	bool b_inEnnemyZone = false;
	
	float f_LifeAmountMax;
	float f_StartSize;
	public float f_damageReceived;
	float f_CurSize;
	SC_Damage SC_DamageRef;

	public Light mainLight;

	public ParticleSystem p_tanukiParticle;
	public ParticleSystem p_tanukiParticleAttacked;

	public GameObject g_tanuki;
	private Animator _tanukiAnimator;
	private bool b_animationAlert = false;

	public GameObject[] g_tanukiPlayerAlert;

	public SC_GameManager SC_GameManagerRef;
	
	// Use this for initialization
	void Start () 
	{
		_tanukiAnimator = g_tanuki.GetComponent<Animator>();
		g_CrestRoot = this.gameObject;
		f_StartSize = transform.localScale.x;
		f_LifeAmountMax = f_LifeAmount;
	//	p_tanukiParticle = 	this.transform.FindChild("v_tanukiParticle").particleSystem;

		g_tanukiPlayerAlert = GameObject.FindGameObjectsWithTag("tanukiPlayerAlert");

		foreach(GameObject tanukiAlert in g_tanukiPlayerAlert)
		{
			tanukiAlert.SetActive(false);
		}

		SC_GameManagerRef = GameObject.FindGameObjectWithTag("gameManager").GetComponent<SC_GameManager>();
		
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

						p_tanukiParticleAttacked.enableEmission = true;
						p_tanukiParticle.gameObject.SetActive(false);


					}else
					{
						b_inEnnemyZone = false;
						p_tanukiParticleAttacked.enableEmission = false;
						p_tanukiParticle.gameObject.SetActive(true);
					}
				}		
				f_LifeAmount -= f_damageReceived * Time.deltaTime;
				f_damageReceived = 0;
			}else
			{
				b_inEnnemyZone = false;
			}


			if(b_inEnnemyZone && b_animationAlert != true)
			{
				_tanukiAnimator.Play("batimentCentral_alert");

				foreach(GameObject tanukiAlert in g_tanukiPlayerAlert)
				{
					tanukiAlert.SetActive(true);
				}

				b_animationAlert = true;


			}

			if(b_inEnnemyZone != true && b_animationAlert)
			{
				_tanukiAnimator.Play("batimentCentral_idle");
				foreach(GameObject tanukiAlert in g_tanukiPlayerAlert)
				{
					tanukiAlert.SetActive(false);
				}
				b_animationAlert = false;
			}

			
			if(b_inEnnemyZone == false && f_LifeAmount < f_LifeAmountMax)
			{
				f_damageReceived = 0;
				f_LifeAmount += f_LifeRegenSpeed * Time.deltaTime;
				if (f_LifeAmount>f_LifeAmountMax)f_LifeAmount = f_LifeAmountMax;
			}


			
			if(f_LifeAmount <= 0 && b_Alive == true)StartCoroutine(Death());



//			if(b_inEnnemyZone == true && mainLight.color != Color.red)
//			{
//				mainLight.color = Color.red;
//				mainLight.intensity = 1.4f;
//			}
//			if(b_inEnnemyZone == false && mainLight.color != Color.white)
//			{
//				mainLight.color = Color.white;
//				mainLight.intensity = 0.01f;
//			}


			f_CurSize = (f_LifeAmount*f_StartSize)/f_LifeAmountMax;
			transform.localScale = new Vector3(f_CurSize, f_CurSize, f_CurSize); 


//			if(f_LifeAmount != f_LifeAmountMax)
//			{
//				p_tanukiParticle.startLifetime = 11;
//				p_tanukiParticle.startSize = 6;
//				p_tanukiParticle.startSpeed = 0.38f;
//			}

		}
	}	

	
	public void SetLifeAmount(float lifePerc)
	{
		f_LifeAmount += f_LifeAmountMax*lifePerc;
		if(f_LifeAmount > f_LifeAmountMax) f_LifeAmount = f_LifeAmountMax;	
	}
	
	IEnumerator Death()
	{	
		b_Alive = false;
		yield return new WaitForSeconds(0.5f);

		SC_GameManagerRef.endGame();
		//Application.LoadLevel(Application.loadedLevel);
	}
}
