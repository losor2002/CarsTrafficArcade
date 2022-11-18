using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _script
{
    public class GameController : MonoBehaviour
    {
        private AudioSource activeAudio;
        private AudioSource zombieRoar;
        private AudioSource zombieVerse;
        
        private int controlSystem;
        private int zombieMode;
        
        private int kills;
        public int score;
        public int scorePerScoreTime;
        public float scoreTime;
        public float startWait;
        
        [FormerlySerializedAs("scorespawnacc")]
        public int scoreSpawnAcceleration;
        [FormerlySerializedAs("scorespawnacc1")]
        public int scoreSpawnAcceleration1;
        
        [FormerlySerializedAs("spawnwait1")]
        public float spawnWait;
        [FormerlySerializedAs("spawnwait2")]
        public float spawnWait1;
        [FormerlySerializedAs("spawnwait3")]
        public float spawnWait2;
        
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
        
        private bool alertBool;

        [FormerlySerializedAs("avviso")]
        public GameObject alert;
        [FormerlySerializedAs("frecce")]
        public GameObject controlArrows;
        [FormerlySerializedAs("freccedestra")]
        public GameObject controlArrowsRight;
        [FormerlySerializedAs("freccesinistra")]
        public GameObject controlArrowsLeft;
        [FormerlySerializedAs("freccecentro")]
        public GameObject controlArrowsCenter;
        [FormerlySerializedAs("volumeButt")]
        public GameObject volumeButton;
        [FormerlySerializedAs("mutoButt")]
        public GameObject muteButton;
        [FormerlySerializedAs("resumebutt")]
        public GameObject resumeButton;
        [FormerlySerializedAs("menubutt")]
        public GameObject menuButton;
        [FormerlySerializedAs("pausebutt")]
        public GameObject pauseButton;
        
        public GameObject[] hazards;
        [FormerlySerializedAs("spawnvalues")]
        public Vector3[] hazardsSpawnPositions;

        [FormerlySerializedAs("zombieprefab")]
        public GameObject zombie;
        [FormerlySerializedAs("spawnvalueszombie")]
        public Vector3 zombieSpawnPosition;
        
        [FormerlySerializedAs("cars")]
        public GameObject[] playerCars;
        [FormerlySerializedAs("carSpawn")]
        public Vector3[] playerCarsSpawnPositions;

        [FormerlySerializedAs("scoretext")]
        public Text scoreText;
        [FormerlySerializedAs("gameovertext")]
        public Text gameOverText;
        [FormerlySerializedAs("tutorialtx")]
        public Text tutorialText;
        [FormerlySerializedAs("tutorialtxz")]
        public Text tutorialTextZombieMode;
        [FormerlySerializedAs("Killtx")]
        public Text killsText;
        [FormerlySerializedAs("sfiorare")]
        public Text touchedText;
        [FormerlySerializedAs("avvisotx")]
        public Text alertText;

        private void Start()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            
            int arrows = PlayerPrefs.GetInt("frecce", 2);
            controlSystem = PlayerPrefs.GetInt("control", 2);
            if (controlSystem == 0)
            {
                int t = PlayerPrefs.GetInt("tutorial1", 0);
                if (t == 0)
                {
                    StartCoroutine(Tutorial());
                    PlayerPrefs.SetInt("tutorial1", 1);
                }
            }
            else if (controlSystem == 1)
            {
                int t = PlayerPrefs.GetInt("tutorial2", 0);
                if (t == 0)
                {
                    StartCoroutine(Tutorial());
                    PlayerPrefs.SetInt("tutorial2", 1);
                }
            }
            else if (controlSystem == 2)
            {
                if(arrows == 0)
                {
                    controlArrowsRight.SetActive(true);
                }
                else if(arrows == 1)
                {
                    controlArrowsLeft.SetActive(true);
                }
                else if(arrows == 2)
                {
                    controlArrowsCenter.SetActive(true);
                }
            }
            int c = PlayerPrefs.GetInt("car", 0);
            zombieMode = PlayerPrefs.GetInt("zombie");
            if (zombieMode == 1)
            {
                c = 5;
                int tz = PlayerPrefs.GetInt("tutorialz", 0);
                if(tz == 0)
                {
                    StartCoroutine(Tutorialz());
                    PlayerPrefs.SetInt("tutorialz", 1);
                }
            }
            GameObject car = playerCars[c];
            Vector3 spawnPosition = playerCarsSpawnPositions[c];
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(car, spawnPosition, spawnRotation);
            AudioSource[] sounds = GetComponents<AudioSource>();
            AudioSource defaultAudio = sounds[0];
            AudioSource zombieAudio = sounds[1];
            zombieRoar = sounds[2];
            zombieVerse = sounds[3];
            if (zombieMode == 0)
            {
                defaultAudio.Play();
                activeAudio = defaultAudio;
            }
            else
            {
                zombieAudio.Play();
                activeAudio = zombieAudio;
            }
            Time.timeScale = 1.0f;
            scoreText.text = "Score: " + score;
            killsText.text = "";
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            StartCoroutine(score1());
            if (zombieMode == 0)
            {
                StartCoroutine(SpawnWaves());
            }
            else
            {
                StartCoroutine(SpawnZombies());
                StartCoroutine(VersoZombie());
            }
            int av = PlayerPrefs.GetInt("Avviso", 0);
            if (av == 0)
            {
                Avviso1();
                PlayerPrefs.SetInt("Avviso", 1);
            }
            else
            {
                alertBool = true;
            }
        }

        private void Update()
        {
            if (!gameOver)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pausa();
                }
            }

            if (zombieMode == 1)
            {
                kills = PlayerPrefs.GetInt("kills", 0);
                killsText.text = "Kills: " + kills;
            }

            if (pause)
            {
                int audst = PlayerPrefs.GetInt("audst", 1);
                if (audst == 1)
                {
                    AudioListener.volume = 1.0f;
                    volumeButton.SetActive(true);
                    muteButton.SetActive(false);
                }
                else
                {
                    AudioListener.volume = 0.0f;
                    volumeButton.SetActive(false);
                    muteButton.SetActive(true);
                }
            }
            else
            {
                volumeButton.SetActive(false);
                muteButton.SetActive(false);
            }
        }

        private IEnumerator SpawnWaves()
        {
            int lastHazardIndex = -1;
            int lastSpawnPositionIndex = -1;
            
            yield return new WaitForSeconds(startWait);
            while (!gameOver)
            {
                int hazardIndex = Random.Range(0, hazards.Length);
                if (hazardIndex == lastHazardIndex)
                    hazardIndex = (hazardIndex + 1) % hazards.Length;
                lastHazardIndex = hazardIndex;
                GameObject hazard = hazards[hazardIndex];

                int spawnPositionIndex = Random.Range(0, hazardsSpawnPositions.Length);
                if (spawnPositionIndex == lastSpawnPositionIndex)
                    spawnPositionIndex = (spawnPositionIndex + 1) % hazardsSpawnPositions.Length;
                lastSpawnPositionIndex = spawnPositionIndex;
                Vector3 spawnPosition = hazardsSpawnPositions[spawnPositionIndex];
                
                Quaternion spawnRotation = Quaternion.identity;
                
                Instantiate(hazard, spawnPosition, spawnRotation);

                if (score >= scoreSpawnAcceleration && score < scoreSpawnAcceleration1)
                    spawnWait = spawnWait1;
                else if (score >= scoreSpawnAcceleration1)
                    spawnWait = spawnWait2;
                
                yield return new WaitForSeconds(spawnWait);
            }
        }

        IEnumerator VersoZombie()
        {
            while (!gameOver)
            {
                float a = Random.Range(7.5f, 10f);
                yield return new WaitForSeconds(a);
                zombieVerse.Play();
            }
        }

        private IEnumerator SpawnZombies()
        {
            yield return new WaitForSeconds(startWait);
            while (!gameOver)
            {
                Vector3 spawnPosition = new Vector3(
                    Random.Range(-zombieSpawnPosition.x, zombieSpawnPosition.x),
                    zombieSpawnPosition.y,
                    zombieSpawnPosition.z
                    );
                
                Quaternion spawnRotation = Quaternion.identity;
                
                Instantiate(zombie, spawnPosition, spawnRotation);
                
                if (score >= scoreSpawnAcceleration && score < scoreSpawnAcceleration1)
                    spawnWaitZombie = spawnWaitZombie1;
                else if (score >= scoreSpawnAcceleration1)
                    spawnWaitZombie = spawnWaitZombie2;
                
                yield return new WaitForSeconds(spawnWaitZombie);
            }
        }

        IEnumerator score1()
        {
            yield return new WaitForSeconds(scoreTime);
            while (!gameOver)
            {
                score += scorePerScoreTime;
                scoreText.text = "Score: " + score;
                yield return new WaitForSeconds(scoreTime);
            }
        }

        public void GameOver()
        {
            gameOverText.text = "Game Over";
            gameOver = true;
            if (zombieMode == 1)
            {
                zombieRoar.Play();
            }
            StartCoroutine(finaldestroy());
        }

        IEnumerator finaldestroy()
        {
            pauseButton.SetActive(false);
            controlArrows.SetActive(false);
            yield return new WaitForSeconds(1.4f);
            Time.timeScale = 0.0f;
            activeAudio.Stop();
            HighScore();
            PlayerPrefs.Save();
            SceneManager.LoadScene("menu");
        }

        void HighScore()
        {
            PlayerPrefs.SetInt("CurrentScore", score);
            int highscore = PlayerPrefs.GetInt("HighScore", 0);
            if (score > highscore)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
            int c = PlayerPrefs.GetInt("cr");
            PlayerPrefs.SetInt("cr", c + (score / 10) + kills);
        }

        public void pausa()
        {
            pause = true;
            controlArrows.SetActive(false);
            Time.timeScale = 0.0f;
            activeAudio.Pause();
            pauseButton.SetActive(false);
            resumeButton.SetActive(true);
            menuButton.SetActive(true);
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }

        public void riprendi()
        {
            pause = false;
            controlArrows.SetActive(true);
            Time.timeScale = 1.0f;
            activeAudio.Play();
            pauseButton.SetActive(true);
            resumeButton.SetActive(false);
            menuButton.SetActive(false);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        public void menu()
        {
            HighScore();
            SceneManager.LoadScene("menu");
        }

        IEnumerator Tutorial()
        {
            while (!alertBool)
            {
                yield return new WaitForEndOfFrame();
            }
            if (controlSystem == 0)
            {
                tutorialText.text = "Tilt the phone to move the car";
                yield return new WaitForSeconds(5f);
                tutorialText.text = "";
            }
            if (controlSystem == 1)
            {
                tutorialText.text = "Drag the finger to move the car";
                yield return new WaitForSeconds(5f);
                tutorialText.text = "";
            }
        }

        IEnumerator Tutorialz()
        {
            while (!alertBool)
            {
                yield return new WaitForEndOfFrame();
            }
            tutorialTextZombieMode.text = "Touch the screen to shoot";
            yield return new WaitForSeconds(5f);
            tutorialTextZombieMode.text = "";
        }

        public void Sfioramento()
        {
            score += 3;
            touchedText.text = "TOUCHED +3";
            StartCoroutine(Sfioramento1());
        }

        IEnumerator Sfioramento1()
        { 
            yield return new WaitForSecondsRealtime(1f);
            touchedText.text = "";
        }

        public void Volume()
        {
            PlayerPrefs.SetInt("audst", 0);
        }

        public void Muto()
        {
            PlayerPrefs.SetInt("audst", 1);
        }

        void Avviso1()
        {
            Time.timeScale = 0.0f;
            activeAudio.Pause();
            controlArrows.SetActive(false);
            pauseButton.SetActive(false);
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            string control = null;
            if (controlSystem == 0)
            {
                control = "ACCELEROMETER";
            }
            else if (controlSystem == 1)
            {
                control = "FINGER DRAG";
            }
            else if (controlSystem == 2)
            {
                control = "DIRECTIONAL ARROWS";
            }
            alertText.text = "The control system is \n" + control + "\n you can change it \n in the OPTIONS";
            alert.SetActive(true);
        }

        public void Avviso2()
        {
            alert.SetActive(false);
            Time.timeScale = 1.0f;
            activeAudio.Play();
            controlArrows.SetActive(true);
            pauseButton.SetActive(true);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            alertBool = true;
        }
    }
}
