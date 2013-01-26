using UnityEngine;
using System.Collections;

public class PressureField
{
	const int WIDTH = 128;
    const int HEIGHT = 96;
	const int TOTAL_PIXELS = WIDTH * HEIGHT;
	const int ROW_STRIDE = WIDTH * 4;
	int scale = 10;
    
    int counter = 0;
    //var linePosition = 0;
    //var imageData;
    int[] data;
    //var tmpState;
	int[] tmpState1;
	int[] tmpState2;
    
    int mx = 0, my = 0;
    bool mouseMove = false;
	
	public Texture2D texture;

    //window.addEventListener('load', init, false);
    
    public void init(){
		/*
        canvas = document.getElementById('c');
        canvas.width = WIDTH * scale;//"" + (WIDTH * scale) + "px";
        canvas.height = HEIGHT * scale;//"" + (HEIGHT * scale) + "px";
        context = canvas.getContext("2d");
        context.scale(scale, scale);
        
        
        lowResCanvas = document.createElement('canvas');
        lowResCanvas.width = WIDTH;
        lowResCanvas.height = HEIGHT;
        
        lowResContext = lowResCanvas.getContext('2d');
        imageData = lowResContext.createImageData(WIDTH, HEIGHT);    
        data = imageData.data;
        //*/
        
        tmpState1 = new int[TOTAL_PIXELS];
        tmpState2 = new int[TOTAL_PIXELS];
		data = new int[TOTAL_PIXELS*4];
		
		// create texture resolution at screen.width x screen.height

		/*
        document.onmousemove = onMouseMove;
        document.ontouchmove = onTouchMove;
        //*/
        
		// djmc: uncomment when we're ready...
		/*
        seedWorld();
        Update();
        //*/
		
		// duplicate the original texture and assign to the material
		Texture2D temp = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
		texture = (Texture2D)Texture2D.Instantiate(temp);//renderer.material.mainTexture);
	    //renderer.material.mainTexture = texture;
    }
    
    int getAlpha(int x, int y)
    {
        return data[y * ROW_STRIDE + x * 4 + 3];
    }
    
    void seedWorld()
    {
        for(var y = 0; y < HEIGHT; y++)
        {
            for(var x = 0; x < WIDTH; x++)
            {
                tmpState1[y * WIDTH + x] = 0;
                tmpState2[y * WIDTH + x ] = 0;
            }
        }
        
        tmpState2[HEIGHT >> 1 * WIDTH + WIDTH >> 1] = 255;
    }
    
    void processWater(int[] source, int[] dest)
    {
        for (var i = WIDTH; i < TOTAL_PIXELS - WIDTH; i++)
        {
            dest[i] = (((source[i-1] + source[i+1] + source[i-WIDTH] + source[i+WIDTH])  >> 1) ) - dest[i];
            dest[i] -= (dest[i] >> 7);
        }
    }

	/*
    void onMouseMove(event)
    {
        mx = Math.max(Math.min(parseInt(event.pageX / scale), WIDTH - 1), 1);
        my = Math.max(Math.min(parseInt(event.pageY / scale), HEIGHT - 2), 1);
        mouseMove = true;
    }
    
    void onTouchMove(event)
    {
        event.preventDefault();
        mx = Math.max(Math.min(parseInt(event.targetTouches[0].pageX / scale), WIDTH - 1), 1);
        my = Math.max(Math.min(parseInt(event.targetTouches[0].pageY / scale), HEIGHT - 2), 1);
        mouseMove = true;
    }
    //*/
    
    void handleMouseInput()
    {
        if(!mouseMove)
            return;
        
        if(counter % 2 == 0)
            tmpState1[my * WIDTH + mx ] = 2048;
        else
            tmpState2[my * WIDTH + mx ] = 2048;
            
        mouseMove = false;
    }
    
