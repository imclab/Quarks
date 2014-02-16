using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Class for the user interface.
/// </summary>
public class GUIManager : MonoBehaviour {

	// The UI skin
	public GUISkin GameUISkin;

	// Spell keys
	private string[] keys = new string[4] { "Q","W","E","R" };

	// Singleton
	public GUIManager guiManager;
	public static GUIManager Instance { get; private set; }

	// The main player ball
	private GameObject playerBall;

	// Game object to spawn as a notice
	public GameObject playerNotice;

	// List of spells available
	List<Spell> spellList = new List<Spell>();

	// PROGRESS BAR
	public GUIStyle progress_empty;
	public GUIStyle progress_full;
	public Texture GUI_BG;

	public Vector2 pos = new Vector2(0,0);
	private Vector2 size = new Vector2(600,18);
	
	public Texture2D emptyTex;
	public Texture2D fullTex;
	// END OF PROGRESS BAR

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		// Save a reference to our singleton instance
		Instance = this;
	}

	void Start()
	{
		spellList = SpellManager.Instance.GetSpells();
	}

	/// <summary>
	/// Show a notice and play a sound
	/// </summary>
	/// <param name="power">Power.</param>
	public void NoPowerNotice(float power)
	{
		// Play sound
		new OTSound("NoPower");

		// Show notification
		ShowPlayerNotice("Need " + power + " power");

	}

	// Shows a message over the main player ball
	public void ShowPlayerNotice(string notice)
	{
		GameObject tempGO = Instantiate(playerNotice, Camera.main.WorldToViewportPoint(PlayerManager.Instance.playerBall.transform.position), Quaternion.identity) as GameObject;
		tempGO.GetComponent<GUIText>().text = notice;
	}

	/// <summary>
	/// Updates the GUI
	/// </summary>
	void OnGUI()
	{
		// Load skin
		GUI.skin = GameUISkin;

		// BOTTOM UI GROUP
		GUI.BeginGroup(new Rect(0, Screen.height - 108, Screen.width, 108 ));
			
			// Add box at bottom
			GUI.DrawTexture (new Rect (-1,0,1282,108),GUI_BG);
			
			if(GameManager.Instance.isRunning)
			{
				// Draw power box
				GUI.BeginGroup(new Rect(68,26,750,50));

					// Power-label
					GUI.Label(new Rect(0,0,50,19), "Power:", GameUISkin.GetStyle("UIBottomLabel"));
					
					// Draw power bar
					GUI.BeginGroup(new Rect(110,3, size.x, size.y));	

						// Draw bar based on player power
						GUI.BeginGroup(new Rect(0, 0, size.x * PlayerManager.Instance.powerCurrent / PlayerManager.Instance.powerMax, size.y));
						
							GUI.Box(new Rect(0, 0, size.x, size.y), fullTex, progress_full);
						
						GUI.EndGroup();
						
						// Draw border
						GUI.Box(new Rect(0, 0, size.x, size.y), emptyTex, progress_empty);
					
					GUI.EndGroup();

					GUI.Label(new Rect(665,2,50,45), Truncate(PlayerManager.Instance.powerCurrent, 0).ToString("0") + "/" + PlayerManager.Instance.powerMax, GameUISkin.GetStyle("UIBottomLabelPower"));

				GUI.EndGroup();
				
				// Time
			GUI.Label(new Rect(68,62,50,19), "Time: " + Truncate(GameManager.Instance.CurrentGameTime, 0).ToString("0"), GameUISkin.GetStyle("UIBottomLabel"));	

				// Draw the spells
				GUI.BeginGroup(new Rect(980, 20, 270, 66 ));
					
					int i = 0;
					int xpos = 0;

					// Draw each spell
					foreach(Spell spell in spellList)
					{
						GUI.BeginGroup(new Rect(xpos, 0, 54, 66 ));
							GUI.Label(new Rect(0,0,54,20), keys[i], GameUISkin.GetStyle("UISpellLabel"));
							GUI.DrawTexture (new Rect (11,20,32,32), spell.txIcon);
							GUI.Label(new Rect(0,54,54,12),spell.name, GameUISkin.GetStyle("UISpellLabel"));
						GUI.EndGroup();
						i++;
						xpos += 54;
					}
				GUI.EndGroup();
			}
			else
			{
				// Power-label
				GUI.Label(new Rect( Screen.width / 2 - 50, 30, 100, 50), GameManager.Instance.gameInfo, GameUISkin.GetStyle("UIGameInfo"));

			}

		GUI.EndGroup();
	}

	/// <summary>
	/// Truncate the specified value and digits. (i.e set the number of displayed decimals)
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="digits">Digits.</param>
	public float Truncate(float value, int digits)
	{
		double mult = Math.Pow(10.0, digits);
		double result = Math.Truncate( mult * value ) / mult;
		return (float) result;
	}
}
