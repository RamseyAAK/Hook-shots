Shader "Custom/Quantum_rope"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _PointOneTwo("Point One", Vector) = (0.5, 0.5, 0.0, 0.0)
        _Length("Rope Length", Float) = 0.5
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _PointOneTwo;
            float _Length;
            float4 _BaseColor;

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD1;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionWS = TransformObjectToWorld(IN.positionOS);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float abc_length = (distance(IN.positionWS.xy, _PointOneTwo.xy) + distance(IN.positionWS.xy, _PointOneTwo.wz));
                // if (abc_length > _Length) {return float4(0.0,0.0,0.0,0.0);}
                float ac_length = distance(_PointOneTwo.xy, _PointOneTwo.zw);
                float4 color = _BaseColor; 
                color.w = (_Length - abc_length) / (_Length - ac_length);
                return color;
            }
            ENDHLSL
        }
    }
}
