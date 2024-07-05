using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers.Tags;
using UnityEngine.Perception.Randomization.Samplers;
using UnityEngine.Scripting.APIUpdating;
namespace UnityEngine.Perception.Randomization.Randomizers
{
    /// <summary>
    /// Allocates new random placement for the toggled foreground object
    /// </summary>
    [Serializable]
    [AddRandomizerMenu("Perception/RigidBody Placement Randomizer")]
    public class RigidBodyPlacementRandomizer: Randomizer
    {
        [SerializeField] private Vector3 _boundSize;
        [Header("Bound to be null by each iteration")]
        [SerializeField] private List<RigidBodyPlacementRandomizerTag>? _rigidTags;
        public Vector3Parameter sampleBoundSize = new Vector3Parameter
        {
            x = new UniformSampler(0, 360),
            y = new UniformSampler(0, 360),
            z = new UniformSampler(0, 360)
        };

        protected override void OnEnable()
        {
            SingletonManager.CaptureManager.PauseResumeEvent += PauseResume;
        }

        protected override void OnDisable()
        {
            SingletonManager.CaptureManager.PauseResumeEvent -= PauseResume;
        }

        protected override void OnScenarioStart()
        {
            _boundSize = SingletonManager.CaptureManager.BoundSize;
            Vector3 boundCentreOffset = SingletonManager.CaptureManager.BoundCentreOffset;
            Vector3 renderBoundCentreOffset = SingletonManager.CaptureManager.RenderBoundCentreOffset;
            SetBoundSampler(_boundSize, boundCentreOffset, renderBoundCentreOffset);
        }

        public void SetBoundSampler(Vector3 bound, Vector3 boundCentreOffset, Vector3 renderBoundCentreOffset)
        {
            Debug.Log($"Bound Limits SET: {bound}");
            sampleBoundSize = new Vector3Parameter
            {
                x = new UniformSampler(boundCentreOffset.x - bound.x / 2 + renderBoundCentreOffset.x, 
                boundCentreOffset.x + bound.x / 2 + renderBoundCentreOffset.x),
                z = new UniformSampler(boundCentreOffset.z - bound.z / 2 + renderBoundCentreOffset.z, 
                boundCentreOffset.z + bound.z / 2 + renderBoundCentreOffset.z),

                y = new UniformSampler(1.5f, _boundSize.y) // TODO: MAke Flexible Y coordinate changes
            };
        }

        protected override void OnIterationStart()
        {
            Debug.Log("New Iteration On RigidBody");
            _rigidTags = tagManager.Query<RigidBodyPlacementRandomizerTag>().ToList();
            foreach (var tag in _rigidTags)
            {
                var newVectorVal = sampleBoundSize.Sample();
                tag.RigidBody.MovePosition(newVectorVal);
                tag.Init(this);
            }   
        }

        protected override void OnIterationEnd()
        {
            foreach (var tag in _rigidTags)
            {
                tag.SettleRigidBody();
            }
            _rigidTags = null;
        }

        private void PauseResume(bool isPause)
        {
            if (_rigidTags == null) return;
            if (isPause)
            {
                foreach (var tag in _rigidTags)
                {
                    tag.PauseResume(true);
                }
            }
            else
            {
                foreach (var tag in _rigidTags)
                {
                    tag.PauseResume(false);
                }
            }
        }
    }
}