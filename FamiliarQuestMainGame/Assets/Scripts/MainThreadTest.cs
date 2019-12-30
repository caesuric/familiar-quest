using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class MainThreadTest : MonoBehaviour
{
    public static Thread mainThread = null;
    // Start is called before the first frame update
    void Start()
    {
        mainThread = Thread.CurrentThread;
    }

    public static bool OnMainThread() {
        return (mainThread == Thread.CurrentThread);
    }
}
