using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Settings
    {

        static Dictionary<string, KeyCode> Keys = new Dictionary<string, KeyCode>()
        {
            {"RotateLeft", RotateLeft},
            {"RotateRight", RotateRight},
            {"Cast", Cast},
            {"Reel", Reel},
            {"ResetCast", ResetCast},
            {"Catch", Catch},
            {"Shop", Shop},
            {"SettingsPage", SettingsPage},

        };


        public static KeyCode RotateLeft = KeyCode.A;
        public static KeyCode RotateRight = KeyCode.D;
        public static KeyCode Cast = KeyCode.Space;
        public static KeyCode Reel = KeyCode.S;
        public static KeyCode ResetCast = KeyCode.R;
        public static KeyCode Catch = KeyCode.S;
        public static KeyCode Shop = KeyCode.B;
        public static KeyCode SettingsPage = KeyCode.Escape;
        public static float Gravity = .15f;
        public static float AirGravity = .45f;
        public static float ResetSpeed = 2f;
        public static float RotateSpeed = 42.5f;
        public static float SurfaceLevel = -.5f;
        public static bool Paused;
        public static float GetAwayTime = 2;

        public static KeyCode[] GetKeys()
        {
            return Keys.Values.ToArray();
        }

        public static string[] GetKeyNames()
        {
            return Keys.Keys.ToArray();
        }

        public static void SetKeys(KeyCode[] keys)
        {

            RotateLeft = keys[0];
            RotateRight = keys[1];
            Cast = keys[2];
            Reel = keys[3];
            ResetCast = keys[4];
            Catch = keys[5];
            Shop = keys[6];
            SettingsPage = keys[7];

            string[] names = GetKeyNames();
            for(int i = 0; i < names.Length; i++)
            {
                Keys[names[i]] = keys[i];
            }
        }
    }
}
