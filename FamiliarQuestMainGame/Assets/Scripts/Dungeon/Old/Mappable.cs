using UnityEngine;
using System.Collections;

public class Mappable : MonoBehaviour {

    public string value;
    public bool unmappable = false;
    public Mapper mapperRef = null;
    public float xOffset = 0;
    public float yOffset = 0;

    public void OnDestroy() {
        if (unmappable && mapperRef != null) {
            mapperRef.RemoveFromMap(transform, this);
        }
    }
}
