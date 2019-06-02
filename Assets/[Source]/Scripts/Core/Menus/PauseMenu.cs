using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Utilities;

using Sirenix.OdinInspector;

using JetBrains.Annotations;

#if Odin_Inspector
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

public class PauseMenu : Singleton<PauseMenu>
{
	#region Variables

	[SerializeField] private TMP_Text satisfactionText = null, organizationText = null, timeText = null;
	
	[SerializeField] private MultiScene mainScene = null, menuScene;
	
	[FoldoutGroup("Animation")]
	[Sirenix.OdinInspector.ReadOnly]
	[SerializeField] private bool inMenu = false;
	
	[FoldoutGroup("Animation")]
	[SerializeField] private Animator menuAnimator = null;
	
	[FoldoutGroup("Animation")]
	[SerializeField] private string menuEnableBoolName = "InMenu";

	private ScoreManager scoreManager;

	#endregion

	#region Methods

	private void OnDisable()
	{
		scoreManager.GameOverEvent += Animate;
	}

	private void Awake()
	{
		scoreManager = ScoreManager.Instance;

		scoreManager.GameOverEvent += Animate;

		scoreManager.satisfactionTextMesh = satisfactionText;
		scoreManager.organisationTextMesh = organizationText;
	}
	
	[UsedImplicitly]
	public void RestartGame()
	{
		mainScene.Load();
	}
	
	[UsedImplicitly]
	public void LoadMainMenu()
	{
		menuScene.Load();
	}
	
	[UsedImplicitly]
	public void ExitGame()
	{
		Application.Quit();
	}
	
	private void Animate()
	{
		inMenu = !inMenu;
        
		menuAnimator.SetBool(menuEnableBoolName, inMenu);
	}
	
	private void Animate(bool inMenu)
	{
		this.inMenu = inMenu;
        
		menuAnimator.SetBool(menuEnableBoolName, this.inMenu);
	}
	
	#endregion
	
}
