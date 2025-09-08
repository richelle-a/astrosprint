//importing libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    //go back to play menu
    public void GoBack()
    {
        SceneManager.LoadScene("Play Menu"); 
    }
}

