/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Nós mínimos de Behavior Tree para decisão de inimigos sem framework excessivo.
*/
using System;
using System.Collections.Generic;

namespace Ramirestech.AI.BehaviorTree
{
    public abstract class BTNode
    {
        public abstract BTStatus Tick();
    }

    public sealed class BTCondition : BTNode
    {
        private readonly Func<bool> condition;
        public BTCondition(Func<bool> condition) => this.condition = condition;
        public override BTStatus Tick() => condition() ? BTStatus.Success : BTStatus.Failure;
    }

    public sealed class BTAction : BTNode
    {
        private readonly Func<BTStatus> action;
        public BTAction(Func<BTStatus> action) => this.action = action;
        public override BTStatus Tick() => action();
    }

    public sealed class BTSequence : BTNode
    {
        private readonly IReadOnlyList<BTNode> children;
        public BTSequence(params BTNode[] children) => this.children = children;

        public override BTStatus Tick()
        {
            foreach (BTNode child in children)
            {
                BTStatus status = child.Tick();
                if (status != BTStatus.Success) return status;
            }

            return BTStatus.Success;
        }
    }

    public sealed class BTSelector : BTNode
    {
        private readonly IReadOnlyList<BTNode> children;
        public BTSelector(params BTNode[] children) => this.children = children;

        public override BTStatus Tick()
        {
            foreach (BTNode child in children)
            {
                BTStatus status = child.Tick();
                if (status != BTStatus.Failure) return status;
            }

            return BTStatus.Failure;
        }
    }
}
