using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Boundary;
    public GameObject Shot;
    public Transform ShotSpawn;
    public float Acceleration;
    public float FireRate;

    const float PLAYER_BOARDER_MARGIN = 1f;
    float _nextFireTime;
    Rigidbody _rigidBody;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var isUserFire = UserInputUtil.IsScreenTapped || UserInputUtil.IsMouseRightClick;
        if (isUserFire && Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + FireRate;
            InstantiateFire();
        }
    }

    void InstantiateFire()
    {
        Instantiate(Shot, ShotSpawn.position, ShotSpawn.rotation);
        GetComponent<AudioSource>().Play();
    }

    float GetPositionX()
    {
        var boundaryWidth = Boundary.transform.localScale.x - Boundary.transform.position.x;
        var xBound = boundaryWidth / 2 - PLAYER_BOARDER_MARGIN;
        return Mathf.Clamp(_rigidBody.position.x, -xBound, xBound);
    }

    float GetPositionZ()
    {
        float boundaryHeight = Boundary.transform.localScale.z - Boundary.transform.position.z;
        float zBound = boundaryHeight / 2 - PLAYER_BOARDER_MARGIN;
        return Mathf.Clamp(_rigidBody.position.z, -zBound, zBound);
    }

    void FixedUpdate()
    {
        _rigidBody.velocity = UserInputUtil.GetPlayerMove() * Acceleration;
        _rigidBody.position = new Vector3(GetPositionX(), 0, GetPositionZ());
    }
}