using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour {

	[SerializeField]
    AnimatedMeshAnimator BodyMeshAnimator;
	 public string animName = "";
	 public bool isLoop = false;
    [ContextMenu("PlayAnimation")]
    public void PlayCurrent(){
        
        BodyMeshAnimator.Play(animName, 0);
    }
}
