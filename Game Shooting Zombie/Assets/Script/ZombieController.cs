using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {

    public int zombieHealth = 3; // Mức độ máu của zombie
    public float shootTime = 0.5f; // Thời gian giữa các lần bắn của zombie
    public bool isAttack = false; // Trạng thái tấn công của zombie
    public float attackTime = 1; // Thời gian giữa các lần tấn công
    public AudioClip zombieDeathSound; // Âm thanh khi zombie chết

    private Animator anim; // Đối tượng Animator để điều khiển animation của zombie
    private bool isShooten; // Trạng thái bắn của zombie
    private bool isDead = false; // Trạng thái đã chết của zombie
    private float lastAttackTime = 0; // Thời gian lần tấn công cuối cùng
    private AudioSource audioS; // Đối tượng AudioSource để phát âm thanh của zombie
    private float lastShootenTime = 0; // Thời gian lần bắn cuối cùng
    private GameObject player; // Đối tượng người chơi
    private GameObject gameController; // Đối tượng GameController để ghi điểm
    public float damge = 1; // Sát thương gây ra bởi zombie

    public bool IsShooten // Thuộc tính cho biết trạng thái bắn của zombie
    {
        get { return isShooten; } // Trả về trạng thái bắn của zombie
        set // Thiết lập trạng thái bắn của zombie
        {
            isShooten = value; // Thiết lập trạng thái bắn
            ShootenAnim(isShooten); // Gọi phương thức để chạy animation bắn của zombie
            UpdateShootenTime(); // Cập nhật thời gian bắn cuối cùng
        }
    }

    // Hàm khởi tạo
    void Start()
    {
        anim = gameObject.GetComponent<Animator>(); // Lấy Animator của zombie
        IsShooten = false; // Khởi tạo trạng thái bắn của zombie
        anim.SetBool("isDead", false); // Thiết lập trạng thái chết của zombie
        audioS = gameObject.GetComponent<AudioSource>(); // Lấy AudioSource của zombie
        player = GameObject.FindGameObjectWithTag("Player"); // Lấy đối tượng người chơi
        gameController = GameObject.FindGameObjectWithTag("GameController"); // Lấy đối tượng GameController
    }

    // Cập nhật thời gian bắn cuối cùng của zombie
    void UpdateShootenTime()
    {
        lastShootenTime = Time.time; // Gán thời gian bắn cuối cùng bằng thời gian hiện tại
    }

    // Cập nhật thời gian tấn công cuối cùng của zombie
    void UpdateAttackTime()
    {
        lastAttackTime = Time.time; // Gán thời gian tấn công cuối cùng bằng thời gian hiện tại
    }

    // Phát animation bắn của zombie
    void ShootenAnim(bool isShooten)
    {
        anim.SetBool("isShooten", isShooten); // Thiết lập animation bắn của zombie
    }

    // Phát animation tấn công của zombie
    void AttackAnim(bool isAttack)
    {
        anim.SetBool("isAttack", isAttack); // Thiết lập animation tấn công của zombie
    }

    // Phương thức xử lý khi zombie nhận sát thương
    public void GetHit(int damge)
    {
        if (isDead) // Nếu zombie đã chết, thoát khỏi phương thức
            return;
        audioS.Play(); // Phát âm thanh khi zombie nhận sát thương
        IsShooten = true; // Thiết lập zombie đã bị bắn
        zombieHealth -= damge; // Giảm máu của zombie

        if (zombieHealth <= 0) // Nếu máu của zombie dưới hoặc bằng 0
        {
            Dead(); // Zombie chết
        }
    }

    // Phương thức xử lý khi zombie chết
    void Dead()
    {
        isDead = true; // Đánh dấu zombie đã chết
        audioS.clip = zombieDeathSound; // Thiết lập âm thanh khi zombie chết
        audioS.Play(); // Phát âm thanh khi zombie chết
        anim.SetBool("isDead", true); // Phát animation chết của zombie
        gameController.GetComponent<GameController>().GetPoint(1); // Ghi điểm
        Destroy(gameObject, 2f); // Hủy đối tượng zombie sau 2 giây
    }

    // Phương thức xử lý khi zombie tấn công
    void Attack()
    {
        if (Time.time >= lastAttackTime + attackTime) // Nếu đã đủ thời gian để tấn công lại
        {
            AttackAnim(true); // Phát animation tấn công của zombie
            UpdateAttackTime(); // Cập nhật thời gian tấn công cuối cùng
            player.GetComponent<PlayerController>().GetHit(damge); // Người chơi nhận sát thương
        }
        else
        {
            AttackAnim(false); // Dừng animation tấn công của zombie
        }
    }

    // Hàm Update được gọi mỗi frame
    void Update()
    {
        if (IsShooten && Time.time >= lastShootenTime + shootTime) // Nếu zombie đã bị bắn và đã đủ thời gian để kết thúc animation bắn
        {
            IsShooten = false; // Không còn bắn nữa
        }
        if (isAttack) // Nếu zombie đang tấn công
        {
            Attack(); // Thực hiện tấn công
        }
    }
}
