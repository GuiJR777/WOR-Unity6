/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Ponto de recepção de hits que encaminha DamageSpec ao AbilitySystemComponent do dono.
*/
using Ramirestech.Abilities;
using UnityEngine;

namespace Ramirestech.Combat
{
    public sealed class HurtBox : MonoBehaviour
    {
        #region Private Variables
        [Header("Owner")]
        [Tooltip("Ability System que receberá o dano. Se vazio procura no parent.")]
        [SerializeField] private AbilitySystemComponent ownerAbilitySystem;
        #endregion

        #region Properties
        public AbilitySystemComponent OwnerAbilitySystem => ownerAbilitySystem;
        #endregion

        #region Monobehaviour Methods
        private void Awake()
        {
            if (ownerAbilitySystem == null) ownerAbilitySystem = GetComponentInParent<AbilitySystemComponent>();
        }
        #endregion

        #region Public Methods
        public void ReceiveHit(in DamageSpec spec)
        {
            if (ownerAbilitySystem == null || ownerAbilitySystem == spec.Source) return;
            ownerAbilitySystem.ReceiveDamage(spec);
        }
        #endregion
    }
}
