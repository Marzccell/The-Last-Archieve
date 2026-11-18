using UnityEngine;
using System.Runtime.InteropServices;

public static class WebBridge
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void WebBridge_SendScore(int score);

    [DllImport("__Internal")]
    private static extern void WebBridge_SendStatus(string status);
#else
    private static void WebBridge_SendScore(int score) =>
        Debug.Log($"Score: {score}");

    private static void WebBridge_SendStatus(string status) =>
        Debug.Log($"Status: {status}");
#endif

    public static void SendScore(int score) => WebBridge_SendScore(score);

    public static void SendStatus(string status) => WebBridge_SendStatus(status);
}
