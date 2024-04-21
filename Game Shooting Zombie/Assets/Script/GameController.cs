using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Sử dụng để quản lý các scene trong Unity
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject menu;
    public Text txtPoint; // Text hiển thị số điểm trong thanh menu
    private int currentPoint = 0; // Số điểm hiện tại

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1; // Đặt scale thời gian về giá trị mặc định (1) khi trò chơi bắt đầu
        txtPoint.text = "Zombie killed: " + currentPoint.ToString(); // Hiển thị số điểm trên thanh menu
        menu.SetActive(false); // Ẩn menu khi trò chơi bắt đầu
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GetPoint(int point)
    {
        currentPoint++; // Tăng số điểm
        txtPoint.text = "Zombie killed: " + currentPoint.ToString(); // Hiển thị số điểm trên thanh menu
        SaveScore(); // Lưu số điểm vào PlayerPrefs sau mỗi lần tăng điểm
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0); // Tải lại scene hiện tại khi người chơi chọn khởi động lại trò chơi
    }
    public void GoShopNFT()
    {
        SceneManager.LoadScene("NFT Store");
    }
    public void GoToMap1()
    {
        SceneManager.LoadScene("Map1");
    }
    public void GoToMap2()
    {
       
    }
    public void GoToMap3()
    {
       
    }
    public void GoToMap4()
    {
       
    }
    public void GoToMap5()
    {
       
    }
    public void GoToMap6()
    {
       
       
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Start"); // Tên của scene "TrangChu"
    }
    public void GoToChooseMap()
    {
        SceneManager.LoadScene("ChooseMap"); // Tên của scene "ChooseMap"
    }
    public void EndGame()
    {
        menu.SetActive(true); // Hiển thị menu khi trò chơi kết thúc
        Time.timeScale = 0; // Đặt scale thời gian về 0 để tạm dừng trò chơi
    }

    // Phương thức để lưu số điểm vào PlayerPrefs
    private void SaveScore()
    {
        PlayerPrefs.SetInt("CurrentPoint", currentPoint); // Lưu số điểm vào PlayerPrefs
        PlayerPrefs.Save(); // Lưu thay đổi vào bộ nhớ
    }
}
