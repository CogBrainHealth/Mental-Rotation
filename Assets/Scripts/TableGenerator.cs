using UnityEngine;
using System.Collections.Generic;

public class Table
{
    public string Name { get; set; } // 객체 이름 (e.g., "bear", "cat")
    public Sprite Image { get; set; } // Unity Sprite (동물 이미지 등)
    public bool Nullable { get; set; } // 빈 칸 여부

    public Table(string name, Sprite image, bool nullable = false)
    {
        Name = name;
        Image = image;
        Nullable = nullable;
    }
}

public class TableGenerator : MonoBehaviour
{
    Table[,] table = new Table[3, 3];

    List<Table> objects = new List<Table> { };
}
