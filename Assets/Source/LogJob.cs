using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public struct LogJob : IJob
{
    public int Number;
    public void Execute()
    {
        Debug.Log(Mathf.Log(Number));
    }
}
