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
            ""name"": ""New action map"",
            ""id"": ""c7a725c2-e274-4dd3-8077-47936151f8b1"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""id"": ""7acb0f17-5a4a-450e-9eaa-a78fa69f6c87"",
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
                    ""name"": """",
                    ""id"": ""d2bff9e1-d254-433c-ba00-4176173ba4e3"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // New action map
        m_Newactionmap = asset.GetActionMap("New action map");
        m_Newactionmap_Newaction = m_Newactionmap.GetAction("New action");
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

    // New action map
    private InputActionMap m_Newactionmap;
    private INewactionmapActions m_NewactionmapActionsCallbackInterface;
    private InputAction m_Newactionmap_Newaction;
    public struct NewactionmapActions
    {
        private DefaultControlScheme m_Wrapper;
        public NewactionmapActions(DefaultControlScheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction { get { return m_Wrapper.m_Newactionmap_Newaction; } }
        public InputActionMap Get() { return m_Wrapper.m_Newactionmap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(NewactionmapActions set) { return set.Get(); }
        public void SetCallbacks(INewactionmapActions instance)
        {
            if (m_Wrapper.m_NewactionmapActionsCallbackInterface != null)
            {
                Newaction.started -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnNewaction;
                Newaction.performed -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnNewaction;
                Newaction.canceled -= m_Wrapper.m_NewactionmapActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_NewactionmapActionsCallbackInterface = instance;
            if (instance != null)
            {
                Newaction.started += instance.OnNewaction;
                Newaction.performed += instance.OnNewaction;
                Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public NewactionmapActions @Newactionmap
    {
        get
        {
            return new NewactionmapActions(this);
        }
    }
    public interface INewactionmapActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
