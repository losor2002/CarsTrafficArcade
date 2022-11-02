using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _script
{
    public class gamecontrCARS : MonoBehaviour
    {
        public Text cr;
        public Text astontx;
        public Text chevtx;
        public Text f1tx;
        public Text sport1tx;
        public Text muscletx;
        private int c;
        private int aston;
        private int chev;
        private int f1;
        private int sport1;
        private int muscle;

        AsyncOperation asyncLoad;

        private void Start()
        {
            c = PlayerPrefs.GetInt("cr", 0);
            cr.text = "CR: " + c;
            aston = PlayerPrefs.GetInt("aston", 0);
            chev = PlayerPrefs.GetInt("chev", 0);
            f1 = PlayerPrefs.GetInt("f1", 0);
            sport1 = PlayerPrefs.GetInt("sport1", 0);
            muscle = PlayerPrefs.GetInt("muscle", 0);
            if (aston == 1)
            {
                astontx.text = "";
            }
            if (chev == 1)
            {
                chevtx.text = "";
            }
            if (f1 == 1)
            {
                f1tx.text = "";
            }
            if (sport1 == 1)
            {
                sport1tx.text = "";
            }
            if(muscle == 1)
            {
                muscletx.text = "";
            }
            Caricamento();
        }

        void Caricamento()
        {
            asyncLoad = SceneManager.LoadSceneAsync("menu");
            asyncLoad.allowSceneActivation = false;
        }

        private void Update()
        {
            cr.text = "CR: " + c;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                back();
            }
        }

        public void back()
        {
            asyncLoad.allowSceneActivation = true;
        }

        public void Lambo1()
        {
            PlayerPrefs.SetInt("car", 0);
            back();
        }

        public void Aston()
        {
            if (aston == 0 && c >= 50)
            {
                PlayerPrefs.SetInt("cr", c - 50);
                PlayerPrefs.SetInt("aston", 1);
                PlayerPrefs.SetInt("car", 1);
                PlayerPrefs.Save();
                back();
            }

            if (aston == 1)
            {
                PlayerPrefs.SetInt("car", 1);
                back();
            }
        }

        public void Chev()
        {
            if (chev == 0 && c >= 400)
            {
                PlayerPrefs.SetInt("cr", c - 400);
                PlayerPrefs.SetInt("chev", 1);
                PlayerPrefs.SetInt("car", 2);
                PlayerPrefs.Save();
                back();
            }

            if (chev == 1)
            {
                PlayerPrefs.SetInt("car", 2);
                back();
            }
        }

        public void F1()
        {
            if (f1 == 0 && c >= 1000)
            {
                PlayerPrefs.SetInt("cr", c - 1000);
                PlayerPrefs.SetInt("f1", 1);
                PlayerPrefs.SetInt("car", 3);
                PlayerPrefs.Save();
                back();
            }

            if (f1 == 1)
            {
                PlayerPrefs.SetInt("car", 3);
                back();
            }
        }

        public void Sport1()
        {
            if (sport1 == 0 && c >= 425)
            {
                PlayerPrefs.SetInt("cr", c - 425);
                PlayerPrefs.SetInt("sport1", 1);
                PlayerPrefs.SetInt("car", 4);
                PlayerPrefs.Save();
                back();
            }

            if (sport1 == 1)
            {
                PlayerPrefs.SetInt("car", 4);
                back();
            }
        }

        public void Muscle()
        {
            if (muscle == 0 && c >= 100)
            {
                PlayerPrefs.SetInt("cr", c - 100);
                PlayerPrefs.SetInt("muscle", 1);
                PlayerPrefs.SetInt("car", 6);
                PlayerPrefs.Save();
                back();
            }

            if (muscle == 1)
            {
                PlayerPrefs.SetInt("car", 6);
                back();
            }
        }
    }
}
