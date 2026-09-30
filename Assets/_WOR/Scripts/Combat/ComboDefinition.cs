/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Sequência data-driven de Attack Abilities usada por combos com hit-confirm.
*/
using System.Collections.Generic;
using Ramirestech.Abilities;
using UnityEngine;

namespace Ramirestech.Combat
{
    [CreateAssetMenu(menuName = "Whiskers of Rage/Combat/Combo Definition", fileName = "Combo_New")]
    public sealed class ComboDefinition : ScriptableObject
    {
        #region Private Variables
        [Header("Combo")]
        [Tooltip("Sequência de Attack Abilities executada pelo combo.")]
        [SerializeField] private List<AttackGameplayAbilityDefinition> attacks = new();

        [Tooltip("Tempo sem novo input após hit-confirm antes do combo retornar ao primeiro golpe.")]
        [SerializeField, Min(0f)] private float resetDelay = 0.8f;
        #endregion

        #region Properties
        public IReadOnlyList<AttackGameplayAbilityDefinition> Attacks => attacks;
        public float ResetDelay => resetDelay;
        #endregion
    }
}
