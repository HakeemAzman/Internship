// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1443,x:37993,y:33330,varname:node_1443,prsc:2|diff-1688-OUT,spec-4783-OUT,gloss-9290-OUT,normal-888-OUT;n:type:ShaderForge.SFN_Tex2d,id:894,x:33463,y:33194,ptovrint:False,ptlb:MaskMixer4,ptin:_MaskMixer4,varname:node_894,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:ee6aad151b91eb447b4aad3efba0a47b,ntxv:1,isnm:False;n:type:ShaderForge.SFN_Color,id:9461,x:34504,y:33636,ptovrint:False,ptlb:Mask1_color,ptin:_Mask1_color,varname:node_9461,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.007352948,c3:0.007352948,c4:1;n:type:ShaderForge.SFN_Multiply,id:1649,x:34026,y:33687,varname:node_1649,prsc:2|A-894-R,B-9572-OUT;n:type:ShaderForge.SFN_Color,id:944,x:34533,y:33858,ptovrint:False,ptlb:Mask2_color,ptin:_Mask2_color,varname:_Mask1_color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.05082181,c2:0.6911765,c3:0.1082329,c4:1;n:type:ShaderForge.SFN_Color,id:6331,x:34488,y:34102,ptovrint:False,ptlb:Mask3_color,ptin:_Mask3_color,varname:_Mask2_color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.04974049,c2:0.3912004,c3:0.6764706,c4:1;n:type:ShaderForge.SFN_Add,id:2892,x:35020,y:33690,varname:node_2892,prsc:2|A-5946-OUT,B-5186-OUT;n:type:ShaderForge.SFN_Add,id:1664,x:35265,y:33864,varname:node_1664,prsc:2|A-2892-OUT,B-937-OUT;n:type:ShaderForge.SFN_Multiply,id:5946,x:34805,y:33651,varname:node_5946,prsc:2|A-9461-RGB,B-1649-OUT;n:type:ShaderForge.SFN_Multiply,id:2365,x:34047,y:33904,varname:node_2365,prsc:2|A-894-G,B-9323-OUT;n:type:ShaderForge.SFN_Multiply,id:1843,x:34071,y:34121,varname:node_1843,prsc:2|A-894-B,B-4140-OUT;n:type:ShaderForge.SFN_Tex2d,id:47,x:33272,y:33645,ptovrint:False,ptlb:Detail_Map1,ptin:_Detail_Map1,varname:node_47,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:1,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:6552,x:33243,y:33951,ptovrint:False,ptlb:Detail_Map2,ptin:_Detail_Map2,varname:_Detail_Map2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:1,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5824,x:33151,y:34295,ptovrint:False,ptlb:Detail_Map3,ptin:_Detail_Map3,varname:_Detail_Map3,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:1,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5186,x:34805,y:33836,varname:node_5186,prsc:2|A-944-RGB,B-2365-OUT;n:type:ShaderForge.SFN_Multiply,id:937,x:34805,y:34049,varname:node_937,prsc:2|A-6331-RGB,B-1843-OUT;n:type:ShaderForge.SFN_Tex2d,id:2018,x:35532,y:33753,ptovrint:False,ptlb:Detail_Norm1,ptin:_Detail_Norm1,varname:node_2018,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:1912,x:35532,y:33935,ptovrint:False,ptlb:Detail_Norm2,ptin:_Detail_Norm2,varname:_Detail_Norm2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:2122,x:35532,y:34124,ptovrint:False,ptlb:Detail_Norm3,ptin:_Detail_Norm3,varname:_Detail_Norm3,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Multiply,id:5646,x:36102,y:33780,varname:node_5646,prsc:2|A-2223-OUT,B-894-R;n:type:ShaderForge.SFN_Multiply,id:3838,x:36102,y:33955,varname:node_3838,prsc:2|A-1954-OUT,B-894-G;n:type:ShaderForge.SFN_Multiply,id:6696,x:36091,y:34115,varname:node_6696,prsc:2|A-3270-OUT,B-894-B;n:type:ShaderForge.SFN_Multiply,id:7156,x:35377,y:32336,varname:node_7156,prsc:2|A-894-R,B-3509-OUT;n:type:ShaderForge.SFN_Multiply,id:7934,x:35387,y:32556,varname:node_7934,prsc:2|A-894-G,B-3703-OUT;n:type:ShaderForge.SFN_Multiply,id:5407,x:35371,y:32752,varname:node_5407,prsc:2|A-894-B,B-7296-OUT;n:type:ShaderForge.SFN_Slider,id:3509,x:34726,y:32478,ptovrint:False,ptlb:DetailGloss1,ptin:_DetailGloss1,varname:node_3509,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:3703,x:34750,y:32643,ptovrint:False,ptlb:DetailGloss2,ptin:_DetailGloss2,varname:_DetailGloss2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7112322,max:1;n:type:ShaderForge.SFN_Slider,id:7296,x:34790,y:32765,ptovrint:False,ptlb:DetailGloss3,ptin:_DetailGloss3,varname:_DetailGloss3,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4144778,max:1;n:type:ShaderForge.SFN_Add,id:911,x:35658,y:32449,varname:node_911,prsc:2|A-7156-OUT,B-7934-OUT;n:type:ShaderForge.SFN_Add,id:5684,x:35993,y:32710,varname:node_5684,prsc:2|A-911-OUT,B-5407-OUT;n:type:ShaderForge.SFN_Add,id:4876,x:36293,y:34115,varname:node_4876,prsc:2|A-5262-OUT,B-6696-OUT;n:type:ShaderForge.SFN_Color,id:1873,x:34076,y:32878,ptovrint:False,ptlb:DetailSpecCol1,ptin:_DetailSpecCol1,varname:node_1873,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:819,x:34087,y:33160,ptovrint:False,ptlb:DetailSpecCol2,ptin:_DetailSpecCol2,varname:_DetailSpecCol2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.1172414,c2:1,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:8651,x:34040,y:33382,ptovrint:False,ptlb:DetailSpecCol3,ptin:_DetailSpecCol3,varname:_DetailSpecCol3,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:756,x:34501,y:32955,varname:node_756,prsc:2|A-1873-RGB,B-894-R;n:type:ShaderForge.SFN_Multiply,id:5624,x:34539,y:33103,varname:node_5624,prsc:2|A-819-RGB,B-894-G;n:type:ShaderForge.SFN_Multiply,id:3049,x:34513,y:33289,varname:node_3049,prsc:2|A-8651-RGB,B-894-B;n:type:ShaderForge.SFN_Add,id:8387,x:34794,y:33013,varname:node_8387,prsc:2|A-756-OUT,B-5624-OUT;n:type:ShaderForge.SFN_Add,id:4783,x:35890,y:33309,varname:node_4783,prsc:2|A-8387-OUT,B-3049-OUT;n:type:ShaderForge.SFN_Add,id:5262,x:36384,y:33842,varname:node_5262,prsc:2|A-5646-OUT,B-3838-OUT;n:type:ShaderForge.SFN_Color,id:9123,x:36637,y:33002,ptovrint:False,ptlb:Base_Color,ptin:_Base_Color,varname:node_9123,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:33333,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:4841,x:36854,y:33118,varname:node_4841,prsc:2|A-9123-RGB,B-8043-OUT;n:type:ShaderForge.SFN_Clamp01,id:9290,x:36438,y:33171,varname:node_9290,prsc:2|IN-5684-OUT;n:type:ShaderForge.SFN_Clamp01,id:8043,x:35501,y:33577,varname:node_8043,prsc:2|IN-1664-OUT;n:type:ShaderForge.SFN_Tex2d,id:2478,x:36828,y:32863,ptovrint:False,ptlb:Base_Texture,ptin:_Base_Texture,varname:node_2478,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4698,x:37136,y:33937,ptovrint:False,ptlb:Base_normal_Map,ptin:_Base_normal_Map,varname:node_4698,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_NormalBlend,id:888,x:37702,y:33840,varname:node_888,prsc:2|BSE-4876-OUT,DTL-4698-RGB;n:type:ShaderForge.SFN_Blend,id:3232,x:37165,y:33023,varname:node_3232,prsc:2,blmd:1,clmp:True|SRC-2478-RGB,DST-4841-OUT;n:type:ShaderForge.SFN_Tex2d,id:6999,x:36579,y:32486,ptovrint:False,ptlb:AO_texture,ptin:_AO_texture,varname:node_6999,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1688,x:37403,y:32936,varname:node_1688,prsc:2|A-8300-OUT,B-3232-OUT;n:type:ShaderForge.SFN_Lerp,id:9572,x:33724,y:33727,varname:node_9572,prsc:2|A-8377-OUT,B-47-RGB,T-8175-OUT;n:type:ShaderForge.SFN_Vector3,id:8377,x:33445,y:33633,varname:node_8377,prsc:2,v1:1,v2:1,v3:1;n:type:ShaderForge.SFN_Slider,id:8175,x:34925,y:34076,ptovrint:False,ptlb:Detail1_strength,ptin:_Detail1_strength,varname:node_8175,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Vector3,id:8772,x:33560,y:33942,varname:node_8772,prsc:2,v1:1,v2:1,v3:1;n:type:ShaderForge.SFN_Slider,id:6987,x:34925,y:34192,ptovrint:False,ptlb:Detail2_strength,ptin:_Detail2_strength,varname:_Detail1_strength_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Lerp,id:9323,x:33839,y:33969,varname:node_9323,prsc:2|A-8772-OUT,B-6552-RGB,T-6987-OUT;n:type:ShaderForge.SFN_Lerp,id:4140,x:33823,y:34234,varname:node_4140,prsc:2|A-5007-OUT,B-5824-RGB,T-7963-OUT;n:type:ShaderForge.SFN_Vector3,id:5007,x:33560,y:34227,varname:node_5007,prsc:2,v1:1,v2:1,v3:1;n:type:ShaderForge.SFN_Slider,id:7963,x:34925,y:34341,ptovrint:False,ptlb:Detail3_strength,ptin:_Detail3_strength,varname:_Detail2_strength_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4368932,max:1;n:type:ShaderForge.SFN_Multiply,id:8300,x:37286,y:32545,varname:node_8300,prsc:2|A-6999-RGB,B-8074-OUT;n:type:ShaderForge.SFN_ValueProperty,id:144,x:36595,y:32708,ptovrint:False,ptlb:AO_boost,ptin:_AO_boost,varname:node_144,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Vector3,id:1426,x:35650,y:33645,varname:node_1426,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Lerp,id:2223,x:35876,y:33645,varname:node_2223,prsc:2|A-1426-OUT,B-2018-RGB,T-8175-OUT;n:type:ShaderForge.SFN_Lerp,id:1954,x:35905,y:33822,varname:node_1954,prsc:2|A-1426-OUT,B-1912-RGB,T-6987-OUT;n:type:ShaderForge.SFN_Lerp,id:3270,x:35887,y:34021,varname:node_3270,prsc:2|A-1426-OUT,B-2122-RGB,T-7963-OUT;n:type:ShaderForge.SFN_Multiply,id:8074,x:36965,y:32619,varname:node_8074,prsc:2|A-6999-RGB,B-144-OUT;proporder:6999-144-9123-2478-4698-894-9461-944-6331-47-1873-3509-2018-8175-6552-819-3703-1912-6987-5824-8651-7296-2122-7963;pass:END;sub:END;*/

