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
        private int zombie;
        
        public int score;
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
        
        private int kills;
        public bool gameover = false;
        public bool pause = false;
        private bool avvisobl;

        public GameObject avviso;
        public GameObject frecce;
        public GameObject freccedestra;
        public GameObject freccesinistra;
        public GameObject freccecentro;
        public GameObject volumeButt;
        public GameObject mutoButt;
        public GameObject resumebutt;
        public GameObject menubutt;
        public GameObject pausebutt;
        public GameObject explosionE;
        public GameObject[] hazards;
        public Vector3[] spawnvalues;
        public Vector3 spawnvalueszombie;
        public GameObject[] cars;
        public GameObject zombieprefab;
        public Vector3[] carSpawn;
        private Vector3 LspawnPosition;
        private GameObject Lhazard;

        public Text scoretext;
        public Text gameovertext;
        public Text tutorialtx;
        public Text tutorialtxz;
        public Text Killtx;
        public Text sfiorare;
        public Text avvisotx;
        private string control;

        private void Start()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        
            avvisobl = false;
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
                    freccedestra.SetActive(true);
                }
                else if(arrows == 1)
                {
                    freccesinistra.SetActive(true);
                }
                else if(arrows == 2)
                {
                    freccecentro.SetActive(true);
                }
            }
            int c = PlayerPrefs.GetInt("car", 0);
            zombie = PlayerPrefs.GetInt("zombie");
            if (zombie == 1)
            {
                c = 5;
                int tz = PlayerPrefs.GetInt("tutorialz", 0);
                if(tz == 0)
                {
                    StartCoroutine(Tutorialz());
                    PlayerPrefs.SetInt("tutorialz", 1);
                }
            }
            GameObject car = cars[c];
            Vector3 spawnPosition = carSpawn[c];
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(car, spawnPosition, spawnRotation);
            AudioSource[] sounds = GetComponents<AudioSource>();
            AudioSource defaultAudio = sounds[0];
            AudioSource zombieAudio = sounds[1];
            zombieRoar = sounds[2];
            zombieVerse = sounds[3];
            if (zombie == 0)
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
            scoretext.text = "Score: " + score;
            Killtx.text = "";
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            StartCoroutine(score1());
            if (zombie == 0)
            {
                StartCoroutine(SpawnWawes());
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
                avvisobl = true;
            }
        }

        private void Update()
        {
            if (!gameover)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pausa();
                }
            }

            if (zombie == 1)
            {
                kills = PlayerPrefs.GetInt("kills", 0);
                Killtx.text = "Kills: " + kills;
            }

            if (pause)
            {
                int audst = PlayerPrefs.GetInt("audst", 1);
                if (audst == 1)
                {
                    AudioListener.volume = 1.0f;
                    volumeButt.SetActive(true);
                    mutoButt.SetActive(false);
                }
                else
                {
                    AudioListener.volume = 0.0f;
                    volumeButt.SetActive(false);
                    mutoButt.SetActive(true);
                }
            }
            else
            {
                volumeButt.SetActive(false);
                mutoButt.SetActive(false);
            }
        }

        IEnumerator SpawnWawes()
        {
            yield return new WaitForSeconds(startWait);
            while (!gameover)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = spawnvalues[Random.Range(0, spawnvalues.Length)];
                while(LspawnPosition == spawnPosition)
                {
                    spawnPosition = spawnvalues[Random.Range(0, spawnvalues.Length)];
                }
                while (Lhazard == hazard)
                {
                    hazard = hazards[Random.Range(0, hazards.Length)];
                }
                Lhazard = hazard;
                LspawnPosition = spawnPosition;
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                if (score >= scoreSpawnAcceleration && score < scoreSpawnAcceleration1)
                {
                    spawnWait = spawnWait1;
                }
                if (score >= scoreSpawnAcceleration1)
                {
                    spawnWait = spawnWait2;
                }
                yield return new WaitForSeconds(spawnWait);
            }
        }

        IEnumerator VersoZombie()
        {
            while (!gameover)
            {
                float a = Random.Range(7.5f, 10f);
                yield return new WaitForSeconds(a);
                zombieVerse.Play();
            }
        }

        IEnumerator SpawnZombies()
        {
            yield return new WaitForSeconds(startWait);
            while (!gameover)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnvalueszombie.x, spawnvalueszombie.x), spawnvalueszombie.y, spawnvalueszombie.z);
                while (LspawnPosition == spawnPosition)
                {
                    spawnPosition = new Vector3(Random.Range(-spawnvalueszombie.x, spawnvalueszombie.x), spawnvalueszombie.y, spawnvalueszombie.z);
                }
                LspawnPosition = spawnPosition;
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(zombieprefab, spawnPosition, spawnRotation);
                if (score >= scoreSpawnAcceleration && score < scoreSpawnAcceleration1)
                {
                    spawnWaitZombie = spawnWaitZombie1;
                }
                if (score >= scoreSpawnAcceleration1)
                {
                    spawnWaitZombie = spawnWaitZombie2;
                }
                yield return new WaitForSeconds(spawnWaitZombie);
            }
        }

        IEnumerator score1()
        {
            yield return new WaitForSeconds(scoreTime);
            while (!gameover)
            {
                score += scorePerScoreTime;
                scoretext.text = "Score: " + score;
                yield return new WaitForSeconds(scoreTime);
            }
        }

        public void GameOver()
        {
            gameovertext.text = "Game Over";
            gameover = true;
            if (zombie == 1)
            {
                zombieRoar.Play();
            }
            StartCoroutine(finaldestroy());
        }

        IEnumerator finaldestroy()
        {
            pausebutt.SetActive(false);
            frecce.SetActive(false);
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
            frecce.SetActive(false);
            Time.timeScale = 0.0f;
            activeAudio.Pause();
            pausebutt.SetActive(false);
            resumebutt.SetActive(true);
            menubutt.SetActive(true);
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }

        public void riprendi()
        {
            pause = false;
            frecce.SetActive(true);
            Time.timeScale = 1.0f;
            activeAudio.Play();
            pausebutt.SetActive(true);
            resumebutt.SetActive(false);
            menubutt.SetActive(false);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        public void menu()
        {
            HighScore();
            SceneManager.LoadScene("menu");
        }

        IEnumerator Tutorial()
        {
            while (!avvisobl)
            {
                yield return new WaitForEndOfFrame();
            }
            if (controlSystem == 0)
            {
                tutorialtx.text = "Tilt the phone to move the car";
                yield return new WaitForSeconds(5f);
                tutorialtx.text = "";
            }
            if (controlSystem == 1)
            {
                tutorialtx.text = "Drag the finger to move the car";
                yield return new WaitForSeconds(5f);
                tutorialtx.text = "";
            }
        }

        IEnumerator Tutorialz()
        {
            while (!avvisobl)
            {
                yield return new WaitForEndOfFrame();
            }
            tutorialtxz.text = "Touch the screen to shoot";
            yield return new WaitForSeconds(5f);
            tutorialtxz.text = "";
        }

        public void Sfioramento()
        {
            score += 3;
            sfiorare.text = "TOUCHED +3";
            StartCoroutine(Sfioramento1());
        }

        IEnumerator Sfioramento1()
        { 
            yield return new WaitForSecondsRealtime(1f);
            sfiorare.text = "";
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
            frecce.SetActive(false);
            pausebutt.SetActive(false);
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
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
            avvisotx.text = "The control system is \n" + control + "\n you can change it \n in the OPTIONS";
            avviso.SetActive(true);
        }

        public void Avviso2()
        {
            avviso.SetActive(false);
            Time.timeScale = 1.0f;
            activeAudio.Play();
            frecce.SetActive(true);
            pausebutt.SetActive(true);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            avvisobl = true;
        }
    }
}
