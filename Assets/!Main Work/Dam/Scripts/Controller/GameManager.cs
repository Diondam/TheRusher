using System;
using _Main_Work.Dam.Scripts.Character.Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

namespace _Main_Work.Dam.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public UI_Controller uiController;
        public SoundController soundController;
        [Space]
        public Enemy enemy;
        public int level = 0;
        private string sceneNameToLoad = "level";
        public string arbitraryNameScene ;
        public Vector3 diePoint = new Vector3(0,0f,0);
        
        
        private void Awake()
        {
            uiController = FindObjectOfType<UI_Controller>();
            soundController = FindObjectOfType<SoundController>();
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            uiController.OpenStartGame();
            soundController.BGSpeaker.PlayOneShot(soundController.introSound);
        }

      
     
        private void Update()
        {
            print($"Enemy Current State: {enemy?.currentState}");
        }
      
        public void JustLoadAScene()
        {
            SceneManager.LoadScene(arbitraryNameScene);
        }

        public void StartGame()
        {
            LoadScene(level);
            uiController.gameStart.SetActive(false);
            soundController.PlayBGSound(level);
        }
      
        public void Resume()
        {
           // LoadPlayerData();
            Time.timeScale = 1;
        }

        public void ReStart()
        {
            uiController.OpenVictory(false);
            if (level == 6)
            {
                SceneManager.LoadScene("level0", LoadSceneMode.Additive);
                return;
            }

            //TODO: tắt 2 cái trước rồi mới load
            uiController.DelayImage.gameObject.SetActive(true);
            uiController.gameEnd.SetActive(false);
            Time.timeScale = 1;
            //LoadPlayerData();
            SceneManager.LoadScene("level"+ level, LoadSceneMode.Additive);

        }
        public void LoadLevel()
        {
            level++;
            LoadScene(level);
            soundController.PlayBGSound(level);
            SavePlayerData();
        }
        public void LoadScene(int level)
        {
            if (this.level == 6)
            {
                uiController.OpenVictory(true);
                SceneManager.UnloadSceneAsync("level" + (this.level-1));
                return;
            }
            StartCoroutine(uiController.OpenDelayScene());
            if (this.level > 0)
            {
            SceneManager.UnloadSceneAsync("level" + (this.level-1));
                
            }
            SceneManager.LoadSceneAsync("level"+ level, LoadSceneMode.Additive);
        }
        
        void SavePlayerData()
        {
            PlayerPrefs.SetInt("Level", level);
            
                string json = JsonUtility.ToJson(diePoint);
                PlayerPrefs.SetString("DiePoint", json);
            
            PlayerPrefs.Save(); 
        }

        void LoadPlayerData()
        {
            string json = PlayerPrefs.GetString("TransformData");
            if (!string.IsNullOrEmpty(json))
            {
                diePoint = JsonUtility.FromJson<Vector3>(json);
            }
            print("load position");
            if (PlayerPrefs.HasKey("Level"))
            {
                level = PlayerPrefs.GetInt("Level");
            }
        }

        void OnDestroy()
        {
            SavePlayerData();
        }
        public void EndGame()
        {
            Time.timeScale = 0;
            SavePlayerData();
            uiController.OpenEndGame();
            SceneManager.UnloadSceneAsync("level" + level);
        }

        public void Pause()
        {
            Time.timeScale = 0;
            uiController.OpenPause();
        }

        
    }
}