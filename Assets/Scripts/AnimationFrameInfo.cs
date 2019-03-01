using System;

[Serializable]
public class AnimationFrameInfo
{
    public string Name;
    public int StartFrame;
    public int EndFrame;
    public int FrameCount;
    public bool IsLoop;
    public AnimationFrameInfo(string name, int startFrame, int endFrame, int frameCount, bool isLoop)
    {
        Name = name;
        StartFrame = startFrame;
        EndFrame = endFrame;
        FrameCount = frameCount;
        IsLoop = isLoop;
    }
}