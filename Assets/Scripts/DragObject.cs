using UnityEngine;

namespace Assets.Scripts
{
    public class DragObject : MonoBehaviour
    {
        private Vector2 _mousePosition;
        private Vector2 _initialObjectPosition;

        private float _offsetX, _offsetY;

        private static bool _mouseButtonReleased;

        private bool _objectCollided;

        private void OnMouseDown()
        {
            ClickMerge();
            _mouseButtonReleased = false;
            _initialObjectPosition = transform.position;
            _offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
            _offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
        }

        private void OnMouseDrag()
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(_mousePosition.x - _offsetX, _mousePosition.y - _offsetY);
        }

        private void OnMouseUp()
        {
            _mouseButtonReleased = true;
            if (!_objectCollided) transform.position = _initialObjectPosition;
        }

        private void OnTriggerStay2D(Component collision)
        {
            _objectCollided = true;
            switch (_mouseButtonReleased)
            {
                case true when (collision.CompareTag(gameObject.tag) || collision.CompareTag(SpellGenerator.FirstSelectedTag)):
                    //Instantiate();
                    _mouseButtonReleased = false;
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                    _objectCollided = false;
                    SpellGenerator.FirstSelected = false;
                    break;
                case true:
                    _mouseButtonReleased = false;
                    transform.position = _initialObjectPosition;
                    _objectCollided = false;
                    break;
            }
        }

        private void ClickMerge()
        {
            if (!SpellGenerator.FirstSelected)
            {
                SpellGenerator.FirstSelectedTag = gameObject.tag;
                gameObject.tag = "Selected";
                SpellGenerator.FirstSelected = true;
                GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else if (SpellGenerator.FirstSelected && CompareTag("Selected"))
            {
                gameObject.tag = SpellGenerator.FirstSelectedTag;
                SpellGenerator.FirstSelectedTag = null;
                SpellGenerator.FirstSelected = false;
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            else if (SpellGenerator.FirstSelected && CompareTag(SpellGenerator.FirstSelectedTag))
            {
                //Instantiate();
                Destroy(GameObject.FindWithTag("Selected"));
                Destroy(gameObject);
                SpellGenerator.FirstSelectedTag = null;
                SpellGenerator.FirstSelected = false;
            }
        }

    }
}
