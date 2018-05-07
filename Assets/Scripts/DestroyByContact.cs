using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject PlayerExplosion;

    GameController _gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            _gameController = gameControllerObject.GetComponent<GameController>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.IsBoundary() && !other.IsHazard())
        {
            _gameController.InstantiateExplotion(Explosion, transform.position, transform.rotation);
            if (other.IsPlayer())
            {
                _gameController.InstantiateExplotion(PlayerExplosion, other.transform.position,other.transform.rotation);
                _gameController.PlayerHit();
            }
            else if(other.IsShot())
            {
                Destroy(other.gameObject);
                _gameController.PlayerHitHazard();
            }
            Destroy(this.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.IsBoundary())
        {
            Destroy(this.gameObject);
        }
    }
}
