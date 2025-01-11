using System;
using Core.Components.Sounds;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Interactions
{
    public class InteractableButton : Interactable
    {
        [SerializeField] private string animStateName;
        [SerializeField] private string layerName;
        [SerializeField] private Animator animator;
        [SerializeField] private FMODCustomEmitter emitter;

        private void Start()
        {
            if (!animator)
                animator = GetComponentInParent<Animator>();

            if (!emitter)
                emitter = GetComponent<FMODCustomEmitter>();
        }

        public override void SimpleInteract()
        {
            base.SimpleInteract();

            emitter?.Play();
            if (!string.IsNullOrEmpty(animStateName) && !string.IsNullOrEmpty(layerName))
                animator.Play(animStateName, animator.GetLayerIndex(layerName), 0f);
        }
    }
}