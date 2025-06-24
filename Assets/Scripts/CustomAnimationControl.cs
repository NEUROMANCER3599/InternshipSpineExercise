using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;
using DG.Tweening;
using System.Collections;


public class CustomAnimationControl : MonoBehaviour
{
    [Header("Parameters")]
    public string IdleAnimationGroupKeyword = "idle";
    public string InteractionAnimationGroupKeyword = "interacted";
    public string MoveAnimationGroupKeyword = "walk";

    [Header("Movement Control")]
    [Tooltip("Set to 0 to disable Movement")]
    public float MoveSpeed = 1f;
    float movementInterval;
    bool IsMoving;

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
        spineAnimationState.SetAnimation(0,PlayRandomAnimationFromGroup(IdleAnimationGroupKeyword),true);
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

    string PlayRandomAnimationFromGroup(string groupkeyword ) //Type Prefix of animation directory
    {
        List<string> FoundAnim = new List<string>();

        foreach (string Anim in AllAnimation)
        {
            if (Anim.StartsWith(groupkeyword))
            {
                FoundAnim.Add(Anim);
            }
        }

        if(FoundAnim == null)
        {
            FoundAnim = null;
        }

        return FoundAnim[Random.Range(0, FoundAnim.Count)];
    }

    void Update()
    {
        if(movementInterval > 0)
        {
            movementInterval -= 1f * Time.deltaTime;
        }
        else
        {
            if (IsMoving)
            {
                IsMoving = false;
                spineAnimationState.AddAnimation(0, PlayRandomAnimationFromGroup(IdleAnimationGroupKeyword), true, 0);
            }
        }
    }

    public void MoveOnClick(Vector2 clickedposition)
    {
            if(MoveSpeed != 0)
            {
                Debug.Log("Moving " + gameObject.name + " to position: " + clickedposition);
                float distance = Vector2.Distance(clickedposition, transform.position);
                transform.DOMove(new Vector3(clickedposition.x, clickedposition.y), distance * MoveSpeed);
                movementInterval = distance * MoveSpeed;
                IsMoving = true;
                spineAnimationState.SetAnimation(0, PlayRandomAnimationFromGroup(MoveAnimationGroupKeyword), true);

                if (clickedposition.x > transform.position.x)
                {
                    skeleton.ScaleX = 1;
                }
                else
                {
                    skeleton.ScaleX = -1;
                }
            }
           
    }


    public void OnClicked()
    {
        Debug.Log(gameObject.name + " Selected");

        int RandomGroup = Random.Range(0, 2);

        spineAnimationState.SetAnimation(0, PlayRandomAnimationFromGroup(InteractionAnimationGroupKeyword), false);
        spineAnimationState.AddAnimation(0, PlayRandomAnimationFromGroup(IdleAnimationGroupKeyword), true,0);

    }
}
