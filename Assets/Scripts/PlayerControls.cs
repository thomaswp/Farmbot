using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerControls : MonoBehaviour
{
    // Start is called before the first frame update

    public Grid grid;
    public Vector3Int cell;
    public int speed = 1;
    public Tilemap collisionTilemap;

    private float transitionTime = 0;
    private Vector3Int moveStart;

    float TransitionDuration { get { return 1f / speed; } }

    private Animator Animator { get { return GetComponent<Animator>(); } }

    private SpriteRenderer SpriteRenderer { get { return GetComponent<SpriteRenderer>(); } }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (transitionTime == 0)
        {
            if (Input.GetKey(KeyCode.D))
            {
                Move(Vector3Int.right);
            }
            if (Input.GetKey(KeyCode.W))
            {
                Move(Vector3Int.up);
            }
            if (Input.GetKey(KeyCode.A))
            {
                Move(Vector3Int.left);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Move(Vector3Int.down);
            }
        }

        if (transitionTime > 0)
        {
            transitionTime = Mathf.Max(transitionTime - Time.deltaTime, 0);
        } 
        else
        {
            Animator.SetBool("Walking", false);
        }
        
        Vector3 target = PositionFromGrid(cell);
        Vector3 start = PositionFromGrid(moveStart);
        transform.position = Vector3.Lerp(target, start, transitionTime / TransitionDuration);
    }

    private Vector3 PositionFromGrid(Vector3Int cell)
    {
        Vector3 target = grid.CellToLocal(cell);
        target.y += SpriteRenderer.bounds.size.y / 2;
        return target;
    }

    private void Move(Vector3Int direction)
    {
        if (transitionTime != 0) return;

        if (this.Animator != null)
        {
            Animator.SetFloat("DirX", direction.x);
            Animator.SetFloat("DirY", direction.y);
        }

        Vector3Int targetCell = cell + direction;
        if (collisionTilemap)
        {
            if (collisionTilemap.GetTile(targetCell) != null) return;
        }

        transitionTime = TransitionDuration;
        moveStart = cell;
        cell = targetCell;

        if (this.Animator != null)
        {
            Animator.SetBool("Walking", true);
        }
    }
}
