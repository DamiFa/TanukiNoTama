using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_SpawnerManager : MonoBehaviour {

	[System.Serializable]
	public class Ennemis
	{
		public GameObject _Ennemi;
		public AnimationCurve _Curve;
	}

	//Spawner
	public GameObject _EnemyManager;
	public Transform[] _SpawnersLeft;
	public Transform[] _SpawnersRight;
	public Transform[] _SpawnersTop;
	public Transform[] _SpawnersBot;
	private int i_ToSpawn;
	private Transform _CurrentSpawer;
	private int _NbEnnemiMap;
	public int _NbEnnemiNeeded = 0;
	private float f_CurrentTime;
	private float f_StartTime;
	private GameObject[] a_NbEnnemiMap;
	public Ennemis[] a_Ennemi;
	private bool _isSpawning;
	private float f_tot;
	private float secureTot;
	private GameObject _CurrentEnnemi;
	private float[] a_ChancesToAppear;
	private List<GameObject> a_EnnemisToSpawn;
	private int j;

	//public GUIText guiScore;


	//Enemy manager
	private List <GameObject> a_EnnemiesSpawned;
	private int i_FormIdToGive;
	
	// Use this for initialization
	void Start () 
	{
		i_ToSpawn = 3;
		f_StartTime = Time.time;
		_isSpawning = true;

		a_EnnemiesSpawned = new List<GameObject>();

		i_FormIdToGive = 1;
		
		StartCoroutine("Spawning");
	}

	// Update is called once per frame
	void Update () 
	{
		f_CurrentTime =  Time.time - f_StartTime;

		for(int i = 0; i < a_EnnemiesSpawned.Count; i++)
		{
			if(a_EnnemiesSpawned[i] == null)
			{
				a_EnnemiesSpawned.RemoveAt(i);
			}
		}
	}

	void FixedUpdate ()
	{
		if(a_EnnemiesSpawned.Count > 1)
		{
//			CheckEnemiesDistance();
		}
	}
	
	int DeltaEnnemi()
	{
//		((0.3*x+sin(x*0.1)*8)^1.3)*0.05+5
//		((0.5*x+sin(x)*0.7)^1.3)*0.5+5 (Actuelle)
//		_NbEnnemiNeeded = Mathf.FloorToInt((Mathf.Pow((0.5f*f_CurrentTime+Mathf.Sin(f_CurrentTime)*0.7f),1.3f)*0.5f+5f));
//		(sin(x*0.8)*(x*0.8)^0.6)+5 // _NbEnnemiNeeded = Mathf.Max(9, Mathf.FloorToInt(Mathf.Sin(f_CurrentTime*0.8f)*Mathf.Pow((f_CurrentTime*0.8f), 0.6f)+5));
//		(sin(x*0.8)*(x^0.6)*1.5+5) // _NbEnnemiNeeded = Mathf.Max(9, Mathf.FloorToInt(Mathf.Sin(f_CurrentTime*0.8f)*(f_CurrentTime*0.8f) * 1.5f + 5));

		//_NbEnnemiNeeded = Mathf.Max(8, Mathf.FloorToInt(Mathf.Sin(f_CurrentTime*0.22f)*(f_CurrentTime*0.8f) * 1.5f + 5));

		//_NbEnnemiNeeded = Mathf.Clamp(Mathf.FloorToInt(f_CurrentTime*0.22f)*(f_CurrentTime*0.8f) * 1.5f + 5),5, 15);
		_NbEnnemiNeeded = 10;
		a_NbEnnemiMap = GameObject.FindGameObjectsWithTag("Ennemi");
		_NbEnnemiMap = a_NbEnnemiMap.Length;
		
		return _NbEnnemiNeeded - _NbEnnemiMap;
	}
	
	IEnumerator Spawning()
	{
		while(_isSpawning)
		{
			if(DeltaEnnemi() > 0)
			{
				GameObject ennemiTemp = Instantiate(ChooseEnemy(), ChooseSpawner().position, Quaternion.identity) as GameObject;
				ennemiTemp.transform.position = new Vector3(ennemiTemp.transform.position.x,ennemiTemp.transform.position.y, -0.35f);
				a_EnnemiesSpawned.Add(ennemiTemp);
				
				yield return new WaitForSeconds(0.2f);
			}
			else
			{
				yield return null;
			}
		}
	}
	
	private Transform ChooseSpawner ()
	{
		if(i_ToSpawn % 4 == 0)
		{
			int i = Random.Range(0, _SpawnersLeft.Length);
			_CurrentSpawer = _SpawnersLeft[i];
		}
		else if(i_ToSpawn % 4 == 1)
		{
			int i = Random.Range(0, _SpawnersRight.Length);
			_CurrentSpawer = _SpawnersRight[i];
		}
		else if(i_ToSpawn % 4 == 2)
		{
			int i = Random.Range(0, _SpawnersTop.Length);
			_CurrentSpawer = _SpawnersTop[i];
		}
		else if(i_ToSpawn % 4 == 3)
		{
			int i = Random.Range(0, _SpawnersBot.Length);
			_CurrentSpawer = _SpawnersBot[i];
		}

		i_ToSpawn++;

		return _CurrentSpawer;
	}

	public GameObject ChooseEnemy ()
	{
		a_ChancesToAppear = new float[a_Ennemi.Length];
		a_EnnemisToSpawn = new List<GameObject>();

		_CurrentEnnemi = a_Ennemi[a_Ennemi.Length-1]._Ennemi;

		f_tot = 0;
		secureTot = 0;
		for(int i = 0; i < a_Ennemi.Length; i++)
		{
			f_tot += a_Ennemi[i]._Curve.Evaluate(f_CurrentTime/180);
		}

		for(int i = 0; i < a_Ennemi.Length; i++)
		{
			a_ChancesToAppear[i] = Mathf.Max(0, Mathf.Round((a_Ennemi[i]._Curve.Evaluate(f_CurrentTime/180) / f_tot)*100));

			secureTot += a_ChancesToAppear[i];
		}

		for(int i = 0; i < a_ChancesToAppear.Length; i++)
		{
			for(int k = 0; k < a_ChancesToAppear[i]; k++)
			{
				a_EnnemisToSpawn.Add(a_Ennemi[i]._Ennemi);
			}
		}

		_CurrentEnnemi = a_EnnemisToSpawn[Random.Range(0, a_EnnemisToSpawn.Count)];

		return _CurrentEnnemi;
	}

	//Enemy Manager
	public void CheckEnemiesDistance ()
	{
		for(int i = 0; i < a_EnnemiesSpawned.Count; i++)
		{
			if(i != a_EnnemiesSpawned.Count-1)
			{
				for(int j = i+1; j < a_EnnemiesSpawned.Count; j++)
				{
					if(Vector3.Distance(a_EnnemiesSpawned[i].transform.position, a_EnnemiesSpawned[j].transform.position) < 5f)
					{
						if(a_EnnemiesSpawned[i].GetComponent<SC_EnemyBehavior>().i_EnemyType == a_EnnemiesSpawned[j].GetComponent<SC_EnemyBehavior>().i_EnemyType)
						{
							if((a_EnnemiesSpawned[i].GetComponent<SC_EnemyBehavior>().i_FormationId != a_EnnemiesSpawned[j].GetComponent<SC_EnemyBehavior>().i_FormationId) || (a_EnnemiesSpawned[i].GetComponent<SC_EnemyBehavior>().i_FormationId == 0 || a_EnnemiesSpawned[j].GetComponent<SC_EnemyBehavior>().i_FormationId == 0))
							{
								LinkEnemies(a_EnnemiesSpawned[i], a_EnnemiesSpawned[j]);
							}
						}
					}
				}
			}
		}
	}

	public void LinkEnemies (GameObject _Enemy1, GameObject _Enemy2)
	{
		//Je regarde s'ils ne sont pas déja les 2 dans une formation
			//Si oui
				//Je supprime la première formation et je mets les ennemies dans la deuxième formation
			//Si non 
				//je regarde si le premier est dans une formation
					//J'ajoute le deuxième à la formation du premier
				//Je regarde si le premier est dans une formation
					//J'ajoute le deuxième à la formation du premier
				//Sinon
					//Je crée une nouvelle formation

		if(_Enemy1.GetComponent<SC_EnemyBehavior>().i_FormationId != 0 && _Enemy2.GetComponent<SC_EnemyBehavior>().i_FormationId != 0)
		{
			
		}
		else
		{
			if(_Enemy1.GetComponent<SC_EnemyBehavior>().i_FormationId != 0)
			{
				//Enemy1 est dans une formation
				if(_Enemy1.transform.parent.GetComponent<SC_EnemyManager>().a_Enemies.Count < 3)
				{
					_Enemy1.transform.parent.GetComponent<SC_EnemyManager>().AddToFormation(_Enemy2);
				}
			}
			else if(_Enemy2.GetComponent<SC_EnemyBehavior>().i_FormationId != 0)
			{
				if(_Enemy1.transform.parent.GetComponent<SC_EnemyManager>().a_Enemies.Count < 3)
				{
					_Enemy2.transform.parent.GetComponent<SC_EnemyManager>().AddToFormation(_Enemy1);
				}
			}
			else
			{
				GameObject _EMTemp;

				_EMTemp = Instantiate(_EnemyManager, _Enemy1.transform.position, Quaternion.identity) as GameObject;

				_EMTemp.GetComponent<SC_EnemyManager>().i_FormationId = i_FormIdToGive;

				_EMTemp.GetComponent<SC_EnemyManager>().AddToFormation(_Enemy1);
				_EMTemp.GetComponent<SC_EnemyManager>().AddToFormation(_Enemy2);

				i_FormIdToGive ++;
			}
		}
	}
}
