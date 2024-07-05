using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    protected Dictionary<string, RectTransform> transforms; 
    protected Dictionary<string, Button> buttons; 
    protected Dictionary<string, TMP_Text> texts; 

    protected virtual void Awake()
    {
        BindChilren();
    }

    private void BindChilren()
    {
        transforms = new Dictionary<string, RectTransform>();
        buttons = new Dictionary<string, Button>();
        texts = new Dictionary<string, TMP_Text>();

        RectTransform[] children = GetComponentsInChildren<RectTransform>();
        foreach(RectTransform child in children)
        {
            string key = child.name;
            if (transforms.ContainsKey(key)) continue;

            transforms.Add(key, child);

            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                buttons.Add(key, button);
            }

            TMP_Text text = child.GetComponent<TMP_Text>();
            if (text != null)
            {
                texts.Add(key, text);
            }
        }
    }
}