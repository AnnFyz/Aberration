Shader "Unlit/SpawnShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
        _Dissolve("Dissolve", Range(0, 1.1)) = 0.5
        _OutlineThickness("OutlineThickness", Range(0, 1)) = 0.1
        [HDR]_Color("Color", Color) = (2, 1.657266, 0.235849, 1)
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue" = "Transparent"
            "DisableBatching" = "False"
            "ShaderGraphShader" = "true"
            "ShaderGraphTargetId" = "UniversalUnlitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
            // LightMode: <None>
        }

        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag

        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        // GraphKeywords: <None>

        // Defines

        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1


        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

        // Includes
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float3 positionWS : INTERP1;
             float3 normalWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

        PackedVaryings PackVaryings(Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

        Varyings UnpackVaryings(PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }


        // --------------------------------------------------
        // Graph

        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float _Dissolve;
        float _OutlineThickness;
        float4 _Color;
        CBUFFER_END


            // Object and Global properties
            SAMPLER(SamplerState_Linear_Repeat);
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // Graph Includes
            // GraphIncludes: <None>

            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif

            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif

            // Graph Functions

            void Unity_Preview_float(float In, out float Out)
            {
                Out = In;
            }

            void Unity_OneMinus_float(float In, out float Out)
            {
                Out = 1 - In;
            }

            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }

            void Unity_Step_float(float Edge, float In, out float Out)
            {
                Out = step(Edge, In);
            }

            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }

            void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }

            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
            }

            void Unity_Multiply_float_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            // Custom interpolators pre vertex
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

            // Graph Vertex
            struct VertexDescription
            {
                float3 Position;
                float3 Normal;
                float3 Tangent;
            };

            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                description.Position = IN.ObjectSpacePosition;
                description.Normal = IN.ObjectSpaceNormal;
                description.Tangent = IN.ObjectSpaceTangent;
                return description;
            }

            // Custom interpolators, pre surface
            #ifdef FEATURES_GRAPH_VERTEX
            Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
            {
            return output;
            }
            #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
            #endif

            // Graph Pixel
            struct SurfaceDescription
            {
                float3 BaseColor;
                float Alpha;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _Property_613ae99900434d1798e9eb71bc71b308_Out_0_Vector4 = IsGammaSpace() ? LinearToSRGB(_Color) : _Color;
                float4 _UV_446aac7876ea4b4e9759a53f747dbfbd_Out_0_Vector4 = IN.uv0;
                float _Split_efa31210861b43438e2c6e6a1cc87de9_R_1_Float = _UV_446aac7876ea4b4e9759a53f747dbfbd_Out_0_Vector4[0];
                float _Split_efa31210861b43438e2c6e6a1cc87de9_G_2_Float = _UV_446aac7876ea4b4e9759a53f747dbfbd_Out_0_Vector4[1];
                float _Split_efa31210861b43438e2c6e6a1cc87de9_B_3_Float = _UV_446aac7876ea4b4e9759a53f747dbfbd_Out_0_Vector4[2];
                float _Split_efa31210861b43438e2c6e6a1cc87de9_A_4_Float = _UV_446aac7876ea4b4e9759a53f747dbfbd_Out_0_Vector4[3];
                float _Preview_bce5f825f2904de78063e40f1c278ec3_Out_1_Float;
                Unity_Preview_float(_Split_efa31210861b43438e2c6e6a1cc87de9_R_1_Float, _Preview_bce5f825f2904de78063e40f1c278ec3_Out_1_Float);
                float _Property_ab9c408e195e410abfb3caeb785a6a7a_Out_0_Float = _Dissolve;
                float _OneMinus_4cb27d3b3f334737a6a6281dd218700f_Out_1_Float;
                Unity_OneMinus_float(_Property_ab9c408e195e410abfb3caeb785a6a7a_Out_0_Float, _OneMinus_4cb27d3b3f334737a6a6281dd218700f_Out_1_Float);
                float _Property_8180de1a142f4463b1d7c6de227dccdc_Out_0_Float = _OutlineThickness;
                float _Add_768fc9a6f57a457da8cf58f81401f6fc_Out_2_Float;
                Unity_Add_float(_OneMinus_4cb27d3b3f334737a6a6281dd218700f_Out_1_Float, _Property_8180de1a142f4463b1d7c6de227dccdc_Out_0_Float, _Add_768fc9a6f57a457da8cf58f81401f6fc_Out_2_Float);
                float _Step_606235734f5144748b9be4af90aae761_Out_2_Float;
                Unity_Step_float(_Preview_bce5f825f2904de78063e40f1c278ec3_Out_1_Float, _Add_768fc9a6f57a457da8cf58f81401f6fc_Out_2_Float, _Step_606235734f5144748b9be4af90aae761_Out_2_Float);
                float _Step_193c967095834c9a81d986e05dc6f5ea_Out_2_Float;
                Unity_Step_float(_Preview_bce5f825f2904de78063e40f1c278ec3_Out_1_Float, _OneMinus_4cb27d3b3f334737a6a6281dd218700f_Out_1_Float, _Step_193c967095834c9a81d986e05dc6f5ea_Out_2_Float);
                float _Subtract_c809b5a5a07e48a88f1a9a4687976005_Out_2_Float;
                Unity_Subtract_float(_Step_606235734f5144748b9be4af90aae761_Out_2_Float, _Step_193c967095834c9a81d986e05dc6f5ea_Out_2_Float, _Subtract_c809b5a5a07e48a88f1a9a4687976005_Out_2_Float);
                float4 _Multiply_41fa1180021e4266a254dbd00cb37236_Out_2_Vector4;
                Unity_Multiply_float4_float4(_Property_613ae99900434d1798e9eb71bc71b308_Out_0_Vector4, (_Subtract_c809b5a5a07e48a88f1a9a4687976005_Out_2_Float.xxxx), _Multiply_41fa1180021e4266a254dbd00cb37236_Out_2_Vector4);
                UnityTexture2D _Property_9219f559a4324fa2a358d2cec0e3388f_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
                float4 _SampleTexture2D_ae0835b2329a445495d8eee27f934e69_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_9219f559a4324fa2a358d2cec0e3388f_Out_0_Texture2D.tex, _Property_9219f559a4324fa2a358d2cec0e3388f_Out_0_Texture2D.samplerstate, _Property_9219f559a4324fa2a358d2cec0e3388f_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy));
                float _SampleTexture2D_ae0835b2329a445495d8eee27f934e69_R_4_Float = _SampleTexture2D_ae0835b2329a445495d8eee27f934e69_RGBA_0_Vector4.r;
                float _SampleTexture2D_ae0835b2329a445495d8eee27f934e69_G_5_Float = _SampleTexture2D_ae0835b2329a445495d8eee27f934e69_RGBA_0_Vector4.g;
                float _SampleTexture2D_ae0835b2329a445495d8eee27f934e69_B_6_Float = _SampleTexture2D_ae0835b2329a445495d8eee27f934e69_RGBA_0_Vector4.b;
                float _SampleTexture2D_ae0835b2329a445495d8eee27f934e69_A_7_Float = _SampleTexture2D_ae0835b2329a445495d8eee27f934e69_RGBA_0_Vector4.a;
                float4 _Add_be6743b123384479a669919a073f9b51_Out_2_Vector4;
                Unity_Add_float4(_Multiply_41fa1180021e4266a254dbd00cb37236_Out_2_Vector4, _SampleTexture2D_ae0835b2329a445495d8eee27f934e69_RGBA_0_Vector4, _Add_be6743b123384479a669919a073f9b51_Out_2_Vector4);
                float _Multiply_1d0c5f56583b4470b159fd85778d9d87_Out_2_Float;
                Unity_Multiply_float_float(_Step_606235734f5144748b9be4af90aae761_Out_2_Float, _SampleTexture2D_ae0835b2329a445495d8eee27f934e69_A_7_Float, _Multiply_1d0c5f56583b4470b159fd85778d9d87_Out_2_Float);
                surface.BaseColor = (_Add_be6743b123384479a669919a073f9b51_Out_2_Vector4.xyz);
                surface.Alpha = _Multiply_1d0c5f56583b4470b159fd85778d9d87_Out_2_Float;
                return surface;
            }

            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES Attributes
            #define VFX_SRP_VARYINGS Varyings
            #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
            #endif
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                output.ObjectSpaceNormal = input.normalOS;
                output.ObjectSpaceTangent = input.tangentOS.xyz;
                output.ObjectSpacePosition = input.positionOS;

                return output;
            }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

            #ifdef HAVE_VFX_MODIFICATION
            #if VFX_USE_GRAPH_VALUES
                uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
                /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
            #endif
                /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */

            #endif








                #if UNITY_UV_STARTS_AT_TOP
                #else
                #endif


                output.uv0 = input.texCoord0;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
            }

            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

            // --------------------------------------------------
            // Visual Effect Vertex Invocations
            #ifdef HAVE_VFX_MODIFICATION
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
            #endif

            ENDHLSL
            }
    }
        CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
                                                        CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
                                                        FallBack "Hidden/Shader Graph/FallbackError"
}
