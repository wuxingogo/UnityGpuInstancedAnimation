
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wuxingogo.Runtime;

#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

public class CustomAnimator : XMonoBehaviour {

    [System.Serializable]
    public class CustomState
    {
        public string name = string.Empty;
        public string nextName = string.Empty;

    }
    public CustomState[] totalStates;
    public CustomState currentState = null;
    [System.NonSerialized]
    public CustomState nextState = null;
    public string defaultState;
    #if UNITY_EDITOR
    public Animator animator = null;
    #endif
    public AnimatedMeshAnimator animationInstancing = null;
    private void Awake()
    {
        SetNextState(defaultState);
    }
    [X]
    public void SetNextState(string stateName)
    {
        if(string.IsNullOrEmpty(stateName) == false)
        {
            for (int i = 0; i < totalStates.Length; i++)
            {
                var state = totalStates[i];
                if (state.name.Equals(stateName))
                {
                    nextState = state;
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update () {
		if(nextState != null)
        {
            currentState = nextState;
            animationInstancing.Play(currentState.name, 0);
            nextState = null;
        }
        if (animationInstancing.percent >= 1)
        {
            SetNextState(currentState.nextName);
        }
    }
    [X]
    public void GetStateFromAnimator()
    {
#if UNITY_EDITOR
        var runtimeController = animator.runtimeAnimatorController;
        if (runtimeController == null)
        {
            Debug.LogErrorFormat("RuntimeAnimatorController must not be null.");
            return;
        }

        var controller = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>(UnityEditor.AssetDatabase.GetAssetPath(runtimeController));
        if (controller == null)
        {
            Debug.LogErrorFormat("AnimatorController must not be null.");
            return;
        }
        
        var stateMachine = controller.layers[0].stateMachine;
        var states = stateMachine.states;
        if(stateMachine.defaultState != null && stateMachine.defaultState.motion != null)
            defaultState = stateMachine.defaultState.motion.name;
        totalStates = new CustomState[states.Length];
        for (int i = 0; i < states.Length; i++)
        {
            var state = states[i];
            if(state.state == null || state.state.motion == null)
                continue;
            
            var transitions = state.state.transitions;
            
           var newState = new CustomState();
            newState.name = state.state.motion.name;

            if (transitions.Length > 0)
            {
                var nextState = transitions[0].destinationState;
                newState.nextName = nextState.motion.name;
            }
            totalStates[i] = newState;
        }
#endif
    }
}
