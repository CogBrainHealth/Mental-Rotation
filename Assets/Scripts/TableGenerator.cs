using UnityEngine;
using System.Collections.Generic;

public class TableGenerator : MonoBehaviour
{
    public Sprite[] animalSprites; // 동물 Sprites들을 요소로 갖는 배열

    public Sprite[] shapeSprites; // 파일럿 용 도형 Sprites 배열
    public bool nullAssignment; // Null이 포함 됐는지
    public int[] spriteUsage;// selectedSprite 중 도형 배치 여부
    public int rotationAngle; // 90, 180, 270

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
        shapeSprites = Resources.LoadAll<Sprite>("Sprites/Color");
    }

    //--------------------NotPilot--------------------
    public void TableGenerate(TableController table) // 테이블 세팅
    {
        // 동물 스프라이트 랜덤 선택을 위한 배열 복사
        List<Sprite> tempSprites = new List<Sprite>(animalSprites); // 동물 배치를 위한 임시 배열

        // Not Pilot
        // 테이블의 방향별 Circle에 랜덤 스프라이트 배치
        Debug.Log("동물랜덤배치");
        AssignRandomSprite(table.North, tempSprites);
        AssignRandomSprite(table.South, tempSprites);
        AssignRandomSprite(table.East, tempSprites);
        AssignRandomSprite(table.West, tempSprites);
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

    //----------------------Pilot----------------------
    public void TableGeneratePilot(TableController table, int stageType)
    {
        // 도형 스프라이트 랜덤 선택을 위한 배열 복사
        List<Sprite> selectedSprites = new List<Sprite>(); // 도형 배치를 위한 임시 배열

        nullAssignment = false;

        // Pilot 문제 데이터 구성 (8문항)
        switch (stageType)
        {
            case 1: // 같은 도형 3개와 Null (90도)
            case 2: // 같은 도형 3개와 Null (270도)
                spriteUsage = new int[1] { 0 }; // 사용 여부 체크
                selectedSprites.Add(shapeSprites[Random.Range(0, shapeSprites.Length)]);
                rotationAngle = stageType == 1 ? 90 : 270;
                break;

            case 3: // 같은 도형 2개와 다른 도형 1개 (90도)
            case 4: // 같은 도형 2개와 다른 도형 1개 (180도)
            case 5: // 같은 도형 2개와 다른 도형 1개 (270도)
                spriteUsage = new int[2] { 0, 0 }; // 사용 여부 체크
                selectedSprites.Add(shapeSprites[Random.Range(0, shapeSprites.Length)]);
                selectedSprites.Add(shapeSprites[Random.Range(0, shapeSprites.Length)]);
                while (selectedSprites[0] == selectedSprites[1]) //서로 다른 sprite로 세팅
                {
                    selectedSprites[1] = shapeSprites[Random.Range(0, shapeSprites.Length)];
                }
                rotationAngle = stageType == 3 ? 90 : (stageType == 4 ? 180 : 270);
                break;

            case 6: // 모두 다른 도형 (90도)
            case 7: // 모두 다른 도형 (180도)
            case 8: // 모두 다른 도형 (270도)
                while (selectedSprites.Count < 3) //서로 다른 sprite로 세팅
                {
                    Sprite randomShape = shapeSprites[Random.Range(0, shapeSprites.Length)];
                    if (!selectedSprites.Contains(randomShape))
                    {
                        selectedSprites.Add(randomShape);
                    }
                }
                rotationAngle = stageType == 6 ? 90 : (stageType == 7 ? 180 : 270);
                break;

            default:
                Debug.LogError($"알 수 없는 문제 유형: {stageType}");
                return;
        }
        //Debug.Log($"{stageType}");
        AssignPilotSprite(table.North, selectedSprites, stageType);
        AssignPilotSprite(table.South, selectedSprites, stageType);
        AssignPilotSprite(table.East, selectedSprites, stageType);
        AssignPilotSprite(table.West, selectedSprites, stageType);
    }

    public void AssignPilotSprite(GameObject circle, List<Sprite> spritePool, int stageType)
    {
        SpriteRenderer renderer = circle.GetComponent<SpriteRenderer>();

        // 각 문항 조건에 따른 도형 배치
        if (stageType < 3) // 3-null
        {
            if (spriteUsage[0] < 3) // 같은 도형 3번 배치 덜 끝났을 때 -> 둘 중 랜덤
            {
                //null
                if (!nullAssignment && Random.Range(0, 2) == 0) // 50% 확률로 null 배치 
                {
                    renderer.sprite = null;
                    nullAssignment = true;
                    return;
                }
                else //sprite 0
                {
                    renderer.sprite = spritePool[0];
                    spriteUsage[0]++;
                    return;
                }
            }

            // 같은 도형 3번 배치 끝난 경우 -> null
            renderer.sprite = null;
            nullAssignment = true;
            return;
        }
        else if (stageType < 6) // 2-1-null
        {
            //int selectedIndex;

            if (spriteUsage[0] < 2)
            {
                if (spriteUsage[1] < 2) // 같은 도형 2번 배치 덜 끝났을 떄
                {
                    if (!nullAssignment && Random.Range(0, 3) == 0) //null
                    {
                        nullAssignment = true;
                        renderer.sprite = null;
                        return;
                    }

                    int selectedIndex = Random.Range(0, 2); // [0], [1] 중 랜덤 배치
                    renderer.sprite = spritePool[selectedIndex];
                    spriteUsage[selectedIndex]++;
                    return;
                }
                else // spritePool[1] 이미 2번 배치한 경우
                {
                    if (spriteUsage[0] < 1) // [0]을 한 번도 배치하지 않은 경우
                    {
                        if (!nullAssignment && Random.Range(0, 2) == 0) //null
                        {
                            nullAssignment = true;
                            renderer.sprite = null;
                            return;
                        }

                        renderer.sprite = spritePool[0]; // [0] 배치 
                        spriteUsage[0]++;
                        return;
                    }

                    // 같은 도형 2, 다른 도형 1개 배치 -> null 배치
                    nullAssignment = true;
                    renderer.sprite = null;
                    return;
                }
            }
            else // spritePool[0] 이미 2번 배치한 경우
            {
                if (spriteUsage[1] < 1) // [1]을 한 번도 배치하지 않은 경우
                {
                    if (!nullAssignment && Random.Range(0, 2) == 0) //null
                    {
                        nullAssignment = true;
                        renderer.sprite = null;
                        return;
                    }

                    renderer.sprite = spritePool[1]; // [1] 배치
                    spriteUsage[1]++;
                    return;
                }

                // 같은 도형 2, 다른 도형 1개 배치 -> null
                nullAssignment = true;
                renderer.sprite = null;
                return;
            }
        }
        else 
        {
            // null과 서로 다른 도형 3개 배치
            if(spritePool.Count != 0)
            {
                if (!nullAssignment && Random.Range(0, 4) == 0) //null
                {
                    nullAssignment = true;
                    renderer.sprite = null;
                    return;
                }
                renderer.sprite = spritePool[0]; // 서로 다른 도형 배치
                spritePool.RemoveAt(0); // 선택한 도형 제거
                return;
            }
            nullAssignment = true;
            renderer.sprite = null;
            return;
        }
    }

    //for Pilot
    public int RotateAngle(int stageType)
    {
        switch (stageType)
        {
            case 1: // 같은 도형 3개와 Null (90도)
                return 0;
            case 2: // 같은 도형 3개와 Null (270도)
                return 2;
            case 3: // 같은 도형 2개와 다른 도형 1개 (90도)
                return 0;
            case 4: // 같은 도형 2개와 다른 도형 1개 (180도)
                return 1;
            case 5: // 같은 도형 2개와 다른 도형 1개 (270도)
                return 2;
            case 6: // 모두 다른 도형 (90도)
                return 0;
            case 7: // 모두 다른 도형 (180도)
                return 1;
            case 8: // 모두 다른 도형 (270도)
                return 2;

            default:
                Debug.LogError($"알 수 없는 문제 유형: {stageType}");
                return -1;
        }
    }

    /*---------------------------각 셀 직접 세팅(Direct Setting for Each Cell) 
    public void PilotTableGenerate(TableController table)
    {
        List<Sprite> pilotTempSprites = new List<Sprite>(sha); // 도형 배치를 위한 임시 배열

        Debug.Log("도형 랜덤 배치");
        AssignSprite(table.North, pilotTempSprites);
        AssignSprite(table.South, pilotTempSprites);
        AssignSprite(table.East, pilotTempSprites);
        AssignSprite(table.West, pilotTempSprites);
    }

    private void AssignSprite(GameObject circle, int spriteIndex)
    {
        //스프라이트 선택
        if (spriteIndex >= animalSprites.Length)
        {
            Debug.Log("추가되지 않은 동물입니다.");
            return;
        }
        else if (spriteIndex == -1)
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
    */

}