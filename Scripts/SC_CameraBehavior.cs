using UnityEngine;
using System.Collections;

public class SC_CameraBehavior : MonoBehaviour {

	private Vector3 _Center;
	private Vector3 _StartPos;
	private bool b_IsTilting;
	private bool b_IsFreezing;

	private GameObject[] _Players;

	private int j;
	
	private float f_posX;
	private float f_posY;
	private float f_posZ;

	private float f_MinSize = 10f;
	private float f_MaxSize = 16f;

	private Vector3 v_tiltDirection;

	private float f_timer;

	private float f_startCamSize;
	private float f_camSizeDirection;

	// Use this for initialization
	void Start () 
	{
		_Players = GameObject.FindGameObjectsWithTag("Player");

		_StartPos = transform.position;

		f_startCamSize = this.camera.orthographicSize;

		b_IsTilting = false;

		f_camSizeDirection = f_startCamSize;
	}
	
	// Update is called once per frame
	void Update () 
	{
//		SetCamPos ();

//		camera.transform.position = new Vector3(f_posX , f_posY, camera.transform.position.z);

		if(b_IsTilting)
		{
			tilt();
			f_timer += Time.deltaTime;
			if(f_timer >= 0.01f)
			{
				b_IsTilting = false;
				f_timer = 0;
				f_camSizeDirection = f_startCamSize;
			}
		}

		if(transform.position != _StartPos && b_IsTilting != true)
		{
			transform.position = Vector3.Lerp(transform.position, _StartPos, 6*Time.deltaTime);
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, f_startCamSize, 6*Time.deltaTime);

		}




	}

	private void tilt()
	{
		transform.position = Vector3.Lerp(transform.position, new Vector3 (transform.position.x +v_tiltDirection.x,transform.position.y + v_tiltDirection.y, transform.position.z + v_tiltDirection.z) , 20f*Time.deltaTime);
		camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, f_camSizeDirection, 2f*Time.deltaTime);


	}

	public void setTilt (Vector3 v_tiltDir)
	{
//		if(!b_IsTilting)
//		{
//			StartCoroutine(Tilting());
//			b_IsTilting = true;
//		}

		b_IsTilting = true;
		v_tiltDirection = v_tiltDir;

		f_camSizeDirection -= 0.5f;




//		if(!b_IsFreezing)
//		{
//			StartCoroutine(Freeze());
//			b_IsFreezing = true;
//		}
	}

	IEnumerator Tilting ()
	{
		transform.Translate(Vector3.right*0.1f);

		yield return new WaitForSeconds(0.05f);

		transform.position = _StartPos;
		Time.timeScale = 1f;

		b_IsTilting = false;

		yield break;
	}

	IEnumerator Freeze ()
	{
		Time.timeScale = 0.05f;

		yield return new WaitForSeconds(0.015f);

		Time.timeScale = 1f;

		b_IsFreezing = false;

		yield break;
	}

	private void SetCamPos ()
	{
		if(_Players.Length == 1)
		{
			f_posX = _Players[0].transform.position.x;
			f_posY = _Players[0].transform.position.y;
		}
		else if(_Players.Length == 2)
		{
			f_posX = (_Players[0].transform.position.x + _Players[1].transform.position.x) * 0.5f;
			f_posY = (_Players[0].transform.position.y + _Players[1].transform.position.y) * 0.5f;
		}
		else if(_Players.Length == 3)
		{
			f_posX = (_Players[0].transform.position.x + _Players[1].transform.position.x + _Players[2].transform.position.x) / 3f;
			f_posY = (_Players[0].transform.position.y + _Players[1].transform.position.y + _Players[2].transform.position.y) / 3f;
		}
		else if(_Players.Length == 4)
		{
			f_posX = (_Players[0].transform.position.x + _Players[1].transform.position.x + _Players[2].transform.position.x + _Players[3].transform.position.x) * 0.25f;
			f_posY = (_Players[0].transform.position.y + _Players[1].transform.position.y + _Players[2].transform.position.y + _Players[3].transform.position.y) * 0.25f;
		}
	}
}
