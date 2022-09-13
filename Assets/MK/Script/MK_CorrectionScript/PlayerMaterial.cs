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

    // Start is called before the first frame update
    void Start()
    {
        
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
                if(rayInfo.transform.gameObject.GetComponent<MaterialGOD>() == null)
                {
                    return;a
                }
                // 손에 무언갈 들고 있다면
                if (branchArray.Count > 0 || steelArray.Count > 0) 
                {
                    if (branchArray.Count > 0 && rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState == MaterialGOD.Materials.Branch)
                    {
                        
                        // Array에 추가하기
                        GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                        branch.transform.parent = itemPos;
                        
                        branchArray.Add(branch);
                        for(int i = 0; i < branchArray.Count; i++)
                        {
                            branchArray[i].transform.position = itemPos.position + new Vector3(0, i * 0.5f, 0);
                            branchArray[i].transform.eulerAngles = new Vector3(0, 0, 0);
                        }
                        rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState = MaterialGOD.Materials.None;

                    }
                }
                // 손에 없고
                else
                {
                    // 점프키를 눌렀을 때,
                    if (Input.GetButtonDown("Jump") && rayInfo.transform.gameObject.GetComponent<MaterialGOD>())
                    {
                        // 손에 있는 무언가를 든다
                        // 바닥 상태가 Branch라면
                        if (rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState == MaterialGOD.Materials.Branch)
                        {
                            // Array에 추가하기
                            GameObject branch = Instantiate(Resources.Load<GameObject>("MK_Prefab/Branch"));
                            branchArray.Add(branch);
                            branch.transform.parent = itemPos;
                            branch.transform.position = itemPos.position;
                            branch.transform.eulerAngles = new Vector3(90, 0, 90);
                            
                            // 플레이어 손상태 변환
                            GetComponent<PlayerItemDown>().holdState = PlayerItemDown.Hold.Branch;
                            // 바닥상태 변환
                            rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState = MaterialGOD.Materials.None;
                        }
                        // Steel이라면
                        if (rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState == MaterialGOD.Materials.Steel)
                        {
                            // Array에 추가하기
                            GameObject steel = Instantiate(Resources.Load<GameObject>("MK_Prefab/Steel"));
                            steelArray.Add(steel);
                            steel.transform.position = itemPos.position;
                            // 플레이어 손상태 변환
                            GetComponent<PlayerItemDown>().holdState = PlayerItemDown.Hold.Steel;
                            // 바닥상태 변환
                            rayInfo.transform.gameObject.GetComponent<MaterialGOD>().matState = MaterialGOD.Materials.None;
                            
                        }
                    }
                }
            }
        }
    }
}
