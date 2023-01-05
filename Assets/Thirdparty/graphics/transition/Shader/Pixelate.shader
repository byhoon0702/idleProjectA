Shader "Screen Transition/Pixelate" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress ("Progress", Range(0, 1)) = 0
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"
       	float4 frag (v2f_img i) : COLOR
       	{
			float segmentProgress;
			if (_Progress < 0.5)
				segmentProgress = 1 - _Progress * 2;
			else
				segmentProgress = (_Progress - 0.5) * 2;
    
			float pixels = 5 + 1000 * segmentProgress * segmentProgress;
			float2 uv = round(i.uv * pixels) / pixels;
	
			float4 c1 = tex2D(_PrevTex, uv);
			float4 c2 = tex2D(_MainTex, uv);

			float f = saturate((_Progress - 0.4) / 0.2);
			return lerp(c1, c2, f);
       	}
	ENDCG
	SubShader {
		ZTest Off Cull Off ZWrite Off Blend Off
	  	Fog { Mode off }
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			ENDCG
		}
	}
	FallBack Off
}