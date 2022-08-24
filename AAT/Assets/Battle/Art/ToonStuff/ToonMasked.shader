Shader "Toon/ToonMasked"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlossyTex("Glossy Texture", 2D) = "black" {}
        _DarkDotCutoff("Dark Cutoff", float) = -.2
        _LightDotCutoff("Light Cutoff", float) = .8
        _DarkValue("Dark Value", float) = .25
        _LightValue("Light Value", float) = 4
        _MidValue("Mid Value", float) = 1
        _Strength("Strength", float) = 1
        _Color("Color", COLOR) = (1, 1, 1, 1)
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", float) = 8
    }        

    SubShader
    {
        Tags {  "RenderQueue" = "AlphaTest" "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        
        Pass
        {
            ZTest [_ZTest]
            ZWrite Off
            ColorMask 0
            Cull Off
            
            Stencil 
            {
                Ref 2
                Comp Always
                Pass Replace
            }
        }
        
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float4 vertex : SV_POSITION;
                half3 worldNormal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _GlossyTex;
            float4 _GlossyTex_ST;
            float _DarkDotCutoff;
            float _LightDotCutoff;
            float _DarkValue;
            float _LightValue;
            float _MidValue;
            float _Strength;
            half4 _Color;
            
            half3 GlossyToon(float3 normal, Light light, half4 baseColor, half4 glossyColor)
            {
                half3 rgbFinal;
                float NdotL = dot(normalize(normal), normalize(light.direction));
                bool isGlossy = glossyColor.rgb == half3(1, 1, 1)? true : false;
                if (light.shadowAttenuation == 0)
                {
                    rgbFinal = _DarkValue * baseColor.rgb;
                }
                else if (isGlossy && NdotL > _LightDotCutoff) rgbFinal = _LightValue * glossyColor.rgb;
                else if (NdotL < _DarkDotCutoff) rgbFinal = _DarkValue * baseColor.rgb;
                else rgbFinal = _MidValue * baseColor.rgb;

                rgbFinal *= _Strength;

                return rgbFinal * _Color.rgb;
            }

            v2f vert (appdata v)
            {
                v2f o;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.positionWS = positionInputs.positionWS;
                o.vertex = positionInputs.positionCS;
                o.worldNormal = TransformObjectToWorldNormal(v.normal);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(i.positionWS));
                half4 baseCol = tex2D(_MainTex, i.uv);
                half4 glossyCol = tex2D(_GlossyTex, i.uv);
                baseCol.rgb = GlossyToon(i.worldNormal, mainLight, baseCol, glossyCol);
                return baseCol;
            }
            ENDHLSL
        }

        CGINCLUDE
        #include "UnityCG.cginc"
        struct v2fShadow {
            V2F_SHADOW_CASTER;
            UNITY_VERTEX_OUTPUT_STEREO
        };
    
        v2fShadow vertShadow( appdata_base v )
        {
            v2fShadow o;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
            TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
            return o;
        }
    
        float4 fragShadow( v2fShadow i ) : SV_Target
        {
            SHADOW_CASTER_FRAGMENT(i)
        }
        ENDCG
        
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
            CGPROGRAM
            #pragma vertex vertShadow
            #pragma fragment fragShadow
            #pragma target 2.0
            #pragma multi_compile_shadowcaster
            ENDCG

        }
    }
}
