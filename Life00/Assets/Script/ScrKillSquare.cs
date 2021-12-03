using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrKillSquare : MonoBehaviour
{
/*    private GameObject sq1;
    private GameObject sq2;
    private GameObject sq3;
    private GameObject sq4;
    private GameObject sq6;
    private GameObject sq7;
    private GameObject sq8;
    private GameObject sq9;*/
    private Vector2Int mypos;

    private GameObject[] neighbours = new GameObject[9];

    public string GetDebugInfo()
    {
        return ("ƒа всЄ нормальноЁ");
/*        return ("X:" + mypos.x + " Y:" + mypos.y + " SQ:" +
            ((sq1 == null) ? "0" : "1") + ((sq2 == null) ? "0" : "1") + ((sq3 == null) ? "0" : "1") +
            ((sq4 == null) ? "0" : "1") + ((sq6 == null) ? "0" : "1") +
            ((sq7 == null) ? "0" : "1") + ((sq8 == null) ? "0" : "1") + ((sq9 == null) ? "0" : "1"));*/
    }

    private void Start()
    {
     //   neighbours = new List<GameObject>();
    }

    public Vector2Int GetCellPosition()
    {
        return mypos;
    }

    // «аполн€ет ссылки на соседей и затем заполн€ет их со стороны соседей
    public void FillNext(GameObject a1, GameObject a2, GameObject a3, GameObject a4,
                         GameObject a6, GameObject a7, GameObject a8, GameObject a9)
    {
        neighbours[0] = a1;
        neighbours[1] = a2;
        neighbours[2] = a3;
        neighbours[3] = a4;
        neighbours[5] = a6;
        neighbours[6] = a7;
        neighbours[7] = a8;
        neighbours[8] = a9;

        for (int n=0;n<9;n++) if (neighbours[n]!=null) neighbours[n].GetComponent<ScrKillSquare>().SetupNext(9-n, gameObject);
/*
        sq1 = a1; sq2 = a2; sq3 = a3; sq4 = a4; sq6 = a6; sq7 = a7; sq8 = a8; sq9 = a9;
        if (sq1 != null) sq1.GetComponent<ScrKillSquare>().SetupNext(9, gameObject);
        if (sq2 != null) sq2.GetComponent<ScrKillSquare>().SetupNext(8, gameObject);
        if (sq3 != null) sq3.GetComponent<ScrKillSquare>().SetupNext(7, gameObject);
        if (sq4 != null) sq4.GetComponent<ScrKillSquare>().SetupNext(6, gameObject);
        if (sq6 != null) sq6.GetComponent<ScrKillSquare>().SetupNext(4, gameObject);
        if (sq7 != null) sq7.GetComponent<ScrKillSquare>().SetupNext(3, gameObject);
        if (sq8 != null) sq8.GetComponent<ScrKillSquare>().SetupNext(2, gameObject);
        if (sq9 != null) sq9.GetComponent<ScrKillSquare>().SetupNext(1, gameObject);*/
    }

    public void FillNext(List<GameObject> input)
    {
        foreach (GameObject inputElement in input)
        {
            //neighbours.Add(inputElement);
        }

    }

    public void SetupNext(int sd, GameObject next)
    {
        if ((sd < 1) || (sd > 9)) return;
        neighbours[sd - 1] = next;
 /*       switch (sd)
        {
            case (1): sq1 = next; break;
            case (2): sq2 = next; break;
            case (3): sq3 = next; break;
            case (4): sq4 = next; break;
            case (6): sq6 = next; break;
            case (7): sq7 = next; break;
            case (8): sq8 = next; break;
            case (9): sq9 = next; break;
        }*/
    }

    // –ождение новой клеточки в нужных координатах
    public void BornMe(Transform parent, Vector2Int newpos)
    {
        Vector3 np = new Vector3(newpos.x, newpos.y, 0);
        mypos = newpos;
        gameObject.transform.parent = parent;
        gameObject.transform.position = np;
    }

    public void KillMe()
    {
        for (int n=0;n<9;n++)
        {
            if (neighbours[n] != null) neighbours[n].GetComponent<ScrKillSquare>().SetupNext(9-n, null);

        }
/*        if (sq1 != null) sq1.GetComponent<ScrKillSquare>().SetupNext(9, null);
        if (sq2 != null) sq2.GetComponent<ScrKillSquare>().SetupNext(8, null);
        if (sq3 != null) sq3.GetComponent<ScrKillSquare>().SetupNext(7, null);
        if (sq4 != null) sq4.GetComponent<ScrKillSquare>().SetupNext(6, null);
        if (sq6 != null) sq6.GetComponent<ScrKillSquare>().SetupNext(4, null);
        if (sq7 != null) sq7.GetComponent<ScrKillSquare>().SetupNext(3, null);
        if (sq8 != null) sq8.GetComponent<ScrKillSquare>().SetupNext(2, null);
        if (sq9 != null) sq9.GetComponent<ScrKillSquare>().SetupNext(1, null);*/
        transform.parent.gameObject.GetComponent<GameManager>().delCell(mypos);
        Destroy(gameObject);
    }

    readonly Vector2Int[] sides = new Vector2Int[9] { new Vector2Int(-1,1), new Vector2Int(0, 1), new Vector2Int(1, 1),
    new Vector2Int(-1,0), new Vector2Int(0,0), new Vector2Int(1,0),
    new Vector2Int(-1,-1), new Vector2Int(0,-1), new Vector2Int(1,-1)};

    // ¬озвращает список пустых клеточек вокруг текущей клетки
    public List<Vector2Int> GetNextCellsCount()
    {
        List<Vector2Int> newadd = new List<Vector2Int>();
        for (int n = 0; n < 9; n++)
        {
            if ((neighbours[n] == null)&&(n!=4)) newadd.Add(mypos - sides[n]);
        }

        /*        if (sq1 == null) newadd.Add(new Vector2Int(mypos.x - 1, mypos.y + 1));
                if (sq2 == null) newadd.Add(new Vector2Int(mypos.x, mypos.y + 1));
                if (sq3 == null) newadd.Add(new Vector2Int(mypos.x + 1, mypos.y + 1));
                if (sq4 == null) newadd.Add(new Vector2Int(mypos.x - 1, mypos.y));
                if (sq6 == null) newadd.Add(new Vector2Int(mypos.x + 1, mypos.y));
                if (sq7 == null) newadd.Add(new Vector2Int(mypos.x - 1, mypos.y - 1));
                if (sq8 == null) newadd.Add(new Vector2Int(mypos.x, mypos.y - 1));
                if (sq9 == null) newadd.Add(new Vector2Int(mypos.x + 1, mypos.y - 1));*/
        return newadd;
    }


}