Shader "Custom/UltraShader" {
    Properties {
        _AO_texture ("AO_texture", 2D) = "white" {}
        _AO_boost ("AO_boost", Float ) = 1
        _Base_Color ("Base_Color", Color) = (33333,1,1,1)
        _Base_Texture ("Base_Texture", 2D) = "white" {}
        _Base_normal_Map ("Base_normal_Map", 2D) = "bump" {}
        _MaskMixer4 ("MaskMixer4", 2D) = "gray" {}
        _Mask1_color ("Mask1_color", Color) = (1,0.007352948,0.007352948,1)
        _Mask2_color ("Mask2_color", Color) = (0.05082181,0.6911765,0.1082329,1)
        _Mask3_color ("Mask3_color", Color) = (0.04974049,0.3912004,0.6764706,1)
        _Detail_Map1 ("Detail_Map1", 2D) = "gray" {}
        _DetailSpecCol1 ("DetailSpecCol1", Color) = (1,0,0,1)
        _DetailGloss1 ("DetailGloss1", Range(0, 1)) = 1
        _Detail_Norm1 ("Detail_Norm1", 2D) = "bump" {}
        _Detail1_strength ("Detail1_strength", Range(0, 1)) = 0.5
        _Detail_Map2 ("Detail_Map2", 2D) = "gray" {}
        _DetailSpecCol2 ("DetailSpecCol2", Color) = (0.1172414,1,0,1)
        _DetailGloss2 ("DetailGloss2", Range(0, 1)) = 0.7112322
        _Detail_Norm2 ("Detail_Norm2", 2D) = "bump" {}
        _Detail2_strength ("Detail2_strength", Range(0, 1)) = 0.5
        _Detail_Map3 ("Detail_Map3", 2D) = "gray" {}
        _DetailSpecCol3 ("DetailSpecCol3", Color) = (0,0,1,1)
        _DetailGloss3 ("DetailGloss3", Range(0, 1)) = 0.4144778
        _Detail_Norm3 ("Detail_Norm3", 2D) = "bump" {}
        _Detail3_strength ("Detail3_strength", Range(0, 1)) = 0.4368932
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _MaskMixer4; uniform float4 _MaskMixer4_ST;
            uniform float4 _Mask1_color;
            uniform float4 _Mask2_color;
            uniform float4 _Mask3_color;
            uniform sampler2D _Detail_Map1; uniform float4 _Detail_Map1_ST;
            uniform sampler2D _Detail_Map2; uniform float4 _Detail_Map2_ST;
            uniform sampler2D _Detail_Map3; uniform float4 _Detail_Map3_ST;
            uniform sampler2D _Detail_Norm1; uniform float4 _Detail_Norm1_ST;
            uniform sampler2D _Detail_Norm2; uniform float4 _Detail_Norm2_ST;
            uniform sampler2D _Detail_Norm3; uniform float4 _Detail_Norm3_ST;
            uniform float _DetailGloss1;
            uniform float _DetailGloss2;
            uniform float _DetailGloss3;
            uniform float4 _DetailSpecCol1;
            uniform float4 _DetailSpecCol2;
            uniform float4 _DetailSpecCol3;
            uniform float4 _Base_Color;
            uniform sampler2D _Base_Texture; uniform float4 _Base_Texture_ST;
            uniform sampler2D _Base_normal_Map; uniform float4 _Base_normal_Map_ST;
            uniform sampler2D _AO_texture; uniform float4 _AO_texture_ST;
            uniform float _Detail1_strength;
            uniform float _Detail2_strength;
            uniform float _Detail3_strength;
            uniform float _AO_boost;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 node_1426 = float3(0,0,1);
                float3 _Detail_Norm1_var = UnpackNormal(tex2D(_Detail_Norm1,TRANSFORM_TEX(i.uv0, _Detail_Norm1)));
                float4 _MaskMixer4_var = tex2D(_MaskMixer4,TRANSFORM_TEX(i.uv0, _MaskMixer4));
                float3 _Detail_Norm2_var = UnpackNormal(tex2D(_Detail_Norm2,TRANSFORM_TEX(i.uv0, _Detail_Norm2)));
                float3 _Detail_Norm3_var = UnpackNormal(tex2D(_Detail_Norm3,TRANSFORM_TEX(i.uv0, _Detail_Norm3)));
                float3 _Base_normal_Map_var = UnpackNormal(tex2D(_Base_normal_Map,TRANSFORM_TEX(i.uv0, _Base_normal_Map)));
                float3 node_888_nrm_base = (((lerp(node_1426,_Detail_Norm1_var.rgb,_Detail1_strength)*_MaskMixer4_var.r)+(lerp(node_1426,_Detail_Norm2_var.rgb,_Detail2_strength)*_MaskMixer4_var.g))+(lerp(node_1426,_Detail_Norm3_var.rgb,_Detail3_strength)*_MaskMixer4_var.b)) + float3(0,0,1);
                float3 node_888_nrm_detail = _Base_normal_Map_var.rgb * float3(-1,-1,1);
                float3 node_888_nrm_combined = node_888_nrm_base*dot(node_888_nrm_base, node_888_nrm_detail)/node_888_nrm_base.z - node_888_nrm_detail;
                float3 node_888 = node_888_nrm_combined;
                float3 normalLocal = node_888;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = saturate((((_MaskMixer4_var.r*_DetailGloss1)+(_MaskMixer4_var.g*_DetailGloss2))+(_MaskMixer4_var.b*_DetailGloss3)));
                float specPow = exp2( gloss * 10.0+1.0);
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = (((_DetailSpecCol1.rgb*_MaskMixer4_var.r)+(_DetailSpecCol2.rgb*_MaskMixer4_var.g))+(_DetailSpecCol3.rgb*_MaskMixer4_var.b));
                float specularMonochrome = max( max(specularColor.r, specularColor.g), specularColor.b);
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
                float specularPBL = max(0, (NdotL*visTerm*normTerm) * (UNITY_PI / 4) );
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _AO_texture_var = tex2D(_AO_texture,TRANSFORM_TEX(i.uv0, _AO_texture));
                float4 _Base_Texture_var = tex2D(_Base_Texture,TRANSFORM_TEX(i.uv0, _Base_Texture));
                float4 _Detail_Map1_var = tex2D(_Detail_Map1,TRANSFORM_TEX(i.uv0, _Detail_Map1));
                float4 _Detail_Map2_var = tex2D(_Detail_Map2,TRANSFORM_TEX(i.uv0, _Detail_Map2));
                float4 _Detail_Map3_var = tex2D(_Detail_Map3,TRANSFORM_TEX(i.uv0, _Detail_Map3));
                float3 diffuseColor = ((_AO_texture_var.rgb*(_AO_texture_var.rgb*_AO_boost))*saturate((_Base_Texture_var.rgb*(_Base_Color.rgb*saturate((((_Mask1_color.rgb*(_MaskMixer4_var.r*lerp(float3(1,1,1),_Detail_Map1_var.rgb,_Detail1_strength)))+(_Mask2_color.rgb*(_MaskMixer4_var.g*lerp(float3(1,1,1),_Detail_Map2_var.rgb,_Detail2_strength))))+(_Mask3_color.rgb*(_MaskMixer4_var.b*lerp(float3(1,1,1),_Detail_Map3_var.rgb,_Detail3_strength)))))))));
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _MaskMixer4; uniform float4 _MaskMixer4_ST;
            uniform float4 _Mask1_color;
            uniform float4 _Mask2_color;
            uniform float4 _Mask3_color;
            uniform sampler2D _Detail_Map1; uniform float4 _Detail_Map1_ST;
            uniform sampler2D _Detail_Map2; uniform float4 _Detail_Map2_ST;
            uniform sampler2D _Detail_Map3; uniform float4 _Detail_Map3_ST;
            uniform sampler2D _Detail_Norm1; uniform float4 _Detail_Norm1_ST;
            uniform sampler2D _Detail_Norm2; uniform float4 _Detail_Norm2_ST;
            uniform sampler2D _Detail_Norm3; uniform float4 _Detail_Norm3_ST;
            uniform float _DetailGloss1;
            uniform float _DetailGloss2;
            uniform float _DetailGloss3;
            uniform float4 _DetailSpecCol1;
            uniform float4 _DetailSpecCol2;
            uniform float4 _DetailSpecCol3;
            uniform float4 _Base_Color;
            uniform sampler2D _Base_Texture; uniform float4 _Base_Texture_ST;
            uniform sampler2D _Base_normal_Map; uniform float4 _Base_normal_Map_ST;
            uniform sampler2D _AO_texture; uniform float4 _AO_texture_ST;
            uniform float _Detail1_strength;
            uniform float _Detail2_strength;
            uniform float _Detail3_strength;
            uniform float _AO_boost;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 node_1426 = float3(0,0,1);
                float3 _Detail_Norm1_var = UnpackNormal(tex2D(_Detail_Norm1,TRANSFORM_TEX(i.uv0, _Detail_Norm1)));
                float4 _MaskMixer4_var = tex2D(_MaskMixer4,TRANSFORM_TEX(i.uv0, _MaskMixer4));
                float3 _Detail_Norm2_var = UnpackNormal(tex2D(_Detail_Norm2,TRANSFORM_TEX(i.uv0, _Detail_Norm2)));
                float3 _Detail_Norm3_var = UnpackNormal(tex2D(_Detail_Norm3,TRANSFORM_TEX(i.uv0, _Detail_Norm3)));
                float3 _Base_normal_Map_var = UnpackNormal(tex2D(_Base_normal_Map,TRANSFORM_TEX(i.uv0, _Base_normal_Map)));
                float3 node_888_nrm_base = (((lerp(node_1426,_Detail_Norm1_var.rgb,_Detail1_strength)*_MaskMixer4_var.r)+(lerp(node_1426,_Detail_Norm2_var.rgb,_Detail2_strength)*_MaskMixer4_var.g))+(lerp(node_1426,_Detail_Norm3_var.rgb,_Detail3_strength)*_MaskMixer4_var.b)) + float3(0,0,1);
                float3 node_888_nrm_detail = _Base_normal_Map_var.rgb * float3(-1,-1,1);
                float3 node_888_nrm_combined = node_888_nrm_base*dot(node_888_nrm_base, node_888_nrm_detail)/node_888_nrm_base.z - node_888_nrm_detail;
                float3 node_888 = node_888_nrm_combined;
                float3 normalLocal = node_888;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = saturate((((_MaskMixer4_var.r*_DetailGloss1)+(_MaskMixer4_var.g*_DetailGloss2))+(_MaskMixer4_var.b*_DetailGloss3)));
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float LdotH = max(0.0,dot(lightDirection, halfDirection));
                float3 specularColor = (((_DetailSpecCol1.rgb*_MaskMixer4_var.r)+(_DetailSpecCol2.rgb*_MaskMixer4_var.g))+(_DetailSpecCol3.rgb*_MaskMixer4_var.b));
                float specularMonochrome = max( max(specularColor.r, specularColor.g), specularColor.b);
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float NdotH = max(0.0,dot( normalDirection, halfDirection ));
                float VdotH = max(0.0,dot( viewDirection, halfDirection ));
                float visTerm = SmithBeckmannVisibilityTerm( NdotL, NdotV, 1.0-gloss );
                float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(NdotH, RoughnessToSpecPower(1.0-gloss)));
                float specularPBL = max(0, (NdotL*visTerm*normTerm) * (UNITY_PI / 4) );
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 directDiffuse = ((1 +(fd90 - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd90 - 1)*pow((1.00001-NdotV), 5)) * NdotL) * attenColor;
                float4 _AO_texture_var = tex2D(_AO_texture,TRANSFORM_TEX(i.uv0, _AO_texture));
                float4 _Base_Texture_var = tex2D(_Base_Texture,TRANSFORM_TEX(i.uv0, _Base_Texture));
                float4 _Detail_Map1_var = tex2D(_Detail_Map1,TRANSFORM_TEX(i.uv0, _Detail_Map1));
                float4 _Detail_Map2_var = tex2D(_Detail_Map2,TRANSFORM_TEX(i.uv0, _Detail_Map2));
                float4 _Detail_Map3_var = tex2D(_Detail_Map3,TRANSFORM_TEX(i.uv0, _Detail_Map3));
                float3 diffuseColor = ((_AO_texture_var.rgb*(_AO_texture_var.rgb*_AO_boost))*saturate((_Base_Texture_var.rgb*(_Base_Color.rgb*saturate((((_Mask1_color.rgb*(_MaskMixer4_var.r*lerp(float3(1,1,1),_Detail_Map1_var.rgb,_Detail1_strength)))+(_Mask2_color.rgb*(_MaskMixer4_var.g*lerp(float3(1,1,1),_Detail_Map2_var.rgb,_Detail2_strength))))+(_Mask3_color.rgb*(_MaskMixer4_var.b*lerp(float3(1,1,1),_Detail_Map3_var.rgb,_Detail3_strength)))))))));
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
