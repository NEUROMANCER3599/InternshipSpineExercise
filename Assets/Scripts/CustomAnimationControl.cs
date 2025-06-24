using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;
using DG.Tweening;
using System.Collections;


public class CustomAnimationControl : MonoBehaviour
{
    [Header("Animations List")]
    [SpineAnimation] public List<string> IdleAnimation;
    [SpineAnimation] public List<string> WalkAnimation;
    [SpineAnimation] public List<string> InteractAnimation;

    [Header("Movement Control")]
    public float MoveSpeed = 1f;
    float movementInterval;
    bool IsMoving;

    [Header("System")]
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;



    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;

        spineAnimationState.SetAnimation(0, IdleAnimation[Random.Range(0,IdleAnimation.Count)],true);
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
                spineAnimationState.AddAnimation(0, IdleAnimation[Random.Range(0, IdleAnimation.Count)], true, 0);
            }
        }
    }

    public void MoveOnClick(Vector2 clickedposition)
    {
            Debug.Log("Moving " + gameObject.name + " to position: " + clickedposition);
            float distance = Vector2.Distance(clickedposition, transform.position);
            transform.DOMove(new Vector3(clickedposition.x, clickedposition.y), distance * MoveSpeed);
            movementInterval = distance * MoveSpeed;
            IsMoving = true;
            spineAnimationState.SetAnimation(0, WalkAnimation[Random.Range(0, WalkAnimation.Count)], true);

            if(clickedposition.x > transform.position.x)
            {
                skeleton.ScaleX = 1;
            }
            else
            {
                skeleton.ScaleX = -1;
            }
    }


    public void OnClicked()
    {
        Debug.Log(gameObject.name + " Selected");

        int RandomGroup = Random.Range(0, 2);

        spineAnimationState.SetAnimation(0, InteractAnimation[Random.Range(0, InteractAnimation.Count)], false);

        spineAnimationState.AddAnimation(0, IdleAnimation[Random.Range(0, IdleAnimation.Count)], true, 0);

    }
}
