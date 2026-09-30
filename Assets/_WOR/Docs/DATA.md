# Data Model

AttackDefinition configura sem código:
- Attack Scale e Flat Damage;
- Poise Damage;
- Back Attack Multiplier;
- Stun;
- Knockback;
- Launch;
- Knockdown;
- HitBox offset/size;
- windup/active/recovery prototype;
- Animator Trigger.

AttackGameplayAbilityDefinition referencia um AttackDefinition e executa o CombatController.
ComboDefinition contém a lista ordenada de Attack Abilities.
DashGameplayAbilityDefinition configura distância, duração e invulnerabilidade prototype.
TimedTagAbilityDefinition serve para janela de Parry e outros estados temporários.
EnemyBrainConfig configura detection, attack range, spacing, strafe e decision interval.

Create Prototype Data cria assets em Assets/_WOR/ScriptableObjects/Prototype.
Esses números são de playtest, não balanceamento final.
