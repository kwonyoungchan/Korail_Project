using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    //플레이어 레이더 기능 추가 
    // 오버랩 스피어를 이용해서 영역안에 들어온 곳만 블럭의 스크립트를 키고 나머지는 끈다.
    [SerializeField]Collider[] blocks;
    [SerializeField] GameObject TileMap;
    [SerializeField] List<Collider> detectBlocks = new List<Collider>();
    [SerializeField] GameObject[] StartBlock;
    [SerializeField] GameObject[] EndBlock;
    void Start()
    {
        //시작시 모든 블럭의 스크립트를 끈다.
        for (int i = 0; i < TileMap.transform.childCount; i++)
        {
            fuck(TileMap.transform.GetChild(i).gameObject,false);
        }
    }

    void Update()
    {
        OnSpecialBlock();

        for (int i = 0; i < detectBlocks.Count; i++)
        {
            fuck(detectBlocks[i].transform.gameObject, false);
        }
        detectBlocks.Clear();
        // blocks 배열에 주변 콜라이더 정보가 들어온다.
        blocks = Physics.OverlapSphere(transform.position, 2, 1 << 7);

        //검출한 콜라이더의 정보를 리스트에 저장한다.
        //저장하기전에 List.
        for (int i = 0; i < blocks.Length; i++)
        {
            if (!detectBlocks.Contains(blocks[i]) && blocks[i].name != "Player")
            {
                detectBlocks.Add(blocks[i]);
                fuck(blocks[i].transform.gameObject, true);
            }
            //리스트의 목록이 검출블록에 없다면 그 리스트의 인자 삭제
            // 들어온배열의 오브젝트는 관련 스크립트를 킨다.
            //blocks[i].transform.gameObject.GetComponent<MaterialGOD>().enabled = true;
            //blocks[i].transform.gameObject.GetComponent<ToolGOD>().enabled = true;
        }

        //영역에 벗어나면 스크립트를 끈다.
    }
    void fuck(GameObject GO ,bool B)
    {
        GO.GetComponent<ItemGOD>().enabled = B;
        GO.GetComponent<MaterialGOD>().enabled = B;
        GO.GetComponent<ToolGOD>().enabled = B;
    }

    //시작지점과 도착지점의 블럭은 항상 켜둔다.
    void OnSpecialBlock()
    {
        DefineBlocks Blocks = DefineBlocks.instance;
        for (int i = 0; i < Blocks.StartBlocks.Length; i++)
        {
            fuck(Blocks.StartBlocks[i], true);
        }
        for (int i = 0; i < Blocks.EndBlocks.Length; i++)
        {
            fuck(Blocks.EndBlocks[i], true);
        }
    }
}


