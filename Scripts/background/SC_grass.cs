using UnityEngine;
using System.Collections;

public class SC_grass : MonoBehaviour {


	private Animator _grassAnim;

	public ParticleSystem p_grassCut;

	public Sprite s_shortGrass;
	public Sprite s_medGrass;
	public Sprite s_longGrass;

	private SpriteRenderer _thisSpriteRenderer;


	private int i_state = 2;

	//state
//	1 = short
//	2 = medium
//	3 = long


	private float timer;

	// Use this for initialization
	void Start () 
	{
		_thisSpriteRenderer = this.GetComponent<SpriteRenderer>();
		_grassAnim = GetComponent<Animator>();

		
//		i_state = Random.Range(1,3);
//		if(i_state == 1)
//		{
//			_thisSpriteRenderer.sprite = s_medGrass;
//		}
//		if(i_state == 2)
//		{
//			_thisSpriteRenderer.sprite = s_longGrass;
//		}

	}
	
	// Update is called once per frame
	void Update () 
	{	

		if(i_state  == 0)
		{
			timer += Time.deltaTime;
			if(timer>10)
			{
				_grassAnim.Play("1_grass_grow_to_1");
				timer = 0;
				i_state = 1;
			}
		}


		if(i_state  == 1)
		{
			timer += Time.deltaTime;
			if(timer>10)
			{
				_grassAnim.Play("1_grass_grow_to_2");
				timer = 0;
				i_state = 2;
			}
		}




	}


	void OnTriggerEnter(Collider col)
	{
		if(i_state!= 0)
		{
			if(col.tag == "trigger")
			{
				if(i_state == 1) p_grassCut.Emit(8);
				if(i_state == 2) p_grassCut.Emit(24);
				_thisSpriteRenderer.sprite = s_shortGrass;
				_grassAnim.Play("1_grass_idle_0");

				i_state = 0;

			}
		}
	}

}
