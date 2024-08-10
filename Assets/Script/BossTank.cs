using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossTank : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;//Vị trí của Player
    public float speed = 1f;//Tốc độ của Boss
    private Vector3 localScale;
    Animator animator;
    [SerializeField] private float _AttackRange;//Phạm vi tấn của Boss
    private float _AttackBossStart;//Thời gian tấn công của Boss (mặc định là 0)
    [SerializeField] private float _AttackBossCoolDown;//Thời gian cooldown lượt tấn công
    float distancePlayer;//Vị trí của người chơi
    Rigidbody2D rb;
    BoxCollider2D boxCollider2D;
    [SerializeField] private GameObject _BossBullet;//Dạn của Boss
    public Transform _BossAttackTransform;//Vị trí bắn ra viên đạn
    [SerializeField] private GameObject _Metorite;//Đạn của skill 
    public float skill1Cooldown = 1f; // Cooldown cho skill đánh thường
    public float skill2Cooldown = 5f; // Cooldown cho skill 2
    public float skill3Cooldown = 10f; // Cooldown cho skill 3
    public float skill4Cooldown = 10f;// Cooldown cho skill4
    private float nextSkill1Time;//Thời gian tiếp theo để sử dụng skill(mặc định ban đầu là 0)
    private float nextSkill2Time;
    private float nextSkill3Time;
    private float nextSkill4Time;
    public GameObject _Lazer;//Đạn của skill
    public Slider hpBossTankSlider;//Thanh slider của Boss
    public int _HpBossTankValue;//Giá trị máu của Boss
    public TextMeshProUGUI _HpText;
    public GameObject _OffSlider;
    bool isDead;//Biến xác định Boss đã chết chưa
    bool _CanUseSkillTele;//Biến xác định Boss có được phép tele không
    private bool isPreparingToTeleport = false;//Biến xác định chuẩn bị tele
    public TextMeshProUGUI _DameText;
    GameController1 gameController1;//Script của GameController1
    AudioSource audioSource;
    public AudioClip audioClipHitEnemy;
    public AudioClip audioClipDead;
    public AudioClip audioClipLazedSkill;
    void Start()
    {
        // Lưu trữ scale ban đầu của Boss
        localScale = transform.localScale;
        animator = GetComponent<Animator>();
        _AttackBossStart = 0;
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        hpBossTankSlider.maxValue = 500;
        _HpBossTankValue = 500;
        _HpText.text = _HpBossTankValue.ToString("");
        isDead = false;
        _CanUseSkillTele = false;
        gameController1 = FindObjectOfType<GameController1>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        distancePlayer = Vector2.Distance(transform.position, player.position);
        // Tính khoảng cách của nhân vật đến Boss
        Debug.Log("Khoảng cách của player hiện tại là: " + distancePlayer);
        if (player.position.x > transform.position.x)
        {
            _HpText.transform.localScale = new Vector3(1, 1, 1);
            // Người chơi ở bên phải, quay mặt sang phải
            transform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
        else if (player.position.x < transform.position.x)
        {
            _HpText.transform.localScale = new Vector3(-1, 1, 1);
            // Người chơi ở bên trái, quay mặt sang trái
            transform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
        if (distancePlayer > _AttackRange)//Nếu mà vị trí của Player lớn hơn pv tấn công của Boss
        {
            MoveBoss();//Boss sẽ di chuyển
        }
        else
        {
            if (distancePlayer <= _AttackRange) // Nếu mà vị trí của người nhỏ hơn phạm vi
                                                   // di chuyển của Boss thì Boss sẽ tiến hành xác định điều kiện tiếp theo
            {
                animator.SetBool("IsBossRun", false);
                BossSkillAttack();//Nếu người chơi nằm trong pv tấn công
                //Boss được phép tấn công
            }
        }
        if (_HpBossTankValue <= 0 && isDead == false)
        {//Nếu máu của Boss <=0 và biến isDead == false
            _OffSlider.SetActive(false);//Tắt thanh máu của Boss
            audioSource.PlayOneShot(audioClipDead);//Sound Boss
            animator.SetTrigger("IsBossDead");//chạy aniamtion của Boss
            isDead = true;//Biến isDead chuyển sang true nhằm xác định
            //Boss chỉ được phép chết 1 lần 
            Destroy(gameObject, 2f);//Phá hủy đối tượng sau 2s
        }
    }
    public void MoveBoss()
    {
        if (_HpBossTankValue <= 0) return;//Nếu máu của Boss <=0
        //Boss không được phép di chuyển
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Lấy trạng thái Aniamtion hiện tại của Boss
        if(stateInfo.IsName("Boss_TeleStartAnimation")||
           stateInfo.IsName("Boss_teleAnimation")) { return; }
        //Nếu mà Boss Đang trọng trạng thái tele thì Boss không thể di chuyển
        // Tính toán hướng tới người chơi nhưng chỉ trên trục X
        else
        {
            animator.SetBool("IsBossRun", true);
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
            //Lấy vị trí của người chơi
            // Di chuyển Boss tới vị trí mới
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            // Xác định hướng của Boss dựa trên vị trí của người chơi
        }
    }
    public int randomskillPercent()//Hàm dùng để xác định tỉ lệ ra skill của Boss
    {
        int randomSkill = Random.Range(0, 101);//Biến chạy từ 0 -100
        if (randomSkill <= 20)
        {
            //20% là Boss sẽ đánh thường
            return 1;
        }
        else if (randomSkill <= 40)
        {
            // 20% xác suất cho skill 3
            return 3;
        }
        if (randomSkill <= 60)
        {
            // 20% xác suất cho skill 2
            return 2;
        }
        else
        {
            //60% là ra skill 4
            return 4;
        }
    }
    public void BossSkillAttack()//Hàm tấn công của Boss
    {
        int skill = randomskillPercent();
        Debug.Log("Hiện tại skill nằm trong là: " + skill);
        _AttackBossStart -= Time.deltaTime;
        if (_AttackBossStart <= 0)
        {
            Debug.Log("Dã chạy");
            switch (skill)
            {
                case 1:
                    if(Time.time >= nextSkill1Time)
                    {
                        //Lấy thời gian khi bắt đầu game so sánh với thời gian tiếp theo
                        //ra skill
                        _CanUseSkillTele = true;//Biến này có tác dụng Boss phải dùng 
                        //1 skill nào đó để có thể tele
                        Debug.Log(">>>>>>>>>>>>>1");
                        animator.SetBool("IsBossAttack", true);
                        nextSkill1Time = Time.time + skill1Cooldown;
                        //Tạo mới khoảng thời gian hồi chiêu
                    }                  
                    break;
                case 2:
                    if (Time.time >= nextSkill2Time)
                    {
                        _CanUseSkillTele = true;
                        Debug.Log(">>>>>>>>>>>>>2");
                        animator.SetBool("IsBossAttack2", true);
                        nextSkill2Time = Time.time + skill2Cooldown;
                    }                   
                    break;
                case 3:
                    if(Time.time >= nextSkill3Time)
                    {
                        Debug.Log(">>>>>>>>>>>>>3");
                        _CanUseSkillTele = true;
                        animator.SetBool("IsBossAttack3", true);
                        nextSkill3Time = Time.time + skill3Cooldown;
                    }
                    break;
                case 4:
                    if (Time.time >= nextSkill4Time)
                    {
                        if (_CanUseSkillTele == true)//Sau khi dùng skill bất kỳ 
                        {
                            //thì được phép tele
                            StartCoroutine(PrepareToTeleport());
                            nextSkill4Time = Time.time + skill4Cooldown;
                        }
                    }
                    break;
            }
            _AttackBossStart = _AttackBossCoolDown;//Cooldown lại tốc độ tấn công
        }
    }
    public void BossNormalAttack()//Hàm tạo ra viên đạn của normalAttack
    {
        Debug.Log("Đã tạo ra viên đạn");
        var createBullet = Instantiate(_BossBullet, _BossAttackTransform.position, Quaternion.identity);
        var speedBullet = new Vector2(3f, 0);
        createBullet.transform.localScale = new Vector3(6, 6, 6);
        if (transform.localScale.x < 0)
        {
            speedBullet = new Vector2(-2f, 0);
            createBullet.transform.localScale = new Vector3(-6, -6, -6);
        }
        createBullet.GetComponent<Rigidbody2D>().velocity = speedBullet;
        Destroy(createBullet, 2f);
    }
    public void EndNormalAttack()
    {
        animator.SetBool("IsBossAttack", false);
    }//Hàm Event dùng để kết thúc hoạt ảnh normalAttack
    public void EndSkill1()
    {
        animator.SetBool("IsBossAttack2", false);
    }//Kết thúc hoạt ảnh của Skill1
    public void EndSkill2()
    {
        animator.SetBool("IsBossAttack3", false);
    }
    public void CreateMetorite()//Hàm khởi chạy cho skill1 
    {
        StartCoroutine(WaitCreateMetorite());
    }
    IEnumerator WaitCreateMetorite()//Hàm tạo đạn và spawn từng giây
    {
        for (int i = 0; i < 30; i++)
        {
            var randomPostion = new Vector2(Random.Range(-7.69f, 7.32f), 6);
            var createMetorite = Instantiate(_Metorite, randomPostion, Quaternion.Euler(0, 0, -90.322f));
            var speedfall = new Vector2(0, -3f);
            createMetorite.GetComponent<Rigidbody2D>().velocity = speedfall;
            yield return new WaitForSeconds(1.5f);
        }
    }
    private IEnumerator PrepareToTeleport()
    {
        isPreparingToTeleport = true;//Boss đang chuẩn bị Tele
        Debug.Log(">>>>>>>>>4");
        // Tắt hoạt ảnh bị tấn công trong thời gian đệm
        animator.ResetTrigger("IsBossHurt");//Tạm dừng trạng thái Animation
        //tránh trường hợp bị gián đoạn quá trình tele
        yield return new WaitForSeconds(0.5f); // Thời gian đệm

        // Bắt đầu hoạt ảnh teleport
        animator.SetBool("IsBossAttack", false);//Tắt hết các Animation tấn công
        animator.SetBool("IsBossAttack2", false);
        animator.SetBool("IsBossAttack3", false);
        animator.SetTrigger("IsBossStartTele");//Khởi chạy hoạt ảnh tele

        // Đợi hoạt ảnh teleport kết thúc
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("Boss_teleAnimation"))
        {// // Nếu hoạt ảnh hiện tại không phải là "Boss_teleAnimation",
         // tiếp tục lấy trạng thái hiện tại
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //Lấy trạng thái của Animation hiện tại
            yield return null;//Chờ đợi frame tiếp theo để kiểm tra lại
        }

        yield return new WaitForSeconds(stateInfo.length);//tạm dừng theo độ dài của Animation
        // hiện tại
        isPreparingToTeleport = false;
    }
    public void EndSkillBossTele()
    {
        animator.SetTrigger("IsBossIdle");
    }//hàm kết thúc hoạt ảnh tele(Event)
    public void CreateLaze()
    {

        StartCoroutine(LazerSpawn());
    }//Hàm khởi chạy skill2
    IEnumerator LazerSpawn()//Hàm tạo ra laze và chạy trong từng giây
    {
        for (int i = 0; i < 30; i++)
        {
            var spawnLazerPosition = new Vector2(Random.Range(-7.3f, 7.8f), 1.6f);
            var createLaze = Instantiate(_Lazer, spawnLazerPosition, Quaternion.Euler(0,0,90));
            createLaze.transform.localScale = new Vector2(18.16f, 8.218f);
            audioSource.PlayOneShot(audioClipLazedSkill);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void TakeDameBoss(int dame)//Hàm Boss nhân dame
    {
        if (isPreparingToTeleport) return;//Nếu mà Boss đang trong trạng thái tele
        //thì không thể nhận dame
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //Lấy trạng thái Animation hiện tại của Boss
        if (stateInfo.IsName("Boss_TeleStartAnimation") || stateInfo.IsName("Boss_teleAnimation"))
        {
            return;
        }

        if (_HpBossTankValue > 0)//Nếu máu của Boss lớn hơn 0
        {
            animator.SetTrigger("IsBossHurt");
            audioSource.PlayOneShot(audioClipHitEnemy);
            _HpBossTankValue -= dame;
            if (_HpBossTankValue <= 0) { _HpBossTankValue = 0; }
            gameController1.StartDameText(dame, _DameText, gameObject.transform);
            hpBossTankSlider.value = _HpBossTankValue;
            _HpText.text = _HpBossTankValue.ToString("");
            animator.SetTrigger("IsBossIdle");
        }
    }
    public void BossFinishedTele()//Hàm này dùng để xác định việc hoàn thành tele
    {
        animator.SetTrigger("IsBossFinishedTele");
        // Thực hiện teleport
        if (transform.position.x > 0)
        {
            transform.position = new Vector2(-7.88f, -1.07f);
        }
        else if (transform.position.x < 0)
        {
            transform.position = new Vector2(6.57f, -1.07f);
        }
    }
}
