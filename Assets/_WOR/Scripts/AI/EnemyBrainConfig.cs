/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Configuração data-driven da IA prototype para spacing, strafe e ataques.
*/
using UnityEngine;

namespace Ramirestech.AI
{
    [CreateAssetMenu(menuName = "Whiskers of Rage/AI/Enemy Brain Config", fileName = "AI_Enemy")]
    public sealed class EnemyBrainConfig : ScriptableObject
    {
        #region Private Variables
        [Header("Awareness")]
        [Tooltip("Distância em que o inimigo começa a reagir ao Player.")]
        [SerializeField, Min(0f)] private float detectionRange = 12f;

        [Tooltip("Distância máxima para tentar atacar.")]
        [SerializeField, Min(0f)] private float attackRange = 2.2f;

        [Tooltip("Distância alvo mantida enquanto faz spacing.")]
        [SerializeField, Min(0f)] private float stopDistance = 1.6f;

        [Header("Decision")]
        [Tooltip("Intervalo mínimo entre decisões da IA.")]
        [SerializeField, Min(0.05f)] private float decisionInterval = 0.2f;

        [Tooltip("Chance prototype de escolher strafe quando está em alcance de combate.")]
        [SerializeField, Range(0f, 1f)] private float strafeChance = 0.35f;

        [Tooltip("Tempo médio de strafe antes de uma nova decisão.")]
        [SerializeField, Min(0.05f)] private float strafeDuration = 0.7f;
        #endregion

        #region Properties
        public float DetectionRange => detectionRange;
        public float AttackRange => attackRange;
        public float StopDistance => stopDistance;
        public float DecisionInterval => decisionInterval;
        public float StrafeChance => strafeChance;
        public float StrafeDuration => strafeDuration;
        #endregion
    }
}
