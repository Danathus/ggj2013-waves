using UnityEngine;
using System.Collections;

public class WaveField
{
	const int WIDTH = 1024/4;//128;
    const int HEIGHT = 768/4;//96;
	const int TOTAL_PIXELS = WIDTH * HEIGHT;
	const int ROW_STRIDE = WIDTH;// * 4;
	int scale = 10;
    
    int counter = 0;
    //var linePosition = 0;
    //var imageData;
    Color[] data;
    //var tmpState;
	int[] tmpState1;
	int[] tmpState2;
    
    int mx = 0, my = 0;
    bool mouseMove = false;
	
	public Texture2D texture;

    //window.addEventListener('load', init, false);
    
    public void init()
	{        
        tmpState1 = new int[TOTAL_PIXELS];
        tmpState2 = new int[TOTAL_PIXELS];
		data = new Color[TOTAL_PIXELS];
		
		// create texture resolution at screen.width x screen.height
        
		// djmc: uncomment when we're ready...
        seedWorld();
		
		// duplicate the original texture and assign to the material
		//Texture2D temp = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
		Texture2D temp = new Texture2D(WIDTH, HEIGHT, TextureFormat.ARGB32, false);
		texture = (Texture2D)Texture2D.Instantiate(temp);//renderer.material.mainTexture);
	    //renderer.material.mainTexture = texture;
    }
    
	/*
    Color getColor(int x, int y)
    {
        //return data[y * ROW_STRIDE + x * 4 + 3];
		return data[y*ROW_STRIDE + x];
    }
    //*/

	Vector2 ConvertScreenCoordinatesToGridCoordinates(Vector2 screenCoordinates)
	{
		Vector2 normalizedScreenCoordinates = new Vector2(
			(screenCoordinates.x - Screen.width/2) / (Screen.width/2),
			(screenCoordinates.y - Screen.height/2) / (Screen.height/2));
		Vector2 gridCoordinates = new Vector2(
			normalizedScreenCoordinates.x * WIDTH/2 + WIDTH/2,
			normalizedScreenCoordinates.y * HEIGHT/2 + HEIGHT/2);
		return gridCoordinates;
	}
	
	public void SetPressure(Vector2 screenCoordinates, int pressure) // think big, powers of 2
	{
		// todo: need to convert between real-space and grid-space
		//    question: how many units to the edge of the screen? Can we affix that to something specific?
		Vector2 gridCoordinates = ConvertScreenCoordinatesToGridCoordinates(screenCoordinates);
		int grid_x = (int)(gridCoordinates.x);
		int grid_y = (int)(gridCoordinates.y);
		int index = grid_y * WIDTH + grid_x;
		
		// If we're in bounds apply the pressure
		if (index < tmpState1.Length) {
			if(counter % 2 == 0)
	            tmpState1[index] = pressure;
	        else
	            tmpState2[index] = pressure;
		}
		
	}
	
	public int GetPressure(Vector2 screenCoordinates)
	{
		// todo: need to convert between real-space and grid-space
		Vector2 gridCoordinates = ConvertScreenCoordinatesToGridCoordinates(screenCoordinates);
		int grid_x = (int)(gridCoordinates.x);
		int grid_y = (int)(gridCoordinates.y);
		int pressure = 0;
		if(counter % 2 == 0)
            pressure = tmpState1[grid_y * WIDTH + grid_x ];
        else
            pressure = tmpState2[grid_y * WIDTH + grid_x ];
		return pressure;
	}
    
    void seedWorld()
    {
        for(var y = 0; y < HEIGHT; y++)
        {
            for(var x = 0; x < WIDTH; x++)
            {
                //tmpState1[y * WIDTH + x]  = 0;
                //tmpState2[y * WIDTH + x ] = 0;
				tmpState1[y * WIDTH + x]  = 0;
                tmpState2[y * WIDTH + x ] = 0;
            }
        }
        
        tmpState2[HEIGHT/2 * WIDTH + WIDTH /2] = 1 << 15; //12;
    }
    
    void processWater(int[] source, int[] dest)
    {
        for (var i = WIDTH; i < TOTAL_PIXELS - WIDTH; i++)
        {
			// blend in from adjacent cells
            dest[i] = (((source[i-1] + source[i+1] + source[i-WIDTH] + source[i+WIDTH])  >> 1) ) - dest[i];
			// dampen
            //dest[i] -= (dest[i] >> 7);
			//dest[i] -= (dest[i] >> 4);
			dest[i] -= (dest[i] >> 5);
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
		// hack
		//mouseMove = true;
		//mx = WIDTH/2;
		//my = HEIGHT/2;
		// hack

        if(!mouseMove)
            return;
        
        if(counter % 2 == 0)
            tmpState1[my * WIDTH + mx ] = 4096;
        else
            tmpState2[my * WIDTH + mx ] = 4096;
            
        mouseMove = false;
    }
    
    public void Update(){		
		// physical integration step
		//*
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
		//*/
        
        for(var y = 0; y < HEIGHT; y++)
        {
            for(var x = 0; x < WIDTH; x++)
            { 
                //set the cell color
                int val = 127 + ((counter % 2 == 0) ? tmpState2[y * WIDTH + x] : tmpState1[y * WIDTH + x]) >> 4;
                
                //clamp the value to the valid ranges.
                if(val > 255 )
                    val = 255;
                else if(val < 0)
                    val = 0;
                float fval = (float)val / 255.0f;
                data[y * ROW_STRIDE + x].r = fval;
                data[y * ROW_STRIDE + x].g = fval;
                data[y * ROW_STRIDE + x].b = fval;
                data[y * ROW_STRIDE + x].a = 1.0f;//255;              
            }
        }

		texture.SetPixels(data, 0);
	
	    // actually apply all SetPixels, don't recalculate mip levels
	    texture.Apply(false);
    }
}

/*
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
		//Vector3 downLeft  = (Vector3.down + Vector3.left).normalized;
		//Vector3 downRight = (Vector3.down + Vector3.right).normalized;
		//Vector3 upLeft    = (Vector3.up + Vector3.left).normalized;
		//Vector3 upRight   = (Vector3.up + Vector3.right).normalized;

		// propagate the wave fields
		//for (int y = 1; y < kPointFieldHeight-1; ++y)
		//{
		//	for (int x = 1; x < kPointFieldWidth-1; ++x)
		//	{
		//		WavePoint nextPt = GetPoint(nextBufferIdx, x, y);
		//		nextPt.direction = Vector3.zero +
					//Vector3.Dot(GetPoint(currBufferIdx, x-1, y-1).direction, downLeft) +
					//Vector3.Dot(GetPoint(currBufferIdx, x  , y-1).direction, Vector3.down) +
					//Vector3.Dot(GetPoint(currBufferIdx, x+1, y-1).direction, downRight) +
					//Vector3.Dot(GetPoint(currBufferIdx, x-1, y  ).direction, Vector3.left) +
					//Vector3.Dot(GetPoint(currBufferIdx, x  , y  ) +
					//Vector3.Dot(GetPoint(currBufferIdx, x+1, y  ).direction, Vector3.right) +
					//Vector3.Dot(GetPoint(currBufferIdx, x-1, y+1).direction, upLeft) +
					//Vector3.Dot(GetPoint(currBufferIdx, x  , y+1).direction, Vector3.up) +
		//			Vector3.Dot(GetPoint(currBufferIdx, x+1, y+1).direction, upRight);
		//		nextPt.direction /= 9;
		//	}
		//}
	}
}
//*/
