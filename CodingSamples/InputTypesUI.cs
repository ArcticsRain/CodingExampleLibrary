using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputTypesUI : MonoBehaviour {
    private PlayerActions inputs;
    [SerializeField] GameObject primaryButton;
    [SerializeField] EventSystem uiEvents;
    [SerializeField] List<Image> gamepadImage = new List<Image> ();
    [SerializeField] List<Image> keyboardImages = new List<Image> ();
    private bool uiInputActive;
    private enum inputState {
        mouseKeyboard,
        Gamepad
    }

    private inputState currentInputState;

    private void Awake () {
        inputs = new PlayerActions ();
        inputs.Enable ();
        inputs.inputSwitch.Gamepad.performed += x => SwitchUIInput (inputState.mouseKeyboard);
        inputs.inputSwitch.Mouse.performed += x => SwitchUIInput (inputState.Gamepad);
    }

    private void OnDisable () {
        inputs.Disable ();
    }

    private void SwitchUIInput (inputState incomingState) {
        currentInputState = incomingState;

        switch (currentInputState) {
            case inputState.Gamepad:
                if (!uiInputActive) {
                    uiInputActive = !uiInputActive;
                    uiEvents.SetSelectedGameObject (primaryButton, new BaseEventData (uiEvents));
                    UpdateUI (gamepadImage);
                    Debug.Log ($"GAMEPAD UI");
                    // Switch Input
                }
                break;
            case inputState.mouseKeyboard:
                if (uiInputActive) {
                    uiInputActive = !uiInputActive;
                    uiEvents.SetSelectedGameObject (null, new BaseEventData (uiEvents));
                    UpdateUI (keyboardImages);
                    Debug.Log ($"MOUSE UI");
                    // Switch Input
                }
                break;
        }
    }

    private void UpdateUI (List<Image> incomingList) {

    }
}