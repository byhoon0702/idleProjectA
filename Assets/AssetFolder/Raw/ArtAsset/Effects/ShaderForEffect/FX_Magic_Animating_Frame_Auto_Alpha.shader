// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "B_Project/FX/Magic_Animating_Frame_Auto_Alpha"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_Columns("Columns", Int) = 0
		_Rows("Rows", Int) = 0
		_Speed("Speed", Int) = 10
		_SpriteTexture("SpriteTexture", 2D) = "white" {}
		_MainTex("MainTex", 2D) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#include "UnityShaderVariables.cginc"

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform sampler2D _SpriteTexture;
			uniform float4 _MainTex_ST;
			uniform int _Columns;
			uniform int _Rows;
			uniform int _Speed;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles108 = (float)_Columns * (float)_Rows;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset108 = 1.0f / (float)_Columns;
				float fbrowsoffset108 = 1.0f / (float)_Rows;
				// Speed of animation
				float fbspeed108 = _Time.y * (float)_Speed;
				// UV Tiling (col and row offset)
				float2 fbtiling108 = float2(fbcolsoffset108, fbrowsoffset108);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex108 = round( fmod( fbspeed108 + 0.0, fbtotaltiles108) );
				fbcurrenttileindex108 += ( fbcurrenttileindex108 < 0) ? fbtotaltiles108 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox108 = round ( fmod ( fbcurrenttileindex108, (float)_Columns ) );
				// Multiply Offset X by coloffset
				float fboffsetx108 = fblinearindextox108 * fbcolsoffset108;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy108 = round( fmod( ( fbcurrenttileindex108 - fblinearindextox108 ) / (float)_Columns, (float)_Rows ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy108 = (int)((float)_Rows-1) - fblinearindextoy108;
				// Multiply Offset Y by rowoffset
				float fboffsety108 = fblinearindextoy108 * fbrowsoffset108;
				// UV Offset
				float2 fboffset108 = float2(fboffsetx108, fboffsety108);
				// Flipbook UV
				half2 fbuv108 = uv_MainTex * fbtiling108 + fboffset108;
				// *** END Flipbook UV Animation vars ***
				
				half4 color = tex2D( _SpriteTexture, fbuv108 );
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18935
2140;23;1511;992;1768.719;598.465;1.332085;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;46;-1386.217,-18.72455;Inherit;True;Property;_MainTex;MainTex;4;0;Create;True;0;0;0;False;0;False;None;9bd75d5c162054b48aafc8a6a5869e62;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-1104.6,-96.30353;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;35;-1028.597,24.89635;Inherit;False;Property;_Columns;Columns;0;0;Create;True;0;0;0;False;0;False;0;4;False;0;1;INT;0
Node;AmplifyShaderEditor.TimeNode;110;-1344.245,279.4858;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;36;-1031.897,105.5961;Inherit;False;Property;_Rows;Rows;1;0;Create;True;0;0;0;False;0;False;0;4;False;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;111;-1053.045,201.4859;Inherit;False;Property;_Speed;Speed;2;0;Create;True;0;0;0;False;0;False;10;35;False;0;1;INT;0
Node;AmplifyShaderEditor.TexturePropertyNode;42;-1368.774,-234.3322;Inherit;True;Property;_SpriteTexture;SpriteTexture;3;0;Create;True;0;0;0;False;0;False;None;9bd75d5c162054b48aafc8a6a5869e62;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;108;-846.345,-48.21285;Inherit;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;4;False;2;FLOAT;2;False;3;FLOAT;0.1;False;4;FLOAT;0;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;44;-623.8992,-158.8037;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;127;-290.4791,-143.2857;Float;False;True;-1;2;ASEMaterialInspector;0;6;B_Project/FX/Magic_Animating_Frame_Auto_Alpha;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;True;0;True;-9;False;False;False;False;False;False;False;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;39;2;46;0
WireConnection;108;0;39;0
WireConnection;108;1;35;0
WireConnection;108;2;36;0
WireConnection;108;3;111;0
WireConnection;108;5;110;2
WireConnection;44;0;42;0
WireConnection;44;1;108;0
WireConnection;127;0;44;0
ASEEND*/
//CHKSM=9DE541AE3A04E8EFD85BD58D0011D3C3E342B300