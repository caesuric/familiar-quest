using UnityEngine;

public class Cave : IncidentalDungeon {
    public void Initialize() {
        var nms = LevelGen.instance.navMeshSurface.gameObject;
        var floorPrefab = LevelGenPrefabs.prefabs["cave"]["floor"]["Cave/Floor1"];
        var floor1 = GameObject.Instantiate(floorPrefab, new Vector3(0, 2, 0), floorPrefab.transform.rotation);
        floor1.transform.parent = nms.transform;
        var floor2 = GameObject.Instantiate(floorPrefab, new Vector3(5, 2.01f, 5), floorPrefab.transform.rotation);
        floor2.transform.parent = nms.transform;
        //MeshFilter[] meshFilters = { floor1.GetComponent<MeshFilter>(), floor2.GetComponent<MeshFilter>() };
        //CombineInstance[] combineInstance = new CombineInstance[meshFilters.Length];
        //for (int i=0; i< meshFilters.Length; i++) {
        //    combineInstance[i].mesh = meshFilters[i].sharedMesh;
        //    combineInstance[i].transform = meshFilters[i].transform.localToWorldMatrix;
        //}
        //var meshFilter = nms.AddComponent<MeshFilter>();
        //meshFilter.mesh = new Mesh();
        //var meshRenderer = nms.AddComponent<MeshRenderer>();
        //meshRenderer.material = floor1.GetComponent<MeshRenderer>().material;
        //nms.AddComponent<MeshCollider>();
        LevelGen.instance.navMeshSurface.BuildNavMesh();
        //meshFilter.mesh.CombineMeshes(combineInstance);
    }
}
