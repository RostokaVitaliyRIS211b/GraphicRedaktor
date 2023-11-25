

namespace RealizationOfApp;

public static class SomeFuncs
{

    static SomeFuncs()
    {

    }
    /// <summary>
    /// Функция конвертирующая мировые координаты в экранные
    /// </summary>
    /// <param name="position">
    /// Мировые координаты точки
    /// </param>
    /// <returns></returns>
    public static Vector2f Convert3DTo2D(Vector3f position)
    {
        Vector2f pos = new(0, 0);
        float x2 = MathF.Pow(Application.EyePosition.X, 2),y2 = MathF.Pow(Application.EyePosition.Y, 2),
            z2 = MathF.Pow(Application.EyePosition.Z, 2);
        float distance = MathF.Sqrt(z2 + x2 + y2);
        float angleZ = MathF.Atan(Application.EyePosition.Y/Application.EyePosition.X);
        float angleX = MathF.Atan(MathF.Sqrt(x2+y2)/Application.EyePosition.Z);
        Matrix matrixOfView = new();
        float sinO = MathF.Sin(angleZ), sinF = MathF.Sin(angleX), cosO = MathF.Cos(angleZ), cosF = MathF.Cos(angleX);
        matrixOfView.AddLastString(new float[] { -sinO,-cosF*cosO,sinF*cosO,0 });
        matrixOfView.AddLastString(new float[] { cosO, -cosF*sinO, sinF*sinO, 0 });
        matrixOfView.AddLastString(new float[] { 0, sinF, cosF, 0 });
        matrixOfView.AddLastString(new float[] { 0, 0, -distance, 1 });
        float d = distance;
        Vector3f positionInView = new(position.X * matrixOfView[0, 0] + position.Y*matrixOfView[1,0],
            position.X * matrixOfView[1, 0] + position.Y*matrixOfView[1, 1] + position.Z*matrixOfView[2,1],
            position.X * matrixOfView[2, 0] + position.Y*matrixOfView[1, 2] + position.Z*matrixOfView[2, 2] + matrixOfView[3,2]);
        pos.X = -500 * (positionInView.X/positionInView.Z) + Application.CurrentWidth/2;
        pos.Y = Application.CurrentHeight/2-(-500*(positionInView.Y/positionInView.Z));
        return pos;
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
        else if(key == Key.Comma)
        {
            text+=',';
        }
        else if (key==Keyboard.Key.Backspace && text.Length>0)
        {
            text = text.Remove(text.Length-1, 1);
        }
        return text;
    }
}

