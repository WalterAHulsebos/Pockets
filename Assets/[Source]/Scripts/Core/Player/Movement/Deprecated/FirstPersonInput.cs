using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Movement
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of First Person Input
    /// </summary>
    public class FirstPersonInput : CharacterInput
    {
        //TODO: Re-implement Crouching.
        
        // /// <summary> Fired when the crouch is started </summary>
        //public event Action CrouchStarted;
		
        // /// <summary> Fired when the crouch is ended </summary>
        // public event Action CrouchEnded; 

        /// <summary> Resets the input states </summary>
        // /// <remarks>used by the <see cref="StandardAssets.Characters.FirstPerson.FirstPersonBrain"/> to reset inputs when entering the walking state</remarks>
        public void ResetInputs()
        {
            //isCrouching = false;
            IsSprinting = false;
        }

        /// <summary> Registers crouch input </summary>
        protected override void RegisterAdditionalInputs()
        {
            //StandardControls.Movement.crouch.performed += OnCrouchInput;
            //StandardControls.Movement.crouch.cancelled += OnCrouchInput;
        }

        /// <inheritdoc/>
        protected override void OnSprintInput(InputAction.CallbackContext context)
        {
            base.OnSprintInput(context);
            //m_IsCrouching = false
        }

        protected void OnCrouchInput(InputAction.CallbackContext context)
        {
            //BroadcastInputAction(ref m_IsCrouching, CrouchStarted, CrouchEnded);
            //isSprinting = false;
        }

    }
}