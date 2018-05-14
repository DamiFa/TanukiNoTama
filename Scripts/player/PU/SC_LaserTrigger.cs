using UnityEngine;
using System.Collections;

public class SC_LaserTrigger : MonoBehaviour {
	
	private SC_Ennemi SC_EnnemiRef;
	private SC_ScoreCounter SC_ScoreCounterRef;
	public float f_LaserSize;
	private float f_ActualLaserSize;
	public float f_Laser;
//	public GameObject _BoutLaser;
//	private GameObject _BoutLaserTemp;
	
	Vector3 v_HitDirection;
	float f_StrenghtRef = 0;

	private GameObject g_parentRoot;

public SC_playerComboCounter SC_playerComboCounterRef; 
	
	// Use this for initialization

	void Awake()
	{
		SC_ScoreCounterRef = GameObject.Find("guiSCOREcounter").GetComponent<SC_ScoreCounter>();	
		g_parentRoot = this.transform.parent.gameObject;
	}

	void Start () 
	{
		f_ActualLaserSize = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if(f_ActualLaserSize < 25)
		{
			f_ActualLaserSize += 75*Time.deltaTime;
		}
		else
		{
			Destroy(transform.parent.gameObject);
		}

		transform.parent.localScale = new Vector3(transform.parent.localScale.x,
		                                   		  f_ActualLaserSize,
		                                          transform.parent.localScale.z);

	}
	
	private void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.CompareTag("Ennemi"))
		{
			SC_EnnemiRef = col.gameObject.GetComponent<SC_Ennemi>();			
			v_HitDirection = getHitDir(col.transform);
			SC_EnnemiRef.death(v_HitDirection, f_StrenghtRef, 0, SC_playerComboCounterRef.i_killCount, SC_playerComboCounterRef.SC_PlayerRef);
			SC_ScoreCounterRef.IncreaseScore(SC_EnnemiRef.i_ScoreValue*Mathf.Clamp(SC_playerComboCounterRef.i_killCount, 1,999));
		}
	}
	
	public Vector3 getHitDir(Transform Ennemy)
	{
		v_HitDirection = -(transform.position - Ennemy.transform.position);
		v_HitDirection = new Vector3(v_HitDirection.x, v_HitDirection.y, 0);
		v_HitDirection = v_HitDirection.normalized;
		return v_HitDirection;
	}
}
