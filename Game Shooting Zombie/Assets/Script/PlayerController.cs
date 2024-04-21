using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int damge = 1; // Sát thương mà người chơi gây ra
    public float fireTime = 0.3f; // Thời gian giữa mỗi lần bắn
    public GameObject smoke; // Đối tượng smoke để hiển thị hiệu ứng khi bắn
    public GameObject gunHead; // Điểm bắn của vũ khí
    public float playerHealth = 10; // Mức máu của người chơi
    public AudioClip playerDeathSound; // Âm thanh khi người chơi chết
    public Slider healthBar; // Thanh trạng thái máu của người chơi

    // Các biến private được sử dụng bên trong lớp
    private float playerCurrentHealth = 10; // Mức máu hiện tại của người chơi
    private float lastFireTime = 0; // Thời điểm cuối cùng khi bắn
    private Animator anim; // Đối tượng Animator để điều khiển animation
    private AudioSource audioS; // Đối tượng AudioSource để phát âm thanh
    private GameObject gameController; // Đối tượng GameController để gọi các phương thức trong lớp GameController

    // Hàm Start được gọi khi đối tượng được khởi tạo
    void Start()
    {
        anim = gameObject.GetComponent<Animator>(); // Lấy Animator của đối tượng
        UpdateFireTime(); // Cập nhật thời gian bắn
        audioS = gameObject.GetComponent<AudioSource>(); // Lấy AudioSource của đối tượng
        gameController = GameObject.FindGameObjectWithTag("GameController"); // Tìm đối tượng GameController trong scene

        // Thiết lập giá trị ban đầu cho thanh trạng thái máu
        healthBar.maxValue = playerHealth;
        healthBar.value = playerCurrentHealth;
        healthBar.minValue = 0;
    }

    // Hàm cập nhật thời gian bắn
    void UpdateFireTime()
    {
        lastFireTime = Time.time;
    }

    // Hàm thiết lập animation bắn
    void SetFireAnim(bool isFire)
    {
        anim.SetBool("isShoot", isFire);
    }

    // Phương thức xử lý khi nhận sát thương
    public void GetHit(float damge)
    {
        audioS.Play(); // Phát âm thanh khi nhận sát thương
        playerCurrentHealth -= damge; // Giảm máu của người chơi
        healthBar.value = playerCurrentHealth; // Cập nhật thanh trạng thái máu

        // Kiểm tra nếu máu của người chơi dưới 0 thì chết
        if (playerCurrentHealth <= 0)
        {
            Dead(); // Gọi phương thức chết
        }
    }

    // Phương thức xử lý khi người chơi chết
    void Dead()
    {
        audioS.clip = playerDeathSound; // Thiết lập âm thanh khi chết
        audioS.Play(); // Phát âm thanh khi chết
        gameController.GetComponent<GameController>().EndGame(); // Gọi phương thức kết thúc trò chơi từ GameController
    }

    // Hàm bắn
    void Fire()
    {
        // Kiểm tra nếu đã đến thời điểm bắn
        if (Time.time >= lastFireTime + fireTime)
        {
            // Tạo một tia ray từ vị trí chuột
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#if UNITY_IOS || UNITY_ANDROID
            // Nếu là di động, sử dụng tia ray từ vị trí chuột
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            // Kiểm tra va chạm với đối tượng
            if (Physics.Raycast(ray, out hit))
            {
                // Nếu va chạm với đối tượng có tag là "Zombie"
                if (hit.transform.tag.Equals("Zombie"))
                {
                    SetFireAnim(true); // Thiết lập animation bắn
                    InsSmoke(); // Tạo hiệu ứng smoke
                    hit.transform.gameObject.GetComponent<ZombieController>().GetHit(damge); // Gọi phương thức nhận sát thương của ZombieController
                }
            }
#else
            // Nếu không phải di động, sử dụng tia ray từ gunHead
            RaycastHit hit;

            // Kiểm tra va chạm với đối tượng
            if (Physics.Raycast(gunHead.transform.position, gunHead.transform.forward, out hit))
            {
                // Nếu va chạm với đối tượng có tag là "Zombie"
                if (hit.transform.tag.Equals("Zombie"))
                {
                    SetFireAnim(true); // Thiết lập animation bắn
                    InsSmoke(); // Tạo hiệu ứng smoke
                    hit.transform.gameObject.GetComponent<ZombieController>().GetHit(damge); // Gọi phương thức nhận sát thương của ZombieController
                }
            }
#endif
            UpdateFireTime(); // Cập nhật thời gian bắn
        }
        else
        {
            SetFireAnim(false); // Không bắn
        }
    }

    // Hàm tạo hiệu ứng smoke
    void InsSmoke()
    {
        GameObject sm = Instantiate(smoke, gunHead.transform.position, gunHead.transform.rotation) as GameObject; // Tạo ra đối tượng smoke
        Destroy(sm, 0.5f); // Hủy đối tượng smoke sau một khoảng thời gian
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) // Kiểm tra nếu người chơi nhấn chuột trái
        {
            Fire(); // Bắn
        }
    }
}
