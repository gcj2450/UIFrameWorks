// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shadertoy/Simple Line" { 
	Properties{
		_CircleRadius ("Circle Radius", float) = 5
		_CircleColor ("Circle Color", Color) = (1, 1, 1, 1)
		_LineWidth ("Line Width", float) = 5
		_LineColor ("Line Color", Color) = (1, 1, 1, 1)
		_Antialias ("Antialias Factor", float) = 3
		_BackgroundColor ("Background Color", Color) = (1, 1, 1, 1)
	}
	  
	CGINCLUDE    
	 	#include "UnityCG.cginc"   
  		#pragma target 3.0   
  		#pragma glsl   

  		float _CircleRadius;
  		float4 _CircleColor;
  		float _LineWidth;
  		float4 _LineColor;
  		float _Antialias;
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
			float2 fragCoord = (_iParam.scrPos.xy / _iParam.scrPos.w)*_ScreenParams.xy;
        	return main((_iParam.scrPos.xy / _iParam.scrPos.w)*_ScreenParams.xy);
        }  
        
			float4 Line(float2 pos, float2 point1, float2 point2, float width, float3 color, float antialias) {
//        	float k = (point1.y - point2.y)/(point1.x - point2.x);
//    		float b = point1.y - k * point1.x;
//    		
//    		float d = abs(k * pos.x - pos.y + b) / sqrt(k * k + 1);
//    		float t = smoothstep(width/2.0, width/2.0 + antialias, d);
    		
			float2 dir0 = point2 - point1;
			float2 dir1 = pos - point1;
			float2 dir2 = dir0 * dot(dir0, dir1)/dot(dir0, dir0);
			float2 dir3 = dir1 - dir2;
			float d = length(dir3);
			float t = smoothstep(width/2.0, width/2.0 + antialias, d);
    		
        	return float4(color, 1.0 - t);
        }
        
		float4 Circle(float2 pos, float2 center, float radius, float3 color, float antialias) {
        	float d = length(pos - center) - radius;
        	float t = smoothstep(0, antialias, d);
        	return float4(color, 1.0 - t);
        }
        
		float4 main(float2 fragCoord) {

			float2 point1 = float2(0.5, 0.5) * _ScreenParams.xy;
			float2 point2 = float2(0.8, 0.8) * _ScreenParams.xy;
			
			float4 layer1 = float4(_BackgroundColor.rgb, 1.0);
			float4 layer2 = Line(fragCoord, point1, point2, _LineWidth, _LineColor.rgb, _Antialias);
			float4 layer3 =  Circle(fragCoord, point1, _CircleRadius, _CircleColor.rgb, _Antialias);
			float4 layer4 =  Circle(fragCoord, point2, _CircleRadius, _CircleColor.rgb, _Antialias);
			
			float4 fragColor = lerp(layer1, layer2, layer2.a);
			fragColor = lerp(fragColor, layer3, layer3.a);
			fragColor = lerp(fragColor, layer4, layer4.a);
			
			return fragColor;
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
