using UnityEngine;

public static class CursorController
{
    public static void EnableCursor(bool enable) {
        if (enable) {
            ShowCursor();
        } else {
            HideCursor();
        }
    }

    private static void ShowCursor() {
        Cursor.visible = true;  
        Cursor.lockState = CursorLockMode.None;  
    }

    private static void HideCursor() {
        Cursor.visible = false;  
        Cursor.lockState = CursorLockMode.Locked;  
    }
}
