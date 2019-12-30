using System.Collections.Generic;
using UnityEngine;

public abstract class Room {
    public float size;
    public float grandiosity;
    public int x;
    public int y;
    public int floor;
    public int xSize;
    public int ySize;
    public List<Vector3> entrances = new List<Vector3>();
    public List<Vector2> dressingLocations = new List<Vector2>();

    public void SetEntrance(int x, int y, int direction) {
        x = ClampEntranceX(x);
        y = ClampEntranceY(y);
        entrances.Add(new Vector3(x, y, direction));
    }

    public int ClampEntranceX(int x) {
        if (x < this.x) x = this.x;
        if (x > this.x + xSize - 1) x = this.x + xSize - 1;
        return x;
    }

    public int ClampEntranceY(int y) {
        if (y < this.y) y = this.y;
        if (y > this.y + ySize - 1) y = this.y + ySize - 1;
        return y;
    }

    public void TopLeftCorner(out int x, out int y) {
        int i = Random.Range(0, 2);
        if (i == 0) {
            x = this.x;
            y = this.y - 1;
        }
        else {
            x = this.x - 1;
            y = this.y;
        }
    }

    public void TopRightCorner(out int x, out int y) {
        int i = Random.Range(0, 2);
        if (i == 0) {
            x = this.x + xSize - 1;
            y = this.y - 1;
        }
        else {
            x = this.x + xSize;
            y = this.y;
        }
    }

    public void BottomLeftCorner(out int x, out int y) {
        int i = Random.Range(0, 2);
        if (i == 0) {
            x = this.x;
            y = this.y + ySize;
        }
        else {
            x = this.x - 1;
            y = this.y + ySize - 1;
        }
    }

    public void BottomRightCorner(out int x, out int y) {
        int i = Random.Range(0, 2);
        if (i == 0) {
            x = this.x + xSize - 1;
            y = this.y + ySize;
        }
        else {
            x = this.x + xSize;
            y = this.y + ySize - 1;
        }
    }

    public void LeftSide(Room room, out int x, out int y) {
        x = this.x - 1;
        var min = Mathf.Max(this.y, room.y);
        var max = Mathf.Min(this.y + ySize, room.y + room.ySize);
        y = Random.Range(min, max);
    }

    public void RightSide(Room room, out int x, out int y) {
        x = this.x + xSize;
        var min = Mathf.Max(this.y, room.y);
        var max = Mathf.Min(this.y + ySize, room.y + room.ySize);
        y = Random.Range(min, max);
    }

    public void TopSide(Room room, out int x, out int y) {
        y = this.y - 1;
        var min = Mathf.Max(this.x, room.x);
        var max = Mathf.Min(this.x + ySize, room.x + room.xSize);
        x = Random.Range(min, max);
    }

    public void BottomSide(Room room, out int x, out int y) {
        y = this.y + ySize;
        var min = Mathf.Max(this.x, room.x);
        var max = Mathf.Min(this.x + ySize, room.x + room.xSize);
        x = Random.Range(min, max);
    }
}

public class AssignedRoom : Room {
    public int socialTier;
    public List<MonsterData> inhabitants = new List<MonsterData>();
}

public class Corridor : Room {
    public List<Room> connectedRooms = new List<Room>();
}

public class LivingQuarters : AssignedRoom {

}

public class CommonSpace : AssignedRoom {

}

public class BossRoom: AssignedRoom {

}