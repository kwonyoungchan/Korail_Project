using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;


// �÷��̾ ���� �������� ���̽��� => ���̷� ������ �� collider��
// �÷��̾ ���� ���⿡ ������ ö�� �ִٸ� ĳ��
public class PlayerForwardRay : MonoBehaviourPun
{
    // ���� ��ġ
    public GameObject rPos;
    public GameObject iPos;
    public bool isBranch;
    // �÷��̾� ����
    PlayerMaterial player;
    // �÷��̾� ������ ����
    PlayerItemDown playerHand;
    PlayerAnim anim;
    RiverGOD riverGOD;
    Theif theif;
    // �ð�
    float currentTime;
    public float waterTime = 4;
    // ��ä����
    public GameObject water;
    Animal animal;

    public bool isWater = false;
    public bool isItemDown = false;
    public bool isMat = false;
    public int cnt;


    public AudioClip[] audioClips;
    AudioSource audioSource;

    // UI
    public Slider slider;
    float audioTime;

    // ä���� ���� ������
    // ingredientItem
    IngredientItem item;

    public bool isGathering = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMaterial>();
        playerHand = GetComponent<PlayerItemDown>();
        anim = GetComponent<PlayerAnim>();
        audioSource = GetComponent<AudioSource>();
        slider.value = 0;
        slider.maxValue = waterTime;
        slider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {

            // �÷��̾ ������ ���̸� ����
            Ray playerRay = new Ray(rPos.transform.position, transform.forward);
            RaycastHit rayInfo;
            // ���� ���� ��ü�� �ִٸ�
            if (Physics.Raycast(playerRay, out rayInfo, 1.3f))
            {
                Debug.DrawRay(rPos.transform.position, transform.forward, Color.blue);
                // ���� ���� ��
                riverGOD = rayInfo.transform.GetComponentInParent<RiverGOD>();
                // ���� ���� ���� �����ϴ� ��
                item = rayInfo.transform.GetComponentInParent<IngredientItem>();
                animal = rayInfo.transform.GetComponent<Animal>();
                theif = rayInfo.transform.GetComponent<Theif>();
                if (riverGOD)
                {
                    isBranch = true;
                    // �÷��̾��� �տ� ������ ������
                    if (player.branchArray.Count > 0)
                    {
                        // ����Ű�� �����ٸ�
                        if (Input.GetButtonDown("Jump"))
                        {
                            if (rayInfo.transform.gameObject.layer == 8)
                            {

                                player.RemoveBranch();
                                // bridge�� �ٲ�
                                riverGOD.ChangeRiver(RiverGOD.River.Bridge);
                            }
                        }
                    }
                    // ���� ��ó�� ������
                    // �絿�̸� ���� �ִٸ�
                    if (playerHand.holdState == PlayerItemDown.Hold.Pail)
                    {
                        currentTime += Time.deltaTime;
                        // UI 작업 = 동기화 동시에 하기
                        WaterSlider(true, currentTime);

                        // ���� �ð� �� �絿�̿� ���� ä������
                        if (currentTime > waterTime)
                        {

                            Water(true);
                        }
                    }
                }
                else
                {
                    isBranch = false;
                }
                // ���� ���� ������ ö�̶���
                if (item)
                {
                    item.isGathering = true;
                    if (playerHand.holdState == PlayerItemDown.Hold.Ax)
                    {
                        if (item.name.Contains("Tree"))
                        {
                            audioSource.clip = audioClips[1];
                            anim.AnimState(PlayerAnim.Anim.Gather);

                            item.isAx = true;
                            item.isPick = false;
                            audioTime += Time.deltaTime;
                            if (audioTime > 1)
                            {
                                audioTime = 0;
                                audioSource.Stop();
                                audioSource.Play();
                            }
                        }
                    }
                    else if (playerHand.holdState == PlayerItemDown.Hold.Pick)
                    {
                        if (item.gameObject.name.Contains("Iron"))
                        {
                            anim.AnimState(PlayerAnim.Anim.Gather);
                            item.isPick = true;
                            item.isAx = false;
                            audioTime += Time.deltaTime;
                            if (audioTime > 1)
                            {
                                audioTime = 0;
                                audioSource.clip = audioClips[0];
                                audioSource.Stop();
                                audioSource.Play();
                            }
                        }
                    }
                    else
                    {
                        item.isAx = false;
                        item.isPick = false;
                    }

                }
                if (animal)
                {
                    if (playerHand.holdState == PlayerItemDown.Hold.Ax || playerHand.holdState == PlayerItemDown.Hold.Pick)
                    {
                        animal.Damage();
                    }
                }

                if (theif)
                {
                    if (playerHand.holdState == PlayerItemDown.Hold.Ax || playerHand.holdState == PlayerItemDown.Hold.Pick)
                    {
                        theif.Damage();
                    }
                }
            }
            else
            {
                WaterSlider(false, 0);
                if (playerHand.holdState != PlayerItemDown.Hold.Mat)
                {
                    if (playerHand.holdState == PlayerItemDown.Hold.Pail)
                    {
                        anim.AnimState(PlayerAnim.Anim.Idle);
                        return;
                    }
                    anim.AnimState(PlayerAnim.Anim.Move);
                }
            }
        }

    }
    void WaterSlider(bool water, float time)
    {
        photonView.RPC("RpcWaterSlider", RpcTarget.All, water, time);
    }
    [PunRPC]
    void RpcWaterSlider(bool isFill, float time)
    {
        // 나의 앞방향을 카메라 앞방향으로 셋팅하자
        slider.transform.forward = Camera.main.transform.forward;
        if (isFill)
        {
            slider.gameObject.SetActive(true);
            slider.value = time;
            if (slider.value == slider.maxValue)
            {
                slider.gameObject.SetActive(false);
            }
        }
        else
        {
            currentTime = 0;
            slider.gameObject.SetActive(false);
            slider.value = 0;
        }
    }
    public void Water(bool s)
    {
        photonView.RPC("RPCWater", RpcTarget.All, s);
    }

    [PunRPC]
    void RPCWater(bool s)
    {
        isWater = s;
        if(isWater == false)
        {
            water.SetActive(false);
        }
        else
        {
            water.SetActive(true);
        }
    }


}
