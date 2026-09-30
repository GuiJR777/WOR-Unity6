/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Behavior Tree prototype que decide aproximação, strafe e ataque usando o mesmo Ability System do Player.
*/
using Ramirestech.Abilities;
using Ramirestech.AI.BehaviorTree;
using Ramirestech.Characters.Movement;
using UnityEngine;

namespace Ramirestech.AI
{
    [RequireComponent(typeof(AbilitySystemComponent))]
    [RequireComponent(typeof(CharacterMotor))]
    public sealed class EnemyBrain : MonoBehaviour
    {
        #region Private Variables
        [Header("Dependencies")]
        [Tooltip("Alvo principal. Se vazio, procura GameObject com tag Player.")]
        [SerializeField] private Transform target;

        [Tooltip("Configuração da IA prototype.")]
        [SerializeField] private EnemyBrainConfig config;

        [Tooltip("Ability de ataque usada pelo inimigo prototype.")]
        [SerializeField] private AttackGameplayAbilityDefinition attackAbility;

        private AbilitySystemComponent abilitySystem;
        private CharacterMotor motor;
        private BTNode root;
        private float nextDecisionTime;
        private float strafeUntil;
        private float strafeSign = 1f;
        #endregion

        #region Monobehaviour Methods
        private void Awake()
        {
            abilitySystem = GetComponent<AbilitySystemComponent>();
            motor = GetComponent<CharacterMotor>();

            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) target = player.transform;
            }

            BuildTree();
        }

        private void Update()
        {
            if (Time.time < nextDecisionTime || root == null || abilitySystem.HasTag("State.Dead")) return;

            nextDecisionTime = Time.time + (config != null ? config.DecisionInterval : 0.2f);
            root.Tick();
        }
        #endregion

        #region Public Methods
        public void Configure(Transform newTarget, EnemyBrainConfig newConfig, AttackGameplayAbilityDefinition newAttack)
        {
            target = newTarget;
            config = newConfig;
            attackAbility = newAttack;
            BuildTree();
        }
        #endregion

        #region Private Methods
        private void BuildTree()
        {
            root = new BTSelector(
                new BTSequence(
                    new BTCondition(IsTargetInAttackRange),
                    new BTAction(CombatDecision)),
                new BTSequence(
                    new BTCondition(IsTargetDetected),
                    new BTAction(Chase)),
                new BTAction(Idle));
        }

        private bool IsTargetDetected() =>
            target != null &&
            config != null &&
            Vector3.Distance(transform.position, target.position) <= config.DetectionRange;

        private bool IsTargetInAttackRange() =>
            target != null &&
            config != null &&
            Vector3.Distance(transform.position, target.position) <= config.AttackRange;

        private BTStatus Chase()
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0f;

            if (direction.magnitude <= config.StopDistance)
                motor.SetWorldMoveDirection(Vector3.zero);
            else
                motor.SetWorldMoveDirection(direction.normalized);

            FaceTarget(direction);
            return BTStatus.Success;
        }

        private BTStatus CombatDecision()
        {
            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;
            FaceTarget(toTarget);

            if (Time.time < strafeUntil)
            {
                Vector3 tangent = Vector3.Cross(Vector3.up, toTarget.normalized) * strafeSign;
                motor.SetWorldMoveDirection(tangent);
                return BTStatus.Success;
            }

            if (Random.value < config.StrafeChance)
            {
                strafeSign = Random.value < 0.5f ? -1f : 1f;
                strafeUntil = Time.time + config.StrafeDuration;
                return BTStatus.Success;
            }

            motor.SetWorldMoveDirection(Vector3.zero);

            if (attackAbility != null)
            {
                AbilityContext context = new(
                    abilitySystem,
                    target.gameObject,
                    toTarget.sqrMagnitude > 0.001f ? toTarget.normalized : transform.forward,
                    transform.position);

                abilitySystem.TryActivate(attackAbility, context);
            }

            return BTStatus.Success;
        }

        private BTStatus Idle()
        {
            motor.SetWorldMoveDirection(Vector3.zero);
            return BTStatus.Success;
        }

        private void FaceTarget(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.001f) return;
            transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        }
        #endregion
    }
}
