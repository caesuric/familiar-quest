﻿using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputMovement {
    public static InputController controller;
    public static Character character;
    private static float stepCount = 0;
    private static readonly float updateRate = 0;
    private static readonly float timer = 0;
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) character.transform.LookAt(hit.point, character.transform.up);
            character.transform.eulerAngles = new Vector3(0, character.transform.eulerAngles.y, 0);
            controller.CmdUsePrimaryAbility(character.transform.eulerAngles);
        }
        else if (Input.GetMouseButton(1) && !ClickIsOnUi()) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) character.transform.LookAt(hit.point, character.transform.up);
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
        //var extraMultiplier = 10f;
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        //var calculatedSpeed = controller.speed * multiplier * SecondaryStatUtility.CalcMoveSpeed(character.dexterity, character.GetComponent<ExperienceGainer>().level);
        var calculatedSpeed = controller.speed * multiplier * CharacterAttribute.attributes["moveSpeed"].instances[character].TotalValue / 100f;
        if (!controller.gamepadMode) {
            if (MovingOntoWater(horizontal * calculatedSpeed, vertical * calculatedSpeed)) {
                CancelMovement();
                return;
            }
            MoveCharacterMaintenance(horizontal != 0 || vertical != 0);
            if (controller.moving) controller.transform.eulerAngles = new Vector3(0, angles[(int)Math.Sign(horizontal) + 1, (int)Math.Sign(vertical) + 1], 0);
            controller.rigidbody.velocity = new Vector3(horizontal * calculatedSpeed, 0, vertical * calculatedSpeed);
        }
        else {
            horizontal = Input.GetAxis("Left Stick Horizontal");
            vertical = Input.GetAxis("Left Stick Vertical");
            if (MovingOntoWater(horizontal * calculatedSpeed, vertical * calculatedSpeed)) {
                CancelMovement();
                return;
            }
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

    private static bool MovingOntoWater(float x, float y) {
        if (SceneInitializer.instance.inside || OverworldGenerator.instance == null) return false;
        var newPosition = controller.transform.position + new Vector3(x, 0, y).normalized * 2f;
        if (newPosition.x < 0 || newPosition.x >= 1024 || newPosition.y < 0 || newPosition.y >= 1024) return true;
        var height = OverworldGenerator.instance.terrain.SampleHeight(newPosition);
        if (height == 0) return true;
        return false;
    }

    private static void CancelMovement() {
        controller.rigidbody.velocity = new Vector3(0, 0, 0);
        MoveCharacterMaintenance(false);
    }

    private static void MoveCharacterWithoutFacing() {
        //var calculatedSpeed = controller.speed * multiplier * SecondaryStatUtility.CalcMoveSpeed(character.dexterity, character.GetComponent<ExperienceGainer>().level);
        var calculatedSpeed = controller.speed * multiplier * CharacterAttribute.attributes["moveSpeed"].instances[character].TotalValue / 100f;
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
        var pointerEventData = new PointerEventData(EventSystem.current) {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        caster.Raycast(pointerEventData, results);
        var whitelistObjects = new List<string>() {
            "Large Status Text",
            "Level Up Text",
            "Minimap",
            "Party Health Pane",
            "Canvas"
        };
        if (results.Count > 0 && !whitelistObjects.Contains(results[0].gameObject.name) && !results[0].gameObject.name.Contains("Minimap")) return true;
        return false;
    }
}
