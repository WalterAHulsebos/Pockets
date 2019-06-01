using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KinematicCharacterController;
using Rewired;

using Utilities;
using Utilities.Extensions;

using Sirenix.OdinInspector;

using ReadOnlyAttribute = Sirenix.OdinInspector.ReadOnlyAttribute;

#if Odin_Inspector
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

namespace Core.PlayerSystems.Movement
{
	public class PlayerController : Multiton<PlayerController>, ICharacterController
	{
		#region Variables

		#region Serialized

		//public int playerId = 0; // The Rewired player id of this character
		
		[Required]
		public PlayerCamera playerCamera;
		
		[PropertySpace(spaceBefore: 0, spaceAfter: 5)]
		[Required]
		public KinematicCharacterMotor motor;
		
		[BoxGroup("Index", showLabel: false)]
		[SerializeField] private int playerIndex = 0;
		
		[FoldoutGroup("Ground Movement")]
		[SerializeField] private float maxGroundMoveSpeed = 10f;
		[FoldoutGroup("Ground Movement")]
		[SerializeField] private float groundMovementSharpness = 15f;
		[FoldoutGroup("Ground Movement")]
		[SerializeField] private float orientationSharpness = 10f;
		
		private OrientationMethod orientationMethod = OrientationMethod.TowardsCamera;

		[FoldoutGroup("Air Movement")]
		[SerializeField] private float 
			maxAirMoveSpeed = 100f, 
			airAccelerationSpeed = 15f, 
			drag = 0.1f;

		[FoldoutGroup("Jumping")]
		[SerializeField] private bool allowJumpingWhenSliding = false;
		[FoldoutGroup("Jumping")]
		[SerializeField] private float 
			jumpUpSpeed = 10f, 
			jumpScalableForwardSpeed = 10f,
			jumpPreGroundingGraceTime = 0f, 
			jumpPostGroundingGraceTime = 0f;

		[FoldoutGroup("Misc")]
		[SerializeField] private List<Collider> ignoredColliders = new List<Collider>();
		[FoldoutGroup("Misc")]
		[SerializeField] private bool orientTowardsGravity = false;
		[FoldoutGroup("Misc")]
		[SerializeField] private Vector3 gravity = new Vector3(0, -30f, 0);
		[FoldoutGroup("Misc")]
		[SerializeField] private Transform meshRoot;
		
		#endregion

		#region Non-Serialized

		public CharacterState CurrentCharacterState { get; private set; }

		private readonly Collider[] probedColliders = new Collider[8];
		private Vector3 moveInputVector;
		private Vector3 lookInputVector;
		private bool jumpRequested = false;
		private bool jumpConsumed = false;
		private bool jumpedThisFrame = false;
		private float timeSinceJumpRequested = Mathf.Infinity;
		private float timeSinceLastAbleToJump = 0f;
		private Vector3 internalVelocityAdd = Vector3.zero;
		private bool shouldBeCrouching = false;
		private bool isCrouching = false;

		private Vector3 lastInnerNormal = Vector3.zero;
		private Vector3 lastOuterNormal = Vector3.zero;
		
		//[SerializeField] [ReadOnly]
		public Player Player { get; private set; } // The Rewired Player
		
