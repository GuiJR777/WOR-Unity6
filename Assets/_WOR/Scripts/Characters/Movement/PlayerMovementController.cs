/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Adapta o input humano para o CharacterMotor usando orientação previsível da câmera.
*/
using Ramirestech.Core.Input;
using UnityEngine;

namespace Ramirestech.Characters.Movement
{
    [RequireComponent(typeof(CharacterMotor))]
    public sealed class PlayerMovementController : MonoBehaviour
    {
        #region Private Variables
        [Header("Dependencies")]
        [Tooltip("Fonte de input do Player.")]
        [SerializeField] private PlayerInputReader inputReader;

        [Tooltip("Câmera usada para projetar o input no plano XZ. Se vazio usa Camera.main.")]
        [SerializeField] private Transform cameraTransform;

        private CharacterMotor motor;
        #endregion

        #region Monobehaviour Methods
        private void Awake()
        {
            motor = GetComponent<CharacterMotor>();
            if (inputReader == null) inputReader = GetComponent<PlayerInputReader>();
            if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            if (inputReader == null || motor == null) return;
            inputReader.JumpPressed += motor.RequestJump;
            inputReader.JumpReleased += motor.ReleaseJump;
        }

        private void OnDisable()
        {
            if (inputReader == null || motor == null) return;
            inputReader.JumpPressed -= motor.RequestJump;
            inputReader.JumpReleased -= motor.ReleaseJump;
        }

        private void Update()
        {
            if (inputReader == null || motor == null) return;
            Vector2 input = Vector2.ClampMagnitude(inputReader.Move, 1f);
            motor.SetWorldMoveDirection(CalculateCameraRelative(input));
        }
        #endregion

        #region Private Methods
        private Vector3 CalculateCameraRelative(Vector2 input)
        {
            if (cameraTransform == null) return new Vector3(input.x, 0f, input.y);

            Vector3 forward = cameraTransform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = cameraTransform.right;
            right.y = 0f;
            right.Normalize();

            return Vector3.ClampMagnitude(right * input.x + forward * input.y, 1f);
        }
        #endregion
    }
}
