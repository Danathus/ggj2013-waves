Shader "Custom/pulse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}		
		_PulseRadius ("_PulseRadius", Float) = 0.0
		_PulseRadiusSquared ("_PulseRadiusSquared", Float) = 0.0	
		_PulseColor ("_PulseColor", color) = (1.0, 1.0, 1.0, 1.0)
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	
	// vertex input: position, UV
	struct appdata {
	    half4 vertex : POSITION;	    
	    half4 texcoord : TEXCOORD0;
	};
	
	struct v2f {
	    half4 pos : SV_POSITION;	    
	    half4 uv : TEXCOORD0;
	    half4 worldPos : TEXCOORD1;
	};
	
	float _ElapsedTime;
	float _PulseRadius;	
	float _PulseRadiusSquared;		
	
	v2f vert (appdata v) {
	    v2f o;
	    
	    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);	  	    
	    o.uv = v.texcoord;
	    o.worldPos = v.vertex;	    
	    
	    return o;
	}	
	
	float4 _PulseColor;
	
	half4 frag(v2f i) : COLOR {    
		half4 color = half4(0.0, 0.0, 0.0, 0.0f);
		
		// Avoid executing sqrt per pixel
		float lengthSquared = dot(i.worldPos, i.worldPos);
		
		half dist = abs(lengthSquared - _PulseRadiusSquared);
		if (dist < 0.75) {
			color = _PulseColor;
			color.a *= 1.0 - dist / 0.75;
			color.a *= color.a;
			color.a *= color.a; 
		}		
		
		return color;
	}
	
	ENDCG
	
	Subshader { 
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	 Pass { 	 
		  Cull Back ZWrite Off Blend SrcAlpha OneMinusSrcAlpha
		  Fog { Mode off }      
	
	      CGPROGRAM
	      #pragma target 2.0
	      #pragma fragmentoption ARB_precision_hint_fastest
	      #pragma vertex vert
	      #pragma fragment frag
	      ENDCG
	  }
	}

}

