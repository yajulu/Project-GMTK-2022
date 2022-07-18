using System.Collections;
using System.Collections.Generic;
using Essentials;
using UnityEngine;
using Player;
using UI;
using Core;
public class TutorialManager : MonoBehaviour
{
    public string[] hints;
    public int currentHint;

    public PlayerMainControllerBase playerController;






    public void ActivateNextHint()
    {
        if (currentHint < hints.Length)
        {
            UIManager.Instance.ShowHint(hints[currentHint]);
            currentHint++;
        }

    }



    public void DestroyFirstEnemy()
    {
        Debug.Log(currentHint);
        ActivateNextHint();
        transform.GetChild(currentHint).gameObject.SetActive(true);
        playerController.SwitchPlayerType(Core.ePlayerType.MotorCycle);
        transform.GetChild(currentHint - 1).gameObject.SetActive(false);
    }

    public void DestroySecondEnemy()
    {
        ActivateNextHint();
        transform.GetChild(currentHint).gameObject.SetActive(true);
        playerController.SwitchPlayerType(Core.ePlayerType.Bus);
        transform.GetChild(currentHint - 1).gameObject.SetActive(false);


    }
    public void DestroyThirdEnemy()
    {
        ActivateNextHint();
        transform.GetChild(currentHint).gameObject.SetActive(true);
        transform.GetChild(currentHint - 1).gameObject.SetActive(false);


    }

    public void FinalHint()
    {
        ActivateNextHint();
        transform.GetChild(5).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(false);


    }








}
