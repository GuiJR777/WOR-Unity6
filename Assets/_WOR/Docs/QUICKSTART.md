# Quick Start

1. Copie _WOR para Assets/_WOR.
2. Aguarde o Unity compilar.
3. Requisitos: Unity 6, Input System, URP. Cinemachine recomendado.
4. Mantenha seu Player com Visuals/Capsule e GroundCheck.
5. Execute Tools > Whiskers of Rage > Quick Start > Create Prototype Data.
6. Selecione Player e execute Setup Selected Player.
7. Execute Create Test Enemy.

Controles Keyboard:
- WASD Move
- Space Jump
- J / Mouse Left Light Attack
- K / Mouse Right Heavy Attack
- Left Shift Dash
- Q Block
- E Parry

Gamepad:
- Left Stick Move
- South Jump
- West Light
- North Heavy
- East Dash
- LB Block
- RB Parry

A base inclui câmera 3/4 fallback. Depois substitua pelo rig Cinemachine final.

Ao integrar Ryu:
1. troque somente o conteúdo de Visuals;
2. adicione Animator;
3. ligue triggers;
4. substitua timers prototype por Animation Events;
5. mantenha HitBox.Begin/End como contrato.
