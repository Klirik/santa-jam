using UnityEngine;

public class MovementController : MonoBehaviour
{        
    public enum Direction 
    {
        Right = 0,
        Up = 90,
        Left = 180,
        Down = 270
    }

    public Direction MyDirection = Direction.Right;

    [SerializeField] private float speed = 8f;

    public void DoStep()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"),  Input.GetAxis("Vertical"), 0);

        if(direction.magnitude != 0)
        {
            transform.position += speed * Time.deltaTime * direction;

            float angle = Vector2.SignedAngle(Vector3.right, direction);
            if(angle <= 45 && angle >= -45)
            {
                transform.rotation = Quaternion.Euler(0, 0, (float)Direction.Right);
                MyDirection = Direction.Right;
            }
            else if (angle < 135 && angle > 45)
            {
                transform.rotation = Quaternion.Euler(0, 0, (float)Direction.Up);
                MyDirection = Direction.Up;
            }
            else if (angle < -45 && angle > -135)
            {
                transform.rotation = Quaternion.Euler(0, 0, (float)Direction.Down);
                MyDirection = Direction.Down;
            }
            else 
            {
                transform.rotation = Quaternion.Euler(0, 0, (float)Direction.Left);
                MyDirection = Direction.Left;
            }
        }
        
    }


}
