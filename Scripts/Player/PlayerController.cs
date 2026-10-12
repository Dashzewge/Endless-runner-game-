using System.Collections;
using UnityEngine;
//using TMPro;


public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    private bool isSliding = false;

    [Header("Movement")]
    public float forwardSpeed = 10f;
    public float laneDistance = 4f;
    public float laneChangeSpeed = 25f;
    public float maxSpeed;

    [Header("Jump")]
    public float jumpForce = 10f;
    public float Gravity = -20f;

    [Header("Animation")]
    public Animator animator;

    private int desiredLane = 1;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (controller == null)
            return;
        //increase speed
        if(forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }
        forwardSpeed += 0.1f * Time.deltaTime;

        // Тоглоом эхлээгүй эсвэл дууссан үед
        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
        {
            direction = Vector3.zero;

            if (animator != null)
            {
                animator.SetBool("isGameStarted", false);
                animator.SetBool("isGrounded", true);
            }

            return;
        }

        // Газарт байгаа эсэх
        isGrounded = controller.isGrounded;

        // Animation
        if (animator != null)
        {
            animator.SetBool("isGameStarted", true);
            animator.SetBool("isGrounded", isGrounded);
        }

        // Урагшлах хөдөлгөөн
        direction.z = forwardSpeed;

        // Үсрэлт ба таталцал
        if (isGrounded)
        {
            if (direction.y < 0)
                direction.y = -1f;

            if (SwipeManager.swipeUp)
            {
                Jump();
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }

        if(SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }
        // Баруун тийш эгнээ солих
        if (SwipeManager.swipeRight)
        {
            desiredLane = Mathf.Min(desiredLane + 1, 2);
        }

        // Зүүн тийш эгнээ солих
        if (SwipeManager.swipeLeft)
        {
            desiredLane = Mathf.Max(desiredLane - 1, 0);
        }

        // Зорилтот эгнээний байрлал
        Vector3 targetPosition =
            transform.position.z * transform.forward +
            transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        // Эгнээ хооронд шилжих
        Vector3 diff = targetPosition - transform.position;

        if (diff.sqrMagnitude > 0.001f)
        {
            Vector3 moveDir =
                diff.normalized * laneChangeSpeed * Time.deltaTime;

            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }
    }

    void FixedUpdate()
    {
        if (controller == null)
            return;

        if (!PlayerManager.isGameStarted || PlayerManager.gameOver)
            return;

        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("GAME OVER! Саад мөргөв.");

            PlayerManager.gameOver = true;
        }
    }
    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;
        yield return new WaitForSeconds(1.3f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }
}