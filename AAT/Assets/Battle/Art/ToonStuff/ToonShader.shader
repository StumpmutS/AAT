Shader "Toon/ToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlossyTex("Glossy Texture", 2D) = "black" {}
        _DarkDotCutoff("Dark Cutoff", float) = -.12
        _LightDotCutoff("Light Cutoff", float) = .995
        _DarkValue("Dark Value", float) = -.45
        _LightValue("Light Value", float) = 6.78
        _MidValue("Mid Value", float) = 1.39
        _Strength("Strength", float) = .5
        _Brightness("Brightness", Range(0, 1)) = 1
        _Color("Color", COLOR) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
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
            float _Brightness;
            float4 _Color;

            half3 Toon(float3 normal, float3 lightDir)
            {
                float NdotL = dot(normalize(normal), normalize(lightDir));
                if (NdotL < _DarkDotCutoff) return _DarkValue;
                else return _MidValue;
            }
            
            half3 GlossyToon(float3 normal, float3 lightDir, fixed4 baseColor, fixed4 glossyColor)
            {
                half3 rgbFinal;
                float NdotL = dot(normalize(normal), normalize(lightDir));
                bool isGlossy = glossyColor.rgb == half3(1, 1, 1)? true : false;
                if (isGlossy && NdotL > _LightDotCutoff) rgbFinal = _LightValue * glossyColor.rgb;
                else if (NdotL < _DarkDotCutoff) rgbFinal = _DarkValue * baseColor.rgb ;
                else rgbFinal = _MidValue * baseColor.rgb;

                if (!isGlossy)
                {
                    rgbFinal += _Brightness;
                    rgbFinal *= _Strength;
                }
                return rgbFinal * _Color;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 baseCol = tex2D(_MainTex, i.uv);
                fixed4 glossyCol = tex2D(_GlossyTex, i.uv);
                baseCol.rgb = GlossyToon(i.worldNormal, _WorldSpaceLightPos0.xyz, baseCol, glossyCol);
                return baseCol;
            }
            ENDCG
        }
    }
}
