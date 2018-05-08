﻿using System.Collections;
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
    public float SpawnWait;
    public float WaveWait;

    public Text ScoreText;
    public Text GameOverText;
    public Text LifeText;
    public Text WaveCountText;

    public bool IsGameOver;

    public Button RestartBtn;

    public int HazardHitScoreValue;

    int _waveCount = 0;

    const float SPAWN_WAIT_DIFFICULTY = 0.4f;

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

    IEnumerator SpawnWaves()
    {
        while (!IsGameOver)
        {
            if (_hazards.TrueForAll((hazard) => hazard == null))
            {
                yield return SpreadWaveHazards();
            }
            else
            {
                yield return new WaitForSeconds(WaveWait);
            }
        }
        RestartBtn.gameObject.SetActive(true);
    }

    IEnumerator SpreadWaveHazards()
    {
        foreach (var wave in LevelManager.CurrentLevel.Waves)
        {
            yield return new WaitForSeconds(wave.SpawnDelaySec);

            ClearExplosions();

            _hazards.Clear();
            _waveCount++;
            SpawnWait -= SPAWN_WAIT_DIFFICULTY;

            WaveCountText.gameObject.SetActive(true);
            UpdateWaveCount();

            yield return new WaitForSeconds(WaveWait);
            WaveCountText.gameObject.SetActive(false);

            HazardCount = HazardCount * _waveCount;

            for (int i = 0; i < wave.WaveSize; i++)
            {

                yield return new WaitForSeconds(wave.SpawnDelaySec);
                _hazards.Add(InstantiateHazard(wave));
                if (IsGameOver)
                {
                    break;
                }
            }
        }
        LevelManager.LevelEnd();
    }

    GameObject InstantiateHazard(Wave wave)
    {
        return Instantiate(wave.Hazard, wave.GetNextSpawnPosition(), Quaternion.identity);

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
        WaveCountText.text = "Wave " + _waveCount;
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
}
