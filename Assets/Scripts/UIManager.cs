using UnityEngine;
using Yajulu.Input;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Essentials;

public class UIManager : Singleton<UIManager>
{


    private Main mainInput;



    [SerializeField]
    GameObject StartMenuPanel, PauseMenuPanel, HUDPanel, CreditsPanel;

    bool isGamePaused;


    protected override void OnAwake()
    {
        base.OnAwake();
        mainInput = new Main();

    }
    private void OnEnable()
    {
        mainInput.UI.Enable();
        mainInput.UI.Pause.performed += OnPausePerformed;

    }

    private void OnPausePerformed(InputAction.CallbackContext obj)
    {
        if (!PauseMenuPanel.activeSelf && !StartMenuPanel.activeSelf)
        {
            PauseMenuPanel.SetActive(true);
            HUDPanel.SetActive(false);
            Time.timeScale = 0;
            //Set "Start Game Boolean" here
        }
        else
        {
            PauseMenuPanel.SetActive(false);
            HUDPanel.SetActive(true);
            Time.timeScale = 1;

        }


    }

    public void StartGame()
    {
        Debug.Log("Started");

        if (StartMenuPanel.activeSelf)
        {
            StartMenuPanel.SetActive(false);
            HUDPanel.SetActive(true);
            Time.timeScale = 1;
        }



        //Set "Start Game Boolean" here
    }


    public void Resume()
    {
        if (PauseMenuPanel.activeSelf)
        {
            PauseMenuPanel.SetActive(false);
            HUDPanel.SetActive(true);
            Time.timeScale = 1;
            //Set "Start Game Boolean" here
        }
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
        if (PauseMenuPanel.activeSelf)
        {

            SceneManager.LoadScene(0);


            //Set "Start Game Boolean" here
        }
    }



}
