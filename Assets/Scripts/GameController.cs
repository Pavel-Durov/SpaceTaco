using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public AudioClip HazardAudioClip;
    public AudioClip GameOverAudioClip;
    public AudioClip PlayerDeadAudioClip;

    public GameObject Boundary;
    public GameObject Player;
    public GameObject Hazard;

    public int LifeCount = 3;
    public int CurrentLife;
    public int Score;
    public int HazardCount;

    public float WaveWait;

    public Text ScoreText;
    public Text GameOverText;
    public Text LifeText;
    public Text WaveCountText;

    public bool IsGameOver;
    public bool IsGameDone;

    public Button RestartBtn;

    public int HazardHitScoreValue;

    int _waveCount = 0;

    List<GameObject> _hazards = new List<GameObject>();
    List<GameObject> _explosions = new List<GameObject>();

    AudioSource _hazardAudioSource;
    AudioSource _gameOverAudioSource;
    AudioSource _playerDeadAudioSource;

    void SetAudioSources()
    {
        _hazardAudioSource = gameObject.AddComponent<AudioSource>();
        _hazardAudioSource.clip = HazardAudioClip;

        _gameOverAudioSource = gameObject.AddComponent<AudioSource>();
        _gameOverAudioSource.clip = GameOverAudioClip;

        _playerDeadAudioSource = gameObject.AddComponent<AudioSource>();
        _playerDeadAudioSource.clip = PlayerDeadAudioClip;
    }

    void Start()
    {
        LevelManager.Init(Hazard, Boundary.transform.localScale.x, Boundary.transform.localScale.z);

        SetAudioSources();

        WaveCountText.gameObject.SetActive(false);

        if (DevicePlatformUtil.IsMobile)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        RestartBtn.gameObject.SetActive(false);
        CurrentLife = LifeCount;
        IsGameOver = false;
        GameOverText.text = "";

        SetLifeText();

        Score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    public void InstantiateExplotion(GameObject explosion, Vector3 position, Quaternion rotattion)
    {
        var exp = Instantiate(explosion, position, rotattion);
        _explosions.Add(exp);
    }

    void ClearExplosions()
    {
        foreach (var exp in _explosions)
        {
            Destroy(exp);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    bool isWaveCompeted()
    {
        
           return _hazards.TrueForAll((hazard) => hazard == null);

    }

    IEnumerator SpawnWaves()
    {
        while (!IsGameOver && !IsGameDone)
        {
            yield return new WaitUntil(isWaveCompeted);
            if (isWaveCompeted())
            {
                yield return SpreadWaveHazards();
            }
            else
            {
                yield return new WaitForSeconds(WaveWait);
            }
        }
        RestartBtn.gameObject.SetActive(true);
        if(IsGameDone)
        {
            GameDone();
        }
    }

    IEnumerator SpreadWaveHazards()
    {
        foreach (var wave in LevelManager.CurrentLevel.Waves)
        {
            ClearExplosions();
            LevelManager.CurrentLevel.CurrentWave = wave;
            yield return new WaitUntil(isWaveCompeted);

            _hazards.Clear();
            _waveCount++;
           
            WaveCountText.gameObject.SetActive(true);
            UpdateWaveCount();
            yield return new WaitForSeconds(wave.SpawnDelaySec);

            yield return new WaitForSeconds(WaveWait);
            if (IsGameOver)
            {
                break;
            }
            WaveCountText.gameObject.SetActive(false);

            HazardCount = HazardCount * _waveCount;

            for (int i = 0; i < wave.WaveSize; i++)
            {
                var hazard = InstantiateHazard(wave);
                if(hazard != null)
                {
					_hazards.Add(InstantiateHazard(wave));
                }
            }
            yield return new WaitForSeconds(wave.SpawnDelaySec);
            if (IsGameOver)
            {
                break;
            }
        }
        ClearExplosions();
        yield return new WaitUntil(isWaveCompeted);

        LevelManager.LevelEnd();

        if(LevelManager.CurrentLevel == null)
        {
            IsGameDone = true;
        }
    }

    GameObject InstantiateHazard(Wave wave)
    {
        GameObject instance = null;
        Vector3 position;
        if(wave.NextSpawnPosition(out position)){
            instance = Instantiate(wave.Spread.GameObj, position , Quaternion.identity);
            var mover = instance.GetComponentInChildren<Mover>();
            mover.Speed = wave.Speed;
        }
        return instance;
    }

    public void PlayerHitHazard()
    {
        _hazardAudioSource.Play();

        Score += HazardHitScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        ScoreText.text = "Score: " + Score;
    }

    void UpdateWaveCount()
    {

        var msg = "Level " + LevelManager.CurrentLevel.Number;
        msg += " -  Wave " + LevelManager.CurrentLevel.CurrentWave.Number;
        WaveCountText.text = msg;
    }

    public void PlayerHit()
    {
        --CurrentLife;
        if (CurrentLife == 0)
        {
            GameOver();
        }
        SetLifeText();
        _playerDeadAudioSource.Play();
    }

    void SetLifeText()
    {
        var sb = new StringBuilder().Insert(0, "♡", CurrentLife);
        LifeText.text = sb.ToString();
    }

    public void GameOver()
    {
        _gameOverAudioSource.Play();
        Destroy(Player);
        GameOverText.text = "Game Over";
        IsGameOver = true;
    }

    public void GameDone()
    {
        Destroy(Player);
        GameOverText.text = "Bravo";
    }
}