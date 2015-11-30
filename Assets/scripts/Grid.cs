using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

    public static int w = 16;
    public static int h = 20;
    public static Transform[,] grid = new Transform[w, h];

    public static float hitTimeStamp = 1000000f;
    public static float delay = 0.2f;

    static GameObject bonusSound; 
    

    public static void initialize(GameObject particlePrefab)
    {
        Sprite[] types = particlePrefab.gameObject.GetComponent<Block>().colors;

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                
                if (Random.Range(0f,1f) < 0.75)
                {
                    GameObject newParticle = Instantiate(particlePrefab);
                    newParticle.transform.position = new Vector3(x, y + 0.5f);
                    int type = Random.Range(0, types.Length);
                    newParticle.gameObject.GetComponent<SpriteRenderer>().sprite = types[type];
                    newParticle.gameObject.GetComponent<Block>().type = type;
                    newParticle.SetActive(true);
                    grid[x, y] = newParticle.transform;
                }
            }
        }
        do
        {
            dropParticles();
        } while (deleteGroups());


        bonusSound = GameObject.Find("meow");
    }

    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),
                           Mathf.Round(v.y-0.5f));
    }

    public static bool insideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < w &&
                (int)pos.y >= 0 && 
                (int)pos.y < h);
    }
    public static void deleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }
    
    public static void decreaseRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                // Move one towards bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // Update Block position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }
    public static void decreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
            decreaseRow(i);
    }
    public static bool isRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
            if (grid[x, y] == null)
                return false;
        return true;
    }
    public static void deleteFullRows()
    {
        for (int y = 1; y < h; ++y)
        {
            if (isRowFull(y))
            {
                bonusSound.gameObject.GetComponent<AudioSource>().Play();
                deleteRow(y);
                decreaseRowsAbove(y + 1);
                --y;
            }
        }
    }

    //**********************
    public static void dropParticles()
    {
        for (int y = h - 2; y >= 0; y--)
        {
            for (int x = 0; x < w; x++)
            {
                if (grid[x, y] == null && grid[x, y+1] != null)
                {
                    moveParticlesAbove(x, y);
                }
            }
        }
        
    }
    public static bool isWinCondition()
    {
            int count = 0;
        for (int x = 0; x < w; x++)
        {
            
            if (grid[x, 0] == null)
            {
                count++;
            }
            else
            {
                count = 0;
            }

            if (count == 4) return true;

        }
        return false;
    }
    public static bool deleteGroups()
    {
        bool deleted = false;
        //for each row
        for (int y = 0; y < h ; y++)
        {
            for (int x = 2; x < w; x++)
            {
                int type1, type2, type3;
                if (grid[x, y] != null) type3 = grid[x, y].gameObject.GetComponent<Block>().type; 
                else continue;
                if (grid[x - 1, y] != null) type2 = grid[x - 1, y].gameObject.GetComponent<Block>().type;
                else continue;
                if (grid[x - 2, y] != null) type1 = grid[x - 2, y].gameObject.GetComponent<Block>().type;
                else continue;
                //we have a match!
                if (type1 == type2 && type2 == type3)
                {
                    deleted = true;
                    for (int i = x-2; i < w; i++)
                    {
                        if (grid[i, y] == null) break;
                        
                        if (grid[i, y].gameObject.GetComponent<Block>().type == type1)
                        {
                            deleteParticle(i, y);
                        }
                        else break;
                    }
                    //play sound
                    
                }
            }
        }


        //for each column
        for (int x = 0; x < w; x++)
        {
            for (int y = h-3; y >= 0; y--)
            {
                int type1, type2, type3;
                if (grid[x, y] != null) type3 = grid[x, y].gameObject.GetComponent<Block>().type;
                else continue;
                if (grid[x, y+1] != null) type2 = grid[x , y+1].gameObject.GetComponent<Block>().type;
                else continue;
                if (grid[x, y+2] != null) type1 = grid[x, y+2].gameObject.GetComponent<Block>().type;
                else continue;
                //we have a match!
                if (type1 == type2 && type2 == type3)
                {
                    deleted = true;
                    for (int i = y + 2; i >= 0; i--)
                    {
                        if (grid[x, i] == null) break;

                        if (grid[x, i].gameObject.GetComponent<Block>().type == type1)
                        {
                            deleteParticle(x, i);
                            moveParticlesAbove(x, i);
                        }
                        else break;

                    }
                    //play sound

                }
            }
        }
        return deleted;

    }
    public static void deleteParticle(int x, int y)
    {
        Destroy(grid[x, y].gameObject);
        grid[x, y] = null;

    }

    //make the particles above the given position fall
    private static void moveParticlesAbove(int x, int y)
    {
        for (int i = y+1; i < h; i++)
        {
            if (grid[x, i] == null) break;

            // Update Block position
            grid[x, i].position += new Vector3(0, -1, 0);
            // Move towards bottom
            grid[x, i - 1] = grid[x, i];
            grid[x, i] = null;
            
            
        }
    }

    private static void updateGrid()
    {

    }
}
