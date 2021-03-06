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
	
	public static float WavePixelAmplitude(Color enemyColor, WavePixel pixel)
	{
		//return (pixel.red * enemyColor.r) + (pixel.green * enemyColor.g) + (pixel.blue * enemyColor.b) + (pixel.yellow + (enemyColor.r + enemyColor.g)/2);
		//
		WavePixel temp = new WavePixel();
		temp.red    = pixel.red;
		temp.blue   = pixel.blue;
		temp.green  = pixel.green;
		temp.yellow = pixel.yellow;
		temp = CrossPollinateIntensity(temp);
		
		return (temp.red * enemyColor.r) + (temp.green * enemyColor.g) + (temp.blue * enemyColor.b) + (temp.yellow + (enemyColor.r + enemyColor.g)/2);
		
		/*
		scaled.red    = (int)(pixel.red * enemyColor.r);
		scaled.blue   = (int)(pixel.blue  * enemyColor.b);
		scaled.green  = (int)(pixel.green * enemyColor.g);
		scaled.yellow = (int)((enemyColor.r + enemyColor.g)/2);
		//scaled = CrossPollinateIntensity(scaled);
		return WavePixelAmplitude(scaled);
		//return
		//	(pixel.red   * enemyColor.r) +
		//	(pixel.green * enemyColor.g) +
		//	(pixel.blue  * enemyColor.b) +
		//	(pixel.yellow + (enemyColor.r + enemyColor.g)/2);
		//*/
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
	//*
    void seedWorld()
    {
		return;
		int intensity = 1 << 11; // 11 is too much
        tmpState2[HEIGHT/2 * WIDTH + WIDTH /2].red    = intensity;
		tmpState2[HEIGHT/2 * WIDTH + WIDTH /2].green  = intensity;
		tmpState2[HEIGHT/2 * WIDTH + WIDTH /2].blue   = intensity;
		tmpState2[HEIGHT/2 * WIDTH + WIDTH /2].yellow = intensity;//12;
    }
    //*/
	
	static public WavePixel CrossPollinateIntensity(WavePixel input)
	{
		WavePixel output = input;
		int blue    = output.blue;
		int red     = output.red;
		int green   = output.green;
		int yellow  = output.yellow;
		int threshold  = 1 << 7;
		int threshold2 = 1 << 8;
		int threshold3 = 1 << 9;
		if (red > threshold && blue > threshold)
		{
			output.red  += blue >> 5; // 5
			output.blue += red  >> 5;
		}
		if (red > threshold && green > threshold)
		{
			output.red   += green >> 5; // 5
			output.green += red   >> 5;
		}
		if (red > threshold && yellow > threshold)
		{
			output.red    += yellow >> 5; // 5
			output.yellow += red    >> 5;
		}
		if (blue > threshold && green > threshold)
		{
			output.green  += blue >> 5; // 5
			output.blue   += green  >> 5;
		}
		if (blue > threshold && yellow > threshold)
		{
			output.yellow  += blue >> 5; // 5
			output.blue    += yellow  >> 5;
		}
		if (green > threshold && yellow > threshold)
		{
			output.green  += yellow >> 5; // 5
			output.yellow += green  >> 5;
		}
		//*/
		/*
		if (red > threshold2 && blue > threshold2 && green > threshold2)
		{
			output.red   += (blue >> 6) + (green >> 6); // 5
			output.blue  += (red  >> 6) + (green >> 6);
			output.green += (red  >> 6) + (blue  >> 6);
		}
		if (red > threshold2 && blue > threshold2 && yellow > threshold2)
		{
			output.red    += (blue >> 6) + (yellow >> 6); // 5
			output.blue   += (red  >> 6) + (yellow >> 6);
			output.yellow += (red  >> 6) + (blue   >> 6);
		}
		if (green > threshold2 && blue > threshold2 && yellow > threshold2)
		{
			output.green  += (blue   >> 6) + (yellow >> 6); // 5
			output.blue   += (green  >> 6) + (yellow >> 6);
			output.yellow += (green  >> 6) + (blue   >> 6);
		}
		if (green > threshold2 && red > threshold2 && yellow > threshold2)
		{
			output.green  += (red    >> 6) + (yellow >> 6); // 5
			output.red    += (green  >> 6) + (yellow >> 6);
			output.yellow += (green  >> 6) + (blue   >> 6);
		}
		//*/
		/*
		if (green > threshold3 && red > threshold3 && yellow > threshold3 && blue > threshold3)
		{
			output.green  += (red    >> 7) + (yellow >> 7) + (blue >> 7); // 5
			output.red    += (green  >> 7) + (yellow >> 7) + (blue >> 7);
			output.yellow += (green  >> 7) + (blue   >> 7) + (red  >> 7);
			output.blue   += (green  >> 7) + (yellow >> 7) + (red  >> 7);
		}
		//*/
		return output;
	}

	// ------------------------------------------------------------------------
    void processWater(WavePixel[] source, WavePixel[] dest)
    {
        for (var i = WIDTH; i < TOTAL_PIXELS - WIDTH; i++)
        {
			// blend in from adjacent cells
            dest[i].red    = (((source[i-1].red    + source[i+1].red    + source[i-WIDTH].red    + source[i+WIDTH].red)    >> 1)) - dest[i].red;
			dest[i].green  = (((source[i-1].green  + source[i+1].green  + source[i-WIDTH].green  + source[i+WIDTH].green)  >> 1)) - dest[i].green;
			dest[i].blue   = (((source[i-1].blue   + source[i+1].blue   + source[i-WIDTH].blue   + source[i+WIDTH].blue)   >> 1)) - dest[i].blue;
			dest[i].yellow = (((source[i-1].yellow + source[i+1].yellow + source[i-WIDTH].yellow + source[i+WIDTH].yellow) >> 1)) - dest[i].yellow;

			// experimentation -- let colors combine powers
			/*
			int blue    = dest[i].blue;
			int red     = dest[i].red;
			int green   = dest[i].green;
			int yellow  = dest[i].yellow;
			int threshold  = 1 << 7;
			int threshold2 = 1 << 8;
			int threshold3 = 1 << 9;
			if (red > threshold && blue > threshold)
			{
				dest[i].red  += blue >> 5; // 5
				dest[i].blue += red  >> 5;
			}
			if (red > threshold && green > threshold)
			{
				dest[i].red   += green >> 5; // 5
				dest[i].green += red   >> 5;
			}
			if (red > threshold && yellow > threshold)
			{
				dest[i].red    += yellow >> 5; // 5
				dest[i].yellow += red    >> 5;
			}
			if (blue > threshold && green > threshold)
			{
				dest[i].green  += blue >> 5; // 5
				dest[i].blue   += green  >> 5;
			}
			if (blue > threshold && yellow > threshold)
			{
				dest[i].yellow  += blue >> 5; // 5
				dest[i].blue    += yellow  >> 5;
			}
			if (green > threshold && yellow > threshold)
			{
				dest[i].green  += yellow >> 5; // 5
				dest[i].yellow += green  >> 5;
			}
			//*/
			/*
			if (red > threshold2 && blue > threshold2 && green > threshold2)
			{
				dest[i].red   += (blue >> 6) + (green >> 6); // 5
				dest[i].blue  += (red  >> 6) + (green >> 6);
				dest[i].green += (red  >> 6) + (blue  >> 6);
			}
			if (red > threshold2 && blue > threshold2 && yellow > threshold2)
			{
				dest[i].red    += (blue >> 6) + (yellow >> 6); // 5
				dest[i].blue   += (red  >> 6) + (yellow >> 6);
				dest[i].yellow += (red  >> 6) + (blue   >> 6);
			}
			if (green > threshold2 && blue > threshold2 && yellow > threshold2)
			{
				dest[i].green  += (blue   >> 6) + (yellow >> 6); // 5
				dest[i].blue   += (green  >> 6) + (yellow >> 6);
				dest[i].yellow += (green  >> 6) + (blue   >> 6);
			}
			if (green > threshold2 && red > threshold2 && yellow > threshold2)
			{
				dest[i].green  += (red    >> 6) + (yellow >> 6); // 5
				dest[i].red    += (green  >> 6) + (yellow >> 6);
				dest[i].yellow += (green  >> 6) + (blue   >> 6);
			}
			//*/
			/*
			if (green > threshold3 && red > threshold3 && yellow > threshold3 && blue > threshold3)
			{
				dest[i].green  += (red    >> 7) + (yellow >> 7) + (blue >> 7); // 5
				dest[i].red    += (green  >> 7) + (yellow >> 7) + (blue >> 7);
				dest[i].yellow += (green  >> 7) + (blue   >> 7) + (red  >> 7);
				dest[i].blue   += (green  >> 7) + (yellow >> 7) + (red  >> 7);
			}
			//*/
			//

			// dampen
            //dest[i] -= (dest[i] >> 7);
			//dest[i] -= (dest[i] >> 4);

			int dampen_exponent = 4; //5;
			/*
			// testing out: the more active components, the less we dampen
			int numcomponents =
				((red    > threshold) ? 1 : 0) +
				((blue   > threshold) ? 1 : 0) +
				((green  > threshold) ? 1 : 0) +
				((yellow > threshold) ? 1 : 0);
			dampen_exponent = 5 - numcomponents;
			//*/
			//

			dest[i].red    -= (dest[i].red    >> dampen_exponent);
			dest[i].green  -= (dest[i].green  >> dampen_exponent);
			dest[i].blue   -= (dest[i].blue   >> dampen_exponent);
			dest[i].yellow -= (dest[i].yellow >> dampen_exponent);
			
			// clamp to non-negative -- breaks shit!
			//dest[i].red    = Mathf.Max(dest[i].red,    0);
			//dest[i].green  = Mathf.Max(dest[i].green,  0);
			//dest[i].blue   = Mathf.Max(dest[i].blue,   0);
			//dest[i].yellow = Mathf.Max(dest[i].yellow, 0);

			// if the colors are too powerful, clamp them
			/*
			int tooPowerful = (1 << 16); // 13
			if (WavePixelAmplitude(dest[i]) >  tooPowerful ||
				WavePixelAmplitude(dest[i]) < -tooPowerful)
			{
				//dest[i].red    = Mathf.Min(dest[i].red,    tooPowerful/8);
				//dest[i].green  = Mathf.Min(dest[i].green,  tooPowerful/8);
				//dest[i].blue   = Mathf.Min(dest[i].blue,   tooPowerful/8);
				//dest[i].yellow = Mathf.Min(dest[i].yellow, tooPowerful/8);
				dest[i].red    = 0;//dest[i].red    >> 13; // 5, then 6
				dest[i].green  = 0;//dest[i].green  >> 13;
				dest[i].blue   = 0;//dest[i].blue   >> 13;
				dest[i].yellow = 0;//dest[i].yellow >> 13;
			}
			//*/
			
			// if radius is too far, dampen
			if (i%WIDTH == 0)
			{
				dest[i].red = dest[i].green = dest[i].blue = dest[i].yellow = 0;
			}
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
				//
				//set the cell color
				WavePixel curr = (counter % 2 == 0) ? tmpState2[y * WIDTH + x] : tmpState1[y * WIDTH + x];
				int redValue    = 127 + curr.red    >> 4;
				int greenValue  = 127 + curr.green  >> 4;
				int blueValue   = 127 + curr.blue   >> 4;
				int yellowValue = 127 + curr.yellow >> 4;

				//clamp the value to the valid ranges.
				redValue   = Mathf.Clamp(redValue   + yellowValue / 2, 0, 255);
				greenValue = Mathf.Clamp(greenValue + yellowValue / 2, 0, 255);
				blueValue  = Mathf.Clamp(blueValue,                    0, 255);
				
				// if the pulse is too weak to be effective, pale it out
				float amplitude = WaveField.WavePixelAmplitude(curr);
				if (amplitude > 256 && amplitude < Enemy.tooMuchPressure/2)
				{
					redValue   = (redValue   + 127)/2;
					greenValue = (greenValue + 127)/2;
					blueValue  = (blueValue  + 127)/2;
					//redValue = greenValue = blueValue = 255;
				}

				float redFloat   = (float)redValue   / 255.0f;
				float greenFloat = (float)greenValue / 255.0f;
				float blueFloat  = (float)blueValue  / 255.0f;

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
