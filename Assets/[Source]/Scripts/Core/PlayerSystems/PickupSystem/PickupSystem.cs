using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.PlayerSystems.Movement;
using CGDebug = Utilities.CGTK.CGDebug;
using Rewired;
using PlayerController = Core.PlayerSystems.Movement.PlayerController;

using Sirenix.OdinInspector;
using Math = Utilities.Extensions.Math;

#if Odin_Inspector
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

namespace Core.PlayerSystems
{
	//[RequireComponent(PlayerController), RequireComponent(PlayerCamera)]
	 public class PickupSystem : MonoBehaviour
	 {
		 #region Variables

		 [SerializeField] private LayerMask pickupLayermask = 1 << 14;
		 
		 [SerializeField] private float grabRange = 10f;
		 [SerializeField] private Vector3 holdOffset = new Vector3(2,0,0);
		 
		 [FoldoutGroup("Throw Force")]
		 
		 [HorizontalGroup("Throw Force/Group", 0.5f, LabelWidth = 60)] 
		 [LabelText("Min")] [SerializeField] private float minThrowForce = 0f;
		 [HorizontalGroup("Throw Force/Group")]
		 [LabelText("Max")] [SerializeField] private float maxThrowForce = 25f;
		 
		 [FoldoutGroup("Throw Force")]
		 [SerializeField] private float chargeTime = 0.75f;
		 
		 [FoldoutGroup("Throw Force")]
		 [SerializeField] private ForceMode throwForceMode;

		 private PlayerController playerController = null;

		 private PlayerCamera playerCamera = null;

		 private bool holdingAnObject = false;

		 private Vector3 heldObjectMoveTowardsPosition = Vector3.zero;
		 
		 private float timeSinceStartedCharging = 0f;
		 private float chargePercentage = 0f;
		 
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
		 }

		 private void Update()
		 {
			 if(!ReInput.isReady) return;
			 if(!playerController.initialized) return;
			 if(InputPlayer == null) return;
			 
			 Transform cameraTransform = playerCamera.TargetTransforms[0]; 
			 
			 Ray ray = new Ray(cameraTransform.position, cameraTransform.forward * grabRange);
			 
			 Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
			 
			 //CGDebug.DrawRay(ray).Color(Color.cyan).Duration(1f);
			 
			 if(holdingAnObject == false)
			 {
				 if (!InputPlayer.GetButtonDown(PICKUP_BUTTON)) return;

				 if (!Physics.Raycast(ray, out RaycastHit hit, grabRange, pickupLayermask)) return;
				 
				 heldObject = hit.collider.transform;

				 holdingAnObject = true;
				 
				 //TODO: Walter - Edit this.
				 heldObjectRigidBody = heldObject.GetComponent<Rigidbody>();
				 //heldObjectCollider = heldObject.GetComponent<Collider>();
				 
				 heldObjectRigidBody.isKinematic = true;
				 //heldObjectCollider.enabled = false;
			 }
			 else
			 {
				 Transform heldTransform = heldObject.transform;

				 //Matrix4x4 matrix = Math.LocalMatrix(cameraTransform); //Matrix4x4.TRS(cameraTransform.position, cameraTransform.rotation, cameraTransform.localScale);
				 
				 Matrix4x4 matrix = Matrix4x4.TRS(cameraTransform.position, cameraTransform.rotation, cameraTransform.lossyScale);

				 heldTransform.position = matrix.MultiplyPoint3x4(holdOffset);
				 
				 //heldObjectMoveTowardsPosition = matrix.MultiplyPoint3x4(holdOffset);

				 if(InputPlayer.GetButtonDown(PICKUP_BUTTON))
				 {
					 timeSinceStartedCharging += Time.deltaTime;
					 chargePercentage = timeSinceStartedCharging / chargeTime;
				 }
				 
				 if (!InputPlayer.GetButtonUp(PICKUP_BUTTON)) return;

				 float throwForce = Mathf.Lerp(minThrowForce, maxThrowForce, chargePercentage);

				 timeSinceStartedCharging = 0f;
				 
				 heldObjectRigidBody.isKinematic = false;
				 //heldObjectCollider.enabled = true;
				 heldObjectRigidBody.AddForce(throwForce * cameraTransform.forward, throwForceMode);

				 holdingAnObject = false;
				 heldObject = null;
				 heldObjectRigidBody = null;
				 //heldObjectCollider = null;
			 }
		 }

		 private void FixedUpdate()
		 {
			 if (heldObjectRigidBody != null)
			 {
			 	//heldObjectRigidBody.MovePosition(heldObjectMoveTowardsPosition);
			 }
		 }

		 #endregion
	 }
}
