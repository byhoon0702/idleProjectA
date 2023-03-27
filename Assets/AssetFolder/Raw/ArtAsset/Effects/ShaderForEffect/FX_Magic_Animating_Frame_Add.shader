// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "B_Project/FX/Magic_Animating_Frame_Add"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		_Columns("Columns", Int) = 0
		_Rows("Rows", Int) = 0
		_SpriteTexture("SpriteTexture", 2D) = "white" {}
		_PlayFrame("PlayFrame", Int) = 0
		_MainTex("MainTex", 2D) = "white" {}

		[HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Cull Off
		HLSLINCLUDE
		#pragma target 2.0
		
		#pragma prefer_hlslcc gles
		#pragma exclude_renderers d3d11_9x 

		ENDHLSL

		
		Pass
		{
			Name "Sprite Unlit"
            Tags { "LightMode"="Universal2D" }

			Blend SrcAlpha One, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			
			#define ASE_SRP_VERSION 999999

			
			#pragma vertex vert
			#pragma fragment frag

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS SHADERPASS_SPRITEUNLIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"

			

			sampler2D _SpriteTexture;
			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			int _Columns;
			int _Rows;
			int _PlayFrame;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 positionWS : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D( _AlphaTex ); SAMPLER( sampler_AlphaTex );
				float _EnableAlphaTexture;
			#endif

			float4 _RendererColor;

			
			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.vertex.xyz );

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.clipPos = vertexInput.positionCS;
				o.positionWS = vertexInput.positionWS;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles108 = (float)_Columns * (float)_Rows;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset108 = 1.0f / (float)_Columns;
				float fbrowsoffset108 = 1.0f / (float)_Rows;
				// Speed of animation
				float fbspeed108 = _Time[ 1 ] * 0.0;
				// UV Tiling (col and row offset)
				float2 fbtiling108 = float2(fbcolsoffset108, fbrowsoffset108);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex108 = round( fmod( fbspeed108 + (float)_PlayFrame, fbtotaltiles108) );
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
				
				float4 Color = tex2D( _SpriteTexture, fbuv108 );

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D( _AlphaTex, sampler_AlphaTex, IN.texCoord0.xy );
					Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture );
				#endif

				#if defined(DEBUG_DISPLAY)
				SurfaceData2D surfaceData;
				InitializeSurfaceData(Color.rgb, Color.a, surfaceData);
				InputData2D inputData;
				InitializeInputData(IN.positionWS.xy, half2(IN.texCoord0.xy), inputData);
				half4 debugColor = 0;

				SETUP_DEBUG_DATA_2D(inputData, IN.positionWS);

				if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
				{
					return debugColor;
				}
				#endif
				
				Color *= IN.color * _RendererColor;
				return Color;
			}

			ENDHLSL
		}
		
		Pass
		{
			
			Name "Sprite Unlit Forward"
            Tags { "LightMode"="UniversalForward" }

			Blend SrcAlpha One, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			
			#define ASE_SRP_VERSION 999999

			
			#pragma vertex vert
			#pragma fragment frag

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS SHADERPASS_SPRITEFORWARD

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"

			

			sampler2D _SpriteTexture;
			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			int _Columns;
			int _Rows;
			int _PlayFrame;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 positionWS : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D( _AlphaTex ); SAMPLER( sampler_AlphaTex );
				float _EnableAlphaTexture;
			#endif

			float4 _RendererColor;

			
			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.vertex.xyz );

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.clipPos = vertexInput.positionCS;
				o.positionWS = vertexInput.positionWS;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles108 = (float)_Columns * (float)_Rows;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset108 = 1.0f / (float)_Columns;
				float fbrowsoffset108 = 1.0f / (float)_Rows;
				// Speed of animation
				float fbspeed108 = _Time[ 1 ] * 0.0;
				// UV Tiling (col and row offset)
				float2 fbtiling108 = float2(fbcolsoffset108, fbrowsoffset108);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex108 = round( fmod( fbspeed108 + (float)_PlayFrame, fbtotaltiles108) );
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
				
				float4 Color = tex2D( _SpriteTexture, fbuv108 );

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D( _AlphaTex, sampler_AlphaTex, IN.texCoord0.xy );
					Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture );
				#endif

				
				#if defined(DEBUG_DISPLAY)
				SurfaceData2D surfaceData;
				InitializeSurfaceData(Color.rgb, Color.a, surfaceData);
				InputData2D inputData;
				InitializeInputData(IN.positionWS.xy, half2(IN.texCoord0.xy), inputData);
				half4 debugColor = 0;

				SETUP_DEBUG_DATA_2D(inputData, IN.positionWS);

				if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
				{
					return debugColor;
				}
				#endif

				Color *= IN.color * _RendererColor;
				return Color;
			}

			ENDHLSL
		}
		
        Pass
        {
			
            Name "SceneSelectionPass"
            Tags { "LightMode"="SceneSelectionPass" }
        
            Cull Off
        
            HLSLPROGRAM
        
            #define ASE_SRP_VERSION 999999

        
            #pragma target 2.0
			#pragma exclude_renderers d3d11_9x 
			#pragma vertex vert
			#pragma fragment frag
        
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS SHADERPASS_DEPTHONLY
			#define SCENESELECTIONPASS 1
        
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
			

			sampler2D _SpriteTexture;
			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			int _Columns;
			int _Rows;
			int _PlayFrame;
			CBUFFER_END


            struct VertexInput
			{
				float3 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
        
            
            int _ObjectId;
            int _PassValue;
            
			
			VertexOutput vert(VertexInput v )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
				float3 positionWS = TransformObjectToWorld(v.vertex);
				o.clipPos = TransformWorldToHClip(positionWS);
		
				return o;
			}

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				float2 uv_MainTex = IN.ase_texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles108 = (float)_Columns * (float)_Rows;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset108 = 1.0f / (float)_Columns;
				float fbrowsoffset108 = 1.0f / (float)_Rows;
				// Speed of animation
				float fbspeed108 = _Time[ 1 ] * 0.0;
				// UV Tiling (col and row offset)
				float2 fbtiling108 = float2(fbcolsoffset108, fbrowsoffset108);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex108 = round( fmod( fbspeed108 + (float)_PlayFrame, fbtotaltiles108) );
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
				
				float4 Color = tex2D( _SpriteTexture, fbuv108 );

				half4 outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				return outColor;
			}

            ENDHLSL
        }

		
        Pass
        {
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }
        
            Cull Back
        
            HLSLPROGRAM
        
            #define ASE_SRP_VERSION 999999

        
            #pragma target 2.0
			#pragma exclude_renderers d3d11_9x 
			#pragma vertex vert
			#pragma fragment frag
        
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS SHADERPASS_DEPTHONLY
			#define SCENEPICKINGPASS 1
        
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        	

			sampler2D _SpriteTexture;
			sampler2D _MainTex;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			int _Columns;
			int _Rows;
			int _PlayFrame;
			CBUFFER_END


            struct VertexInput
			{
				float3 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
        
            float4 _SelectionID;
        
			
			VertexOutput vert(VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
				float3 positionWS = TransformObjectToWorld(v.vertex);
				o.clipPos = TransformWorldToHClip(positionWS);
		
				return o;
			}

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				float2 uv_MainTex = IN.ase_texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles108 = (float)_Columns * (float)_Rows;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset108 = 1.0f / (float)_Columns;
				float fbrowsoffset108 = 1.0f / (float)_Rows;
				// Speed of animation
				float fbspeed108 = _Time[ 1 ] * 0.0;
				// UV Tiling (col and row offset)
				float2 fbtiling108 = float2(fbcolsoffset108, fbrowsoffset108);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex108 = round( fmod( fbspeed108 + (float)_PlayFrame, fbtotaltiles108) );
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
				
				float4 Color = tex2D( _SpriteTexture, fbuv108 );
				half4 outColor = _SelectionID;
				return outColor;
			}

            ENDHLSL
        }
		
	}
	CustomEditor "ASEMaterialInspector"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18935
