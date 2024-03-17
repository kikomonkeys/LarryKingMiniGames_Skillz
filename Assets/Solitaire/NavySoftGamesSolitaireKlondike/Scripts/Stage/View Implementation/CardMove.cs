using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class CardMove : MonoBehaviour
{
    public Transform followContTransform;
    class MovingObj
    {
        public int id;
        public Transform target;
        public Transform destination;
        public Vector3 velocity = Vector3.zero;
        public Vector3 offset = Vector3.zero;
        public UnityAction onFinish;
    }
    private MovingObj mov;
    private bool isUpdate = false;

    private Transform followTransform;
    private float followSpeed = 0.05f;
    private Vector3 followVelocity = Vector3.zero;
    private Vector3 startPosition;

    private enum MoveType { None,Clicked,Drag };
    private MoveType moveType;
    public void Move(int id, Transform selectedCard, Transform destinationCard, Vector2 offset, UnityAction onFight)
    {
      

        mov = new MovingObj();
        mov.id = id;
        mov.target = selectedCard;
        mov.destination = destinationCard;
        mov.offset = offset;
        mov.onFinish = onFight;

        isUpdate = true;
        startPosition = transform.position;
        moveType = MoveType.Clicked;
    }

 

    public void MoveDragCard( bool move)
    {
        this.moveType = (move) ? MoveType.Drag : MoveType.None;
    }

    private void Update()
    {
        switch (moveType)
        {
            case MoveType.Drag:
                transform.position = Input.mousePosition;
                break;
            case MoveType.Clicked:
                Vector3 destinationPosition = mov.destination.transform.TransformPoint(mov.offset);
                transform.position = Vector3.SmoothDamp(transform.position, destinationPosition, ref mov.velocity, SmoothMovementManager.instance.GetSmoothSpeed);
                bool distanse_near_destination = Vector3.Distance(transform.position, destinationPosition) < SmoothMovementManager.instance.GetDistanceMove;
                if (distanse_near_destination)
                {
                    transform.position = destinationPosition;
                    TriggerFinish(mov);
                }
                break;

        }
     
    
        
    }
    private void TriggerFinish(MovingObj mo)
    {
        mo.onFinish();
        moveType = MoveType.None;
        
    }
}
