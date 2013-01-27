using UnityEngine;
using System.Collections;

public class WaveField
{
	public struct WavePixel {
		public int red;
		public int green;
		public int blue;
		public int yellow;
	}
	public static float WavePixelAmplitude(WavePixel pixel)
	{
		return pixel.red + pixel.green + pixel.blue + pixel.yellow;
	}
	
	readonly int WIDTH;
    readonly int HEIGHT;
	readonly int TOTAL_PIXELS;
	readonly int ROW_STRIDE;
	//int scale = 10;
    
    int counter = 0;
    //var linePosition = 0;
    //var imageData;
    Color[] data;
    //var tmpState;
	WavePixel[] tmpState1;
	WavePixel[] tmpState2;
    
    int mx = 0, my = 0;
    bool mouseMove = false;
	
	public Texture2D texture;
	
	public WaveField() {
		WIDTH = 1024 / 4;//128;
	    HEIGHT = 768 / 4;//96;
		TOTAL_PIXELS = WIDTH * HEIGHT;
		ROW_STRIDE = WIDTH;// * 4;
		//scale = 10;
	}

    //window.addEventListener('load', init, false);
    
    public void init()
	{        
        tmpState1 = new WavePixel[TOTAL_PIXELS];
        tmpState2 = new WavePixel[TOTAL_PIXELS];
		data = new Color[TOTAL_PIXELS];
		
		// create texture resolution at screen.width x screen.height
        
        //seedWorld();
		
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
	
	// ------------------------------------------------------------------------
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
	
	// ------------------------------------------------------------------------
	public void SetPressure(Vector2 screenCoordinates, int pressure, Player.ColorCode colorCode) // think big, powers of 2
	{
		// todo: need to convert between real-space and grid-space
		//    question: how many units to the edge of the screen? Can we affix that to something specific?
		Vector2 gridCoordinates = ConvertScreenCoordinatesToGridCoordinates(screenCoordinates);
		int grid_x = (int)(gridCoordinates.x);
		int grid_y = (int)(gridCoordinates.y);
		if (grid_x < 0 || grid_x >= WIDTH || grid_y < 0 || grid_y >= HEIGHT) return;
		int index = grid_y * WIDTH + grid_x;
		
		// If we're in bounds apply the pressure
		if (index < tmpState1.Length) {
			if(counter % 2 == 0) {
				
				//*
				switch (colorCode) {
					case Player.ColorCode.RED:    tmpState1[index].red = pressure; break;
					case Player.ColorCode.GREEN:  tmpState1[index].green = pressure; break;
					case Player.ColorCode.BLUE:   tmpState1[index].blue = pressure; break;
					case Player.ColorCode.YELLOW: tmpState1[index].yellow = pressure; break;
				default:
				//case Player.ColorCode.MAGENTA:
				//case Player.ColorCode.WTF:
					tmpState1[index].red = pressure;
					tmpState1[index].green = 0;
					tmpState1[index].blue = pressure;
					tmpState1[index].yellow = 0;
					break;
				//	default: break;
				}//*/
	            
			}
	        else {
				switch (colorCode) {
					case Player.ColorCode.RED:    tmpState2[index].red = pressure; break;
					case Player.ColorCode.GREEN:  tmpState2[index].green = pressure; break;
					case Player.ColorCode.BLUE:   tmpState2[index].blue = pressure; break;
					case Player.ColorCode.YELLOW: tmpState2[index].yellow = pressure; break;
				default:
					//case Player.ColorCode.MAGENTA:
				//case Player.ColorCode.WTF:
					tmpState2[index].red = pressure;
					tmpState2[index].green = 0;
					tmpState2[index].blue = pressure;
					tmpState2[index].yellow = 0;
					break;
				//	default: break;
				//	default: break;
				}	            
			}
		}
		
	}
	
	// ------------------------------------------------------------------------
	public WavePixel GetPressure(Vector2 screenCoordinates)
	{
		WavePixel pressure = new WavePixel();
		pressure.red = pressure.blue = pressure.green = pressure.yellow = 0;

		// need to convert between real-space and grid-space
		Vector2 gridCoordinates = ConvertScreenCoordinatesToGridCoordinates(screenCoordinates);
		int grid_x = (int)(gridCoordinates.x);
		int grid_y = (int)(gridCoordinates.y);
		if (grid_x < 0 || grid_x >= WIDTH || grid_y < 0 || grid_y >= HEIGHT) return pressure;

		if(counter % 2 == 0)
            pressure = tmpState1[grid_y * WIDTH + grid_x];
        else
            pressure = tmpState2[grid_y * WIDTH + grid_x];
		return pressure;
	}
    
	// ------------------------------------------------------------------------
	/*
    void seedWorld()
    {  
        tmpState2[HEIGHT/2 * WIDTH + WIDTH /2].red = 1 << 15;
		tmpState2[HEIGHT/2 * WIDTH + WIDTH /2].green = 1 << 15;
		tmpState2[HEIGHT/2 * WIDTH + WIDTH /2].blue = 1 << 15;
		tmpState2[HEIGHT/2 * WIDTH + WIDTH /2].yellow = 1 << 15;//12;
    }
    //*/
    
	// ------------------------------------------------------------------------
    void processWater(WavePixel[] source, WavePixel[] dest)
    {
        for (var i = WIDTH; i < TOTAL_PIXELS - WIDTH; i++)
        {
			// blend in from adjacent cells
            dest[i].red = (((source[i-1].red + source[i+1].red + source[i-WIDTH].red + source[i+WIDTH].red) >> 1)) - dest[i].red;
			dest[i].green = (((source[i-1].green + source[i+1].green + source[i-WIDTH].green + source[i+WIDTH].green) >> 1)) - dest[i].green;
			dest[i].blue = (((source[i-1].blue + source[i+1].blue + source[i-WIDTH].blue + source[i+WIDTH].blue) >> 1)) - dest[i].blue;
			dest[i].yellow = (((source[i-1].yellow + source[i+1].yellow + source[i-WIDTH].yellow + source[i+WIDTH].yellow) >> 1)) - dest[i].yellow;
			// dampen
            //dest[i] -= (dest[i] >> 7);
			//dest[i] -= (dest[i] >> 4);
			int dampen_exponent = 4; //5;
			dest[i].red -= (dest[i].red >> dampen_exponent);
			dest[i].green -= (dest[i].green >> dampen_exponent);
			dest[i].blue -= (dest[i].blue >> dampen_exponent);
			dest[i].yellow -= (dest[i].yellow >> dampen_exponent);
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
    
	// ------------------------------------------------------------------------
    public void Update(){		
		// physical integration step
		//*
		counter += 1;

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
                int redValue = 127 + ((counter % 2 == 0) ? tmpState2[y * WIDTH + x].red : tmpState1[y * WIDTH + x].red) >> 4;
				int greenValue = 127 + ((counter % 2 == 0) ? tmpState2[y * WIDTH + x].green : tmpState1[y * WIDTH + x].green) >> 4;
				int blueValue = 127 + ((counter % 2 == 0) ? tmpState2[y * WIDTH + x].blue : tmpState1[y * WIDTH + x].blue) >> 4;
				int yellowValue = 127 + ((counter % 2 == 0) ? tmpState2[y * WIDTH + x].yellow : tmpState1[y * WIDTH + x].yellow) >> 4;
				
                //clamp the value to the valid ranges.
				redValue = Mathf.Clamp(redValue + yellowValue / 2, 0, 255);
				greenValue = Mathf.Clamp(greenValue + yellowValue / 2, 0, 255);
				blueValue = Mathf.Clamp(blueValue, 0, 255);				
				
                float redFloat = (float)redValue / 255.0f;
				float greenFloat = (float)greenValue / 255.0f;
				float blueFloat = (float)blueValue / 255.0f;
				
                data[y * ROW_STRIDE + x].r = redFloat;
                data[y * ROW_STRIDE + x].g = greenFloat;
                data[y * ROW_STRIDE + x].b = blueFloat;
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