2119;18;1511;992;1830.442;704.6132;1.3;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;46;-1386.217,-18.72455;Inherit;True;Property;_MainTex;MainTex;4;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-1104.6,-96.30353;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;36;-1031.897,105.5961;Inherit;False;Property;_Rows;Rows;1;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;35;-1028.597,24.89635;Inherit;False;Property;_Columns;Columns;0;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;78;-1040.052,186.4014;Inherit;False;Property;_PlayFrame;PlayFrame;3;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.TexturePropertyNode;42;-1368.774,-234.3322;Inherit;True;Property;_SpriteTexture;SpriteTexture;2;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;108;-846.345,-48.21285;Inherit;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;4;False;2;FLOAT;2;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;44;-623.8992,-158.8037;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;68;-290.4791,-143.2857;Float;False;True;-1;2;ASEMaterialInspector;0;14;B_Project/FX/Magic_Animating_Frame_Add;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit;0;0;Sprite Unlit;4;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;True;True;8;5;False;-1;1;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;3;Vertex Position;1;0;Debug Display;0;0;External Alpha;0;0;0;4;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;71;-290.4791,-143.2857;Float;False;False;-1;2;ASEMaterialInspector;0;13;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;ScenePickingPass;0;3;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;70;-290.4791,-143.2857;Float;False;False;-1;2;ASEMaterialInspector;0;13;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;SceneSelectionPass;0;2;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;69;-290.4791,-143.2857;Float;False;False;-1;2;ASEMaterialInspector;0;13;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit Forward;0;1;Sprite Unlit Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;8;5;False;-1;1;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;39;2;46;0
WireConnection;108;0;39;0
WireConnection;108;1;35;0
WireConnection;108;2;36;0
WireConnection;108;4;78;0
WireConnection;44;0;42;0
WireConnection;44;1;108;0
WireConnection;68;1;44;0
ASEEND*/
//CHKSM=13BCAFDDE61FA9968B84EF2DF298C1500619241A