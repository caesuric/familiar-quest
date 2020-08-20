using System;

public class RNG {
    private static Random random = new Random();

    public static int Int(int min, int max) {
        if (MainThreadTest.OnMainThread()) return UnityEngine.Random.Range(min, max);
        else return random.Next(min, max);
    }

    public static float Float(float min, float max) {
        if (MainThreadTest.OnMainThread()) return UnityEngine.Random.Range(min, max);
        else return (float)((random.NextDouble() * (max - min)) + min);
    }

    public static bool Bool() {
        if (MainThreadTest.OnMainThread()) return UnityEngine.Random.Range(0, 2) == 0;
        else return random.Next(0, 2) == 0;
    }
    
    public static T EnumValue<T>() where T:struct,IConvertible {
        if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type.");
        var values = Enum.GetValues(typeof(T));
        if (MainThreadTest.OnMainThread()) return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        else return (T)values.GetValue(random.Next(values.Length));
    }
}
