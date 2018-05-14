using UnityEngine;
using System.Collections;

public class SC_Tanuki : MonoBehaviour {

	[System.Serializable]
	public class SoundsTanuki
	{
		public string _Name = "noName";
		public int _Priority;
		public AudioClip _Sound;
		
		public SoundsTanuki ()
		{
			
		}
	}

	public SoundsTanuki[] _Sounds;
	private AudioSource _AudioSource_TanukiTalking;
	private SC_templeLife _TanukiLife;
	private GameObject[] a_Players;
	private bool b_PrioritySound;
	private bool b_TanukiAttacked;
	private bool b_TanukiKilled;
	
	// Use this for initialization
	void Start () 
	{
		b_PrioritySound = false;
		_AudioSource_TanukiTalking = gameObject.AddComponent<AudioSource>();
		_TanukiLife = GetComponent<SC_templeLife>();
		a_Players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//faire les conditions de priorités ici, et changer les variables de lancement de son à true
		if(!_TanukiLife.b_Alive)
		{
			b_TanukiKilled = true;
			b_PrioritySound = true;
		}
		else if(_TanukiLife.f_damageReceived > 0)
		{
			b_TanukiAttacked = true;
			b_PrioritySound = true;
		}
	}
}
