using System;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Interactions
{
    public class InteractableKnob : MonoBehaviour
    {
        [Header("Interactables")]
        [SerializeField] private InteractableButton clickInteractable;
        [SerializeField] private Interactable leftInteractable;
        [SerializeField] private Interactable rightInteractable;

        [Header("Knob Settings")] [SerializeField]
        private int knobSteps = 12;

        [SerializeField] private int defaultKnobPosition = 0;
        [SerializeField] private string knobAnimParam;
        [SerializeField] private Animator animator;

        // [Header("Events")]
        // [SerializeField] private UnityEvent onKnobLeft;
        // [SerializeField] private UnityEvent onKnobRight;
        // [SerializeField] private UnityEvent knobClicked;
        
        private int _currentKnobPosition;

        private void Start()
        {
            if (animator == null)
                animator = GetComponentInParent<Animator>();
            
            _currentKnobPosition = defaultKnobPosition;
            leftInteractable.interact.AddListener(KnobLeft);
            rightInteractable.interact.AddListener(KnobRight);
        }

        private void KnobLeft()
        {
            _currentKnobPosition--;
            if (_currentKnobPosition < 0)
                _currentKnobPosition = knobSteps + _currentKnobPosition;
            UpdateKnobAnim();
        }

        private void KnobRight()
        {
            _currentKnobPosition++;
            if (_currentKnobPosition > knobSteps)
                _currentKnobPosition -= knobSteps;
            UpdateKnobAnim();
        }

        private void UpdateKnobAnim()
        {
            animator?.SetFloat(knobAnimParam, _currentKnobPosition / (float)knobSteps);
        }
    }
}