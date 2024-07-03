using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Perception.GroundTruth;
using UnityEngine.Perception.GroundTruth.DataModel;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Randomizers.Tags;


public class CaptureManager : MonoBehaviour
{
    [SerializeField] private bool _isAutoMode;
    [SerializeField] private Camera _camera;
    [SerializeField] private PerceptionCamera? _perceptionCamera;
    [SerializeField] private Vector3 _boundSize = Vector3.one;
    [SerializeField] private Rigidbody _captureBody;
    [SerializeField] private LayerMask _wallLayer;

    /// <summary>
    /// Event is Triggered during IterationStart, and ScenarioEnd, and Whenever Scenario is Paused;
    /// <see cref="CameraPlacementRandomizer"
    /// <see cref=""/>/>
    /// </summary>
    public event Action<CaptureTriggerMode> CaptureChangeEvent;
    private CaptureTriggerMode _captureMode; 
    public CaptureTriggerMode CaptureMode
    {
        get => _captureMode;
        set
        {
            if (value != _captureMode)
            {
                CaptureChangeEvent?.Invoke(value);
            }
            _captureMode = value;
        }
    }

    public PerceptionCamera PerceptionCamera
    {
        get => _perceptionCamera;
        set => _perceptionCamera = value;
    }

    public Camera Camera
    {
        get => _camera;
        set
        {
            if (_perceptionCamera == null)
            {
                _perceptionCamera = value.GetComponent<PerceptionCamera>();
            }
            _camera = value;
        }
    }

    public Vector3 BoundSize => _boundSize;
    public float MaxHeight
    {
        set => _boundSize.y = value;
    }

    private void Awake()
    {
        _wallLayer = LayerMask.GetMask("Wall");
    }

    private void Start()
    {
        //StartCoroutine(captureTimer());
    }

    public void SetForCapture(RigidBodyPlacementRandomizerTag rigidBody, float camDist)
    {
        if (_isAutoMode)
        {
            StartCoroutine(CaptureItem(rigidBody, camDist));
        }
    }

    public void Init(GameObject floorObj)
    {
        if (floorObj == null)
        {
            _boundSize = Vector3.one * 5f;
        }
        else
        {
            _boundSize = floorObj.GetComponent<Renderer>().bounds.size;
            _boundSize.y = 3f;
        }
        _isAutoMode = true;
    }

    private void Capture(Vector3 newPosition, Vector3 targetPosition)
    {
        _camera.transform.position = newPosition;
        _camera.transform.LookAt(targetPosition);
        _perceptionCamera.RequestCapture();
    }

    WaitForSeconds waitInterval = new WaitForSeconds(1.5f);
    private IEnumerator CaptureItem(RigidBodyPlacementRandomizerTag rigidBodyTag, float camDist)
    {
        yield return waitInterval;
        yield return null;
        rigidBodyTag.SettleRigidBody();
        yield return null;

        bool hasWallInbetween = true;
        Vector3 newCamTransform = Vector3.one;
        while (hasWallInbetween)
        {
            newCamTransform = UnityEngine.Random.onUnitSphere * camDist + rigidBodyTag.transform.position;
            Vector3 lookDir = (rigidBodyTag.transform.position - newCamTransform).normalized;
            if (Physics.Raycast(newCamTransform, lookDir, out RaycastHit hitInfo))
            {
                hasWallInbetween = _wallLayer.Contain(hitInfo.transform.gameObject.layer); 
            }
            else
            {
                hasWallInbetween = false; 
            }
            Debug.Log(hasWallInbetween);
            Debug.DrawRay(newCamTransform, lookDir, Color.red);
            Debug.Log($"{hasWallInbetween}");
        }
            
        Capture(newCamTransform, rigidBodyTag.transform.position);
    }
}