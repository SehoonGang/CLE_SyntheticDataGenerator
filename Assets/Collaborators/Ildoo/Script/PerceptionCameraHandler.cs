using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Perception.GroundTruth;
using UnityEngine.Perception.GroundTruth.DataModel;

[RequireComponent(typeof(PerceptionCamera))]
public class PerceptionCameraHandler : MonoBehaviour
{
    private PerceptionCamera _perceptionCamera;
    private PlayerInput _cameraInput;
    [SerializeField] private bool _isAutoMode;
    [SerializeField] private CaptureTriggerMode _triggerMode;
    [SerializeField] [Range(0.5f, 10f)] private float _mouseSensitivity;
    [SerializeField] [Range(1f, 10)] private int _moveSpeed;
    private float _yRotation; 
    private float _xRotation;
    private Vector2 _lookDelta; 
    private Vector3 _moveDir = Vector3.zero;

    private Vector3 _dropValue = new Vector3(0, -1f, 0); 
    private Vector3 _elevateValue = new Vector3(0, 1f, 0); 
    

    [SerializeField] private bool _inputEnabled = false;
    private void Awake()
    {
        _cameraInput = GetComponent<PlayerInput>(); 
        _perceptionCamera = GetComponent<PerceptionCamera>();
        var camComp = GetComponent<Camera>();
        SingletonManager.CaptureManager.Camera = camComp;
    }

    private void OnEnable()
    {
        SingletonManager.CaptureManager.CaptureChangeEvent += CaptureManager_CaptureChangeEvent;
        SingletonManager.CaptureManager.CameraModeChangeEvent += CaptureManager_CameraModeChangeEvent;
    }

    private void CaptureManager_CameraModeChangeEvent(bool isAutoMode)
    {
        _inputEnabled = !isAutoMode;
    }

    private void OnDisable()
    {
        SingletonManager.CaptureManager.CaptureChangeEvent -= CaptureManager_CaptureChangeEvent;
        SingletonManager.CaptureManager.CameraModeChangeEvent -= CaptureManager_CameraModeChangeEvent;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!_inputEnabled) return;

        Look(); 
        Move();
    }

    private void CaptureManager_CaptureChangeEvent(CaptureTriggerMode obj)
    {
        _triggerMode = obj;
        _perceptionCamera.captureTriggerMode = _triggerMode;
    }

    #region Camera Movements
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>(); 
        _moveDir.x = input.x;
        _moveDir.z = input.y;
    }

    private void OnLook(InputValue value)
    {
        _lookDelta = value.Get<Vector2>();
    }

    private void OnElevate(InputValue value)
    {
        if (value.isPressed)
        {
            _moveDir += _elevateValue;
        }
        else
        {
            _moveDir.y = 0;
        }
    }

    private void OnDrop(InputValue value)
    {
        if (value.isPressed)
        {
            _moveDir += _dropValue;
        }
        else
        {
            _moveDir.y = 0;
        }
    }

    private void Look()
    {
        _yRotation += _lookDelta.x * _mouseSensitivity * Time.deltaTime;
        _xRotation -= _lookDelta.y * _mouseSensitivity * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
    }

    private void Move()
    {
        transform.Translate(_moveDir * _moveSpeed * Time.deltaTime, Space.Self);
    }
    #endregion
}