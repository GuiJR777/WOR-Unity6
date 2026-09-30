/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Ability prototype que concede uma Gameplay Tag por tempo limitado.
*/
using UnityEngine;

namespace Ramirestech.Abilities
{
    [CreateAssetMenu(menuName = "Whiskers of Rage/Abilities/Timed Tag Ability", fileName = "GA_TimedTag")]
    public sealed class TimedTagAbilityDefinition : GameplayAbilityDefinition
    {
        #region Private Variables
        [Header("Timed Tag")]
        [Tooltip("Tag concedida pela Ability.")]
        [SerializeField] private string grantedTag = "State.Parrying";

        [Tooltip("Duração da tag em segundos.")]
        [SerializeField, Min(0.01f)] private float duration = 0.15f;
        #endregion

        #region Public Methods
        public override bool Activate(AbilitySystemComponent system, in AbilityContext context)
        {
            if (string.IsNullOrWhiteSpace(grantedTag)) return false;
            system.AddTagForDuration(grantedTag, duration);
            return true;
        }
        #endregion
    }
}
