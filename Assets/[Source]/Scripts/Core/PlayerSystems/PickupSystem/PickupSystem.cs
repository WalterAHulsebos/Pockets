using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.PlayerSystems.Movement;
using CGDebug = Utilities.CGTK.CGDebug;
using Rewired;
using PlayerController = Core.PlayerSystems.Movement.PlayerController;

using Sirenix.OdinInspector;
using Math = Utilities.Extensions.Math;
using Core.PlayerSystems.Ballistics;
using Lean.Pool;

#if Odin_Inspector
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

namespace Core.PlayerSystems
{
	//[RequireComponent(PlayerController), RequireComponent(PlayerCamera)]
	 public class PickupSystem : MonoBehaviour
	 {
		 #region Variables

		 #region Serialized
		 
		 [FoldoutGroup("Grab Settings")]
		 [SerializeField] private LayerMask pickupLayermask = 1 << 14;
		 
		 [FoldoutGroup("Grab Settings")]
		 [SerializeField] private float grabRange = 10f;
		 
		 [FoldoutGroup("Grab Settings")]
		 [SerializeField] private Vector3 holdOffset = new Vector3(2,0,0);

		 //[SerializeField] private float objectFollowingSpeed = 10f;
		 
		 [FoldoutGroup("Sensitivity")]
		 [SerializeField] private Vector2 rotationSensitivity = new Vector3(1,1,1);
		 
		 [FoldoutGroup("Throwing")]
		 
		 [HorizontalGroup("Throwing/Group", 0.5f, LabelWidth = 60)] 
		 [LabelText("Min")] [SerializeField] private float minThrowForce = 0f;
		 [HorizontalGroup("Throwing/Group")]
		 [LabelText("Max")] [SerializeField] private float maxThrowForce = 25f;
		 
		 [FoldoutGroup("Throwing")]
		 [SerializeField] private float chargeTime = 0.75f;
		 
		 [FoldoutGroup("Throwing")]
		 [SerializeField] private ForceMode throwForceMode;

		 [FoldoutGroup("Throwing")]
		 [SerializeField] private float launchAngle = 30f;
		 
		 [FoldoutGroup("Throwing")]
		 [Required]
		 [SerializeField] private PhysicMaterial defaultPhysicsMaterial = null;
		 
		 
		 #if UNITY_EDITOR
		 [FoldoutGroup("Object References"), Required, AssetsOnly,
		  Tooltip("Used to show the ballistic trajectory of your projectile.")]
		 #endif
		 [SerializeField] private GameObject trajectoryDebugObject;
		
		 #if UNITY_EDITOR
		 [FoldoutGroup("Object References"), Required, AssetsOnly,
		  Tooltip("Used to mark the end of a trajectory.")]
		 #endif
		 [SerializeField] private GameObject trajectoryEndMarkObject;

		 #endregion
		 
		 private PlayerController playerController = null;

		 private PlayerCamera playerCamera = null;
		 	
		 private Vector3 projectileDirection => Quaternion.AngleAxis(launchAngle, Vector3.left) * heldObject.forward;

		 private readonly BallisticTrajectoryPredictor trajectoryPredictor = new BallisticTrajectoryPredictor()
		 {
			 accuracy = .95f,
			 iterationLimit = 1500
		 };

		 private bool holdingAnObject = false;

		 private bool initialGrab = false;

		 private Vector3 heldObjectMoveTowardsPosition = Vector3.zero;
		 
		 private float timeSinceStartedCharging = 0f;
		 
		 private Vector2 rotationInput = Vector3.zero;

		 private Transform cameraTransform;
		 
		 private List<Vector3> trajectory = null;

		 private float throwForce = 0;
		 
		 [ReadOnly]
		 [SerializeField] private float chargePercentage = 0f;
		 
		 private Transform heldObject = null;
		 private Rigidbody heldObjectRigidBody = null;
		 private Collider heldObjectCollider = null;

		 private Player InputPlayer => playerController.Player;
		 
		 #region Consts

		 private const string PICKUP_BUTTON = "Interact";
		 private const string ROTATE_HORIZONTAL = "Move Horizontal";
		 private const string ROTATE_VERTICAL = "Move Vertical";
		
		 #endregion

		 #endregion
	 
		 #region Methods

