using UnityEngine;
using System.Collections;

public class SC_playerComboCounter : MonoBehaviour {
	public Transform player;
	public Camera camera;
	// Use this for initialization

	public int i_killCount = 0;

	[HideInInspector]
	public int i_storedKillCount;

	SC_ScoreCounter SC_ScoreCounterRef;
	[HideInInspector]
	public bool b_isActive = true;
	Vector3 v_screenPos;
	Vector3 v_startPos;
	Vector3 v_startSize;

	Color c_startColor = Color.white;

	private bool b_IsInPuMode;
	public float f_TimeToLoosePuMode;
	private float f_StartTime;
	private float f_CurrentTime;

	[HideInInspector]
	public SC_Player SC_PlayerRef;

	public Light l_playerLamp;
	private Color c_lampColor;

	public Texture t_counterTexture;

	private float f_timerGUIpu;
	private bool b_guiSizeNormal;
	private float f_guiSizeShift = 0;

	void Start () 
	{
		b_IsInPuMode = false;
		SC_ScoreCounterRef = GameObject.FindGameObjectWithTag("scoreCounter").GetComponent<SC_ScoreCounter>();
		v_startSize = transform.localScale;
		v_startPos = transform.position;

		SC_PlayerRef = player.GetComponent<SC_Player>();

		this.guiText.color = SC_PlayerRef.s_Color;
	}
	
	// Update is called once per frame



	void Update () 
	{
		guiText.text = "x "+i_killCount.ToString();

		//c_lampColor = new Color(255, Mathf.Clamp (Mathf.Round (i_killCount*255/30),0,255),Mathf.Clamp (Mathf.Round (i_killCount*255/30),0,255), 255);

		//l_playerLamp.color = SC_PlayerRef.s_brightColor*Mathf.Clamp(i_killCount/10,0.1f,1)*10;

		l_playerLamp.intensity =  1+Mathf.Clamp(2.5f*i_killCount/20, 0,2.5f);

		//l_playerLamp.color = SC_PlayerRef.s_brightColor - Color.white/(Mathf.Clamp(i_killCount/5,0.1f,1)*10);

		v_screenPos = camera.WorldToScreenPoint(new Vector3 (player.position.x, player.position.y + 0.5f, player.position.z - 0.1f));	
		//transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z - 0.1f);

		if(b_isActive)
		{

			guiText.pixelOffset = new Vector2(v_screenPos.x, v_screenPos.y);

//			if( transform.position != v_startPos)
//			{
//				transform.position = Vector3.Lerp(transform.position, v_startPos, 2*Time.deltaTime);
//			}
		}

		if(i_killCount > 0)
		{
			if(guiText.enabled == false) guiText.enabled = true;

			guiText.text = "x " + i_killCount.ToString();

			if(b_IsInPuMode)
			{
				f_CurrentTime = Time.time - f_StartTime;

				if(f_CurrentTime > f_TimeToLoosePuMode)
				{
					i_killCount--;
					f_StartTime = Time.time;
				}

				guiText.material.color = SC_PlayerRef.s_Color;
			}
			else
			{
				guiText.material.color = c_startColor;
			}
		}
		else
		{
			guiText.enabled = false;

			if(b_isActive)
			{
				player.GetComponent<SC_Player>().b_IsInPuMode = false;
				b_IsInPuMode = false;
				b_isActive = false;
			}
		}


	}

	void OnGUI() 
	{



		if(SC_PlayerRef.b_IsDashing)
		{
			//GUI.DrawTexture(new Rect(v_screenPos.x-50, Screen.height - v_screenPos.y-50,  10, Mathf.Clamp((50*(float)i_killCount/25),0,25)), t_counterTexture);
			GUI.DrawTexture(new Rect(v_screenPos.x-30, Screen.height - v_screenPos.y-15,  60, 1), t_counterTexture);
			GUI.DrawTexture(new Rect(v_screenPos.x-30, Screen.height - v_screenPos.y-15,  Mathf.Clamp((60*(float)i_killCount/25),0,25), 5), t_counterTexture);


		}




		//GUI.DrawTexture(new Rect(Screen.width/2 - 200/2, Screen.height/20,  200, 1), t_ExpBar);  200*(float)i_ScoreForEXPbar/(float)i_ScoreToLvlUp
	}


	public void addKill(int i_ennemiScoreValue, bool b_inDash)
	{
		b_isActive = true;

		if(b_inDash && !b_IsInPuMode)
		{
			i_killCount ++;
			//transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z - 0.1f);
			SC_ScoreCounterRef.IncreaseScore(i_ennemiScoreValue * i_killCount);
		}
		else if(b_IsInPuMode)
		{
			SC_ScoreCounterRef.IncreaseScore(i_ennemiScoreValue * i_killCount);

			guiText.text = i_killCount.ToString();
			//transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z - 0.1f);
		}
		else
		{
			SC_ScoreCounterRef.IncreaseScore(i_ennemiScoreValue);
		}
	}

	public void endCount()
	{
		b_isActive = false;
		i_killCount = 0;
		guiText.enabled = false;
	}

	public void Set_inPuMode(bool pumode)
	{
		b_IsInPuMode = pumode;
	}

}
