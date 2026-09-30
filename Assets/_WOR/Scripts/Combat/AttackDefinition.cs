/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Dados completos de um ataque configurável pelo Inspector.
*/
using UnityEngine;

namespace Ramirestech.Combat
{
    [CreateAssetMenu(menuName = "Whiskers of Rage/Combat/Attack Definition", fileName = "Attack_New")]
    public sealed class AttackDefinition : ScriptableObject
    {
        #region Private Variables
        [Header("Damage")]
        [Tooltip("Percentual multiplicador do Attack do atacante. 1 = 100%.")]
        [SerializeField, Min(0f)] private float attackScale = 1f;

        [Tooltip("Dano fixo somado após o scaling.")]
        [SerializeField, Min(0f)] private float flatDamage;

        [Tooltip("Dano causado ao Poise do alvo.")]
        [SerializeField, Min(0f)] private float poiseDamage = 10f;

        [Tooltip("Multiplicador de dano quando o atacante estiver atrás do alvo.")]
        [SerializeField, Min(1f)] private float backAttackMultiplier = 1.25f;

        [Header("Hit Reaction")]
        [Tooltip("Se o golpe aplica Stun quando conectar.")]
        [SerializeField] private bool causesStun;

        [Tooltip("Duração do Stun em segundos.")]
        [SerializeField, Min(0f)] private float stunDuration = 0.2f;

        [Tooltip("Velocidade horizontal de knockback.")]
        [SerializeField, Min(0f)] private float knockback = 2f;

        [Tooltip("Velocidade vertical de lançamento.")]
        [SerializeField, Min(0f)] private float launch;

        [Tooltip("Se o golpe força Knockdown.")]
        [SerializeField] private bool causesKnockdown;

        [Tooltip("Duração prototype do Knockdown.")]
        [SerializeField, Min(0f)] private float knockdownDuration = 0.7f;

        [Header("HitBox")]
        [Tooltip("Offset local do centro da HitBox em relação ao atacante.")]
        [SerializeField] private Vector3 hitboxOffset = new(0f, 1f, 1f);

        [Tooltip("Meio tamanho da HitBox usada no Physics.OverlapBox.")]
        [SerializeField] private Vector3 hitboxHalfExtents = new(0.75f, 0.8f, 0.8f);

        [Tooltip("LayerMask das HurtBoxes atingíveis.")]
        [SerializeField] private LayerMask hittableLayers = ~0;

        [Header("Animation / Prototype Timing")]
        [Tooltip("Trigger opcional enviado ao Animator quando o ataque começa.")]
        [SerializeField] private string animatorTrigger;

        [Tooltip("Tempo prototype antes da HitBox abrir.")]
        [SerializeField, Min(0f)] private float windup = 0.08f;

        [Tooltip("Tempo prototype em que a HitBox fica ativa.")]
        [SerializeField, Min(0.01f)] private float activeTime = 0.12f;

        [Tooltip("Tempo prototype de recovery após a HitBox fechar.")]
        [SerializeField, Min(0f)] private float recovery = 0.18f;
        #endregion

        #region Properties
        public float AttackScale => attackScale;
        public float FlatDamage => flatDamage;
        public float PoiseDamage => poiseDamage;
        public float BackAttackMultiplier => backAttackMultiplier;
        public bool CausesStun => causesStun;
        public float StunDuration => stunDuration;
        public float Knockback => knockback;
        public float Launch => launch;
        public bool CausesKnockdown => causesKnockdown;
        public float KnockdownDuration => knockdownDuration;
        public Vector3 HitboxOffset => hitboxOffset;
        public Vector3 HitboxHalfExtents => hitboxHalfExtents;
        public LayerMask HittableLayers => hittableLayers;
        public string AnimatorTrigger => animatorTrigger;
        public float Windup => windup;
        public float ActiveTime => activeTime;
        public float Recovery => recovery;
        #endregion
    }
}
