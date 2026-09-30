/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Valor semântico leve para identificar estados, habilidades e condições de gameplay.
*/
using System;

namespace Ramirestech.Core.Gameplay
{
    [Serializable]
    public readonly struct GameplayTag : IEquatable<GameplayTag>
    {
        #region Properties
        public string Value { get; }
        public bool IsValid => !string.IsNullOrWhiteSpace(Value);
        #endregion

        #region Public Methods
        public GameplayTag(string value) => Value = value?.Trim() ?? string.Empty;
        public bool Equals(GameplayTag other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object obj) => obj is GameplayTag other && Equals(other);
        public override int GetHashCode() => Value == null ? 0 : StringComparer.Ordinal.GetHashCode(Value);
        public override string ToString() => Value ?? string.Empty;
        public static implicit operator GameplayTag(string value) => new(value);
        public static bool operator ==(GameplayTag left, GameplayTag right) => left.Equals(right);
        public static bool operator !=(GameplayTag left, GameplayTag right) => !left.Equals(right);
        #endregion
    }
}
