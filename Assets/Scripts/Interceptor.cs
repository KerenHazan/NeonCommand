using UnityEngine;

public class Interceptor : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 12f;
    [SerializeField] private Explosion explosion;

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
            if (explosion != null)
            {
                explosion.Play(targetPosition);
            }
            else
            {
                Debug.LogError("Interceptor cannot create an explosion: assign its Explosion reference.", this);
            }

            gameObject.SetActive(false);
        }
    }
}
