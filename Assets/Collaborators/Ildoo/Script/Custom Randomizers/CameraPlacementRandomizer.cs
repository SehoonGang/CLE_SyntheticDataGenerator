using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers.Tags;
using Unity.VisualScripting;
using UnityEngine;

namespace UnityEngine.Perception.Randomization.Randomizers
{
    [Serializable]
    [AddRandomizerMenu("Perception/Camera Placement Randomizer")]
    public class CameraPlacementRandomizer : Randomizer
    {
        [Header("Minimum range set to 0.1 equivalent to Camera's near plane value")]
        private float _cameraMinDist = 1f;
        [SerializeField] [Range(0.1f, 5f)] private float _cameraMaxDist;
        public float CameraMaxDist
        {
            set
            {
                if (value < 0.1f) value = 0.1f; 
                _cameraMaxDist = value;
            }
        }

        protected override void OnAwake()
        {
            var placementObjects = tagManager.Query<RigidBodyPlacementRandomizerTag>().ToList();
        }
        protected override void OnIterationStart()
        {
            var rigidBodies = tagManager.Query<RigidBodyPlacementRandomizerTag>().ToList();
            int randomIndex = Random.Range(0, rigidBodies.Count);
            float camDist = Random.Range(_cameraMinDist, _cameraMaxDist);
            SingletonManager.CaptureManager.SetForCapture(rigidBodies[randomIndex], camDist);
        }
    }
}