using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LetterboxCamera;
using Utilities;
using Utilities.Extensions;
using System;

using Rewired;
using Enumerable = System.Linq.Enumerable;
    
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Core.Managers
{   
    public class CameraManager : Singleton<CameraManager>
    {
        #region Variables
    
        [OdinSerialize]
        public static IEnumerable cameraGroups = new ValueDropdownList<int>()
        {
            { "SinglePlayer", 0 },
            { "TwoPlayer", 1 },
            { "ThreePlayer", 2 },
            { "FourPlayer", 3 }
        };
        
        [SerializeField] private Vector2 ratio = new Vector2(16, 9);
        [SerializeField] private bool forceRatioOnAwake = true;
        [SerializeField] private bool findCamerasAutomatically = true;

        #region LetterBoxCamera
        
        [SerializeField] private bool useLetterBoxCamera = true;
        [ShowIf("useLetterBoxCamera")]
        [SerializeField] private Color letterBoxCameraColor = new Color(0, 0, 0, 1);
        [ShowIf("useLetterBoxCamera")]
        [SerializeField] private bool createLetterBoxCamera = true;
        [ShowIf("useLetterBoxCamera"), HideIf("createLetterBoxCamera")]
        [SerializeField] private Camera letterBoxCamera;
        
        #endregion

        public List<CameraRatio> cameras;
        private bool isletterBoxCameraNotNull;

        private const int DEFAULT_PLAYER_COUNT = 0;
        
        [Serializable]
        /// <summary> A class for tracking individual Cameras and their Viewports </summary>
        public sealed class CameraRatio
        {
            private IEnumerable cameraGroups => CameraManager.cameraGroups;

            public enum CameraAnchor
            {
                Center,
                Top,
                Bottom,
                Left,
                Right,
                TopLeft,
                TopRight,
                BottomLeft,
                BottomRight
            }
    
            [ValueDropdown("cameraGroups")]
            public int cameraGroup;
            
            [Tooltip("The Camera assigned to have an automatically calculated Viewport Ratio")]
            [HorizontalGroup]
            public Camera camera;

            [Tooltip(
                "When a Camera Viewport is shrunk to fit a ratio, it will anchor the new Viewport Rectangle at the given point (relative to the original, unshrunk Viewport)")]
            [HorizontalGroup]
            public CameraAnchor anchor = CameraAnchor.Center;
    
            [HideInInspector]
            public Vector2 vectorAnchor;
            private Rect originViewPort;
    
            public CameraRatio(Camera camera, Vector2 anchor)
            {
                this.camera = camera;
                this.vectorAnchor = anchor;
                this.originViewPort = camera.rect;
            }
    
            /// <summary> Sets the Camera's current Viewport as the viewport measurements to fill on resizing </summary>
            public void ResetOriginViewport ()
            {
                originViewPort = camera.rect;
                SetAnchorBasedOnEnum();
            }
    
            /// <summary> Sets the Anchor for this Camera when it is resized based on a given enum description </summary>
            public void SetAnchorBasedOnEnum()
            {
                switch (anchor)
                {
                    case CameraAnchor.Center:
                        vectorAnchor = new Vector2(0.5f, 0.5f);
                        break;
                    case CameraAnchor.Top:
                        vectorAnchor = new Vector2(0.5f, 1f);
                        break;
                    case CameraAnchor.Bottom:
                        vectorAnchor = new Vector2(0.5f, 0f);
                        break;
                    case CameraAnchor.Left:
                        vectorAnchor = new Vector2(0f, 0.5f);
                        break;
                    case CameraAnchor.Right:
                        vectorAnchor = new Vector2(1f, 0.5f);
                        break;
                    case CameraAnchor.TopLeft:
                        vectorAnchor = new Vector2(0f, 1f);
                        break;
                    case CameraAnchor.TopRight:
                        vectorAnchor = new Vector2(1f, 1f);
                        break;
                    case CameraAnchor.BottomLeft:
                        vectorAnchor = new Vector2(0f, 0f);
                        break;
                    case CameraAnchor.BottomRight:
                        vectorAnchor = new Vector2(1f, 0f);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
    
            /// <summary>
            /// Forces a camera to render at a given ratio
            /// Creates a letter box effect if the new viewport does not match the current Window ratio
            /// </summary>
            /// <param name="_targetAspect"></param>
            /// <param name="_currentAspect"></param>
            public void CalculateAndSetCameraRatio (float _width, float _height, bool _horizontalLetterbox) {
    
                Rect localViewPort = new Rect();
    
                // Force the viewport to a width and height accurate to the target ratio
                if (_horizontalLetterbox) { // current aspect is wider than target aspect so shorten down height of the viewport
                    localViewPort.height = _height;
                    localViewPort.width = 1;
    
                } else { // current aspect is taller than target aspect so thin down width of the viewport
                    localViewPort.height = 1f;
                    localViewPort.width = _width;
                }
    
                // Resize and position the viewport to fit in it's original position on screen (adhering to a given anchor point)
                Rect screenViewPortHorizontal = new Rect();
                Rect screenViewPortVertical = new Rect();
    
                // Calculate both a horizontally and vertically resized viewport
                screenViewPortHorizontal.width = originViewPort.width;
                screenViewPortHorizontal.height = originViewPort.width * (localViewPort.height / localViewPort.width);
                screenViewPortHorizontal.x = originViewPort.x;
                screenViewPortHorizontal.y = Mathf.Lerp(originViewPort.y, originViewPort.y + (originViewPort.height - screenViewPortHorizontal.height), vectorAnchor.y);
    
                screenViewPortVertical.width = originViewPort.height * (localViewPort.width / localViewPort.height);
                screenViewPortVertical.height = originViewPort.height;
                screenViewPortVertical.x = Mathf.Lerp(originViewPort.x, originViewPort.x + (originViewPort.width - screenViewPortVertical.width), vectorAnchor.x);
                screenViewPortVertical.y = originViewPort.y;
    
                // Use the best fitting of the two
                if (screenViewPortHorizontal.height >= screenViewPortVertical.height && screenViewPortHorizontal.width >= screenViewPortVertical.width) {
                    if (screenViewPortHorizontal.height <= originViewPort.height && screenViewPortHorizontal.width <= originViewPort.width) {
                        camera.rect = screenViewPortHorizontal;
                    } else {
                        camera.rect = screenViewPortVertical;
                    }
                } else {
                    if (screenViewPortVertical.height <= originViewPort.height && screenViewPortVertical.width <= originViewPort.width) {
                        camera.rect = screenViewPortVertical;
                    } else {
                        camera.rect = screenViewPortHorizontal;
                    }
                }
            }
        }
    
        #endregion
    
        #region Methods
    
        /// <summary>
        /// Validate any insecure variables
        /// </summary>
        private void Awake()
        {
            isletterBoxCameraNotNull = letterBoxCamera != null;
            // If no cameras have been assigned in editor, search for cameras in the scene
            if (findCamerasAutomatically)
            {
                FindAllCamerasInScene();
            }
            else if (cameras == null || cameras.Count == 0)
            {
                cameras = new List<CameraRatio>();
            }

            ValidateCameraArray();

            // Set the origin viewport space for each Camera
            foreach(CameraRatio cameraRatio in cameras)
            {
                cameraRatio.ResetOriginViewport();
            }

            // If requested, a Camera will be generated that renders a letter box Color
            if (createLetterBoxCamera)
            {
                GameObject letterBoxCameraObj = new GameObject();
                letterBoxCamera = letterBoxCameraObj.AddComponent<Camera>();

                letterBoxCamera.backgroundColor = letterBoxCameraColor;
                letterBoxCamera.cullingMask = 0;
                letterBoxCamera.depth = -100;
                letterBoxCamera.farClipPlane = 1;
                letterBoxCamera.useOcclusionCulling = false;
                letterBoxCamera.allowHDR = false;
                letterBoxCamera.clearFlags = CameraClearFlags.Color;
                letterBoxCamera.name = "Letter Box Camera";

                foreach (CameraRatio cameraRatio in cameras)
                {
                    if (cameraRatio.camera.depth.Approximately(-100))
                    {
                        Debug.LogError(cameraRatio.camera.name + " has a depth of -100 and may conflict with the Letter Box Camera in Forced Camera Ratio!");
                    }
                }
            }

            if (forceRatioOnAwake)
            {
                CalculateAndSetAllCameraRatios();
            }

            GetCamerasFromGroup(DEFAULT_PLAYER_COUNT).For(cam => cam.camera.enabled = true);
            GetCamerasNotFromGroup(DEFAULT_PLAYER_COUNT).For(cam => cam.camera.enabled = false);
            
        }
        
        private void Update()
        {
            //if (!listenForWindowChanges) return;
            
            CalculateAndSetAllCameraRatios();
            
            if (isletterBoxCameraNotNull)
            {
                letterBoxCamera.backgroundColor = letterBoxCameraColor;
            }
            
            if(!ReInput.isReady) return;
            
            AssignJoysticksToPlayers();
        }

        #region Camera Management

        /// <summary>
        /// Returns the container class for a Camera and it's ratio by the _camera it contains
        /// Returns null if the given _camera is not being tracked
        /// </summary>
        /// <param name="_camera"></param>
        /// <returns></returns>
        private CameraRatio GetCameraRatioByCamera(Camera camera)
        {
            if (cameras == null) return null;

            foreach (CameraRatio cameraRatio in cameras)
            {
                if (cameraRatio != null && cameraRatio.camera == camera)
                {
                    return cameraRatio;
                }
            }

            return null;
        }

        /// <summary>
        /// Removes any null elements from the CameraRatio Array
        /// </summary>
        private void ValidateCameraArray()
        {
            for (int i = cameras.Count - 1; i >= 0; i--)
            {
                if (cameras[i].camera == null)
                {
                    cameras.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Populates the tracked Camera Array with every Camera currently in the scene
        /// </summary>
        public void FindAllCamerasInScene()
        {
            Camera[] allCameras = FindObjectsOfType<Camera>();
            cameras = new List<CameraRatio>();

            foreach (Camera cam in allCameras)
            {
                if ((createLetterBoxCamera || cam != letterBoxCamera))
                { // Ignore the Custom LetterBox Camera
                    cameras.Add(new CameraRatio(cam, new Vector2(0.5f, 0.5f)));
                }
            }
        }

        /// <summary>
        /// Loops through all cameras in scene (or that have been set in editor)
        /// Forces each camera to render at a given ratio
        /// Creates a letter box effect if the new viewport does not match the current Window ratio
        /// </summary>
        public void CalculateAndSetAllCameraRatios()
        {
            float targetAspect = ratio.x / ratio.y;
            float currentAspect = ((float)Screen.width) / ((float)Screen.height);

            bool horizontalLetterbox = false;
            float fullWidth = targetAspect / currentAspect;
            float fullHeight = currentAspect / targetAspect;

            if (currentAspect > targetAspect)
            {
                horizontalLetterbox = false;
            }

            foreach (CameraRatio cam in cameras)
            {
                //cameras[i].SetAnchorBasedOnEnum(cameras[i].anchor);
                cam.SetAnchorBasedOnEnum();
                cam.CalculateAndSetCameraRatio(fullWidth, fullHeight, horizontalLetterbox);
            }
        }

        /// <summary>
        /// Set the anchor for a given Camera
        /// </summary>
        /// <param name="_camera"></param>
        /// <param name="_anchor"></param>
        public void SetCameraAnchor(Camera camera, Vector2 anchor)
        {
            CameraRatio cameraRatio = GetCameraRatioByCamera(camera);
            if (cameraRatio != null)
            {
                cameraRatio.vectorAnchor = anchor;
            }
        }

        public CameraRatio[] GetCameras()
        {
            if (cameras == null)
            {
                cameras = new List<CameraRatio>();
            }
            return cameras.ToArray();
        }

        public List<CameraRatio> GetCamerasFromGroup(int cameraGroup)
            => cameras.GetMultiple(func: (cameraRatio => cameraRatio.cameraGroup == cameraGroup));
        
        public List<CameraRatio> GetCamerasNotFromGroup(int cameraGroup)
            => cameras.GetMultiple(func: (cameraRatio => cameraRatio.cameraGroup != cameraGroup));

        #endregion

        #region Controller Management

        private void AssignJoysticksToPlayers()
        {
            // Check all joysticks for a button press and assign it tp
            // The first Player found without a joystick
            IList<Joystick> joysticks = ReInput.controllers.Joysticks;
            foreach (Joystick joystick in joysticks)
            {
                if(ReInput.controllers.IsControllerAssigned(joystick.type, joystick.id)) continue;
    
                // Check if a button was pressed on the joystick
                if (!joystick.GetAnyButtonDown()) continue;
                
                // Find the next Player without a Joystick
                Player player = FindPlayerWithoutJoystick();
                if(player == null) return; // no free joysticks
    
                // Assign the joystick to this Player
                player.controllers.AddController(joystick, false);
            }
    
            // If all players have joysticks, enable joystick auto-assignment
            if (!DoAllPlayersHaveJoysticks()) return;
            
            ReInput.configuration.autoAssignJoysticks = true;
            this.enabled = false; // disable this script
        }
    
        // Searches all Players to find the next Player without a Joystick assigned
        private Player FindPlayerWithoutJoystick()
        {
            IList<Player> players = ReInput.players.Players;
            
            return Enumerable.FirstOrDefault(players, player => player.controllers.joystickCount <= 0);
        }
    
        private bool DoAllPlayersHaveJoysticks()
        {
            return FindPlayerWithoutJoystick() == null;
        }

        #endregion
    
        #endregion
    }
}
