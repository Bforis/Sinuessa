using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class InputFieldUtilities : MonoBehaviour
{
    InputField _inputField;
    // Start is called before the first frame update
    void Start()
    {
        _inputField = GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Gamepad.current.bButton.wasPressedThisFrame)
        {
            _inputField.DeactivateInputField();
        }
    }
}
