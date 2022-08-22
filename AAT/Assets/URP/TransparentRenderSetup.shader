Shader "Toon/TransparentRenderSetup"
{
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            ZWrite On
            Colormask 0
        }
    }
}
