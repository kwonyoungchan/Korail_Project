using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 안 보면 물건을 훔쳐서 달아남
public class Theif : MonoBehaviour
{
    public enum TState
    {
        Idle,
        Move,
        Hold,
        Rot,
        Run,
        Die
    }
    public TState state = TState.Idle;
    // 속도
    public float speed = 2;
    // 랜덤 위치 거리
    public float range = 3;
    // 도둑 체력
    public int hp = 6;
    // 반지름
    public float r = 5;

    // 레이어
    int layer = 0;
    // material 배열
    Collider[] material;
    Collider[] mat;

    // 랜덤 위치
    Vector3 rndPos;
    // 시간
    float currentTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TheifState();
    }

    void TheifState()
    {
        switch (state)
        {
            // 움직이지 않는 상태
            case TState.Idle:
                break;
            // 근처에 재료가 생성된다면 그곳으로 움직임
            case TState.Move:
                Move();
                break;
            // 물건 들기
            case TState.Hold:
                break;
            // 랜덤 위치로 움직이기
            case TState.Rot:
                break;
            // 플레이어가 보면 물건 놓고 도망가기
            case TState.Run:
                break;
        }
    }

    // 근처에 재료가 생성된다면 그곳으로 움직임
    // 
    void Move()
    {
        layer = 1 << 10;
        // 도둑 주변에 물체가 있는지 여부 확인
        material = Physics.OverlapSphere(transform.position, r, layer);
        // 물체가 있다면
        if (material.Length > 0)
        {
            for(int i = 0; i < material.Length; i++)
            {
                if(material[i].transform.parent.name.Equals("IngredientPos"))
                {
                    if (mat.Length > 0)
                    {
                        mat[0] = material[i];
                        Vector3 pos = new Vector3(mat[0].gameObject.transform.position.x, 1.5f, mat[0].gameObject.transform.position.z);
                        // 방향이 정해지고 
                        Vector3 dir = pos - transform.position;
                        dir.Normalize();
                        float dis = Vector3.Distance(pos, transform.position);
                        // 거리 확인
                        if (dis < 0.3f)
                        {
                            transform.position = pos;
                        }
                        else
                        {
                            // 움직인다
                            transform.position += dir * speed * Time.deltaTime;
                        }
                    }
                }
                else
                {
                    state = TState.Idle;
                }
            }

        }
    }
    // 랜덤 위치로 움직이기
    void Rot()
    {
        // 랜덤 위치 정하기
        rndPos = UnityEngine.Random.insideUnitSphere * range;
        rndPos.y = 1.5f;
        transform.LookAt(rndPos);

        Vector3 rndDir = rndPos - transform.position;
        transform.position += rndDir * speed * Time.deltaTime;
    }
    // 플레이어가 보면 도망가기

    // 데미지 입으면
    public void Damage()
    {
        currentTime += Time.deltaTime;
        if (currentTime > 1)
        {
            hp--;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
            currentTime = 0;
        }
    }
}
