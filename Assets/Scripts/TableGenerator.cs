using UnityEngine;
using System.Collections.Generic;

public class TableGenerator : MonoBehaviour
{
    public Sprite[] animalSprites; // 동물 Sprites들을 요소로 갖는 배열

    public static TableGenerator Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

    public void Start()
    {
        // Asset/Resources/Sprites/Animals에 있는 png 파일 배열에 할당하기
        animalSprites = Resources.LoadAll<Sprite>("Sprites/Animals");
    }

    public bool DecideNullable()
    {
        // ...
        // 3칸짜리인 경우 조건문
        // ...
            // return true;

        // 4칸짜리
        return false;
    }

    public void TableGenerate(TableController table) // 테이블 세팅
    {
        // 동물 스프라이트 랜덤 선택을 위한 배열 복사
        List<Sprite> availableSprites = new List<Sprite>(animalSprites); // 동물 배치를 위한 임시 배열

        // Not Pilot
        // 테이블의 방향별 Circle에 랜덤 스프라이트 배치
        Debug.Log("동물랜덤배치");
        AssignRandomSprite(table.North, availableSprites);
        AssignRandomSprite(table.Sorth, availableSprites);
        AssignRandomSprite(table.East, availableSprites);
        AssignRandomSprite(table.West, availableSprites);
    }

    private void AssignRandomSprite(GameObject circle, List<Sprite> spritePool)
    {
        if (spritePool.Count == 0) return;

        // 랜덤으로 스프라이트 선택
        int randomIndex = Random.Range(0, spritePool.Count);
        Sprite selectedSprite = spritePool[randomIndex];

        // 선택한 스프라이트를 해당 Circle의 SpriteRenderer에 적용
        var renderer = circle.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            renderer = circle.AddComponent<SpriteRenderer>();
        }

        renderer.sprite = selectedSprite;

        // 동물이 중앙을 바라보도록 sprite를 회전
        if (circle.name == "Circle_E")
            circle.transform.rotation = Quaternion.Euler(0, 0, -90); // East: 시계방향 90도

        else if (circle.name == "Circle_W")
            circle.transform.rotation = Quaternion.Euler(0, 0, 90); // West: 반시계방향 90도

        else if (circle.name == "Circle_S")
            circle.transform.rotation = Quaternion.Euler(0, 0, 180); // South: 시계방향 180도

        // 사용된 스프라이트는 제거 (중복 방지)
        spritePool.RemoveAt(randomIndex);
    }

    public void PilotTableGenerate(TableController table, int[] data)
    {
        if (data.Length != 4) return; //이상한 배열이 들어왔서요

        AssignSprite(table.North, data[0]);
        AssignSprite(table.Sorth, data[1]);
        AssignSprite(table.East, data[2]);
        AssignSprite(table.West, data[3]);
    }

    private void AssignSprite(GameObject circle, int spriteIndex)
    {
        //스프라이트 선택
        if (spriteIndex >= animalSprites.Length)
        {
            Debug.Log("추가되지 않은 동물입니다.");
            return;
        }else if (spriteIndex == -1)
        {
            Debug.Log("null: 빈칸입니다");
            return;
        }

        //서클에 스프라이트 적용
        Sprite selectedSprite = animalSprites[spriteIndex];

        var renderer = circle.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            renderer = circle.AddComponent<SpriteRenderer>();
        }

        renderer.sprite = selectedSprite;

        //동물이 중앙을 바라보도록 sprite를 회전
        if (circle.name == "Circle_E")
            circle.transform.rotation = Quaternion.Euler(0, 0, -90); // East: 시계방향 90도

        else if (circle.name == "Circle_W")
            circle.transform.rotation = Quaternion.Euler(0, 0, 90); // West: 반시계방향 90도

        else if (circle.name == "Circle_S")
            circle.transform.rotation = Quaternion.Euler(0, 0, 180); // South: 시계방향 180도
    }
}