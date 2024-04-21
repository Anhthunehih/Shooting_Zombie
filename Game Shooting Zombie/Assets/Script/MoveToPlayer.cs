using UnityEngine;
using System.Collections;

public class MoveToPlayer : MonoBehaviour {

    float moveSpeed; // Tốc độ di chuyển của đối tượng
    public float minMoveSpeed = 0.05f; // Tốc độ di chuyển tối thiểu
    public float maxMoveSpeed = 0.3f; // Tốc độ di chuyển tối đa
    GameObject player; // Đối tượng người chơi
    public float attackRange = 1; // Khoảng cách tấn công

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Tìm đối tượng người chơi trong scene
        UpdateMoveSpeed(); // Cập nhật tốc độ di chuyển ban đầu
    }

    // Cập nhật tốc độ di chuyển mới
    void UpdateMoveSpeed()
    {
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed); // Chọn ngẫu nhiên một tốc độ di chuyển trong khoảng min và max
    }

    // Phương thức di chuyển đối tượng đến người chơi
    void Move()
    {
        if (player == null) // Kiểm tra nếu không có người chơi
            return; // Không thực hiện gì cả

        // Kiểm tra nếu khoảng cách giữa đối tượng và người chơi lớn hơn khoảng cách tấn công
        if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
        {
            // Di chuyển đối tượng đến vị trí của người chơi dùng hàm Lerp để mịn
            transform.position = Vector3.Lerp(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Nếu khoảng cách đã đủ gần, đặt trạng thái đối tượng thành đứng yên và chuẩn bị tấn công
            gameObject.GetComponent<Animator>().SetBool("isIdle", true);
            gameObject.GetComponent<ZombieController>().isAttack = true;
            gameObject.GetComponent<MoveToPlayer>().enabled = false; // Tắt script di chuyển
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move(); // Gọi phương thức di chuyển mỗi frame
    }
}
