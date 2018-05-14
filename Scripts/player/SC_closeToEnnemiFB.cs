using UnityEngine;
using System.Collections;

public class SC_closeToEnnemiFB : MonoBehaviour {

	public Collider[] a_CloseForHitEnemies;
	

	public bool b_inEnnemyZone = false;	

	Light l_lifeLamp;
	float f_attackDist = 2.5f;
	Color c_startColor;
	int i_EnnemyCount;

	SC_GetColorSprite SC_lifeLampRef;
	
	// Use this for initialization
	void Start () 
	{
//		l_lifeLamp = transform.root.Find("LifeLamp").light;
//		SC_lifeLampRef = l_lifeLamp.GetComponent<SC_GetColorSprite>();	
//		
//		c_startColor = this.GetComponent<SC_Player>().s_Color;
	}
	
	// Update is called once per frame
	void Update () 
	{	

	}
	
	public void inEnnemyZone()
	{
//		a_CloseForHitEnemies = Physics.OverlapSphere(transform.position, f_attackDist);
//		
//		foreach(Collider ennemy in a_CloseForHitEnemies)
//		{
//			if(ennemy.tag == "Ennemi")
//			{
//				if(b_inEnnemyZone == false)
//				{
//					b_inEnnemyZone = true;
//					StartCoroutine(inEnnemyZoneFlash());
//				}
//				i_EnnemyCount ++;
//			}
//		}
//		
//		if(i_EnnemyCount == 0) b_inEnnemyZone = false;
//		
//		
//		
//		i_EnnemyCount = 0;		
//		
	}
	
//	IEnumerator inEnnemyZoneFlash()
//	{
////		closeByFB();
////		yield return new WaitForSeconds(0.1f);
////		noFB();
////		if(b_inEnnemyZone == true)
////		{
////			StartCoroutine(inEnnemyZoneFlash());
////		}
//	}	
	
	public void closeByFB()
	{
//		l_lifeLamp.color = Color.white;	
	}
	
	public void noFB()
	{
//		SC_lifeLampRef.setCurrentColor();
	}
	
	private void OnDrawGizmosSelected() {
//		Gizmos.color = Color.white;			
//		Gizmos.DrawWireSphere(transform.position, 2.5f);
	}	
}
