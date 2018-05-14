using UnityEngine;
using System.Collections;

public class SC_WalkTrail : MonoBehaviour {

	public float f_TrailSpeed;
	public float f_DistBetween;

	public Transform _TrailPrefabRight;
	public Transform _TrailPrefabLeft;	

	Vector3 _LastPos;	
	bool b_RightFoot;	
	bool b_TrailOn;	
	Color _Color;	

	SC_Player SC_PlayerRef;
	
	// Use this for initialization
	void Start () 
	{
		SC_PlayerRef = this.GetComponent<SC_Player>();
		_Color = SC_PlayerRef.GetSpriteColor();
		
		b_TrailOn = false;
		_LastPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		
	}
	
	void FixedUpdate()
	{
		
		if (Vector3.Distance(_LastPos, transform.position)> f_DistBetween && b_TrailOn == false){
			b_TrailOn = true;
			StartCoroutine(spawner());
		}
		_LastPos = transform.position;
		
	}
	
	
	IEnumerator spawner()
	{	
		if(b_RightFoot == true){
			//Transform Trail = Instantiate(_TrailPrefabRight, transform.position, Quaternion.Euler(new Vector3(270,transform.rotation.eulerAngles.y,transform.rotation.z))) as Transform;
			Transform Trail = Instantiate(_TrailPrefabRight, new Vector3(transform.position.x,  transform.position.y, -0.03f), transform.rotation) as Transform;
			Trail.renderer.material.color = _Color;
		}
		if(b_RightFoot == false){
			//Transform Trail = Instantiate(_TrailPrefabLeft, transform.position, Quaternion.Euler(new Vector3(270,transform.rotation.eulerAngles.y,transform.rotation.z))) as Transform;
			Transform Trail = Instantiate(_TrailPrefabLeft, new Vector3(transform.position.x,  transform.position.y, -0.03f), transform.rotation) as Transform;
			Trail.renderer.material.color = _Color;
		}
		
		b_RightFoot = ! b_RightFoot;
		yield return new WaitForSeconds(f_TrailSpeed);			
		b_TrailOn = false;
		
	}
	
}
