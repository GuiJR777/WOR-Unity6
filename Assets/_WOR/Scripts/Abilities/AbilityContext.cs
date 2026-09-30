/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Contexto de execução de uma Gameplay Ability.
*/
using UnityEngine;

namespace Ramirestech.Abilities
{
    public readonly struct AbilityContext
    {
        #region Properties
        public AbilitySystemComponent Source { get; }
        public GameObject Target { get; }
        public Vector3 Direction { get; }
        public Vector3 Position { get; }
        #endregion

        #region Public Methods
        public AbilityContext(AbilitySystemComponent source, GameObject target, Vector3 direction, Vector3 position)
        {
            Source = source;
            Target = target;
            Direction = direction;
            Position = position;
        }
        #endregion
    }
}
