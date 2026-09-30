/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Pacote imutável de dano enviado por HitBox para HurtBox e resolvido pelo Ability System.
*/
using Ramirestech.Abilities;
using UnityEngine;

namespace Ramirestech.Combat
{
    public readonly struct DamageSpec
    {
        #region Properties
        public AbilitySystemComponent Source { get; }
        public AttackDefinition Attack { get; }
        public float Damage { get; }
        public float PoiseDamage { get; }
        public Vector3 HitDirection { get; }
        public bool IsBackAttack { get; }
        #endregion

        #region Public Methods
        public DamageSpec(AbilitySystemComponent source, AttackDefinition attack, float damage, float poiseDamage, Vector3 hitDirection, bool isBackAttack)
        {
            Source = source;
            Attack = attack;
            Damage = damage;
            PoiseDamage = poiseDamage;
            HitDirection = hitDirection;
            IsBackAttack = isBackAttack;
        }
        #endregion
    }
}
