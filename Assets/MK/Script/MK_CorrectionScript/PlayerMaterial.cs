using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 material을 판별
#region material 판별 로직 
// 플레이어가 Ray를 쏘고 다닌다
// 만약 플레이어가 손에 branch나 steel을 들고 있다면 
// branch나 steel이 위로 쌓인다
// 아닐 경우
// 점프키를 누르면 
// 스테이트가 변경된다
#endregion
public class PlayerMaterial : MonoBehaviour
{
    // 리스트
    // 나뭇가지
    public List<GameObject> branchArray = new List<GameObject>();
    public List<GameObject> steelArray = new List<GameObject>();
    // 아이템 위치
    public Transform itemPos;
    // MaterialGod 컴포넌트
    MaterialGOD matGod;
    // ToolGod 컴포넌트
    ToolGOD toolGOD;
    // 플레이어 손 상태 컴포넌트
    PlayerItemDown playerItem;

    // Start is called before the first frame update
    void Start()
    {
        playerItem = GetComponent<PlayerItemDown>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Branch"))
        {
            // 플레이어가 레이를 발사한다
            Ray pRay = new Ray(transform.position, -transform.up);
            RaycastHit rayInfo;
            if (Physics.Raycast(pRay, out rayInfo))
            {
                matGod = rayInfo.transform.gameObject.GetComponent<MaterialGOD>();
                toolGOD = rayInfo.transform.gameObject.GetComponent<ToolGOD>();
                
                // 손에 무언갈 들고 있다면
                if (branchArray.Count > 0 || steelArray.Count > 0) 
                {
                    if (toolGOD == null)
                    {
                        return;
                    }
                    #region branch가 손에 있는 경우
                    if (branchArray.Count > 0)
                    {
                        // 바닥 상태가 Branch라면
                        if (matGod.matState == MaterialGOD.Materials.Branch)
                        {
                            // Array에 추가하기
                            MakeMat("MK_Prefab/Branch", branchArray);
                            // 손 위치 위로 아이템 쌓게 만들기
                            for (int i = 0; i < branchArray.Count; i++)
                            {
                                branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                            }
                            matGod.matState = MaterialGOD.Materials.None;

                        }
                        // 바닥 상태가 idle이라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                DeleteBranch();
                            }
                        }
                        // 바닥 상태가 Ax라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Ax)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Ax;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteBranch();
                            }
                        }
                        // 바닥 상태가 Pick라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Pick)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Pick;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteBranch();
                            }
                        }
                        // 바닥 상태가 Pail라면
                        if (toolGOD.toolsState == ToolGOD.Tools.Pail)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                playerItem.holdState = PlayerItemDown.Hold.Pail;
                                toolGOD.toolsState = ToolGOD.Tools.Idle;
                                DeleteBranch();
                            }
                        }
                    }
                    #endregion
                    #region Steel가 손에 있는 경우
                    if (steelArray.Count > 0)
                    {
                        // 바닥 상태가 Steel 상태라면
                        if (matGod.matState == MaterialGOD.Materials.Steel)
                        {
                            // Array에 추가하기
                            MakeMat("MK_Prefab/Steel", steelArray);
                            // 손 위치 위로 아이템 쌓게 만들기
                            for (int i = 0; i < steelArray.Count; i++)
                            {
                                steelArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                steelArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                            }
                            matGod.matState = MaterialGOD.Materials.None;

                        }
                        // 바닥 상태가 idle이라면
                        else if (toolGOD.toolsState == ToolGOD.Tools.Idle)
                        {
                            // 점프키를 눌렀을 때,
                            if (Input.GetButtonDown("Jump"))
                            {
                                // 바닥에 손에 있는 개수만큼 쌓임
                                matGod.steelCount = steelArray.Count;
                                matGod.matState = MaterialGOD.Materials.Steel;
                                // 손에 있는 모든 것들이 제거
                                for (int i = 0; i < steelArray.Count; i++)
                                {
                                    Destroy(steelArray[i].gameObject);
                                }
                                steelArray.Clear();
                            }
                        }
                    }
                    #endregion
                }
                // 손에 없고
                else
                {
                    // 점프키를 눌렀을 때,
                    if (Input.GetButtonDown("Jump"))
                    {
                        // 바닥 상태가 Branch라면
                        if (matGod.matState == MaterialGOD.Materials.Branch)
                        {
                            // 바닥에 branch가 여러개인 경우
                            if (matGod.branchCount > 0)
                            {
                                for(int i = 0; i < matGod.branchCount; i++)
                                {
                                    MakeMat("MK_Prefab/Branch", branchArray);
                                    branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                    branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                }
                            }
                            // 플레이어 손상태 변환
                            playerItem.holdState = PlayerItemDown.Hold.Branch;
                            // 바닥상태 변환
                            matGod.matState = MaterialGOD.Materials.None;
                        }
                        // Steel이라면
                        if (matGod.matState == MaterialGOD.Materials.Steel)
                        {
                            // 바닥에 steel이 여러개인 경우
                            if (matGod.steelCount > 0)
                            {
                                for (int i = 0; i < matGod.branchCount; i++)
                                {
                                    MakeMat("MK_Prefab/Steel", steelArray);
                                    steelArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.2f, 0);
                                    steelArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                                }
                            }
                            // 플레이어 손상태 변환
                            playerItem.holdState = PlayerItemDown.Hold.Steel;
                            // 바닥상태 변환
                            matGod.matState = MaterialGOD.Materials.None;
                        }

                    }
                }
            }
        }
    }

    void MakeMat(string s, List<GameObject> matArray)
    {
        // Array에 추가하기
        GameObject mat = Instantiate(Resources.Load<GameObject>(s));
        mat.transform.parent = itemPos;
        matArray.Add(mat);
    }

    void DeleteBranch()
    {
        matGod.branchCount = branchArray.Count;
        matGod.matState = MaterialGOD.Materials.Branch;
        // 바닥에 손에 있는 개수만큼 쌓임
        // 손에 있는 모든 것들이 제거
        for (int i = 0; i < branchArray.Count; i++)
        {
            Destroy(branchArray[i].gameObject);
        }
        branchArray.Clear();
    }
}
