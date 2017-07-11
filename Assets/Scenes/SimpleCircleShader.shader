// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shadertoy/Simple Line" {
	Properties{
		_CircleRadius("Circle Radius", float) = 5
		_CircleColor("Circle Color", Color) = (1, 1, 1, 1)
		_Antialias("Antialias Factor", float) = 3
		_BackgroundColor("Background Color", Color) = (1, 1, 1, 1)
	}

		CGINCLUDE
#include "UnityCG.cginc"   
#pragma target 3.0   
#pragma glsl   

		float _CircleRadius;
	float4 _CircleColor;
	float _Antialias;
	float4 _BackgroundColor;

	struct v2f {
		float4 pos : SV_POSITION;
		float4 scrPos : TEXCOORD0;
	};

	v2f vert(appdata_base v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.scrPos = ComputeScreenPos(o.pos);
		return o;
	}

	float4 main(float2 fragCoord);

	fixed4 frag(v2f _iParam) : COLOR0{
		float2 fragCoord = (_iParam.scrPos.xy / _iParam.scrPos.w)*_ScreenParams.xy;
		return main((_iParam.scrPos.xy / _iParam.scrPos.w)*_ScreenParams.xy);
	}

	float4 Circle(float2 pos, float2 center, float radius, float3 color, float antialias) {
		float d = length(pos - center) - radius;
		float t = smoothstep(0, antialias, d);
		return float4(color, 1.0 - t);
	}

	float4 main(float2 fragCoord) {

		float2 point1 = float2(0.5, 0.5) * _ScreenParams.xy;

		float4 color1 = float4(_BackgroundColor.rgb, 1.0);

		float d = length(fragCoord - point1) - _CircleRadius;
		float t = smoothstep(0, _Antialias, d);
		float4 color2 =float4(_CircleColor.rgb, 1.0 - t);

		//float4 color2 = Circle(fragCoord, point1, _CircleRadius, _CircleColor.rgb, _Antialias);

		float4 fragColor = lerp(color1, color2, color2.a);
		fragColor = lerp(fragColor, color2, color2.a);

		return fragColor;
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