		[NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
		public bool initialized;

		[SerializeField] [ReadOnly] 
		private PlayerCharacterInputs inputs;
		
		#endregion
			
		#region Local Classes
		
		public enum CharacterState
		{
			Default,
		}

		public enum OrientationMethod
		{
			TowardsCamera,
			TowardsMovement,
		}

		[Serializable]
		public struct PlayerCharacterInputs
		{
			public float moveAxisForward;
			public float moveAxisRight;
			public Quaternion cameraRotation;
			public bool jumpDown;
		}
		
		#endregion
		
		#region Consts
		
		private const string LOOK_HORIZONTAL = "Look Horizontal";
		private const string LOOK_VERTICAL = "Look Vertical";
		private const string MOVE_HORIZONTAL = "Move Horizontal";
		private const string MOVE_VERTICAL = "Move Vertical";
		private const string JUMP_KEY = "Jump";
		
		#endregion

		#endregion

		#region Methods

		private void Reset()
		{
			playerIndex = Instances.GetIndex(func: (instance => instance == this));
		}

		private void Awake()
		{
			// Handle initial state
			TransitionToState(CharacterState.Default);

			motor = motor ?? GetComponent<KinematicCharacterMotor>() ?? GetComponentInChildren<KinematicCharacterMotor>();
			
			// Assign the characterController to the motor
			motor.CharacterController = this;
		}

		private void Update()
	  	{
		   if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
		   
		   if(!initialized)
		   {
			   Initialize();
			}
		   
		   if (Input.GetMouseButtonDown(0))
			{
				 Cursor.lockState = CursorLockMode.Locked;
			}
 
			HandleCameraInput();
			HandleCharacterInput();
	   }
		
		/// <summary>
		/// Links a player to their playerID
		/// </summary>
		private void Initialize()
		{	
			// Get the Rewired Player object for this player.
			Player = ReInput.players.GetPlayer(playerIndex);
			
			initialized = true;
		}

		#region Input Handling
		
		private void HandleCameraInput()
		{
			float mouseLookAxisUp = Player.GetAxisRaw(LOOK_VERTICAL);
			float mouseLookAxisRight = Player.GetAxisRaw(LOOK_HORIZONTAL);
			Vector3 lookInputVector = new Vector3(mouseLookAxisRight, -mouseLookAxisUp, 0f);
		
			// Prevent moving the camera while the cursor isn't locked
			if (Cursor.lockState != CursorLockMode.Locked)
			{
				 lookInputVector = Vector3.zero;
			}
		
			//float scrollInput = -player.GetAxis(MOUSE_SCROLL_INPUT);
		
			playerCamera.UpdateWithInput(lookInputVector, meshRoot);
		}

		private void HandleCharacterInput()
		{
			//TODO: Don't make a new one every frame, cache it.
			 PlayerCharacterInputs characterInputs = new PlayerCharacterInputs
			 {
				 moveAxisForward = Player.GetAxisRaw(MOVE_VERTICAL),
				 moveAxisRight = Player.GetAxisRaw(MOVE_HORIZONTAL),
				 cameraRotation = playerCamera.TargetTransforms[0].rotation,
				 jumpDown = Player.GetButtonDown(JUMP_KEY),
			 };

			 inputs = characterInputs;
			 
			 SetInputs(ref characterInputs);
		 }
		 
		 #endregion

		#region Character Control

		 /// <summary>
        /// Handles movement state transitions and enter/exit callbacks
        /// </summary>
		  public void TransitionToState(CharacterState newState)
		  {
				CharacterState tmpInitialState = CurrentCharacterState;
				OnStateExit(tmpInitialState, newState);
				CurrentCharacterState = newState;
				OnStateEnter(newState, tmpInitialState);
		  }
		  
		  /// <summary>
		  /// Event when entering a state
		  /// </summary>
		  public void OnStateEnter(CharacterState state, CharacterState fromState)
		  {
				switch (state)
				{
					 case CharacterState.Default:
						  {
								break;
						  }
				}
		  }
		  
		  /// <summary>
		  /// Event when exiting a state
		  /// </summary>
		  public void OnStateExit(CharacterState state, CharacterState toState)
		  {
				switch (state)
				{
					 case CharacterState.Default:
						  {
								break;
						  }
				}
		  }
		  
		  /// <summary>
		  /// This is called every frame by ExamplePlayer in order to tell the character what its inputs are
		  /// </summary>
		  public void SetInputs(ref PlayerCharacterInputs inputs)
		  {
				// Clamp input
				Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.moveAxisRight, 0f, inputs.moveAxisForward), 1f);
		  
				// Calculate camera direction and rotation on the character plane
				Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.cameraRotation * Vector3.forward, motor.CharacterUp).normalized;
				
