/// <summary>
/// DevManager
/// For developer debug purpose, which can display the custorm Debug GUI panel.
/// 
/// Usage :
/// 	Call "DevManager.Instance.reg(xxxxx) to easily regist custorm GUI panel under DevManager control
/// Ref :
/// 	by default will register DevLog As default Unity3D debug console. 
/// 
/// By Canis 20130406
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DevelopManager
{
	/// <summary>
	/// Develop Manager.
	/// </summary>
	/// <remarks>
	/// Developed by Canis Wong (www.clonefactor.com)
	/// Reference : Matthew Miner (www.matthewminer.com) // https://gist.github.com/mminer/975374
	/// </remarks>
	public class DevManager : MonoBehaviour
	{
		private const int PANEL_HEIGHT = 40;
		private const string DEV_TITLE = ":DM:";
		
		// [System.NonSerializedAttribute]
		private static DevManager instance = null;
		public static DevManager Instance
		{
			get
			{
				if( instance == null )
				{
					if( !GameObject.Find("DevManager") )
					{
						instance = new GameObject("DevManager").AddComponent<DevManager>();
						instance.CanTriggerByKey = false;
					}
				}
				return instance;
			}
		}
		public bool CanTriggerByKey = true;
		public KeyCode TriggerMenuKey = KeyCode.ScrollLock;
		public bool mVisible = false;
		
		// color set
		public Texture2D mIcon;
		public UnityEngine.Color fontColor = new UnityEngine.Color(0.28f,0.69f,0.93f,1.0f);
		public UnityEngine.Color backgroundColor = new UnityEngine.Color(0.19f,0.25f,0.28f,0.8f);
		private Rect mWindowPos;
		private GUIStyle mDockStyle = new GUIStyle();
		public GUIStyle mWindowStyle = new GUIStyle();
		private int mWindowCount = 0;
		private Vector2 mScrollPos = Vector2.zero;
		private List<WindowTask> mList = new List<WindowTask>();
		
		// default DevWindow
		DevLog mDevLog = new DevLog();
		private void Awake()
		{
			instance = this;
			PlayerPrefs.SetString("DevManager","DevManager GUI ver 1.1");
			this.reg ("Log", mDevLog.HandleGUI, true);
		}
		private void Start ()
		{
			initGUI ();
		}
		private void Update ()
		{
			this.TriggerByKey();
		}
		private void OnGUI()
		{
			this.drawGUL();
		}
		private void OnEnable()
		{	// Link up debug console event
			mDevLog.OnEnable();
		}
		private void OnDisable()
		{	// Dislink debug console event
			mDevLog.OnDisable();
		}
		private void TriggerByKey()
		{	// Triggers DevManager GUI by config key.
			if( CanTriggerByKey &&
				Input.GetKeyUp(TriggerMenuKey) )
			{
				// Only do following by TriggerMenuKey press
				this.Toggle();
				Debug.Log ("DevManager "+ ((mVisible)?"Enable":"Disable") );
			}//if
		}
		private void initGUI()
		{
			mWindowPos = new Rect(0,0,Screen.width,PANEL_HEIGHT);
			mDockStyle.normal.textColor = fontColor;
			mDockStyle.normal.background = DevGUI.fillColor(backgroundColor);
			mWindowStyle.normal.textColor = fontColor;
			mWindowStyle.normal.background = DevGUI.fillColor(backgroundColor);
		}
		private void drawGUL()
		{
			if( !mVisible ) return;
			// DevManager main panel.
			mWindowPos.width = Screen.width;
			mWindowPos = GUI.Window(0, mWindowPos, drawDevManager, "", mDockStyle);
			drawRegistedWindowTask();
		}
		private void drawDevManager(int id)
		{
			if( id != 0 ) return;
			// update Screen size.
			// GUI.color = fontColor;
			GUILayout.BeginHorizontal();
			{
				if( mIcon != null )
				{
					GUILayout.Label(mIcon);
				}
				else
				{
					GUILayout.Box (DEV_TITLE);
				}
				mScrollPos = GUILayout.BeginScrollView(mScrollPos,false,false);
				GUILayout.BeginHorizontal();
				{
					GUI.skin.box.wordWrap = true;
					// all registed window's toggle button
					for(int i=0; i<mList.Count; i++)
					{
						bool _tmp = mList[i].visible;
						// Toggle button for each regist window
						mList[i].visible = GUILayout.Toggle(mList[i].visible, new GUIContent(mList[i].label), GUILayout.ExpandWidth(false) );
						if( _tmp != mList[i].visible )
						{
							// developer chanage debugGUI show/hide
							string _winKey = "DevGUI_"+mList[i].label;
							if( mList[i].visible && !PlayerPrefs.HasKey(_winKey) )
							{
								PlayerPrefs.SetInt(_winKey, 1 );
							}
							else if( !mList[i].visible && PlayerPrefs.HasKey(_winKey) )
							{
								PlayerPrefs.DeleteKey(_winKey);
							}
							PlayerPrefs.Save();
						}
						GUILayout.Space(10.0f);
					}
				}
				GUILayout.EndHorizontal();
				GUILayout.EndScrollView();
			}
			GUILayout.EndHorizontal();
		}
		private void drawRegistedWindowTask()
		{
			for(int i=0; i<mList.Count; i++)
			{
				WindowTask _tmp = mList[i];
				// when it's going to visible.
				if( _tmp.visible )
				{
					// Auto Full Screen
					if( _tmp.autoFullScreen )
					{	// when Full Screen
						_tmp.defaultPos.width = Screen.width;
						_tmp.defaultPos.height = Screen.height-PANEL_HEIGHT;
					}//if
					// When Developer design. not allow to Overlap DevManager panel.
					if( _tmp.defaultPos.y < PANEL_HEIGHT ) _tmp.defaultPos.y = PANEL_HEIGHT;
					if( _tmp.defaultPos.height+PANEL_HEIGHT > Screen.height )
					{
						_tmp.defaultPos.height = Screen.height-PANEL_HEIGHT;
					}
					// Allow Drag window or not
					if( _tmp.allowDrag )
					{
						_tmp.defaultPos = GUI.Window( _tmp.windowID, _tmp.defaultPos, _tmp.UpdateGUI, "", mWindowStyle); // _tmp.label
					}
					else
					{
						GUI.Window( _tmp.windowID, _tmp.defaultPos, _tmp.UpdateGUI, "", mWindowStyle);
					}//if
				}
				mList[i] = _tmp;
			}	
		}
		
		/// <summary>
		/// Display DevManager Interface
		/// </summary>
		public void Show()
		{
			mVisible = true;
		}
		/// <summary>
		/// Hide DevManager Interface
		/// </summary>
		public void Hide()
		{
			mVisible = false;
		}
		/// <summary>
		/// Toggle DevManager Interface
		/// </summary>
		public void Toggle()
		{
			mVisible=!mVisible;	
		}
		
		/// <summary>
		/// Reg the specified debug GUI for any other function.
		/// simply using "Debug.Log" for debug.
		/// </summary>
		/// <param name="_label">label of window you want to display.</param>
		/// <param name="_defaultPos">_default position & size of the window</param>
		/// <param name="_callback">_callback OnGUI() function for your OWN debug panel</param>
		/// <param name="_allowDrag">_allow drag of the window.</param>
		public void reg(string _label, Rect _defaultPos, DrawGUI _callback, bool _allowDrag)
		{
			mWindowCount++;	// zero was used by manager bar
			WindowTask _win = new WindowTask(mWindowCount,_label,_defaultPos,_callback,_allowDrag);
			_win.visible = checkLastPrefsVisible(_label);
			mList.Add (_win);
			
		}
		/// <summary>
		/// Reg the specified debug GUI for any other function.
		/// simply using "Debug.Log" for debug.
		/// </summary>
		/// <param name="_label">label of window you want to display.</param>
		/// <param name="_callback">_callback OnGUI() function for your OWN debug panel</param>
		/// <param name="_allowDrag">_allow drag of the window.</param>
		public void reg(string _label, DrawGUI _callback, bool _allowDrag)
		{
			mWindowCount++;	// zero was used by manager bar
			WindowTask _win = new WindowTask(mWindowCount,_label,_callback,_allowDrag);
			_win.visible = checkLastPrefsVisible(_label);
			mList.Add (_win);
		}
		/// <summary>
		/// Reg the specified debug GUI for any other function.
		/// simply using "Debug.Log" for debug.
		/// </summary>
		/// <param name="_label">label of window you want to display.</param>
		/// <param name="_callback">_callback OnGUI() function for your OWN debug panel</param>
		public void reg(string _label, DrawGUI _callback)
		{
			mWindowCount++;	// zero was used by manager bar
			WindowTask _win = new WindowTask(mWindowCount,_label,_callback,false);
			_win.visible = checkLastPrefsVisible(_label);
			mList.Add (_win);
		}
		
		private bool checkLastPrefsVisible(string _winLabel)
		{
			return PlayerPrefs.HasKey("DevGUI_"+_winLabel);
		}
	}//class
}// namespace
