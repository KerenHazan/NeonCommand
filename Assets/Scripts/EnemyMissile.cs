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
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.simulated = true;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        body.position = startPosition;
        body.rotation = 0f;
        gameObject.SetActive(true);
    }

    public bool DestroyMissile()
    {
        if (isResolved || !gameObject.activeInHierarchy)
        {
            return false;
        }

        isResolved = true;
        EnemySpawner owner = spawner;
        target = null;
        spawner = null;
        gameObject.SetActive(false);
        if (owner != null)
        {
            owner.NotifyMissileResolved(this);
        }
        return true;
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
