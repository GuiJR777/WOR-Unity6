/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: HFSM compartilhável de locomotion e incapacidade que publica Gameplay Tags sem duplicar Abilities.
*/

using Ramirestech.Abilities;
using Ramirestech.Characters.Movement;
using Ramirestech.Core.HFSM;
using UnityEngine;

using HFSMStateMachine = Ramirestech.Core.HFSM.StateMachine;

namespace Ramirestech.Characters.StateMachine
{
    [RequireComponent(typeof(CharacterMotor))]
    [RequireComponent(typeof(AbilitySystemComponent))]
    public sealed class CharacterLocomotionHFSM : MonoBehaviour
    {
        #region Private Variables

        private CharacterMotor motor;
        private AbilitySystemComponent abilitySystem;
        private HFSMStateMachine stateMachine;

        private LocomotionState idle;
        private LocomotionState move;
        private LocomotionState jump;
        private LocomotionState fall;
        private LocomotionState stunned;
        private LocomotionState knockdown;
        private LocomotionState dead;

        #endregion

        #region Properties

        public string CurrentStateName =>
            stateMachine?.CurrentState?.Name ?? "None";

        #endregion

        #region Monobehaviour Methods

        private void Awake()
        {
            motor = GetComponent<CharacterMotor>();
            abilitySystem = GetComponent<AbilitySystemComponent>();

            stateMachine = new HFSMStateMachine();

            idle = new LocomotionState(
                "Grounded/Idle",
                abilitySystem,
                "Locomotion.Idle");

            move = new LocomotionState(
                "Grounded/Move",
                abilitySystem,
                "Locomotion.Move");

            jump = new LocomotionState(
                "Airborne/Jump",
                abilitySystem,
                "Locomotion.Jump");

            fall = new LocomotionState(
                "Airborne/Fall",
                abilitySystem,
                "Locomotion.Fall");

            stunned = new LocomotionState(
                "Disabled/Stunned",
                abilitySystem,
                "Locomotion.Disabled");

            knockdown = new LocomotionState(
                "Disabled/Knockdown",
                abilitySystem,
                "Locomotion.Disabled");

            dead = new LocomotionState(
                "Disabled/Dead",
                abilitySystem,
                "Locomotion.Disabled");
        }

        private void Start()
        {
            EvaluateState();
        }

        private void Update()
        {
            EvaluateState();
            stateMachine.Tick(Time.deltaTime);
        }

        #endregion

        #region Private Methods

        private void EvaluateState()
        {
            IState next;

            if (abilitySystem.HasTag("State.Dead"))
            {
                next = dead;
            }
            else if (abilitySystem.HasTag("State.KnockedDown"))
            {
                next = knockdown;
            }
            else if (abilitySystem.HasTag("State.Stunned"))
            {
                next = stunned;
            }
            else if (!motor.IsGrounded)
            {
                next = motor.VerticalVelocity > 0.05f
                    ? jump
                    : fall;
            }
            else
            {
                next = motor.IsMoving
                    ? move
                    : idle;
            }

            stateMachine.ChangeState(next);
        }

        #endregion

        #region Nested Classes

        private sealed class LocomotionState : HierarchicalState
        {
            #region Private Variables

            private readonly string stateName;
            private readonly AbilitySystemComponent abilitySystem;
            private readonly string tag;

            #endregion

            #region Properties

            public override string Name => stateName;

            #endregion

            #region Public Methods

            public LocomotionState(
                string name,
                AbilitySystemComponent system,
                string stateTag)
            {
                stateName = name;
                abilitySystem = system;
                tag = stateTag;
            }

            public override void Enter()
            {
                abilitySystem.AddTag(tag);
            }

            public override void Exit()
            {
                abilitySystem.RemoveTag(tag);
            }

            #endregion
        }

        #endregion
    }
}