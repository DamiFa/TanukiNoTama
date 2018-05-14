using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class SC_BatimentChoice : MonoBehaviour {
	
	private string s_LeftBat;
	private string s_RightBat;
	private SC_BatimentManager _BatimentManager;
	private GameObject _LeftChoice;
	private GameObject _RightChoice;
	public bool b_ChoiceGiven;
	private int i_PlayerId;

	public Transform t_choiceL;
	public Transform t_choiceR;

	private bool b_isChoosingAnim;
	private bool b_leftChosen;

	private float f_timer;

	private SC_Player SC_PlayerRef;

	private Color l_Color;
	private Color r_Color;

	private SpriteRenderer l_SpriteRenderer;
	private SpriteRenderer r_SpriteRenderer;

	private float f_timerColorFlash;
	private bool b_flashingRedPUchoice = false;

	public ParticleSystem p_rightParticle;
	public ParticleSystem p_leftParticle;

	public Transform t_particleDestination;

	SC_particleSetRotation SC_particleRefR;
	SC_particleSetRotation SC_particleRefL;

	public ParticleSystem p_rightBackground;
	public ParticleSystem p_leftBackground;

	Color c_brightPlayerColor;

	// Use this for initialization
	void Start () 
	{
		i_PlayerId = (int)GetComponent<SC_Player>().playerIndex;
		b_ChoiceGiven = false;
		_BatimentManager = GameObject.FindWithTag("BATIMENT_MANAGER").GetComponent<SC_BatimentManager>();

		SC_PlayerRef = this.GetComponent<SC_Player>();

		p_rightParticle.startColor = SC_PlayerRef.s_brightColor;
		p_leftParticle.startColor = SC_PlayerRef.s_brightColor;

		SC_particleRefR = p_rightParticle.GetComponent<SC_particleSetRotation>();
		SC_particleRefL = p_leftParticle.GetComponent<SC_particleSetRotation>();


		c_brightPlayerColor = SC_PlayerRef.s_brightColor;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(b_ChoiceGiven)
		{
			if(GetComponent<SC_Player>()._ControllerState.Buttons.RightShoulder == ButtonState.Pressed && GetComponent<SC_Player>()._PreviousControllerState.Buttons.RightShoulder == ButtonState.Released)
			{
				_BatimentManager.PlaceBatiment(s_LeftBat, i_PlayerId, this.GetComponent<SC_BatimentChoice>());

				StartCoroutine(DestroyAnim(_LeftChoice, _RightChoice));	

				p_rightParticle.gameObject.SetActive(true);
				SC_particleRefR.t_destination = t_particleDestination;
				p_rightParticle.Emit(150);

				b_ChoiceGiven = false;
			}
			else if(GetComponent<SC_Player>()._ControllerState.Buttons.LeftShoulder == ButtonState.Pressed && GetComponent<SC_Player>()._PreviousControllerState.Buttons.LeftShoulder == ButtonState.Released)
			{
				_BatimentManager.PlaceBatiment(s_RightBat, i_PlayerId, this.GetComponent<SC_BatimentChoice>());

				StartCoroutine(DestroyAnim(_RightChoice, _LeftChoice));

				p_leftParticle.gameObject.SetActive(true);
				SC_particleRefL.t_destination = t_particleDestination;
				p_leftParticle.Emit(150);

				b_ChoiceGiven = false;
			}
		}
	}


	IEnumerator DestroyAnim(GameObject Choose, GameObject NotChoose)
	{
		Choose.GetComponent<SC_PUchoiceSafety>().b_timerOn = true;
		NotChoose.GetComponent<SC_PUchoiceSafety>().b_timerOn = true;
		p_rightBackground.Stop();
		p_leftBackground.Stop();


		float t = 0;
		while(t < 0.2f)
		{
			Choose.transform.localScale = new Vector3(Choose.transform.localScale.x * 1.05f,
			                                          Choose.transform.localScale.y * 1.05f,
			                                          Choose.transform.localScale.z * 1.05f);

			NotChoose.transform.localScale = new Vector3(NotChoose.transform.localScale.x * 0.5f,
			                                             NotChoose.transform.localScale.y * 0.5f,
			                                             NotChoose.transform.localScale.z * 0.5f);

			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		instantDestroy();
		p_leftParticle.gameObject.SetActive(true);
		SC_particleRefL.t_destination = t_particleDestination;
		p_leftParticle.Emit(5);
	}

	public void GiveChoice (string _FirstChoice, string _SecondChoice)
	{
		_LeftChoice = null;
		_RightChoice = null;
		s_LeftBat = null;
		s_RightBat = null;
		l_SpriteRenderer = null;
		r_SpriteRenderer = null;
		b_ChoiceGiven = false;


		s_LeftBat = _FirstChoice;
		s_RightBat = _SecondChoice;

		_LeftChoice = Instantiate(_BatimentManager.GetBatChoice(s_LeftBat),t_choiceL.transform.position,
		                          Quaternion.Euler(0,0,0)) as GameObject;

		_RightChoice = Instantiate(_BatimentManager.GetBatChoice(s_RightBat),t_choiceR.transform.position,
		                           Quaternion.Euler(0,0,0)) as GameObject;

		_LeftChoice.GetComponent<SpriteRenderer>().color = c_brightPlayerColor;
		_RightChoice.GetComponent<SpriteRenderer>().color = c_brightPlayerColor;


		p_rightBackground.Play();
		p_leftBackground.Play();


		b_ChoiceGiven = true;

	}

	public void instantDestroy()
	{
		Destroy(_LeftChoice);
		Destroy(_RightChoice);
	}
}
