using UnityEngine;
using System.Collections;

public class SC_dashTrail : MonoBehaviour {	


//to add  whole script buddy

	public Transform _TrailPrefab;
	Color _Color;	
	public float f_TrailSpeed;
		
	float f_TrailSpeedFade;	
	bool b_TrailOn;
	bool b_TrailLaunched;

	SC_Player SC_PlayerRef;

	[HideInInspector]
	public bool b_comboAttack = false;

	public ParticleSystem p_dashTrailParticle;

	bool b_whiteTrail;

	public Material playerTrail;

	// Use this for initialization
	void Start () 
	{
		//SYSTEM AVEC PARTICLE SYSTEM
		//p_dashTrailParticle = this.transform.FindChild("p_dashTrailParticle").particleSystem;

		SC_PlayerRef = this.GetComponent<SC_Player>();
		_Color = SC_PlayerRef.GetSpriteColor();

		//renderer.material.color = _Color;
		b_TrailOn = false;
		b_TrailLaunched = false;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (b_TrailOn == true && b_TrailLaunched == false){
			b_TrailLaunched = true;
			StartCoroutine(spawner());

		//SYSTEM AVEC PARTICLE SYSTEM
//			if(p_dashTrailParticle.enableEmission == false) p_dashTrailParticle.enableEmission = true;
//			emitTrail();

		}
	}
	
	public void activateTrail(float trailSpeed)
	{
		f_TrailSpeedFade = trailSpeed;
		b_TrailOn = true;
	}
	
	public void deactivateTrail()
	{
		b_TrailOn = false;
		b_TrailLaunched = false;

	//	p_dashTrailParticle.enableEmission = false;
	}

	public void emitTrail()
	{

		if(this.GetComponent<SpriteRenderer>().sprite.texture.name.StartsWith("attack"))
		{
			
		}else
		{
			playerTrail.mainTexture = this.GetComponent<SpriteRenderer>().sprite.texture;
		}

	}
	
	
	IEnumerator spawner()
	{
		if(b_TrailOn == true && b_TrailLaunched == true)
		{	
	


			if(this.GetComponent<SpriteRenderer>().sprite.texture.name.StartsWith("attack"))
			{

			}else
			{
				Transform Trail = Instantiate(_TrailPrefab, new Vector3(transform.position.x,  transform.position.y, -0.04f), transform.rotation) as Transform;
				Trail.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
				if(SC_PlayerRef.b_IsInPuMode)Trail.renderer.material.color = SC_PlayerRef.s_brightColor;
				SC_trailFade SC_trailFadeRef = Trail.GetComponent<SC_trailFade>();
				SC_trailFadeRef.f_fadeOutSpeed = f_TrailSpeedFade;
			}


			yield return new WaitForSeconds(f_TrailSpeed);
			StartCoroutine(spawner());
		}
	}
	
}
