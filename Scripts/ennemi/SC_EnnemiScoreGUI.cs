using UnityEngine;
using System.Collections;

public class SC_EnnemiScoreGUI : MonoBehaviour {

	public Camera _camera;
	// Use this for initialization
	
	public int i_scoreValue;

	Vector3 v_screenPos;
	Vector3 v_startPos;
	Vector3 v_startSize;
	
	Color c_startColor = Color.white;


	private float f_alpha = 1;

	private bool b_isActive = false;

	public Transform g_ennemi;
	
	void Start () 
	{
		_camera = Camera.main;

		//b_IsInPuMode = false;
		//SC_ScoreCounterRef = GameObject.FindGameObjectWithTag("scoreCounter").GetComponent<SC_ScoreCounter>();
		//v_startSize = transform.localScale;
		v_startPos = transform.position;

		guiText.fontSize += Mathf.Clamp((Mathf.RoundToInt(i_scoreValue/50)*4),1,60); 

		StartCoroutine(waitBeforeStart());
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(b_isActive)
		{
			//(i_killCount);

			if(g_ennemi == null) 
			{
				Destroy(gameObject);
			}
			else
			{

			
				//to add
				//v_screenPos = _camera.WorldToScreenPoint(g_ennemi.transform.position);	
				//guiText.pixelOffset = new Vector2(v_screenPos.x, v_screenPos.y);

				//f_alpha -= 0.5f*Time.deltaTime;
				//if(f_alpha <= 0) Destroy(gameObject);

				//c_startColor.a = f_alpha;
				//guiText.color = c_startColor;
			}
		}
	}

	IEnumerator waitBeforeStart()
	{
		yield return new WaitForSeconds(0.02f);

		//to add
		v_screenPos = _camera.WorldToScreenPoint(g_ennemi.transform.position);	
		guiText.pixelOffset = new Vector2(v_screenPos.x, v_screenPos.y);

		guiText.text = "+ " + i_scoreValue.ToString();		

		b_isActive = true;
	}



	
}
