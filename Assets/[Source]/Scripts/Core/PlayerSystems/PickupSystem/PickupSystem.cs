using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.PlayerSystems.Movement;
using CGDebug = Utilities.CGTK.CGDebug;
using Rewired;
using PlayerController = Core.PlayerSystems.Movement.PlayerController;

#if Odin_Inspector
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

namespace Core.PlayerSystems
{
	//[RequireComponent(PlayerController), RequireComponent(PlayerCamera)]
	 public class PickupSystem : MonoBehaviour
	 {
		 #region Variables

		 [SerializeField] private float grabRange = 10f;
		 
		 [SerializeField] private Vector3 holdOffset = new Vector3(2,0,0);

		 [SerializeField] private float chargeTime = 0.75f;
		 [SerializeField] private float throwForce = 25f;
		 [SerializeField] private ForceMode throwForceMode;
		 
		 [SerializeField] private LayerMask pickupLayermask = -1;

		 private PlayerController playerController = null;

		 private PlayerCamera playerCamera = null;

		 private float timeSinceStartedCharging = 0f;
		 private float chargePercentage = 0f;
		 
		 private Transform heldObject = null;
		 private Rigidbody heldObjectRigidBody = null;
		 private Collider heldObjectCollider = null;

		 private Player Player => playerController.Player;
		 
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

		 private void Update ()
		 {
			 Transform myTransform = this.transform;
			 
			 Ray ray = new Ray(myTransform.position, myTransform.forward * grabRange);
			 
			 //CGDebug.DrawRay(ray).Color(Color.cyan);
			 
			 if(heldObject == null)
			 {
				 if (!Player.GetButtonDown(PICKUP_BUTTON)) return;

				 if (!Physics.Raycast(ray, out RaycastHit hit, grabRange, pickupLayermask)) return;
				 
				 heldObject = hit.collider.transform;
				 
				 //TODO: Walter - Edit this.
				 heldObjectRigidBody = heldObject.GetComponent<Rigidbody>();
				 heldObjectCollider = GetComponent<Collider>();
				 
				 heldObjectRigidBody.isKinematic = true;
				 heldObjectCollider.enabled = false;
			 }
			 else
			 {
				 Transform heldTransform = heldObject.transform;

				 Matrix4x4 matrix = Matrix4x4.TRS(myTransform.position, myTransform.rotation, myTransform.localScale);

				 heldTransform.position = matrix.MultiplyPoint3x4(holdOffset);
				 heldTransform.rotation = (Quaternion.Inverse(playerCamera.TargetTransforms[0].rotation) * heldTransform.rotation);

				 if(Player.GetButtonDown(PICKUP_BUTTON))
				 {
					 timeSinceStartedCharging += Time.deltaTime;
					 chargePercentage = timeSinceStartedCharging / chargeTime;
				 }
				 
				 if (!Player.GetButtonUp(PICKUP_BUTTON)) return;

				 timeSinceStartedCharging = 0f;
				 
				 heldObjectRigidBody.isKinematic = false;
				 heldObjectCollider.enabled = true;
				 heldObjectRigidBody.AddForce(throwForce * myTransform.forward, throwForceMode);
				 
				 heldObject = null;
			 }
		 }
		 
		 #endregion
	 }
}
