using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;
using DG.Tweening;
public class ActorBehavior : MonoBehaviour
{
    [Header("Movement Control")]
    [Tooltip("Set to 0 to disable Movement")]
    public float MoveSpeed = 1f;
    float movementInterval;
    bool IsMoving;

    [Header("Animation Parameters")]
    public string MovementAnimGroupKeyword;
    public string InteractedAnimGroupKeyword;

    [Header("System")]
    [SerializeField] private CustomAnimationControl AnimControl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void InitializeData()
    {
        AnimControl = GetComponent<CustomAnimationControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementInterval > 0)
        {
            movementInterval -= 1f * Time.deltaTime;
        }
        else
        {
            if (IsMoving)
            {
                IsMoving = false;
                AnimControl.SetIdleAnim();
            }
        }
    }

    public void MoveOnClick(Vector2 clickedposition)
    {
        if (MoveSpeed != 0)
        {
            Debug.Log("Moving " + gameObject.name + " to position: " + clickedposition);
            float distance = Vector2.Distance(clickedposition, transform.position);
            transform.DOMove(new Vector3(clickedposition.x, clickedposition.y), distance * MoveSpeed);
            movementInterval = distance * MoveSpeed;
            IsMoving = true;
            AnimControl.PlayRandomAnimationFromGroup(AnimPlayBackType.Set, MovementAnimGroupKeyword, true);

            if (clickedposition.x > transform.position.x)
            {
                AnimControl.skeleton.ScaleX = 1;
            }
            else
            {
                AnimControl.skeleton.ScaleX = -1;
            }
        }

    }


    public void OnClicked()
    {
        Debug.Log(gameObject.name + " Selected");

        int RandomGroup = Random.Range(0, 2);

        AnimControl.PlayRandomAnimationFromGroup(AnimPlayBackType.Set, InteractedAnimGroupKeyword, false);
        AnimControl.QueueIdleAnim();

    }

    public void ChangeSpeed(float newspeed)
    {
        MoveSpeed = newspeed;
    }    

    
}
