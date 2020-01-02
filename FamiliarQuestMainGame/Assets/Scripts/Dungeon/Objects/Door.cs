using UnityEngine;

public class Door : MonoBehaviour { // : Hideable

    public bool open = false;
    public Collider doorCollider;
    //private float startAngle = 90;
    //private float targetAngle = 0;
    //public Vector3 closedPosition;
    //public Vector3 openPosition;
    //private float openTime = 0.5f;
    //private float timeSpentOpening = 0;
    //public bool reversedDoor = false;
    //public bool reverseHinge = false;
    //private Mappable mappable;

    void Start() {
        //items.Add(this);
        //closedPosition = transform.position;
        //openPosition = closedPosition;
        //mappable = GetComponent<Mappable>();
    }

    void Update() {
        //if (transform.localEulerAngles != new Vector3(transform.localEulerAngles.x, targetAngle, transform.localEulerAngles.z)) {
        //    timeSpentOpening += Time.deltaTime;
        //    transform.localEulerAngles = new Vector3(0, Mathf.LerpAngle(startAngle, targetAngle, timeSpentOpening / openTime), 0);
        //    if (targetAngle==90) transform.position = Vector3.Lerp(closedPosition, openPosition, timeSpentOpening / openTime);
        //    else transform.position = Vector3.Lerp(openPosition, closedPosition, timeSpentOpening / openTime);
        //    if (timeSpentOpening>=openTime && !open) GetComponent<BoxCollider>().isTrigger = false;
        //}
    }


    private void OnTriggerStay(Collider other) {
        //if (other.GetComponent<PlayerCharacter>() != null && isSecret) OpenSecretDoor(other.gameObject);
    }

    private void OpenSecretDoor(GameObject other) {
        if (other == null) other = PlayerCharacter.localPlayer.gameObject;
        other.GetComponent<AudioGenerator>().PlaySoundByName("sfx_door_open1");
        if (other.GetComponent<Mapper>() != null) other.GetComponent<Mapper>().CmdUpdateMap((int)((transform.position.x + 120) / 2), (int)((transform.position.z + 120) / 2), "{REMOVE}");
        //prune = true;
        Destroy(gameObject);
    }

    public void OpenDoor() {
        if (!open) {
            GetComponent<BoxCollider>().isTrigger = true;
            //mappable.unmappable = true;
            GetComponent<AudioSource>().Play();
            open = true;
            GetComponent<UsableObject>().helpText = "close door";
            //timeSpentOpening = 0;
            //startAngle = 0;
            //targetAngle = 90;
            //var pos = transform.position;
            //if (!reversedDoor) {
            //    if (reverseHinge) openPosition = closedPosition + new Vector3(-1, 0, -1);
            //    else openPosition = closedPosition + new Vector3(1, 0, 1);
            //}
            //else {
            //    if (reverseHinge) openPosition = closedPosition + new Vector3(-1, 0, 1);
            //    else openPosition = closedPosition + new Vector3(1, 0, -1);
            //}
            GetComponentInChildren<Animation>().Play("open");
            doorCollider.isTrigger = true;

        }
        else {
            GetComponent<AudioSource>().Play();
            open = false;
            GetComponent<UsableObject>().helpText = "open door";
            //timeSpentOpening = 0;
            //startAngle = 90;
            //targetAngle = 0;
            GetComponentInChildren<Animation>().Play("close");
            doorCollider.isTrigger = false;

        }
    }


    //private void BlockMonster()
    //{
    //    var obstacle = GetComponent<NavMeshObstacle>();
    //    obstacle.enabled = true;
    //}
}
