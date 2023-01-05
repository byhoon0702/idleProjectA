Shader "SKYEffect/UI/Add" {
    Properties{
        _MainTex("Particle Texture", 2D) = "white" {}
    }

        Category{
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
            Blend One One
            Cull Off Lighting Off ZWrite Off Fog { Color(1,1,1,1) }

            BindChannels {
                Bind "Color", color
                Bind "Vertex", vertex
                Bind "TexCoord", texcoord
            }

            SubShader {
                Pass {
                    SetTexture[_MainTex] {
                        combine texture * primary
                    }
                    SetTexture[_MainTex] {
                        constantColor(0,0,0,1)
                        combine previous lerp(previous) constant
                        
                    }
                }
            }
    }
}