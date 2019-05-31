using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LetterboxCamera;
using Utilities;
using Utilities.Extensions;

public class CameraManager : Singleton<CameraManager>
{

    #region Variables
    
    [SerializeField] private ForceCameraRatio forceRatio;

    [SerializeField] private Camera[] singlePlayerCameras;
    [SerializeField] private Camera[] twoPlayerCameras;
    [SerializeField] private Camera[] threePlayerCameras;
    [SerializeField] private Camera[] fourPlayerCameras;

    //public int startingDefault = 0;
    //public Vector2[] defaultRatios;

    //private float guiColorProgress = 1f;
    //private int currentDefault = 0;

    #endregion

    #region Unity Default Functions

    /// <summary>
    /// Validate any insecure variables
    /// </summary>
    private void Awake ()
    {
        //currentDefault = startingDefault;
        singlePlayerCameras.For(cam => cam.enabled = true);
        twoPlayerCameras.For(cam => cam.enabled = false);
        threePlayerCameras.For(cam => cam.enabled = false);
        fourPlayerCameras.For(cam => cam.enabled = false);
    }

    #endregion
}
