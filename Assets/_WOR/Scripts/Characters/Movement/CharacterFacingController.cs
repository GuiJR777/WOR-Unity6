/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Controla facing visual a partir do movimento sem acoplar rotação ao motor físico.
*/
using UnityEngine;

namespace Ramirestech.Characters.Movement
{
    [RequireComponent(typeof(CharacterMotor))]
    public sealed class CharacterFacingController : MonoBehaviour
    {
        #region Private Variables
        [Header("Facing")]
        [Tooltip("Root visual rotacionado. Se vazio, procura o filho Visuals; em último caso usa o próprio transform.")]
        [SerializeField] private Transform visuals;

        [Tooltip("Velocidade de rotação em graus por segundo.")]
        [SerializeField, Min(0f)] private float rotationSpeed = 900f;

        private CharacterMotor motor;
        #endregion

        #region Properties
        public Vector3 Forward => visuals != null ? visuals.forward : transform.forward;
        #endregion

        #region Monobehaviour Methods
        private void Awake()
        {
            motor = GetComponent<CharacterMotor>();
            if (visuals == null)
            {
                Transform found = transform.Find("Visuals");
                visuals = found != null ? found : transform;
            }
        }

        private void Update()
        {
            Vector3 direction = motor.DesiredWorldDirection;
            if (direction.sqrMagnitude < 0.001f) return;

            Quaternion target = Quaternion.LookRotation(direction, Vector3.up);
            visuals.rotation = Quaternion.RotateTowards(visuals.rotation, target, rotationSpeed * Time.deltaTime);
        }
        #endregion

        #region Public Methods
        public void Face(Vector3 worldDirection)
        {
            worldDirection.y = 0f;
            if (worldDirection.sqrMagnitude < 0.001f) return;
            visuals.rotation = Quaternion.LookRotation(worldDirection.normalized, Vector3.up);
        }
        #endregion
    }
}
