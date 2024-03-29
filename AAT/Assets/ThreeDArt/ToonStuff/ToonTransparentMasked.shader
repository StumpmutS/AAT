Shader "Toon/ToonTransparentMasked"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", COLOR) = (0, 0, 0, .5)
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", float) = 8
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            ZWrite On
            ColorMask 0
        }
        
        Pass
        {
            ZWrite Off
            ZTest [_ZTest]
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
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _Color;
            }
            ENDCG
        }
    }
}
