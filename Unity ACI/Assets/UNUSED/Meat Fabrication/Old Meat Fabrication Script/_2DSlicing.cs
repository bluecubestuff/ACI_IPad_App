using UnityEngine;
using System.Collections.Generic;
using UnitySpriteCutter;

public class _2DSlicing : MonoBehaviour
{

    public LayerMask layerMask;

    Vector2 mouseStart;
    void Update()
    {
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            Vector2 mouseEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonUp(0))
            {
                //If cut = success, proceed to cut
                if (MeatFabManager.Instance.startSuccess == true && MeatFabManager.Instance.endSuccess == true)
                {
                    LinecastCut(mouseStart, mouseEnd, layerMask.value);
                    MeatFabManager.Instance.startSuccess = false;
                    MeatFabManager.Instance.endSuccess = false;
                }
            }
        }
    }

    void LinecastCut(Vector2 lineStart, Vector2 lineEnd, int layerMask = Physics2D.AllLayers)
    {
        List<GameObject> gameObjectsToCut = new List<GameObject>();
        RaycastHit2D[] hits = Physics2D.LinecastAll(lineStart, lineEnd, layerMask);
        foreach (RaycastHit2D hit in hits)
        {
            //if slicing hits any sprite
            if (HitCounts(hit))
            {
                //cut gameobject
                gameObjectsToCut.Add(hit.transform.gameObject);
                //Debug.Log("HIT MAYBE");
            }
        }

        foreach (GameObject go in gameObjectsToCut)
        {
            SpriteCutterOutput output = _SpriteCutter.Cut(new _SpriteCutterInput()
            {
                lineStart = lineStart,
                lineEnd = lineEnd,
                gameObject = go,
                gameObjectCreationMode = _SpriteCutterInput.GameObjectCreationMode.CUT_OFF_COPY,
                //gameObjectCreationMode = _SpriteCutterInput.GameObjectCreationMode.CUT_OFF_NEW,
            });
        }
    }

    bool HitCounts(RaycastHit2D hit)
    {
        return (hit.transform.GetComponent<SpriteRenderer>() != null ||
                 hit.transform.GetComponent<MeshRenderer>() != null);
    }

}
