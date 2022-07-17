using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDestinationPoint : MonoBehaviour
{
    TutorialManager tutorialManager;


    void Start()
    {
        tutorialManager = transform.parent.GetComponent<TutorialManager>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        tutorialManager.ActivateNextHint();
        tutorialManager.transform.GetChild(tutorialManager.currentHint).gameObject.SetActive(true);
        this.gameObject.SetActive(false);

    }

}
