using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class SC_Menu : MonoBehaviour {

	[System.Serializable]
	public class Bouton
	{
		public string _Name;
		public GameObject _Bouton;
		public Transform _PositionDepart;
		public Transform _PositionDehors;
		
		public Bouton ()
		{
			
		}
	}

	[System.Serializable]
	public class Player
	{
		public string _Name;
		public GameObject _PlayerEmpty;
		public GameObject _PlayerActive;
		public Transform _PositionDehors;
		public Transform _PositionDedans;
		public bool b_IsActive = false;
		
		public Player ()
		{
			
		}
	}

	[System.Serializable]
	public class Tanuki
	{
		public GameObject _Tanuki;
		public Transform _PositionDepart;
		public Transform _PositionIntermediaire;
		public Transform _PositionDehors;
		
		public Tanuki ()
		{
			
		}
	}

	[System.Serializable]
	public class Titre
	{
		public GameObject _TitreObj;
		public Transform _PositionDepart;
		public Transform _PositionDehors;
		
		public Titre ()
		{
			
		}
	}

	public Tanuki _Tanuki;
	public Titre _Titre;

	//MainMenu
	private string s_CurrentState;
	public Bouton[] _MenuButons;
	private int i_CurrentButton;
	public GameObject _Viseur;

	//Tuto
	private int i_CurrentTutoSlide;
	private int i_PreviousTutoSlide;
	public GameObject[] a_SlideTuto;

	//Game
	public Player[] _GamePlayers;

	//Credit
	public GameObject _Credit;

	//xINPUT
	private GamePadState _PreviousControllerState1;
	private GamePadState _ControllerState1;
	private PlayerIndex _PlayerIndex1;

	private GamePadState _PreviousControllerState2;
	private GamePadState _ControllerState2;
	private PlayerIndex _PlayerIndex2;

	private GamePadState _PreviousControllerState3;
	private GamePadState _ControllerState3;
	private PlayerIndex _PlayerIndex3;

	private GamePadState _PreviousControllerState4;
	private GamePadState _ControllerState4;
	private PlayerIndex _PlayerIndex4;

	private GamePadState _PreviousControllerStateTest;
	private GamePadState _ControllerStateTest;
	private PlayerIndex _PlayerIndexTest;
	
	private bool b_A1;
	private bool b_B1;
	private bool b_StickLeft1;
	private bool b_StickRight1;

	private bool b_A2;
	private bool b_B2;

	private bool b_A3;
	private bool b_B3;

	private bool b_A4;
	private bool b_B4;

	private int i_MannetteConnected;
	private int i_PlayersActive;

	// Use this for initialization
	void Start () 
	{
		_Tanuki._Tanuki.transform.position = _Tanuki._PositionDepart.position;
		_Titre._TitreObj.transform.position = _Titre._PositionDepart.position;
		_Credit.renderer.enabled = false;

		for(int i = 0; i < a_SlideTuto.Length; i ++)
		{
			a_SlideTuto[i].renderer.enabled = false;
		}

		for(int i = 0; i < _MenuButons.Length; i++)
		{
			_MenuButons[i]._Bouton.transform.position = _MenuButons[i]._PositionDepart.position;
		}

		for(int i = 0; i < _GamePlayers.Length; i++)
		{
			_GamePlayers[i]._PlayerEmpty.transform.position = _GamePlayers[i]._PositionDehors.position;
			_GamePlayers[i]._PlayerActive.transform.position = _GamePlayers[i]._PositionDehors.position;
			_GamePlayers[i].b_IsActive = false;
		}

		s_CurrentState = "MainMenu";
		i_CurrentButton = 0;

		i_CurrentTutoSlide = 0;


		//mannette 1
		_PlayerIndexTest = (PlayerIndex)0;
		_ControllerStateTest = GamePad.GetState(_PlayerIndexTest);

		if(_ControllerStateTest.IsConnected)
		{
			_PlayerIndex1 = (PlayerIndex)0;
			_ControllerState1 = GamePad.GetState(_PlayerIndex1);
			i_MannetteConnected++;
		}
		else
		{
			Debug.Log("Connectez Mannette 1");
		}

		//mannette 2
		_PlayerIndexTest = (PlayerIndex)1;
		_ControllerStateTest = GamePad.GetState(_PlayerIndexTest);
		
		if(_ControllerStateTest.IsConnected)
		{
			_PlayerIndex2 = (PlayerIndex)1;
			_ControllerState2 = GamePad.GetState(_PlayerIndex2);
			i_MannetteConnected++;
		}
		else
		{
			_PlayerIndex2 = (PlayerIndex)4;
			_ControllerState2 = GamePad.GetState(_PlayerIndex2);
		}

		//mannette 3
		_PlayerIndexTest = (PlayerIndex)2;
		_ControllerStateTest = GamePad.GetState(_PlayerIndexTest);
		
		if(_ControllerStateTest.IsConnected)
		{
			_PlayerIndex3 = (PlayerIndex)2;
			_ControllerState1 = GamePad.GetState(_PlayerIndex3);
			i_MannetteConnected++;
		}
		else
		{
			_PlayerIndex3 = (PlayerIndex)4;
			_ControllerState3 = GamePad.GetState(_PlayerIndex3);
		}

		//mannette 4
		_PlayerIndexTest = (PlayerIndex)3;
		_ControllerStateTest = GamePad.GetState(_PlayerIndexTest);
		
		if(_ControllerStateTest.IsConnected)
		{
			_PlayerIndex4 = (PlayerIndex)3;
			_ControllerState4 = GamePad.GetState(_PlayerIndex4);
			i_MannetteConnected++;
		}
		else
		{
			_PlayerIndex4 = (PlayerIndex)4;
			_ControllerState4 = GamePad.GetState(_PlayerIndex4);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//xInput
		_PreviousControllerState1 = _ControllerState1;
		_ControllerState1 = GamePad.GetState(_PlayerIndex1);

		_PreviousControllerState2 = _ControllerState2;
		_ControllerState2 = GamePad.GetState(_PlayerIndex2);

		_PreviousControllerState3 = _ControllerState3;
		_ControllerState3 = GamePad.GetState(_PlayerIndex3);

		_PreviousControllerState4 = _ControllerState4;
		_ControllerState4 = GamePad.GetState(_PlayerIndex4);

		if(_PreviousControllerState1.Buttons.A == ButtonState.Released && _ControllerState1.Buttons.A == ButtonState.Pressed)
		{
			b_A1 = true;
		}
		else
		{
			b_A1 = false;
		}

		if(_PreviousControllerState1.Buttons.B == ButtonState.Released && _ControllerState1.Buttons.B == ButtonState.Pressed)
		{
			b_B1 = true;
		}
		else
		{
			b_B1 = false;
		}

		if(_PreviousControllerState1.ThumbSticks.Left.X < 0.2f && _ControllerState1.ThumbSticks.Left.X > 0.2f)
		{
			b_StickRight1 = true;
		}
		else
		{
			b_StickRight1 = false;
		}

		if(_PreviousControllerState1.ThumbSticks.Left.X > -0.2f && _ControllerState1.ThumbSticks.Left.X < -0.2f)
		{
			b_StickLeft1 = true;
		}
		else
		{
			b_StickLeft1 = false;
		}

		if(_PreviousControllerState2.Buttons.A == ButtonState.Released && _ControllerState2.Buttons.A == ButtonState.Pressed)
		{
			b_A2 = true;
		}
		else
		{
			b_A2 = false;
		}
		
		if(_PreviousControllerState2.Buttons.B == ButtonState.Released && _ControllerState2.Buttons.B == ButtonState.Pressed)
		{
			b_B2 = true;
		}
		else
		{
			b_B2 = false;
		}

		if(_PreviousControllerState3.Buttons.A == ButtonState.Released && _ControllerState3.Buttons.A == ButtonState.Pressed)
		{
			b_A3 = true;
		}
		else
		{
			b_A3 = false;
		}
		
		if(_PreviousControllerState3.Buttons.B == ButtonState.Released && _ControllerState3.Buttons.B == ButtonState.Pressed)
		{
			b_B3 = true;
		}
		else
		{
			b_B3 = false;
		}

		if(_PreviousControllerState4.Buttons.A == ButtonState.Released && _ControllerState4.Buttons.A == ButtonState.Pressed)
		{
			b_A4 = true;
		}
		else
		{
			b_A4 = false;
		}
		
		if(_PreviousControllerState4.Buttons.B == ButtonState.Released && _ControllerState4.Buttons.B == ButtonState.Pressed)
		{
			b_B4 = true;
		}
		else
		{
			b_B4 = false;
		}


		switch(s_CurrentState)
		{
		case "MainMenu" :

			MainMenuUpdate ();

			break;

		case "Tuto" :

			TutoUpdate ();

			break;
			
		case "Credits" :

			CreditUpdate ();
			
			break;

		case "Game1" :
			
			Game1Update ();
			
			break;

		case "Game2" :
			
			Game2Update ();
			
			break;

		default :

			break;
		}
	}

	private void MainMenuUpdate ()
	{
		_Viseur.transform.position = new Vector3(_MenuButons[i_CurrentButton]._Bouton.transform.position.x,
		                                         _MenuButons[i_CurrentButton]._Bouton.transform.position.y,
		                                         _MenuButons[i_CurrentButton]._Bouton.transform.position.z -0.2f);

		if(b_StickRight1) i_CurrentButton ++;

		if(b_StickLeft1) i_CurrentButton --;

		if(i_CurrentButton > _MenuButons.Length-1) i_CurrentButton = 0;

		if(i_CurrentButton < 0) i_CurrentButton = _MenuButons.Length-1;

		if(b_B1) Application.Quit();

		if(b_A1)
		{
			StartCoroutine(Transition("MainMenu",_MenuButons[i_CurrentButton]._Name));
		}
	}

	private void TutoUpdate ()
	{
		if(b_StickRight1) 
		{
			i_PreviousTutoSlide = i_CurrentTutoSlide;
			i_CurrentTutoSlide ++;

			if(i_CurrentTutoSlide > a_SlideTuto.Length-1) i_CurrentTutoSlide = 0;
			
			a_SlideTuto[i_PreviousTutoSlide].renderer.enabled = false;
			a_SlideTuto[i_CurrentTutoSlide].renderer.enabled = true;
		}
		
		if(b_StickLeft1)
		{
			i_PreviousTutoSlide = i_CurrentTutoSlide;
			i_CurrentTutoSlide --;

			if(i_CurrentTutoSlide < 0) i_CurrentTutoSlide = a_SlideTuto.Length-1;

			a_SlideTuto[i_PreviousTutoSlide].renderer.enabled = false;
			a_SlideTuto[i_CurrentTutoSlide].renderer.enabled = true;
		}

		if(b_B1)
		{
			StartCoroutine(Transition("Tuto", "MainMenu"));
		}
	}

	private void CreditUpdate ()
	{
		if(b_B1)
		{
			StartCoroutine(Transition("Credits", "MainMenu"));
		}
	}

	private void Game1Update ()
	{
		if(b_A1)
		{
			_GamePlayers[0].b_IsActive = !_GamePlayers[0].b_IsActive;
		}

		if(b_A2)
		{
			_GamePlayers[1].b_IsActive = !_GamePlayers[1].b_IsActive;
		}

		if(b_A3)
		{
			_GamePlayers[2].b_IsActive = !_GamePlayers[2].b_IsActive;
		}

		if(b_A4)
		{
			_GamePlayers[3].b_IsActive = !_GamePlayers[3].b_IsActive;
		}

		for(int i = 0; i < _GamePlayers.Length; i++)
		{
			if(_GamePlayers[i].b_IsActive)
			{
				_GamePlayers[i]._PlayerActive.transform.position = new Vector3(_GamePlayers[i]._PositionDedans.transform.position.x,
				                                                               _GamePlayers[i]._PositionDedans.transform.position.y,
				                                                               _GamePlayers[i]._PositionDedans.transform.position.z - 0.2f);
			}
			else
			{
				_GamePlayers[i]._PlayerActive.transform.position = new Vector3(_GamePlayers[i]._PositionDehors.transform.position.x,
				                                                               _GamePlayers[i]._PositionDehors.transform.position.y,
				                                                               _GamePlayers[i]._PositionDehors.transform.position.z);
			}
		}

		if(b_B1)
		{
			StartCoroutine(Transition("Game1", "MainMenu"));
		}
	}

	private void Game2Update ()
	{
		
	}

	IEnumerator Transition (string currentState, string nextState)
	{
		if(currentState == "MainMenu")
		{
			_Titre._TitreObj.transform.position = _Titre._PositionDehors.position;

			//faire les translations 
			int secure = 0;
			float pos = 0;

			while(pos < 1 && secure < 100)
			{
				pos += 0.1f;
				for(int i = 0; i < _MenuButons.Length; i++)
				{
					_MenuButons[i]._Bouton.transform.position = Vector3.Lerp(_MenuButons[i]._PositionDepart.position, 
					                                                         _MenuButons[i]._PositionDehors.position,
					                                                         pos);
				}

				secure ++;
				yield return new WaitForEndOfFrame();
			}

			if(nextState == "Tuto")
			{
				_Tanuki._Tanuki.transform.position = _Tanuki._PositionDehors.position;
				a_SlideTuto[0].renderer.enabled = true;
				i_CurrentTutoSlide = 0;
				s_CurrentState = "Tuto";
			}
			else if (nextState == "Game1")
			{
				_Tanuki._Tanuki.transform.position = _Tanuki._PositionIntermediaire.position;

				int secureP = 0;
				float posP = 0;
				while(posP < 1 && secureP < 100)
				{
					posP += 0.1f;
					for(int i = 0; i < _GamePlayers.Length; i++)
					{
						_GamePlayers[i]._PlayerEmpty.transform.position = Vector3.Lerp(_GamePlayers[i]._PositionDehors.position, 
						                                                               _GamePlayers[i]._PositionDedans.position,
						                                                               posP);
					}
					
					secureP ++;
					yield return new WaitForEndOfFrame();
				}

				s_CurrentState = "Game1";
			}
			else if (nextState == "Game2")
			{
				
			}
			else if (nextState == "Credits")
			{
				_Tanuki._Tanuki.transform.position = _Tanuki._PositionIntermediaire.position;
				_Credit.renderer.enabled = true;
				s_CurrentState = "Credits";
			}
		}
		else if(nextState == "MainMenu")
		{
			if(currentState == "Tuto")
			{
				a_SlideTuto[i_CurrentTutoSlide].renderer.enabled = false;
			}
			else if (currentState == "Game1")
			{
				int secureP = 0;
				float posP = 0;
				while(Vector3.Distance(_GamePlayers[0]._PlayerEmpty.transform.position, _GamePlayers[0]._PositionDehors.position) > 0.01f && secureP < 100)
				{
					posP += 0.1f;
					for(int i = 0; i < _GamePlayers.Length; i++)
					{
						_GamePlayers[i].b_IsActive = false;
						_GamePlayers[i]._PlayerEmpty.transform.position = Vector3.Lerp(_GamePlayers[i]._PositionDedans.position, 
						                                                               _GamePlayers[i]._PositionDehors.position,
						                                                               posP);

						_GamePlayers[i]._PlayerActive.transform.position = Vector3.Lerp(_GamePlayers[i]._PositionDedans.position,
						                                                                _GamePlayers[i]._PositionDehors.position,
						                                                                posP);
					}
					
					secureP ++;
					yield return new WaitForEndOfFrame();
				}
			}
			else if (currentState == "Game2")
			{
				
			}
			else if (currentState == "Credits")
			{
				_Credit.renderer.enabled = false;
				s_CurrentState = "MainMenu";
			}

			int secure = 0;
			float pos = 0;
			
			while(Vector3.Distance(_MenuButons[0]._Bouton.transform.position, _MenuButons[0]._PositionDepart.position) > 0.01f && secure < 100)
			{
				pos += 0.1f;
				for(int i = 0; i < _MenuButons.Length; i++)
				{
					_MenuButons[i]._Bouton.transform.position = Vector3.Lerp(_MenuButons[i]._PositionDehors.position, 
					                                                         _MenuButons[i]._PositionDepart.position,
					                                                         pos);
				}
				
				secure ++;
				yield return new WaitForEndOfFrame();
			}

			_Tanuki._Tanuki.transform.position = _Tanuki._PositionDepart.position;
			_Titre._TitreObj.transform.position = _Titre._PositionDepart.position;

			s_CurrentState = "MainMenu";
		}

		yield break;
	}
}
