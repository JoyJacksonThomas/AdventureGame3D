using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DenominationButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouse_over = false;
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (mouse_over)
        {
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;

        //  we dont want there to be a selected color so this is a hack to set it to the highlighted color
        ColorBlock colors = button.colors;
        colors.selectedColor = button.colors.highlightedColor;
        button.colors = colors;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;

        //  we dont want there to be a selected color so this is a hack to set it to the normal color
        ColorBlock colors = button.colors;
        colors.selectedColor = button.colors.normalColor;
        button.colors = colors;
    }
}

