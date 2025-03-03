using System;
using System.Collections.Generic;
using UnityEngine;

public class TableGenerator : MonoBehaviour
{
    public Sprite[] animalSprites; // 동물 Sprites들을 요소로 갖는 배열
    public Sprite[] colorSprites; // 파일럿 용 색상 Sprites 배열
    public List<Sprite> selectedSprites = new List<Sprite>(); // 색상 배치를 위한 임시 배열
    Action[] assignActions; // 랜덤으로 동서남북 채우는 용
                            // using System; 추가했더니 Random에 에러 생김 -> UnityEngine.Random으로 수정

    private bool nullAssignment; // Null이 포함 됐는지
    private int[] spriteUsage;// selectedSprite 중 색상 배치 여부
    private int rotationAngle; // 90, 180, 270
    private int twoTimesIndex; // 2번 사용되는 색상

    //Game
    private List<Sprite> spritePool;

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

        // 파일럿 용 Unity 내 기본 제공 도형 스프라이트 로드 
        colorSprites = Resources.LoadAll<Sprite>("Sprites/Color");

        spritePool = new List<Sprite>();
    }

    //sprite pool In This Stage
    public void SettingSpritePool(int colorNum)
    {
        spritePool.Clear();
        
        if (colorNum == 3 )
            spritePool.Add(null);

        List<Sprite> tempSprites = new List<Sprite>(animalSprites);
        for (int i = 0; i < colorNum; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, tempSprites.Count);
            spritePool.Add(tempSprites[randomIndex]);
            tempSprites.RemoveAt(randomIndex);
        }
    }

    public void TableGenerate(TableController table) // 테이블 세팅
    {
        List<Sprite> pool = new List<Sprite>(spritePool);

        AssignRandomSprite(table.North, pool);
        AssignRandomSprite(table.South, pool);
        AssignRandomSprite(table.East, pool);
        AssignRandomSprite(table.West, pool);
    }

    private void AssignRandomSprite(GameObject circle, List<Sprite> spritePool)
    {
        if (spritePool.Count == 0) return;

        // 랜덤으로 스프라이트 선택
        int randomIndex = UnityEngine.Random.Range(0, spritePool.Count);
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

    //----------------------Pilot----------------------
    public void TableGeneratePilot(TableController table, int stageType, bool isExample )
    {
      
        nullAssignment = false;

        // Pilot 문제 데이터 구성 (8문항)
        // TableEx인 경우일 때만 selectedSprites를 새로 생성

        switch (stageType)
        {

            case 1: // 같은 색상 2개와 다른 색상 1개 (90도)
            case 2: // 같은 색상 2개와 다른 색상 1개 (180도)
            case 3: // 같은 색상 2개와 다른 색상 1개 (270도)
                spriteUsage = new int[2] { 2, 1 }; // 사용 여부 초기화사용 여부 초기화
                                                   // 선지에서도 같은 색상 두 번 사용

                if (isExample)
                {
                    selectedSprites.Clear(); // 기존 데이터 초기화

                    selectedSprites.Add(colorSprites[UnityEngine.Random.Range(0, colorSprites.Length)]);
                    selectedSprites.Add(colorSprites[UnityEngine.Random.Range(0, colorSprites.Length)]);
                    while (selectedSprites[0] == selectedSprites[1]) //서로 다른 sprite로 세팅
                    {
                        selectedSprites[1] = colorSprites[UnityEngine.Random.Range(0, colorSprites.Length)];
                    }
                }
                break;

            case 4: // 모두 다른 색상 (90도)
            case 5: // 모두 다른 색상 (180도)
            case 6: // 모두 다른 색상 (270도)
                spriteUsage = new int[3] { 1, 1, 1 }; // 사용 여부 초기화

                if (isExample)
                {
                    selectedSprites.Clear(); // 기존 데이터 초기화
                    while (selectedSprites.Count < 3) //서로 다른 sprite로 세팅
                    {
                        Sprite randomColor = colorSprites[UnityEngine.Random.Range(0, colorSprites.Length)];
                        if (!selectedSprites.Contains(randomColor))
                        {
                            selectedSprites.Add(randomColor);
                        }
                    }
                }
                break;

            case 7: // 3가지 색상으로 4칸 채움 (180)
            case 8: // 3가지 색상으로 4칸 채움 (270)
                spriteUsage = new int[3] { 2, 1, 1 }; // 사용 여부 초기화
                                                      // 선지에서도 같은 색상 두 번 사용 

                if (isExample)
                {
                    selectedSprites.Clear();
                    while (selectedSprites.Count < 3)
                    {
                        Sprite randomColor = colorSprites[UnityEngine.Random.Range(0, colorSprites.Length)];
                        if (!selectedSprites.Contains(randomColor))
                        {
                            selectedSprites.Add(randomColor);
                        }
                    }
                }
                break;

            case 9: // 4가지 색상으로 4칸 채움 (90)
            case 10: // 4가지 색상으로 4칸 채움 (180)
                     // -> 이 두 케이스를 추가하면 자꾸 유니티가 다운 된다 ...
                     // -> 이것도 동서남북 랜덤 배치로 해결
                spriteUsage = new int[4] { 1, 1, 1, 1 };

                if (isExample)
                {
                    selectedSprites.Clear();
                    while (selectedSprites.Count < 4)
                    {
                        Sprite randomColor = colorSprites[UnityEngine.Random.Range(0, colorSprites.Length)];
                        if (!selectedSprites.Contains(randomColor))
                        {
                            selectedSprites.Add(randomColor);
                        }
                    }
                }
                break;

            default:
                Debug.LogError($"알 수 없는 문제 유형: {stageType}");
                return;
        }

        // 보기와 선지가 두 번 사용하는 색상이 같도록 했더니 에러가 남
        // -> 같은 순서로 배치되니까 비교메소드에서 true를 반환할 수밖에 없음
        // 동서남북 랜덤으로 배치 시작
        assignActions = new Action[]
        {
            () => AssignPilotSprite(table.North, stageType),
            () => AssignPilotSprite(table.South, stageType),
            () => AssignPilotSprite(table.East, stageType),
            () => AssignPilotSprite(table.West, stageType)
        };

        // Fisher-Yates Shuffle 알고리즘
        System.Random rnd = new System.Random();
        for (int i = assignActions.Length - 1; i > 0; i--)
        {
            int j = rnd.Next(0, i + 1);
            (assignActions[i], assignActions[j]) = (assignActions[j], assignActions[i]); // Swap
        }

        // 섞인 순서로 실행
        foreach (var action in assignActions)
        {
            action();
        }
    }

    public void AssignPilotSprite(GameObject circle, int stageType)
    {
        SpriteRenderer renderer = circle.GetComponent<SpriteRenderer>();

        switch (stageType)
        {
            case 1:
            case 2:
            case 3: // 2-1-null
                if (!nullAssignment && UnityEngine.Random.Range(0, 3) == 0) // null
                {
                    nullAssignment = true;
                    renderer.sprite = null;
                    return;
                }
                AssignSpriteOrNull(renderer);
                break;

            case 4:
            case 5:
            case 6: // 1-1-1-null
                if (!nullAssignment && UnityEngine.Random.Range(0, 4) == 0) // null
                {
                    nullAssignment = true;
                    renderer.sprite = null;
                    return;
                }
                AssignSpriteOrNull(renderer);
                break;

            case 7:
            case 8: // 2-1-1
                for (int i = 0; i < 3; i++)
                {
                    if (spriteUsage[i] > 0)
                    {
                        renderer.sprite = selectedSprites[i];
                        spriteUsage[i]--;
                        return;
                    }
                }
                break;

            case 9:
            case 10: // 1-1-1-1
                for (int i = 0; i < 4; i++)
                {
                    if (spriteUsage[i] > 0)
                    {
                        renderer.sprite = selectedSprites[i];
                        spriteUsage[i]--;
                        return;
                    }
                }
                break;

            default:
                Debug.LogError($"예상치 못한 stageType: {stageType}");
                break;
        }

    }

    // 스프라이트 배치 로직을 별도 함수로 정리
    public void AssignSpriteOrNull(SpriteRenderer renderer)
    {
        for (int i = 0; i < selectedSprites.Count; i++)
        {
            if (spriteUsage[i] > 0) // 아직 배치되지 않은 스프라이트라면
            {
                renderer.sprite = selectedSprites[i];
                spriteUsage[i]--; // 사용 체크
                return;
            }
        }
        nullAssignment = true;
        renderer.sprite = null;
        return;
    }

    public int RotateAngle(int stageType)
    {
        // Pilot stage answer setting
        switch (stageType)
        {
            case 1: // 같은 색상 2개와 다른 색상 1개 (90도)
                return 0;
            case 2: // 같은 색상 2개와 다른 색상 1개 (180도)
                return 1;
            case 3: // 같은 색상 2개와 다른 색상 1개 (270도)
                return 2;
            case 4: // 모두 다른 색상 3개 (90도)
                return 0;
            case 5: // 모두 다른 색상 3개 (180도)
                return 1;
            case 6: // 모두 다른 색상 3개 (270도)
                return 2;
            case 7: // 같은 색상 2개와 다른 색상 2개 (180도)
                return 1;
            case 8: // 같은 색상 2개와 다른 색상 2개 (270도)
                return 2;
            case 9: // 모두 다른 색상 4개 (90도)
                return 0;
            case 10: // 모두 다른 색상 4개 (180도)
                return 1;


            default:
                Debug.LogError($"알 수 없는 문제 유형: {stageType}");
                return -1;
        }
    }
    //////////////////////////// For Pilot End ////////////////////////////
}