using UnityEngine;
using System.Collections;

public class BarsEffect : MonoBehaviour {

    [SerializeField]
    private Transform[] bars;

    [SerializeField]
    private float showValue;
    [SerializeField]
    private float hideValue;
    [SerializeField]
    private float speed;

    public void SetLetterbox(bool state)
    {
        if (state)
        { //we want letterboxing
            StopAllCoroutines();
            StartCoroutine("ShowBars", bars[0]);
            StartCoroutine("ShowBars", bars[1]);
        }
        else //we don't want letterboxing
        {
            StopAllCoroutines();
            StartCoroutine("HideBars", bars[0]);
            StartCoroutine("HideBars", bars[1]);
        }
    }

    public IEnumerator HideBars(Transform bar)
    {
        while (bar.position.y != hideValue)
        {
            float newYvalue = Mathf.MoveTowards(bar.position.y, transform.position.y + hideValue, Time.deltaTime * speed);
            bar.position = new Vector3(bar.position.x, newYvalue, bar.position.z);
            yield return null;
        }
    }

    public IEnumerator ShowBars(Transform bar)
    {
        while (bar.position.y != showValue)
        {
            float newYvalue = Mathf.MoveTowards(bar.position.y, transform.position.y + showValue, Time.deltaTime * speed);
            bar.position = new Vector3(bar.position.x, newYvalue, bar.position.z);
            yield return null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetLetterbox(true);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetLetterbox(false);
    }
}
