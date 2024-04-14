//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Runtime/Input/WizardInput.inputactions
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

public partial class @WizardInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @WizardInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""WizardInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""2a0a5458-606e-4802-9223-3d3e44ba883d"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""36498b40-9590-44c2-b89a-2a30d3d35568"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""6035f686-dc9a-4c18-b871-76a2382b9004"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""53190115-4e58-4cac-92a4-fa166830b5b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3d11ff58-ab18-443b-aa7f-c743a90cfeab"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""ef244003-8de0-4841-b0f4-6235430b1e95"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c18fb1c6-3377-4d2a-b62b-aebe34c80104"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7d36f272-aeab-4701-994a-81973bf49f82"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1a1ebed8-05a2-4602-897f-b5a0de381be3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""629f14dd-e210-4873-b6e5-00df6d97b5d2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6061e6d7-3f11-4ff6-a957-792ed43f5f26"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6698e7e4-a1e9-4cd9-b135-0d896cbd4d5a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15becb2d-33d9-4926-89cd-b918062bbb84"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Interactions"",
            ""id"": ""ad67587d-7953-4a5d-bf42-293ef7e5c6cc"",
            ""actions"": [
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""b8a2d131-3d97-42b7-9b41-3a1b6b4d9429"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e7278d72-2cd1-4957-980c-8ee322332a6c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Glyphs"",
            ""id"": ""03b7a4ba-28fb-48fb-9ad4-4b81f211f0c4"",
            ""actions"": [
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""02c542d6-c668-4784-8e3a-9f62a95c51f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""18418391-98ce-460d-bde4-bab0cf2a872d"",
                    ""path"": ""*/{PrimaryAction}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Inventory"",
            ""id"": ""d7e97cad-ddf0-45a4-9e71-4a0d240d3978"",
            ""actions"": [
                {
                    ""name"": ""Scroll Inventory"",
                    ""type"": ""Value"",
                    ""id"": ""f44c0168-0b4b-4bcc-91ec-68676f7098ea"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Inventory Slot 1"",
                    ""type"": ""Button"",
                    ""id"": ""7e5b31f4-1216-4121-bcf0-ce316735b742"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory Slot 2"",
                    ""type"": ""Button"",
                    ""id"": ""281d5995-21f6-4516-ac1d-b91db90dff06"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory Slot 3"",
                    ""type"": ""Button"",
                    ""id"": ""6304048c-dbb5-46bc-a7a5-1f6fbe512206"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory Slot 4"",
                    ""type"": ""Button"",
                    ""id"": ""7b914af3-5f7d-47c2-9db2-8636d370189b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1693f91f-4e7c-4e2c-807d-f573c1d28065"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1dc3ccf2-c5b8-4903-b0fe-6973e003fb65"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory Slot 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""638710ba-0a04-445a-99d5-f3326e6a98b0"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory Slot 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9bc172f-2206-4ea2-80d1-d7236f744916"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory Slot 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e0b1b58-49c2-4bde-b747-f8fa3b821f5e"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory Slot 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Pause"",
            ""id"": ""b662021f-a4d3-46a9-b791-c65e75dce7ef"",
            ""actions"": [
                {
                    ""name"": ""Toggle Pause"",
                    ""type"": ""Button"",
                    ""id"": ""fe99eb8a-e5bc-4c52-a7f4-68bdae264cb9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""680c8e23-ca6e-4e07-903e-514c1321bec8"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6910dfd0-a10d-478c-a1f4-f28cd19552dd"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Pause"",
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
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        // Interactions
        m_Interactions = asset.FindActionMap("Interactions", throwIfNotFound: true);
        m_Interactions_Interact = m_Interactions.FindAction("Interact", throwIfNotFound: true);
        // Glyphs
        m_Glyphs = asset.FindActionMap("Glyphs", throwIfNotFound: true);
        m_Glyphs_Interact = m_Glyphs.FindAction("Interact", throwIfNotFound: true);
        // Inventory
        m_Inventory = asset.FindActionMap("Inventory", throwIfNotFound: true);
        m_Inventory_ScrollInventory = m_Inventory.FindAction("Scroll Inventory", throwIfNotFound: true);
        m_Inventory_InventorySlot1 = m_Inventory.FindAction("Inventory Slot 1", throwIfNotFound: true);
        m_Inventory_InventorySlot2 = m_Inventory.FindAction("Inventory Slot 2", throwIfNotFound: true);
        m_Inventory_InventorySlot3 = m_Inventory.FindAction("Inventory Slot 3", throwIfNotFound: true);
        m_Inventory_InventorySlot4 = m_Inventory.FindAction("Inventory Slot 4", throwIfNotFound: true);
        // Pause
        m_Pause = asset.FindActionMap("Pause", throwIfNotFound: true);
        m_Pause_TogglePause = m_Pause.FindAction("Toggle Pause", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_Jump;
    public struct PlayerActions
    {
        private @WizardInput m_Wrapper;
        public PlayerActions(@WizardInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
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

    // Interactions
    private readonly InputActionMap m_Interactions;
    private List<IInteractionsActions> m_InteractionsActionsCallbackInterfaces = new List<IInteractionsActions>();
    private readonly InputAction m_Interactions_Interact;
    public struct InteractionsActions
    {
        private @WizardInput m_Wrapper;
        public InteractionsActions(@WizardInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Interact => m_Wrapper.m_Interactions_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Interactions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InteractionsActions set) { return set.Get(); }
        public void AddCallbacks(IInteractionsActions instance)
        {
            if (instance == null || m_Wrapper.m_InteractionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InteractionsActionsCallbackInterfaces.Add(instance);
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
        }

        private void UnregisterCallbacks(IInteractionsActions instance)
        {
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
        }

        public void RemoveCallbacks(IInteractionsActions instance)
        {
            if (m_Wrapper.m_InteractionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInteractionsActions instance)
        {
            foreach (var item in m_Wrapper.m_InteractionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InteractionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InteractionsActions @Interactions => new InteractionsActions(this);

    // Glyphs
    private readonly InputActionMap m_Glyphs;
    private List<IGlyphsActions> m_GlyphsActionsCallbackInterfaces = new List<IGlyphsActions>();
    private readonly InputAction m_Glyphs_Interact;
    public struct GlyphsActions
    {
        private @WizardInput m_Wrapper;
        public GlyphsActions(@WizardInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Interact => m_Wrapper.m_Glyphs_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Glyphs; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GlyphsActions set) { return set.Get(); }
        public void AddCallbacks(IGlyphsActions instance)
        {
            if (instance == null || m_Wrapper.m_GlyphsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GlyphsActionsCallbackInterfaces.Add(instance);
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
        }

        private void UnregisterCallbacks(IGlyphsActions instance)
        {
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
        }

        public void RemoveCallbacks(IGlyphsActions instance)
        {
            if (m_Wrapper.m_GlyphsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGlyphsActions instance)
        {
            foreach (var item in m_Wrapper.m_GlyphsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GlyphsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GlyphsActions @Glyphs => new GlyphsActions(this);

    // Inventory
    private readonly InputActionMap m_Inventory;
    private List<IInventoryActions> m_InventoryActionsCallbackInterfaces = new List<IInventoryActions>();
    private readonly InputAction m_Inventory_ScrollInventory;
    private readonly InputAction m_Inventory_InventorySlot1;
    private readonly InputAction m_Inventory_InventorySlot2;
    private readonly InputAction m_Inventory_InventorySlot3;
    private readonly InputAction m_Inventory_InventorySlot4;
    public struct InventoryActions
    {
        private @WizardInput m_Wrapper;
        public InventoryActions(@WizardInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @ScrollInventory => m_Wrapper.m_Inventory_ScrollInventory;
        public InputAction @InventorySlot1 => m_Wrapper.m_Inventory_InventorySlot1;
        public InputAction @InventorySlot2 => m_Wrapper.m_Inventory_InventorySlot2;
        public InputAction @InventorySlot3 => m_Wrapper.m_Inventory_InventorySlot3;
        public InputAction @InventorySlot4 => m_Wrapper.m_Inventory_InventorySlot4;
        public InputActionMap Get() { return m_Wrapper.m_Inventory; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InventoryActions set) { return set.Get(); }
        public void AddCallbacks(IInventoryActions instance)
        {
            if (instance == null || m_Wrapper.m_InventoryActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InventoryActionsCallbackInterfaces.Add(instance);
            @ScrollInventory.started += instance.OnScrollInventory;
            @ScrollInventory.performed += instance.OnScrollInventory;
            @ScrollInventory.canceled += instance.OnScrollInventory;
            @InventorySlot1.started += instance.OnInventorySlot1;
            @InventorySlot1.performed += instance.OnInventorySlot1;
            @InventorySlot1.canceled += instance.OnInventorySlot1;
            @InventorySlot2.started += instance.OnInventorySlot2;
            @InventorySlot2.performed += instance.OnInventorySlot2;
            @InventorySlot2.canceled += instance.OnInventorySlot2;
            @InventorySlot3.started += instance.OnInventorySlot3;
            @InventorySlot3.performed += instance.OnInventorySlot3;
            @InventorySlot3.canceled += instance.OnInventorySlot3;
            @InventorySlot4.started += instance.OnInventorySlot4;
            @InventorySlot4.performed += instance.OnInventorySlot4;
            @InventorySlot4.canceled += instance.OnInventorySlot4;
        }

        private void UnregisterCallbacks(IInventoryActions instance)
        {
            @ScrollInventory.started -= instance.OnScrollInventory;
            @ScrollInventory.performed -= instance.OnScrollInventory;
            @ScrollInventory.canceled -= instance.OnScrollInventory;
            @InventorySlot1.started -= instance.OnInventorySlot1;
            @InventorySlot1.performed -= instance.OnInventorySlot1;
            @InventorySlot1.canceled -= instance.OnInventorySlot1;
            @InventorySlot2.started -= instance.OnInventorySlot2;
            @InventorySlot2.performed -= instance.OnInventorySlot2;
            @InventorySlot2.canceled -= instance.OnInventorySlot2;
            @InventorySlot3.started -= instance.OnInventorySlot3;
            @InventorySlot3.performed -= instance.OnInventorySlot3;
            @InventorySlot3.canceled -= instance.OnInventorySlot3;
            @InventorySlot4.started -= instance.OnInventorySlot4;
            @InventorySlot4.performed -= instance.OnInventorySlot4;
            @InventorySlot4.canceled -= instance.OnInventorySlot4;
        }

        public void RemoveCallbacks(IInventoryActions instance)
        {
            if (m_Wrapper.m_InventoryActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInventoryActions instance)
        {
            foreach (var item in m_Wrapper.m_InventoryActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InventoryActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InventoryActions @Inventory => new InventoryActions(this);

    // Pause
    private readonly InputActionMap m_Pause;
    private List<IPauseActions> m_PauseActionsCallbackInterfaces = new List<IPauseActions>();
    private readonly InputAction m_Pause_TogglePause;
    public struct PauseActions
    {
        private @WizardInput m_Wrapper;
        public PauseActions(@WizardInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @TogglePause => m_Wrapper.m_Pause_TogglePause;
        public InputActionMap Get() { return m_Wrapper.m_Pause; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PauseActions set) { return set.Get(); }
        public void AddCallbacks(IPauseActions instance)
        {
            if (instance == null || m_Wrapper.m_PauseActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PauseActionsCallbackInterfaces.Add(instance);
            @TogglePause.started += instance.OnTogglePause;
            @TogglePause.performed += instance.OnTogglePause;
            @TogglePause.canceled += instance.OnTogglePause;
        }

        private void UnregisterCallbacks(IPauseActions instance)
        {
            @TogglePause.started -= instance.OnTogglePause;
            @TogglePause.performed -= instance.OnTogglePause;
            @TogglePause.canceled -= instance.OnTogglePause;
        }

        public void RemoveCallbacks(IPauseActions instance)
        {
            if (m_Wrapper.m_PauseActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPauseActions instance)
        {
            foreach (var item in m_Wrapper.m_PauseActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PauseActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PauseActions @Pause => new PauseActions(this);
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
    public interface IInteractionsActions
    {
        void OnInteract(InputAction.CallbackContext context);
    }
    public interface IGlyphsActions
    {
        void OnInteract(InputAction.CallbackContext context);
    }
    public interface IInventoryActions
    {
        void OnScrollInventory(InputAction.CallbackContext context);
        void OnInventorySlot1(InputAction.CallbackContext context);
        void OnInventorySlot2(InputAction.CallbackContext context);
        void OnInventorySlot3(InputAction.CallbackContext context);
        void OnInventorySlot4(InputAction.CallbackContext context);
    }
    public interface IPauseActions
    {
        void OnTogglePause(InputAction.CallbackContext context);
    }
}
