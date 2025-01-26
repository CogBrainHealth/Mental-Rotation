using UnityEngine;
using System.Collections.Generic;

// 
public class TableGenerator : MonoBehaviour
{
    public Sprite[] animalSprites; // 동물 Sprites들을 요소로 갖는 배열

    public void Start()
    {
        // Asset/Resources/Sprites/Animals에 있는 png 파일 배열에 할당하기
        animalSprites = Resources.LoadAll<Sprite>("Sprites/Animals");
    }

    public bool DecideNullable()
    {
        // 3칸짜리인 경우 조건문
        // ...
            // return true;

        // 4칸짜리
        return false;
    }

    public void TableGenerate(TableController table)
    {
        // 동물 스프라이트 랜덤 선택을 위한 배열 복사
        List<Sprite> availableSprites = new List<Sprite>(animalSprites); // 동물 배치를 위한 임시 배열

        // Pilot
        // ...

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

        // 동물이 중앙을 바라보도록 sprite를 회전
        if (circle.name == "Circle_E")
            circle.transform.rotation = Quaternion.Euler(0, 0, -90); // East: 시계방향 90도

        else if (circle.name == "Circle_W")
            circle.transform.rotation = Quaternion.Euler(0, 0, 90); // West: 반시계방향 90도

        else if (circle.name == "Circle_S")
            circle.transform.rotation = Quaternion.Euler(0, 0, 180); // South: 시계방향 180도

        renderer.sprite = selectedSprite;

        // 사용된 스프라이트는 제거 (중복 방지)
        spritePool.RemoveAt(randomIndex);
    }
}