using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}

