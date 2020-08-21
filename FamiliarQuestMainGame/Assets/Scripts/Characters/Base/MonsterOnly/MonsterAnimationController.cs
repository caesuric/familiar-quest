using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

class MonsterAnimationController : MonoBehaviour {
    public GameObject spiritEffect = null;
    public float rotationTimer = 0;
    public float rotationCount = 4;
    public Quaternion rotationAngle;
    private bool showSpiritEffect = false;
    public bool moving = false;
    public bool attacking = false;
    public string attackAnimation;
    public string idleAnimation;
    public string moveAnimation;
    private string currentClip = "";


    void Start() {
        rotationTimer += Random.Range(0, 4);
        if (spiritEffect == null) {
            var particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null) spiritEffect = particleSystem.gameObject;
        }
        showSpiritEffect = (GetComponent<AbilityUser>().soulGemActives.Count > 0 || GetComponent<AbilityUser>().soulGemPassive != null);
    }

    void Update() {
        if (spiritEffect != null) spiritEffect.SetActive(showSpiritEffect);
        var animation = GetComponentInChildren<Animation>();
        var animator = GetComponentInChildren<Animator>();
        if (animation != null) {
            if (attacking) animation.CrossFade(attackAnimation);
            else if (moving) animation.CrossFade(moveAnimation);
            else animation.CrossFade(idleAnimation);
        }
        //else if (animator != null) {
        //    if (GetComponent<AnimationController>()==null || !GetComponent<AnimationController>().hasIsAttacking) {
        //        if (attacking && !IsClipRunning(attackAnimation)) PlayClip(animator, attackAnimation);
        //        else if (moving && !IsClipRunning(moveAnimation)) PlayClip(animator, moveAnimation);
        //        else if (!IsClipRunning(idleAnimation)) PlayClip(animator, idleAnimation);
        //    }
        //}
        if (GetComponent<StatusEffectHost>().CheckForEffect("paralysis")) {
            if (GetComponentInChildren<Animation>() != null) {
                GetComponentInChildren<Animation>().CrossFade(idleAnimation);
                moving = false;
                attacking = false;
            }
        }
    }

    public void PlayClip(Animator animator, string clip) {
        animator.Play(clip);
        currentClip = clip;
    }

    public bool IsClipRunning(string clipName) {
        if (currentClip == clipName) return true;
        return false;
    }
}
