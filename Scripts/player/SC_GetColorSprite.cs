using UnityEngine;
using System.Collections;

public class SC_GetColorSprite : MonoBehaviour {

	public	bool b_isSprite = true;
	public bool b_isLight = false;

	public SpriteRenderer s_renderer;
	public SC_Player SC_PlayerRef;
	public Color _Color;
	//[HideInInspector]
	public Color _CurrentColor;

	// Use this for initialization
	void Start () {
		if(b_isSprite == true)
		{
			s_renderer = this.gameObject.GetComponent<SpriteRenderer>();
			_Color = SC_PlayerRef.s_brightColor;
			s_renderer.color = _Color;
		}
		if(b_isLight == true)
		{
			_Color = SC_PlayerRef.s_brightColor;
			_CurrentColor = _Color;
			light.color = _Color;
		}
	}	

	// Update is called once per frame
	void Update () 
	{	
	}

	public void setBackColor()
	{
		_CurrentColor = _Color;
		if(b_isLight == true) light.color = _Color;
		if(b_isSprite == true) s_renderer.color = _Color;
	}
	
	public void setCurrentColor()
	{
		if(b_isLight) light.color = _CurrentColor;
		if(b_isSprite) s_renderer.color = _CurrentColor;
	}
}
