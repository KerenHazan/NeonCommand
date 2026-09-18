using UnityEngine;

public class City : MonoBehaviour
{
    public bool IsAlive { get; private set; } = true;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DestroyCity()
    {
        if (!IsAlive)
        {
            return;
        }

        IsAlive = false;
        spriteRenderer.enabled = false;
    }

    public void ResetCity()
    {
        IsAlive = true;
        spriteRenderer.enabled = true;
    }
}
