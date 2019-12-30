using System.Collections.Generic;

public class MonsterData {
    public string generalType;
    public string specificType;
    public int level;
    public int quality;
    public SocialNode node;
    public List<Room> associatedRooms = new List<Room>();

    public MonsterData(string generalType, string specificType, int level, int quality, SocialNode node) {
        this.generalType = generalType;
        this.specificType = specificType;
        this.level = level;
        this.quality = quality;
        this.node = node;
    }
}
