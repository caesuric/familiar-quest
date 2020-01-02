public class Altar : Hideable {

    private bool used = false;
    // Use this for initialization
    void Start() {
        items.Add(this);
    }

    public void Use() {
        //if (!NetworkServer.active) return;
        if (used) return;
        var playerCharacter = PlayerCharacter.localPlayer;
        if (playerCharacter != null) {
            used = true;
            var spirit = new Spirit(LevelGen.targetLevel);
            foreach (var ability in spirit.activeAbilities) playerCharacter.GetComponent<SpiritUser>().spirits[0].activeAbilities.Add(ability);
            prune = true;
            Destroy(gameObject);
            playerCharacter.GetComponent<ObjectSpawner>().CreateFloatingStatusText("ABILITY FOUND", "Ability found!");
            playerCharacter.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
    }
}