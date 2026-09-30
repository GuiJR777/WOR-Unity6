/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Gameplay Ability data-driven que executa um AttackDefinition através do CombatController.
*/
using Ramirestech.Combat;
using UnityEngine;

namespace Ramirestech.Abilities
{
    [CreateAssetMenu(menuName = "Whiskers of Rage/Abilities/Attack Ability", fileName = "GA_Attack")]
    public sealed class AttackGameplayAbilityDefinition : GameplayAbilityDefinition
    {
        #region Private Variables
        [Header("Attack")]
        [Tooltip("Definição do golpe executado por esta Ability.")]
        [SerializeField] private AttackDefinition attack;
        #endregion

        #region Properties
        public AttackDefinition Attack => attack;
        #endregion

        #region Public Methods
        public override bool Activate(AbilitySystemComponent system, in AbilityContext context)
        {
            CombatController combat = system.GetComponent<CombatController>();
            return combat != null && combat.ExecuteAttack(attack);
        }
        #endregion
    }
}
