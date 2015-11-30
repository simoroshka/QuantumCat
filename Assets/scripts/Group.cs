using UnityEngine;
using System.Collections;

public class Group : MonoBehaviour
{
    // Time since last gravity tick
    float lastFall = 0;
    bool colorMeasured = false;
    bool orientMeasured = false;
    bool falling = false;
    bool opened = false;
  

    public Sprite[] types;
    int type;

    void open()
    {
        opened = true;
        openShape();
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "grey")
            {
                child.gameObject.SetActive(false);
            }
            else if (child.gameObject.tag != "secret")
            {
                child.gameObject.SetActive(true);
            }
        }

    }
    void openColor()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "secret")
            {
                child.gameObject.GetComponent<SpriteRenderer>().sprite = types[type];
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    void openShape()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "secret")
            {
                child.gameObject.SetActive(false);
            }
            if (child.gameObject.tag == "grey")
            {
                child.gameObject.SetActive(true);
            }
        }
    }


    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag != "grey" && child.gameObject.tag != "secret")
            {
                Vector2 v = Grid.roundVec2(child.position);

                // Not inside Border?
                if (!Grid.insideBorder(v))
                    return false;

                // Block in grid cell (and not part of same group)?
                if ((int)v.x < Grid.w && (int)v.x >= 0 && (int)v.y < Grid.h && (int)v.y >= 0)

                    if (Grid.grid[(int)v.x, (int)v.y] != null &&                        
                        Grid.grid[(int)v.x, (int)v.y].parent != transform)
                         return false;
            }
        }
                
        return true;
    }
   
             

    void updateGrid()
    {
        
            // Remove old children from grid
            for (int y = 0; y < Grid.h; ++y)
            for (int x = 0; x < Grid.w; ++x)
                if (Grid.grid[x, y] != null)
                    if (Grid.grid[x, y].parent == transform)
                        Grid.grid[x, y] = null;

            // Add new children to grid
            foreach (Transform child in transform)
            {
                if (child.gameObject.tag != "grey" && child.gameObject.tag != "secret")
                {
                    Vector2 v = Grid.roundVec2(child.position);
                        Grid.grid[(int)v.x, (int)v.y] = child;
                }
           
            }
    }

    void Update()
    {

        if (!opened)
        {
            move();
        }

        else { 

            if (Time.time >= Grid.hitTimeStamp + Grid.delay)
            {
                deleteThings();
                Grid.hitTimeStamp = 1000000f;
                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();
                // Disable script
                enabled = false;
            }
        }
    }
       

    void moveLeft()
    {
        // Modify position
        transform.position += new Vector3(-1, 0, 0);

        // See if valid
        if (isValidGridPos())
            // Its valid. Update grid.
            updateGrid();
        else
            // Its not valid. revert.
            transform.position += new Vector3(1, 0, 0);
    }
    void moveRight()
    {
        // Modify position
        transform.position += new Vector3(1, 0, 0);

        // See if valid
        if (isValidGridPos())
            // It's valid. Update grid.
            updateGrid();
        else
            // It's not valid. revert.
            transform.position += new Vector3(-1, 0, 0);
    }
    void measureColor()
    {
        if (!colorMeasured && !orientMeasured)
        {
            colorMeasured = true;
            openColor();
        }

    }
    void measureOrientation()
    {
        if (!colorMeasured && !orientMeasured)
        {
            orientMeasured = true;
            openShape();
        }

    }
    void fall()
    {
        
        if (falling)
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (isValidGridPos())
            {
                // It's valid. Update grid.
                updateGrid();
            }
            else
            {
                Grid.hitTimeStamp = Time.time;

                // It's not valid. revert.
                transform.position += new Vector3(0, 1, 0);
                
                // open the thing
                open();
                
                Grid.dropParticles();       
            }
        }
        
        // Move Downwards and Fall every second
        else if (Time.time - lastFall >= 1)
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (isValidGridPos())
            {
                // It's valid. Update grid.
                updateGrid();
            }
            else
            {
                Grid.hitTimeStamp = Time.time;
                // It's not valid. revert.
                transform.position += new Vector3(0, 1, 0);
                
                open();

                Grid.dropParticles();
               
            }
            lastFall = Time.time;
        }
    }
    void move()
    {
        // Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveLeft();
        }
        // Move Right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveRight();
        }
        // Measure color
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            measureColor();
        }
        // Measure orientation
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            measureOrientation();

        }
        //start falling
        else if (Input.GetKeyDown(KeyCode.Space) && !falling)
        {
            falling = true;
        }
        // Fall
        fall();
        

    }   

    void deleteThings()
    {
        do
        {
            Grid.dropParticles();
            Grid.deleteFullRows();
        } while (Grid.deleteGroups());
    }

    void Start()
    {
        

        //choose random type
        type = Random.Range(0, types.Length);
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag != "grey" && child.gameObject.tag != "secret")
            {
                SpriteRenderer renderer = child.gameObject.GetComponent<SpriteRenderer>();

                renderer.sprite = types[type];
                child.gameObject.GetComponent<Block>().type = type;
            }
           
         
        }

        // random orientation
        int o = Random.Range(0, 4);
        transform.Rotate(0, 0, 90 * o);
        foreach (Transform child in transform)
        {
            child.Rotate(0, 0, -90 * o);
        }

        // Default position not valid? Then it's game over
        if (!isValidGridPos())
        {
            Debug.Log("GAME OVER");
            Application.LoadLevel("lose");
        }
    }
}
