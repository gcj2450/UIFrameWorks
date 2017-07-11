// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shadertoy/Simple Circle" { 
	Properties{
		_Parameters ("Circle Parameters", Vector) = (0.5, 0.5, 10, 1) // Center: (x, y), Radius: z
		_CircleColor ("Circle Color", Color) = (1, 1, 1, 1)
		_BackgroundColor ("Background Color", Color) = (1, 1, 1, 1)
	}
	  
	CGINCLUDE    
	 	#include "UnityCG.cginc"   
  		#pragma target 3.0      

  		/*#define PI2 6.28318530718
  		#define pi 3.14159265358979
  		#define halfpi (pi * 0.5)
  		#define oneoverpi (1.0 / pi)*/
  		
  		float4 _Parameters;
  		float4 _CircleColor;
  		float4 _BackgroundColor;
  		
        struct v2f {    
            float4 pos : SV_POSITION;    
            float4 scrPos : TEXCOORD0;  
        };              
        
        v2f vert(appdata_base v) {  
        	v2f o;
        	o.pos = UnityObjectToClipPos (v.vertex);
            o.scrPos = ComputeScreenPos(o.pos);  
            return o;    
        }  
        
		float4 main(float2 fragCoord);
        
        fixed4 frag(v2f _iParam) : COLOR0 { 
			float2 fragCoord = ((_iParam.scrPos.xy / _iParam.scrPos.w)*_ScreenParams.xy);
        	return main(
				(_iParam.scrPos.xy / _iParam.scrPos.w)*_ScreenParams.xy
			);
        }  
        
			float4 Circle(float2 pos, float2 center, float radius, float3 color, float antialias) {
        	float d = length(pos - center) - radius;
        	float t = smoothstep(0, antialias, d);
        	return float4(color, 1.0 - t);
        }
        
		float4 main(float2 fragCoord) {
			float2 pos = fragCoord; // pos.x ~ (0, _ScreenParams.x), pos.y ~ (0, _ScreenParams.y)

			float4 layer1 = float4(_BackgroundColor.rgb, 1.0);
			float4 layer2 = Circle(pos, _Parameters.xy * _ScreenParams.xy, _Parameters.z, _CircleColor.rgb, _Parameters.w);
			
			return lerp(layer1, layer2, layer2.a);
		}

    ENDCG    
  
    SubShader { 		
        Pass {		
            CGPROGRAM    
  
            #pragma vertex vert    
            #pragma fragment frag    
            #pragma fragmentoption ARB_precision_hint_fastest     
  
            ENDCG    
        }    
    }     
    FallBack Off    
}
