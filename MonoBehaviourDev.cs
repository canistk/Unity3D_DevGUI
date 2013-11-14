using UnityEngine;
using System;
using System.Collections;
/// <summary>
/// Mono behaviour for DevManager
/// </summary>
public class MonoBehaviourDev : MonoBehaviour {
	public string DevFunctionLabel = "";
	public bool DevWindowMoveable = true;
	private bool DevManagerExist = false;
	[System.NonSerializedAttribute]
	public bool DevDisplayGUI = false;
	
	public virtual void OnEnable()
	{
		if( DevFunctionLabel.Equals("") )
		{	// If you don't define, I make one for you.
			DevFunctionLabel = this.name.ToString();
		}
		if( PlayerPrefs.HasKey("DevManager") )
		{
			DevManagerExist = true;
			DevelopManager.DevManager.Instance.Reg(DevFunctionLabel, HandleDebugGUI, DevWindowMoveable);
			Debug.Log("\""+PlayerPrefs.GetString("DevManager")+"\" detected - "+DevFunctionLabel+" registed.");
		}
	}
	public virtual void OnDisable(){}
	public virtual void OnGUI()
	{
		if( !DevManagerExist && DevDisplayGUI )
		{
			try{ HandleDebugGUI(); }
			catch(ArgumentException){}
		}
	}
	/// <summary>override this function and do your own GUI over here.</summary>
	public virtual void HandleDebugGUI(){
		// DevManager will handle the rest.
	}
}
