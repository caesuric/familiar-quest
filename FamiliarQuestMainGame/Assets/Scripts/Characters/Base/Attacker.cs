using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(StatusEffectHost))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(AudioGenerator))]
[RequireComponent(typeof(SimulatedNoiseGenerator))]
[RequireComponent(typeof(CacheGrabber))]
[RequireComponent(typeof(AbilityUser))]
public class Attacker : MonoBehaviour {
    public bool isAttacking = false;
    //public float critRate;
    //public float critMultiplier;
    public AttackAbility meleeAttackAbility = null;
    public float meleeAttackDamage = 0;
    public GameObject invisibleHealingZonePrefab;
    public GameObject bossCircleAoePrefab;
    public GameObject bossLineAoePrefab;
    public GameObject homingProjectilePrefab;

    private void Start() {
        invisibleHealingZonePrefab = (GameObject)Resources.Load("Prefabs/DamageZones/InvisibleHealing");
        bossCircleAoePrefab = (GameObject)Resources.Load("Prefabs/AOEs/BossCircle");
        bossLineAoePrefab = (GameObject)Resources.Load("Prefabs/AOEs/BossLine");
        homingProjectilePrefab = (GameObject)Resources.Load("Prefabs/Projectiles/Homing");
    }

    private void Update() {
        if (GetComponent<StatusEffectHost>().CheckForEffect("paralysis")) isAttacking = false;
    }

    public void Calculate() {
        int level = 1;
        if (GetComponent<ExperienceGainer>() != null) level = GetComponent<ExperienceGainer>().level;
        else level = GetComponent<MonsterScaler>().level;
        //critRate = SecondaryStatUtility.CalcCriticalHitRate(GetComponent<Character>().luck, level);
        //critMultiplier = SecondaryStatUtility.CalcCriticalDamage(GetComponent<Character>().luck, level);
    }

    public IEnumerator ActivateMeleeHitbox() {
        if (GetComponent<StatusEffectHost>().CheckForEffect("paralysis")) yield break;
        MeleeHitboxOn();
        yield return new WaitForSeconds(AbilityUser.maxGCDTime / 2);
        MeleeHitboxOff();
    }

    public void MeleeHitboxOn() {
        if (meleeAttackAbility == null || meleeAttackAbility.FindAttribute("stealthy") == null) GetComponent<StatusEffectHost>().RemoveStealth();
        isAttacking = true;
        GetComponent<AnimationController>().SetAttacking(true);
        if (meleeAttackAbility == null || meleeAttackAbility.FindAttribute("offGCD") == null) GetComponent<AbilityUser>().GCDTime = AbilityUser.maxGCDTime;
        GetComponent<AudioGenerator>().PlaySoundByName("sfx_melee_swipe3");
    }

    public void MeleeHitboxOff() {
        isAttacking = false;
        GetComponent<AnimationController>().SetAttacking(false);
        if (meleeAttackAbility != null) {
            meleeAttackAbility = null;
            GetComponent<Character>().CalculateAll();
        }
    }

