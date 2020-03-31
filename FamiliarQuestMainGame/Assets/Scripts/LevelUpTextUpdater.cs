using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpTextUpdater : MonoBehaviour
{
    private bool triggered = false;
    private float timer = 0f;
    public Text text;
    private static readonly float timeToFadeIn = 1f;
    private static readonly float timeToStay = 2f;
    private static readonly float postFadeAmount = 0.1f;
    private static readonly float finalFadeTime = 1f;
    private static LevelUpTextUpdater instance;
    private Vector3 startingScale = new Vector3(1, 1, 1);

    void Start() {
        text.enabled = false;
        instance = this;
        startingScale = transform.localScale;
    }

    void Update()
    {
        if (triggered) AnimationStep();
    }

    private void AnimationStep() {
        timer += Time.deltaTime;
        if (timer > timeToFadeIn + timeToStay + finalFadeTime) {
            timer = 0;
            triggered = false;
            text.enabled = false;
            return;
        }
        else if (timer > timeToFadeIn + timeToStay) {
            var timerModified = timer - timeToFadeIn - timeToStay;
            var percentage = timerModified / finalFadeTime;
            text.color = new Color(0, 1, 0, 1 - percentage);
        }
        else if (timer > timeToFadeIn) {
            var timerModified = timer - timeToFadeIn;
            var percentage = postFadeAmount * timerModified / timeToStay;
            text.color = new Color(0, 1, 0, 1 - percentage);
            transform.localScale = startingScale * (1 - percentage);
        }
        else {
            text.color = new Color(0, 1, 0, timer / timeToFadeIn);
            transform.localScale = startingScale * timer / timeToFadeIn;
        }
    }

    public static void Trigger() {
        instance.text.color = new Color(0, 1, 0, 0);
        instance.transform.localScale = instance.startingScale * 0.001f;
        instance.text.enabled = true;
        instance.triggered = true;
    }
}
