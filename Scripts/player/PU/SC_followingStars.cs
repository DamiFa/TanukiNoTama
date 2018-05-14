using UnityEngine;
using System.Collections;

public class SC_followingStars : MonoBehaviour {

	public float f_rangeRadius = 5;
	public float f_Speed = 7;
	public float f_dissapearDelay = 0.25f;
	public float f_pushStrenght = 0;
	public GameObject _Destination;
	public int _ScoreValue = 1;

	public SC_ScoreCounter SC_ScoreCounterRef;

	Collider[] a_CloseEnemies;
	bool b_isActive = true;
	GameObject g_ennemiCible;
	float f_timer;
	float f_rotationSpeed = 300;

	SC_Ennemi SC_EnnemiRef;


	private ParticleSystem p_starParticle;

	public SC_playerComboCounter SC_playerComboCounterRef;



	// Use this for initialization
	void Start () 
	{
		SC_ScoreCounterRef = GameObject.Find("guiSCOREcounter").GetComponent<SC_ScoreCounter>();
		p_starParticle = this.transform.GetChild(0).particleSystem;
		p_starParticle.startColor = this.renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () 
	{
	//	transform.Rotate(0,0,f_rotationSpeed*Time.deltaTime,Space.World);

		if(_Destination != null)
		{
			transform.position = Vector3.MoveTowards(transform.position, _Destination.transform.position , f_Speed*Time.deltaTime);
			
			if(Vector3.Distance(new Vector3(_Destination.transform.position.x, _Destination.transform.position.y, 0f), new Vector3(transform.position.x, transform.position.y, 0f)) < 0.5f)
			{
				//lance animation explosion qui lance cette fonction suivante a la fin
				killEnnemy();
				death();
			}
		}
		else
		{
			 death ();

		}
	}

	public void moveToward()
	{
		transform.position = Vector3.MoveTowards(transform.position, _Destination.transform.position , f_Speed*Time.deltaTime);

		if(Vector3.Distance(new Vector3(g_ennemiCible.transform.position.x, g_ennemiCible.transform.position.y, 0f), new Vector3(transform.position.x, transform.position.y, 0f)) < 0.5f)
		{
			//lance animation explosion qui lance cette fonction suivante a la fin
			killEnnemy();
		}
	}

	public void killEnnemy()
	{
		Vector3 v_HitDirection;
		v_HitDirection = -(transform.position - _Destination.transform.position);
		v_HitDirection = new Vector3(v_HitDirection.x, v_HitDirection.y, 0);
		v_HitDirection = v_HitDirection.normalized;

		SC_EnnemiRef = _Destination.GetComponent<SC_Ennemi>();
		SC_EnnemiRef.death(v_HitDirection, f_pushStrenght, 0, SC_playerComboCounterRef.i_killCount, SC_playerComboCounterRef.SC_PlayerRef);
		SC_ScoreCounterRef.IncreaseScore(SC_EnnemiRef.i_ScoreValue*Mathf.Clamp(SC_playerComboCounterRef.i_killCount, 1,999));
	}

	public void death()
	{
		Destroy(gameObject);
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;			
		Gizmos.DrawWireSphere(transform.position, f_rangeRadius);
	}
}
