using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string trainingScene = "TrainingScene";

    public void goToTrainingScene()
    {
        SceneManager.LoadScene(trainingScene);
    }
}
