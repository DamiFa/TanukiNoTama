using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_ScoreCounter : MonoBehaviour {

	public int i_Score = 0;
	public GUIText _ScoreText;
	public GameObject g_Spawner;

	int i_player1Kill;
	int i_player2Kill;
	int i_player3Kill;
	int i_player4Kill;

	
	private int i_scoreToAdd;
	bool b_addingPoints = false;
	float f_addPtsSpeed = 0.1f;

	Color c_player1color;
	Color c_player2color;
	Color c_player3color;
	Color c_player4color;

	GameObject[] a_Players;

	public GameObject g_LifeEdge;
	public Light l_LifeCircleLamp;

	int i_bestPlayer;

	public Texture t_ExpBar;
	public int i_ScoreToLvlUp = 1000;

	private float i_ScoreForEXPbar;

	public SC_BatimentManager SC_BatimentManagerRef;

	private float i_currentLevel = 0f;

	private GameObject[] a_Batiment;

	// Use this for initialization

	public Light l_lvlUpLight;
	
	private float f_timerAnim = 0;
	
	//TEMPORARY
	int i_lvlUpTotal = 1;
	
	
	private bool b_lvlUpAnim;
	
	public ParticleSystem[] p_LvlUpParticles;

	public Animator _tanukiAnimator;
	public GUIText g_lvlIndicator;

	public AudioSource _LvlUPAudioSource;
	[HideInInspector]
	public SC_SoundManager _Sounds;

	private float f_timerForLvlUpSound;

	void Start () 
	{
		i_Score = 0;
		
		transform.tag = "scoreCounter";

		_Sounds = GameObject.FindGameObjectWithTag("SOUND_MANAGER").GetComponent<SC_SoundManager>();


		a_Players = GameObject.FindGameObjectsWithTag("Player");
		for(int i = 0; i < a_Players.Length; i++)
		{
			if(a_Players[i].name == "Player1") c_player1color = a_Players[i].GetComponent<SC_Player>().s_brightColor;
			if(a_Players[i].name == "Player2") c_player2color = a_Players[i].GetComponent<SC_Player>().s_brightColor;
			if(a_Players[i].name == "Player3") c_player3color = a_Players[i].GetComponent<SC_Player>().s_brightColor;
			if(a_Players[i].name == "Player4") c_player4color = a_Players[i].GetComponent<SC_Player>().s_brightColor;
		}


		g_lvlIndicator.pixelOffset =  new Vector2(Screen.width/2 - 200/1.3f, Screen.height-Screen.height/20);

		calulateNexLVL();

	}	
	// Update is called once per frame
	void Update ()
	{

		f_timerForLvlUpSound += Time.deltaTime;

		if(i_scoreToAdd>0)
		{
			guiText.pixelOffset = new Vector2(0,0);
			addScore();
		}

		if(i_ScoreForEXPbar >= i_ScoreToLvlUp && SC_BatimentManagerRef.b_ConditionTemp != true)
		{
			
			l_lvlUpLight.enabled = true;
			l_lvlUpLight.intensity = 4;
			
			b_lvlUpAnim = true;		
			f_timerAnim = 0;

			//if(f_timerForLvlUpSound < 34f)_LvlUPAudioSource.PlayOneShot(_Sounds.GetSound("lvlUp1"));

			_LvlUPAudioSource.PlayOneShot(_Sounds.GetLVLupSound());

			_tanukiAnimator.Play("batimentCentral_lvlUp");

			for(int i = 0; i <  a_Players.Length+1; i++)
			{
				p_LvlUpParticles[i].Play();
			}

			i_currentLevel ++;

			g_lvlIndicator.text = "lvl "+i_currentLevel.ToString();

			i_ScoreForEXPbar = 0;	
			calulateNexLVL();
			SC_BatimentManagerRef.b_ConditionTemp = true;
		}

		if(b_lvlUpAnim)lvlUpAnim();
	}


	public void lvlUpAnim()
	{
		l_lvlUpLight.intensity = Mathf.Lerp(l_lvlUpLight.intensity,0,0.5f*Time.deltaTime);
		f_timerAnim += Time.deltaTime;

		if(f_timerAnim>=1f)
		{
			l_lvlUpLight.enabled = false;
			b_lvlUpAnim = false;
			f_timerAnim = 0;
			_tanukiAnimator.Play("batimentCentral_idle");
		}
	}


	private void calulateNexLVL()
	{
		//i_ScoreToLvlUp = Mathf.RoundToInt(1000*(((Mathf.Pow(i_currentLevel,4f))/50)+0.5f));

		a_Batiment = GameObject.FindGameObjectsWithTag("batiment");

		//	i_ScoreToLvlUp = Mathf.RoundToInt((1000-(a_Batiment.Length*25))*a_Batiment.Length/a_Players.Length)
		if(a_Batiment.Length-2>0)
		{
			//i_ScoreToLvlUp = Mathf.RoundToInt((1000-(a_Batiment.Length*25))* 0.1f*Mathf.Pow((a_Batiment.Length/a_Players.Length),2));
			i_ScoreToLvlUp = Mathf.RoundToInt((1000-(a_Batiment.Length-2*25))*(a_Batiment.Length-2/a_Players.Length));

		}else
		{
			i_ScoreToLvlUp = 1000;
		}

	}



	void OnGUI() 
	{
		GUI.DrawTexture(new Rect(Screen.width/2 - 200/2, Screen.height/20,  200*(float)i_ScoreForEXPbar/(float)i_ScoreToLvlUp, 5), t_ExpBar);

		GUI.DrawTexture(new Rect(Screen.width/2 - 200/2, Screen.height/20,  200, 1), t_ExpBar);
	}


	private void addScore()
	{
		if(i_scoreToAdd > 0 && i_scoreToAdd <=50)
		{
			i_ScoreForEXPbar ++;
			i_Score += 1;
			i_scoreToAdd -= 1;
			displayScoreText();
			return;
		}
		if(i_scoreToAdd > 50 && i_scoreToAdd <=100)
		{
			i_ScoreForEXPbar += 10;
			i_Score += 10;
			i_scoreToAdd -= 10;

			displayScoreText();

			guiText.pixelOffset = new Vector2(Random.Range(-3,3),Random.Range(-1,1));

			return;
		}
		if(i_scoreToAdd > 100 && i_scoreToAdd <=500)
		{
			i_ScoreForEXPbar += 50;
			i_Score += 50;
			i_scoreToAdd -= 50;

			displayScoreText();

			guiText.pixelOffset = new Vector2(Random.Range(-3,3),Random.Range(-2,2));

			return;
		}
		if(i_scoreToAdd > 500 && i_scoreToAdd <=1000)
		{
			i_ScoreForEXPbar += 100;
			i_Score += 100;
			i_scoreToAdd -= 100;

			displayScoreText();

			guiText.pixelOffset = new Vector2(Random.Range(-3,3),Random.Range(-3,3));

			return;
		}

		if(i_scoreToAdd>1000)
		{
			i_ScoreForEXPbar += 500;
			i_Score += 500;
			i_scoreToAdd -= 500;

			displayScoreText();
			
			guiText.pixelOffset = new Vector2(Random.Range(-4,4),Random.Range(-4,4));
			
			return;
		}
	}


	public void displayScoreText()
	{
		if(i_Score<1000)_ScoreText.text = i_Score.ToString();  
		if(i_Score>999 && i_Score < 9999)_ScoreText.text = i_Score.ToString("0 000");  
		if(i_Score>9999 && i_Score< 99999)_ScoreText.text = i_Score.ToString("00 000");  
		if(i_Score>99999 && i_Score < 999999)_ScoreText.text = i_Score.ToString("000 000");  
		if(i_Score>999999)_ScoreText.text = i_Score.ToString("0 000 000"); 
	}



	public void IncreaseScore(int x)
	{
		i_scoreToAdd += x;
	}

	public void IncreaseScoreForPlayer(int player)
	{
		if(player == 1) i_player1Kill++;
		if(player == 2) i_player2Kill++;
		if(player == 3) i_player3Kill++;
		if(player == 4) i_player4Kill++;
		CompareScore();
	}

	public void ResetScoreForPlayer(int player)
	{
//		if(player == 1) i_player1Kill = 0;
//		if(player == 2) i_player2Kill = 0;
//		if(player == 3) i_player3Kill = 0;
//		if(player == 4) i_player4Kill = 0;
//		CompareScore();
	}

	public void CompareScore()
	{
		if(i_player1Kill > i_player2Kill && i_player1Kill > i_player3Kill && i_player1Kill > i_player4Kill) g_LifeEdge.renderer.material.SetColor("_Emission", c_player1color);
		if(i_player2Kill > i_player1Kill && i_player2Kill > i_player3Kill && i_player2Kill > i_player4Kill) g_LifeEdge.renderer.material.SetColor("_Emission", c_player2color);
		if(i_player3Kill > i_player2Kill && i_player3Kill > i_player1Kill && i_player3Kill > i_player4Kill)	g_LifeEdge.renderer.material.SetColor("_Emission", c_player3color);
		if(i_player4Kill > i_player2Kill && i_player4Kill > i_player3Kill && i_player4Kill > i_player1Kill)	g_LifeEdge.renderer.material.SetColor("_Emission", c_player4color);

	}

}
