/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Container runtime de Gameplay Tags com contagem de fontes e eventos de alteração.
*/
using System;
using System.Collections.Generic;

namespace Ramirestech.Core.Gameplay
{
    public sealed class GameplayTagContainer
    {
        #region Private Variables
        private readonly Dictionary<GameplayTag, int> counts = new();
        #endregion

        #region Events
        public event Action<GameplayTag, bool> TagChanged;
        #endregion

        #region Public Methods
        public bool Has(GameplayTag tag) => tag.IsValid && counts.TryGetValue(tag, out int count) && count > 0;

        public void Add(GameplayTag tag)
        {
            if (!tag.IsValid) return;
            bool wasPresent = Has(tag);
            counts[tag] = counts.TryGetValue(tag, out int count) ? count + 1 : 1;
            if (!wasPresent) TagChanged?.Invoke(tag, true);
        }

        public void Remove(GameplayTag tag)
        {
            if (!tag.IsValid || !counts.TryGetValue(tag, out int count)) return;
            count--;
            if (count <= 0)
            {
                counts.Remove(tag);
                TagChanged?.Invoke(tag, false);
            }
            else counts[tag] = count;
        }

        public bool HasAny(IEnumerable<string> tags)
        {
            if (tags == null) return false;
            foreach (string value in tags) if (Has(new GameplayTag(value))) return true;
            return false;
        }

        public bool HasAll(IEnumerable<string> tags)
        {
            if (tags == null) return true;
            foreach (string value in tags) if (!Has(new GameplayTag(value))) return false;
            return true;
        }
        #endregion
    }
}