    public void RangedWeaponAttack() {
        if (GetComponent<AbilityUser>().GCDTime > 0) return;
        GetComponent<AbilityUser>().GCDTime = AbilityUser.maxGCDTime;
        int projectile = 0;
        if (GetComponent<PlayerCharacter>() != null) projectile = ((RangedWeapon)(GetComponent<PlayerCharacter>().weapon)).projectileModel;
        var obj = Instantiate(GetComponent<CacheGrabber>().projectiles[projectile], new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
        //NetworkServer.Spawn(obj);
        var rhdd = obj.GetComponentInChildren<RangedHitboxDealDamage>();
        rhdd.character = GetComponent<Character>();
        rhdd.faction = GetComponent<Character>().faction;
        if (((RangedWeapon)(GetComponent<PlayerCharacter>().weapon)).usesInt) {
            rhdd.damage = (int)(CharacterAttribute.attributes["intelligence"].instances[GetComponent<Character>()].TotalValue * GetComponent<PlayerCharacter>().weapon.attackPower);
            //rhdd.damage = (int)(GetComponent<Character>().intelligence * GetComponent<PlayerCharacter>().weapon.attackPower);
            GetComponent<AudioGenerator>().PlaySoundByName("sfx_spell_project");
        }
        else {
            //rhdd.damage = (int)(GetComponent<Character>().dexterity * GetComponent<PlayerCharacter>().weapon.attackPower);
            rhdd.damage = (int)(CharacterAttribute.attributes["dexterity"].instances[GetComponent<Character>()].TotalValue * GetComponent<PlayerCharacter>().weapon.attackPower);
            GetComponent<AudioGenerator>().PlaySoundByName("sfx_bow_release");
        }
    }

    public float GetBaseDamage(BaseStat stat) {
        float damage = GetStatDamage(stat);
        float baseDamage = 0;
        if (GetComponent<PlayerCharacter>() != null) baseDamage = GetComponent<PlayerCharacter>().weapon.attackPower;
        else baseDamage = GetComponent<Monster>().attackFactor;
        return baseDamage * damage;
    }

    public float GetStatDamage(BaseStat stat) {
        switch (stat) {
            case BaseStat.strength:
            default:
                return CharacterAttribute.attributes["strength"].instances[GetComponent<Character>()].TotalValue;
                //return GetComponent<Character>().strength;
            case BaseStat.dexterity:
                return CharacterAttribute.attributes["dexterity"].instances[GetComponent<Character>()].TotalValue;
            //return GetComponent<Character>().dexterity;
            case BaseStat.intelligence:
                return CharacterAttribute.attributes["intelligence"].instances[GetComponent<Character>()].TotalValue;
                //return GetComponent<Character>().intelligence;
        }
    }

    public void HitAllInRadius(int damage, float radius, AttackAbility ability = null) {
        var objects = GameObject.FindGameObjectsWithTag(GetComponent<Character>().oppositeFaction);
        foreach (var obj in objects) if (Vector3.Distance(transform.position, obj.transform.position) <= radius && obj.GetComponent<Health>() != null) obj.GetComponent<Health>().TakeDamage(damage, Element.slashing, GetComponent<Character>(), ability: ability);
    }

    private float ApplyDamageBoosts(float damage, AttackAbility ability) {
        foreach (var effect in GetComponent<StatusEffectHost>().statusEffects) {
            if (effect.type == "bashingDamageBuff" && ability.element == Element.bashing) damage *= (1 + (effect.degree / 100));
            else if (effect.type == "slashingDamageBuff" && ability.element == Element.slashing) damage *= (1 + (effect.degree / 100));
            else if (effect.type == "piercingDamageBuff" && ability.element == Element.piercing) damage *= (1 + (effect.degree / 100));
            else if (effect.type == "acidDamageBuff" && ability.element == Element.acid) damage *= (1 + (effect.degree / 100));
            else if (effect.type == "fireDamageBuff" && ability.element == Element.fire) damage *= (1 + (effect.degree / 100));
            else if (effect.type == "iceDamageBuff" && ability.element == Element.ice) damage *= (1 + (effect.degree / 100));
            else if (effect.type == "lightDamageBuff" && ability.element == Element.light) damage *= (1 + (effect.degree / 100));
            else if (effect.type == "darkDamageBuff" && ability.element == Element.dark) damage *= (1 + (effect.degree / 100));
        }
        return damage;
    }

    public void UseAttack(AttackAbility ability) {
        if (GetComponent<PlayerCharacter>() != null) GetComponent<SimulatedNoiseGenerator>().CmdMakeNoise(transform.position, 22);
        var damage = CalculateAttackDamage(ability);
        var calculatedDamage = damage * ability.damage;
        if (ability.isRanged) {
            if (ability.FindAttribute("projectileSpread") != null && ability.FindAttribute("projectileSpread").priority >= 50) UseProjectileSpread(ability, damage, calculatedDamage);
            else if (ability.FindAttribute("bossDamageZone") != null) UseBossDamageZone(ability, damage, calculatedDamage);
            else if (ability.FindAttribute("bossHealingDamageZone") != null) UseBossHealingDamageZone(ability, damage, calculatedDamage);
            else if (ability.FindAttribute("bossCircleAoe") != null) UseBossCircleAoe(ability, damage, calculatedDamage);
            else if (ability.FindAttribute("bossLineAoe") != null) UseBossLineAoe(ability, damage, calculatedDamage);
            else if (ability.FindAttribute("bossBulletHell") != null) UseBossBulletHell(ability, damage, calculatedDamage);
            else if (ability.FindAttribute("bossHomingProjectile") != null) UseBossHomingProjectile(ability, damage, calculatedDamage);
            else if (ability.FindAttribute("bossJumpAndShoot") != null) UseBossJumpAndShoot(ability, damage, calculatedDamage);
            else UseRangedAttack(ability, damage, calculatedDamage);
        }
        else UseMeleeAttack(ability, damage, calculatedDamage);
    }

    public float CalculateAttackDamage(AttackAbility ability) {
        var damage = GetBaseDamage(ability.baseStat);
        if (GetComponent<Monster>() != null) damage *= 1.6f * 2f; // stat boost to deal with armor // stat boost to offset difficulty
        if (GetComponent<StatusEffectHost>().CheckForEffect("damage+")) damage *= (1 + GetComponent<StatusEffectHost>().GetEffect("damage+").degree);
        if (ability.FindAttribute("backstab") != null && GetComponent<StatusEffectHost>().CheckForEffect("stealth")) {
            damage *= (float)ability.FindAttribute("backstab").FindParameter("degree").value;
        }
        return ApplyDamageBoosts(damage, ability);
    }

    private void UseMeleeAttack(AttackAbility ability, float damage, float calculatedDamage) {
        if (ability==null && GetComponent<AbilityUser>().GCDTime > 0) return;
        GetComponent<AbilityUser>().GCDTime = AbilityUser.maxGCDTime;
        if (ability.FindAttribute("offGCD") != null) GetComponent<AbilityUser>().GCDTime = 0f;
        if ((ability.FindAttribute("chargeTowards") != null && ability.FindAttribute("chargeTowards").priority >= 50) || GetComponent<AbilityUser>().HasPassive("charge")) ChargeTowards();
        if (ability.FindAttribute("createDamageZone") != null) CreateMeleeDamageZone(ability);
        else if (ability.radius > 0) HitAllInRadius((int)calculatedDamage, ability.radius, ability);
        else {
            meleeAttackAbility = ability;
            meleeAttackDamage = calculatedDamage;
            StartCoroutine(ActivateMeleeHitbox());
        }
    }

    private void ChargeTowards() {
        gameObject.AddComponent<PushingEffect>().Initialize(transform.position + transform.forward * (10), 0.5f);
        GetComponent<AbilityUser>().RpcAddPushingEffect(transform.position + transform.forward * 10, 0.5f);
    }

    private void CreateMeleeDamageZone(AttackAbility ability) {
        var obj = Instantiate(GetComponent<CacheGrabber>().damageZones[ability.aoe], transform.position, transform.rotation);
        obj.transform.Rotate(-90f, 0f, 0f);
        obj.transform.localScale *= ability.radius;
        obj.GetComponent<DamageZoneDealDamage>().Initialize(ability, GetComponent<Character>(), GetComponent<Character>().faction);
        //NetworkServer.Spawn(obj);
    }

    private void UseRangedAttack(AttackAbility ability, float damage, float calculatedDamage) {
        var obj = Instantiate(GetComponent<CacheGrabber>().projectiles[ability.rangedProjectile], new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
        obj.GetComponentInChildren<RangedHitboxDealDamage>().Initialize(GetComponent<Character>(), (int)calculatedDamage, ability.attributes, ability.radius, damage * ability.dotDamage, ability.dotTime, GetComponent<Character>().faction, ability);
        //NetworkServer.Spawn(obj);
        var noises = new Dictionary<Element, string>() { { Element.bashing, "sfx_rock_throw1" }, { Element.piercing, "sfx_bow_release" }, { Element.slashing, "sfx_rock_throw1" } };
        var noise = "sfx_spell_project";
        if (noises.ContainsKey(ability.element)) noise = noises[ability.element];
        GetComponent<AudioGenerator>().PlaySoundByName(noise);
    }

    private void UseProjectileSpread(AttackAbility ability, float damage, float calculatedDamage) {
        float angle = 45;
        int projectiles = 5;
        float startingAngle = 0 - angle;
        float increment = (angle * 2) / (projectiles - 1);
        for (int i = 0; i < projectiles; i++) {
            var currentAngle = startingAngle + (increment * i);
            FireIndividualSpreadProjectile(ability, damage, calculatedDamage, currentAngle);
        }
    }

    private void UseBossDamageZone(AttackAbility ability, float damage, float calculatedDamage) {
        Damage.CreateDamageZone(GetComponent<Character>(), GetRandomPlayer(), ability, ability.radius, "Enemy");
    }

    private void UseBossHealingDamageZone(AttackAbility ability, float damage, float calculatedDamage) {
        var player = GetRandomPlayer();
        Damage.CreateDamageZone(GetComponent<Character>(), player, ability, ability.radius, "Enemy");
        var obj = GameObject.Instantiate(invisibleHealingZonePrefab, player.transform.position, player.transform.rotation);
        obj.transform.Rotate(-90f, 0f, 0f);
        var hz = obj.GetComponent<HealingZone>();
        hz.size = ability.radius;
        //NetworkServer.Spawn(obj);
        if (hz != null) {
            hz.ability = ability;
            hz.character = GetComponent<Character>();
            hz.faction = "Enemy";
        }
    }

    private void UseBossBulletHell(AttackAbility ability, float damage, float calculatedDamage) {
        float angle = 45;
        int projectiles = 5;
        float startingAngle = 0 - angle;
        for (int i = 0; i < projectiles; i++) {
            var currentAngle = Random.Range(startingAngle, angle);
            FireIndividualSpreadProjectile(ability, damage, calculatedDamage, currentAngle);
        }
        GetComponent<AbilityUser>().GCDTime = 0.25f;
    }

    private void UseBossJumpAndShoot(AttackAbility ability, float damage, float calculatedDamage) {
        GetComponent<AbilityUser>().BossTeleportToRandomSpot();
        UseRangedAttack(ability, damage, calculatedDamage);
    }

    private void UseBossCircleAoe(AttackAbility ability, float damage, float calculatedDamage) {
        var player = GetRandomPlayer();
        var obj = Instantiate(bossCircleAoePrefab, player.transform.position, bossCircleAoePrefab.transform.rotation);
        obj.GetComponent<TelegraphedAoe>().Initialize(ability, calculatedDamage, gameObject);
        //NetworkServer.Spawn(obj);
    }

    private void UseBossLineAoe(AttackAbility ability, float damage, float calculatedDamage) {
        var player = GetRandomPlayer();
        var obj = Instantiate(bossLineAoePrefab, (transform.position + player.transform.position) / 2, Quaternion.LookRotation(player.transform.position - transform.position));
        obj.transform.Rotate(0, 90, 0);
        obj.GetComponent<TelegraphedAoe>().Initialize(ability, calculatedDamage, gameObject);
        //NetworkServer.Spawn(obj);
    }

    private void UseBossHomingProjectile(AttackAbility ability, float damage, float calculatedDamage) {
        var player = GetRandomPlayer();
        var obj = Instantiate(homingProjectilePrefab, transform.position, Quaternion.LookRotation(player.transform.position - transform.position));
        obj.GetComponent<HomingProjectile>().Initialize(GetComponent<Character>(), ability, calculatedDamage, player);
        //NetworkServer.Spawn(obj);
    }

    private GameObject GetRandomPlayer() {
        int roll = Random.Range(0, PlayerCharacter.players.Count);
        var player = PlayerCharacter.players[roll];
        return player.gameObject;
    }

    private void FireIndividualSpreadProjectile(AttackAbility ability, float damage, float calculatedDamage, float currentAngle) {
        var obj = Instantiate(GetComponent<CacheGrabber>().projectiles[ability.rangedProjectile], new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
        obj.transform.Rotate(0, currentAngle, 0);
        obj.GetComponentInChildren<RangedHitboxDealDamage>().Initialize(GetComponent<Character>(), (int)calculatedDamage, ability.attributes, ability.radius, damage * ability.dotDamage, ability.dotTime, GetComponent<Character>().faction, ability);
        //NetworkServer.Spawn(obj);
    }
}
