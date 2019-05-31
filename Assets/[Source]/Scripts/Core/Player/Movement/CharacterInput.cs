using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour;
#endif

namespace Core.Movement
{
	/// <summary> Abstract base class for Player Controlled CharacterInput </summary>
	public abstract class CharacterInput : MonoBehaviour
	{
		#region Variables
		
		#region Events
		
		/// <summary> Fired when the jump input is pressed - i.e. on key down </summary>
		public event Action JumpPressed;

		/// <summary> Fired when the sprint input is started </summary>
		public event Action sprintStarted;

		/// <summary> Fired when the sprint input is disengaged </summary>
		public event Action sprintEnded;
		
		#endregion

		[Tooltip("Input Action Maps")]
		// ReSharper disable once InconsistentNaming
		[SerializeField] private DefaultControlScheme standardControls = null;

		[Tooltip("Invert look direction?")]
		[HorizontalGroup]
		[SerializeField] private bool invertX, invertY;

		[Tooltip("Look sensitivity"), MinValue(0), MaxValue(1)]
		[SerializeField] private Vector2 lookSensitivity = Vector2.one *.5f;

		[Tooltip("Toggle the Cursor Lock Mode? Press ESCAPE during play mode to unlock")]
		[SerializeField] private bool cursorLocked = true;

		// Instance of UI for Touch Controls
		private GameObject touchControlsCanvasInstance;

		#region Accessors
		
		///<summary> Gets if the movement input is being applied </summary>
		public bool HasMovementInput => (MoveInput != Vector2.zero);

		///<summary> Gets/sets the look input vector </summary>
		private Vector2 LookInput { get; set; }
		
		///<summary> Rotation of a moving platform applied to the look input vector (so that the platform rotates the camera) </summary>
		public Vector2 MovingPlatformLookInput { get; set; }
		
		///<summary> Raw look vector. </summary>
		private Vector2 rawLookInput;

		///<summary> Gets/sets the move input vector. </summary>
		public Vector2 MoveInput { get; protected set; } = default;

		///<summary> Gets whether or not the jump input is currently applied. </summary>
		public bool HasJumpInput { get; protected set; } = false;

		///<summary> Gets a reference to the currently set Standard Controls asset. </summary>
		//[ShowInInspector, OdinSerialize]
		public DefaultControlScheme StandardControls 
		{ 
			get => standardControls;
			protected set => standardControls = value; 
		}

		///<summary> Gets/sets the internal flag that tracks the Sprinting state </summary>
		public bool IsSprinting { get; protected set; } = false;
		
		#endregion
		
		#endregion

		#region Methods

		///<summary> Sets up the Cinemachine delegate and subscribes to new input's performed events </summary>
		private void Awake()
		{
			if (StandardControls == null) { return; }
			
			/*
			StandardControls.Movement.move.performed += OnMoveInput;
			StandardControls.Movement.look.performed += OnLookInput;
			StandardControls.Movement.jump.started += OnJumpInputStarted;
			StandardControls.Movement.sprint.performed += OnSprintInput;
				
			StandardControls.Movement.move.cancelled += OnMoveInputCancelled;
			StandardControls.Movement.look.cancelled += OnLookInputCancelled;
			StandardControls.Movement.sprint.cancelled += OnSprintInput;
			StandardControls.Movement.jump.cancelled += OnJumpInputEnded;
			*/

			RegisterAdditionalInputs();
		}

		/// <summary> Enables associated controls </summary>
		protected void OnEnable()
		{
			StandardControls?.Enable();
			HandleCursorLock();
			CinemachineCore.GetInputAxis += LookInputOverride;
		}

		/// <summary> Disables associated controls </summary>
		protected void OnDisable()
		{
			StandardControls?.Disable();
			CinemachineCore.GetInputAxis -= LookInputOverride;
		}

		/// <summary> Checks for lock state input </summary>
		protected void Update()
		{
			if (!Input.GetKeyDown(KeyCode.Escape)) return;
			
			cursorLocked = !cursorLocked;
			HandleCursorLock();

		}	

		/// <summary> Handles registration of additional inputs that are not common between the First and Third person characters </summary>
		protected abstract void RegisterAdditionalInputs();

		/// <summary>
		/// Handles the sprint input
		/// </summary>
		/// <param name="context">context is required by the performed event</param>
		protected virtual void OnSprintInput(InputAction.CallbackContext context)
		{
			bool isSprinting = IsSprinting;
			BroadcastInputAction(ref isSprinting, sprintStarted, sprintEnded);
		}

		/// <summary>
		/// Helper function for broadcasting the start and end events of a specific action. e.g. start sprint and end sprint
		/// </summary>
		/// <param name="isDoingAction">The boolean to toggle</param>
		/// <param name="started">The start event</param>
		/// <param name="ended">The end event</param>
		protected static void BroadcastInputAction(ref bool isDoingAction, Action started, Action ended)
		{
			isDoingAction = !isDoingAction;

			if (isDoingAction)
			{
				started?.Invoke();
			}
			else
			{
				ended?.Invoke();
			}
		}

		// Provides the input vector for the look control
		private void OnLookInput(InputAction.CallbackContext context)
		{
			LookInput = context.ReadValue<Vector2>();
		}

		// Provides the input vector for the move control
		private void OnMoveInput(InputAction.CallbackContext context)
		{
			MoveInput = context.ReadValue<Vector2>();
		}
		
		// Resets the move input vector to zero once input has stopped
		private void OnMoveInputCancelled(InputAction.CallbackContext context)
		{
			MoveInput = Vector2.zero;
		}
			
		// Resets the look input vector to zero once input has stopped
		private void OnLookInputCancelled(InputAction.CallbackContext context)
		{
			LookInput = Vector2.zero;
		}

		// Handles the ending of jump event from the new input system
		private void OnJumpInputEnded(InputAction.CallbackContext context)
		{
			HasJumpInput = false;
		}
		
		// Handles the start of the jump event from the new input system
		private void OnJumpInputStarted(InputAction.CallbackContext context)
		{
			HasJumpInput = true;
			JumpPressed?.Invoke();
		}

		// Handles the Cinemachine delegate
		private float LookInputOverride(string axis)
		{
			switch (axis)
			{
				case "Horizontal":
					return invertX
						? LookInput.x * lookSensitivity.x + MovingPlatformLookInput.x
						: -LookInput.x * lookSensitivity.x + MovingPlatformLookInput.x;
				case "Vertical":
					return invertY 
						? LookInput.y * lookSensitivity.y 
						: -LookInput.y * lookSensitivity.y;
				default:
					return 0;
			}
		}

		// Handles the cursor lock state
		private void HandleCursorLock()
		{
			Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
		}
		
		#endregion
	}
}