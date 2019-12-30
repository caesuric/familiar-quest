using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InputAbilities : MonoBehaviour {
    private InputController controller;
    private List<Action> abilities;
    private Dictionary<string, Action> buttons = new Dictionary<string, Action>();
    public static Character character;

    public void Initialize(InputController controller) {
        this.controller = controller;
        character = controller.character;
        abilities = new List<Action>() {
            // controller.CmdAttack
        };
        //for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++) abilities.Add(() => controller.CmdUseHotbarAbility(i, j)); // this does not work - all i and j values end up as 3. look for a workaround
        abilities.Add(() => controller.CmdUseHotbarAbility(0, 0));
        abilities.Add(() => controller.CmdUseHotbarAbility(0, 1));
        abilities.Add(() => controller.CmdUseHotbarAbility(0, 2));
        //abilities.Add(() => controller.CmdUseHotbarAbility(1, 0));
        //abilities.Add(() => controller.CmdUseHotbarAbility(1, 1));
        //abilities.Add(() => controller.CmdUseHotbarAbility(1, 2));
        //abilities.Add(() => controller.CmdUseHotbarAbility(2, 0));
        //abilities.Add(() => controller.CmdUseHotbarAbility(2, 1));
        //abilities.Add(() => controller.CmdUseHotbarAbility(2, 2));
        abilities.Add(() => controller.CmdUseHotbarAbility(0, 3));
        abilities.Add(() => controller.CmdUseHotbarAbility(0, 4));
        abilities.Add(() => controller.CmdUseHotbarAbility(0, 5));
        abilities.Add(() => controller.CmdUseHotbarAbility(0, 6));
        abilities.Add(() => controller.CmdUseHotbarAbility(0, 7));
        abilities.Add(() => controller.CmdUseHotbarAbility(0, 8));
        abilities.Add(() => controller.CmdUseHotbarAbility(0, 9));
        //for (int i = 0; i < 2; i++) abilities.Add(() => controller.CmdUseItem(i)); //same issue
        abilities.Add(() => controller.CmdUseItem(0));
        abilities.Add(() => controller.CmdUseItem(1));
        for (int i = 1; i < 10; i++) {
            var key = "Ability " + i.ToString();
            if (!buttons.ContainsKey(key)) buttons.Add(key, abilities[i - 1]);
        }
    }

    public void UseAbility(int number) {
        if (controller.character.GetComponent<AbilityUser>().GCDTime > 0 || controller.character.GetComponent<Health>().hp <= 0) return;
        abilities[number]();
    }

    public void KeyboardCheck() {
        foreach (string button in buttons.Keys) if (Input.GetButtonDown(button)) buttons[button]();
    }

    public void RightStickCheck() {
        if (!controller.gamepadMode) return;
        var horizontal = Input.GetAxis("Right Stick Horizontal");
        var vertical = Input.GetAxis("Right Stick Vertical");
        var direction = new Vector3(horizontal, 0, vertical);
        if (horizontal!=0 || vertical!=0) {
            transform.rotation = Quaternion.LookRotation(direction);
            controller.CmdUsePrimaryAbility(direction);
        }
    }

    public void DPadCheck() {
        if (!controller.gamepadMode) return;
        var horizontal = Input.GetAxis("D-Pad Horizontal");
        var vertical = Input.GetAxis("D-Pad Vertical");
        if (controller.dPadActive && (horizontal!=0 || vertical != 0)) return;
        if (controller.dPadActive && horizontal == 0 && vertical == 0) controller.dPadActive = false;
        if (horizontal<0) {
            controller.dPadActive = true;
            controller.CmdUseItem(0);
        }
        else if (horizontal>0) {
            controller.dPadActive = true;
            controller.CmdUseItem(1);
        }
        if (vertical>0) {
            controller.dPadActive = true;
            controller.minimapActive = !controller.minimapActive;
            controller.minimap.SetActive(controller.minimapActive);
        }
    }
}
