using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.PlayerSystems.Movement;

#if Odin_Inspector
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

namespace Core.PlayerSystems
{
	 public class PickupSystem : MonoBehaviour
	 {
		 #region Variables

		 [SerializeField] private float grabRange = 10f;
		 
		 [SerializeField] private Vector3 holdOffset = new Vector3(2,0,0);
		 
		 [SerializeField] private float throwForce = 25f;
		 [SerializeField] private ForceMode throwForceMode;
		 
		 [SerializeField] private LayerMask pickupLayermask = -1;

		 private PlayerController playerController = null;

		 private PlayerCamera playerCamera = null;
		 
		 private Transform heldObject = null;
		 private Rigidbody heldObjectRigidBody = null;
		 private Collider heldObjectCollider = null;
		 
		 #region Consts

		 private const string PICKUP_BUTTON = "Fire";
		 private const string ROTATE_HORIZONTAL = "Move Horizontal";
		 private const string ROTATE_VERTICAL = "Move Vertical";
		
		 #endregion

		 
		 #endregion
	 
		 #region Methods

		 private void Awake()
		 {
			 playerController = playerController ?? GetComponentInChildren<PlayerController>();
		 }

		 private void Update ()
		 {
			 if(heldObject == null)
			 {
				 if (!Input.GetButtonDown(PICKUP_BUTTON)) return;

				 if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabRange, pickupLayermask)) return;
				 
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

				 Transform myTransform = this.transform;
				 Matrix4x4 matrix = Matrix4x4.TRS(myTransform.position, myTransform.rotation, myTransform.localScale);

				 heldTransform.position = matrix.MultiplyPoint3x4(holdOffset);
				 heldTransform.rotation = (Quaternion.Inverse(playerCamera.TargetTransforms[0].rotation) * heldTransform.rotation);

				 if (!Input.GetButtonDown(PICKUP_BUTTON)) return;
				 
				 heldObjectRigidBody.isKinematic = false;
				 heldObjectCollider.enabled = true;
				 heldObjectRigidBody.AddForce(throwForce*transform.forward, throwForceMode);
				 
				 heldObject = null;
			 }
		 }
		 
		 #endregion
	 }
}
