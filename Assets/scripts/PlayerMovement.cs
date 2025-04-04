using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Movement 
    [SerializeField] private int speed = 2;
    [SerializeField] private int runs = 1;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private CharacterController controller;

    //Animation
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        //Get horizontal and vertical inputs 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //update movement direction
        moveDirection = new Vector3(horizontal,0,vertical);

        //Animations
        if (moveDirection == Vector3.zero)
        {
            //idle
            animator.SetFloat("Speed",0);
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            //walk
            animator.SetFloat("Speed",0.5f);
            moveDirection *= speed;
        }
        else
        {
            //run
            animator.SetFloat("Speed",1);
            moveDirection *= speed + runs;
        }

        //update speed
        

        //Rotation - rotates the character along the x-z plane in the movement direction
        // Calculate the rotation based on the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            // Smoothly interpolate between current rotation and the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        
        //Move character along x-z plane
        controller.Move(moveDirection * Time.deltaTime);

    }
}