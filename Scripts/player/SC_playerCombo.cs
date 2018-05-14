using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_playerCombo : MonoBehaviour {

	public int i_DashingKillCount1;
	int i_DashingKillCount2;

	//Dashkill Boost
	public int i_KillBeforeBonus = 2;
	public int i_KillBeforeComboBonus = 3;
	public float _BoostAdd;
	public float f_PUcomboDuration;
	public float f_puDurationMultiplicator = 0;

	float _BoostAddTotal;
	public bool b_comboOn = false;
	Color c_Color;
	Light l_lifeLamp;
	float f_puTimer;

	public SC_GetColorSprite SC_lifeLampRef;
	SC_Player SC_PlayerRef;
	SC_playerDeath SC_playerDeathRef;
	SC_dashTrail SC_dashTrailRef;
	SC_mainLight SC_mainLightRef;
	SC_playerCombo SC_playerComboRef;

	public SC_Trigger SC_TriggerRef;
	Vector3 v_HitDir;

	void Start () 
	{
		SC_playerComboRef = this.GetComponent<SC_playerCombo>();

		l_lifeLamp = this.transform.FindChild("LifeLamp").light;
		SC_lifeLampRef = l_lifeLamp.GetComponent<SC_GetColorSprite>();

		SC_mainLightRef = GameObject.Find("flashLight_root").GetComponent<SC_mainLight>();

		SC_dashTrailRef = this.GetComponent<SC_dashTrail>();
		SC_playerDeathRef = this.GetComponent<SC_playerDeath>();
		SC_PlayerRef = this.GetComponent<SC_Player>();

		c_Color = SC_PlayerRef.s_Color;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(b_comboOn == true)
		{
			f_puTimer += Time.deltaTime;
			if(f_puTimer >= f_PUcomboDuration * f_puDurationMultiplicator)
			{
				endPUcombo();
			}
			//Activate COMBO special POWERUP (devrait etre lancer une fois donc pas le bon endroit ici
			//ex: SC_powerUpRef.enabled = true
			// comment on va les incrémenter? meh
		}
	}

	public void resetKillCount()
	{	
		_BoostAddTotal = 0;
		i_DashingKillCount1 = 0;
		i_DashingKillCount2 = 0;
		f_puDurationMultiplicator = 0;
		endPUcombo();
	}

	public void addKillCount(int addKillValue)
	{
		i_DashingKillCount1 += addKillValue;	
		//BOOSTvar for each dash kill
		if(i_DashingKillCount1%i_KillBeforeBonus == 0)
		{
//			if(_BoostAddTotal>0) SC_PlayerRef.removeComboBoost(_BoostAddTotal);
			_BoostAddTotal += _BoostAdd;
//			SC_PlayerRef.comboBoost(_BoostAddTotal);
		}
		if(b_comboOn == false)
		{		
			i_DashingKillCount2 += addKillValue;	
			// INITIATE SPECIAL COMBO ATTACK
			if(i_DashingKillCount2%i_KillBeforeComboBonus == 0)
			{
				f_puDurationMultiplicator ++;
				startPUcombo();
			}
		}
	}

	public void startPUcombo()
	{
	//	SC_mainLightRef.comboColorFlash(c_Color);		
		b_comboOn = true;
	//	SC_dashTrailRef.b_comboAttack = true;		
		
		//TEMPORARY TO SEE IF COMBO IS ACTIVE
	//	SC_lifeLampRef._CurrentColor = Color.magenta;
	//	SC_lifeLampRef.setCurrentColor();
	}

	public void endPUcombo()
	{
	//	SC_lifeLampRef.setBackColor();
	//	SC_dashTrailRef.b_comboAttack = false;
		b_comboOn = false;
		f_puTimer = 0;
	}



}
