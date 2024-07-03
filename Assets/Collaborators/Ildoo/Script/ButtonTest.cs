using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ButtonTest : MonoBehaviour
{
    // Start is called before the first frame update
    int count = 0;
    TMP_Text tmp_Text;
    void Start()
    {
        
    }

    private void Awake()
    {
        tmp_Text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        Debug.Log("Button has been Clicked");
        tmp_Text.text = $"Button Clicked Count {count++}";
    }
}