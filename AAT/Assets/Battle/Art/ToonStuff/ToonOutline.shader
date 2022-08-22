Shader "Toon/ToonOutline"
{
    Properties
    {
        _Color("Color", COLOR) = (0, 0, 0, 1)
        _OutlineWidth("OutlineWidth", float) = 0
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent+20" "RenderType" = "Transparent" }
        
        Pass
        {
            Tags { "LightMode" = "UniversalForward" }
            
            ZTest Always
            ZWrite Off
            
            Stencil
            {
                Ref 1
                Comp NotEqual
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            half4 _Color;
            float _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                float3 normal = normalize(v.normal);
                v.vertex -= float4(normal * _OutlineWidth, 0);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            half4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}
