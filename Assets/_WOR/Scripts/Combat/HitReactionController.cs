/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Resolve Poise, Stun, Knockback, Launch e Knockdown a partir do dano processado pelo Ability System.
*/
using System.Collections;
using Ramirestech.Abilities;
using Ramirestech.Characters.Movement;
using UnityEngine;

namespace Ramirestech.Combat
{
    [RequireComponent(typeof(AbilitySystemComponent))]
    public sealed class HitReactionController : MonoBehaviour
    {
        #region Private Variables
        [Header("Poise")]
        [Tooltip("Tempo sem receber dano antes do Poise ser restaurado totalmente.")]
        [SerializeField, Min(0f)] private float poiseRecoveryDelay = 1.5f;

        private AbilitySystemComponent abilitySystem;
        private CharacterMotor motor;
        private AttributeSet attributes;
        private float currentPoise;
        private float lastHitTime;
        #endregion

        #region Monobehaviour Methods
        private void Awake()
        {
            abilitySystem = GetComponent<AbilitySystemComponent>();
            motor = GetComponent<CharacterMotor>();
            attributes = GetComponent<AttributeSet>();
            currentPoise = attributes.MaxPoise;
        }

        private void OnEnable()
        {
            if (abilitySystem != null) abilitySystem.DamageReceived += OnDamageReceived;
        }

        private void OnDisable()
        {
            if (abilitySystem != null) abilitySystem.DamageReceived -= OnDamageReceived;
        }

        private void Update()
        {
            if (Time.time - lastHitTime >= poiseRecoveryDelay)
                currentPoise = attributes.MaxPoise;
        }
        #endregion

        #region Private Methods
        private void OnDamageReceived(DamageSpec spec, float _)
        {
            if (spec.Attack == null) return;
            lastHitTime = Time.time;
            currentPoise -= spec.PoiseDamage;

            Vector3 impulse = spec.HitDirection * spec.Attack.Knockback + Vector3.up * spec.Attack.Launch;
            motor?.AddImpulse(impulse);

            bool poiseBroken = currentPoise <= 0f;

            if (spec.Attack.CausesKnockdown || poiseBroken)
            {
                currentPoise = attributes.MaxPoise;
                StartCoroutine(TemporaryState("State.KnockedDown", Mathf.Max(0.1f, spec.Attack.KnockdownDuration)));
            }
            else if (spec.Attack.CausesStun)
            {
                StartCoroutine(TemporaryState("State.Stunned", Mathf.Max(0.05f, spec.Attack.StunDuration)));
            }
        }

        private IEnumerator TemporaryState(string tag, float duration)
        {
            abilitySystem.AddTag(tag);
            yield return new WaitForSeconds(duration);
            abilitySystem.RemoveTag(tag);
        }
        #endregion
    }
}
