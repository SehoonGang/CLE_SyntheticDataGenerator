using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Randomizers.Tags;
using UnityEngine.Perception.Randomization.Samplers;

namespace UnityEngine.Perception.Randomization.Randomizers.Tags
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidBodyPlacementRandomizerTag: RandomizerTag 
    {
        private Rigidbody _rb;
        private float _lastY = 0f;
        [SerializeField] private float _newThresholdInput;
        private float _fallThreshold = -0.05f;
        public bool IsFalling = false;
        public Rigidbody RigidBody
        {
            get
            {
                _rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
                return _rb;
            }
        }
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        public void Init()
        {
            _rb.isKinematic = false;
            IsFalling = true;
            _lastY = 0;
        }

        public void SettleRigidBody()
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        public void FreezeRigidBody()
        {
            SettleRigidBody();
            _rb.isKinematic = true;
            IsFalling = false;
        }

        private void FixedUpdate()
        {
            if (_rb.position.y < _fallThreshold)
            {
                AvoidSinking();
            }
            //if (!_rb.isKinematic)
            //{
            //    float distSinceLastFrame = (transform.position.y - _lastY);
            //    _lastY = transform.position.y;

            //    if (distSinceLastFrame <= _fallThreshold)
            //    {
            //        IsFalling = false;
            //    }
            //    else
            //    {
            //        IsFalling = true;
            //    }
            //}
        }

        private void AvoidSinking()
        {
            SettleRigidBody();
            Vector3 newPosition = _rb.position; 
            newPosition.y = 0.5f;
            _rb.MovePosition(newPosition);
        }
    }
}