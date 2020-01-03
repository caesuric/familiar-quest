using System.Collections.Generic;

public class QuestLog {
    public List<Quest> quests = new List<Quest>();
    public QuestLog() {
        quests.Add(new Quest());
    }
}

public class Quest {
    public bool completed = false;
    public string text = "- Get to the final floor and defeat the boss";
}