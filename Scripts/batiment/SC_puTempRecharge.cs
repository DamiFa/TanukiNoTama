using UnityEngine;
using System.Collections;

public class SC_puTempRecharge : MonoBehaviour {

	public float f_chargeSpeed;

	bool b_isCharged;
	float f_chargeValue = 1;

	// Use this for initialization
	void Start () 
	{	

	}
	
	// Update is called once per frame
	void Update ()
	{
		if(b_isCharged == false)
		{
			charge();
		}
	

	}

	public void takePU()
	{
		if(b_isCharged == true)
		{
			//RETURN VALUE (change void to retun value) a notre player qui call cette fonction
			f_chargeValue = 1;
			renderer.material.SetFloat("_Cutoff",f_chargeValue);
			b_isCharged = false;
		}
	}

	public void charge()
	{
		if(f_chargeValue>0)
		{
			f_chargeValue -= f_chargeSpeed*Time.deltaTime;
			if(f_chargeValue<0) f_chargeValue = 0;
			renderer.material.SetFloat("_Cutoff",f_chargeValue);
		}

		if(f_chargeValue <= 0)
		{
			//Juice particle lumiere
			b_isCharged = true;
		}
	}

	public void setColor(Color color)
	{
		renderer.material.color = color;
	}




}
