/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Facade do Unity Input System para expor intenções de gameplay sem consultar teclas diretamente.
*/
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ramirestech.Core.Input
{
    public sealed class PlayerInputReader : MonoBehaviour
    {
        #region Private Variables
        [Header("Input Asset")]
        [Tooltip("InputActionAsset usado pelo player. O Quick Start tenta vincular WOR_InputActions automaticamente.")]
        [SerializeField] private InputActionAsset inputActions;

        [Tooltip("Nome do Action Map de gameplay.")]
        [SerializeField] private string actionMapName = "Player";

        private InputActionMap playerMap;
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction lightAttackAction;
        private InputAction heavyAttackAction;
        private InputAction dashAction;
        private InputAction blockAction;
        private InputAction parryAction;
        #endregion

        #region Properties
        public Vector2 Move => moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
        public bool IsBlockHeld => blockAction?.IsPressed() ?? false;
        #endregion

        #region Events
        public event Action JumpPressed;
        public event Action JumpReleased;
        public event Action LightAttackPressed;
        public event Action HeavyAttackPressed;
        public event Action DashPressed;
        public event Action BlockStarted;
        public event Action BlockCanceled;
        public event Action ParryPressed;
        #endregion

        #region Monobehaviour Methods
        private void Awake() => ResolveActions();

        private void OnEnable()
        {
            ResolveActions();
            Subscribe();
            playerMap?.Enable();
        }

        private void OnDisable()
        {
            Unsubscribe();
            playerMap?.Disable();
        }
        #endregion

        #region Private Methods
        private void ResolveActions()
        {
            if (inputActions == null) return;
            playerMap = inputActions.FindActionMap(actionMapName, false);
            if (playerMap == null) return;
            moveAction = playerMap.FindAction("Move", false);
            jumpAction = playerMap.FindAction("Jump", false);
            lightAttackAction = playerMap.FindAction("LightAttack", false);
            heavyAttackAction = playerMap.FindAction("HeavyAttack", false);
            dashAction = playerMap.FindAction("Dash", false);
            blockAction = playerMap.FindAction("Block", false);
            parryAction = playerMap.FindAction("Parry", false);
        }

        private void Subscribe()
        {
            if (jumpAction != null) { jumpAction.performed += OnJumpPerformed; jumpAction.canceled += OnJumpCanceled; }
            if (lightAttackAction != null) lightAttackAction.performed += OnLightAttack;
            if (heavyAttackAction != null) heavyAttackAction.performed += OnHeavyAttack;
            if (dashAction != null) dashAction.performed += OnDash;
            if (blockAction != null) { blockAction.started += OnBlockStarted; blockAction.canceled += OnBlockCanceled; }
            if (parryAction != null) parryAction.performed += OnParry;
        }

        private void Unsubscribe()
        {
            if (jumpAction != null) { jumpAction.performed -= OnJumpPerformed; jumpAction.canceled -= OnJumpCanceled; }
            if (lightAttackAction != null) lightAttackAction.performed -= OnLightAttack;
            if (heavyAttackAction != null) heavyAttackAction.performed -= OnHeavyAttack;
            if (dashAction != null) dashAction.performed -= OnDash;
            if (blockAction != null) { blockAction.started -= OnBlockStarted; blockAction.canceled -= OnBlockCanceled; }
            if (parryAction != null) parryAction.performed -= OnParry;
        }

        private void OnJumpPerformed(InputAction.CallbackContext _) => JumpPressed?.Invoke();
        private void OnJumpCanceled(InputAction.CallbackContext _) => JumpReleased?.Invoke();
        private void OnLightAttack(InputAction.CallbackContext _) => LightAttackPressed?.Invoke();
        private void OnHeavyAttack(InputAction.CallbackContext _) => HeavyAttackPressed?.Invoke();
        private void OnDash(InputAction.CallbackContext _) => DashPressed?.Invoke();
        private void OnBlockStarted(InputAction.CallbackContext _) => BlockStarted?.Invoke();
        private void OnBlockCanceled(InputAction.CallbackContext _) => BlockCanceled?.Invoke();
        private void OnParry(InputAction.CallbackContext _) => ParryPressed?.Invoke();
        #endregion

        #region Public Methods
        public void SetInputActions(InputActionAsset asset)
        {
            inputActions = asset;
            ResolveActions();
        }
        #endregion
    }
}
