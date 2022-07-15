using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;

    public float Speed = .2f;
    
    private Camera _camera;

    private Transform _transform;

    public Vector2 Position => this._transform.position;
    
    // Start is called before the first frame update
    void Awake()
    {
        this._transform = this.transform;
        this._camera = Camera.main;
    }

    private void FixedUpdate()
    {
        Vector2 worldPosition = this._camera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 position = this.Position;
        Vector2 moveVector = position - worldPosition;
        
        this.Rigidbody2D.MovePosition(Time.deltaTime * this.Speed * -moveVector.normalized + position);
    }

    private void LateUpdate()
    {
        this._camera.transform.position = new Vector3(Position.x, Position.y, -10);
    }
}
