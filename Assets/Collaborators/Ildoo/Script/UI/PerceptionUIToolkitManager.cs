using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionUIToolkitManager : MonoBehaviour
{
    [SerializeField] Canvas _uiCanvas;
    public bool _isEndOfFrame = false;
    private Coroutine _eofRoutine;
    private void Awake()
    {
        _uiCanvas = GameObject.FindWithTag("UI")?.GetComponent<Canvas>();
        if (_uiCanvas == null )
        {
            Debug.Log("UI CANVAS NOT FOUND"); 
            return;
        }
    }

    private void Update()
    {
        if (!_isEndOfFrame)
        {
            _eofRoutine = StartCoroutine(HideUI(this));
        }
    }
    WaitForEndOfFrame eofWaiter = new WaitForEndOfFrame();
    private IEnumerator HideUI(PerceptionUIToolkitManager manager)
    {
        manager._isEndOfFrame = false;
        yield return eofWaiter;

        foreach (Canvas canvas in _uiCanvas.GetComponentsInChildren<Canvas>())
        {
            canvas.enabled = true;
        }
        manager._isEndOfFrame = true;
    }
}
