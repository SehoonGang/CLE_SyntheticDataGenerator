using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PerceptionUIToolkitManager : MonoBehaviour
{
    [SerializeField] Canvas _uiCanvas;
    public bool _isEndOfFrame = false;
    private Coroutine _eofRoutine;
    private void Awake()
    {
        _uiCanvas = GameObject.FindWithTag("UI").GetComponent<Canvas>();
        if (_uiCanvas == null )
        {
            Debug.Log("UI CANVAS NOT FOUND"); 
            return;
        }
        RenderPipelineManager.beginFrameRendering += RenderPipelineManager_beginFrameRendering;
    }

    private void RenderPipelineManager_beginFrameRendering(ScriptableRenderContext arg1, Camera[] arg2)
    {
        if (this == null) return;
        _isEndOfFrame = false; 
        _eofRoutine = StartCoroutine(HideUI(this));
    }

    private void Update()
    {
        //if (!_isEndOfFrame)
        //{
        //    _eofRoutine = StartCoroutine(HideUI(this));
        //}
        //_uiCanvas.enabled = false; 
    }
    WaitForEndOfFrame eofWaiter = new WaitForEndOfFrame();
    private IEnumerator HideUI(PerceptionUIToolkitManager manager)
    {
        yield return eofWaiter;

        _uiCanvas.enabled = true; 
        manager._isEndOfFrame = true;
    }
}
