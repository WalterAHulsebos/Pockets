using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if Odin_Inspector
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

namespace Core.Movement
{
    public class TempCamera : MonoBehaviour
    {
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
        
    }
    
}