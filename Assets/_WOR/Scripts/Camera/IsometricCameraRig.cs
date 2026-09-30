/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Fallback simples de câmera 3/4 para validar gameplay antes do rig Cinemachine final.
*/
using UnityEngine;

namespace Ramirestech.CameraSystem
{
    public sealed class IsometricCameraRig : MonoBehaviour
    {
        #region Private Variables
        [Header("Follow")]
        [Tooltip("Alvo seguido pela câmera.")]
        [SerializeField] private Transform target;

        [Tooltip("Offset 3/4 em relação ao alvo.")]
        [SerializeField] private Vector3 offset = new(10f, 12f, -10f);

        [Tooltip("Suavização do follow.")]
        [SerializeField, Min(0.01f)] private float followSmooth = 12f;

        [Tooltip("Altura do ponto para onde a câmera olha.")]
        [SerializeField] private float lookHeight = 1f;
        #endregion

        #region Monobehaviour Methods
        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(
                transform.position,
                desired,
                1f - Mathf.Exp(-followSmooth * Time.deltaTime));

            transform.rotation = Quaternion.LookRotation(
                (target.position + Vector3.up * lookHeight) - transform.position,
                Vector3.up);
        }
        #endregion

        #region Public Methods
        public void SetTarget(Transform newTarget) => target = newTarget;
        #endregion
    }
}
