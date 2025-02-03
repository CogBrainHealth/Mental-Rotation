using UnityEngine;

public class TableController : MonoBehaviour
{
    //스크립트 의존성
    private TableGenerator tg;
    private GameManager gm;

    //현재 오브젝트와 자식 오브젝트
    private GameObject table;

    public GameObject East;
    public GameObject West;
    public GameObject South;
    public GameObject North;

    //현재 테이블 멤버 변수
    public bool nullAble = false;
    public TableController[] sameTable;

    void Awake()
    {
        tg = TableGenerator.Instance;
        gm = GameManager.Instance;

        table = gameObject;

        East = table.transform.Find("Circle_E").gameObject;
        West = table.transform.Find("Circle_W").gameObject;
        South = table.transform.Find("Circle_S").gameObject;
        North = table.transform.Find("Circle_N").gameObject;
    }

    public void RotateTable(int rotationAngle)
    {
        table.transform.Rotate(0, 0, 90 * (rotationAngle+1));
        table.transform.position = new Vector3(-5, 1.2f * rotationAngle, 0); //it was side
        table.transform.localScale = new Vector3(0.21f, 0.21f, 0);  //it was side at small
    }

    public bool CompareTable(TableController target)
    {
        //Setting My Sprite
        SpriteRenderer[] myRenderer = new SpriteRenderer[4];
        myRenderer[0] = North.GetComponent<SpriteRenderer>();
        myRenderer[1] = East.GetComponent<SpriteRenderer>();
        myRenderer[2] = South.GetComponent<SpriteRenderer>();
        myRenderer[3] = West.GetComponent<SpriteRenderer>();

        //Setting Target Sprite
        SpriteRenderer[] targetRenderer = new SpriteRenderer[4];
        targetRenderer[0] = target.North.GetComponent<SpriteRenderer>();
        targetRenderer[1] = target.East.GetComponent<SpriteRenderer>();
        targetRenderer[2] = target.South.GetComponent<SpriteRenderer>();
        targetRenderer[3] = target.West.GetComponent<SpriteRenderer>();

        // 비교
        for (int i = 0; i < 4; i++) //0, 90, 180, 270 각 회전까지 4번 비교
        {
            bool isSameTable = true;

            for (int j = 0; j < myRenderer.Length; j++) // N, E, S, W 각 셀 비교
            {
                int index = (i + j) % myRenderer.Length; //90도 회전 당 1칸씩 뒤 셀과 비교
                if (myRenderer[j].sprite != targetRenderer[index].sprite)
                {
                    isSameTable = false;
                    break; //다른 셀 발견 -> 다음 테이블
                }
                else isSameTable = true; //모든 셀이 같은 경우
            }

            if (isSameTable)
                return true;
        }
        return false; //모두 다르면 false
    }
}
