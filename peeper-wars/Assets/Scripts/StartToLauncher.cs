using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartToLauncher : MonoBehaviour
{
    RectTransform titleTextRect;
    public Text titleText;
    bool isChanging = false;
    float cnt = 0f;
    float nowY = 200f;

    public void SceneMove()
    {
        SceneManager.LoadScene(1);
    }

    void Start()
    {
        titleTextRect = titleText.GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        if (cnt > 1f) isChanging = true;
        else if (cnt < 0f) isChanging = false;

        Debug.Log(cnt);

        if (isChanging == false)
        {
            cnt += 0.01f;
            nowY += 0.2f;
            titleTextRect.anchoredPosition = new Vector2(0, nowY);
        } else
        {
            cnt -= 0.01f;
            nowY -= 0.2f;
            titleTextRect.anchoredPosition = new Vector2(0, nowY);
        }
    }
}
