using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTutorial : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Color show, dontShow;
    [SerializeField] bool isShowing = true;


    public void Toggle()
    {
        Tutorial.instance.ToggleAll();
        isShowing = !isShowing;
        if(isShowing)
            image.color = show;
        else image.color = dontShow;


    }

}
