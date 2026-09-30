/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: HUD temporário de debug para validar ataques, hit-confirm e dano durante o protótipo.
*/

using Ramirestech.Abilities;
using UnityEngine;

namespace Ramirestech.Combat
{
    [RequireComponent(typeof(CombatController))]
    [RequireComponent(typeof(HealthComponent))]
    public sealed class PrototypeCombatDebugger : MonoBehaviour
    {
        #region Private Variables

        [Header("Debug")]
        [Tooltip("Exibe informações temporárias de combate na tela.")]
        [SerializeField] private bool showHud = true;

        [Tooltip("Nome do inimigo prototype procurado automaticamente na cena.")]
        [SerializeField] private string targetObjectName = "Enemy_Test";

        private CombatController combatController;
        private HealthComponent playerHealth;
        private HealthComponent targetHealth;

        private string currentAttack = "None";
        private string lastResult = "Waiting...";
        private float resultUntil;

        #endregion

        #region Monobehaviour Methods

        private void Awake()
        {
            combatController = GetComponent<CombatController>();
            playerHealth = GetComponent<HealthComponent>();
        }

        private void Start()
        {
            FindTarget();
        }

        private void OnEnable()
        {
            if (combatController == null)
            {
                combatController = GetComponent<CombatController>();
            }

            combatController.AttackStarted += OnAttackStarted;
            combatController.AttackHitConfirmed += OnAttackHitConfirmed;
            combatController.AttackFinished += OnAttackFinished;
        }

        private void OnDisable()
        {
            if (combatController == null)
            {
                return;
            }

            combatController.AttackStarted -= OnAttackStarted;
            combatController.AttackHitConfirmed -= OnAttackHitConfirmed;
            combatController.AttackFinished -= OnAttackFinished;
        }

        private void OnGUI()
        {
            if (!showHud)
            {
                return;
            }

            if (targetHealth == null)
            {
                FindTarget();
            }

            GUILayout.BeginArea(new Rect(20f, 20f, 360f, 230f));

            GUILayout.Label("<b>WOR — COMBAT DEBUG</b>");

            GUILayout.Space(5f);

            if (playerHealth != null)
            {
                GUILayout.Label(
                    $"Player HP: {playerHealth.CurrentHealth:0.0} / {playerHealth.MaxHealth:0.0}");
            }

            if (targetHealth != null)
            {
                GUILayout.Label(
                    $"Enemy HP: {targetHealth.CurrentHealth:0.0} / {targetHealth.MaxHealth:0.0}");
            }
            else
            {
                GUILayout.Label("Enemy HP: target not found");
            }

            GUILayout.Space(8f);

            GUILayout.Label($"Attack: {currentAttack}");

            if (Time.time <= resultUntil)
            {
                GUILayout.Label($"Result: <b>{lastResult}</b>");
            }
            else
            {
                GUILayout.Label("Result: -");
            }

            GUILayout.Space(10f);

            GUILayout.Label("J / Mouse 1 = Light Attack");
            GUILayout.Label("K / Mouse 2 = Heavy Attack");
            GUILayout.Label("Shift = Dash");
            GUILayout.Label("Q = Block");
            GUILayout.Label("E = Parry");

            GUILayout.EndArea();
        }

        #endregion

        #region Private Methods

        private void FindTarget()
        {
            GameObject target = GameObject.Find(targetObjectName);

            if (target != null)
            {
                targetHealth = target.GetComponent<HealthComponent>();
            }
        }

        private void OnAttackStarted(AttackDefinition attack)
        {
            currentAttack = attack != null
                ? attack.name
                : "Unknown";

            lastResult = "ATTACK";
            resultUntil = Time.time + 1f;
        }

        private void OnAttackHitConfirmed(
            AttackDefinition attack,
            HurtBox hurtBox,
            DamageSpec damageSpec)
        {
            lastResult =
                $"HIT! {damageSpec.Damage:0.0} DAMAGE";

            resultUntil = Time.time + 1.25f;
        }

        private void OnAttackFinished(
            AttackDefinition attack,
            bool hitConfirmed)
        {
            if (!hitConfirmed)
            {
                lastResult = "WHIFF";
                resultUntil = Time.time + 1f;
            }

            currentAttack = "None";
        }

        #endregion
    }
}