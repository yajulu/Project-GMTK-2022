using System;
using UnityEngine;

namespace Essentials
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorEventTrigger : MonoBehaviour
    {
        private Animator attachedAnimator;

        private AnimatorStateInfo currentState;

        private bool started;

        private Action currentCompleteCallBack;

        public Animator AttachedAnimator => attachedAnimator;

        private int currentStateHash;

        private void Awake()
        {
            attachedAnimator = GetComponent<Animator>();
        }
        
        private void Update()
        {
            // Debug.Log(attachedAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            CheckAnimationState();
        }
        
        public void PlayAnimation(int stateHash, Action onCompleteCallBack)
        {
            currentCompleteCallBack = onCompleteCallBack;
            currentStateHash = stateHash;
            attachedAnimator.Play(stateHash, 0, 0);
            currentState = attachedAnimator.GetCurrentAnimatorStateInfo(0);
            started = true;
        }
        
        private void CheckAnimationState()
        {
            if (started)
            {
                var stateInfo = attachedAnimator.GetCurrentAnimatorStateInfo(0);
                if (attachedAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash == currentStateHash)
                {
                    currentState = stateInfo;
                }
                
                if (currentState.shortNameHash == currentStateHash && currentState.normalizedTime >= 0.9f)
                {
                    started = false;
                    currentCompleteCallBack?.Invoke();
                }
            }
        }
    }
}
