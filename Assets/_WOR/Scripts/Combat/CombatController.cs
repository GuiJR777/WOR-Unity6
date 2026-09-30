/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Orquestra execução de ataques, Animator e janelas prototype de HitBox.
*/
using System;
using System.Collections;
using Ramirestech.Abilities;
using UnityEngine;

namespace Ramirestech.Combat
{
    [RequireComponent(typeof(AbilitySystemComponent))]
    public sealed class CombatController : MonoBehaviour
    {
        #region Private Variables
        [Header("Dependencies")]
        [Tooltip("HitBox principal usada pelos ataques corpo a corpo.")]
        [SerializeField] private HitBox mainHitBox;

        [Tooltip("Animator opcional. Até o Ryu ser integrado, timers prototype controlam o ataque.")]
        [SerializeField] private Animator animator;

        private AbilitySystemComponent abilitySystem;
        private Coroutine attackRoutine;
        private AttackDefinition currentAttack;
        private bool currentAttackHit;
        #endregion

        #region Properties
        public bool IsAttacking => attackRoutine != null;
        #endregion

        #region Events
        public event Action<AttackDefinition> AttackStarted;
        public event Action<AttackDefinition, HurtBox, DamageSpec> AttackHitConfirmed;
        public event Action<AttackDefinition, bool> AttackFinished;
        #endregion

        #region Monobehaviour Methods
        private void Awake()
        {
            abilitySystem = GetComponent<AbilitySystemComponent>();
            if (mainHitBox == null) mainHitBox = GetComponentInChildren<HitBox>(true);
            if (animator == null) animator = GetComponentInChildren<Animator>(true);

            if (mainHitBox != null) mainHitBox.HitConfirmed += OnHitConfirmed;
        }

        private void OnDestroy()
        {
            if (mainHitBox != null) mainHitBox.HitConfirmed -= OnHitConfirmed;
        }
        #endregion

        #region Public Methods
        public bool ExecuteAttack(AttackDefinition definition)
        {
            if (definition == null || IsAttacking || abilitySystem.HasTag("State.Dead")) return false;
            attackRoutine = StartCoroutine(AttackRoutine(definition));
            return true;
        }

        public void OpenHitBox()
        {
            if (currentAttack != null && mainHitBox != null) mainHitBox.Begin(abilitySystem, currentAttack);
        }

        public void CloseHitBox() => mainHitBox?.End();
        #endregion

        #region Private Methods
        private IEnumerator AttackRoutine(AttackDefinition definition)
        {
            currentAttack = definition;
            currentAttackHit = false;
            abilitySystem.AddTag("State.Attacking");
            AttackStarted?.Invoke(definition);

            if (animator != null && !string.IsNullOrWhiteSpace(definition.AnimatorTrigger))
                animator.SetTrigger(definition.AnimatorTrigger);

            yield return new WaitForSeconds(definition.Windup);
            mainHitBox?.Begin(abilitySystem, definition);

            yield return new WaitForSeconds(definition.ActiveTime);
            mainHitBox?.End();

            yield return new WaitForSeconds(definition.Recovery);

            abilitySystem.RemoveTag("State.Attacking");
            AttackFinished?.Invoke(definition, currentAttackHit);
            currentAttack = null;
            attackRoutine = null;
        }

        private void OnHitConfirmed(HurtBox hurtBox, DamageSpec spec)
        {
            currentAttackHit = true;
            AttackHitConfirmed?.Invoke(currentAttack, hurtBox, spec);
        }
        #endregion
    }
}
