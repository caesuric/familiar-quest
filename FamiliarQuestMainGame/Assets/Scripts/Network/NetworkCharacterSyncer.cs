using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkCharacterSyncer : MonoBehaviour
{
    public float secondsBetweenSelfUpdate = 1;
    public float secondsBetweenGlobalUpdate = 1;
    public NetworkCharacterSyncerType type = NetworkCharacterSyncerType.player;
    private float selfTimer = 0;
    private float globalTimer = 0;
    public float lerpTimer = 0;
    public float lerpSeconds = 0;
    private bool isLerping = false;
    private Vector3 lerpSpeed = new Vector3(0, 0, 0);
    private Vector3 finalLerpPoint = new Vector3(0, 0, 0);
    private Quaternion lerpRotation = new Quaternion();
    public string id = null;
    public CharacterWorldState worldState = null;
    public double lastUpdateTimestamp = 0;
    public static double currentUpdateTimestamp = 0;
    private bool deathPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        if (id==null || id=="") id = Guid.NewGuid().ToString();
        if (worldState == null) {
            worldState = new CharacterWorldState();
            worldState.Id = id;
        }
        ProtoClient.syncedCharacters.Add(this);
        if (ProtoClient.localPlayer == null && type == NetworkCharacterSyncerType.player) {
            ProtoClient.localPlayer = this;
            worldState.ModelData = new ModelData();
            worldState.ModelData.PlayerAppearance = new PlayerAppearanceData();
        }
        var pc = GetComponent<PlayerCharacter>();
        if (pc != null && type == NetworkCharacterSyncerType.player) {
            pc.characterId = id;
        }
        worldState.IsPlayer = (type == NetworkCharacterSyncerType.player);
    }

    private void SetAppearanceData() {
        if (type == NetworkCharacterSyncerType.player) {
            if (ProtoClient.localPlayer!=this && worldState.ModelData.PlayerAppearance.FurType > 0) {
                GetComponent<PlayerSyncer>().furType = worldState.ModelData.PlayerAppearance.FurType;
                GetComponent<PlayerSyncer>().furTypeSet = true;
            }
            else if (GetComponent<PlayerSyncer>().furTypeSet) {
                worldState.ModelData = new ModelData();
                worldState.ModelData.PlayerAppearance = new PlayerAppearanceData();
                worldState.ModelData.PlayerAppearance.FurType = GetComponent<PlayerSyncer>().furType;
            }
        }
        else {
            worldState.ModelData = new ModelData();
            worldState.ModelData.MonsterPrefab = new MonsterPrefabData();
            worldState.ModelData.MonsterPrefab.PrefabName = gameObject.name.Replace(" (Clone)", "");
        }
    }

    // Update is called once per frame
    void Update() {
        if (type == NetworkCharacterSyncerType.player && (!GetComponent<PlayerSyncer>().furTypeSet || worldState.ModelData.PlayerAppearance.FurType < 1)) SetAppearanceData();
        selfTimer += Time.deltaTime;
        globalTimer += Time.deltaTime;
        if (selfTimer >= secondsBetweenSelfUpdate) {
            selfTimer = 0;
            if ((type == NetworkCharacterSyncerType.monster || ProtoClient.localPlayer != this) && currentUpdateTimestamp > lastUpdateTimestamp) {
                var location = worldState.Location;
                var trajectory = worldState.Trajectory;
                var orientation = worldState.Orientation;
                if (location!=null && (location.X != 0 || location.Y != 0 || location.Z != 0) && location != null && trajectory != null) {
                    lerpTimer = 0;
                    lerpSeconds = (float)(currentUpdateTimestamp - lastUpdateTimestamp);
                    isLerping = true;
                    lerpSpeed = new Vector3(location.X + trajectory.X, location.Y + trajectory.Y, location.Z + trajectory.Z) - transform.position;
                    //finalLerpPoint = new Vector3(location.X + trajectory.X, location.Y + trajectory.Y, location.Z + trajectory.Z);
                    finalLerpPoint = new Vector3(location.X, location.Y, location.Z);
                    lerpRotation = new Quaternion(orientation.X, orientation.Y, orientation.Z, orientation.W);
                }
                RunAnimations();
                lastUpdateTimestamp = currentUpdateTimestamp;
            }
            if (type != NetworkCharacterSyncerType.monster && ProtoClient.localPlayer == this) {
                SendPersonalUpdate();
            }
        }

        if (globalTimer >= secondsBetweenGlobalUpdate) {
            globalTimer = 0;
            if (type == NetworkCharacterSyncerType.player && ProtoClient.isHost && ProtoClient.localPlayer == this) {
                AddNewCharacters();
                GetPlayerUpdates();
                //SendUpdates();
            }
            else if (type == NetworkCharacterSyncerType.player && !ProtoClient.isHost && ProtoClient.localPlayer == this) {
                AddNewCharacters();
                ReceiveUpdates();
            }
        }
    }

    private void RunAnimations() {
        if (worldState.AnimationState == null) return;
        if (worldState.IsPlayer) {
            switch (worldState.AnimationState.State) {
                case "idle":
                default:
                    GetComponentInChildren<Animation>().CrossFade("Idle");
                    break;
                case "moving":
                    GetComponentInChildren<Animation>().CrossFade("kitt_RunInPlace");
                    break;
                case "dead":
                    if (!deathPlayed) {
                        deathPlayed = true;
                        GetComponentInChildren<Animation>().CrossFade("kitt_DeathA");
                    }
                    break;

            }
        }
    }

    void FixedUpdate() {
        if (isLerping) {
            lerpTimer += Time.fixedDeltaTime;
            if (lerpTimer >= lerpSeconds) {
                isLerping = false;
                //transform.position = finalLerpPoint;
            }
            //else transform.position += lerpSpeed * Time.deltaTime / lerpSeconds;
            else transform.position = Vector3.Lerp(transform.position, finalLerpPoint, lerpTimer / lerpSeconds);
            transform.rotation = Quaternion.Lerp(transform.rotation, lerpRotation, lerpTimer / lerpSeconds);
        }
    }
    
    private async void GetPlayerUpdates() {
        await Task.Run(ProtoClient.GetPlayerUpdates);
    }


    private async void SendUpdates() {
        foreach (var syncer in ProtoClient.syncedCharacters) {
            if (syncer.type == NetworkCharacterSyncerType.player) continue;
            syncer.worldState.Location = new Location();
            syncer.worldState.Location.X = syncer.transform.position.x;
            syncer.worldState.Location.Y = syncer.transform.position.y;
            syncer.worldState.Location.Z = syncer.transform.position.z;
            if (syncer.worldState.Trajectory==null) syncer.worldState.Trajectory = new Trajectory() { X = 0, Y = 0, Z = 0 };
            syncer.worldState.Orientation = new Orientation();
            syncer.worldState.Orientation.X = syncer.transform.rotation.x;
            syncer.worldState.Orientation.Y = syncer.transform.rotation.y;
            syncer.worldState.Orientation.Z = syncer.transform.rotation.z;
            syncer.worldState.Orientation.W = syncer.transform.rotation.w;
            syncer.worldState.IsPlayer = (syncer.type == NetworkCharacterSyncerType.player);
            SetModelData(syncer);
            SetAnimationState(syncer);
            SetHealthAndStatus(syncer);
            SetVisibility(syncer);
        }
        await Task.Run(ProtoClient.UpdateCharacterWorldStates);
    }

    private void SetVisibility(NetworkCharacterSyncer syncer) {
        if (syncer.type == NetworkCharacterSyncerType.player) syncer.worldState.IsVisible = true;
        else if (Hider.instance.IsSeen(syncer.GetComponent<Hideable>())) syncer.worldState.IsVisible = true;
        else syncer.worldState.IsVisible = false;
    }

    private void SetHealthAndStatus(NetworkCharacterSyncer syncer) {
        syncer.worldState.HealthAndStatus = new HealthAndStatus();
        if (syncer.type == NetworkCharacterSyncerType.player) syncer.worldState.HealthAndStatus.Level = syncer.GetComponent<ExperienceGainer>().level;
        else syncer.worldState.HealthAndStatus.Level = syncer.GetComponent<MonsterScaler>().level;
        syncer.worldState.HealthAndStatus.Hp = (int)syncer.GetComponent<Health>().hp;
        syncer.worldState.HealthAndStatus.MaxHp = (int)syncer.GetComponent<Health>().maxHP;
    }

    private void SetModelData(NetworkCharacterSyncer syncer) {
        if (syncer.type == NetworkCharacterSyncerType.player) {
            syncer.worldState.ModelData = new ModelData();
            syncer.worldState.ModelData.PlayerAppearance = new PlayerAppearanceData();
            syncer.worldState.ModelData.PlayerAppearance.FurType = syncer.GetComponent<PlayerSyncer>().furType;
        }
        else {
            syncer.worldState.ModelData = new ModelData();
            syncer.worldState.ModelData.MonsterPrefab = new MonsterPrefabData();
            syncer.worldState.ModelData.MonsterPrefab.PrefabName = syncer.gameObject.name.Replace(" (Clone)", "");
        }
    }

    private void SetAnimationState(NetworkCharacterSyncer syncer) {
        if (syncer.type == NetworkCharacterSyncerType.player) {
            syncer.worldState.AnimationState = new AnimationState();
            syncer.worldState.AnimationState.State = GetPlayerAnimationState(syncer);
        }
        else {
            var controller = syncer.GetComponent<MonsterAnimationController>();
            if (syncer.worldState.AnimationState==null) syncer.worldState.AnimationState = new AnimationState();
            if (controller.attacking) syncer.worldState.AnimationState.State = "attacking";
            else if (controller.moving) syncer.worldState.AnimationState.State = "moving";
            else syncer.worldState.AnimationState.State = "idle";
        }
    }

    private string GetPlayerAnimationState(NetworkCharacterSyncer syncer) {
        if (syncer.GetComponent<Health>().hp <= 0) return "dead";
        else if (syncer.GetComponent<InputController>().moving) return "moving";
        else return "idle";
    }

    private async void SendPersonalUpdate() {
        if (worldState.Location == null) {
            worldState.Trajectory = new Trajectory() { X = 0f, Y = 0f, Z = 0f };
        }
        else {
            worldState.Trajectory.X = (transform.position.x - worldState.Location.X) / 4f;
            worldState.Trajectory.Y = 0f;
            worldState.Trajectory.Z = (transform.position.z - worldState.Location.Z) / 4f;
        }
        worldState.Location = new Location();
        worldState.Location.X = transform.position.x;
        worldState.Location.Y = transform.position.y;
        worldState.Location.Z = transform.position.z;
        worldState.Orientation = new Orientation();
        worldState.Orientation.X = transform.rotation.x;
        worldState.Orientation.Y = transform.rotation.y;
        worldState.Orientation.Z = transform.rotation.z;
        worldState.Orientation.W = transform.rotation.w;
        worldState.AnimationState = new AnimationState();
        worldState.HealthAndStatus = new HealthAndStatus();
        worldState.HealthAndStatus.Level = GetComponent<ExperienceGainer>().level;
        SetAnimationState(this);
        await Task.Run(() => ProtoClient.UpdatePersonalWorldState(worldState));
    }

    private async void ReceiveUpdates() {
        await Task.Run(ProtoClient.GetCharacterWorldStates);
    }

    private void AddNewCharacters() {
        ProtoClient.newSyncedCharactersLocked = true;
        foreach (var character in ProtoClient.newSyncedCharacters) AddNewCharacter(character);
        ProtoClient.newSyncedCharacters.Clear();
        ProtoClient.newSyncedCharactersLocked = false;
    }

    private void AddNewCharacter(CharacterWorldState data) {
        foreach (var item in ProtoClient.syncedCharacters) if (item.worldState.Id == data.Id) return;
        if (data.IsPlayer) {
            var prefab = Resources.Load("Prefabs/Player/kittenCharacter") as GameObject;
            var newPlayer = Instantiate(prefab);
            var components = newPlayer.GetComponents(typeof(MonoBehaviour));
            foreach (MonoBehaviour component in components) {
                component.enabled = false;
            }
            //newPlayer.GetComponent<InputController>().enabled = true;
            newPlayer.GetComponent<CacheGrabber>().enabled = true;
            newPlayer.GetComponent<PlayerSyncer>().enabled = true;
            //newPlayer.GetComponent<PlayerSyncer>().furType = data.ModelData.PlayerAppearance.FurType;
            //newPlayer.GetComponent<PlayerSyncer>().furTypeSet = true;
            newPlayer.GetComponent<NetworkCharacterSyncer>().enabled = true;
            newPlayer.GetComponent<NetworkCharacterSyncer>().worldState = data;
            newPlayer.GetComponent<Character>().enabled = true;
            newPlayer.GetComponent<PlayerCharacter>().enabled = true;
        }
        else {
            var prefab = Resources.Load("Prefabs/Monsters/" + data.ModelData.MonsterPrefab.PrefabName) as GameObject;
            var newMonster = Instantiate(prefab);
            var components = newMonster.GetComponents(typeof(MonoBehaviour));
            foreach (MonoBehaviour component in components) {
                component.enabled = false;
            }
            var mac = newMonster.GetComponent<MonsterAnimationController>();
            mac.enabled = true;
            if (data.AnimationState.State == "attacking") mac.attacking = true;
            else mac.attacking = false;
            if (data.AnimationState.State == "moving") mac.moving = true;
            else mac.moving = false;
            var animator = newMonster.GetComponent<Animator>();
            var animation = newMonster.GetComponent<Animation>();
            if (animator != null) animator.enabled = true;
            if (animation != null) animation.enabled = true;
            newMonster.GetComponent<NetworkCharacterSyncer>().enabled = true;
            newMonster.GetComponent<NetworkCharacterSyncer>().worldState = data;
        }
    }

    void OnDestroy() {
        ProtoClient.syncedCharacters.Remove(this);
    }
}

public enum NetworkCharacterSyncerType {
    player=1,
    monster=2
}