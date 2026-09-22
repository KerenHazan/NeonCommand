using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float maxRadius = 1.8f;
    [SerializeField] private float expandDuration = 0.25f;
    [SerializeField] private float holdDuration = 0.2f;

    public void Play(Vector3 position)
    {
        StopAllCoroutines();
        transform.position = position;
        transform.localScale = Vector3.one * 0.02f;
        gameObject.SetActive(true);
        StartCoroutine(ExplosionSequence());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyMissile missile = other.GetComponent<EnemyMissile>();
        if (missile != null)
        {
            missile.DestroyMissile();
        }
    }

    private IEnumerator ExplosionSequence()
    {
        float elapsed = 0f;
        float diameter = maxRadius * 2f;

        while (elapsed < expandDuration)
        {
            float size = Mathf.SmoothStep(0.02f, diameter, elapsed / expandDuration);
            transform.localScale = Vector3.one * size;
            yield return null;
            elapsed += Time.deltaTime;
        }

        transform.localScale = Vector3.one * diameter;
        yield return new WaitForSeconds(holdDuration);
        gameObject.SetActive(false);
    }
}
