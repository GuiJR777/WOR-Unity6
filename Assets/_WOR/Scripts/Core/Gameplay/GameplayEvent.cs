/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Evento semântico de gameplay compartilhado por combate, técnicas e demais sistemas.
*/
using UnityEngine;

namespace Ramirestech.Core.Gameplay
{
    public readonly struct GameplayEvent
    {
        #region Properties
        public GameplayTag Tag { get; }
        public GameObject Instigator { get; }
        public GameObject Target { get; }
        public float Magnitude { get; }
        public Vector3 Position { get; }
        #endregion

        #region Public Methods
        public GameplayEvent(GameplayTag tag, GameObject instigator, GameObject target, float magnitude, Vector3 position)
        {
            Tag = tag;
            Instigator = instigator;
            Target = target;
            Magnitude = magnitude;
            Position = position;
        }
        #endregion
    }
}
