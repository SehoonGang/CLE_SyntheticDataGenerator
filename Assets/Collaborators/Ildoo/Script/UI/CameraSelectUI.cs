using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Perception.GroundTruth;
using UnityEngine.Perception.GroundTruth.DataModel;
using UnityEngine.UI;

public class CameraSelectUI : DropDownSelectUI<string>
{
    [SerializeField] Toggle _cameraAutoToggle;
    protected override void Awake()
    {
        base.Awake();
        _cameraAutoToggle = GetComponentInChildren<Toggle>();
    }

    private void Start()
    {
        LoadItems();
        Init(); 
    }

    protected override void Init()
    {
        _dropdown.value = 0;
        _dropdown.Select();
    }

    protected override void SelectItemRequested(int index)
    {
        CaptureTriggerMode mode = (CaptureTriggerMode) index; 
        SingletonManager.CaptureManager.CaptureMode = mode;
    }

    protected override void LoadItems()
    {
        _items = new List<string>()
        {
            CaptureTriggerMode.Scheduled.ToString(),
            CaptureTriggerMode.Manual.ToString()
        };
        _dropdown.ClearOptions();
        _dropdown.AddOptions(_items);
    }

    protected override void PopulateDropdown() { }

    private void OnEnable()
    {
        SingletonManager.CaptureManager.CaptureChangeEvent += CaptureManager_CaptureChangeEvent;
        _cameraAutoToggle?.onValueChanged.AddListener(CameraAutoModeChanged);
    }

    private void OnDisable()
    {
        SingletonManager.CaptureManager.CaptureChangeEvent -= CaptureManager_CaptureChangeEvent;
        _cameraAutoToggle?.onValueChanged.RemoveListener(CameraAutoModeChanged);
    }

    private void CameraAutoModeChanged(bool result)
    {
        SingletonManager.CaptureManager.CameraAutoMode = result;
    }

    private void CaptureManager_CaptureChangeEvent(CaptureTriggerMode triggerMode)
    {
        if (triggerMode == CaptureTriggerMode.Manual)
        {
            ControlToggle(false);
        }
        else
        {
            ControlToggle(true);
        }
    }

    private void ControlToggle(bool isOn)
    {
        if (isOn)
        {
            _cameraAutoToggle.interactable = true;
            _cameraAutoToggle.isOn = true;
        }
        else
        {
            _cameraAutoToggle.isOn = false;
            _cameraAutoToggle.interactable = false;
        }
    }
}