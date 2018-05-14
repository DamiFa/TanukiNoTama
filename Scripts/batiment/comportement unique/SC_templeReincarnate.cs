using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_templeReincarnate : MonoBehaviour {


	public ParticleSystem p_particle;
	public float f_lifeRegen;

	List<GameObject> a_DeadPlayers = new List<GameObject>();

	Color c_lifeRegenColor;

	
	int i_regenSpeedBonus;

	float f_CutOffValue;
	bool b_reincarnateIsActive = false;
	bool b_reincarnateReady = false;
	GameObject g_reincarnatingPlayer;

	SC_Player SC_PlayerRef;
	SC_playerDeath SC_playerDeathRef;
	SC_playerComboCounter SC_playerComboCounterRef;

	// Use this for initialization
	void Start () 
	{	
		f_CutOffValue = 1;
		renderer.material.SetFloat("_Cutoff", f_CutOffValue );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(f_CutOffValue > 0 && a_DeadPlayers.Count>0)
		{
			setColor();

			//this line creates a liferegen inverted bonus for longcombo
			//f_CutOffValue -= (f_lifeRegen-Mathf.Clamp((i_regenSpeedBonus*f_lifeRegen/15),0,(f_lifeRegen-(f_lifeRegen/10))))* Time.deltaTime;

			f_CutOffValue -= (f_lifeRegen)* Time.deltaTime;

			renderer.material.SetFloat("_Cutoff", f_CutOffValue );
			if( f_CutOffValue < 0) f_CutOffValue = 0;
		}
		
		if(f_CutOffValue == 0 && b_reincarnateReady == false)
		{
			b_reincarnateReady = true;
			b_reincarnateIsActive = true;
		}

		if(a_DeadPlayers.Count>0 && b_reincarnateIsActive == true)
		{
			b_reincarnateIsActive = false;
			StartCoroutine(Reincarnate());
		}
	}

	public void setColor()
	{
		if(a_DeadPlayers.Count>0)
		{
			SC_PlayerRef = a_DeadPlayers[0].GetComponent<SC_Player>();

			//a_DeadPlayers[0]

			SC_playerDeathRef = a_DeadPlayers[0].GetComponent<SC_playerDeath>();
			i_regenSpeedBonus =  SC_playerDeathRef.i_storedKillCount;

			c_lifeRegenColor = SC_PlayerRef.s_Color;
			c_lifeRegenColor.a = 0.5f;
			renderer.material.color = c_lifeRegenColor;
		}
		else
		{
			renderer.material.color = Color.white;
		}
	}

	public void addDeadPlayer(GameObject deadPlayer)
	{
		a_DeadPlayers.Add(deadPlayer);
	}

//	public void Reincarnate()
//	{
//		p_particle.startColor = SC_PlayerRef.s_Color;
//		p_particle.Emit(50);
//
//		SC_playerDeathRef = a_DeadPlayers[0].GetComponent<SC_playerDeath>();
//		SC_playerDeathRef.Reincarnate();
//		a_DeadPlayers.RemoveAt(0);	
//		f_CutOffValue = 1;
//		renderer.material.SetFloat("_Cutoff", f_CutOffValue );
//		b_reincarnateReady = false;
//		setColor();
//	}


	IEnumerator Reincarnate()
	{
		p_particle.startColor = SC_PlayerRef.s_brightColor;
		p_particle.Emit(50);

		yield return new WaitForSeconds(0.2f);

		SC_playerDeathRef = a_DeadPlayers[0].GetComponent<SC_playerDeath>();
		SC_playerDeathRef.Reincarnate();
		a_DeadPlayers.RemoveAt(0);	
		f_CutOffValue = 1;
		renderer.material.SetFloat("_Cutoff", f_CutOffValue );
		b_reincarnateReady = false;
		setColor();
	}
}
