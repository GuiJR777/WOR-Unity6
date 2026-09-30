/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Estado base com referência opcional a um estado pai para organização hierárquica.
*/
namespace Ramirestech.Core.HFSM
{
    public abstract class HierarchicalState : IState
    {
        #region Properties
        public abstract string Name { get; }
        public HierarchicalState Parent { get; }
        #endregion

        #region Protected Methods
        protected HierarchicalState(HierarchicalState parent = null) => Parent = parent;
        #endregion

        #region Public Methods
        public virtual void Enter() { }
        public virtual void Tick(float deltaTime) { }
        public virtual void Exit() { }
        #endregion
    }
}
