using UnityEngine;
using System.Collections;

public class SC_SoundManager : MonoBehaviour {

	[System.Serializable]
	public class Sounds
	{
		public string _Name = "noName";
		public AudioClip[] a_Sounds;
		
		public Sounds ()
		{
			
		}
		
		public AudioClip GetAudioClip()
		{
			AudioClip _AudioClipToGive = a_Sounds[Random.Range(0, a_Sounds.Length - 1)];

			return _AudioClipToGive;
		}
	}

	public Sounds[] a_GameSounds;
	public AudioClip _ErrorSound;
	private int _CurrentScene;
	public int i_TutoScene;
	public AudioClip _TutoMuic;
	public int i_GameScene;
	public AudioClip[] _GameMusicStart;
	public AudioClip _GameMusicMain;
	public int i_MenuScene;
	public AudioClip _MenuMusic;
	public AudioClip _endMusic;
	public AudioSource _AudioSource_Music;
	public AudioClip[] _LvLupSound;



	private float f_timer;

	// Use this for initialization
	void Start () 
	{
		SoundTrack();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_CurrentScene != Application.loadedLevel)
		{
			_CurrentScene = Application.loadedLevel;
			SoundTrack ();
		}
		f_timer += Time.deltaTime;
		if(f_timer > _AudioSource_Music.clip.length && _AudioSource_Music.clip != _GameMusicMain)
		{
			_AudioSource_Music.clip = _GameMusicMain;
			_AudioSource_Music.Play();
		}

//		DontDestroyOnLoad(this.gameObject);
	}


	public void endGame()
	{
		_AudioSource_Music.clip = _endMusic;
		_AudioSource_Music.Play();
	}

	public AudioClip GetSound (string _SoundName)
	{
		AudioClip _AudioClipToGive = _ErrorSound;

		for(int i = 0; i < a_GameSounds.Length; i ++)
		{
			if(a_GameSounds[i]._Name == _SoundName)
			{
				_AudioClipToGive = a_GameSounds[i].GetAudioClip();

				i = a_GameSounds.Length;
			}
		}

		return _AudioClipToGive;
	}



	public AudioClip GetLVLupSound()
	{
		AudioClip _AudioClipToGive = _ErrorSound;
		float f_timePosition;

		f_timePosition = _AudioSource_Music.timeSamples/_AudioSource_Music.clip.frequency;

		if(_AudioSource_Music.clip == _GameMusicMain)
		{
			if(f_timePosition < 34f) _AudioClipToGive = _LvLupSound[1];
			if(f_timePosition >= 34f && f_timePosition < 68f) _AudioClipToGive = _LvLupSound[2];
			if(f_timePosition >= 68f && f_timePosition < 102f) _AudioClipToGive = _LvLupSound[3];
			if(f_timePosition >= 102f && f_timePosition < 137f) _AudioClipToGive = _LvLupSound[4];
		}

		if(_AudioSource_Music.clip == _GameMusicStart[0] || _AudioSource_Music.clip == _GameMusicStart[1])
		{
			if(f_timePosition < 34f) _AudioClipToGive = _LvLupSound[0];
			if(f_timePosition >= 34f) _AudioClipToGive = _LvLupSound[1];
		}
		Debug.Log(_AudioClipToGive.name);
		Debug.Log(f_timePosition);

		return _AudioClipToGive;
	}

	public void SoundTrack ()
	{
		if(_CurrentScene == i_TutoScene)
		{
			_AudioSource_Music.clip = _TutoMuic;
		}
		else if(_CurrentScene == i_GameScene)
		{
			_AudioSource_Music.clip = _GameMusicStart[Random.Range(0,1)];
		}
		else if(_CurrentScene == i_TutoScene)
		{
			_AudioSource_Music.clip = _TutoMuic;
		}

		_AudioSource_Music.Play();
	}
}
