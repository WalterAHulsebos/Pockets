using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using static Core.Utilities.Helpers;

#if Odin_Inspector
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

namespace Core.Movement
{
    public class TempCamera : MonoBehaviour
    {
        #region Variables
        
        public	Transform	targetTrans;
        
        public bool inputActive = true;
        public bool controlCursor = false;
        public bool pitchClamp	= true;
        
        [Range(-90f, 90f)] [SerializeField] private float defaultVerticalAngle = 20f;
        
        [ShowIf("pitchClamp")]
        [Range(-90f, 90f)] [SerializeField] private float minVerticalAngle = -90f, 
                                                          maxVerticalAngle = 90f;
        
        [Header("Smoothing")]
        public bool byPassSmoothing	= false;
        public float mouseSmoothing = 20f;	//Lambda | higher = less latency but also less smoothing
        
        [Header("Sensitivity")]
        [HorizontalGroup("Sensitivity")]
        public float horizontalSensitivity = 4f;
        public float verticalSensitivity = 4f;
        
        public BufferV2	mouseBuffer = new BufferV2();
        
        #region Accessors

        public Transform Transform { get; private set; }
        
        #endregion
        
        #endregion
        
        #region Methods

        private void Awake()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateWithInput(Vector3 rotationInput)
        {
            //if(Input.GetKeyDown(KeyCode.Space)){inputActive = !inputActive;}
            if(controlCursor)
            {	//Cursor Control
                if (inputActive && Cursor.lockState != CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }

                if (!inputActive && Cursor.lockState != CursorLockMode.None)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
            }		
            if(!inputActive){ return; }	//active?

            UpdateMouseBuffer(rotationInput);
            targetTrans.rotation = Quaternion.Euler(mouseBuffer.curAbs);
        }

        //consider late Update for applying the rotation if your game needs it (e.g. if camera parents are rotated in Update for some reason)
        private void LateUpdate() {}

        private	void UpdateMouseBuffer(Vector3 rotationInput)
        {
            mouseBuffer.target += new Vector2( verticalSensitivity * rotationInput.y, horizontalSensitivity * rotationInput.x); //Mouse Input is inherently framerate independend!
            
            mouseBuffer.target.x = pitchClamp
                ? Mathf.Clamp(mouseBuffer.target.x, minVerticalAngle, maxVerticalAngle) 
                : mouseBuffer.target.x;
            
            mouseBuffer.Update(mouseSmoothing, Time.deltaTime, byPassSmoothing);
        }
        
        #endregion
        
