Shader "Screen Transition/Advertising Board" {
	Properties {
		[HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector] _PrevTex ("Previous Scene", 2D) = "white" {}
		_Progress   ("Progress", Range(0, 1)) = 0
		_Width      ("Width", Float) = 0.5
		_Shadow     ("Shadow", Range(0, 2)) = 1
		_SpaceColor ("Space Color", Color) = (0, 0, 0, 0)
		_ScaleY     ("ScaleY", Range(0.5, 1)) = 0
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "ScreenTransition.cginc"

		float _Width, _Shadow, _ScaleY;
		float4 _SpaceColor;
		
       	float4 frag (v2f_img i) : COLOR
       	{
			fixed posfloor = floor(_Progress / _Width) * _Width;
			float posTypeA = (_Progress - posfloor) / _Width;
		
			float posmod = fmod(i.uv.x, _Width);
			fixed2 uvA = fixed2(i.uv.x + (posmod - _Width) * _Progress / (1.0 - _Progress), i.uv.y);
			fixed2 uvB = fixed2(i.uv.x + (1.0 - _Progress) * posmod / _Progress, i.uv.y);

			_ScaleY += abs(0.5 - _Progress) * 2.0 * (1.0 - _ScaleY);
			
			fixed _ScaleYRight = _ScaleY + (_Width - posmod) * (1.0 - _ScaleY) / (_Width * (1.0 - _Progress));
			uvA.y = 0.5 + (uvA.y - 0.5) / _ScaleYRight;
			
			fixed _ScaleYLeft = _ScaleY + posmod * (1.0 - _ScaleY) / (_Width * _Progress);
			uvB.y = 0.5 + (uvB.y - 0.5) / _ScaleYLeft;

			fixed sr = 0.5 + (i.uv.y - 0.5) / _ScaleYRight;
			fixed sl = 0.5 + (i.uv.y - 0.5) / _ScaleYLeft;
			if (sl <= 0 || sl >= 1 || sr <= 0 || sr >= 1)
				return _SpaceColor;

			float4 c1 = tex2D(_PrevTex, uvA);
			float4 c2 = tex2D(_MainTex, uvB);

			fixed shadowOnTexB = _Shadow * abs(_Progress - 1.0);
			shadowOnTexB *= (_Width * _Progress - posmod) / (_Width * _Progress);
			c2.rgb *= 1.0 - shadowOnTexB;

			float4 c;
			if (posmod / _Width > _Progress)
				c = c1;
			else
				c = c2;
			return c;
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