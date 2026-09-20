using UnityEngine;

public class Interceptor : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 12f;

    private Vector3 targetPosition;
    private bool isMoving;

    public void Launch(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        isMoving = true;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isMoving)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            movementSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMoving = false;
            gameObject.SetActive(false);
        }
    }
}
