using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float PLAYER_BOARDER_MARGIN = 1f;

    public GameObject Boundary;
    public GameObject Shot;
    public Transform ShotSpawn;
    public float Acceleration;
    public float FireRate;

    private float _nextFireTime;
    private Rigidbody _rigidBody;
    private FireButton _fireButton;
    private Joystick _joystick;

    void Start()
    {
        _fireButton = FindObjectOfType<FireButton>();
        _joystick = FindObjectOfType<Joystick>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_fireButton.IsPressed && Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + FireRate;
            InstantiateFire();
        }
    }

    private void InstantiateFire()
    {
        Instantiate(Shot, ShotSpawn.position, ShotSpawn.rotation);
        GetComponent<AudioSource>().Play();
    }

    private float PositionX
    {
        get
        {
            var boundaryWidth = Boundary.transform.localScale.x - Boundary.transform.position.x;
            var xBound = boundaryWidth / 2 - PLAYER_BOARDER_MARGIN;
            return Mathf.Clamp(_rigidBody.position.x, -xBound, xBound);
        }
    }

    private float PositionZ
    {
        get
        {
            float boundaryHeight = Boundary.transform.localScale.z - Boundary.transform.position.z;
            float zBound = boundaryHeight / 2 - PLAYER_BOARDER_MARGIN;
            return Mathf.Clamp(_rigidBody.position.z, -zBound, zBound);
        }
    }

    void FixedUpdate()
    {
        Vector3 move = UserInputUtil.GetPlayerMove();
        if (DevicePlatformUtil.IsMobile)
        {
            move = UserInputUtil.GetPlayerJoystickMove(_joystick);
        }
        _rigidBody.velocity = move * Acceleration;
        _rigidBody.position = new Vector3(PositionX, 0, PositionZ);
    }
}