		 private void Awake()
		 {
			 playerController = playerController ?? GetComponentInChildren<PlayerController>();
			 playerCamera = playerCamera ?? GetComponentInChildren<PlayerCamera>();
			 
			 trajectoryPredictor.raycastMask = ~pickupLayermask;
		 }

		 private void Update()
		 {
			 if(!ReInput.isReady) return;
			 if(!playerController.initialized) return;
			 if(InputPlayer == null) return;
			 
			 cameraTransform = playerCamera.TargetTransforms[0]; 
			 
			 Ray ray = new Ray(cameraTransform.position, cameraTransform.forward * grabRange);
			 
			 Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
			 
			 trajectoryPredictor.debugLineDuration = Time.deltaTime;
			 
			 LeanPool.DespawnAll();
			 
			 if(holdingAnObject == false)
			 {
				 if (!InputPlayer.GetButtonDown(PICKUP_BUTTON)) return;

				 if (!Physics.Raycast(ray, out RaycastHit hit, grabRange, pickupLayermask)) return;
				 
				 heldObject = hit.collider.transform;

				 holdingAnObject = true;
				 
				 heldObjectRigidBody = heldObject.GetComponent<Rigidbody>();
				 heldObjectCollider = heldObject.GetComponent<Collider>();
				 
				 heldObjectRigidBody.isKinematic = true;

				 initialGrab = true;
				 //heldObjectCollider.enabled = false;
			 }
			 else
			 {
				 Transform heldTransform = heldObject.transform;

				 Matrix4x4 matrix = Math.LocalMatrix(cameraTransform);

				 heldTransform.position = matrix.MultiplyPoint3x4(holdOffset);
				 
				 /*
				 rotationInput = new Vector3(
					 InputPlayer.GetAxis(ROTATE_HORIZONTAL), 
					 InputPlayer.GetAxis(ROTATE_VERTICAL), 
					 0);
					 */
				 
				 Debug.Log($"rotationInput = {rotationInput}");
				 
				 //heldObjectMoveTowardsPosition = matrix.MultiplyPoint(holdOffset);

				 if(InputPlayer.GetButtonDown(PICKUP_BUTTON))
				 {
					 initialGrab = false;
				 }
				 
				 if(initialGrab){return;}
				 
				 if(InputPlayer.GetButton(PICKUP_BUTTON))
				 {
					 timeSinceStartedCharging += Time.deltaTime;
					 chargePercentage = timeSinceStartedCharging / chargeTime;
					 
					 throwForce = Mathf.Lerp(minThrowForce, maxThrowForce, chargePercentage);
				
					 trajectory = trajectoryPredictor.GetTrajectoryPoints
					 (
						 startPos: heldTransform.position,
						 velocity: projectileDirection * throwForce,
						 gravity: Physics.gravity,
						 physicMaterial: defaultPhysicsMaterial
						 //physicMaterial: heldObjectCollider.material
					 );
				 }
				 
				 
				 foreach(Vector3 orbPos in trajectory)
				 {
					 if(trajectoryDebugObject){LeanPool.Spawn(trajectoryDebugObject, orbPos, Quaternion.identity);}
				 }
				 
				 if (!(chargePercentage >= 0.05 )) return;
				 if(!InputPlayer.GetButtonUp(PICKUP_BUTTON)) return;

				 //LeanPool.DespawnAll();

				 timeSinceStartedCharging = 0f;
				 
				 heldObjectRigidBody.isKinematic = false;
				 //heldObjectCollider.enabled = true;
				 heldObjectRigidBody.AddForce(throwForce * projectileDirection, throwForceMode);

				 holdingAnObject = false;
				 heldObject = null;
				 heldObjectRigidBody = null;
				 //heldObjectCollider = null;
			 }
		 }

		 /*
		 private void FixedUpdate()
		 {
			 if (heldObjectRigidBody == null) return;
			 
			 //heldObjectRigidBody.AddTorque(transform.up * rotationInput.y * rotationSensitivity.y);
			 //heldObjectRigidBody.AddTorque(transform.right * rotationInput.x * rotationSensitivity.x);
			 
			 //Vector3 direction = (heldObjectMoveTowardsPosition - heldObject.position).normalized;
			 //heldObjectRigidBody.MovePosition(heldObject.position + direction * objectFollowingSpeed * Time.deltaTime);

			 //heldObjectRigidBody.MovePosition(heldObjectMoveTowardsPosition); // * Time.fixedDeltaTime);
		 }
		 */

		 #endregion
	 }
}
