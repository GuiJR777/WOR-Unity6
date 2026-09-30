/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 22/09/2026

Descrição: Contrato de input compartilhável entre diferentes fontes de controle de personagens.
*/

using System;
using UnityEngine;

namespace Ramirestech.Core.Input
{
    public interface ICharacterInputSource
    {
        #region Properties

        Vector2 MoveInput { get; }

        bool IsJumpHeld { get; }

        #endregion

        #region Events

        event Action JumpPressed;

        event Action JumpReleased;

        #endregion
    }
}