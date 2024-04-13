using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoldByWizards.Input
{
    /// <summary>
    /// Container object that holds a shared <see cref="WizardInput"/> instance.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        /// <summary>
        /// Shared <see cref="WizardInput"/> for consumers to use.
        /// </summary>
        public WizardInput Input { get; private set; }

        // used to disable input from multiple sources without accidentally re-enabling it when we're not supposed to
        public HashSet<PlayerInputDisableSource> PlayerInputDisableSources = new();

        /// <summary>
        /// Enabled state
        /// </summary>
        public bool Enabled { get; private set; }

        public bool PlayerInputEnabled => Input?.Player.enabled ?? false;

        private void Awake()
        {
            Input = new();
            Enable();
        }

        public void Enable()
        {
            foreach (var inputAction in Input)
            {
                inputAction.Enable();
            }

            Enabled = true;
        }

        public void Disable(params InputAction[] ignore)
        {
            foreach (var inputAction in Input)
            {
                // Iterate through our ignore list to see if we should ignore this action
                var willIgnore = false;
                for (var i = 0; i < ignore.Length; i++)
                {
                    willIgnore |= inputAction == ignore[i];
                }

                if (willIgnore) continue;

                inputAction.Disable();
            }

            Enabled = false;
        }

        public void EnablePlayerInput(PlayerInputDisableSource source)
        {
            if (PlayerInputDisableSources.Contains(source))
            {
                PlayerInputDisableSources.Remove(source);
            }

            if (PlayerInputDisableSources.Count != 0)
                return;

            Input.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void DisablePlayerInput(PlayerInputDisableSource source)
        {
            if (PlayerInputDisableSources.Contains(source))
            {
                // do nothing, it's already disabled
                return;
            }

            PlayerInputDisableSources.Add(source);

            Input.Player.Disable();
            Cursor.lockState = CursorLockMode.None;
            // TODO: Don't do this in the pause menu!! This is only to do cool computer stuff
            Cursor.visible = false;
        }
    }
}
