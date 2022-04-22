using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using BlocklyBridge;

namespace Farmbot
{
    [ScriptableBehavior("Movement", 60)]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Interpreter))]
    public class ActorActions : MonoBehaviour
    {
        public const string MOVEMENT_CATEGORY = "movement";

        public Grid grid;
        public Vector3Int cell;
        public int speed = 1;
        public Tilemap collisionTilemap;

        private float transitionTime = 0;
        private Vector3Int moveStart;

        private Direction _direction = Direction.Down;
        public Direction Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                Animator anim = Animator;
                if (anim != null)
                {
                    anim.SetFloat("DirY", 0);
                    anim.SetFloat("DirX", 0);
                    switch (_direction)
                    {
                        case Direction.Down: anim.SetFloat("DirY", -1); break;
                        case Direction.Up: anim.SetFloat("DirY", 1); break;
                        case Direction.Left: anim.SetFloat("DirX", -1); break;
                        case Direction.Right: anim.SetFloat("DirX", 1); break;
                    }
                    
                }
            }
        }


        public static Direction DirectionFromXY(int x, int y)
        {
            if (x > 0) return Direction.Right;
            if (x < 0) return Direction.Left;
            if (y < 0) return Direction.Down;
            if (y > 0) return Direction.Up;
            return Direction.Down;
        }

        float TransitionDuration { get { return 1f / speed; } }

        private Animator Animator { get { return GetComponent<Animator>(); } }
        public Interpreter Interpreter { get { return GetComponent<Interpreter>(); } }

        private SpriteRenderer SpriteRenderer { get { return GetComponent<SpriteRenderer>(); } }

        public bool CanMove
        {
            get
            {
                return transitionTime == 0 && Interpreter.CountCategory(MOVEMENT_CATEGORY) == 0;
            }
        }

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private Vector3 PositionFromGrid(Vector3Int cell)
        {
            Vector3 target = grid.CellToLocal(cell);
            target.y += SpriteRenderer.bounds.size.y / 2;
            return target;
        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Clicked!");
                OnClick();
            }
        }

        [ScriptableEvent(false)]
        public void OnClick()
        {
            BlocklyGenerator.SendEvent(Interpreter, System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        [ScriptableMethod]
        public AsyncMethod TurnRight()
        {
            return Turn(1);
        }

        [ScriptableMethod]
        public AsyncMethod TurnLeft()
        {
            return Turn(-1);
        }

        private AsyncMethod Turn(int change)
        {
            return Interpreter.ExecuteMethod(MOVEMENT_CATEGORY).Do(() =>
            {
                Direction = (Direction)((((int)Direction) + change + 4) % 4);
                Debug.Log(Direction);
            });
        }

        [ScriptableMethod]
        public AsyncMethod MoveForward()
        {
            return Move(Direction);
        }

        [ScriptableMethod]
        public AsyncMethod FaceDirection(Direction direction)
        {
            return Interpreter.ExecuteMethod(MOVEMENT_CATEGORY).Do(() =>
            {
                Direction = direction;
            });
        }

        [ScriptableMethod]
        public AsyncMethod Move(Direction direction)
        {
            if (direction == Farmbot.Direction.Down) return Move(0, -1);
            if (direction == Farmbot.Direction.Up) return Move(0, 1);
            if (direction == Farmbot.Direction.Left) return Move(-1, 0);
            if (direction == Farmbot.Direction.Right) return Move(1, 0);
            return null;
        }

        public AsyncMethod Move(int dx, int dy)
        {
            return Interpreter.ExecuteMethod(MOVEMENT_CATEGORY)
            .Do(() =>
            {

                Direction = DirectionFromXY(dx, dy);

                Vector3Int targetCell = cell + new Vector3Int(dx, dy, 0);
                if (collisionTilemap)
                {
                    if (collisionTilemap.GetTile(targetCell) != null) return;
                }

                transitionTime = TransitionDuration;
                moveStart = cell;
                cell = targetCell;

                if (Animator != null)
                {
                    Animator.SetBool("Walking", true);
                }
            })
            .UpdateUntil(() =>
            {
                if (transitionTime <= 0)
                {
                    Animator.SetBool("Walking", false);
                    return true;
                }

                transitionTime = Mathf.Max(transitionTime - Time.deltaTime, 0);

                Vector3 target = PositionFromGrid(cell);
                Vector3 start = PositionFromGrid(moveStart);
                transform.position = Vector3.Lerp(target, start, transitionTime / TransitionDuration);
                return false;
            });
        }
    }

}