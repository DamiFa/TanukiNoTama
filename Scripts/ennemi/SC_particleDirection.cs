using UnityEngine;
using System.Collections;

public class SC_particleDirection : MonoBehaviour {

	public Transform t_destination;

	private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(t_destination != null)
		{
			int length = particleSystem.GetParticles(particles);
	
			
			for(int i = 0; i<particles.Length;i++)
			{
				
				particles[i].position = Vector3.Lerp(particles[i].position, t_destination.transform.position, 3*Time.deltaTime);


				if(Vector3.Distance(particles[i].position,t_destination.transform.position)<0.5f) particles[i].lifetime = 0;
								
			}			
			
			particleSystem.SetParticles(particles, length); 

		

		}
	}
}