    public void Update(){
        counter += 1;
       
        handleMouseInput();
       
        if(counter % 2 == 0)
        {
            processWater(tmpState1, tmpState2);
        }
        else
        {
            processWater(tmpState2, tmpState1);
        }
       
        
        for(var y = 0; y < HEIGHT; y++)
        {
            for(var x = 0; x < WIDTH; x++)
            { 
                //set the cell color
                var value = 127 + ((counter % 2 == 0) ? tmpState2[y * WIDTH + x] : tmpState1[y * WIDTH + x]) >> 4;
                
                //clamp the value to the valid ranges.
                if(value > 255 )
                    value = 255;
                else if(value < 0)
                    value = 0;
                
                data[y * ROW_STRIDE + x * 4] = value;
                data[y * ROW_STRIDE + x * 4 + 1] = value;
                data[y * ROW_STRIDE + x * 4 + 2] = value;
                data[y * ROW_STRIDE + x * 4 + 3] = 255;              
            }
        }

		/*
        lowResContext.clearRect(0, 0, WIDTH, HEIGHT);
        lowResContext.putImageData(imageData, 0, 0,0,0,WIDTH * scale,HEIGHT * scale);
        //*/
        
        //context.clearRect(0, 0, WIDTH * scale,HEIGHT * scale);
		/*
        context.fillStyle = context.createPattern(lowResCanvas, 'repeat');
        context.fillRect(0, 0, WIDTH * scale,HEIGHT * scale);
        window.setTimeout(runLoop, 14);
        //*/
	
	    // colors used to tint the first 3 mip levels
	    Color[] colors = new Color[3];
	    colors[0] = Color.red;
	    colors[1] = Color.green;
	    colors[2] = Color.blue;
	    int mipCount = Mathf.Min(3, texture.mipmapCount);
	
	    // tint each mip level
	    for(int mip = 0; mip < mipCount; ++mip)
		{
	        Color[] cols = texture.GetPixels(mip);
	        for(int i = 0; i < cols.Length; ++i)
			{
	            cols[i] = Color.Lerp(cols[i], colors[mip], 0.33f);
	        }
	        texture.SetPixels(cols, mip);
	    }
	
	    // actually apply all SetPixels, don't recalculate mip levels
	    texture.Apply(false);
    }
}

public class WaveField // there can be only one!
{
	// constant
	public static int kNumBuffers = 2;
	public static int kPointFieldWidth  = 10; // width of (square) field in points
	public static int kPointFieldHeight = 10;
	public static int kBufferSize = kPointFieldWidth*kPointFieldHeight;

	public class WavePoint
	{
		public Vector3 direction;
		public WavePoint()
		{
			direction = Vector3.zero;
		}
	}

	// these update regularly
	WavePoint[] vectorField; // double buffered; 2 * height * width
	int currBufferIdx;
	public int nextBufferIdx
	{
		get { return (currBufferIdx+1)%kNumBuffers; }
	}
	
	public WaveField()
	{
		vectorField = new WavePoint[kNumBuffers*kBufferSize];
		for (int bufferIdx = 0; bufferIdx < kNumBuffers; ++bufferIdx)
		{
			for (int y = 0; y < kPointFieldHeight; ++y)
			{
				for (int x = 0; x < kPointFieldWidth; ++x)
				{
					vectorField[bufferIdx*kBufferSize+y*kPointFieldWidth+x] = new WavePoint();
				}
			}
		}
		currBufferIdx = 0;
	}

	WavePoint GetPoint(int bufferIdx, int x, int y)
	{
		return vectorField[bufferIdx*kBufferSize + y*kPointFieldWidth + x];
	}

	void Update()
	{
		/*
		Vector3 downLeft  = (Vector3.down + Vector3.left).normalized;
		Vector3 downRight = (Vector3.down + Vector3.right).normalized;
		Vector3 upLeft    = (Vector3.up + Vector3.left).normalized;
		Vector3 upRight   = (Vector3.up + Vector3.right).normalized;

		// propagate the wave fields
		for (int y = 1; y < kPointFieldHeight-1; ++y)
		{
			for (int x = 1; x < kPointFieldWidth-1; ++x)
			{
				WavePoint nextPt = GetPoint(nextBufferIdx, x, y);
				nextPt.direction = Vector3.zero +
					//Vector3.Dot(GetPoint(currBufferIdx, x-1, y-1).direction, downLeft) +
					//Vector3.Dot(GetPoint(currBufferIdx, x  , y-1).direction, Vector3.down) +
					//Vector3.Dot(GetPoint(currBufferIdx, x+1, y-1).direction, downRight) +
					//Vector3.Dot(GetPoint(currBufferIdx, x-1, y  ).direction, Vector3.left) +
					//Vector3.Dot(GetPoint(currBufferIdx, x  , y  ) +
					//Vector3.Dot(GetPoint(currBufferIdx, x+1, y  ).direction, Vector3.right) +
					//Vector3.Dot(GetPoint(currBufferIdx, x-1, y+1).direction, upLeft) +
					//Vector3.Dot(GetPoint(currBufferIdx, x  , y+1).direction, Vector3.up) +
					Vector3.Dot(GetPoint(currBufferIdx, x+1, y+1).direction, upRight);
				nextPt.direction /= 9;
			}
		}
		//*/
	}
}
