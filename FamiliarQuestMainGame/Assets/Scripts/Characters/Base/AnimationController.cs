using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(StatusEffectHost))]
public class AnimationController : MonoBehaviour {

    public bool hasIsAttacking = false;
    public bool hasIsMoving = false;
    bool checkedParameters = false;
    // Use this for initialization
    void Start() {
        var animator = GetComponentInChildren<Animator>();
        if (animator != null) {
            if (!checkedParameters) {
                checkedParameters = true;
                foreach (var param in animator.parameters) {
                    if (param.name == "isAttacking") hasIsAttacking = true;
                    else if (param.name == "isMoving") hasIsMoving = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (GetComponent<StatusEffectHost>().CheckForEffect("paralysis"))
        {
                if (hasIsAttacking) GetComponentInChildren<Animator>().SetBool("isAttacking", false);
                if (hasIsMoving) GetComponentInChildren<Animator>().SetBool("isMoving", false);
        }
    }

    public void SetAttacking(bool isAttacking) {
        var animator = GetComponentInChildren<Animator>();
        if (animator != null && hasIsAttacking) animator.SetBool("isAttacking", isAttacking);
    }
}
