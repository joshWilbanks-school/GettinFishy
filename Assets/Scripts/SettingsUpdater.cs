using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUpdater : MonoBehaviour
{
    public float Gravity = .15f;
    public float ResetSpeed = .02f;
    public float RotateSpeed = 42.5f;
    public float SurfaceLevel = 0f;


    private void OnValidate()
    {
        Settings.Gravity = Gravity;
        Settings.ResetSpeed = ResetSpeed;
        Settings.RotateSpeed = RotateSpeed;
        Settings.SurfaceLevel = SurfaceLevel;
    }
}
