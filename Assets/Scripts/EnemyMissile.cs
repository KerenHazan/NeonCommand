using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2.5f;
    private City target;

    public void Launch(City targetCity, Vector3 startPosition)
    {
        target = targetCity;
        transform.position = startPosition;
        gameObject.SetActive(true);
    }

    public void DestroyMissile()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (target == null || !target.IsAlive)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            movementSpeed * Time.deltaTime);

        if (transform.position == target.transform.position)
        {
            target.DestroyCity();
            gameObject.SetActive(false);
        }
    }
}
