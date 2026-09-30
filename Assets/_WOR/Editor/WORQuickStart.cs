/*
RamiresTech Games

Whiskers of Rage

Autor: Guilherme Jesuino Ramires
Data: 29/09/2026

Descrição: Ferramentas de Editor para montar rapidamente Player, dados prototype e Enemy_Test.
*/
#if UNITY_EDITOR
using System.IO;
using Ramirestech.Abilities;
using Ramirestech.AI;
using Ramirestech.CameraSystem;
using Ramirestech.Characters.Movement;
using Ramirestech.Characters.StateMachine;
using Ramirestech.Combat;
using Ramirestech.Core.Input;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ramirestech.EditorTools
{
    public static class WORQuickStart
    {
        #region Private Variables
        private const string PrototypePath = "Assets/_WOR/ScriptableObjects/Prototype";
        #endregion

        #region Public Methods
        [MenuItem("Tools/Whiskers of Rage/Quick Start/Create Prototype Data")]
        public static void CreatePrototypeData()
        {
            EnsureFolder("Assets/_WOR/ScriptableObjects");
            EnsureFolder(PrototypePath);

            AttackDefinition light1 = GetOrCreate<AttackDefinition>($"{PrototypePath}/Attack_Light_01.asset");
            AttackDefinition light2 = GetOrCreate<AttackDefinition>($"{PrototypePath}/Attack_Light_02.asset");
            AttackDefinition light3 = GetOrCreate<AttackDefinition>($"{PrototypePath}/Attack_Light_03.asset");
            AttackDefinition heavy = GetOrCreate<AttackDefinition>($"{PrototypePath}/Attack_Heavy.asset");
            AttackDefinition enemyAttack = GetOrCreate<AttackDefinition>($"{PrototypePath}/Attack_Enemy.asset");

            AttackGameplayAbilityDefinition ga1 = GetOrCreate<AttackGameplayAbilityDefinition>($"{PrototypePath}/GA_Light_01.asset");
            AttackGameplayAbilityDefinition ga2 = GetOrCreate<AttackGameplayAbilityDefinition>($"{PrototypePath}/GA_Light_02.asset");
            AttackGameplayAbilityDefinition ga3 = GetOrCreate<AttackGameplayAbilityDefinition>($"{PrototypePath}/GA_Light_03.asset");
            AttackGameplayAbilityDefinition gaHeavy = GetOrCreate<AttackGameplayAbilityDefinition>($"{PrototypePath}/GA_Heavy.asset");
            AttackGameplayAbilityDefinition gaEnemy = GetOrCreate<AttackGameplayAbilityDefinition>($"{PrototypePath}/GA_EnemyAttack.asset");

            GetOrCreate<DashGameplayAbilityDefinition>($"{PrototypePath}/GA_Dash.asset");
            GetOrCreate<TimedTagAbilityDefinition>($"{PrototypePath}/GA_Parry.asset");
            ComboDefinition combo = GetOrCreate<ComboDefinition>($"{PrototypePath}/Combo_Light.asset");
            GetOrCreate<EnemyBrainConfig>($"{PrototypePath}/AI_Enemy_Test.asset");

            SetObjectReference(ga1, "attack", light1);
            SetObjectReference(ga2, "attack", light2);
            SetObjectReference(ga3, "attack", light3);
            SetObjectReference(gaHeavy, "attack", heavy);
            SetObjectReference(gaEnemy, "attack", enemyAttack);
            SetArrayReferences(combo, "attacks", new Object[] { ga1, ga2, ga3 });

            TuneAttack(light1, 0.9f, 8f, 1.5f, false, false, 0.06f, 0.10f, 0.16f);
            TuneAttack(light2, 1.0f, 10f, 2.0f, true, false, 0.07f, 0.11f, 0.18f);
            TuneAttack(light3, 1.35f, 22f, 5.5f, true, true, 0.10f, 0.14f, 0.28f);
            TuneAttack(heavy, 1.65f, 30f, 7.0f, true, true, 0.22f, 0.16f, 0.40f);
            TuneAttack(enemyAttack, 0.8f, 12f, 2.5f, true, false, 0.22f, 0.12f, 0.35f);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("WOR: Prototype Data criado/atualizado.");
        }

        [MenuItem("Tools/Whiskers of Rage/Quick Start/Setup Selected Player")]
        public static void SetupSelectedPlayer()
        {
            GameObject player = Selection.activeGameObject;

            if (player == null)
            {
                Debug.LogError("WOR: selecione o GameObject Player.");
                return;
            }

            player.tag = "Player";

            CharacterController characterController = EnsureComponent<CharacterController>(player);
            characterController.center = new Vector3(0f, 1f, 0f);
            characterController.height = 2f;
            characterController.radius = 0.5f;

            EnsureComponent<AttributeSet>(player);
            EnsureComponent<HealthComponent>(player);
            EnsureComponent<AbilitySystemComponent>(player);
            EnsureComponent<CharacterMotor>(player);
            EnsureComponent<CharacterFacingController>(player);
            PlayerInputReader inputReader = EnsureComponent<PlayerInputReader>(player);
            EnsureComponent<PlayerMovementController>(player);
            EnsureComponent<CharacterLocomotionHFSM>(player);
            EnsureComponent<CombatController>(player);
            ComboController comboController = EnsureComponent<ComboController>(player);
            PlayerCombatInput combatInput = EnsureComponent<PlayerCombatInput>(player);
            EnsureComponent<HitReactionController>(player);

            EnsureHurtBox(player);
            EnsureHitBox(player);
            LinkInputAsset(inputReader);
            LinkPrototypePlayerData(comboController, combatInput);
            SetupFallbackCamera(player.transform);

            EditorUtility.SetDirty(player);
            Debug.Log("WOR: Player configurado. Pressione Play para validar movimento e combate.");
        }

        [MenuItem("Tools/Whiskers of Rage/Quick Start/Create Test Enemy")]
        public static void CreateTestEnemy()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                Debug.LogError("WOR: configure o Player primeiro.");
                return;
            }

            GameObject enemy = GameObject.Find("Enemy_Test");

            if (enemy == null)
            {
                enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                enemy.name = "Enemy_Test";
            }

            enemy.transform.position = player.transform.position + new Vector3(0f, 1f, 5f);

            CapsuleCollider primitiveCollider = enemy.GetComponent<CapsuleCollider>();
            if (primitiveCollider != null)
                Object.DestroyImmediate(primitiveCollider);

            CharacterController characterController = EnsureComponent<CharacterController>(enemy);
            characterController.center = Vector3.zero;
            characterController.height = 2f;
            characterController.radius = 0.5f;

            EnsureComponent<AttributeSet>(enemy);
            EnsureComponent<HealthComponent>(enemy);
            EnsureComponent<AbilitySystemComponent>(enemy);
            EnsureComponent<CharacterMotor>(enemy);
            EnsureComponent<CharacterLocomotionHFSM>(enemy);
            EnsureComponent<CombatController>(enemy);
            EnsureComponent<HitReactionController>(enemy);
            EnemyBrain brain = EnsureComponent<EnemyBrain>(enemy);

            EnsureHurtBox(enemy);
            EnsureHitBox(enemy);

            EnemyBrainConfig config =
                AssetDatabase.LoadAssetAtPath<EnemyBrainConfig>($"{PrototypePath}/AI_Enemy_Test.asset");

            AttackGameplayAbilityDefinition attack =
                AssetDatabase.LoadAssetAtPath<AttackGameplayAbilityDefinition>($"{PrototypePath}/GA_EnemyAttack.asset");

            brain.Configure(player.transform, config, attack);

            Selection.activeGameObject = enemy;
            EditorUtility.SetDirty(enemy);
            Debug.Log("WOR: Enemy_Test criado.");
        }

        [MenuItem("Tools/Whiskers of Rage/Quick Start/Build Playable Sandbox")]
        public static void BuildPlayableSandbox()
        {
            CreatePrototypeData();

            GameObject player = GameObject.Find("Player");

            if (player == null)
            {
                Debug.LogError("WOR: crie/renomeie seu root para Player primeiro.");
                return;
            }

            Selection.activeGameObject = player;
            SetupSelectedPlayer();
            CreateTestEnemy();
            Debug.Log("WOR: sandbox montado. Ajuste posições se necessário e pressione Play.");
        }
        #endregion

        #region Private Methods
        private static void EnsureHurtBox(GameObject owner)
        {
            Transform combat = EnsureChild(owner.transform, "Combat");
            Transform existing = combat.Find("BodyHurtBox");

            if (existing != null) return;

            GameObject go = new("BodyHurtBox");
            go.transform.SetParent(combat, false);
            go.transform.localPosition = owner.name == "Enemy_Test"
                ? Vector3.zero
                : new Vector3(0f, 1f, 0f);

            CapsuleCollider collider = go.AddComponent<CapsuleCollider>();
            collider.isTrigger = true;
            collider.height = 2f;
            collider.radius = 0.5f;
            go.AddComponent<HurtBox>();
        }

        private static void EnsureHitBox(GameObject owner)
        {
            Transform visuals = owner.transform.Find("Visuals");
            Transform hitBoxParent = visuals != null ? visuals : owner.transform;
            Transform hitBoxRoot = EnsureChild(hitBoxParent, "HitBoxRoot");

            if (hitBoxRoot.GetComponent<HitBox>() == null)
                hitBoxRoot.gameObject.AddComponent<HitBox>();
        }

        private static void LinkInputAsset(PlayerInputReader reader)
        {
            InputActionAsset asset =
                AssetDatabase.LoadAssetAtPath<InputActionAsset>(
                    "Assets/_WOR/Input/WOR_InputActions.inputactions");

            if (asset != null)
                reader.SetInputActions(asset);

            EditorUtility.SetDirty(reader);
        }

        private static void LinkPrototypePlayerData(
            ComboController combo,
            PlayerCombatInput input)
        {
            ComboDefinition comboData =
                AssetDatabase.LoadAssetAtPath<ComboDefinition>(
                    $"{PrototypePath}/Combo_Light.asset");

            AttackGameplayAbilityDefinition heavy =
                AssetDatabase.LoadAssetAtPath<AttackGameplayAbilityDefinition>(
                    $"{PrototypePath}/GA_Heavy.asset");

            DashGameplayAbilityDefinition dash =
                AssetDatabase.LoadAssetAtPath<DashGameplayAbilityDefinition>(
                    $"{PrototypePath}/GA_Dash.asset");

            TimedTagAbilityDefinition parry =
                AssetDatabase.LoadAssetAtPath<TimedTagAbilityDefinition>(
                    $"{PrototypePath}/GA_Parry.asset");

            combo.SetLightCombo(comboData);
            input.Configure(heavy, dash, parry);

            EditorUtility.SetDirty(combo);
            EditorUtility.SetDirty(input);
        }

        private static void SetupFallbackCamera(Transform player)
        {
            Camera camera = Camera.main;
            if (camera == null) return;

            IsometricCameraRig rig =
                camera.GetComponent<IsometricCameraRig>() ??
                camera.gameObject.AddComponent<IsometricCameraRig>();

            rig.SetTarget(player);
            EditorUtility.SetDirty(rig);
        }

        private static Transform EnsureChild(Transform parent, string name)
        {
            Transform child = parent.Find(name);
            if (child != null) return child;

            GameObject go = new(name);
            go.transform.SetParent(parent, false);
            return go.transform;
        }

        private static T EnsureComponent<T>(GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            return component != null ? component : go.AddComponent<T>();
        }

        private static T GetOrCreate<T>(string path) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (asset != null)
                return asset;

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            string parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
            string name = Path.GetFileName(path);

            if (!string.IsNullOrEmpty(parent))
                EnsureFolder(parent);

            AssetDatabase.CreateFolder(parent, name);
        }

        private static void SetObjectReference(
            Object target,
            string propertyName,
            Object value)
        {
            SerializedObject serializedObject = new(target);
            SerializedProperty property =
                serializedObject.FindProperty(propertyName);

            if (property != null)
                property.objectReferenceValue = value;

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }

        private static void SetArrayReferences(
            Object target,
            string propertyName,
            Object[] values)
        {
            SerializedObject serializedObject = new(target);
            SerializedProperty property =
                serializedObject.FindProperty(propertyName);

            if (property == null)
                return;

            property.arraySize = values.Length;

            for (int i = 0; i < values.Length; i++)
                property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }

        private static void TuneAttack(
            AttackDefinition attack,
            float scale,
            float poise,
            float knockback,
            bool stun,
            bool knockdown,
            float windup,
            float active,
            float recovery)
        {
            SerializedObject serializedObject = new(attack);

            serializedObject.FindProperty("attackScale").floatValue = scale;
            serializedObject.FindProperty("poiseDamage").floatValue = poise;
            serializedObject.FindProperty("knockback").floatValue = knockback;
            serializedObject.FindProperty("causesStun").boolValue = stun;
            serializedObject.FindProperty("causesKnockdown").boolValue = knockdown;
            serializedObject.FindProperty("windup").floatValue = windup;
            serializedObject.FindProperty("activeTime").floatValue = active;
            serializedObject.FindProperty("recovery").floatValue = recovery;

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(attack);
        }
        #endregion
    }
}
#endif
