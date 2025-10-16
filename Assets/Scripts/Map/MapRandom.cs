using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRandom : MonoBehaviour
{
    public GameObject[] rooms; 
    private List<Vector2> placedPositions = new List<Vector2>();
    public float minimumDistance = 1.0f;
    public GameObject Player;
    public GameObject Palace;

    [System.Serializable]
    public struct BoundsArea
    {
        public Vector2 minBounds;
        public Vector2 maxBounds;
    }
    public BoundsArea[] boundsAreas;

    void Start()
    {
        ShuffleBounds(); 
        RoomsPlacing();
    }

    void ShuffleBounds()
    {
        // Randomly shuffle the boundsAreas array
        for (int i = 0; i < boundsAreas.Length; i++)
        {
            BoundsArea temp = boundsAreas[i];
            int randomIndex = Random.Range(i, boundsAreas.Length);
            boundsAreas[i] = boundsAreas[randomIndex];
            boundsAreas[randomIndex] = temp;
        }
    }

    public void RoomsPlacing()
    {
        placedPositions.Clear();
        for (int i = 0; i < rooms.Length; i++)
        {
            Vector2 newPosition = PositionFinding(boundsAreas[i]);
            if (newPosition != Vector2.zero)
            {
                rooms[i].SetActive(true);
                rooms[i].transform.position = new Vector3(newPosition.x, newPosition.y, rooms[i].transform.position.z);
                Debug.Log($"Room {rooms[i].name} placed at: {newPosition}");
            }
            else
            {
                rooms[i].SetActive(false);
            }
        }
    }

    public Vector2 PositionFinding(BoundsArea bounds)
    {
        for (int attempts = 0; attempts < 10; attempts++)
        {
            Vector2 possiblePos = new Vector2(
                Random.Range(bounds.minBounds.x, bounds.maxBounds.x),
                Random.Range(bounds.minBounds.y, bounds.maxBounds.y)
            );

            if (IsValidPosition(possiblePos))
            {
                placedPositions.Add(possiblePos);
                return possiblePos;
            }
        }
        return Vector2.zero;
    }

    public bool IsValidPosition(Vector2 position)
    {
        foreach (Vector2 placedPosition in placedPositions)
        {
            if (Vector2.Distance(position, placedPosition) < minimumDistance)
            {
                return false;
            }
        }
        return true;
    }

    public void SaveMapData()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].activeSelf)
            {
                PlayerPrefs.SetFloat("Room" + i + "_X", rooms[i].transform.position.x);
                PlayerPrefs.SetFloat("Room" + i + "_Y", rooms[i].transform.position.y);
                PlayerPrefs.SetInt("Room" + i + "_Active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Room" + i + "_Active", 0);
            }
        }
    }

    public void LoadMapData()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (PlayerPrefs.GetInt("Room" + i + "_Active") == 1)
            {
                float x = PlayerPrefs.GetFloat("Room" + i + "_X");
                float y = PlayerPrefs.GetFloat("Room" + i + "_Y");
                rooms[i].transform.position = new Vector3(x, y, rooms[i].transform.position.z);
                rooms[i].SetActive(true);
            }
            else
            {
                rooms[i].SetActive(false);
            }
        }
    }
}
