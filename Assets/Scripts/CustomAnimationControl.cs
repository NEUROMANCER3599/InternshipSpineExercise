using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;
using DG.Tweening;
using System.Collections;

public enum AnimPlayBackType
{
    Set,Add
}
public class CustomAnimationControl : MonoBehaviour
{
    [Header("Parameters")]
    public string IdleAnimationGroupKeyword = "idle";


    [Header("System")]
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    public Spine.SkeletonData skeletonData;
    [Tooltip("Do Not Edit | This list is auto generated")]
    [SpineAnimation] public List<string> AllAnimation;


    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        skeletonData = skeleton.Data;
        InitializeData();
        PlayRandomAnimationFromGroup(AnimPlayBackType.Set, IdleAnimationGroupKeyword, true);
    }

    void InitializeData()
    {
        if (AllAnimation != null)
        {
            AllAnimation.Clear();
        }
        foreach (var skin in skeletonData.Animations)
        {
            AllAnimation.Add(skin.ToString());
        }
    }

    public void PlayRandomAnimationFromGroup(AnimPlayBackType playtype,string groupkeyword,bool SetLoop) //Type Prefix of animation directory
    {
        List<string> FoundAnim = new List<string>();

        foreach (string Anim in AllAnimation)
        {
            if (Anim.StartsWith(groupkeyword))
            {
                FoundAnim.Add(Anim);
            }
        }

        switch (playtype)
        {
            case AnimPlayBackType.Set: spineAnimationState.SetAnimation(0, FoundAnim[Random.Range(0,FoundAnim.Count)], SetLoop); break;
            case AnimPlayBackType.Add: spineAnimationState.AddAnimation(0, FoundAnim[Random.Range(0, FoundAnim.Count)], SetLoop, 0); break;
        }
    }

    
    public void SetIdleAnim()
    {
        PlayRandomAnimationFromGroup(AnimPlayBackType.Set, IdleAnimationGroupKeyword, true);
    }

    public void QueueIdleAnim()
    {
        PlayRandomAnimationFromGroup(AnimPlayBackType.Add, IdleAnimationGroupKeyword, true);
    }
   
}
