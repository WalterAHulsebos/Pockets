using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

#if Odin_Inspector
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

public class MainMenu : Singleton<MainMenu>
{
	#region Variables

	[SerializeField] private MultiScene multiScene = null;
	
	#endregion

	#region Methods

	public void StartGame()
	{
		multiScene.Load();
	}

	public void ExitGame()
	{
		Application.Quit();
	}
	
	#endregion
	
}
