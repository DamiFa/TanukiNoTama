using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_Couche : MonoBehaviour {

	public GameObject _Baby;
	public int _NbBaby;
	private List <GameObject> a_Couches;
	public float f_DistanceFromeCore;
	public Collider[] a_PlayersAround;
	public float f_DistanceDetect;

	// Use this for initialization
	void Start () 
	{
		for(int i = 0; i < _NbBaby; i++)
		{
			GameObject _Temp = Instantiate(_Baby, new Vector3(transform.position.x + f_DistanceFromeCore * i+1,
			                                                  transform.position.y,
			                                                  transform.position.z), Quaternion.identity) as GameObject;
			_Temp.transform.parent = this.transform;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//les faire tourner autour du coeur
		a_PlayersAround = Physics.OverlapSphere(transform.position, f_DistanceDetect);

		for (int i = 0; i < a_PlayersAround.Length; i ++)
		{
			if (a_PlayersAround[i].CompareTag("Player")) 
			{
				LookToPlayer(a_PlayersAround[i].gameObject);
			}
		}
	}

	public void AddBaby ()
	{
		//ajouter dans le tableau
	}

	private void LookToPlayer (GameObject _PlayerToLookTo)
	{
		//Lancé quand un joueur rentre dans la zone de l'ennemi

		//Faire regarder le denière boule de la list qui n'est pas déja en train de ragrder

		//garder un index du nombre de boule qui regarde en fonction du nombre de joueur autour (je ne sais pas encore trop comment faire ça ...)
	}

	//La première boule regarde le joueur le plus proche
	//La deuxième le deuxième joueur plus proche
	//...
}
