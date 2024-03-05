using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    BasicActions basicActions;
    private InputAction click;
    private Board S_Board;
    
    private void Awake()
    {
        basicActions = new BasicActions();
        S_Board = GetComponent<Board>();
    }

    private void OnEnable()
    {
        click = basicActions.Default.Click;
        click.Enable();
        click.performed += OnClick;
    }

    private void OnDisable()
    {
        click.Disable();
        click.performed -= OnClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        TILE_POSITION tileClicked = ReturnPositionClicked();
        S_Board.Interact(tileClicked);
    }

    private TILE_POSITION ReturnPositionClicked()
    {
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Perform a 2D raycast at click position
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

        // Check if raycast hit a collider
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Tile"))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Tile"))
            {
                return hitObject.GetComponent<Tile>().tilePosition;
            }
        }

        // Default case
        return TILE_POSITION.BOTTOM_LEFT;
    }
}