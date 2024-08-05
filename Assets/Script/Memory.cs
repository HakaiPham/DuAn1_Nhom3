using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Memory : MonoBehaviour
{
    // Start is called before the first frame update
    GameController1 _Controller1;
    GameData _DataGame;
    public TextMeshProUGUI _BestScore;
    public TextMeshProUGUI _CurrentScore;
    void Start()
    {
        _Controller1 = FindObjectOfType<GameController1>();
    }

    // Update is called once per frame
    void Update()
    {
        //ReadData
        //ShowData
        //Update new Data
        if (GameController1.coin > 0)
        {
            ReadData();
            ShowData();
            WriteDatatoFile();
        }
    }
    public void ReadData()
    {
        _DataGame = DataManager.ReadData();
        if (_DataGame == null)
        {
            _DataGame = new GameData() { coin = 0 };
        }
    }
    public void ShowData()
    {
        //Score sau khi chơi
        var coinScore = GameController1.GetScore();
        //Score từ File
        var scoreFromFile = _DataGame.coin;
        //Lấy điểm cao nhất
        var bestScore = Mathf.Max(coinScore, scoreFromFile);
        _BestScore.text = "BestScore: "+bestScore;
        _CurrentScore.text = "CurrentScore: " + coinScore;
        //Save dữ liệu vào file
        //lấy điểm lớn nhất
        _DataGame.coin = bestScore;
    }
    public void WriteDatatoFile()
    {
        DataManager.SAVEDATA(_DataGame);
    }
}
