/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Adapta intenções de combate do Player para Combo, Ability System, Block e Parry.
*/
using Ramirestech.Abilities;
using Ramirestech.Characters.Movement;
using Ramirestech.Core.Input;
using UnityEngine;

namespace Ramirestech.Combat
{
    [RequireComponent(typeof(AbilitySystemComponent))]
    public sealed class PlayerCombatInput : MonoBehaviour
    {
        #region Private Variables
        [Header("Dependencies")]
        [Tooltip("Fonte de input do Player.")]
        [SerializeField] private PlayerInputReader inputReader;

        [Tooltip("Controller do combo Light.")]
        [SerializeField] private ComboController comboController;

        [Header("Abilities")]
        [Tooltip("Ability do Heavy Attack prototype.")]
        [SerializeField] private AttackGameplayAbilityDefinition heavyAttack;

        [Tooltip("Ability de Dash prototype.")]
        [SerializeField] private DashGameplayAbilityDefinition dashAbility;

        [Tooltip("Ability de Parry prototype.")]
        [SerializeField] private TimedTagAbilityDefinition parryAbility;

        private AbilitySystemComponent abilitySystem;
        private CharacterMotor motor;
        #endregion

        #region Monobehaviour Methods
        private void Awake()
        {
            abilitySystem = GetComponent<AbilitySystemComponent>();
            motor = GetComponent<CharacterMotor>();
            if (inputReader == null) inputReader = GetComponent<PlayerInputReader>();
            if (comboController == null) comboController = GetComponent<ComboController>();
        }

        private void OnEnable()
        {
            if (inputReader == null) return;
            inputReader.LightAttackPressed += OnLightAttack;
            inputReader.HeavyAttackPressed += OnHeavyAttack;
            inputReader.DashPressed += OnDash;
            inputReader.BlockStarted += OnBlockStarted;
            inputReader.BlockCanceled += OnBlockCanceled;
            inputReader.ParryPressed += OnParry;
        }

        private void OnDisable()
        {
            if (inputReader == null) return;
            inputReader.LightAttackPressed -= OnLightAttack;
            inputReader.HeavyAttackPressed -= OnHeavyAttack;
            inputReader.DashPressed -= OnDash;
            inputReader.BlockStarted -= OnBlockStarted;
            inputReader.BlockCanceled -= OnBlockCanceled;
            inputReader.ParryPressed -= OnParry;
        }
        #endregion

        #region Public Methods
        public void Configure(AttackGameplayAbilityDefinition heavy, DashGameplayAbilityDefinition dash, TimedTagAbilityDefinition parry)
        {
            heavyAttack = heavy;
            dashAbility = dash;
            parryAbility = parry;
        }
        #endregion

        #region Private Methods
        private void OnLightAttack() => comboController?.TryLightAttack();

        private void OnHeavyAttack()
        {
            if (heavyAttack == null) return;
            AbilityContext context = new(abilitySystem, null, transform.forward, transform.position);
            abilitySystem.TryActivate(heavyAttack, context);
        }

        private void OnDash()
        {
            if (dashAbility == null) return;
            Vector3 direction = motor != null && motor.DesiredWorldDirection.sqrMagnitude > 0.001f
                ? motor.DesiredWorldDirection
                : transform.forward;
            AbilityContext context = new(abilitySystem, null, direction, transform.position);
            abilitySystem.TryActivate(dashAbility, context);
        }

        private void OnBlockStarted()
        {
            if (!abilitySystem.HasTag("State.Parrying")) abilitySystem.AddTag("State.Blocking");
        }

        private void OnBlockCanceled() => abilitySystem.RemoveTag("State.Blocking");

        private void OnParry()
        {
            if (parryAbility == null) return;
            abilitySystem.RemoveTag("State.Blocking");
            AbilityContext context = new(abilitySystem, null, transform.forward, transform.position);
            abilitySystem.TryActivate(parryAbility, context);
        }
        #endregion
    }
}
