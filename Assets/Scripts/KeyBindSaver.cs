using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindSaver : MonoBehaviour
{

    [SerializeField] KeyCode close = KeyCode.Escape;
    [SerializeField] KeyBind closeKey;
    [SerializeField] GameObject settingsObj;

    private void Update()
    {

        if(closeKey.gameObject.activeSelf)
            close = closeKey.GetKey();

        if(Input.GetKeyDown(close) && !StoreManager.instance.IsOpen())
        {

            settingsObj.SetActive(!settingsObj.activeSelf);
            if (settingsObj.activeSelf)
            {

                Time.timeScale = 0;
                Settings.Paused = true;
            }
            else
            {
                Time.timeScale = 1;
                SaveKeys();
                Settings.Paused = false;
            }
        }
    }

    void SaveKeys()
    {
        KeyBind[] binds = settingsObj.GetComponentsInChildren<KeyBind>();
        KeyCode[] keys = new KeyCode[binds.Length];
        for(int i = 0; i < binds.Length; i++)
        {
            keys[i] = binds[i].GetKey();
        }

        Settings.SetKeys(keys);
        Tutorial.instance.UpdateText();
    }
}
