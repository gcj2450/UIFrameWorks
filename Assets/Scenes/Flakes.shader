// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "shadertoy/Flakes" {  // https://www.shadertoy.com/view/4d2Xzc  
	Properties{
		iMouse("Mouse Pos", Vector) = (100,100,0,0)
		iChannel0("iChannel0", 2D) = "white" {}
	iChannelResolution0("iChannelResolution0", Vector) = (100,100,0,0)
	}

		CGINCLUDE
#include "UnityCG.cginc"     
#pragma target 3.0        
#pragma glsl  

#define vec2 float2  
#define vec3 float3  
#define vec4 float4  
#define mat2 float2x2  
#define iGlobalTime _Time.y  
#define mod fmod  
#define mix lerp  
#define atan atan2  
#define fract frac   
#define texture2D tex2D  
		// 屏幕的尺寸  
#define iResolution _ScreenParams  
		// 屏幕中的坐标，以pixel为单位  
#define gl_FragCoord ((_iParam.srcPos.xy/_iParam.srcPos.w)*_ScreenParams.xy)   

#define PI2 6.28318530718  
#define pi 3.14159265358979  
#define halfpi (pi * 0.5)  
#define oneoverpi (1.0 / pi)  

		fixed4 iMouse;
	sampler2D iChannel0;
	fixed4 iChannelResolution0;

	struct v2f {
		float4 pos : SV_POSITION;
		float4 srcPos : TEXCOORD0;
	};

	//   precision highp float;  
	v2f vert(appdata_base v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.srcPos = ComputeScreenPos(o.pos);
		return o;
	}

	vec4 main(v2f _iParam);

	fixed4 frag(v2f _iParam) : COLOR0{
		return main(_iParam);
	}


		vec4 main(v2f _iParam) {
		vec2 p = gl_FragCoord.xy / iResolution.xy;
		vec3  col = vec3(0, 0, 0);
		float dd = 150;
		for (int i = 0; i<dd; i++)
		{
			float an = 6.2831*float(i) / dd;
			vec2  of = vec2(cos(an), sin(an)) * (1.0 + 0.6*cos(7.0*an + iGlobalTime)) + vec2(0.0, iGlobalTime);
			col = max(col, texture2D(iChannel0, p + 20 * of / iResolution.xy).xyz);
			col = max(col, texture2D(iChannel0, p + 5.0*of / iResolution.xy).xyz);
		}
		col = pow(col, vec3(1.0, 2.0, 3.0)) * pow(4.0*p.y*(1.0 - p.y), 0.2);

		return vec4(col, 1.0);
	}

	ENDCG

		SubShader{
		Pass{
		CGPROGRAM

#pragma vertex vert      
#pragma fragment frag      
#pragma fragmentoption ARB_precision_hint_fastest       

		ENDCG
	}
	}
		FallBack Off
}