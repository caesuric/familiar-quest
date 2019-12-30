using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public static class Dependencies {

    public static void Check(NetworkedDependencyUser component) {
        //foreach (var dependency in component.dependencies) {
        //    if (dependency == "{{PLAYER_OR_MONSTER}}") {
        //        if (component.GetComponent<PlayerCharacter>() == null && component.GetComponent<Monster>() == null) throw new DependencyNotFoundError("PlayerCharacter or Monster");
        //    }
        //    else if (dependency == "{{ANIMATION_OR_ANIMATOR}}") {
        //        if (component.GetComponentInChildren<Animation>() == null && component.GetComponentInChildren<Animator>() == null) throw new DependencyNotFoundError("Animation or Animator");
        //    }
        //    else if (component.GetComponent(dependency) == null) throw new DependencyNotFoundError(dependency);
        //}

        //STUBBING OUT WHILE ON THE MACHINE WITH BROKEN UNITY
    }

    public static void Check(DependencyUser component) {
        foreach (var dependency in component.dependencies) {
            if (dependency == "{{PLAYER_OR_MONSTER}}") {
                if (component.GetComponent<PlayerCharacter>() == null && component.GetComponent<Monster>() == null) throw new DependencyNotFoundError("PlayerCharacter or Monster");
            }
            else if (dependency == "{{ANIMATION_OR_ANIMATOR}}") {
                if (component.GetComponentInChildren<Animation>() == null && component.GetComponentInChildren<Animator>() == null) throw new DependencyNotFoundError("Animation or Animator");
            }
            else if (component.GetComponent(dependency) == null) throw new DependencyNotFoundError(dependency);
        }
    }

    public static void Check(GameObject component, List<string> dependencies) {
        foreach (var dependency in dependencies) {
            if (dependency == "{{PLAYER_OR_MONSTER}}") {
                if (component.GetComponent<PlayerCharacter>() == null && component.GetComponent<Monster>() == null) throw new DependencyNotFoundError("PlayerCharacter or Monster");
            }
            else if (dependency == "{{ANIMATION_OR_ANIMATOR}}") {
                if (component.GetComponentInChildren<Animation>() == null && component.GetComponentInChildren<Animator>() == null) throw new DependencyNotFoundError("Animation or Animator");
            }
            else if (component.GetComponent(dependency) == null) throw new DependencyNotFoundError(dependency);
        }
    }
}

public class NetworkedDependencyUser : MonoBehaviour {
    public List<string> dependencies;
}

public class DependencyUser : MonoBehaviour {
    public List<string> dependencies;
}

class DependencyNotFoundError : Exception {

    public DependencyNotFoundError(string message) : base(message) {
        message = "Dependency Not Found: " + message;
    }
}