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
        [SerializeField] private float _lastY = 0f;
        [SerializeField] private float _newThresholdInput;
        private float _fallThreshold = -0.05f;
        public bool IsNearGround = false;
        public Rigidbody RigidBody
        {
            get
            {
                _rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
                return _rb;
            }
        }
        private RigidBodyPlacementRandomizer _randomizer;
        private LayerMask _wallMask;
        private void Awake()
        {
            _wallMask = LayerMask.GetMask("Wall");
            _rb = GetComponent<Rigidbody>();
             _rb.isKinematic = true;
        }
        public void Init(RigidBodyPlacementRandomizer randomizer)
        {
            _rb.isKinematic = false;
            IsNearGround = false;
            _lastY = 0;
            _randomizer = randomizer;
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
            IsNearGround = true;
        }

        private void FixedUpdate()
        {
            if (_rb.isKinematic) return;
            if (_rb.position.y < _fallThreshold)
            {
                AvoidSinking();
            }
            float distSinceLastFrame = (transform.position.y - _lastY);
            _lastY = transform.position.y;

            if (distSinceLastFrame <= 0.05f)
            {
                if (Physics.Raycast(_rb.transform.position, Vector3.down, 0.1f, _wallMask))
                {
                    //Debug.DrawRay(_rb.transform.position, Vector3.down * 0.05f, Color.red, 3f, false);
                    IsNearGround = true;
                }
            }
            else
            {
                IsNearGround = false;
            }
        }

        private void AvoidSinking()
        {
            SettleRigidBody();
            Vector3 newPosition = _randomizer.sampleBoundSize.Sample();
            newPosition.y = 0.5f;
            Debug.Log("Moving Object to Avoid Sinking");
            _rb.MovePosition(newPosition);
        }

        /// <summary>
        /// true = Pause 
        /// false = Resume
        /// </summary>
        /// <param name="isPause"></param>
        public void PauseResume(bool isPause)
        {
            if (isPause)
            {
                _rb.isKinematic = true;
            }
            else // Resume
            {
                _rb.isKinematic = false;
            }
        }
    }
}