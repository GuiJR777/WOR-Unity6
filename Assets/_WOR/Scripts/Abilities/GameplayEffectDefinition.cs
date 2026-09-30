/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Definição mínima de Gameplay Effect para expansão de buffs, debuffs e efeitos persistentes.
*/
using UnityEngine;

namespace Ramirestech.Abilities
{
    public enum GameplayEffectDurationType
    {
        Instant,
        Duration,
        Infinite
    }

    [CreateAssetMenu(menuName = "Whiskers of Rage/Abilities/Gameplay Effect", fileName = "GE_NewEffect")]
    public sealed class GameplayEffectDefinition : ScriptableObject
    {
        #region Private Variables
        [Header("Effect")]
        [Tooltip("Tipo de duração do efeito.")]
        [SerializeField] private GameplayEffectDurationType durationType = GameplayEffectDurationType.Instant;

        [Tooltip("Duração em segundos quando o tipo for Duration.")]
        [SerializeField, Min(0f)] private float duration;

        [Tooltip("Tag concedida enquanto o efeito estiver ativo.")]
        [SerializeField] private string grantedTag;
        #endregion

        #region Properties
        public GameplayEffectDurationType DurationType => durationType;
        public float Duration => duration;
        public string GrantedTag => grantedTag;
        #endregion
    }
}
