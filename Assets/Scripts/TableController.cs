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
    public GameObject Sorth;
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
        Sorth = table.transform.Find("Circle_S").gameObject;
        North = table.transform.Find("Circle_N").gameObject;
    }

    public void RotateTable(int rotationAngle)
    {
        table.transform.Rotate(0, 0, 90 * (rotationAngle+1));
        table.transform.position = new Vector3(-5, 1.2f * rotationAngle, 0); //it was side
        table.transform.localScale = new Vector3(0.21f, 0.21f, 0);  //it was side at small
    }

    public bool CompareTable()
    {
        // 비교 메소드
        return false;
    }
}
