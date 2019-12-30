using UnityEngine;
using System.Collections;

public class PlayerAnimation {
    private Animator animator;
    private Animation animation;
    private Character character;
    private InputController controller;
    private bool deathPlayed = false;

    public PlayerAnimation(GameObject player) {
        animator = player.GetComponentInChildren<Animator>();
        animation = player.GetComponentInChildren<Animation>();
        character = player.GetComponent<Character>();
        controller = player.GetComponent<InputController>();
    }

    public void Animate() {
        if (character != null && character.GetComponent<Health>().hp <= 0) AnimateDeath();
        else if (controller.moving) AnimateMovement();
        else AnimateIdle();
    }

    public void AnimateDeath() {
        if (controller==null) {
            if (!deathPlayed) {
                animation.CrossFade("kitt_DeathA");
                deathPlayed = true;
            }
            return;
        }
        if (controller.moving) controller.CmdSetMoving(false);
        if (!controller.dead && animator != null) {
            //animator.SetBool("isDead", true); //not set up yet
        }
        else if (!controller.dead && animation != null) animation.CrossFade("kitt_DeathA");
        if (!controller.dead) controller.dead = true;
    }

    public void AnimateMovement() {
        if (animator != null) animator.SetBool("isMoving", true);
        else if (animation != null) animation.CrossFade("kitt_RunInPlace");
    }

    public void AnimateIdle() {
        if (animator != null) animator.SetBool("isMoving", false);
        else if (animation != null) animation.CrossFade("Idle");
    }
}