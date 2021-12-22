using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UI를 관리하기 위함
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class PlayerCtrl : MonoBehaviourPun, IPunObservable
{
    private float h = 0f;
    private float v = 0f;
    private Transform tr;
    private Animator animator;
    private PhotonView PV;

    public float Basic_speed = 10f;//기본 속도

    public float speed = 10f;
    public float rotSpeed = 100f;

    private Vector3 currPos;
    private Quaternion currRot;

    public TextMesh playerName;
    public string team = "";

    private Transform team_Spawner;
    public Transform firePos;

    public GameObject bullet;
    public GameObject flag;
    public GameObject Timer;

    private bool isDie = false;
    private int hp = 100;
    private float respwnTime = 3.0f;    
    private Vector3 Knockback_pos;
    
    public bool get_flag = false;
    //플레이어가 떨어졌을 떄 되돌아가는 지점
    //아직 게임 시작할 때 정해진 팀에 해당 팀 스포너를 넣는 코드는 만들지 않음.


    public float fireRange = 50;
    public float BulletSpeed = 10;
    public float fire_delay=0.5f;

    public float base_fireRange = 50;
    public float base_BulletSpeed = 50;
    public float base_fire_delay = 0.5f;

    bool shoot_Fire = true;
    IEnumerator CreateBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
        bullet.GetComponent<Bullet>().team = team;
        bullet.GetComponent<Bullet>().fireRange = this.fireRange;
        bullet.GetComponent<Bullet>().speed = BulletSpeed;
        yield return null;
    }

    void Fire()
    {
        StartCoroutine(CreateBullet());
        PV.RPC("FireRPC", RpcTarget.Others);
        shoot_Fire = false;
        StartCoroutine(Fire_delay_sec());
    }

    IEnumerator Fire_delay_sec()
    {
        yield return new WaitForSeconds(fire_delay);
        shoot_Fire = true;
    }

    [PunRPC]
    void FireRPC()
    {
        animator.SetTrigger("Attack");
        StartCoroutine(CreateBullet());
    }

    private void Start()
    {
        tr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        PV = GetComponent<PhotonView>();
        flag = GameObject.FindGameObjectWithTag("flag");
        Timer = GameObject.Find("Time_System");
        PV.ObservedComponents[0] = this;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (PV.IsMine)
        {
            Camera.main.GetComponent<FollowCam>().Target = tr.Find("Cube").gameObject.transform;
        }
    }

    private void Update()
    {
        if (PV.IsMine && isDie == false)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
            tr.Translate(moveDir.normalized * Time.deltaTime * speed);
            tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));

            if (moveDir.magnitude > 0)
            {
                animator.SetFloat("Speed", 1.0f);
            }
            else
            {
                animator.SetFloat("Speed", 0.0f);
            }

            if (Input.GetButtonDown("Fire1") && shoot_Fire)
            {
                animator.SetTrigger("Attack");
                Fire();
            }
            if (Timer.GetComponent<Time_System_cs>().time <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        else if (!PV.IsMine && isDie == false)
        {
            if (tr.position != currPos)
            {
                animator.SetFloat("Speed", 1.0f);
            }
            else
            {
                animator.SetFloat("Speed", 0.0f);
            }
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10);
            tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 10);
            if (Timer.GetComponent<Time_System_cs>().time <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(name);
            stream.SendNext(team);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            SetPlayerName((string)stream.ReceiveNext());
            SetPlayertema((string)stream.ReceiveNext());
        }
    }
    public void SetPlayerName(string name)
    {
        this.name = name;
        GetComponent<PlayerCtrl>().playerName.text = this.name;
    }
    public void SetPlayertema(string team)
    {
        this.team = team;
    }

    public void SetPlayerTeam(string team)
    {
        this.team = team;
        if(team == "Red")
        {
            team_Spawner = GameObject.Find("RedTeam_SpwanPoint").transform;
        }
        else
        {
            team_Spawner = GameObject.Find("BlueTeam_SpwanPoint").transform;
        }
    }

    public string GetPlayerTeam()
    {
        return this.team;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "PUNCH")//총알에 맞으면 밀림, 깃발을 가지고있으면 깃발도 떨굼
        {
            if (coll.gameObject.GetComponent<Bullet>().team == team)
            {
                Debug.Log(team);
                return;
            }
            animator.SetTrigger("IsHit");
            if (get_flag == true)
            {
                get_flag = false;
                flag.GetComponent<FlagCatch>().Drop_Flag();
            }
            Knockback_pos = coll.transform.forward.normalized;
            this.transform.position += (Knockback_pos * 5.0f);
        }
        if(coll.gameObject.tag == "GOAL_RED")
        {
            if(team == "Red")
            {
                get_flag = false;
                flag.GetComponent<FlagCatch>().Drop_Flag();
            }
        }
        if (coll.gameObject.tag == "GOAL_BLUE")
        {
            if (team == "Blue")
            {
                get_flag = false;
                flag.GetComponent<FlagCatch>().Drop_Flag();
            }
        }
        if (coll.gameObject.tag == "flag")
        {
            get_flag = true;
            flag = coll.gameObject;
            Debug.Log("깃발 획득");
        }
    }

    [PunRPC]
    public void Join_GOAL()
    {
        flag.transform.position = new Vector3(70.0f, 5.0f, 40.0f);
    }
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "DAED_ZONE")
        {
            if (get_flag)
            {
                get_flag = false;
                Join_GOAL();
                PV.RPC("Join_GOAL", RpcTarget.Others);
                flag.GetComponent<FlagCatch>().Drop_Flag();
            }
            tr.position = team_Spawner.position;
        }
    }

    IEnumerator PlayerVisible(bool visibled, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GetComponent<MeshRenderer>().enabled = visibled;
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Path")
        {
            speed = Basic_speed*1.5f;
        }
    }
    private void OnTriggerExit(Collider coll)
    {

        if (coll.tag == "Path")
        {
            speed = Basic_speed;
        }
    }
}
