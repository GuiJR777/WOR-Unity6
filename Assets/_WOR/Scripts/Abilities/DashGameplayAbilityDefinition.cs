/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Gameplay Ability de Dash configurável e reutilizável por player e AI.
*/
using Ramirestech.Characters.Movement;
using UnityEngine;

namespace Ramirestech.Abilities
{
    [CreateAssetMenu(menuName = "Whiskers of Rage/Abilities/Dash Ability", fileName = "GA_Dash")]
    public sealed class DashGameplayAbilityDefinition : GameplayAbilityDefinition
    {
        #region Private Variables
        [Header("Dash")]
        [Tooltip("Distância percorrida pelo dash.")]
        [SerializeField, Min(0f)] private float distance = 4f;

        [Tooltip("Duração do dash em segundos.")]
        [SerializeField, Min(0.01f)] private float duration = 0.16f;

        [Tooltip("Se concede invulnerabilidade durante a duração prototype.")]
        [SerializeField] private bool invulnerable = true;
        #endregion

        #region Public Methods
        public override bool Activate(AbilitySystemComponent system, in AbilityContext context)
        {
            CharacterMotor motor = system.GetComponent<CharacterMotor>();
            if (motor == null) return false;

            Vector3 direction = context.Direction.sqrMagnitude > 0.001f
                ? context.Direction
                : system.transform.forward;

            bool activated = motor.TryDash(direction, distance, duration);

            if (activated && invulnerable)
                system.AddTagForDuration("State.Invulnerable", duration);

            return activated;
        }
        #endregion
    }
}