        /*
        #region Variables

        #region Serialized

        [Header("Framing")]
#pragma warning disable 108,114
        [SerializeField]
        private Camera camera;
#pragma warning restore 108,114
        [SerializeField] private Vector2 followPointFraming = new Vector2(0f, 0f);
        [SerializeField] private float followingSharpness = 10000f;

        [Header("Distance")] [SerializeField] public float defaultDistance = 6f;
        [SerializeField] private float minDistance = 0f;
        [SerializeField] private float maxDistance = 10f;
        [SerializeField] private float distanceMovementSpeed = 5f;
        [SerializeField] private float distanceMovementSharpness = 10f;

        [Header("Rotation")] [SerializeField] private bool invertX = false;
        [SerializeField] private bool invertY = false;
        [Range(-90f, 90f)] [SerializeField] private float defaultVerticalAngle = 20f;
        [Range(-90f, 90f)] [SerializeField] private float minVerticalAngle = -90f;
        [Range(-90f, 90f)] [SerializeField] private float maxVerticalAngle = 90f;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float rotationSharpness = 10000f;

        [Header("Obstruction")] [SerializeField]
        private float obstructionCheckRadius = 0.2f;

        [SerializeField] private LayerMask obstructionLayers = -1;
        [SerializeField] private float obstructionSharpness = 10000f;
        [SerializeField] private List<Collider> ignoredColliders = new List<Collider>();

        #endregion

        #region Non-Serialized

        #region Accessors

        public Transform Transform { get; private set; }
        public Vector3 PlanarDirection { get; private set; }
        public Transform FollowTransform { get; private set; }
        public float TargetDistance { get; set; }

        #endregion

        private bool distanceIsObstructed;
        private float currentDistance;
        private float targetVerticalAngle;
        private RaycastHit obstructionHit;
        private int obstructionCount;
        private RaycastHit[] obstructions = new RaycastHit[MAX_OBSTRUCTIONS];
        private float obstructionTime;
        private Vector3 currentFollowPosition;

        private const int MAX_OBSTRUCTIONS = 32;

        #endregion

        #endregion

        #region Methods

        void OnValidate()
        {
            defaultDistance = Mathf.Clamp(defaultDistance, minDistance, maxDistance);
            defaultVerticalAngle = Mathf.Clamp(defaultVerticalAngle, minVerticalAngle, maxVerticalAngle);
        }

        void Awake()
        {
            Transform = this.transform;

            currentDistance = defaultDistance;
            TargetDistance = currentDistance;

            targetVerticalAngle = 0f;

            PlanarDirection = Vector3.forward;
        }

        // Set the transform that the camera will orbit around
        public void SetFollowTransform(Transform t)
        {
            FollowTransform = t;
            PlanarDirection = FollowTransform.forward;
            currentFollowPosition = FollowTransform.position;
        }

        public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
        {
            if (!FollowTransform) return;

            if (invertX)
            {
                rotationInput.x *= -1f;
            }

            if (invertY)
            {
                rotationInput.y *= -1f;
            }

            // Process rotation input
            Quaternion rotationFromInput = Quaternion.Euler(FollowTransform.up * (rotationInput.x * rotationSpeed));
            PlanarDirection = rotationFromInput * PlanarDirection;
            PlanarDirection = Vector3.Cross(FollowTransform.up, Vector3.Cross(PlanarDirection, FollowTransform.up));
            targetVerticalAngle -= (rotationInput.y * rotationSpeed);
            targetVerticalAngle = Mathf.Clamp(targetVerticalAngle, minVerticalAngle, maxVerticalAngle);

            // Process distance input
            if (distanceIsObstructed && Mathf.Abs(zoomInput) > 0f)
            {
                TargetDistance = currentDistance;
            }

            TargetDistance += zoomInput * distanceMovementSpeed;
            TargetDistance = Mathf.Clamp(TargetDistance, minDistance, maxDistance);

            // Find the smoothed follow position
            currentFollowPosition = Vector3.Lerp(currentFollowPosition, FollowTransform.position,
                1f - Mathf.Exp(-followingSharpness * deltaTime));

            // Calculate smoothed rotation
            Quaternion planarRot = Quaternion.LookRotation(PlanarDirection, FollowTransform.up);
            Quaternion verticalRot = Quaternion.Euler(targetVerticalAngle, 0, 0);
            Quaternion targetRotation = Quaternion.Slerp(Transform.rotation, planarRot * verticalRot,
                1f - Mathf.Exp(-rotationSharpness * deltaTime));

            // Apply rotation
            Transform.rotation = targetRotation;

            // Handle obstructions
            {
                RaycastHit closestHit = new RaycastHit();
                closestHit.distance = Mathf.Infinity;
                obstructionCount = Physics.SphereCastNonAlloc(currentFollowPosition, obstructionCheckRadius,
                    -Transform.forward, obstructions, TargetDistance, obstructionLayers,
                    QueryTriggerInteraction.Ignore);
                for (int i = 0; i < obstructionCount; i++)
                {
                    bool isIgnored = false;
                    foreach (Collider t in ignoredColliders)
                    {
                        if (t != obstructions[i].collider) continue;

                        isIgnored = true;
                        break;
                    }

                    if (!isIgnored && obstructions[i].distance < closestHit.distance && obstructions[i].distance > 0)
                    {
                        closestHit = obstructions[i];
                    }
                }

                // If obstructions detecter
                if (closestHit.distance < Mathf.Infinity)
                {
                    distanceIsObstructed = true;
                    currentDistance = Mathf.Lerp(currentDistance, closestHit.distance,
                        1 - Mathf.Exp(-obstructionSharpness * deltaTime));
                }
                // If no obstruction
                else
                {
                    distanceIsObstructed = false;
                    currentDistance = Mathf.Lerp(currentDistance, TargetDistance,
                        1 - Mathf.Exp(-distanceMovementSharpness * deltaTime));
                }
            }

            // Find the smoothed camera orbit position
            Vector3 targetPosition = currentFollowPosition - ((targetRotation * Vector3.forward) * currentDistance);

            // Handle framing
            targetPosition += Transform.right * followPointFraming.x;
            targetPosition += Transform.up * followPointFraming.y;

            // Apply position
            Transform.position = targetPosition;
        }

        #endregion
        */
        
    }
    
}