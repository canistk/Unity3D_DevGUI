/// <summary>
/// DevWindow
/// Sub-Class of DevManager
/// </summary>
using UnityEngine;
using System.Collections;

namespace DevelopManager
{
	/// <summary>
	/// handle Window task.
	/// </summary>
	public delegate void DrawGUI();
	
	public class WindowTask
	{
		public int windowID;
		public string label;
		public Rect defaultPos;
		public DrawGUI callback;
		public bool allowDrag;
		public bool autoFullScreen;
		public bool visible;
		private Vector2 mScrollPos;
		public WindowTask(int _windowID, string _label, Rect _defaultPos, DrawGUI _callback, bool _allowDrag)
		{
			windowID = _windowID;
			label = _label;
			defaultPos = _defaultPos;
			callback = _callback;
			allowDrag = _allowDrag;
			visible = false;
			autoFullScreen = false;
			mScrollPos = Vector2.zero;
		}
		public WindowTask(int _windowID, string _label, DrawGUI _callback, bool _allowDrag)
		{
			windowID = _windowID;
			label = _label;
			defaultPos = new Rect(0,0,Screen.width,Screen.height);
			callback = _callback;
			allowDrag = _allowDrag;
			visible = false;
			autoFullScreen = true;
			mScrollPos = Vector2.zero;
		}
		// Call a passed-in delegate function to draw GUI Window.
		public void UpdateGUI(int _id)
		{
			// function callback delegate ref.
			// http://msdn.microsoft.com/en-us/library/aa288459%28v=vs.71%29.aspx
			if( _id != windowID ) return;
			mScrollPos = GUILayout.BeginScrollView(mScrollPos);
			GUILayout.BeginVertical();
				callback();
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
			if( allowDrag ) GUI.DragWindow();
		}
	}
}//namespace
