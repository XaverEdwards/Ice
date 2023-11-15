using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float moveDistance = 1.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 forwardDirection;


    private bool isMoving = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        moveDistance = 4f;

        forwardDirection = transform.forward; 


        endPosition = transform.position + forwardDirection * moveDistance;

    }

    void Update()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        if (movement != Vector3.zero && !isMoving)
        {
            // Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * 500f);
            forwardDirection = movement;
            transform.forward = forwardDirection;
            RecalculateEndPosition();
            
        }

        if(Input.GetKeyDown(KeyCode.Space) && !isMoving){
            StartCoroutine(MoveObjectCoroutine());
        }
        
       
        animator.SetBool("isMove", isMoving);

        
    }

    IEnumerator MoveObjectCoroutine(){
        isMoving = true;
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPosition, endPosition);
        while(Vector3.Distance(transform.position, endPosition) > 0.01f){
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            if(transform.forward != forwardDirection){
                transform.forward = forwardDirection;


            }


            transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
            yield return null;


        }
        startPosition = transform.position;
        RecalculateEndPosition(); 
        
         isMoving = false;

    }

    void RecalculateEndPosition()
    {
        endPosition = transform.position + forwardDirection * moveDistance;
    }

    
}