/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Conjunto runtime de atributos compartilhado por player e inimigos.
*/
using UnityEngine;

namespace Ramirestech.Abilities
{
    public sealed class AttributeSet : MonoBehaviour
    {
        #region Private Variables
        [Header("Primary Attributes")]
        [Tooltip("Vida máxima do personagem.")]
        [SerializeField, Min(1f)] private float maxHealth = 100f;

        [Tooltip("Dano físico base usado por ataques.")]
        [SerializeField, Min(0f)] private float attack = 10f;

        [Tooltip("Defesa usada na redução prototype de dano.")]
        [SerializeField, Min(0f)] private float defense = 5f;

        [Tooltip("Poise máximo antes de entrar em reação forte.")]
        [SerializeField, Min(0f)] private float maxPoise = 50f;

        [Tooltip("Agilidade base reservada para mobilidade e janelas futuras.")]
        [SerializeField, Min(0f)] private float agility = 10f;
        #endregion

        #region Properties
        public float MaxHealth => maxHealth;
        public float Attack => attack;
        public float Defense => defense;
        public float MaxPoise => maxPoise;
        public float Agility => agility;
        #endregion
    }
}
