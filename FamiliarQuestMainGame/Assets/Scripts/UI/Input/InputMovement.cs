using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputMovement {
    public static InputController controller;
    public static Character character;
    private static float stepCount = 0;
    private static float updateRate = 0;
    private static float timer = 0;
    public static float lastPosX = 0;
    public static float lastPosY = 0;
    public static float lastRotation = 0;
    private static float multiplier = 1;
    private static float[,] angles;
    private static GameObject canvas = null;
    public static bool isDragging = false;

    public static void Initialize(InputController controller) {
        InputMovement.controller = controller;
        character = controller.character;
        angles = new float[3, 3] {
            { 225, 270, 315 },
            { 180, 0, 0 },
            { 135, 90, 45 }
        };
    }

    public static void KeyboardCheck() {
        if (character == null || character.GetComponent<StatusEffectHost>().CheckForEffect("paralysis")) return;
        CheckForStealthAndApplyEffect();
        MoveCharacter();
    }

    public static void MouseCheck() {
        if (character == null || character.GetComponent<StatusEffectHost>().CheckForEffect("paralysis") || isDragging) return;
        if (Input.GetMouseButton(0) && !ClickIsOnUi()) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) character.transform.LookAt(hit.point, character.transform.up);
            character.transform.eulerAngles = new Vector3(0, character.transform.eulerAngles.y, 0);
            controller.CmdUsePrimaryAbility(character.transform.eulerAngles);
        }
        else if (Input.GetMouseButton(1) && !ClickIsOnUi()) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) character.transform.LookAt(hit.point, character.transform.up);
            character.transform.eulerAngles = new Vector3(0, character.transform.eulerAngles.y, 0);
            controller.CmdUseSecondaryAbility(character.transform.eulerAngles);
        }
    }

    private static void CheckForStealthAndApplyEffect() {
        multiplier = character.GetComponent<PlayerSyncer>().speedMultiplier;
        if (character.GetComponent<PlayerSyncer>().stealthy) {
            if (!controller.stealthy) TurnOnStealth();
            multiplier = 0.5f;
        }
        else if (controller.stealthy) TurnOffStealth();
    }

    private static void TurnOnStealth() {
        controller.stealthy = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile = controller.stealthEffect;
        controller.gameObject.GetComponentInChildren<Light>().intensity = 1.3f;
    }

    private static void TurnOffStealth() {
        controller.stealthy = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().profile = controller.normalEffect;
        controller.gameObject.GetComponentInChildren<Light>().intensity = 2;
    }

    private static void MoveCharacter() {
        if (character.GetComponent<StatusEffectHost>().CheckForEffect("immobilize")) return;
        var extraMultiplier = 10f;
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var calculatedSpeed = controller.speed * multiplier * SecondaryStatUtility.CalcMoveSpeed(character.dexterity, character.GetComponent<ExperienceGainer>().level);
        if (!controller.gamepadMode) {
            MoveCharacterMaintenance(horizontal != 0 || vertical != 0);
            if (controller.moving) controller.transform.eulerAngles = new Vector3(0, angles[(int)Math.Sign(horizontal) + 1, (int)Math.Sign(vertical) + 1], 0);
            controller.rigidbody.velocity = new Vector3(horizontal * calculatedSpeed, 0, vertical * calculatedSpeed);
        }
        else {
            horizontal = Input.GetAxis("Left Stick Horizontal");
            vertical = Input.GetAxis("Left Stick Vertical");
            MoveCharacterMaintenance(horizontal != 0 || vertical != 0);
            var direction = new Vector3(horizontal, 0, vertical);
            if (direction != Vector3.zero) {
                controller.transform.rotation = Quaternion.LookRotation(direction);
                controller.rigidbody.velocity = new Vector3(horizontal * calculatedSpeed, 0, vertical * calculatedSpeed);
            }
            else {
                controller.rigidbody.velocity = new Vector3(0, 0, 0);
            }
        }
    }

    private static void MoveCharacterWithoutFacing() {
        var calculatedSpeed = controller.speed * multiplier * SecondaryStatUtility.CalcMoveSpeed(character.dexterity, character.GetComponent<ExperienceGainer>().level);
        MoveCharacterMaintenance(true);
        controller.rigidbody.velocity = character.transform.forward * calculatedSpeed;
    }

    private static void MoveCharacterMaintenance(bool isMoving) {
        controller.moving = isMoving;
        controller.CmdSetMoving(isMoving);
        if (isMoving) {
            stepCount += Time.deltaTime;
            if (!controller.moving) controller.CmdSetMoving(true);
        }
        else if (controller.moving) controller.CmdSetMoving(false);
        if (stepCount >= 0.25) {
            stepCount = 0;
            if (!character.GetComponent<StatusEffectHost>().CheckForEffect("stealth")) character.GetComponent<SimulatedNoiseGenerator>().CmdMakeNoise(character.transform.position, 18);
            character.GetComponent<SmellGenerator>().CmdMakeSmell(character.transform.position, 10);
        }
    }

    public static bool ClickIsOnUi() {
        if (canvas == null) canvas = GameObject.FindGameObjectWithTag("Canvas");
        var caster = canvas.GetComponent<GraphicRaycaster>();
        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        caster.Raycast(pointerEventData, results);
        if (results.Count > 0 && results[0].gameObject.name != "Large Status Text" && results[0].gameObject.name != "Minimap" && results[0].gameObject.name != "Party Health Pane" && !results[0].gameObject.name.Contains("Minimap") && results[0].gameObject.name != "Canvas") return true;
        return false;
    }
}

