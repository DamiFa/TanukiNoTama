using UnityEngine;
using System.Collections;

public class SC_ODCanim : MonoBehaviour {


	public GameObject g_ODCanimSimple;


	public bool b_isMaster = false;

	public float f_numberOfODCtoLaunch;

	public float f_numberOfODCtoLaunchRef;


	// Use this for initialization
	void Start ()
	{
	


	}
	
	// Update is called once per frame
	void Update ()
	{

		//if(!animation.isPlaying)animation.Play();

		if(b_isMaster)
		{
			StartCoroutine(spawnAnim());
			b_isMaster = false;
		}
	}

	public void death()
	{
		if(b_isMaster != true)Destroy(this.gameObject);		
	}


	IEnumerator spawnAnim()
	{

		yield return new WaitForSeconds(animation.clip.length/f_numberOfODCtoLaunch);

		GameObject _tempAnimODC = Instantiate(g_ODCanimSimple, new Vector3(transform.position.x,transform.position.y,transform.position.z) , Quaternion.identity) as GameObject;
		_tempAnimODC.renderer.material.color = this.renderer.material.color;
	//	SC_ODCanim SC_ODCanimRef = _tempAnimODC.GetComponent<SC_ODCanim>();
	//	SC_ODCanimRef.b_isMaster = false;


		f_numberOfODCtoLaunchRef --;

		if(f_numberOfODCtoLaunchRef > 1)
		{
			StartCoroutine(spawnAnim());
		}else{
			Destroy(this.gameObject);
		}
	}


}
