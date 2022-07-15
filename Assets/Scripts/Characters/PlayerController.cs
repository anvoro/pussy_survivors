using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : CharacterBase
{
    public Rigidbody2D Rigidbody2D;

    public Canvas Canvas;
    
    private Camera _camera;

    public Transform HealthBarPivot;

    public Slider HealthBar;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        this.OnHealthChanged += this.SetHealthBar;
        
        this._camera = Camera.main;
        
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        
        this.SetHealthBar();
    }

    private void FixedUpdate()
    {
        Vector2 worldPosition = this._camera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 position = this.Position;
        Vector2 moveVector = position - worldPosition;
        
        this._velocity = moveVector;
        this.Rigidbody2D.MovePosition(Time.deltaTime * this.Speed * -moveVector.normalized + position);
    }

    void SetHealthBar()
    {
        this.HealthBar.maxValue = this.MaxHealth;
        this.HealthBar.value = this.CurrentHealth;
    }

    private void LateUpdate()
    {
        this._camera.transform.position = new Vector3(Position.x, Position.y, -10);
        this.HealthBar.transform.position = worldToUISpace(Canvas, HealthBarPivot.position);
        
        Vector2 worldToUISpace(Canvas parentCanvas, Vector2 worldPos)
        {
            //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
            Vector3 screenPos = this._camera.WorldToScreenPoint(worldPos);
            //Convert the screenpoint to ui rectangle local point
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, null, out Vector2 movePos);
            //Convert the local point to world point
            return parentCanvas.transform.TransformPoint(movePos);
        }
    }
}
