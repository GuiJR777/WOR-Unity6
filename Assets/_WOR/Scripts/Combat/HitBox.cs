/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: HitBox runtime baseada em Physics.OverlapBox, com hit-confirm e prevenção de multi-hit por ativação.
*/
using System;
using System.Collections.Generic;
using Ramirestech.Abilities;
using UnityEngine;

namespace Ramirestech.Combat
{
    public sealed class HitBox : MonoBehaviour
    {
        #region Private Variables
        private readonly HashSet<AbilitySystemComponent> hitTargets = new();
        private AbilitySystemComponent source;
        private AttackDefinition attack;
        private bool active;
        #endregion

        #region Events
        public event Action<HurtBox, DamageSpec> HitConfirmed;
        #endregion

        #region Monobehaviour Methods
        private void Update()
        {
            if (active) Scan();
        }
        #endregion

        #region Public Methods
        public void Begin(AbilitySystemComponent abilitySource, AttackDefinition definition)
        {
            source = abilitySource;
            attack = definition;
            hitTargets.Clear();
            active = source != null && attack != null;
        }

        public void End()
        {
            active = false;
            hitTargets.Clear();
        }
        #endregion

        #region Private Methods
        private void Scan()
        {
            Vector3 center = transform.TransformPoint(attack.HitboxOffset);
            Collider[] colliders = Physics.OverlapBox(
                center,
                attack.HitboxHalfExtents,
                transform.rotation,
                attack.HittableLayers,
                QueryTriggerInteraction.Collide);

            foreach (Collider collider in colliders)
            {
                HurtBox hurtBox = collider.GetComponent<HurtBox>() ?? collider.GetComponentInParent<HurtBox>();
                if (hurtBox == null || hurtBox.OwnerAbilitySystem == null || hurtBox.OwnerAbilitySystem == source) continue;
                if (!hitTargets.Add(hurtBox.OwnerAbilitySystem)) continue;

                Transform targetTransform = hurtBox.OwnerAbilitySystem.transform;
                Vector3 sourceToTarget = targetTransform.position - source.transform.position;
                sourceToTarget.y = 0f;

                Vector3 directionToSource = source.transform.position - targetTransform.position;
                directionToSource.y = 0f;

                bool backAttack = directionToSource.sqrMagnitude > 0.001f &&
                                  Vector3.Dot(targetTransform.forward, directionToSource.normalized) < -0.35f;

                float damage = source.Attributes.Attack * attack.AttackScale + attack.FlatDamage;
                if (backAttack) damage *= attack.BackAttackMultiplier;

                DamageSpec spec = new(
                    source,
                    attack,
                    damage,
                    attack.PoiseDamage,
                    sourceToTarget.sqrMagnitude > 0.001f ? sourceToTarget.normalized : source.transform.forward,
                    backAttack);

                hurtBox.ReceiveHit(spec);
                HitConfirmed?.Invoke(hurtBox, spec);
            }
        }
        #endregion
    }
}
