using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2.5f;
    private City target;
    private EnemySpawner spawner;
    private bool isResolved;

    public void Launch(City targetCity, Vector3 startPosition, EnemySpawner enemySpawner)
    {
        target = targetCity;
        spawner = enemySpawner;
        isResolved = false;
        transform.position = startPosition;
        gameObject.SetActive(true);
    }

    public void DestroyMissile()
    {
        if (isResolved)
        {
            return;
        }

        isResolved = true;
        if (spawner != null)
        {
            spawner.NotifyMissileResolved(this);
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (target == null || !target.IsAlive)
        {
            DestroyMissile();
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            movementSpeed * Time.deltaTime);

        if (transform.position == target.transform.position)
        {
            target.DestroyCity();
            DestroyMissile();
        }
    }
}
