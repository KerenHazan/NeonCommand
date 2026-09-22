using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLauncher : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerBattery;
    [SerializeField] private Interceptor interceptor;
    [SerializeField] private float minimumTargetY = -2.5f;

    private void Update()
    {
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            LaunchAtScreenPosition(Touchscreen.current.primaryTouch.position.ReadValue());
            return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            LaunchAtScreenPosition(Mouse.current.position.ReadValue());
        }
    }

    private void LaunchAtScreenPosition(Vector2 screenPosition)
    {
        if (mainCamera == null || playerBattery == null || interceptor == null ||
            interceptor.gameObject.activeSelf)
        {
            return;
        }

        if (!mainCamera.pixelRect.Contains(screenPosition))
        {
            return;
        }

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, -mainCamera.transform.position.z));
        worldPosition.z = 0f;

        if (worldPosition.y <= minimumTargetY)
        {
            return;
        }

        interceptor.transform.position = playerBattery.position;
        interceptor.Launch(worldPosition);
    }
}
