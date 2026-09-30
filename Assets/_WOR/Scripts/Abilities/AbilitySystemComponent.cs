/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Núcleo GAS-inspired mínimo para atributos, tags, cooldowns, abilities, dano e eventos.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using Ramirestech.Combat;
using Ramirestech.Core.Gameplay;
using UnityEngine;

namespace Ramirestech.Abilities
{
    [RequireComponent(typeof(AttributeSet))]
    [RequireComponent(typeof(HealthComponent))]
    public sealed class AbilitySystemComponent : MonoBehaviour
    {
        #region Private Variables
        private readonly Dictionary<GameplayAbilityDefinition, float> cooldownUntil = new();
        private AttributeSet attributes;
        private HealthComponent health;
        private GameplayTagContainer tags;
        #endregion

        #region Properties
        public AttributeSet Attributes => attributes;
        public HealthComponent Health => health;
        public GameplayTagContainer Tags => tags;
        #endregion

        #region Events
        public event Action<GameplayEvent> GameplayEventRaised;
        public event Action<DamageSpec, float> DamageReceived;
        #endregion

        #region Monobehaviour Methods
        private void Awake()
        {
            attributes = GetComponent<AttributeSet>();
            health = GetComponent<HealthComponent>();
            tags = new GameplayTagContainer();
            health.Died += OnDied;
        }

        private void OnDestroy()
        {
            if (health != null) health.Died -= OnDied;
        }
        #endregion

        #region Public Methods
        public bool TryActivate(GameplayAbilityDefinition definition, in AbilityContext context)
        {
            if (definition == null || health.IsDead) return false;
            if (tags.HasAny(definition.BlockedTags) || !tags.HasAll(definition.RequiredTags)) return false;
            if (cooldownUntil.TryGetValue(definition, out float until) && Time.time < until) return false;
            if (!definition.CanActivate(this, context)) return false;

            bool activated = definition.Activate(this, context);
            if (activated && definition.Cooldown > 0f) cooldownUntil[definition] = Time.time + definition.Cooldown;
            return activated;
        }

        public void AddTag(string value) => tags.Add(new GameplayTag(value));
        public void RemoveTag(string value) => tags.Remove(new GameplayTag(value));
        public bool HasTag(string value) => tags.Has(new GameplayTag(value));

        public void AddTagForDuration(string value, float duration)
        {
            if (duration <= 0f) return;
            StartCoroutine(TagDurationRoutine(value, duration));
        }

        public void RaiseEvent(GameplayEvent gameplayEvent) => GameplayEventRaised?.Invoke(gameplayEvent);

        public void ReceiveDamage(in DamageSpec spec)
        {
            if (health.IsDead || HasTag("State.Invulnerable")) return;

            if (HasTag("State.Parrying"))
            {
                RaiseEvent(new GameplayEvent("Event.Parry.Success", gameObject, spec.Source != null ? spec.Source.gameObject : null, 0f, transform.position));
                return;
            }

            float finalDamage = spec.Damage;
            if (HasTag("State.Blocking")) finalDamage *= 0.35f;
            finalDamage *= 100f / (100f + Mathf.Max(0f, attributes.Defense));
            finalDamage = Mathf.Max(0f, finalDamage);

            health.TakeDamage(finalDamage);
            DamageReceived?.Invoke(spec, finalDamage);
            RaiseEvent(new GameplayEvent("Event.Damage.Received", spec.Source != null ? spec.Source.gameObject : null, gameObject, finalDamage, transform.position));
        }
        #endregion

        #region Private Methods
        private IEnumerator TagDurationRoutine(string value, float duration)
        {
            AddTag(value);
            yield return new WaitForSeconds(duration);
            RemoveTag(value);
        }

        private void OnDied()
        {
            AddTag("State.Dead");
            RaiseEvent(new GameplayEvent("Event.Death", gameObject, gameObject, 0f, transform.position));
        }
        #endregion
    }
}
