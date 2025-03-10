﻿using System.Collections;
using CtaScript.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CtaScript.GameController
{
    public class GameControllerPlayScene : MonoBehaviour
    {
        public int score;
        public int kills;
        public int scorePerScoreTime;
        public float scoreTime;
        public float startWait;
        
        public int scoreSpawnAcceleration;
        public int scoreSpawnAcceleration1;

        public float spawnWait;
        public float spawnWait1;
        public float spawnWait2;

        public float spawnWaitZombie;
        public float spawnWaitZombie1;
        public float spawnWaitZombie2;

        public float horizontalPlayerMovement;
        public float verticalPlayerMovement;
        public bool gameOver;
        public bool pause;

        public GameObject alert;
        public GameObject controlArrows;
        public GameObject controlArrowsRight;
        public GameObject controlArrowsLeft;
        public GameObject controlArrowsCenter;
        public GameObject volumeButton;
        public GameObject muteButton;
        public GameObject resumeButton;
        public GameObject menuButton;
        public GameObject pauseButton;
        public GameObject menuPlayAgainButton;
        public GameObject playAgainText;
        
        public GameObject[] hazards;
        public Vector3[] hazardsSpawnPositions;
        public GameObject zombie;
        public Vector3 zombieSpawnPosition;

        public GameObject[] playerCars;
        public Vector3[] playerCarsSpawnPositions;
        
        public Text scoreText;
        public Text gameOverText;
        public Text tutorialText;
        public Text tutorialTextZombieMode;
        public Text killsText;
        public Text touchedText;
        public Text alertText;

        private AudioSource _activeAudio;
        private bool _alertBool;
        private int _controlSystem;
        private int _zombieMode;
        private AudioSource _zombieRoar;
        private AudioSource _zombieSound;
        
        private AsyncOperation _asyncLoadPlayScene;
        private bool _canPlayAgain;

        private void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
            _zombieMode = PlayerPrefs.GetInt("zombie");
            if (_zombieMode == 1)
            {
                _controlSystem = PlayerPrefs.GetInt("control", 2);
                switch (_controlSystem)
                {
                    case 0:
                    {
                        var t = PlayerPrefs.GetInt("tutorial1", 0);
                        if (t == 0)
                        {
                            StartCoroutine(Tutorial());
                            PlayerPrefs.SetInt("tutorial1", 1);
                        }

                        break;
                    }
                    case 1:
                    {
                        var t = PlayerPrefs.GetInt("tutorial2", 0);
                        if (t == 0)
                        {
                            StartCoroutine(Tutorial());
                            PlayerPrefs.SetInt("tutorial2", 1);
                        }

                        break;
                    }
                    case 2:
                    {
                        switch (PlayerPrefs.GetInt("frecce", 2))
                        {
                            case 0:
                                controlArrowsRight.SetActive(true);
                                break;
                            case 1:
                                controlArrowsLeft.SetActive(true);
                                break;
                            case 2:
                                controlArrowsCenter.SetActive(true);
                                break;
                        }

                        break;
                    }
                }
            }

            var selectedCar = PlayerPrefs.GetInt("car", 0);

            if (_zombieMode == 1)
            {
                selectedCar = 5;
                if (PlayerPrefs.GetInt("tutorialz", 0) == 0)
                {
                    StartCoroutine(TutorialZombie());
                    PlayerPrefs.SetInt("tutorialz", 1);
                }
            }

            Instantiate(playerCars[selectedCar], playerCarsSpawnPositions[selectedCar], Quaternion.identity);

            var sounds = GetComponents<AudioSource>();
            var defaultAudio = sounds[0];
            var zombieAudio = sounds[1];
            _zombieRoar = sounds[2];
            _zombieSound = sounds[3];
            if (_zombieMode == 0)
            {
                defaultAudio.Play();
                _activeAudio = defaultAudio;
            }
            else
            {
                zombieAudio.Play();
                _activeAudio = zombieAudio;
            }
            
            scoreText.text = "Score: " + score;
            killsText.text = _zombieMode == 0 ? "" : "Kills: 0";
            StartCoroutine(UpdateScore());
            if (_zombieMode == 0)
            {
                StartCoroutine(SpawnWaves());
            }
            else
            {
                StartCoroutine(SpawnZombies());
                StartCoroutine(ZombieSound());
            }

            if (_zombieMode == 1)
            {
                if (PlayerPrefs.GetInt("Avviso", 0) == 0)
                {
                    ControlSystemAlert();
                    PlayerPrefs.SetInt("Avviso", 1);
                }
                else
                {
                    _alertBool = true;
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !gameOver && (_zombieMode == 0 || (_zombieMode == 1 && _alertBool)))
            {
                Pause();
            }
        }

        private IEnumerator SpawnWaves()
        {
            var lastHazardIndex = -1;
            var lastSpawnPositionIndex = -1;

            yield return new WaitForSeconds(startWait);
            while (!gameOver)
            {
                var hazardIndex = Random.Range(0, hazards.Length);
                if (hazardIndex == lastHazardIndex)
                {
                    hazardIndex = (hazardIndex + 1) % hazards.Length;
                }

                lastHazardIndex = hazardIndex;
                var hazard = hazards[hazardIndex];

                var spawnPositionIndex = Random.Range(0, hazardsSpawnPositions.Length);
                if (spawnPositionIndex == lastSpawnPositionIndex)
                {
                    spawnPositionIndex = (spawnPositionIndex + 1) % hazardsSpawnPositions.Length;
                }

                lastSpawnPositionIndex = spawnPositionIndex;
                var spawnPosition = hazardsSpawnPositions[spawnPositionIndex];

                Instantiate(hazard, spawnPosition, Quaternion.identity);

                if (score >= scoreSpawnAcceleration && score < scoreSpawnAcceleration1)
                {
                    spawnWait = spawnWait1;
                }
                else if (score >= scoreSpawnAcceleration1)
                {
                    spawnWait = spawnWait2;
                }

                yield return new WaitForSeconds(spawnWait);
            }
        }

        private IEnumerator ZombieSound()
        {
            while (!gameOver)
            {
                var a = Random.Range(7.5f, 10f);
                yield return new WaitForSeconds(a);
                _zombieSound.Play();
            }
        }

        private IEnumerator SpawnZombies()
        {
            yield return new WaitForSeconds(startWait);
            while (!gameOver)
            {
                var spawnPosition = new Vector3(
                    Random.Range(-zombieSpawnPosition.x, zombieSpawnPosition.x),
                    zombieSpawnPosition.y,
                    zombieSpawnPosition.z
                );

                Instantiate(zombie, spawnPosition, Quaternion.identity);

                if (score >= scoreSpawnAcceleration && score < scoreSpawnAcceleration1)
                {
                    spawnWaitZombie = spawnWaitZombie1;
                }
                else if (score >= scoreSpawnAcceleration1)
                {
                    spawnWaitZombie = spawnWaitZombie2;
                }

                yield return new WaitForSeconds(spawnWaitZombie);
            }
        }

        private IEnumerator UpdateScore()
        {
            while (!gameOver)
            {
                yield return new WaitForSeconds(scoreTime);
                score += scorePerScoreTime;
                scoreText.text = "Score: " + score;
            }
        }

        public void GameOver()
        {
            gameOverText.text = "Game Over";
            gameOver = true;
            if (_zombieMode == 1)
            {
                _zombieRoar.Play();
            }

            StartCoroutine(Finish());
        }

        private IEnumerator Finish()
        {
            _asyncLoadPlayScene = Scenes.LoadSceneAsync(Scenes.PlayScene);
            pauseButton.SetActive(false);
            controlArrows.SetActive(false);
            yield return new WaitForSeconds(1.4f);
            _activeAudio.Stop();
            gameOverText.text = "";
            playAgainText.SetActive(true);
            menuPlayAgainButton.SetActive(true);
            _canPlayAgain = true;
        }

        private void UpdateHighScoreAndCr()
        {
            if (_zombieMode == 0)
            {
                if (score > PlayerPrefs.GetInt("HighScore"))
                {
                    PlayerPrefs.SetInt("HighScore", score);
                }
                
                PlayerPrefs.SetInt("cr", PlayerPrefs.GetInt("cr") + score / 2);
            }
            else
            {
                int totalZombieScore = score + kills;
                int totalZombieHighScore = PlayerPrefs.GetInt("ZombieHighScore") + PlayerPrefs.GetInt("ZombieHighKills");
                if (totalZombieScore > totalZombieHighScore)
                {
                    PlayerPrefs.SetInt("ZombieHighScore", score);
                    PlayerPrefs.SetInt("ZombieHighKills", kills);
                }
                
                PlayerPrefs.SetInt("cr", PlayerPrefs.GetInt("cr") + score / 4 + kills / 2);
            }

            PlayerPrefs.Save();
        }

        public void Pause()
        {
            pause = true;

            Time.timeScale = 0.0f;
            _activeAudio.Pause();

            controlArrows.SetActive(false);
            pauseButton.SetActive(false);
            resumeButton.SetActive(true);
            menuButton.SetActive(true);
            if (PlayerPrefs.GetInt("audst", 1) == 1)
            {
                volumeButton.SetActive(true);
            }
            else
            {
                muteButton.SetActive(true);
            }
        }

        public void Resume()
        {
            pause = false;

            Time.timeScale = 1.0f;
            _activeAudio.Play();

            controlArrows.SetActive(true);
            pauseButton.SetActive(true);
            resumeButton.SetActive(false);
            menuButton.SetActive(false);
            volumeButton.SetActive(false);
            muteButton.SetActive(false);
        }

        public void LoadMenu()
        {
            Time.timeScale = 1.0f;
            UpdateHighScoreAndCr();
            SceneManager.LoadScene(Scenes.Menu);
        }

        public void PlayAgain()
        {
            if (!_canPlayAgain)
            {
                return;
            }
            
            UpdateHighScoreAndCr();
            Scenes.ActivateScene(_asyncLoadPlayScene);
        }

        private IEnumerator Tutorial()
        {
            while (!_alertBool)
            {
                yield return new WaitForEndOfFrame();
            }

            switch (_controlSystem)
            {
                case 0:
                    tutorialText.text = "Tilt the phone to move the car";
                    yield return new WaitForSeconds(5f);
                    tutorialText.text = "";
                    break;
                case 1:
                    tutorialText.text = "Drag the finger to move the car";
                    yield return new WaitForSeconds(5f);
                    tutorialText.text = "";
                    break;
            }
        }

        private IEnumerator TutorialZombie()
        {
            while (!_alertBool)
            {
                yield return new WaitForEndOfFrame();
            }

            tutorialTextZombieMode.text = "Touch the screen to shoot";
            yield return new WaitForSeconds(5f);
            tutorialTextZombieMode.text = "";
        }

        public void Touched()
        {
            score += 5;
            touchedText.text = "Touched +5";
            StartCoroutine(TouchedCoroutine());
        }

        private IEnumerator TouchedCoroutine()
        {
            yield return new WaitForSecondsRealtime(1f);
            touchedText.text = "";
        }

        public void Mute()
        {
            PlayerPrefs.SetInt("audst", 0);
            AudioListener.volume = 0.0f;
            volumeButton.SetActive(false);
            muteButton.SetActive(true);
        }

        public void Unmute()
        {
            PlayerPrefs.SetInt("audst", 1);
            AudioListener.volume = 1.0f;
            volumeButton.SetActive(true);
            muteButton.SetActive(false);
        }

        private void ControlSystemAlert()
        {
            Time.timeScale = 0.0f;
            _activeAudio.Pause();
            controlArrows.SetActive(false);
            pauseButton.SetActive(false);
            var control = _controlSystem switch
            {
                0 => "ACCELEROMETER",
                1 => "FINGER DRAG",
                2 => "DIRECTIONAL ARROWS",
                _ => null
            };

            alertText.text = "The control system is \n" + control + "\n you can change it \n in the OPTIONS";
            alert.SetActive(true);
        }

        public void ControlSystemAlertClose()
        {
            alert.SetActive(false);
            Time.timeScale = 1.0f;
            _activeAudio.Play();
            controlArrows.SetActive(true);
            pauseButton.SetActive(true);
            _alertBool = true;
        }

        public void ZombieKill()
        {
            killsText.text = "Kills: " + ++kills;
        }
    }
}