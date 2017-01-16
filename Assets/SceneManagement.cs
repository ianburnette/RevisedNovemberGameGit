using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour {
    #region PrivateProperties
    [SerializeField] Vector3[] mainWorldLocations;
    [SerializeField] Transform player;
    #endregion

    #region PublicVariables
        public static SceneManagement publicSceneManager;
    #endregion

    #region UnityFunctions
    void Start()
    {
       // locations = new Vector3[8][3];
        publicSceneManager = this;
    }
    #endregion

    #region CustomFunctions
    public void ChangeLevel(int levelIndex, int location, int version)
    {
        SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Single);
        SetLocation(levelIndex, location, version);
    }

    void SetLocation(int index, int location, int version)
    {
        switch (index)
        {
            case 0:     //MAIN WORLD
                player.transform.position = mainWorldLocations[location];
                break;
            case 1:     //TOOL SHED
                //no differences at present
                break;
        }
    }
    #endregion
}
