using UnityEngine;
using System.Collections;

public class SC_WrapBorder : MonoBehaviour {

	public Transform t_RightWall;
	public Transform t_TopWall;
	public Transform t_LeftWall;
	public Transform t_BottomWall;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(transform.position.x >= t_RightWall.transform.position.x)
		{
			transform.position = new Vector3(t_RightWall.transform.position.x, transform.position.y, transform.position.z);
		}
		if(transform.position.x <= t_LeftWall.transform.position.x)
		{
			transform.position = new Vector3(t_LeftWall.transform.position.x, transform.position.y, transform.position.z);
		}
		if(transform.position.y >= t_TopWall.transform.position.y)
		{
			transform.position = new Vector3(transform.position.x, t_TopWall.transform.position.y, transform.position.z);
		}
		if(transform.position.y <= t_BottomWall.transform.position.y)
		{
			transform.position = new Vector3(transform.position.x, t_BottomWall.transform.position.y, transform.position.z);
		}	
	}
}
