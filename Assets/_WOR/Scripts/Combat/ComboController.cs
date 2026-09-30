/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Resolve combo por hit-confirm e impede progressão quando o golpe erra.
*/
using Ramirestech.Abilities;
using UnityEngine;

namespace Ramirestech.Combat
{
    [RequireComponent(typeof(AbilitySystemComponent))]
    [RequireComponent(typeof(CombatController))]
    public sealed class ComboController : MonoBehaviour
    {
        #region Private Variables
        [Header("Combo")]
        [Tooltip("Combo Light principal do personagem.")]
        [SerializeField] private ComboDefinition lightCombo;

        private AbilitySystemComponent abilitySystem;
        private CombatController combat;
        private int currentIndex;
        private float lastConfirmedTime = float.NegativeInfinity;
        #endregion

        #region Monobehaviour Methods
        private void Awake()
        {
            abilitySystem = GetComponent<AbilitySystemComponent>();
            combat = GetComponent<CombatController>();
        }

        private void OnEnable()
        {
            combat.AttackFinished += OnAttackFinished;
        }

        private void OnDisable()
        {
            if (combat != null) combat.AttackFinished -= OnAttackFinished;
        }

        private void Update()
        {
            if (lightCombo == null) return;

            if (currentIndex > 0 &&
                !combat.IsAttacking &&
                Time.time - lastConfirmedTime > lightCombo.ResetDelay)
            {
                ResetCombo();
            }
        }
        #endregion

        #region Public Methods
        public bool TryLightAttack()
        {
            if (lightCombo == null || lightCombo.Attacks.Count == 0 || combat.IsAttacking) return false;
            if (currentIndex >= lightCombo.Attacks.Count) currentIndex = 0;

            AttackGameplayAbilityDefinition ability = lightCombo.Attacks[currentIndex];
            AbilityContext context = new(abilitySystem, null, transform.forward, transform.position);
            return abilitySystem.TryActivate(ability, context);
        }

        public void SetLightCombo(ComboDefinition definition) => lightCombo = definition;
        #endregion

        #region Private Methods
        private void OnAttackFinished(AttackDefinition _, bool hitConfirmed)
        {
            if (hitConfirmed)
            {
                lastConfirmedTime = Time.time;
                currentIndex++;

                if (lightCombo != null && currentIndex >= lightCombo.Attacks.Count)
                    currentIndex = 0;
            }
            else
            {
                ResetCombo();
            }
        }

        private void ResetCombo()
        {
            currentIndex = 0;
            lastConfirmedTime = float.NegativeInfinity;
        }
        #endregion
    }
}
