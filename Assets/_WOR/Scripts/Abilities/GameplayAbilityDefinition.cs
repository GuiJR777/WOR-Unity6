/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Definição data-driven base para Gameplay Abilities executadas pelo AbilitySystemComponent.
*/
using System.Collections.Generic;
using UnityEngine;

namespace Ramirestech.Abilities
{
    public abstract class GameplayAbilityDefinition : ScriptableObject
    {
        #region Private Variables
        [Header("Identity")]
        [Tooltip("ID semântico da ability, por exemplo Ability.Attack.Light01.")]
        [SerializeField] private string abilityTag = "Ability.Unknown";

        [Header("Activation Rules")]
        [Tooltip("Tags que precisam estar presentes para ativar a Ability.")]
        [SerializeField] private List<string> requiredTags = new();

        [Tooltip("Tags que bloqueiam a ativação da Ability.")]
        [SerializeField] private List<string> blockedTags = new() { "State.Dead", "State.Stunned", "State.KnockedDown" };

        [Tooltip("Cooldown prototype em segundos.")]
        [SerializeField, Min(0f)] private float cooldown;
        #endregion

        #region Properties
        public string AbilityTag => abilityTag;
        public IReadOnlyList<string> RequiredTags => requiredTags;
        public IReadOnlyList<string> BlockedTags => blockedTags;
        public float Cooldown => cooldown;
        #endregion

        #region Public Methods
        public virtual bool CanActivate(AbilitySystemComponent system, in AbilityContext context) => true;
        public abstract bool Activate(AbilitySystemComponent system, in AbilityContext context);
        #endregion
    }
}
