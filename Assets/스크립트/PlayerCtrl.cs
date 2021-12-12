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

    public float speed = 10f;
    public float rotSpeed = 100f;

    private Vector3 currPos;
    private Quaternion currRot;

    public TextMesh playerName;
    string team = "";

    public Transform firePos;
    public GameObject bullet;
    public GameObject flag;

    private bool isDie = false;
    private int hp = 100;
    private float respwnTime = 3.0f;
    private Vector3 Knockback_pos;

    private bool get_flag = false;
    
    //시간을 관리
    private Time_System_cs TSCS;
    //플레이어 화면에 보여주는 시간 UI
    public Text Time_Text_UI;


    IEnumerator CreateBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
        bullet.GetComponent<Bullet>().team = team;
        yield return null;
    }

    void Fire()
    {
        StartCoroutine(CreateBullet());
        PV.RPC("FireRPC", RpcTarget.Others);
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

        PV.ObservedComponents[0] = this;
        //타임 시스템 스크립트를 가져옴
        TSCS =GameObject.Find("Time_System").GetComponent<Time_System_cs>();
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

            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetTrigger("Attack");
                Fire();
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
        }
        //만약 시간이 0보다 클 시
        if (TSCS.get_Time() > 0)
        {   //현재 시간을 변영
            Time_Text_UI.text = TSCS.get_Time().ToString();
        }
        else
        {
            Time_Text_UI.text = "게임 종료";
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(name);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            SetPlayerName((string)stream.ReceiveNext());
        }
    }
    public void SetPlayerName(string name)
    {
        this.name = name;
        GetComponent<PlayerCtrl>().playerName.text = this.name;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "PUNCH")//총알에 맞으면 밀림, 깃발을 가지고있으면 깃발도 떨굼
        {
            if (coll.gameObject.GetComponent<Bullet>().team == team)
            {
                return;
            }
            //깃발떨굼, get_flag false -> 깃발의 따라가기 비활성화 -> 캐릭터 밀고 -> 콜라이더 활성화
            if (get_flag == true)
            {
                get_flag = false;
                flag.GetComponent<FollowFlag>().enabled = false;
                flag.GetComponent<FlagCatch>().Iscatched = false;
                flag.GetComponent<Rigidbody>().useGravity = true;              
                flag.GetComponent<CapsuleCollider>().enabled = true;
            }
            Knockback_pos = coll.transform.forward.normalized;
            this.transform.position += (Knockback_pos * 5.0f);
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "flag")
        {
            get_flag = true;
            flag = coll.gameObject;
        }
    }

    IEnumerator RespawnPlayer(float waitTime)
    {
        Debug.Log("Died");
        isDie = true;
        StartCoroutine(PlayerVisible(false, 0.0f));
        yield return new WaitForSeconds(waitTime);

        tr.position = new Vector3(Random.Range(-20.0f, 20.0f), 0f, Random.Range(-20.0f, 20.0f));

        hp = 100;
        isDie = false;
        animator.SetTrigger("Reset");
        StartCoroutine(PlayerVisible(true, 1.0f));
    }

    IEnumerator PlayerVisible(bool visibled, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GetComponent<MeshRenderer>().enabled = visibled;
    }

    [PunRPC]
    void OnDamage(int damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //hp -= damage;
            //photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, hp);
            //photonView.RPC("OnDamage", RpcTarget.Others, damage);
        }
        //if (hp <= 0)
        //{
        //    animator.SetTrigger("Die");
        //    StartCoroutine(RespawnPlayer(respwnTime));
        //}
        else
        {
            animator.SetTrigger("IsHit");
        }
    }
    [PunRPC]
    public void ApplyUpdateHealth(int newhp)
    {
        hp = newhp;
    }
}
