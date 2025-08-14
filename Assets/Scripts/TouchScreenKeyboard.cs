using UnityEngine;

public class TouchScreenKeyboard
{
    public static bool hideInput;

    public static bool visible;

    public bool done;

    public bool active;

    public string text;

    public static UnityEngine.TouchScreenKeyboard Open(string text, TouchScreenKeyboardType t, bool autoCorrection, bool multiline, bool secure, bool alert, string caption)
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            TouchScreenKeyboard.hideInput = true;
            return UnityEngine.TouchScreenKeyboard.Open(text, (UnityEngine.TouchScreenKeyboardType)(int)t, autoCorrection, multiline, secure, alert, caption);
        }
        return null;
    }

    public static void Clear()
    {
        visible = false;
    }
}
