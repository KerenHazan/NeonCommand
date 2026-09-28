using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float maxRadius = 1.8f;
    [SerializeField] private float expandDuration = 0.25f;
    [SerializeField] private float holdDuration = 0.2f;
    [SerializeField] private float chainBlastScale = 0.65f;

    private readonly List<Explosion> extraBlasts = new List<Explosion>();
    private Explosion poolOwner;
    private int chainGeneration;

    public void Play(Vector3 position)
    {
        RequestBlast(position, 0);
    }

    private void RequestBlast(Vector3 position, int generation)
    {
        Explosion owner = poolOwner != null ? poolOwner : this;
        Explosion blast = !owner.gameObject.activeSelf ? owner : null;
        if (blast == null)
        {
            foreach (Explosion candidate in owner.extraBlasts)
            {
                if (!candidate.gameObject.activeSelf)
                {
                    blast = candidate;
                    break;
                }
            }
        }
        if (blast == null)
        {
            blast = Instantiate(owner, owner.transform.parent);
            blast.name = "PooledExplosion";
            blast.gameObject.SetActive(false);
            blast.poolOwner = owner;
            owner.extraBlasts.Add(blast);
        }

        blast.chainGeneration = generation;
        blast.transform.position = position;
        blast.transform.localScale = Vector3.one * 0.02f;
        blast.gameObject.SetActive(true);
        blast.StartCoroutine(blast.ExplosionSequence());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameManager == null || gameManager.CurrentState != GameState.Playing)
        {
            return;
        }

        EnemyMissile missile = other.GetComponent<EnemyMissile>();
        if (missile == null) return;

        Vector3 position = missile.transform.position;
        if (missile.DestroyMissile())
        {
            gameManager.AddMissileDestroyedScore(chainGeneration);
            // Physics processes the new blast on a later step, not recursively here.
            RequestBlast(position, chainGeneration + 1);
        }
    }

    private IEnumerator ExplosionSequence()
    {
        float elapsed = 0f;
        float diameter = maxRadius * 2f * (chainGeneration == 0 ? 1f : chainBlastScale);

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
