using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_GameManager : MonoBehaviour {


	public bool b_resetScore = false;

	public int score;

	private List<int> l_highScores = new List<int>();

	private int[] a_highScores;



	public GUIText[] gt_score;
	public GameObject g_scoreDisplayROOT;


	private SC_ScoreCounter SC_ScoreCouterRef;

	private int i_guiScorePos;

	private bool b_flashScore = false;
	private bool b_flashScoreIsRed = false;
	private float f_timerFlash;

	public GUITexture g_backgroundTexture;


	private GameObject[] a_Players;

	public SC_SoundManager _Sounds;

	public AudioSource _HighScoreAudioSource;

	private bool b_highScoreAchieved;

	// Use this for initialization
	void Start () 
	{
		_Sounds = GameObject.FindGameObjectWithTag("SOUND_MANAGER").GetComponent<SC_SoundManager>();

		Time.timeScale = 1f;
		g_scoreDisplayROOT.SetActive(false);

		SC_ScoreCouterRef = GameObject.FindGameObjectWithTag("scoreCounter").GetComponent<SC_ScoreCounter>();


		a_Players = GameObject.FindGameObjectsWithTag("Player");



		a_highScores = PlayerPrefsX.GetIntArray("high scores"+a_Players.Length.ToString());

		foreach(int scores in a_highScores)
		{
			l_highScores.Add(scores);
		}		

		if(l_highScores.Count < 5)
		{
			while(l_highScores.Count <5)
			{
				l_highScores.Add(1000);
			}
		}	

		l_highScores.Sort();

		while(l_highScores.Count > 5)
		{
			l_highScores.RemoveAt(0);
		}


		if(b_resetScore)
		{
			for(int i = 0;i<5;i++)
			{
				l_highScores[i] = 1000;
			}
		}

//		a_highScores = l_highScores.ToArray();		
//		PlayerPrefsX.SetIntArray("high scores", a_highScores);		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.Space))
		{
			Application.LoadLevel(Application.loadedLevel);
		}

		if(b_flashScore) flashScore();
	}

	public void flashScore()
	{
		f_timerFlash += Time.deltaTime;

		if(f_timerFlash>0.00001f)
		{
			if(b_flashScoreIsRed)gt_score[l_highScores.IndexOf(score)].color = Color.white;
			if(b_flashScoreIsRed != true) gt_score[l_highScores.IndexOf(score)].color = Color.red;
			f_timerFlash = 0;
			b_flashScoreIsRed = !b_flashScoreIsRed;
		}
	}

	public void highScoreAchieved(bool b_achieved)
	{



		if(b_achieved)
		{
			_HighScoreAudioSource.PlayOneShot(_Sounds.GetSound("HighScore"));
		}else
		{
			_HighScoreAudioSource.PlayOneShot(_Sounds.GetSound("HighScoreNotAchieved"));
		}

		_Sounds.endGame();

	}

	public void endGame()
	{
		StartCoroutine(compareScore());
	}

	IEnumerator compareScore()
	{
		yield return new WaitForSeconds(1f);

		score = SC_ScoreCouterRef.i_Score;

		while(l_highScores.Count > 5)
		{
			l_highScores.RemoveAt(0);
		}
		
		for(int i = 0; i < 5 ; i++)
		{
			if(score > l_highScores[i])
			{
				l_highScores.Add(score);
				l_highScores.Sort();
				l_highScores.RemoveAt(0);

				a_highScores = l_highScores.ToArray();
				PlayerPrefsX.SetIntArray("high scores"+a_Players.Length.ToString(), a_highScores);

				b_flashScore = true;

		
				b_highScoreAchieved = true;
				//i_guiScorePos = i;
				break;
			}
		}

		highScoreAchieved(b_highScoreAchieved);

		displayScore();
		Time.timeScale = 0.0001f;
	}

	public void displayScore()
	{
		g_scoreDisplayROOT.SetActive(true);
		g_backgroundTexture.color = SC_ScoreCouterRef.g_LifeEdge.renderer.material.GetColor("_Emission");
		gt_score[1].guiText.text = score.ToString();

		for(int i = 0; i < 5; i++)
		{
			if(l_highScores[i]<1000)gt_score[i].text = l_highScores[i].ToString();  
			if(l_highScores[i]>999 && l_highScores[i] < 9999)gt_score[i].text = l_highScores[i].ToString("0 000");  
			if(l_highScores[i]>9999 && l_highScores[i] < 99999)gt_score[i].text = l_highScores[i].ToString("00 000");  
			if(l_highScores[i]>99999 && l_highScores[i] < 999999)gt_score[i].text = l_highScores[i].ToString("000 000");  
			if(l_highScores[i]>999999)gt_score[i].text = l_highScores[i].ToString("0 000 000"); 

			if(gt_score[i].text == score.ToString()) i_guiScorePos = i;
		}
	}
}
