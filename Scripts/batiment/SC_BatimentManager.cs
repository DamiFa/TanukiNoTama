using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_BatimentManager : MonoBehaviour {

	[System.Serializable]
	public class Batiment
	{
		public string _Name = "noName";
		public GameObject _Batiment;
		public GameObject _Choix;
		private int i_PlayerId;
		
		public Batiment ()
		{
			
		}
		
		public string GetName ()
		{
			return _Name;
		}
		
		public GameObject GetBatmiment()
		{
			return _Batiment;
		}
		
		public GameObject GetChoix()
		{
			return _Choix;
		}
	}
	
	public bool b_ConditionTemp;
	private bool b_HasGivenChoice;
	private GameObject[] a_Players;
	public Batiment[] a_BatimentList;
	private List <GameObject> a_BatimentPlaced;
	private SC_ScoreCounter _ScoreCounter;
	private SC_SoundManager _Sounds;
	public AudioSource _BatimentAudioSource;




	// Use this for initialization
	void Start () 
	{
		_Sounds = GameObject.FindGameObjectWithTag("SOUND_MANAGER").GetComponent<SC_SoundManager>();

		this.tag = "BATIMENT_MANAGER";
		a_Players = new GameObject[GameObject.FindGameObjectsWithTag("Player").Length];
		_ScoreCounter = GameObject.FindGameObjectWithTag("scoreCounter").GetComponent<SC_ScoreCounter>();


		for(int i = 0; i < a_Players.Length; i++)
		{
			for(int j = 0; j < a_Players.Length; j++)
			{
				if((int)GameObject.FindGameObjectsWithTag("Player")[j].GetComponent<SC_Player>().playerIndex == i+1)
				{
					a_Players[i] = GameObject.FindGameObjectsWithTag("Player")[j];
				}
			}
		}

		a_BatimentPlaced = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(b_ConditionTemp)
		{
			if(!b_HasGivenChoice) 
			{
				StartCoroutine("GiveChoice");
				b_HasGivenChoice = true;
				b_ConditionTemp = false;
//				PlaceBatiment (ChooseBatiment(), 1); //Pour tester le placement plus vite


			}
		}

		for(int i = 0; i < a_BatimentPlaced.Count; i++)
		{
			if(a_BatimentPlaced[i] == null)
			{
				a_BatimentPlaced.RemoveAt(i);
			}
		}
	}

	public void PlaceBatiment (string s_BatimentName, int _Player, SC_BatimentChoice SC_BatimentChoiceRef)
	{
		Vector3 _posTemp;
		int securityLoop = 3000;

		float _Eloignement = 2.9f; //augment de +1 à chaque fois que
		float _NbTest = 0; //reduit à chaque test et est reinitialisé avec le nouveau NbAutour
		float _test = 1;
		float _NbAutour = Mathf.Round((Mathf.Log10(_test)+15)*_test);

		do{
			_posTemp = new Vector3(transform.position.x + Mathf.Cos(Mathf.Deg2Rad * (360 * _NbTest / _NbAutour)) * _Eloignement *1.7f,
			                       transform.position.y + Mathf.Sin(Mathf.Deg2Rad * (360 * _NbTest / _NbAutour)) * _Eloignement,
			                       transform.position.z);

			_NbTest ++;

			securityLoop --;

			if(_NbTest > _NbAutour)
			{
				_test ++;
				_NbAutour = Mathf.Round((Mathf.Log10(_test)+13)*_test*0.6f);
				_Eloignement = _Eloignement +1.2f;
				_NbTest = 0;
			}
		}while(IsBatAround(_posTemp) && securityLoop > 0);

		_BatimentAudioSource.PlayOneShot(_Sounds.GetSound("ChoiceMade"));
		GameObject _Temp = Instantiate(GetBatBatiment(s_BatimentName),_posTemp, Quaternion.identity) as GameObject;

		a_BatimentPlaced.Add(_Temp);

		_Temp.GetComponent<SC_batimentVie>().s_BatType = s_BatimentName;
		_Temp.GetComponent<SC_batimentVie>().SetPlayer(a_Players[_Player -1]);

		SC_BatimentChoiceRef.t_particleDestination = _Temp.transform;

	}

//	private bool ConditionMet ()
//	{
////		//Est true pour 1 frame
////
//////		if((_ScoreCounter.i_Score % 10*i_lvlUpTotal) == 0)
//////		{
//////			//SOLUTION TEMPORAIRE POUR EVITER LA REPITION DU CHOIX
//////			_ScoreCounter.i_Score ++;
//////
//////			i_lvlUpTotal++;
//////			b_ConditionTemp = true;
//////		}
////
//		return b_ConditionTemp;
//	}

	IEnumerator GiveChoice ()
	{
		for(int i = 0; i < a_Players.Length; i++)
		{
			if(!a_Players[i].GetComponent<SC_BatimentChoice>().b_ChoiceGiven)
			{
				//Instancier le choix
				a_Players[i].GetComponent<SC_BatimentChoice>().GiveChoice(ChooseBatiment(), ChooseBatiment());
			}
		}

		_BatimentAudioSource.PlayOneShot(_Sounds.GetSound("ChoiceGiven"));

		yield return new WaitForEndOfFrame();

		b_HasGivenChoice = false;

		yield break;
	}

	private string ChooseBatiment ()
	{
		string _BatNameTemp;
		
		_BatNameTemp = a_BatimentList[Random.Range(0, a_BatimentList.Length)].GetName();
		
		return _BatNameTemp;
	}

	public GameObject GetBatChoice (string s_BatimentName)
	{
		GameObject _BatTemp = null;

		for(int i = 0; i < a_BatimentList.Length; i++)
		{
			if(a_BatimentList[i].GetName() == s_BatimentName)
			{
				_BatTemp = a_BatimentList[i].GetChoix();
			}
		}

		return _BatTemp;
	}

	public GameObject GetBatBatiment (string s_BatimentName)
	{
		GameObject _BatTemp = null;
		
		for(int i = 0; i < a_BatimentList.Length; i++)
		{
			if(a_BatimentList[i].GetName() == s_BatimentName)
			{
				_BatTemp = a_BatimentList[i].GetBatmiment();
			}
		}
		
		return _BatTemp;
	}

	private bool IsBatAround (Vector3 _ThisPos)
	{
		bool _ThereIsBatAround = false;

		for(int i = 0; i < a_BatimentPlaced.Count; i++)
		{
			if(Vector3.Distance(_ThisPos, a_BatimentPlaced[i].transform.position) < 0.1f)
			{
				_ThereIsBatAround = true;
			}
		}

		return _ThereIsBatAround;
	}
}
