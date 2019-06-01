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
		 [SerializeField] private Transform holdPosition;
		 [SerializeField] private float throwForce = 25f;
		 [SerializeField] private ForceMode throwForceMode;
		 [SerializeField] private LayerMask pickupLayermask = -1;

		 private PlayerController playerController = null;
		 
		 private Transform heldObject = null;
		 
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
				 heldObject.GetComponent<Rigidbody>().isKinematic = true;
				 heldObject.GetComponent<Collider>().enabled = false;
			 }
			 else
			 {
				 Transform heldTransform = heldObject.transform;
				 heldTransform.position = holdPosition.position;
				 heldTransform.rotation = holdPosition.rotation;

				 if (!Input.GetButtonDown(PICKUP_BUTTON)) return;
				 Rigidbody body = heldObject.GetComponent<Rigidbody>();
				 body.isKinematic = false;
				 heldObject.GetComponent<Collider>().enabled = true;
				 body.AddForce(throwForce*transform.forward,throwForceMode);
				 heldObject = null;
			 }
		 }
		 
		 #endregion
	 }
}
