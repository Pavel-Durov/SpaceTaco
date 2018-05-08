using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Wave
{

    public float SpawnDelaySec { get; private set; }

    public int WaveSize
    {
        get
        {
            return Spread.ItemsCount;
        }
    }

    public SpreadConfig Spread { get; private set; }
    public int Number { get; private set; }
    LinkedList<Vector3> _positions = new LinkedList<Vector3>();
    LinkedListNode<Vector3> _positon;

    const int DEFAULT_SPEED = -5;
    public int Speed { get; private set; }

    public Wave(int number, float spawnDelaySec, SpreadConfig spread, int speed = DEFAULT_SPEED)
    {
        Number = number;
        SpawnDelaySec = spawnDelaySec;
        Spread = spread;
        SetPositions();
        Speed = speed;
    }

    void SetPositions()
    {
        _positions = WaveSpreadUtil.SpreadEvenRows(Spread);
    }

    public bool NextSpawnPosition(out Vector3 position)
    {
        bool sucess = false;
        if (_positon == null)
        {
            _positon = _positions.First;
            sucess = true;
        }
        else
        {
            if (_positon.Next != null)
            {
                _positon = _positon.Next;
                sucess = true;
            }
        }

        position = _positon.Value;
        return sucess;
    }
}
