using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerLauncher : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerBattery;
    [SerializeField] private Interceptor interceptor;
    [SerializeField] private float minimumTargetY = -2.5f;
    private readonly List<RaycastResult> uiHits = new List<RaycastResult>();

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
        if (gameManager == null || !gameManager.CanFire || mainCamera == null || playerBattery == null || interceptor == null ||
            interceptor.gameObject.activeSelf)
        {
            return;
        }

        if (!mainCamera.pixelRect.Contains(screenPosition) || IsOverUI(screenPosition))
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

        if (!gameManager.TryUseAmmo())
        {
            return;
        }

        interceptor.transform.position = playerBattery.position;
        interceptor.Launch(worldPosition);
    }

    private bool IsOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
        {
            return false;
        }

        // Raycast this press's position, avoiding stale pointer state in Update.
        var pointer = new PointerEventData(EventSystem.current) { position = screenPosition };
        uiHits.Clear();
        EventSystem.current.RaycastAll(pointer, uiHits);
        foreach (RaycastResult hit in uiHits)
        {
            if (hit.module is GraphicRaycaster)
            {
                return true;
            }
        }

        return false;
    }
}
