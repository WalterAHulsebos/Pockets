// GENERATED AUTOMATICALLY FROM 'Assets/[Source]/Settings/Input Control Schemes/DefaultControlScheme.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class DefaultControlScheme : IInputActionCollection
{
    private InputActionAsset asset;
    public DefaultControlScheme()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DefaultControlScheme"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""95156e99-b082-4593-8b7d-40c064a4a7ad"",
            ""actions"": [
                {
                    ""name"": ""move"",
                    ""id"": ""5f2ef24d-5358-44ab-a308-fc5871769512"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""look"",
                    ""id"": ""2e3e23c3-9af0-406b-bcda-4360a5e0b5e8"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""jump"",
                    ""id"": ""28de7a09-5225-4048-a367-29705cb58eca"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""strafe"",
                    ""id"": ""14633c1b-147a-4db0-87fa-0f4828246185"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""sprint"",
                    ""id"": ""f5307553-6c82-408f-b78c-14c71f37113f"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""crouch"",
                    ""id"": ""816b50a4-b9a5-4f8b-8d8e-d3026bad15f1"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""strafeToggle"",
                    ""id"": ""32ee6e11-7f9e-4066-a0df-6478e3887abb"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Dpad"",
                    ""id"": ""09bb7d1c-02bc-4b19-88d0-7ca49b944fff"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2b2e7c23-3e38-47f8-8300-9813acd75ea4"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c9066659-1bf7-4e40-b2dc-214bc822021b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""left"",
                    ""id"": ""73338624-2883-4eb5-a5e7-75b3755ce241"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""right"",
                    ""id"": ""12e6e23a-785d-4963-93f8-d5143bb72071"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""7c82b938-6c00-4140-a3ea-bbc868259665"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""6377a0d9-ee9a-4333-8a46-7cd644f3649f"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""d890db3a-c3e5-44e0-9860-317636af1c93"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""dea6666b-0920-404a-b34f-7f537668e366"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""c27b04bf-8f7d-4d2d-809c-8063c27e4d1d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""278db01f-774f-410c-915f-863fe226e1af"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""strafe"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""ee1afff9-4e38-47e6-a9da-61a7778b8d97"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""04f5d25c-dc2c-46b9-9af1-71e76375278c"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""9c10555c-6aa7-4ac4-bac5-a8996883fa0f"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""d1f881fb-5207-440e-8bad-7aee413f280e"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""9d494c45-109b-473a-91e7-646484245097"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""strafeToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movement
        m_Movement = asset.GetActionMap("Movement");
        m_Movement_move = m_Movement.GetAction("move");
        m_Movement_look = m_Movement.GetAction("look");
        m_Movement_jump = m_Movement.GetAction("jump");
        m_Movement_strafe = m_Movement.GetAction("strafe");
        m_Movement_sprint = m_Movement.GetAction("sprint");
        m_Movement_crouch = m_Movement.GetAction("crouch");
        m_Movement_strafeToggle = m_Movement.GetAction("strafeToggle");
    }

    ~DefaultControlScheme()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes
    {
        get => asset.controlSchemes;
    }

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Movement
    private InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private InputAction m_Movement_move;
    private InputAction m_Movement_look;
    private InputAction m_Movement_jump;
    private InputAction m_Movement_strafe;
    private InputAction m_Movement_sprint;
    private InputAction m_Movement_crouch;
    private InputAction m_Movement_strafeToggle;
    public struct MovementActions
    {
        private DefaultControlScheme m_Wrapper;
        public MovementActions(DefaultControlScheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @move { get { return m_Wrapper.m_Movement_move; } }
        public InputAction @look { get { return m_Wrapper.m_Movement_look; } }
        public InputAction @jump { get { return m_Wrapper.m_Movement_jump; } }
        public InputAction @strafe { get { return m_Wrapper.m_Movement_strafe; } }
        public InputAction @sprint { get { return m_Wrapper.m_Movement_sprint; } }
        public InputAction @crouch { get { return m_Wrapper.m_Movement_crouch; } }
        public InputAction @strafeToggle { get { return m_Wrapper.m_Movement_strafeToggle; } }
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                look.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnLook;
                look.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnLook;
                look.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnLook;
                jump.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                jump.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                jump.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                strafe.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnStrafe;
                strafe.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnStrafe;
                strafe.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnStrafe;
                sprint.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
                sprint.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
                sprint.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnSprint;
                crouch.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnCrouch;
                crouch.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnCrouch;
                crouch.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnCrouch;
                strafeToggle.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnStrafeToggle;
                strafeToggle.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnStrafeToggle;
                strafeToggle.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnStrafeToggle;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                move.started += instance.OnMove;
                move.performed += instance.OnMove;
                move.canceled += instance.OnMove;
                look.started += instance.OnLook;
                look.performed += instance.OnLook;
                look.canceled += instance.OnLook;
                jump.started += instance.OnJump;
                jump.performed += instance.OnJump;
                jump.canceled += instance.OnJump;
                strafe.started += instance.OnStrafe;
                strafe.performed += instance.OnStrafe;
                strafe.canceled += instance.OnStrafe;
                sprint.started += instance.OnSprint;
                sprint.performed += instance.OnSprint;
                sprint.canceled += instance.OnSprint;
                crouch.started += instance.OnCrouch;
                crouch.performed += instance.OnCrouch;
                crouch.canceled += instance.OnCrouch;
                strafeToggle.started += instance.OnStrafeToggle;
                strafeToggle.performed += instance.OnStrafeToggle;
                strafeToggle.canceled += instance.OnStrafeToggle;
            }
        }
    }
    public MovementActions @Movement
    {
        get
        {
            return new MovementActions(this);
        }
    }
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnStrafe(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnStrafeToggle(InputAction.CallbackContext context);
    }
}
