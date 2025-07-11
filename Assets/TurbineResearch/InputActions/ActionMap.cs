//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.1
//     from Assets/TurbineResearch/InputActions/ActionMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @ActionMap: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ActionMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ActionMap"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""bb2a4332-c02e-4607-981d-dc26014170cc"",
            ""actions"": [
                {
                    ""name"": ""OpenUI"",
                    ""type"": ""Button"",
                    ""id"": ""21587955-c9bd-4d21-9aa7-bd4fed42c5b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3136a92a-531c-4564-adef-d34d54c4acd3"",
                    ""path"": ""<XRController>/menuButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e17d5b8-3875-4d9d-96a4-4cf7b0e028c0"",
                    ""path"": ""<XRController>/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""8582b4ce-5442-44ef-8178-9c532a080a0c"",
            ""actions"": [
                {
                    ""name"": ""CloseUI"",
                    ""type"": ""Button"",
                    ""id"": ""696044f3-84fa-49e4-bef2-baf3fbb8ee7c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7d1c4cb7-1a6a-41f0-a9f0-84c0133dbbc8"",
                    ""path"": ""<XRController>/menuButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CloseUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4bcf0cf4-9717-4628-82e0-b092b74fecdb"",
                    ""path"": ""<XRController>/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CloseUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_OpenUI = m_Player.FindAction("OpenUI", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_CloseUI = m_UI.FindAction("CloseUI", throwIfNotFound: true);
    }

    public void Dispose()
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

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_OpenUI;
    public struct PlayerActions
    {
        private @ActionMap m_Wrapper;
        public PlayerActions(@ActionMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @OpenUI => m_Wrapper.m_Player_OpenUI;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @OpenUI.started += instance.OnOpenUI;
            @OpenUI.performed += instance.OnOpenUI;
            @OpenUI.canceled += instance.OnOpenUI;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @OpenUI.started -= instance.OnOpenUI;
            @OpenUI.performed -= instance.OnOpenUI;
            @OpenUI.canceled -= instance.OnOpenUI;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
    private readonly InputAction m_UI_CloseUI;
    public struct UIActions
    {
        private @ActionMap m_Wrapper;
        public UIActions(@ActionMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @CloseUI => m_Wrapper.m_UI_CloseUI;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void AddCallbacks(IUIActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
            @CloseUI.started += instance.OnCloseUI;
            @CloseUI.performed += instance.OnCloseUI;
            @CloseUI.canceled += instance.OnCloseUI;
        }

        private void UnregisterCallbacks(IUIActions instance)
        {
            @CloseUI.started -= instance.OnCloseUI;
            @CloseUI.performed -= instance.OnCloseUI;
            @CloseUI.canceled -= instance.OnCloseUI;
        }

        public void RemoveCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IPlayerActions
    {
        void OnOpenUI(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnCloseUI(InputAction.CallbackContext context);
    }
}
