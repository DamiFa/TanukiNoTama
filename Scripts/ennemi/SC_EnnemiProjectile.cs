using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_EnnemiProjectile : MonoBehaviour {

	public GameObject _Projectile;
	private List<GameObject> a_Projectiles;
	public float f_TimeForProjectile;
	private float f_StartTime;
	private float f_Timer;
	public int i_NbProjectil;

	public ParticleSystem p_spawnParticle;

	private Vector3 v_StartSize;
	private Color c_startColor;
	private GameObject g_corps;

	private SC_SoundManager _Sounds;
	private AudioSource _AudioSource_Spawn;

	// Use this for initialization
	void Start () 
	{
		_AudioSource_Spawn = gameObject.AddComponent<AudioSource>();
		_Sounds = GameObject.FindGameObjectWithTag("SOUND_MANAGER").GetComponent<SC_SoundManager>();

		v_StartSize = this.transform.localScale;
		c_startColor = gameObject.GetComponent<SpriteRenderer>().color;
		a_Projectiles = new List<GameObject>();
		f_StartTime = Time.time;
		g_corps = this.transform.FindChild("corps").gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(transform.localScale != v_StartSize && f_Timer < f_TimeForProjectile-0.7f)
		{
			this.transform.localScale = Vector3.Lerp(this.transform.localScale, v_StartSize, 3*Time.deltaTime);
		}

		
		if(a_Projectiles.Count < i_NbProjectil)
		{
			f_Timer = Time.time - f_StartTime;


			if(f_Timer > f_TimeForProjectile-0.7f)
			{
				this.transform.localScale = Vector3.Lerp(this.transform.localScale, 
					                                         new Vector3(this.transform.localScale.x*0.5f, this.transform.localScale.y*2f, this.transform.localScale.z), 
					                                         2*Time.deltaTime);

				g_corps.GetComponent<SpriteRenderer>().color = Color.Lerp (g_corps.GetComponent<SpriteRenderer>().color, Color.white, 5*Time.deltaTime);
			}


			if(f_Timer > f_TimeForProjectile)
			{
				this.transform.localScale = new Vector3(v_StartSize.x*2f, v_StartSize.y*.5f, 1);
				p_spawnParticle.Play();

				GameObject clone = Instantiate(_Projectile, 
				                               new Vector3(p_spawnParticle.transform.position.x, p_spawnParticle.transform.position.y, transform.position.z), 
				                               Quaternion.identity) as GameObject;

				_AudioSource_Spawn.PlayOneShot(_Sounds.GetSound("SpawnDeProjectil"));

				clone.transform.localScale = v_StartSize*0.75f;
				a_Projectiles.Add(clone);
				g_corps.GetComponent<SpriteRenderer>().color = c_startColor;

				f_StartTime = Time.time;
			}
		}

		for(int i = 0; i < a_Projectiles.Count; i++)
		{
			if(a_Projectiles[i] == null)
			{
				a_Projectiles.RemoveAt(i);
			}
		}
	}

	public void DeleteAllProjectiles ()
	{
		for(int i = a_Projectiles.Count-1; i <= 0; i--)
		{
			if(a_Projectiles[i] != null)
			{
				Destroy(a_Projectiles[i]);
			}
		}
	}
}
