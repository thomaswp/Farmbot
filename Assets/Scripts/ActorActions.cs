using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Farmbot
{
    [ScriptableBehavior]
    [RequireComponent(typeof(SpriteRenderer))]
    public class ActorActions : MonoBehaviour
    {
        // Start is called before the first frame update

        public Grid grid;
        public Vector3Int cell;
        public int speed = 1;
        public Tilemap collisionTilemap;

        private float transitionTime = 0;
        private Vector3Int moveStart;

        private Direction direction;

        private List<AsyncMethod> executingMethods = new List<AsyncMethod>();

        float TransitionDuration { get { return 1f / speed; } }

        private Animator Animator { get { return GetComponent<Animator>(); } }

        private SpriteRenderer SpriteRenderer { get { return GetComponent<SpriteRenderer>(); } }

        public bool CanMove
        {
            get
            {
                return transitionTime == 0;
            }
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < executingMethods.Count; i++)
            {
                AsyncMethod method = executingMethods[i];
                if (method.Update())
                {
                    executingMethods.RemoveAt(i--);
                }
            }
        }

        T ExecuteMethod<T>(T method) where T : AsyncMethod
        {
            executingMethods.Add(method);
            return method;
        }

        private Vector3 PositionFromGrid(Vector3Int cell)
        {
            Vector3 target = grid.CellToLocal(cell);
            target.y += SpriteRenderer.bounds.size.y / 2;
            return target;
        }


        [ScriptableMethod]
        public AsyncFunction<Direction> Direction()
        {
            return ExecuteMethod(new AsyncFunction<Direction>().Return(() => direction));
        }

        [ScriptableMethod]
        public AsyncMethod MoveForward()
        {
            return Move(direction);
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
            return ExecuteMethod(new AsyncMethod()
                .UpdateUntil(() => transitionTime == 0)
                .Do(() =>
                {
                    if (Animator != null)
                    {
                        Animator.SetFloat("DirX", dx);
                        Animator.SetFloat("DirY", dy);
                    }

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
                }));
        }
    }

}