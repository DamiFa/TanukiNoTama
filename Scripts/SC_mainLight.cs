using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_mainLight : MonoBehaviour {

	public Light[] a_flashLights;
	public Texture2D[] a_cookieFor2;
	public Texture2D[] a_cookieFor3;
	public Texture2D[] a_cookieFor4;

	public float f_timerDelay = 0.5f;
	public float f_timerFlashDelay = 0.3f;

	bool b_timerActivated;
	bool b_timerFlashActive;
	float f_timer;
	float f_flashTimer;
	int i_lightsToFlash;

	List<Color> a_colorsToFlash = new List<Color>();


	List<SC_Player> a_SC_Player = new List<SC_Player>();

	List<float> a_i_killCountStart = new List<float>();

	void Start () 
	{
	}	
	// Update is called once per frame
	void Update () 
	{
		if(a_SC_Player.Count > 0)
		{
			flashLights();
		}else
		{
			stopAllflash();
		}

//		if(b_timerActivated == true)
//		{
//			f_timer += Time.deltaTime;
//			if(f_timer >= f_timerDelay)
//			{
//				i_lightsToFlash = a_colorsToFlash.Count;
//				setLightsforFlash();
//				//setComboBoost();
//				b_timerActivated = false;
//				f_timer = 0;
//				b_timerFlashActive = true;
//			}
//		}
//
//		if(b_timerFlashActive == true)
//		{
//			flashLights();
//			f_flashTimer += Time.deltaTime;
//			if(f_flashTimer >= f_timerFlashDelay)
//			{
//		//		stopFlash();
//			}
//		}
	}

	public void setLightsforFlash()
	{
		stopAllflash();

		if(a_SC_Player.Count>0)
		{
			for(int i = 0; i < a_SC_Player.Count; i++)
			{
				if(a_SC_Player.Count == 1) a_flashLights[i].cookie = null;
				if(a_SC_Player.Count == 2) a_flashLights[i].cookie = a_cookieFor2[i];
				if(a_SC_Player.Count == 3) a_flashLights[i].cookie = a_cookieFor3[i];
				if(a_SC_Player.Count == 4) a_flashLights[i].cookie = a_cookieFor4[i];

				//a_flashLights[i].intensity = 8;
				a_flashLights[i].color = a_SC_Player[i].s_brightColor;

				a_flashLights[i].enabled = true;
			}
		}
		//a_colorsToFlash.Clear();	
	}

//	public void setComboBoost()
//	{
//		if(i_lightsToFlash > 1)
//		{
//			for(int i = 0; i< i_lightsToFlash; i++)
//			{
//				float f_puDurationTemp = a_SC_PlayersComboRef[i].f_puDurationMultiplicator;
//				a_SC_PlayersComboRef[i].f_puDurationMultiplicator += i_lightsToFlash*(0.5f*f_puDurationTemp);
//			}
//		}
//		a_SC_PlayersComboRef.Clear();
//	}
//
	public void flashLights()
	{


		for(int i = 0; i < a_SC_Player.Count; i++)
		{
			a_flashLights[i].intensity = a_SC_Player[i].SC_playerComboCounterRef.i_killCount*3/a_i_killCountStart[i];

				//Mathf.Lerp(a_flashLights[i].intensity, 0, 4*Time.deltaTime);

			if( a_SC_Player[i].SC_playerComboCounterRef.i_killCount*3/a_i_killCountStart[i] <= 0.5f)
			{
				stopFlash(a_SC_Player[i]);
			}
		}

	}

	public void stopFlash(SC_Player _SC_PlayerRef)
	{
//		foreach(SCa_Player a_SC_PlayerREF in a_SC_Player)
//		{
//			if(a_SC_PlayerREF == _SC_PlayerRef)
//			{
//
//				//a_SC_Player.Remove(a_SC_PlayerREF);
//
//
//			}
//		}
		a_flashLights[a_SC_Player.IndexOf(_SC_PlayerRef)].enabled = false;

		a_i_killCountStart.RemoveAt(a_SC_Player.IndexOf(_SC_PlayerRef));


		a_SC_Player.Remove(_SC_PlayerRef);
	
		setLightsforFlash();
	}

	private void stopAllflash()
	{
		for(int i = 0; i < 4; i++)
		{
			a_flashLights[i].enabled = false;
		}
	}


	public void addPlayerInPU (SC_Player SC_PlayerRef)
	{
		a_SC_Player.Add(SC_PlayerRef);
		a_flashLights[a_SC_Player.Count-1].intensity = 2;
		a_i_killCountStart.Add((float)SC_PlayerRef.SC_playerComboCounterRef.i_killCount);
		setLightsforFlash();


		//a_colorsToFlash.Add(flashColor);
		//if(b_timerActivated == false) b_timerActivated = true;
	}

}
