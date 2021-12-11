using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
//�÷��̾� ���ο� UI(ȭ�鿡 �ð� �ؽ�Ʈ)�� �ٷ�� ����.
using UnityEngine.UI;


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
    string name = "";

    public Transform firePos;
    public GameObject bullet;

    private bool isDie = false;
    private int hp = 100;
    private float respwnTime = 3.0f;
    //�÷��̾ ������ �ִ� Time_Text�� �������ֱ� ����.
    //getComponent�� �������°� �ƴ϶� ppublic������ �ܺο��� ���������� ������.
    public Text time_text;
    //�ð��� ������ ��ũ��Ʈ.
    Time_System_sc TSSC;

    IEnumerator CreateBullet()
    {
        //Instantiate(bullet, firePos.position, firePos.rotation);
        //bullet.GetComponent<Bullet>().owner = name;
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
        //�ð��� ������ �� �ֵ��� Time_System�� Time_System_sc ��ũ���� �ҷ���.
        TSSC = GameObject.Find("Time_System").GetComponent<Time_System_sc>();
        
        PV.ObservedComponents[0] = this;

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

        //�ð��� �޾ƿ� �ؽ�Ʈ ����
        //0���� ũ�Ⱑ ũ�ٸ� �ʰ� �����.
        if (TSSC.get_time() > 0)
        {
            time_text.text = TSSC.get_time().ToString();
        }
        else
        {
            time_text.text = "���� ����";
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
        if (coll.gameObject.tag == "PUNCH")
        {
            /*if (coll.gameObject.GetComponent<Bullet>().owner == name)
            {
                return;
            }
            if (isDie == true)
            {
                return;
            }
            Debug.Log("Hit");
            Destroy(coll.gameObject);*/

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
            hp -= damage;
            photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, hp);
            photonView.RPC("OnDamage", RpcTarget.Others, damage);
        }
        if (hp <= 0)
        {
            animator.SetTrigger("Die");
            StartCoroutine(RespawnPlayer(respwnTime));
        }
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
