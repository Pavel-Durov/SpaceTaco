using UnityEngine;
using System.Collections.Generic;

public class Wave
{
    public const int DEFAULT_SPEED = -5;
    public const int DEFAULT_HAZARD_HIT_SCORE = 10;
    public const int DEFAULT_ROTATION = 5;

    IList<Vector3> _positions = null;
    IEnumerator<Vector3> _positionsEnumerator;

    public Wave(int number, SpreadConfig spread, int speed = DEFAULT_SPEED)
    {
        Number = number;
        Spread = spread;
        Speed = speed;
        HazardHitScore = DEFAULT_HAZARD_HIT_SCORE;
        Rotation = DEFAULT_ROTATION;
        SetPositions();
    }

    void SetPositions()
    {
        _positions = WaveSpreadUtil.CalcSpread(Spread);
        _positionsEnumerator = _positions.GetEnumerator();
    }

    public int HazardHitScore { get; private set; }

    public SpreadConfig Spread { get; private set; }

    public int Number { get; private set; }

    public int Speed { get; private set; }

    public int Rotation { get; private set; }

    public int WaveSize
    {
        get
        {
            return Spread.ItemsCount;
        }
    }

    public Vector3? GetNextPosition()
    {
        Vector3? result = null;
        if (_positionsEnumerator.MoveNext())
        {
            result = _positionsEnumerator.Current;
        }
        return result;
    }

    public void SetRotation(GameObject hazard)
    {
        var rigidBody = hazard.GetComponent<Rigidbody>();
        rigidBody.angularVelocity = Random.insideUnitSphere * Rotation;
    }

    public void SetSpeed(GameObject hazard)
    {
        var rb = hazard.GetComponent<Rigidbody>();
        rb.velocity = hazard.transform.forward * Speed;
    }
}
