using UnityEngine;
using System.Collections;

public class SC_Particle : MonoBehaviour {

	public GameObject _Particule;
	public int i_NbParticule;
	public bool b_AreOrbiting = false;
	public float f_RotationSpeed;
	private GameObject[] a_Particles;

	// Use this for initialization
	void Start () 
	{
		a_Particles = new GameObject[i_NbParticule];

		for(int i = 0; i < i_NbParticule; i++)
		{
			GameObject clone;
			Vector3 rotation = new Vector3(0, 0, 360 * i / i_NbParticule);
			clone = Instantiate(_Particule, transform.position, Quaternion.identity) as GameObject;
			clone.transform.parent = this.transform;
			clone.transform.rotation = Quaternion.Euler(rotation);
			a_Particles[i] = clone;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(b_AreOrbiting)
		{
			for(int i = 0; i < i_NbParticule; i++)
			{
				a_Particles[i].transform.Rotate(0, 0, f_RotationSpeed);
			}
		}
	}
}
