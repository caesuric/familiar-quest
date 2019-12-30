using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect {

    public float duration;
    public float beginningDuration;
    public float degree;
    public string type;
    public bool good;
    public Character inflicter = null;
    public GameObject icon = null;
    public ActiveAbility ability = null;
    public GameObject visual = null;

    public StatusEffect(string type, float duration, float degree, Character inflicter=null, bool good=false, ActiveAbility ability=null, GameObject visual=null)
    {
        this.duration = beginningDuration = duration;
        this.type = type;
        this.degree = degree;
        this.inflicter = inflicter;
        this.good = good;
        this.ability = ability;
        this.visual = visual;
    }
    
    public void Update () {
        if (duration > 0) duration = Mathf.Max(duration - Time.deltaTime, 0);
	}
}
