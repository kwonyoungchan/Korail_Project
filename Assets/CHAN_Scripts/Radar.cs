using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    //�÷��̾� ���̴� ��� �߰� 
    // ������ ���Ǿ �̿��ؼ� �����ȿ� ���� ���� ���� ��ũ��Ʈ�� Ű�� �������� ����.
    [SerializeField]Collider[] blocks;
    [SerializeField] GameObject TileMap;
    [SerializeField] List<Collider> detectBlocks = new List<Collider>();
    [SerializeField] GameObject[] StartBlock;
    [SerializeField] GameObject[] EndBlock;
    void Start()
    {
        //���۽� ��� ���� ��ũ��Ʈ�� ����.
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
        // blocks �迭�� �ֺ� �ݶ��̴� ������ ���´�.
        blocks = Physics.OverlapSphere(transform.position, 2, 1 << 7);

        //������ �ݶ��̴��� ������ ����Ʈ�� �����Ѵ�.
        //�����ϱ����� List.
        for (int i = 0; i < blocks.Length; i++)
        {
            if (!detectBlocks.Contains(blocks[i]) && blocks[i].name != "Player")
            {
                detectBlocks.Add(blocks[i]);
                fuck(blocks[i].transform.gameObject, true);
            }
            //����Ʈ�� ����� �����Ͽ� ���ٸ� �� ����Ʈ�� ���� ����
            // ���¹迭�� ������Ʈ�� ���� ��ũ��Ʈ�� Ų��.
            //blocks[i].transform.gameObject.GetComponent<MaterialGOD>().enabled = true;
            //blocks[i].transform.gameObject.GetComponent<ToolGOD>().enabled = true;
        }

        //������ ����� ��ũ��Ʈ�� ����.
    }
    void fuck(GameObject GO ,bool B)
    {
        GO.GetComponent<ItemGOD>().enabled = B;
        GO.GetComponent<MaterialGOD>().enabled = B;
        GO.GetComponent<ToolGOD>().enabled = B;
    }

    //���������� ���������� ���� �׻� �ѵд�.
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


