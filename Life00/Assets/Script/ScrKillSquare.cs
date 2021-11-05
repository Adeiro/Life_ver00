using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrKillSquare : MonoBehaviour
{
    private GameObject[] sq;
    private Vector2Int mypos;

    private void Start()
    {
        for (int i = 0; i < 9; i++) sq[i] = null; // 5 элемент - мо€ клетка - будет всегда nil чтобы не сношать мозг преобразованием координат
    }
    private void OnMouseDown()
    {
        KillMe();
    }

    // –ождение новой клеточки в нужных координатах
    public void BornMe(Transform parent, Vector2Int newpos)
    {
        Vector3 np = new Vector3(newpos.x, newpos.y, 0);
        mypos = newpos;
        // —канируем объекты вокруг клетки
        Vector2 dirv = new Vector2((float)0.25, 0);
        sq[0] = Physics2D.Raycast(new Vector2(newpos.x - 1, newpos.y + 1), dirv, (float)0.25, 0xFFFF, (float)-0.5, (float)0.5).collider.gameObject;
        sq[1] = Physics2D.Raycast(new Vector2(newpos.x, newpos.y + 1), dirv, (float)0.25, 0xFFFF, (float)-0.5, (float)0.5).collider.gameObject;
        sq[2] = Physics2D.Raycast(new Vector2(newpos.x + 1, newpos.y + 1), dirv, (float)0.25, 0xFFFF, (float)-0.5, (float)0.5).collider.gameObject;
        sq[3] = Physics2D.Raycast(new Vector2(newpos.x - 1, newpos.y), dirv, (float)0.25, 0xFFFF, (float)-0.5, (float)0.5).collider.gameObject;
        sq[4] = null;
        sq[5] = Physics2D.Raycast(new Vector2(newpos.x + 1, newpos.y), dirv, (float)0.25, 0xFFFF, (float)-0.5, (float)0.5).collider.gameObject;
        sq[6] = Physics2D.Raycast(new Vector2(newpos.x - 1, newpos.y - 1), dirv, (float)0.25, 0xFFFF, (float)-0.5, (float)0.5).collider.gameObject;
        sq[7] = Physics2D.Raycast(new Vector2(newpos.x, newpos.y - 1), dirv, (float)0.25, 0xFFFF, (float)-0.5, (float)0.5).collider.gameObject;
        sq[8] = Physics2D.Raycast(new Vector2(newpos.x + 1, newpos.y - 1), dirv, (float)0.25, 0xFFFF, (float)-0.5, (float)0.5).collider.gameObject;
        gameObject.transform.parent = parent;
        gameObject.transform.position = np;
    }

    public void KillMe()
    {
        // Ѕудем наде€тьс€ что при удалении автоматом удал€тс€ и ссылки на эту клетку у всех соседей
        Destroy(gameObject);
    }

    // ¬озвращает список пустых клеточек вокруг текущей клетки
    public List<Vector2Int> GetNextCellsCount()
    {
        List<Vector2Int> newadd = new List<Vector2Int>();
        for (int n = 0; n < 9; n++) if (n != 4)
            {
                if (sq[n] == null) newadd.Add(new Vector2Int(mypos.x + (((n == 0) || (n == 3) || (n == 6)) ? -1 : (((n == 2) || (n == 5) || (n == 8)) ? 1 : 0)), mypos.y + ((n < 3) ? 1 : ((n > 5) ? 1 : 0))));
            }
        return newadd;
    }


}
