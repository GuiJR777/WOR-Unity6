/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Contrato mínimo para estados reutilizáveis da HFSM.
*/
namespace Ramirestech.Core.HFSM
{
    public interface IState
    {
        string Name { get; }
        void Enter();
        void Tick(float deltaTime);
        void Exit();
    }
}
