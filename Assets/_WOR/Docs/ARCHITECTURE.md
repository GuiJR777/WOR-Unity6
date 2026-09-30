# Architecture

Princípios:
- Composition acima de heranças rígidas.
- Runtime em MonoBehaviour; autoria/configuração em ScriptableObject.
- Player e Enemy compartilham Movement, Combat, Attributes, Health, Hit Reactions e Ability System.
- Input humano e AI são apenas fontes diferentes de intenção.
- HFSM cuida de locomotion/incapacitação; ataques e dash pertencem às Abilities.
- HitBox/HurtBox não alteram Health diretamente.
- Sistemas genéricos não conhecem Ryu.

Character Root:
CharacterController
AttributeSet
HealthComponent
AbilitySystemComponent
CharacterMotor
CharacterFacingController
CharacterLocomotionHFSM
CombatController
HitReactionController
Visuals

Player adiciona PlayerInputReader, PlayerMovementController, ComboController e PlayerCombatInput.
Enemy adiciona EnemyBrain.

HFSM:
Root
- Grounded
  - Idle
  - Move
- Airborne
  - Jump
  - Fall
- Disabled
  - Stunned
  - Knockdown
  - Dead

Ataque, Dash, Block, Parry e Jutsus não são estados locomotores.

GAS-inspired mínimo:
AbilitySystemComponent
- AttributeSet
- GameplayTagContainer
- Cooldowns
- Ability activation
- Damage / Gameplay Events

Combat Flow:
TryActivate(Attack Ability)
-> CombatController.ExecuteAttack()
-> windup prototype
-> HitBox.Begin()
-> Physics.OverlapBox
-> HurtBox.ReceiveHit()
-> Target ASC.ReceiveDamage()
-> Defense
-> Health
-> HitReactionController
-> Poise / Stun / Knockback / Knockdown / Death

Enquanto não houver animações finais, CombatController usa timers prototype. Depois, mantenha o mesmo contrato e controle a HitBox por Animation Events/StateMachineBehaviours.

Combo:
ComboDefinition guarda a sequência. ComboController só avança após AttackHitConfirmed. Whiff reseta.

AI:
A Behavior Tree decide intenção e chama o mesmo AbilitySystemComponent. Não criar EnemyAttack/EnemyDamage paralelos.
