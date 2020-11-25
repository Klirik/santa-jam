using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Range(0.01f,1f)]
    [SerializeField] private float speed = 1f;

    //TODO: Поворот персонажа лево-право вверх-вниз
    private void Update()
    {
        //motion controll of object 
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * speed;
        }
    }
}
