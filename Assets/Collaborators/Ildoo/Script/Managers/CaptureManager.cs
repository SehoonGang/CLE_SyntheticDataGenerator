using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Perception.GroundTruth;
using UnityEngine.Perception.GroundTruth.DataModel;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Randomizers.Tags;


public class CaptureManager : MonoBehaviour
{
    [SerializeField] private bool _cameraAutoMode;
    [SerializeField] private bool _simulationPaused;
    [SerializeField] private Camera _camera;
    [SerializeField] private PerceptionCamera _perceptionCamera;
    [SerializeField] private Vector3 _boundSize = Vector3.one;
    [SerializeField] private Vector3 _boundCentreOffset = Vector3.zero;
    [SerializeField] private Vector3 _renderBoundCentreOffset = Vector3.zero;
    [SerializeField] private Rigidbody _captureBody;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private Coroutine _cameraRoutine;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private CaptureTriggerMode _captureMode = CaptureTriggerMode.Scheduled;
    
    /// <summary>
    /// Event is Triggered during IterationStart, and ScenarioEnd, and Whenever Scenario is Paused;
    /// <see cref="CameraPlacementRandomizer"
    /// <see cref=""/>/>
    /// </summary>
    public event Action<CaptureTriggerMode> CaptureChangeEvent;
    public event Action<bool> CameraModeChangeEvent;
    public event Action<bool> PauseResumeEvent;
    public event Action Initialize;
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

    public bool CameraAutoMode
    {
        get => _cameraAutoMode;
        set
        {
            if (_cameraAutoMode != value)
            {
                CameraModeChangeEvent?.Invoke(value);
            }
            _cameraAutoMode = value;
        }
    }

    public bool SimulationPaused
    {
        get => _simulationPaused;
        set
        {
            if (_simulationPaused != value)
            {
                PauseResumeEvent?.Invoke(value);
            }
            _simulationPaused = value;
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
    public Vector3 BoundCentreOffset => _boundCentreOffset;
    public Vector3 RenderBoundCentreOffset => _renderBoundCentreOffset;

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
        Init();
    }

    public void Init()
    {
        _cameraAutoMode = true; 
        _captureMode = CaptureTriggerMode.Scheduled;
        _simulationPaused = false;
        _boundSize = Vector3.one;
        _boundCentreOffset = Vector3.zero;
        _renderBoundCentreOffset = Vector3.zero;
        CaptureChangeEvent?.Invoke(_captureMode);
        CameraModeChangeEvent?.Invoke(_cameraAutoMode);
    }

    public void SetForCapture(RigidBodyPlacementRandomizerTag rigidBody, float camDist)
    {
        if (_perceptionCamera.captureTriggerMode == CaptureTriggerMode.Manual)
        {
            if (_cameraAutoMode)
            {
                _cameraRoutine = StartCoroutine(CaptureItem(rigidBody, camDist));
            }
        }
        else
        {
                    
            if (_cameraAutoMode)
            {
                _cameraRoutine = StartCoroutine(FollowItem(rigidBody, camDist));
            }
        }
    }

    public void ResetCamera()
    {
        StopAllCoroutines();
    }

    public void SetBound(GameObject floorObj)
    {
        if (floorObj == null)
        {
            _boundSize = Vector3.one * 5f;
        }
        else
        {
            Vector3 localScale = floorObj.transform.localScale;
            Vector3 objBoundSize = floorObj.GetComponent<Renderer>().bounds.size;
            Vector3 objBoundCentre = floorObj.GetComponent<Renderer>().localBounds.center;
            _boundSize.x = objBoundSize.x;
            _boundSize.z = objBoundSize.z;
            if (objBoundSize.y <= 0.5)
            {
                _boundSize.y = 3f;
            }

            _renderBoundCentreOffset = objBoundCentre;
            _boundCentreOffset = floorObj.transform.position;
        }
    }

    private void SetForCapture(Vector3 newPosition, Vector3 targetPosition)
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
            if (newCamTransform.y < 0) newCamTransform.y = Math.Abs(newCamTransform.y);
            hasWallInbetween = CheckWallInBetween(newCamTransform, rigidBodyTag.transform.position);
            Debug.Log(hasWallInbetween);
            Debug.Log($"{hasWallInbetween}");
            
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, newCamTransform, 0.2f);
            _camera.transform.LookAt(rigidBodyTag.transform.position);
            yield return null;
        }
        SetForCapture(newCamTransform, rigidBodyTag.transform.position);
    }

    private IEnumerator FollowItem(RigidBodyPlacementRandomizerTag rigidBodyTag, float camDist)
    {
        //yield return new WaitUntil(() => rigidBodyTag.IsNearGround);
        yield return waitInterval;
        rigidBodyTag.SettleRigidBody(); 
        yield return null;
        Vector3 newCamTransform = Vector3.one;
        bool hasWallInbetween = true;
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
                Debug.Log($"Failed To Get correct Angle on {rigidBodyTag.gameObject.name}");
                hasWallInbetween = false;
            }
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, newCamTransform, 0.2f);
            yield return null;
        }
        _camera.transform.position = newCamTransform;
        _camera.transform.LookAt(rigidBodyTag.transform.position);
    }

    private bool CheckWallInBetween(Vector3 camPos, Vector3 targetPos)
    {
        Vector3 lookDir = (targetPos - camPos).normalized;
        Debug.DrawRay(camPos, lookDir, Color.red, 2f);

        if (Physics.Raycast(camPos, lookDir, out RaycastHit hitInfo))
        {
            return _wallLayer.Contain(hitInfo.transform.gameObject.layer); 
        }
        else
        {
            return false;
        }
    }
}