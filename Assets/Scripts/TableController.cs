using UnityEngine;

public class TableController : MonoBehaviour
{
    public GameObject table;

    public GameObject East;
    public GameObject West;
    public GameObject Sorth;
    public GameObject North;

    public TableGenerator tableGenerator;
    public GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        East = table.transform.Find("Circle_E").gameObject;
        West = table.transform.Find("Circle_W").gameObject;
        Sorth = table.transform.Find("Circle_S").gameObject;
        North = table.transform.Find("Circle_N").gameObject;

        if (table.name == "ExampleTable")
        {
            Debug.Log("보기 생성");
            tableGenerator.TableGenerate(this);
        }
    }

    public void RotateTable()
    {

    }

    public void CompareTable()
    {

    }
}
