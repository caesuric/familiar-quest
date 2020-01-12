using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Actions {
    public class ActLikeBoss : GoapAction {

        private bool started = false;
        private NavMeshAgent navMeshAgent = null;
        private MonsterAnimationController monsterAnimationController = null;
        private Boss boss = null;
        private List<ActiveAbility> bossAbilities = new List<ActiveAbility>();
        private List<float> healthPhaseThresholds = new List<float>();
        private readonly Element element;
        
        public ActLikeBoss() {
            preconditions = new Dictionary<string, object>() {
                { "seePlayer", true },
                { "playerAlive", true },
                { "gcdReady", true },
                { "facingPlayerPrecisely", true }
            };
            effects = new Dictionary<string, object>() {
                { "beingABoss", true },
                { "playerHurt", true }
            };
            element = Spirit.RandomElement();
            cost = 0.001f;
        }

        public override void Execute(GoapAgent agent) {
            if (boss == null) {
                boss = agent.GetComponent<Boss>();
                if (!boss.phasesTimeBased) SetupHealthThresholds();
            }
            if (monsterAnimationController==null) {
                monsterAnimationController = agent.GetComponent<MonsterAnimationController>();
            }
            if (!ActionPossible(agent)) {
                FailAction(agent);
                return;
            }
            if (!started) StartAction(agent);
            ContinueAction(agent);
        }

        private void SetupHealthThresholds() {
            healthPhaseThresholds.Clear();
            int slices = boss.phases.Count * boss.phaseCycles;
            var health = boss.GetComponent<Health>();
            float sliceSize = health.maxHP / slices;
            float amount = health.maxHP;
            while (amount > 0) {
                amount -= sliceSize;
                healthPhaseThresholds.Add(amount);
            }
        }

        private void FailAction(GoapAgent agent) {
            started = false;
            Fail(agent);
            monsterAnimationController.moving = false;
        }

        private void StartAction(GoapAgent agent) {
            var phase = DeterminePhase();
            if (phase != boss.phases[boss.currentPhase]) ChangePhases(phase);
            SelectTarget(agent);
            if (target == null) return;
            started = true;
            isDone = false;
            if (navMeshAgent == null) navMeshAgent = agent.GetComponent<NavMeshAgent>();
            UseAbilityBasedOnMechanics(agent);
        }

        private BossPhase DeterminePhase() {
            if (boss.phasesTimeBased) return DetermineTimeBasedPhase();
            else return DetermineDamageBasedPhase();
        }

        private BossPhase DetermineTimeBasedPhase() {
            int phase = boss.currentPhase;
            if (boss.fightTime >= boss.phaseTime) {
                boss.fightTime = 0;
                phase += 1;
                if (phase >= boss.phases.Count) phase = 0;
            }
            return boss.phases[phase];
        }

        private BossPhase DetermineDamageBasedPhase() {
            int phaseNumber = 0;
            int index = 0;
            var health = boss.GetComponent<Health>().hp;
            while (healthPhaseThresholds[index] > health) {
                phaseNumber++;
                if (phaseNumber >= boss.phases.Count) phaseNumber = 0;
                index++;
            }
            return boss.phases[phaseNumber];
        }

        private void ChangePhases(BossPhase phase) {
            boss.currentPhase = boss.phases.IndexOf(phase);
            bossAbilities.Clear();
            SetupPhase(phase);
        }

        private void SetupPhase(BossPhase phase) {
            foreach (var mechanic in phase.mechanics) AddAbilitiesForMechanic(mechanic);
        }

        private void AddAbilitiesForMechanic(BossMechanic mechanic) {
            var abilities = bossAbilities;
            var baseStat = GetBestStat();
            var proj = GetProjectile(element);
            var hitEffect = GetHitEffect(element);
            var aoe = GetAoeEffect(element);
            Debug.Log("switching to mechanic: " + mechanic.type);
            switch (mechanic.type) {
                case "damageZones":
                    if (mechanic.options.Contains("heals")) abilities.Add(new AttackAbility("Healing Damage Zone", "Damage zone that heals the caster.", 0f, element, baseStat, dotDamage: 4f, dotTime: 10, isRanged: true, cooldown: 10, radius: 4, aoe: aoe, attributes: new AbilityAttribute("bossHealingDamageZone")));
                    else abilities.Add(new AttackAbility("Damage Zone", "Damage zone.", 0f, element, baseStat, dotDamage: 4f, dotTime: 10, isRanged: true, cooldown: 10, radius: 4, aoe: aoe, attributes: new AbilityAttribute("bossDamageZone")));
                    break;
                case "circleAoe":
                    abilities.Add(new AttackAbility("Circle AOE", "Circular AOE that targets player.", 4f, element, baseStat, isRanged: true, cooldown: 5, radius: 4, aoe: aoe, attributes: new AbilityAttribute("bossCircleAoe")));
                    break;
                case "lineAoe":
                    abilities.Add(new AttackAbility("Line AOE", "Line AOE that targets player.", 4f, element, baseStat, isRanged: true, cooldown: 5, aoe: aoe, attributes: new AbilityAttribute("bossLineAoe")));
                    break;
                case "rage":
                    abilities.Add(new UtilityAbility("Rage", "Become enraged.", cooldown: 30, attributes: new AbilityAttribute("bossRage")));
                    break;
                case "bulletHell":
                    abilities.Add(new AttackAbility("Bullet Hell", "Creates a living hell of projectiles.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("bossBulletHell")));
                    break;
                case "homingProjectiles":
                    abilities.Add(new AttackAbility("Homing Projectile", "Fires a homing projectile.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("bossHomingProjectile")));
                    break;
                case "projectileSpreads":
                    abilities.Add(new AttackAbility("Projectile Spread", "Fires a projectile spread.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("projectileSpread")));
                    break;
                case "jumpAndShoot":
                    abilities.Add(new AttackAbility("Jump and fire", "Jumps and fires.", 2.1f, element, baseStat, cooldown: 1.5f, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect, attributes: new AbilityAttribute("bossJumpAndShoot")));
                    abilities.Add(new AttackAbility("Fire", "Ranged attack.", 2f, element, baseStat, isRanged: true, rangedProjectile: proj, hitEffect: hitEffect));
                    break;
                case "charges":
                    abilities.Add(new AttackAbility("Charge", "Charges the enemy.", 4f, element, baseStat, cooldown: 5, hitEffect: hitEffect, attributes: new AbilityAttribute("chargeTowards")));
                    break;
                case "teleports":
                    abilities.Add(new UtilityAbility("Teleport", "Teleport somewhere useful.", cooldown: 5, attributes: new AbilityAttribute("bossTeleport")));
                    break;
                case "eatMinions":
                    abilities.Add(new UtilityAbility("Eat Minion", "Eats a minion.", cooldown: 30, attributes: new AbilityAttribute("bossEatMinion")));
                    break;
                case "spawnAdds":
                    abilities.Add(new UtilityAbility("Summon", "Summons minions.", cooldown: 30, attributes: new AbilityAttribute("bossSummonMinions")));
                    break;
                default:
                    break;
            }
        }

        private BaseStat GetBestStat() {
            var c = boss.GetComponent<Character>();
            if (c.strength > c.dexterity && c.strength > c.intelligence) return BaseStat.strength;
            else if (c.dexterity > c.intelligence) return BaseStat.dexterity;
            return BaseStat.intelligence;
        }

        private int GetProjectile(Element element) {
            var dict = new Dictionary<Element, int> { { Element.bashing, 3 }, { Element.piercing, 0 }, { Element.slashing, 4 }, { Element.fire, 1 }, { Element.ice, 5 }, { Element.acid, 2 }, { Element.light, 6 }, { Element.dark, 7 }, { Element.none, 8 } };
            return dict[element];
        }

        private int GetHitEffect(Element element) {
            var dict = new Dictionary<Element, int> { { Element.bashing, 1 }, { Element.piercing, 2 }, { Element.slashing, 0 }, { Element.fire, 3 }, { Element.ice, 4 }, { Element.acid, 5 }, { Element.light, 6 }, { Element.dark, 7 }, { Element.none, 8 } };
            return dict[element];
        }

        private int GetAoeEffect(Element element) {
            var dict = new Dictionary<Element, int> { { Element.bashing, 2 }, { Element.piercing, 1 }, { Element.slashing, 0 }, { Element.fire, 3 }, { Element.ice, 4 }, { Element.acid, 5 }, { Element.light, 6 }, { Element.dark, 7 }, { Element.none, 8 } };
            return dict[element];
        }

        private void ContinueAction(GoapAgent agent) {
            var phase = DeterminePhase();
            if (phase != boss.phases[boss.currentPhase]) ChangePhases(phase);
            UseAbilityBasedOnMechanics(agent);
        }

        private void SelectTarget(GoapAgent agent) {
            var fov = agent.memory["fieldOfVision"] as Data.FieldOfVision;
            if (fov.players.Count > 0) target = fov.players[0];
            else target = null;
        }

        private void SetDestination() {
            if (RightConditionsToFight()) navMeshAgent.SetDestination(target.transform.position);
            else navMeshAgent.SetDestination(boss.originalLocation);
        }

        private bool RightConditionsToFight() {
            if (boss.PlayersInRoom() && Vector3.Distance(boss.transform.position, boss.originalLocation) < 25f) return true;
            else {
                var health = boss.GetComponent<Health>();
                health.hp = health.maxHP;
                return false;
            }
        }

        private void UseAbilityBasedOnMechanics(GoapAgent agent) {
            var ability = FindBestAbility(agent);
            if (ability != null && boss.GetComponent<AbilityUser>().GCDTime == 0) agent.GetComponent<AbilityUser>().UseAbility(ability);
            else {
                if (navMeshAgent == null) navMeshAgent = agent.GetComponent<NavMeshAgent>();
                SetDestination();
            }
        }

        private ActiveAbility FindBestAbility(GoapAgent agent) {
            var ability = FindBestAbilityIn(bossAbilities, agent);
            if (ability != null) return ability;
            return FindBestAbilityIn(boss.GetComponent<MonsterBaseAbilities>().baseAbilities, agent);
        }

        private ActiveAbility FindBestAbilityIn(List<ActiveAbility> abilities, GoapAgent agent) {
            ActiveAbility bestAbility = null;
            float bestAbilityValue = 0f;
            foreach (var ability in abilities) {
                foreach (var attribute in ability.attributes) Debug.Log(attribute.type);
                if (ability.currentCooldown > 0) continue;
                if (ability is AttackAbility attackAbility && attackAbility.damage>bestAbilityValue) {
                    if (!attackAbility.isRanged && agent.state["inMeleeRangeOfPlayer"].Equals(true) && attackAbility.damage > bestAbilityValue) {
                        bestAbilityValue = attackAbility.damage;
                        bestAbility = ability;
                    }
                    else if (attackAbility.isRanged && agent.state["facingPlayerPrecisely"].Equals(true)) {
                        bestAbilityValue = attackAbility.damage;
                        bestAbility = ability;
                    }
                }
                else if (ability is UtilityAbility utilityAbility) {
                    bestAbilityValue = 10000f;
                    bestAbility = ability;
                }
            }
            return bestAbility;
        }
    }
}
