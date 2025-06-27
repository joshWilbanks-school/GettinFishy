using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeyBind : MonoBehaviour
{
    [SerializeField] KeyCode key;
    [SerializeField] TMP_InputField input;
    [SerializeField] bool pressed;
    [SerializeField] Button button;

    private void Start()
    {
        button.onClick.AddListener(() => { pressed = true; });
        input.text = key.ToString();
    }

    private void Update()
    {
        if (!pressed)
            return;


        input.text = "";

        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(code))
            {

                key = code;
                input.text = key.ToString();
                input.DeactivateInputField();
                pressed = false;
                break;
            }
        }
    }

    public KeyCode GetKey()
    {
        return key;
    }
}
