﻿Shader "FlatKit/GradientSkybox" {

    Properties {
        _Color2 ("Top Color", Color) = (0.97, 0.67, 0.51, 0)
        _Color1 ("Bottom Color", Color) = (0, 0.7, 0.74, 0)

        [Space]
        _Intensity ("Intensity", Range (0, 2)) = 1.0
        _Exponent ("Exponent", Range (0, 3)) = 1.0

        [Space]
        _DirectionYaw ("Direction X angle", Range (0, 180)) = 0
        _DirectionPitch ("Direction Y angle", Range (0, 180)) = 0

        [Space]
        _NoiseIntensity ("Noise Intensity", Range (0, 0.25)) = 0.0

        [HideInInspector]
        _Direction ("Direction", Vector) = (0, 1, 0, 0)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    struct appdata {
        float4 position : POSITION;
        float3 texcoord : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct v2f {
        float4 position : SV_POSITION;
        float3 texcoord : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    half4 _Color1;
    half4 _Color2;
    half3 _Direction;
    half _Intensity;
    half _Exponent;
    half _NoiseIntensity;

    v2f vert (appdata v) {
        v2f o;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
        o.position = UnityObjectToClipPos(v.position);
        o.texcoord = v.texcoord;
        return o;
    }

    fixed4 frag (v2f i) : COLOR {
        const half d = dot(normalize(i.texcoord), _Direction) * 0.5f + 0.5f;
        float t = pow(d, _Exponent);
        t += frac(sin(dot(t, float4(12.9898, 78.233, 45.164, 94.673))) * 43758.5453) * _NoiseIntensity;
        return lerp(_Color1, _Color2, t) * _Intensity;
    }

    ENDCG

    SubShader {
        Tags { "RenderType"="Background" "Queue"="Background" }

        Pass {
            ZWrite Off
            Cull Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }

    CustomEditor "GradientSkyboxEditor"
}
