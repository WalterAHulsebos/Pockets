//      
//   ^\.-
//  c====ɔ   Crafted with <3 by Nate Tessman
//   L__J    nate@madgvox.com
// 
//
//

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if ODIN_INSPECTOR
using ScriptableObject = Sirenix.OdinInspector.SerializedScriptableObject;
#endif

using Utilities.CGTK;

public class MultiScene : ScriptableObject
{
	[Serializable]
	public struct SceneInfo
	{
		public SceneReference asset;
		public bool loadScene;

		public SceneInfo(SceneReference asset, bool loadScene = true)
		{
			this.asset = asset;
			this.loadScene = loadScene;
		}
	}

	public SceneReference activeScene;
	public List<SceneInfo> sceneAssets = new List<SceneInfo>();
}