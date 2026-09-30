# WOR Technical Index

Documentos:
- ARCHITECTURE.md — fluxo entre sistemas.
- DATA.md — ScriptableObjects e dados.
- CODEX.md — regras de manutenção/código.
- QUICKSTART.md — integração no Unity.
- INTEGRATION_CHECKLIST.md — gate do primeiro playable.

Fluxo principal:
Player Input / Enemy Brain
-> AbilitySystemComponent
-> GameplayAbilityDefinition
-> Combat / Movement Runtime
-> HitBox -> HurtBox
-> Target AbilitySystemComponent
-> Health / Poise / Tags / Reactions

Ordem:
1. Integrar e compilar.
2. Configurar Player placeholder.
3. Criar Prototype Data.
4. Criar Enemy_Test.
5. Validar câmera + movimento.
6. Validar Light Combo -> Damage -> Death.
7. Ajustar Dash, Block, Parry, Poise e Hit Reactions.
8. Trocar Visuals/Capsule pelo Ryu.
9. Integrar Animator.
10. Expandir Jutsus, Techniques, Equipment e targeting.
