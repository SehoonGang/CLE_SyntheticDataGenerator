using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DropDownBackgroundHandler : MonoBehaviour
{
    public int SCENE_INDEX = 0;
    private TMP_Dropdown _dropdown;
    public CustomScenario _scenario;

    void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        _dropdown.onValueChanged.AddListener(OnValueChanged);
        ScenarioManagerHandler.Instance._sceneName = _dropdown.options[SCENE_INDEX].text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged(int index)
    {
        ScenarioManagerHandler.Instance._sceneName = _dropdown.options[index].text;
    }
}
