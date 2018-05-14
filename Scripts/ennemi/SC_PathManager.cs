using UnityEngine;
using System.Collections;


public class SC_PathManager : MonoBehaviour {

	[System.Serializable]
	public class EnemyPath
	{
		public string _Name = "noName";
		public bool b_IsVisible = true;
		public Transform[] a_Path;


		public EnemyPath ()
		{

		}

		public string GetName ()
		{
			return _Name;
		}

		public Transform[] GetPath()
		{
			return a_Path;
		}

		public bool IsVisible()
		{
			return b_IsVisible;
		}


	}

	public EnemyPath[] a_EnemyPathes;


	// Use this for initialization
	void Start () 
	{
		this.gameObject.tag = "PATH_MANAGER";
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public Transform[] GetEnemyPath (string _Name)
	{
		Transform[] path = null;

		for(int i = 0; i < a_EnemyPathes.Length; i++)
		{
			if(a_EnemyPathes[i].GetName() == _Name)
			{
				path = a_EnemyPathes[i].GetPath();
			}
		}

		return path;
	}

	void OnDrawGizmos()
	{
		if(a_EnemyPathes.Length > 0)
		{
			for(int i = 0; i < a_EnemyPathes.Length; i++)
			{
				if(a_EnemyPathes[i].IsVisible()){
					iTween.DrawPath(a_EnemyPathes[i].GetPath(), new Color(i/a_EnemyPathes.Length, 0.5F, 0.5F));
				}
			}
		}
	}
}
