using UnityEngine;

public enum Direction 
{
    Up = 0,
    Left = 90,
    Down = 180,
    Right = 270
}

public enum AnimDirect
{
    Up = 1,
    Left = 2,
    Down = 3,
    Right = 4
}

public class MovementController : MonoBehaviour
{
    public static Direction MyDirection { get; private set; } = Direction.Right;

    [SerializeField] private float speed = 8f;
    [SerializeField] private Animator animator;

    private bool isWalk = false;

    public void DoStep()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"),  Input.GetAxis("Vertical"), 0);

        if(!Mathf.Approximately(direction.magnitude, 0))
        {
            transform.position += speed * Time.deltaTime * direction;

            float angle = Vector2.SignedAngle(Vector3.right, direction);
            if(angle <= 45 && angle >= -45) 
            {
                MyDirection = Direction.Right;
                animator.SetInteger("State", (int)AnimDirect.Right);
            }
            else if (angle < 135 && angle > 45)
            {             
                MyDirection = Direction.Up;
                animator.SetInteger("State", (int)AnimDirect.Up);
            }
            else if (angle < -45 && angle > -135)
            {             
                MyDirection = Direction.Down;
                animator.SetInteger("State", (int)AnimDirect.Down);
            }
            else 
            {
                MyDirection = Direction.Left;
                animator.SetInteger("State", (int)AnimDirect.Left);
            }
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }     
        animator.SetBool("IsWalk", isWalk);
    }

    public static Quaternion GetDirection()
    {
        return Quaternion.Euler(0, 0, (float) MyDirection);
    }
}
