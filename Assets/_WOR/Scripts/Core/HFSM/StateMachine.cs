/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Máquina de estados runtime com transições explícitas e rastreáveis.
*/
using System;

namespace Ramirestech.Core.HFSM
{
    public sealed class StateMachine
    {
        #region Properties
        public IState CurrentState { get; private set; }
        #endregion

        #region Events
        public event Action<IState, IState> StateChanged;
        #endregion

        #region Public Methods
        public void ChangeState(IState next)
        {
            if (next == null || ReferenceEquals(CurrentState, next)) return;
            IState previous = CurrentState;
            CurrentState?.Exit();
            CurrentState = next;
            CurrentState.Enter();
            StateChanged?.Invoke(previous, CurrentState);
        }

        public void Tick(float deltaTime) => CurrentState?.Tick(deltaTime);
        #endregion
    }
}
