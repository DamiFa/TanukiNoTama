using UnityEngine;
using System.Collections;

public class SC_Roche : MonoBehaviour {

	private Collider[] a_PlayersAround;
	public float f_DistanceDetect;
	private bool b_IsInRock;
	public float f_DelayBeforeRock;
	private bool b_PlayerInside;


	private SC_SoundManager _Sounds;
	private AudioSource _AudioSource_Roche;

	Color c_inNormalState;
	Color c_inRocheState;

	// Use this for initialization
	void Start () 
	{
//		c_inNormalState = renderer.material.color;

//		c_inRocheState = renderer.material.color;
//		c_inRocheState.a = 0.25f;

		_AudioSource_Roche = gameObject.AddComponent<AudioSource>();
		_Sounds = GameObject.FindGameObjectWithTag("SOUND_MANAGER").GetComponent<SC_SoundManager>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		a_PlayersAround = Physics.OverlapSphere(transform.position, f_DistanceDetect + 10f);

		for (int i = 0; i < a_PlayersAround.Length; i ++)
		{
			if (a_PlayersAround[i].CompareTag("Player")) 
			{
				if(Vector3.Distance(a_PlayersAround[i].transform.position, this.transform.position) < f_DistanceDetect)
				{
					b_PlayerInside = true;

					if(!b_IsInRock)
					{
						StartCoroutine(TurnInRock ());
					}
				}
				else
				{
					b_PlayerInside = false;
				}

				if(a_PlayersAround[i] == null)
				{
					b_PlayerInside = false;
				}
			}
		}
	}

	IEnumerator TurnInRock ()
	{
		b_IsInRock = true;

		this.GetComponent<SC_EnemyBehavior>().i_State = 5;

		yield return new WaitForSeconds(f_DelayBeforeRock);

		_AudioSource_Roche.PlayOneShot(_Sounds.GetSound("Disparition"));


		//Changer animation ici
		for(int i = 0; i < transform.childCount; i++)
		{
			if(transform.GetChild(i).name == "corps" || transform.GetChild(i).name == "masque")
			{
				transform.GetChild(i).renderer.material.color = new Color (transform.GetChild(i).renderer.material.color.r,
				                                                           transform.GetChild(i).renderer.material.color.g,
				                                                           transform.GetChild(i).renderer.material.color.b,
				                                                           0.2f);
			}
		}

		while(b_PlayerInside)
		{
			this.collider.enabled = false;
//			renderer.material.color = c_inRocheState;
			yield return new WaitForSeconds(0.1f);
		}

		_AudioSource_Roche.PlayOneShot(_Sounds.GetSound("Apparition"));


		for(int i = 0; i < transform.childCount; i++)
		{
			if(transform.GetChild(i).name == "corps" || transform.GetChild(i).name == "masque")
			{
				transform.GetChild(i).renderer.material.color = new Color (transform.GetChild(i).renderer.material.color.r,
				                                                           transform.GetChild(i).renderer.material.color.g,
				                                                           transform.GetChild(i).renderer.material.color.b,
				                                                           1);
			}
		}
		this.collider.enabled = true;
		this.GetComponent<SC_EnemyBehavior>().i_State = (int)this.GetComponent<SC_EnemyBehavior>().i_StateStart;

		b_IsInRock = false;

		yield break;
	}
}
