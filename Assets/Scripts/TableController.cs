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

        //Generator 테이블 생성 테스트
        //if (table.name == "ExampleTable")
        //{
        //    Debug.Log("보기 생성");
        //    tg.TableGenerate(this);
        //}
    }

    public void RotateTable()
    {

    }

    public void CompareTable()
    {

    }
}
