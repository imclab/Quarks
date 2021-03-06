using UnityEngine;
using System.Collections;
 
/// <summary>
/// Provides functionality to use sprites in your scenes that are filled with a pattern image.
/// </summary>
public class OTFilledSprite : OTSprite
{
    //-----------------------------------------------------------------------------
    // Editor settings
    //-----------------------------------------------------------------------------

    
    public Vector2 _fillSize = new Vector2(0, 0);
    
    public Vector2 _scrollSpeed = new Vector2(0, 0);

    //-----------------------------------------------------------------------------
    // public attributes (get/set)
    //-----------------------------------------------------------------------------
    /// <summary>
    /// Size x/y in pixels of the fill image
    /// </summary>
    public Vector2 fillSize
    {
        get
        {
            return _fillSize;
        }
        set
        {
            _fillSize = value;
            Clean();
        }
    }
    /// <summary>
    /// Scroll value x/y in pixels per second
    /// </summary>
    public Vector2 scrollSpeed
    {
        get
        {
            return _scrollSpeed;
        }
        set
        {
            _scrollSpeed = value;
        }
    }

    //-----------------------------------------------------------------------------
    // private and protected fields
    //-----------------------------------------------------------------------------
    Vector2 _fillSize_ = Vector2.zero;

    //-----------------------------------------------------------------------------
    // overridden subclass methods
    //-----------------------------------------------------------------------------
    
    protected override void CheckSettings()
    {
		Vector3 oldSize = size;

		var imageChanged = false;
		if (image!=_image_)
			imageChanged = true;
				
        base.CheckSettings();		
		// if we changed the image, reset old size because it was set to dimensions of the new image
		if (imageChanged) 
			size = oldSize;

        if (fillSize != _fillSize_)
        {
            _fillSize_ = fillSize;
            isDirty = true;
        }
    }

    /// <summary>
    /// This will force a refill of the filled sprite.
    /// </summary>
    public void Refill()
    {
        Clean();
    }
    
	public override void PassiveUpdate()
	{
		if (!scrollSpeed.Equals(Vector2.zero))
			Update();
	}
	
    protected override string GetTypeName()
    {
        return "Filled Sprite";
    }

    
    public override string GetMatName()
    {
       return base.GetMatName() + "-Size:"+size.ToString()+"-fill:" + fillSize.ToString();
    }
	
	protected override void Resized()
	{
		SetTexture();
	}
	
    void SetTexture()
    {
        if (image != null)
        {			
            var mat = material;
			if (mat!=null)
			{
				var  oldScale = mat.mainTextureScale;
				var  mainScale = Vector2.zero;
	            if (fillSize.Equals(Vector2.zero) || Vector2.Equals(fillSize, size))
	                mainScale = Vector2.one;
	            else
	                mainScale = new Vector2(1 / (fillSize.x / size.x), 1 / (fillSize.y / size.y));
				if (mainScale!=oldScale)
				{
	            	mat.mainTextureScale = mainScale;
	            	mat.mainTextureOffset = new Vector2(0, mat.mainTextureScale.y * -1);
				}
			}
        }
    }

    
    protected override Material InitMaterial()
    {
        var mat = base.InitMaterial();
        SetTexture();
        return mat;
    }

    
    protected override void Clean()
    {
        base.Clean();
        SetTexture();
    }

    //-----------------------------------------------------------------------------
    // class methods
    //-----------------------------------------------------------------------------

    
    protected override void Awake()
    {
		passiveControl = true;
        _fillSize_ = fillSize;
        base.Awake();
    }


    new void Start()
    {
        base.Start();
    }
	
 	new void Update()
    {
	
		if (otTransform == null)
			return;
		
		if (!_size_.Equals(new Vector2(otTransform.localScale.x,otTransform.localScale.y)))
		{
			isDirty = true;
		}
		
		
		if (!passive)
        	base.Update();
			
		// scroll background
        if (!scrollSpeed.Equals(Vector2.zero))
        {
            var mat = material;
            var dx = ((1 / mat.mainTextureScale.x) * (size.x / fillSize.x / 10)) * scrollSpeed.x * Time.deltaTime;
            var dy = ((1 / mat.mainTextureScale.y) * (size.y / fillSize.y / 10)) * scrollSpeed.y * Time.deltaTime;
			
            var nx = mat.mainTextureOffset.x + dx;
            var ny = mat.mainTextureOffset.y + dy;
            if (dx < 0 && nx < 0) nx += 1;
            if (dx > 0 && nx > 1) nx -= 1;
            if (dy < 0 && ny < 0) ny += 1;
            if (dy > 0 && ny > 1) ny -= 1;

            mat.mainTextureOffset = new Vector2(nx, ny);
        }
			
    }
}
