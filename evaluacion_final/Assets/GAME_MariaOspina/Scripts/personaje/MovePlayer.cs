using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    public float speedplayer = 5.0f;
    public float speedRotation = 200f;

    private float x;
    private float y;

    private Vector2 movementInput;

    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        x = movementInput.x;
        y = movementInput.y;

        transform.Rotate(0, x * speedRotation * Time.deltaTime, 0);
        transform.Translate(0, 0, y * speedplayer * Time.deltaTime);

        if (animator != null)
        {
            animator.SetFloat("VelX", x);
            animator.SetFloat("VelY", y);

            animator.SetFloat("Blend", movementInput.magnitude);


            if (Keyboard.current.mKey.wasPressedThisFrame)
            {
                animator.SetTrigger("Dance");
            }

        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}
