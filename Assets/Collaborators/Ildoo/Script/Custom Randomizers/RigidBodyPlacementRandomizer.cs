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
        protected override void OnScenarioStart()
        {
            _boundSize = SingletonManager.CaptureManager.BoundSize;
            SetBoundSampler(_boundSize);
        }

        public void SetBoundSampler(Vector3 bound)
        {
            Debug.Log($"Bound Limits SET: {bound}");
            sampleBoundSize = new Vector3Parameter
            {
                x = new UniformSampler(0, _boundSize.x), 
                y = new UniformSampler(1.5f, _boundSize.y), 
                z = new UniformSampler(0, _boundSize.z)
            };
        }

        protected override void OnIterationStart()
        {
            Debug.Log("New Iteration On RigidBody");
            _rigidTags = tagManager.Query<RigidBodyPlacementRandomizerTag>().ToList();
            foreach (var tag in _rigidTags)
            {
                var newVectorVal = sampleBoundSize.Sample();
                Debug.Log(newVectorVal);
                tag.RigidBody.MovePosition(newVectorVal);
                tag.Init();
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
    }
}