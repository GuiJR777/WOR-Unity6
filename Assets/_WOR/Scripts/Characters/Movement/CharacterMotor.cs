/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Motor compartilhável de movimento 3D com pulo matemático, dash e impulsos externos.
*/
using System;
using System.Collections;
using UnityEngine;

namespace Ramirestech.Characters.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class CharacterMotor : MonoBehaviour
    {
        #region Private Variables
        [Header("Movement")]
        [Tooltip("Velocidade horizontal máxima no chão.")]
        [SerializeField, Min(0f)] private float moveSpeed = 6f;

        [Tooltip("Multiplicador de controle horizontal no ar.")]
        [SerializeField, Range(0f, 1f)] private float airControl = 0.85f;

        [Header("Jump")]
        [Tooltip("Altura máxima do pulo em unidades.")]
        [SerializeField, Min(0.01f)] private float jumpHeight = 2f;

        [Tooltip("Tempo até o ápice. Define a gravidade junto da altura.")]
        [SerializeField, Min(0.05f)] private float timeToApex = 0.4f;

        [Tooltip("Janela após sair da borda em que o pulo ainda pode ocorrer.")]
        [SerializeField, Min(0f)] private float coyoteTime = 0.12f;

        [Tooltip("Janela em que um input de pulo antecipado fica armazenado.")]
        [SerializeField, Min(0f)] private float jumpBuffer = 0.12f;

        [Tooltip("Multiplicador da velocidade vertical ao soltar o botão durante a subida.")]
        [SerializeField, Range(0f, 1f)] private float jumpCutMultiplier = 0.5f;

        [Tooltip("Velocidade negativa mantida no chão para estabilizar o CharacterController.")]
        [SerializeField] private float groundedStickVelocity = -2f;

        [Header("External Motion")]
        [Tooltip("Velocidade de decaimento dos impulsos externos, como knockback.")]
        [SerializeField, Min(0f)] private float impulseDamping = 12f;

        private CharacterController controller;
        private Vector3 desiredWorldDirection;
        private Vector3 externalVelocity;
        private float verticalVelocity;
        private float lastGroundedTime = float.NegativeInfinity;
        private float lastJumpPressedTime = float.NegativeInfinity;
        private bool jumpHeld;
        private bool dashActive;
        private Vector3 dashVelocity;
        #endregion

        #region Properties
        public bool IsGrounded => controller != null && controller.isGrounded;
        public float VerticalVelocity => verticalVelocity;
        public Vector3 DesiredWorldDirection => desiredWorldDirection;
        public bool IsMoving => desiredWorldDirection.sqrMagnitude > 0.001f;
        private float Gravity => -2f * jumpHeight / (timeToApex * timeToApex);
        private float JumpVelocity => 2f * jumpHeight / timeToApex;
        #endregion

        #region Events
        public event Action Jumped;
        public event Action Landed;
        #endregion

        #region Monobehaviour Methods
        private void Awake() => controller = GetComponent<CharacterController>();

        private void Update()
        {
            bool wasGrounded = IsGrounded;

            if (IsGrounded)
            {
                lastGroundedTime = Time.time;
                if (verticalVelocity < 0f) verticalVelocity = groundedStickVelocity;
            }

            TryConsumeJump();
            verticalVelocity += Gravity * Time.deltaTime;

            float speed = IsGrounded ? moveSpeed : moveSpeed * airControl;
            Vector3 horizontal = dashActive ? dashVelocity : desiredWorldDirection * speed;
            Vector3 velocity = horizontal + Vector3.up * verticalVelocity + externalVelocity;
            controller.Move(velocity * Time.deltaTime);

            externalVelocity = Vector3.MoveTowards(externalVelocity, Vector3.zero, impulseDamping * Time.deltaTime);

            if (!wasGrounded && IsGrounded) Landed?.Invoke();
        }
        #endregion

        #region Public Methods
        public void SetWorldMoveDirection(Vector3 direction)
        {
            direction.y = 0f;
            desiredWorldDirection = Vector3.ClampMagnitude(direction, 1f);
        }

        public void RequestJump()
        {
            jumpHeld = true;
            lastJumpPressedTime = Time.time;
        }

        public void ReleaseJump()
        {
            jumpHeld = false;
            if (verticalVelocity > 0f) verticalVelocity *= jumpCutMultiplier;
        }

        public bool TryDash(Vector3 direction, float distance, float duration)
        {
            if (dashActive || duration <= 0f) return false;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.001f) direction = transform.forward;
            StartCoroutine(DashRoutine(direction.normalized, distance, duration));
            return true;
        }

        public void AddImpulse(Vector3 velocity) => externalVelocity += velocity;
        #endregion

        #region Private Methods
        private void TryConsumeJump()
        {
            bool buffered = Time.time - lastJumpPressedTime <= jumpBuffer;
            bool coyote = Time.time - lastGroundedTime <= coyoteTime;
            if (!buffered || !coyote) return;

            verticalVelocity = JumpVelocity;
            if (!jumpHeld) verticalVelocity *= jumpCutMultiplier;
            lastJumpPressedTime = float.NegativeInfinity;
            lastGroundedTime = float.NegativeInfinity;
            Jumped?.Invoke();
        }

        private IEnumerator DashRoutine(Vector3 direction, float distance, float duration)
        {
            dashActive = true;
            dashVelocity = direction * (distance / duration);
            yield return new WaitForSeconds(duration);
            dashActive = false;
            dashVelocity = Vector3.zero;
        }
        #endregion
    }
}
