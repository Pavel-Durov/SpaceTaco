using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public float WaveWait;

    public Text ScoreText;
    public Text GameOverText;
    public Text LifeText;
    public Text WaveCountText;

    public bool IsGameOver;
    public bool IsGameDone;

    public Button RestartBtn;

    private List<GameObject> _hazards = new List<GameObject>();
    private List<GameObject> _explosions = new List<GameObject>();

    private AudioSource _hazardAudioSource;
    private AudioSource _gameOverAudioSource;
    private AudioSource _playerDeadAudioSource;
    private Wave _currentWave;

    public void PlayerHitHazard()
    {
        _hazardAudioSource.Play();
        Score += _currentWave.HazardHitScore;
        UpdateScore();
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


    public void InstantiateExplotion(GameObject explosion, Vector3 position, Quaternion rotattion)
    {
        var exp = Instantiate(explosion, position, rotattion);
        _explosions.Add(exp);
    }

    void Start()
    {
        Levels.Init(Hazard, Boundary.transform.localScale.x, Boundary.transform.localScale.z);

        SetAudioSources();
        DevicePlatformUtil.SetScreenOrientation();

        WaveCountText.gameObject.SetActive(false);
        RestartBtn.gameObject.SetActive(false);

        CurrentLife = LifeCount;
        IsGameOver = false;
        GameOverText.text = "";

        SetLifeText();

        Score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    private void SetAudioSources()
    {
        _hazardAudioSource = gameObject.AddComponent<AudioSource>();
        _hazardAudioSource.clip = HazardAudioClip;

        _gameOverAudioSource = gameObject.AddComponent<AudioSource>();
        _gameOverAudioSource.clip = GameOverAudioClip;

        _playerDeadAudioSource = gameObject.AddComponent<AudioSource>();
        _playerDeadAudioSource.clip = PlayerDeadAudioClip;
    }

    private void ClearExplosions()
    {
        _explosions.ForEach(exp => Destroy(exp));
    }

    public void Restart()
    {
        SceneManager.LoadScene(Scenes.MAIN_SCENE);
    }


    private bool GameIsOn
    {
        get
        {
            return !IsGameOver && !IsGameDone;
        }
    }

    private bool isWaveCompeted()
    {
        return _hazards.TrueForAll((hazard) => hazard == null);
    }

    private IEnumerator SpawnWaves()
    {
        while (GameIsOn)
        {
            var level = Levels.NextLevel();
            while (level != null)
            {
                yield return SpreadWaveHazards(level);
                level = Levels.NextLevel();
            }
            IsGameDone = true;
        }
        GameDone();
    }

    private IEnumerator SpreadWaveHazards(Level level)
    {
        foreach (var wave in level.Waves)
        {
            _currentWave = wave;
            yield return ShowWaveCountTextBetweenWaves(level, wave);

            foreach (var step in Enumerable.Range(0, wave.WaveSize))
            {
                var hazard = InstantiateHazard(wave);
                if (hazard != null)
                {
                    _hazards.Add(hazard);
                }
            }

            if (IsGameOver)
            {
                break;
            }
            yield return WaitForWaveCompletionAndClearResources();
        }
    }

    private IEnumerator ShowWaveCountTextBetweenWaves(Level level, Wave wave)
    {
        SetWaveCountText(level, wave);
        WaveCountText.gameObject.SetActive(true);
        yield return new WaitForSeconds(WaveWait);
        WaveCountText.gameObject.SetActive(false);
    }

    private void SetWaveCountText(Level level, Wave wave)
    {
        var msg = "Level " + level.Number;
        msg += " -  Wave " + wave.Number;
        WaveCountText.text = msg;
    }

    private IEnumerator WaitForWaveCompletionAndClearResources()
    {
        yield return new WaitUntil(isWaveCompeted);
        ClearExplosions();
        _hazards.Clear();
    }

    private GameObject InstantiateHazard(Wave wave)
    {
        GameObject instance = null;
        Vector3? position = wave.GetNextPosition();
        if (position.HasValue)
        {
            instance = Instantiate(wave.Spread.GameObj, position.Value, Quaternion.identity);
            wave.SetRotation(instance);
            wave.SetSpeed(instance);

        }
        return instance;
    }

    void UpdateScore()
    {
        ScoreText.text = "Score: " + Score;
    }

    void SetLifeText()
    {
        var sb = new StringBuilder().Insert(0, "♡", CurrentLife);
        LifeText.text = sb.ToString();
    }

    private void GameOver()
    {
        _gameOverAudioSource.Play();
        Destroy(Player);
        GameOverText.text = "Game Over";
        IsGameOver = true;
    }

    private void GameDone()
    {
        RestartBtn.gameObject.SetActive(true);
        Destroy(Player);
        GameOverText.text = "Game Completed";
    }
}