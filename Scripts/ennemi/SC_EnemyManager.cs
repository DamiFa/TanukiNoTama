using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_EnemyManager : MonoBehaviour {

	public List <GameObject> a_Enemies;
	public int i_FormationId;
	private int i_EnemyId;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		PlaceChild ();
	}

	void PlaceChild ()
	{
		//Gérer le placement des ennemis en fonction de a taille et du type d'ennemy
		switch(i_EnemyId)
		{
		case 1:
			for(int i = 0; i < a_Enemies.Count; i++)
			{
//				a_Enemies[i].transform.position = new Vector3(transform.position.x + Mathf.Cos(Mathf.Deg2Rad * (360 * i / a_Enemies.Count)) * 2f, 
//				                                              transform.position.y + Mathf.Sin(Mathf.Deg2Rad * (360 * i / a_Enemies.Count)) * 2f,
//				                                              transform.position.z);

				a_Enemies[i].GetComponent<SC_EnemyBehavior>()._OutDestination = new Vector3(transform.position.x + Mathf.Cos(Mathf.Deg2Rad * (360 * i / a_Enemies.Count)) * 2f, 
	                            				                                            transform.position.y + Mathf.Sin(Mathf.Deg2Rad * (360 * i / a_Enemies.Count)) * 2f,
	                            				                                            transform.position.z);
			}

			break;

		default:

			break;
		}

	}

	public void AddToFormation (GameObject EnemyToAdd)
	{
		if(a_Enemies == null)
		{
			a_Enemies = new List<GameObject>();
		}

		i_EnemyId = EnemyToAdd.GetComponent<SC_EnemyBehavior>().i_EnemyType;

		switch(i_EnemyId)
		{
		case 1:
			a_Enemies.Add(EnemyToAdd);
			EnemyToAdd.GetComponent<SC_EnemyBehavior>().i_State = 6;

			EnemyToAdd.transform.parent = this.transform;

			EnemyToAdd.GetComponent<SC_EnemyBehavior>().i_FormationId = i_FormationId;
			
			break;
			
		default:
			
			break;
		}
	}

	//Pour les types qui ne se mettent pas en formation faire les modif necessaires
}
