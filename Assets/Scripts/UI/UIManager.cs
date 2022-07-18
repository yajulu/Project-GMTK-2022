using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yajulu.Input;

namespace UI
{
    public class UIManager : Essentials.Singleton<UIManager>
    {


        private Main mainInput;

        [HideInInspector]
        public float diceCooldown;


        [SerializeField]
        Image dice, diceCoolDown;
        [SerializeField]
        Transform hpParent;




        [SerializeField]
        GameObject StartMenuPanel, PauseMenuPanel, HUDPanel, CreditsPanel, TutorialPanel, GameOverPanel, TutorialManager;

        TextMeshProUGUI hintTextBox;

        public TextMeshProUGUI gameOverScoreText;

        public static event Action TutorialStarted;
        public static event Action GameStarted;
        public static event Action GameOver;

        private int currentScore;

        private GameState currentState;

        public int CurrentScore => currentScore;

        public GameState CurrentGameState => currentState;

        public enum GameState
        {
            Initialized,
            Tutorial,
            Started,
            Paused,
            Ended
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            mainInput = new Main();
            currentState = GameState.Initialized;
        }
        private void OnEnable()
        {
            mainInput.UI.Enable();
            mainInput.UI.Pause.performed += OnPausePerformed;
            hintTextBox = TutorialPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        }

        private void Start()
        {
            currentState = GameState.Initialized;


        }

        private void OnPausePerformed(InputAction.CallbackContext obj)
        {
            if (!PauseMenuPanel.activeSelf && !StartMenuPanel.activeSelf)
            {
                PauseMenuPanel.SetActive(true);
                HUDPanel.SetActive(false);
                currentState = GameState.Paused;
                Time.timeScale = 0;
                //Set "Start Game Boolean" here
            }
            else
            {
                PauseMenuPanel.SetActive(false);
                HUDPanel.SetActive(true);
                currentState = GameState.Paused;
                Time.timeScale = 1;

            }


        }

        public void UpdatePlayerScore(int score)
        {
            if (currentState == GameState.Started)
                currentScore += score;
        }

        private void Update()
        {
            if (currentState == GameState.Started)
                UpdatePlayerScore((int)(1000 * Time.deltaTime));
        }

        public void StartGame()
        {
            if (TutorialPanel.activeSelf)
            {
                TutorialManager.SetActive(false);
                TutorialPanel.SetActive(false);
            }
            HUDPanel.SetActive(true);
            Time.timeScale = 1;
            currentScore = 0;
            currentState = GameState.Started;
            GameStarted?.Invoke();




            //Set "Start Game Boolean" here
        }

        public void StartTutorial()
        {
            if (StartMenuPanel.activeSelf)
            {
                TutorialManager.SetActive(true);
                StartMenuPanel.SetActive(false);
                TutorialPanel.SetActive(true);
                Time.timeScale = 1;
                currentState = GameState.Initialized;
                TutorialStarted?.Invoke();
            }


        }


        public void Resume()
        {
            if (PauseMenuPanel.activeSelf)
            {
                PauseMenuPanel.SetActive(false);
                HUDPanel.SetActive(true);
                Time.timeScale = 1;
                currentState = GameState.Started;
                //Set "Start Game Boolean" here
            }
        }

        public void StopGame()
        {
            currentState = GameState.Ended;
            gameOverScoreText.text = currentScore.ToString();
            HUDPanel.SetActive(false);
            GameOverPanel.SetActive(true);
            GameOver?.Invoke();
        }


        public void ShowCredits()
        {
            if (StartMenuPanel.activeSelf)
            {
                StartMenuPanel.SetActive(false);
                CreditsPanel.SetActive(true);

            }
        }

        public void CloseCredits()
        {
            if (CreditsPanel.activeSelf)
            {
                CreditsPanel.SetActive(false);
                StartMenuPanel.SetActive(true);


            }
        }


        public void Restart()
        {

            if (PauseMenuPanel.activeSelf || GameOverPanel.activeSelf)
            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //SceneManager.LoadScene(1);


                //Set "Start Game Boolean" here
            }
        }


        public void ShowHint(string hint)
        {
            hintTextBox.text = hint;
        }



        public void UpdateHP(int hp)
        {
            if (currentState != GameState.Started)
                return;
            for (int i = 0; i < hp; i++)
            {
                hpParent.GetChild(i).gameObject.SetActive(true);
            }

            for (int j = hp; j < hpParent.childCount; j++)
            {
                hpParent.GetChild(j).gameObject.SetActive(false);
            }
        }

        public void UpdateCooldown(float timeRemaining, float maxTime)
        {


        }



    }
}
