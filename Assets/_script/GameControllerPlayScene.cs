using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _script
{
    public class GameControllerPlayScene : MonoBehaviour
    {
        public int score;
        public int scorePerScoreTime;
        public float scoreTime;
        public float startWait;

        [FormerlySerializedAs("scorespawnacc")]
        public int scoreSpawnAcceleration;

        [FormerlySerializedAs("scorespawnacc1")]
        public int scoreSpawnAcceleration1;

        [FormerlySerializedAs("spawnwait1")] public float spawnWait;
        [FormerlySerializedAs("spawnwait2")] public float spawnWait1;
        [FormerlySerializedAs("spawnwait3")] public float spawnWait2;

        [FormerlySerializedAs("spawnwaitzombie1")]
        public float spawnWaitZombie;

        [FormerlySerializedAs("spawnwaitzombie2")]
        public float spawnWaitZombie1;

        [FormerlySerializedAs("spawnwaitzombie3")]
        public float spawnWaitZombie2;

        public float horizontalPlayerMovement;
        public float verticalPlayerMovement;
        public bool gameOver;
        public bool pause;

        [FormerlySerializedAs("avviso")] public GameObject alert;
        [FormerlySerializedAs("frecce")] public GameObject controlArrows;
        [FormerlySerializedAs("freccedestra")] public GameObject controlArrowsRight;

        [FormerlySerializedAs("freccesinistra")]
        public GameObject controlArrowsLeft;

        [FormerlySerializedAs("freccecentro")] public GameObject controlArrowsCenter;
        [FormerlySerializedAs("volumeButt")] public GameObject volumeButton;
        [FormerlySerializedAs("mutoButt")] public GameObject muteButton;
        [FormerlySerializedAs("resumebutt")] public GameObject resumeButton;
        [FormerlySerializedAs("menubutt")] public GameObject menuButton;
        [FormerlySerializedAs("pausebutt")] public GameObject pauseButton;
        public GameObject[] hazards;
        [FormerlySerializedAs("spawnvalues")] public Vector3[] hazardsSpawnPositions;
        [FormerlySerializedAs("zombieprefab")] public GameObject zombie;

        [FormerlySerializedAs("spawnvalueszombie")]
        public Vector3 zombieSpawnPosition;

        [FormerlySerializedAs("cars")] public GameObject[] playerCars;
        [FormerlySerializedAs("carSpawn")] public Vector3[] playerCarsSpawnPositions;
        [FormerlySerializedAs("scoretext")] public Text scoreText;
        [FormerlySerializedAs("gameovertext")] public Text gameOverText;
        [FormerlySerializedAs("tutorialtx")] public Text tutorialText;
        [FormerlySerializedAs("tutorialtxz")] public Text tutorialTextZombieMode;
        [FormerlySerializedAs("Killtx")] public Text killsText;
        [FormerlySerializedAs("sfiorare")] public Text touchedText;
        [FormerlySerializedAs("avvisotx")] public Text alertText;

        private AudioSource _activeAudio;
        private bool _alertBool;
        private int _controlSystem;
        private int _kills;
        private int _zombieMode;
        private AudioSource _zombieRoar;
        private AudioSource _zombieSound;

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
            killsText.text = "";
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
            if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
            {
                Pause();
            }

            if (_zombieMode == 1)
            {
                _kills = PlayerPrefs.GetInt("kills", 0);
                killsText.text = "Kills: " + _kills;
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
            pauseButton.SetActive(false);
            controlArrows.SetActive(false);
            yield return new WaitForSeconds(1.4f);
            _activeAudio.Stop();
            UpdateHighScoreAndCr();
            SceneManager.LoadScene("menu");
        }

        private void UpdateHighScoreAndCr()
        {
            PlayerPrefs.SetInt("CurrentScore", score);
            if (score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", score);
            }

            PlayerPrefs.SetInt("cr", PlayerPrefs.GetInt("cr") + score / 10 + _kills);

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
            SceneManager.LoadScene("menu");
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
            score += 3;
            touchedText.text = "Touched +3";
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
    }
}