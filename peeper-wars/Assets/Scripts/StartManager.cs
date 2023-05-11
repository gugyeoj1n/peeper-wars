using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;
using MongoDB.Bson;

public class StartManager : MonoBehaviour
{
    RectTransform titleTextRect;
    public Text titleText;
    bool isChanging = false;
    float cnt = 0f;
    public float nowY = 200f;
    public float nowX = -350f;

    public InputField input_id;
    public InputField input_pw;
    public Text StateText;
    private const string MONGO_URI = "mongodb+srv://gugyeoj1n:woojin9821@peeper-wars.76mjqw0.mongodb.net/";
    private const string DB_NAME = "Main";
    private MongoClient mongoClient;
    private IMongoDatabase db;

    public void SceneMove()
    {
        SceneManager.LoadScene(1);
    }

    void Start()
    {
        titleTextRect = titleText.GetComponent<RectTransform>();
        mongoClient = new MongoClient(MONGO_URI);
        db = mongoClient.GetDatabase(DB_NAME);
    }

    void FixedUpdate()
    {
        if (cnt > 1f) isChanging = true;
        else if (cnt < 0f) isChanging = false;

        if (isChanging == false)
        {
            cnt += 0.01f;
            nowY += 0.2f;
            titleTextRect.anchoredPosition = new Vector2(nowX, nowY);
        } else
        {
            cnt -= 0.01f;
            nowY -= 0.2f;
            titleTextRect.anchoredPosition = new Vector2(nowX, nowY);
        }
    }

    public void TryLogin() {
        if(input_id.text == "" || input_pw.text == "")
        {
            Debug.Log("INVALID INPUT !!");
            return;
        }

        var users = db.GetCollection<BsonDocument>("Users");
        var filter = Builders<BsonDocument>.Filter.Eq("name", input_id.text);

        try {
            var checkUser = users.Find(filter).First();
            if(checkUser.GetValue("password") == input_pw.text)
            {
                Debug.Log("!! LOGIN SUCCEED !! MOVE TO PLAY SCENE");
                SceneMove();
            } else
            {
                Debug.Log("!! INVALID PASSWORD !!");
                return;
            }
        } catch {
            Debug.Log("!! LOGIN FAILED !!");
            return;
        }   
    }
}
