using UnityEngine;
using System.Collections;

public class SC_particleSetRotation : MonoBehaviour {



	private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

	public ParticleSystem p_AbsorbParticle;


	public Transform t_destination;

	//private Vector3 v_destination;

	public Color c_PlayerColor;

	private float f_timer;

	public float f_speed;

	public bool b_toBeDestroyed = true;
	public bool b_toLerpColor = true;

	float i_increaseSpeed;

	// Use this for initialization
	void Awake ()
	{
		if(b_toBeDestroyed) p_AbsorbParticle = this.transform.GetChild(0).particleSystem;
	
		//p_AbsorbParticle.Play ();
		//p_AbsorbParticle.Play();
	}

	void Start()
	{
		if(b_toBeDestroyed) p_AbsorbParticle = this.transform.GetChild(0).particleSystem;
		//p_AbsorbParticle.Play();

		if(b_toBeDestroyed) p_AbsorbParticle.Emit(1);
		//Debug.Log("ABSORB22");
	}




	void LateUpdate() {
		if(t_destination != null)
		{
		int length = p_AbsorbParticle.particleSystem.GetParticles(particles);

//		int i = 0;
//
//		//particles[0].velocity += v_destination*Time.deltaTime*150;
//
//
//		particles[0].position = Vector3.Lerp(particles[0].position, t_destination.transform.position, 10f*Time.deltaTime);
//
//	
//		if(Vector3.Distance(particles[i].position, t_destination.transform.position)<1f)
//		{
//			particles[i].velocity = new Vector3(0,0,0);
//			particles[i].size = 10;
//			particles[i].color = Color.white;
//			particles[i].lifetime = 0;
//		}
//
//
//		if(Vector3.Distance(particles[0].position, t_destination.transform.position)<5f)
//		{
//			particles[0].velocity += v_destination*Time.deltaTime*25;
//		}
//
//
//		while (i < length) {			
//			particles[i].color = Color.Lerp(particles[i].color, c_PlayerColor, 3f*Time.deltaTime)  ;
//			i++;
//			particles[i].velocity += v_destination*Time.deltaTime*600;
//			if(Vector3.Distance(particles[i].position, t_destination.transform.position)<6)
//			{		
//				particles[i].size = 4;
//				particles[i].color = Color.white;			
//			}
//			if(Vector3.Distance(particles[i].position, t_destination.transform.position)<1)
//			{
//				particles[i].velocity = new Vector3(0,0,0);
//				particles[i].size = 10;
//				particles[i].color = Color.white;
//				particles[i].lifetime = 0;
//			}
//			if(Vector3.Distance(particles[i].position, t_destination.transform.position)> 5) particles[i].velocity += v_destination*Time.deltaTime*600;
//			if(Vector3.Distance(particles[i].position, t_destination.transform.position)< 5 && Vector3.Distance(particles[i].position, t_destination.transform.position)>= 1) particles[i].velocity += v_destination*Time.deltaTime*150;
//		}



		for(int i = 0; i<particles.Length;i++)
		{
			//particles[0].velocity += v_destination*Time.deltaTime*20;


//			if(f_timer>0.2f)
//			{
				

				i_increaseSpeed += 0.05f*Time.deltaTime;

			if(b_toBeDestroyed != true) particles[i].position = Vector3.Lerp(particles[i].position, t_destination.transform.position, 5*Time.deltaTime*particles[i].size*Random.Range(0.5f,3));

			if(b_toBeDestroyed)
			{
					//particles[i].position = Vector3.Lerp(particles[i].position, t_destination.transform.position, 5*Time.deltaTime*particles[i].size*Random.Range(0.5f,3));
					particles[i].position = Vector3.Lerp(particles[i].position, t_destination.transform.position, Time.deltaTime*10+i_increaseSpeed);

			}
				if(b_toLerpColor)	particles[i].color = Color.Lerp(particles[i].color, c_PlayerColor, 2f*Time.deltaTime)  ;

				if(Vector3.Distance(particles[i].position, t_destination.transform.position)<0.01f)
				{
				//	particles[i].velocity = new Vector3(0,0,0);
					//particles[i].lifetime = 0;
				}
		//	}

		}


		p_AbsorbParticle.particleSystem.SetParticles(particles, length); 

		if( length ==0 && b_toBeDestroyed) Destroy(gameObject);
		if(length ==0 && b_toBeDestroyed != true) gameObject.SetActive(false);
		}
	
	}

	// Update is called once per frame
	void Update () 
	{
//		if(t_destination != null)
//		{
//			v_destination = t_destination.transform.position- this.transform.position;
//			v_destination = v_destination.normalized;
//		}
	//	f_timer += Time.deltaTime;

		//if(!p_AbsorbParticle.isPlaying) Destroy(gameObject);

	}
}
