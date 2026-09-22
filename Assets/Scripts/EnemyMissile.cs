using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2.5f;
    [SerializeField] private City target;

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
