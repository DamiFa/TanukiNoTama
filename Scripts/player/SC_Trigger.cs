using UnityEngine;
using System.Collections;

public class SC_Trigger : MonoBehaviour {

	public GameObject _Player;
	public SC_Player SC_PlayerRef;

	private SC_Ennemi SC_EnnemiRef;

	private	Vector3 v_HitDirection;
	private float f_StrenghtRef = 10;

	private GameObject _MainCam;

	private SC_playerComboCounter SC_playerComboCounterRef;

	public Transform _swordStrikePrefab;
	private float f_angle;
	public Transform t_parentRoot;

	// Use this for initialization
	void Start () 
	{
		t_parentRoot =  this.transform.parent.gameObject.transform;
		f_StrenghtRef = SC_PlayerRef.GetPushStrenght();
		_MainCam = GameObject.FindWithTag("MainCamera");
		SC_playerComboCounterRef = SC_PlayerRef.SC_playerComboCounterRef;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(SC_playerComboCounterRef == null) doStartAgain();
	}

	private void doStartAgain()
	{
		t_parentRoot =  this.transform.parent.gameObject.transform;
		f_StrenghtRef = SC_PlayerRef.GetPushStrenght();
		_MainCam = GameObject.FindWithTag("MainCamera");
		SC_playerComboCounterRef = SC_PlayerRef.SC_playerComboCounterRef;
	}


	private void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.CompareTag("Ennemi"))
		{
			SC_EnnemiRef = col.gameObject.GetComponent<SC_Ennemi>();

			v_HitDirection = getHitDir(col.transform);
			SC_EnnemiRef.death(v_HitDirection, f_StrenghtRef, 1, SC_playerComboCounterRef.i_killCount, SC_PlayerRef);
			SC_PlayerRef.CaCHasTouched(col.gameObject);

			_MainCam.GetComponent<SC_CameraBehavior>().setTilt(v_HitDirection);

			Transform _swordStrikeTEMP = Instantiate(_swordStrikePrefab, 
			                                         new Vector3(col.transform.position.x, 
			                                                     col.transform.position.y, 
			                                                     col.transform.position.z+0.01f),
			                                         Quaternion.Euler(new Vector3(t_parentRoot.transform.rotation.x,
			                             										  t_parentRoot.transform.rotation.y, 
			                             										  t_parentRoot.transform.rotation.z + Random.Range(-90,90)))) as Transform;

			_swordStrikeTEMP.renderer.material.color = SC_PlayerRef.s_brightColor;

			float f_valueForIncrease = Mathf.Clamp(SC_playerComboCounterRef.i_killCount/10, 0,2.5f);

			if(SC_playerComboCounterRef.i_killCount>1)
			{
				_swordStrikeTEMP.localScale = new Vector3(_swordStrikeTEMP.localScale.x + f_valueForIncrease, 
			                                              _swordStrikeTEMP.localScale.y + f_valueForIncrease, 
			                                              _swordStrikeTEMP.localScale.z + f_valueForIncrease);
			}
		}
	}

	public Vector3 getHitDir(Transform Ennemy)
	{
		v_HitDirection = -(_Player.transform.position - Ennemy.transform.position);
		v_HitDirection = new Vector3(v_HitDirection.x, v_HitDirection.y);
		v_HitDirection = v_HitDirection.normalized;
		return v_HitDirection;
	}
}
