using Assets.Scripts.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{
    public void LoadByNameSingle(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    public void LoadByNameAdditive(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadByName(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void LoadUnloadByNameSingle(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).IsValid()) SceneManager.UnloadSceneAsync(sceneName);
        else LoadByNameSingle(sceneName);
    }

    public void LoadUnloadByNameAdditive(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).IsValid()) SceneManager.UnloadSceneAsync(sceneName);
        else LoadByNameAdditive(sceneName);
    }
}