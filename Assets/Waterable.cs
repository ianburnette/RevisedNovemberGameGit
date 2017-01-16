using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Waterable : MonoBehaviour {

    #region PrivateProperties
    [Range(0f,1f)]
    [SerializeField]float wateredAmt;
    [SerializeField] bool important;
    [SerializeField] string variableName;
    [SerializeField] ExampleVariableStorage variables;
    [SerializeField] GameObject plant;
    [SerializeField] float targetScale;
    [SerializeField] float scaleSpeed;
        [SerializeField] float waterSpeed;
        Material myMat;
        [SerializeField] Color normalColor, wateredColor;
    #endregion

    #region PublicVariables
        public float WateredAmt{get{return wateredAmt;}set{ UpdateWateredAmt(); wateredAmt = value;}}
    #endregion

    #region UnityFunctions
    private void Start()
    {
        DOTween.Init();
        myMat = GetComponent<Renderer>().material;
    }

    void Update () {
        myMat.color = Color.Lerp(normalColor, wateredColor, wateredAmt);
    }
    #endregion

    #region CustomFunctions
     void UpdateWateredAmt()
    {
        wateredAmt += waterSpeed;
      
        if (wateredAmt >= 1.1f && transform.tag != "watered")
            Water();
    }
    void Water()
    {
        transform.tag = "watered";
        print("scale here");
        if (important)
            Increment();
     //   plant.transform.DOScale(targetScale, scaleSpeed);
    }
    void Increment()
    {
        variables.PlantsWatered++;

    }
        #endregion
    }
