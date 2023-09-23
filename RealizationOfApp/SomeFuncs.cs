

namespace RealizationOfApp;

public static class SomeFuncs
{

    static SomeFuncs()
    {

    }
    public static string ConvertToString(string text, Keyboard.Key key)
    {
        if (key>=Keyboard.Key.A && key<=Keyboard.Key.Z)
        {
            int add = IsKeyPressed(Key.LShift) ? 0 : 32;
            text+=(char)(key + 65 + add);
        }
        else if (key>=Keyboard.Key.Num0 && key<=Keyboard.Key.Num9)
        {
            text+=(char)(key + 22);
        }
        else if (key==Keyboard.Key.Backspace && text.Length>0)
        {
            text = text.Remove(text.Length-1, 1);
        }
        return text;
    }
}

