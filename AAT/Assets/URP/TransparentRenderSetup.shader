Shader "Toon/TransparentRenderSetup"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", float) = 8
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent-30" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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
    }
}
