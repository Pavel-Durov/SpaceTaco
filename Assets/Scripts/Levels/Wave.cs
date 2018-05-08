using UnityEngine;
using System.Collections;
using System;

public class Wave
{
    
    public float SpawnDelaySec { get; private set; }
    public int WaveSize { get; private set; }
    public GameObject Hazard { get; private set; }

    private float _xBound;
    private float _yBound;

    public Wave(int waveSize, GameObject hazard, float xBound, float yBound, float spawnDelaySec)
    {
        SpawnDelaySec = spawnDelaySec;
        WaveSize = waveSize;
        Hazard = hazard;
        _xBound = xBound;
        _yBound = yBound;
   }

    public Vector3 GetNextSpawnPosition()
    {
        var halfScreen = _xBound / 2;
        var x = UnityEngine.Random.Range(-halfScreen, halfScreen);
        return new Vector3(x, 0, _yBound / 2);
    }
}