				if(cameraPlanarDirection.sqrMagnitude.Approximately(0f))
				{
					 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.cameraRotation * Vector3.up, motor.CharacterUp).normalized;
				}
				Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, motor.CharacterUp);
		  
				switch (CurrentCharacterState)
				{
					 case CharacterState.Default:
						  {
								// Move and look inputs
								this.moveInputVector = cameraPlanarRotation * moveInputVector;
		  
								switch (orientationMethod)
								{
									 case OrientationMethod.TowardsCamera:
										  lookInputVector = cameraPlanarDirection;
										  break;
									 case OrientationMethod.TowardsMovement:
										  lookInputVector = this.moveInputVector.normalized;
										  break;
									 default:
										  throw new ArgumentOutOfRangeException();
								}
		  
								// Jumping input
								if (inputs.jumpDown)
								{
									 timeSinceJumpRequested = 0f;
									 jumpRequested = true;
								}
		  
								break;
						  }
					 default:
						  throw new ArgumentOutOfRangeException();
				}
		  }
		  
		  /// <summary>
		  /// (Called by KinematicCharacterMotor during its update cycle)
		  /// This is called before the character begins its movement update
		  /// </summary>
		  public void BeforeCharacterUpdate(float deltaTime)
		  {
		  }
		  
		  /// <summary>
		  /// (Called by KinematicCharacterMotor during its update cycle)
		  /// This is where you tell your character what its rotation should be right now. 
		  /// This is the ONLY place where you should set the character's rotation
		  /// </summary>
		  public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
		  {
				switch (CurrentCharacterState)
				{
					 case CharacterState.Default:
						  {
								if (lookInputVector.sqrMagnitude > 0f && orientationSharpness > 0f)
								{
									 // Smoothly interpolate from current to target look direction
									 Vector3 smoothedLookInputDirection = Vector3.Slerp(motor.CharacterForward, lookInputVector, 1 - Mathf.Exp(-orientationSharpness * deltaTime)).normalized;
		  
									 // Set the current rotation (which will be used by the KinematicCharacterMotor)
									 //currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, motor.CharacterUp);
								}
								if (orientTowardsGravity)
								{
									 // Rotate from current up to invert gravity
									 currentRotation = Quaternion.FromToRotation((currentRotation * Vector3.up), -gravity) * currentRotation;
								}
								break;
						  }
				}
		  }
		  
		  /// <summary>
		  /// (Called by KinematicCharacterMotor during its update cycle)
		  /// This is where you tell your character what its velocity should be right now. 
		  /// This is the ONLY place where you can set the character's velocity
		  /// </summary>
		  public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
		  {
				switch (CurrentCharacterState)
				{
					 case CharacterState.Default:
						  {
								// Ground movement
								if (motor.GroundingStatus.IsStableOnGround)
								{
									 float currentVelocityMagnitude = currentVelocity.magnitude;
		  
									 Vector3 effectiveGroundNormal = motor.GroundingStatus.GroundNormal;
									 if (currentVelocityMagnitude > 0f && motor.GroundingStatus.SnappingPrevented)
									 {
										  // Take the normal from where we're coming from
										  Vector3 groundPointToCharacter = motor.TransientPosition - motor.GroundingStatus.GroundPoint;
										  if (Vector3.Dot(currentVelocity, groundPointToCharacter) >= 0f)
										  {
												effectiveGroundNormal = motor.GroundingStatus.OuterGroundNormal;
										  }
										  else
										  {
												effectiveGroundNormal = motor.GroundingStatus.InnerGroundNormal;
										  }
									 }
		  
									 // Reorient velocity on slope
									 currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;
		  
									 // Calculate target velocity
									 Vector3 inputRight = Vector3.Cross(moveInputVector, motor.CharacterUp);
									 Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * moveInputVector.magnitude;
									 Vector3 targetMovementVelocity = reorientedInput * maxGroundMoveSpeed;
		  
									 // Smooth movement Velocity
									 currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-groundMovementSharpness * deltaTime));
								}
								// Air movement
								else
								{
									 // Add move input
									 if (moveInputVector.sqrMagnitude > 0f)
									 {
										  Vector3 addedVelocity = moveInputVector * airAccelerationSpeed * deltaTime;
		  
										  // Prevent air movement from making you move up steep sloped walls
										  if (motor.GroundingStatus.FoundAnyGround)
										  {
												Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal), motor.CharacterUp).normalized;
												addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
										  }
		  
										  // Limit air movement from inputs to a certain maximum, without limiting the total air move speed from momentum, gravity or other forces
										  Vector3 resultingVelOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity + addedVelocity, motor.CharacterUp);
										  if(resultingVelOnInputsPlane.magnitude > maxAirMoveSpeed && Vector3.Dot(moveInputVector, resultingVelOnInputsPlane) >= 0f)
										  {
												addedVelocity = Vector3.zero;
										  }
										  else
										  {
												Vector3 velOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);
												Vector3 clampedResultingVelOnInputsPlane = Vector3.ClampMagnitude(resultingVelOnInputsPlane, maxAirMoveSpeed);
												addedVelocity = clampedResultingVelOnInputsPlane - velOnInputsPlane;
										  }
		  
										  currentVelocity += addedVelocity;
									 }
		  
									 // Gravity
									 currentVelocity += gravity * deltaTime;
		  
									 // Drag
									 currentVelocity *= (1f / (1f + (drag * deltaTime)));
								}
		  
								// Handle jumping
								jumpedThisFrame = false;
								timeSinceJumpRequested += deltaTime;
								if (jumpRequested)
								{
									 // See if we actually are allowed to jump
									 if (!jumpConsumed && ((allowJumpingWhenSliding ? motor.GroundingStatus.FoundAnyGround : motor.GroundingStatus.IsStableOnGround) || timeSinceLastAbleToJump <= jumpPostGroundingGraceTime))
									 {
										  // Calculate jump direction before ungrounding
										  Vector3 jumpDirection = motor.CharacterUp;
										  if (motor.GroundingStatus.FoundAnyGround && !motor.GroundingStatus.IsStableOnGround)
										  {
												jumpDirection = motor.GroundingStatus.GroundNormal;
										  }
		  
										  // Makes the character skip ground probing/snapping on its next update. 
										  // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
										  motor.ForceUnground();
		  
										  // Add to the return velocity and reset jump state
										  currentVelocity += (jumpDirection * jumpUpSpeed) - Vector3.Project(currentVelocity, motor.CharacterUp);
										  currentVelocity += (moveInputVector * jumpScalableForwardSpeed);
										  jumpRequested = false;
										  jumpConsumed = true;
										  jumpedThisFrame = true;
									 }
								}
		  
								// Take into account additive velocity
								if (internalVelocityAdd.sqrMagnitude > 0f)
								{
									 currentVelocity += internalVelocityAdd;
									 internalVelocityAdd = Vector3.zero;
								}
								break;
						  }
					 default:
						  throw new ArgumentOutOfRangeException();
				}
		  }
		  
		  /// <summary>
		  /// (Called by KinematicCharacterMotor during its update cycle)
		  /// This is called after the character has finished its movement update
		  /// </summary>
		  public void AfterCharacterUpdate(float deltaTime)
		  {
				switch (CurrentCharacterState)
				{
					 case CharacterState.Default:
						  {
								// Handle jump-related values
								{
									 // Handle jumping pre-ground grace period
									 if (jumpRequested && timeSinceJumpRequested > jumpPreGroundingGraceTime)
									 {
										  jumpRequested = false;
									 }
		  
									 if (allowJumpingWhenSliding ? motor.GroundingStatus.FoundAnyGround : motor.GroundingStatus.IsStableOnGround)
									 {
										  // If we're on a ground surface, reset jumping values
										  if (!jumpedThisFrame)
										  {
												jumpConsumed = false;
										  }
										  timeSinceLastAbleToJump = 0f;
									 }
									 else
									 {
										  // Keep track of time since we were last able to jump (for grace period)
										  timeSinceLastAbleToJump += deltaTime;
									 }
								}
		  
								// Handle uncrouching
								if (isCrouching && !shouldBeCrouching)
								{
									 // Do an overlap test with the character's standing height to see if there are any obstructions
									 motor.SetCapsuleDimensions(0.5f, 2f, 1f);
									 if (motor.CharacterOverlap(
										  motor.TransientPosition,
										  motor.TransientRotation,
										  probedColliders,
										  motor.CollidableLayers,
										  QueryTriggerInteraction.Ignore) > 0)
									 {
										  // If obstructions, just stick to crouching dimensions
										  motor.SetCapsuleDimensions(0.5f, 1f, 0.5f);
									 }
									 else
									 {
										  // If no obstructions, uncrouch
										  meshRoot.localScale = new Vector3(1f, 1f, 1f);
										  isCrouching = false;
									 }
								}
								break;
						  }
					 default:
						  throw new ArgumentOutOfRangeException();
				}
		  }
		  
		  public void PostGroundingUpdate(float deltaTime)
		  {
				// Handle landing and leaving ground
				if (motor.GroundingStatus.IsStableOnGround && !motor.LastGroundingStatus.IsStableOnGround)
				{
					 OnLanded();
				}
				else if (!motor.GroundingStatus.IsStableOnGround && motor.LastGroundingStatus.IsStableOnGround)
				{
					 OnLeaveStableGround();
				}
		  }
		  
		  public bool IsColliderValidForCollisions(Collider coll)
		  {
				if (ignoredColliders.Count == 0)
				{
					 return true;
				}
		  
				return !ignoredColliders.Contains(coll);
		  }
		  
		  public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
		  {
		  }
		  
		  public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
		  {
		  }
		  
		  public void AddVelocity(Vector3 velocity)
		  {
				switch (CurrentCharacterState)
				{
					 case CharacterState.Default:
						  {
								internalVelocityAdd += velocity;
								break;
						  }
					 default:
						  throw new ArgumentOutOfRangeException();
				}
		  }
		  
		  public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
		  {
		  }
		  
		  protected void OnLanded()
		  {
				//TODO: Play Particle
		  }
		  
		  protected void OnLeaveStableGround()
		  {
				//TODO: Play Particle
		  }
		  
		  public void OnDiscreteCollisionDetected(Collider hitCollider)
		  {
				
		  }
		 
		#endregion
		
		#endregion
		
	}
}