using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSensitivity =1f;
    public Camera MainCamera;

    private float _rotationX = 0f;
    private float _rotationY = 0f;
    private Transform _rotateTarget;
    private GameObject _factoryScene;

    private bool _isPauseMoving = false;

    private void Start()
    {
        _factoryScene = GameObject.Find("Factory");
        _rotateTarget = MainCamera.transform;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isPauseMoving = !_isPauseMoving;
        }

        if (_isPauseMoving)
        {
            _rotationX = Input.GetAxis("Mouse X") * rotateSensitivity * Time.deltaTime;
            _rotationY = Input.GetAxis("Mouse Y") * rotateSensitivity * Time.deltaTime;

            transform.RotateAround(_rotateTarget.position, transform.up, -_rotationX);
            transform.RotateAround(_rotateTarget.position, Vector3.right, _rotationY);

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float z = 0.0f;

            if (Input.GetKey(KeyCode.E))
            {
                z = -1f;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                z = 1f;
            }

            Vector3 moveDirection = transform.TransformDirection(new Vector3(v, z, -h) * moveSpeed * Time.deltaTime);
            _factoryScene.transform.position += moveDirection;
        }
    }
}