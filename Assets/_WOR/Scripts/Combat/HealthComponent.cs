/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Componente compartilhável de vida com eventos de dano, cura e morte.
*/
using System;
using Ramirestech.Abilities;
using UnityEngine;

namespace Ramirestech.Combat
{
    [RequireComponent(typeof(AttributeSet))]
    public sealed class HealthComponent : MonoBehaviour
    {
        #region Private Variables
        private AttributeSet attributes;
        private float currentHealth;
        #endregion

        #region Properties
        public float CurrentHealth => currentHealth;
        public float MaxHealth => attributes != null ? attributes.MaxHealth : 0f;
        public bool IsDead => currentHealth <= 0f;
        #endregion

        #region Events
        public event Action<float, float> HealthChanged;
        public event Action Died;
        #endregion

        #region Monobehaviour Methods
        private void Awake()
        {
            attributes = GetComponent<AttributeSet>();
            currentHealth = attributes.MaxHealth;
        }
        #endregion

        #region Public Methods
        public void TakeDamage(float amount)
        {
            if (IsDead || amount <= 0f) return;
            currentHealth = Mathf.Max(0f, currentHealth - amount);
            HealthChanged?.Invoke(currentHealth, MaxHealth);
            if (IsDead) Died?.Invoke();
        }

        public void Heal(float amount)
        {
            if (IsDead || amount <= 0f) return;
            currentHealth = Mathf.Min(MaxHealth, currentHealth + amount);
            HealthChanged?.Invoke(currentHealth, MaxHealth);
        }
        #endregion
    }
}
