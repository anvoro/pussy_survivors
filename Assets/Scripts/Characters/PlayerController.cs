using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

public class PlayerController : CharacterBase
{
    [Serializable]
    public struct LevelConfig
    {
        public int XpToNextLevel;

        public float HpAdd;
        public float SpeedAdd;
        public float AddDamage;
    }

    public LevelConfig[] Levels;
    
    public Rigidbody2D Rigidbody2D;

    public Canvas Canvas;
    
    private Camera _camera;

    public Transform HealthBarPivot;

    public Slider HealthBar;
    public Slider XPBar;
    public TMP_Text LevelText;

    public WeaponBase[] Weapons;

    public int Level => this._level;
    
    private int _level = 0;
    private int _currentXp;
    private int _xpToNextLevel;

    public float DamageBase = 6;

    public float Damage => DamageBase + this.Levels[this._level].AddDamage;
    
    public override float Speed => base.Speed + this.Levels[this._level].SpeedAdd;

    public override float MaxHealth => base.MaxHealth + this.Levels[this._level].HpAdd;

    public int CurrentXP
    {
        get => this._currentXp;
        set
        {
            this._currentXp = value;
            if (this._currentXp >= this.Levels[this._level].XpToNextLevel)
            {
                LevelUP();
                _currentXp = 0;
            }
            
            SetXPBar();
        }
    }

    void LevelUP()
    {
        if (this._level < Levels.Length - 1)
        {
            this._level++;
            
            LevelText.text = $"{this._level + 1}";

            _xpToNextLevel = this.Levels[this._level].XpToNextLevel;
            this._currentHealth = this.MaxHealth;
            this.SetHealthBar();
        }
        else
        {
            LevelText.text = "MAX";
        }
    }
    
    void SetXPBar()
    {
        this.XPBar.maxValue = this._xpToNextLevel;
        this.XPBar.value = this.CurrentXP;
    }

    // Start is called before the first frame update
    protected override void Awake()
    {
        this.OnHealthChanged += this.SetHealthBar;
        
        this._camera = Camera.main;
        
        this.Reset();
        base.Awake();
    }

    private void Start()
    {
        foreach (WeaponBase weapon in this.Weapons)
        {
            weapon.Init(this);
        }
        
        _xpToNextLevel = this.Levels[this._level].XpToNextLevel;
        
        this.SetXPBar();
        this.SetHealthBar();
        LevelText.text = $"{this._level + 1}";
    }

    protected override void Update()
    {
        for (int i = 0; i < this.Weapons.Length; i++)
        {
            WeaponBase weapon = this.Weapons[i];
            weapon.Tick(Time.deltaTime);
        }

        base.Update();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < this.Weapons.Length; i++)
        {
            WeaponBase weapon = this.Weapons[i];
            weapon.DrawG();
        }
    }

    private void FixedUpdate()
    {
        Vector2 worldPosition = this._camera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 position = this.Position;
        Vector2 moveVector = position - worldPosition;
        
        this.Velocity = moveVector;
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
