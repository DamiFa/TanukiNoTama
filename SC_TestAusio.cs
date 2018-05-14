using UnityEngine;
using System.Collections;

public class SC_TestAusio : MonoBehaviour {

	public SC_SoundManager _Sounds;
	public AudioSource _PlayerStateSounds;
	public AudioSource _AttackSounds;
	public AudioSource _PuSounds;

	private AudioClip _Clip_PlayerStateSounds;
	private AudioClip _Clip_AttackSounds;
	private AudioClip _Clip_PuSounds;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(TestSounds());
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	IEnumerator TestSounds ()
	{
		_Clip_PlayerStateSounds = _Sounds.GetSound("Walk");
		_PlayerStateSounds.clip = _Clip_PlayerStateSounds;
		_PlayerStateSounds.Play();

		yield return new WaitForSeconds(2);

//		while(true)
//		{
//			_Clip_AttackSounds = _Sounds.GetSound("Attack");
//			_AttackSounds.PlayOneShot(_Clip_AttackSounds);
//			(_Clip_AttackSounds.name);
//			
//			yield return new WaitForSeconds(_Clip_AttackSounds.length + 1);
//		}

		_Clip_AttackSounds = _Sounds.GetSound("Attack");
		_AttackSounds.PlayOneShot(_Clip_AttackSounds);
		Debug.Log (_Clip_AttackSounds.name);
		
		yield return new WaitForSeconds(_Clip_AttackSounds.length + 1);

		_Clip_AttackSounds = _Sounds.GetSound("Attack");
		_AttackSounds.PlayOneShot(_Clip_AttackSounds);
		Debug.Log (_Clip_AttackSounds.name);
		
		yield return new WaitForSeconds(_Clip_AttackSounds.length*0.5f);

		_Clip_AttackSounds = _Sounds.GetSound("Attack");
		_AttackSounds.PlayOneShot(_Clip_AttackSounds);
		Debug.Log (_Clip_AttackSounds.name);
		
		yield return new WaitForSeconds(_Clip_AttackSounds.length + 1);

		yield break;
	}
}
