using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _script
{
    public class gamecontrMENU : MonoBehaviour
    {
        public GameObject PlayButt;
        public GameObject zombiebutt;
        public GameObject ClassicButt;
        public GameObject menubutton;
        public GameObject controlsysimg;
        public GameObject sceltafrecce;
        public GameObject backbutt;
        public GameObject carsbutt;
        public GameObject tutorialButt;
        public GameObject volumeButt;
        public GameObject mutoButt;
        public GameObject tabella;
        public GameObject quad1;
        public GameObject quad2;
        public GameObject quad3;
        public GameObject quad4;
        public GameObject quad5;
        public GameObject quad6;
        public GameObject quad7;
        public GameObject uscita;

        public Button controlsys;
        public Button controlsys1;
        public Button controlsys2;
        public Button frdestra;
        public Button frsinistra;
        public Button frcentro;
        public Text highscoretx;
        public Text scoretx;
        public Text crtx;

        private AsyncOperation asyncLoad;
        private bool aud = true;
        private int cr;
        private TimeSpan difference;
        private int ft;
        private int highscore;
        private bool main = true;

        private bool options;
        private int score;
        private bool sk;

        private void Start()
        {
            Application.targetFrameRate = -1;

            dayCheck();

            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            cr = PlayerPrefs.GetInt("cr", 0);
            highscore = PlayerPrefs.GetInt("HighScore", 0);
            score = PlayerPrefs.GetInt("CurrentScore", 0);
            PlayerPrefs.SetInt("kills", 0);
            Caricamento();
        }

        private void Update()
        {
            if (main)
            {
                highscoretx.text = "HighScore: " + highscore;
                scoretx.text = "Score: " + score;
                crtx.text = "CR: " + cr;


                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    uscita.SetActive(true);
                    Nulla();
                }
            }
            else
            {
                highscoretx.text = "";
                scoretx.text = "";
                crtx.text = "";

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    back();
                }
            }

            if (aud)
            {
                var audst = PlayerPrefs.GetInt("audst", 1);
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

            if (options)
            {
                var control = PlayerPrefs.GetInt("control", 2);

                if (control == 2)
                {
                    sceltafrecce.SetActive(true);
                }
                else
                {
                    sceltafrecce.SetActive(false);
                }


                if (control == 0)
                {
                    controlsys.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                    controlsys1.GetComponent<Image>().color = default;
                    controlsys2.GetComponent<Image>().color = default;
                }
                else if (control == 1)
                {
                    controlsys.GetComponent<Image>().color = default;
                    controlsys1.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                    controlsys2.GetComponent<Image>().color = default;
                }
                else if (control == 2)
                {
                    controlsys.GetComponent<Image>().color = default;
                    controlsys1.GetComponent<Image>().color = default;
                    controlsys2.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                    var frecce = PlayerPrefs.GetInt("frecce", 2);
                    if (frecce == 0)
                    {
                        frdestra.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                        frsinistra.GetComponent<Image>().color = default;
                        frcentro.GetComponent<Image>().color = default;
                    }
                    else if (frecce == 1)
                    {
                        frdestra.GetComponent<Image>().color = default;
                        frsinistra.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                        frcentro.GetComponent<Image>().color = default;
                    }
                    else if (frecce == 2)
                    {
                        frdestra.GetComponent<Image>().color = default;
                        frsinistra.GetComponent<Image>().color = default;
                        frcentro.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                    }
                }
            }
        }

        private void Caricamento()
        {
            asyncLoad = SceneManager.LoadSceneAsync("scena1");
            asyncLoad.allowSceneActivation = false;
        }

        public void dayCheck()
        {
            ft = PlayerPrefs.GetInt("ft", 0);
            var stringDate = PlayerPrefs.GetString("PlayDate", Convert.ToString(DateTime.Now));
            var oldDate = Convert.ToDateTime(stringDate);
            var newDate = DateTime.Now;

            difference = newDate.Subtract(oldDate);
            if (difference.Days >= 1 || (difference.Days == 0 && ft == 0))
            {
                var newStringDate = Convert.ToString(newDate);
                PlayerPrefs.SetString("PlayDate", newStringDate);
                giveGift();
            }
        }

        private void giveGift()
        {
            tabella.SetActive(true);
            Nulla();
            var day = PlayerPrefs.GetInt("day", 1);

            if ((difference.Days == 0 && ft == 0) || difference.Days > 1)
            {
                quad1.SetActive(true);
                StartCoroutine(lamp1());
                var c = PlayerPrefs.GetInt("cr", 0);
                PlayerPrefs.SetInt("cr", c + 10);
                PlayerPrefs.SetInt("ft", ft + 1);
                PlayerPrefs.SetInt("day", 2);
            }

            if (difference.Days == 1)
            {
                if (day == 1)
                {
                    PlayerPrefs.SetInt("day", 2);
                    quad1.SetActive(true);
                    StartCoroutine(lamp1());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 10);
                }

                if (day == 2)
                {
                    PlayerPrefs.SetInt("day", 3);
                    quad2.SetActive(true);
                    StartCoroutine(lamp2());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 20);
                }

                if (day == 3)
                {
                    PlayerPrefs.SetInt("day", 4);
                    quad3.SetActive(true);
                    StartCoroutine(lamp3());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 30);
                }

                if (day == 4)
                {
                    PlayerPrefs.SetInt("day", 5);
                    quad4.SetActive(true);
                    StartCoroutine(lamp4());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 40);
                }

                if (day == 5)
                {
                    PlayerPrefs.SetInt("day", 6);
                    quad5.SetActive(true);
                    StartCoroutine(lamp5());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 50);
                }

                if (day == 6)
                {
                    PlayerPrefs.SetInt("day", 7);
                    quad6.SetActive(true);
                    StartCoroutine(lamp6());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 60);
                }

                if (day == 7)
                {
                    PlayerPrefs.SetInt("day", 1);
                    quad7.SetActive(true);
                    StartCoroutine(lamp7());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 70);
                }
            }
        }

        public void Ok()
        {
            sk = true;
            tabella.SetActive(false);
            back();
        }

        private IEnumerator lamp1()
        {
            while (!sk)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                quad1.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                quad1.SetActive(true);
            }
        }

        private IEnumerator lamp2()
        {
            while (!sk)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                quad2.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                quad2.SetActive(true);
            }
        }

        private IEnumerator lamp3()
        {
            while (!sk)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                quad3.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                quad3.SetActive(true);
            }
        }

        private IEnumerator lamp4()
        {
            while (!sk)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                quad4.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                quad4.SetActive(true);
            }
        }

        private IEnumerator lamp5()
        {
            while (!sk)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                quad5.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                quad5.SetActive(true);
            }
        }

        private IEnumerator lamp6()
        {
            while (!sk)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                quad6.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                quad6.SetActive(true);
            }
        }

        private IEnumerator lamp7()
        {
            while (!sk)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                quad7.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                quad7.SetActive(true);
            }
        }

        public void startGame()
        {
            PlayerPrefs.SetInt("zombie", 0);
            asyncLoad.allowSceneActivation = true;
        }

        public void zombiestart()
        {
            PlayerPrefs.SetInt("zombie", 1);
            asyncLoad.allowSceneActivation = true;
        }

        public void menu()
        {
            PlayButt.SetActive(false);
            menubutton.SetActive(false);
            carsbutt.SetActive(false);
            aud = false;
            controlsysimg.SetActive(true);
            backbutt.SetActive(true);
            tutorialButt.SetActive(true);
            main = false;
            options = true;
        }

        public void controlSystem()
        {
            PlayerPrefs.SetInt("control", 0);
        }

        public void controlSystem1()
        {
            PlayerPrefs.SetInt("control", 1);
        }

        public void controlSystem2()
        {
            PlayerPrefs.SetInt("control", 2);
        }

        public void freccedestra()
        {
            PlayerPrefs.SetInt("frecce", 0);
        }

        public void freccesinistra()
        {
            PlayerPrefs.SetInt("frecce", 1);
        }

        public void freccecentro()
        {
            PlayerPrefs.SetInt("frecce", 2);
        }

        public void back()
        {
            PlayButt.SetActive(true);
            menubutton.SetActive(true);
            carsbutt.SetActive(true);
            controlsysimg.SetActive(false);
            backbutt.SetActive(false);
            tutorialButt.SetActive(false);
            ClassicButt.SetActive(false);
            zombiebutt.SetActive(false);
            uscita.SetActive(false);
            sceltafrecce.SetActive(false);
            options = false;
            aud = true;
            main = true;
        }

        public void Cars()
        {
            SceneManager.LoadScene("cars");
        }

        public void Tutorial()
        {
            PlayerPrefs.SetInt("tutorial1", 0);
            PlayerPrefs.SetInt("tutorial2", 0);
            PlayerPrefs.SetInt("tutorialz", 0);
        }

        public void Volume()
        {
            PlayerPrefs.SetInt("audst", 0);
        }

        public void Muto()
        {
            PlayerPrefs.SetInt("audst", 1);
        }

        public void PlayB()
        {
            zombiebutt.SetActive(true);
            ClassicButt.SetActive(true);
            backbutt.SetActive(true);
            menubutton.SetActive(false);
            carsbutt.SetActive(false);
            PlayButt.SetActive(false);
            main = false;
        }

        public void Uscire()
        {
            PlayerPrefs.Save();
            Application.Quit();
        }

        private void Nulla()
        {
            main = false;
            PlayButt.SetActive(false);
            menubutton.SetActive(false);
            carsbutt.SetActive(false);
            aud = false;
        }
    }
}