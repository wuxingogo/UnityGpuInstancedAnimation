using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AnimatedMeshAnimator : MonoBehaviour
{
    [SerializeField]
    List<AnimationFrameInfo> FrameInformations;
    [SerializeField]
    MaterialPropertyBlockController PropertyBlockController;

    public bool IsPlaying { get; private set; }
    public AnimationFrameInfo currentState = null;

    public float totalTime = 0;
    public float deltaTime = 0;

    public int currentFrame = 0;
    public bool isLoop = false;
    public void Setup(List<AnimationFrameInfo> frameInformations, MaterialPropertyBlockController propertyBlockController)
    {
        FrameInformations = frameInformations;
        PropertyBlockController = propertyBlockController;
    }
   
    public void Play(string animationName, float offsetSeconds)
    {
        var frameInformation = FrameInformations.First(x => x.Name == animationName);
        currentState = frameInformation;
        isLoop = currentState.IsLoop;
        deltaTime = 0;
        PropertyBlockController.SetFloat("_OffsetSeconds", offsetSeconds);
        PropertyBlockController.SetFloat("_StartFrame", frameInformation.StartFrame);
        PropertyBlockController.SetFloat("_EndFrame", frameInformation.EndFrame);
        PropertyBlockController.SetFloat("_FrameCount", frameInformation.FrameCount);
        
        PropertyBlockController.Apply();

        IsPlaying = true;
    }

    public AnimationFrameInfo GetAnimationInfo(string animationName){
        var frameInformation = FrameInformations.First(x => x.Name == animationName);
        return frameInformation;
    }

    public void Stop()
    {
        PropertyBlockController.SetFloat("_StartFrame", 0);
        PropertyBlockController.SetFloat("_EndFrame", 0);
        PropertyBlockController.SetFloat("_FrameCount", 1);
        
        PropertyBlockController.Apply();

        IsPlaying = false;
    }
    void Update(){
        if(IsPlaying == false)
            return;
        deltaTime += Time.deltaTime;
        totalTime = Time.timeSinceLevelLoad;
        int offsetFrame = (int)((deltaTime) * 30);
        var startFrame = currentState.StartFrame;
        var frameCount = currentState.FrameCount;
        if(isLoop)
		    currentFrame = currentState.StartFrame + offsetFrame % frameCount;
		else
			currentFrame = Mathf.Min(startFrame + frameCount - 1, startFrame + offsetFrame);			
        PropertyBlockController.SetFloat("_CurrentFrame", currentFrame);
        PropertyBlockController.Apply();
    }
    
    public float percent{
        get{
            var startFrame = currentState.StartFrame;
            var frameCount = currentState.FrameCount;
            return currentFrame * 1.0f / (startFrame + frameCount - 1);
        }
    }
}