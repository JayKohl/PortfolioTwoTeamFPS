// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Ice_Rock_02"
{
	Properties
	{
		_T_IceBlock_Albedo("T_IceBlock_Albedo", 2D) = "white" {}
		_T_IceBlock_N("T_IceBlock_N", 2D) = "bump" {}
		_Normal_02("Normal_02", 2D) = "bump" {}
		_Reflection_Scale("Reflection_Scale", Vector) = (0.07,0.1,0.1,0)
		_Emission("Emission", Range( 0 , 5)) = 0
		_Reflection_Intensity("Reflection_Intensity", Range( 0 , 1)) = 0
		_Normal_Scale("Normal_Scale", Range( 0 , 1)) = 0.09
		_Reflection("Reflection", CUBE) = "white" {}
		_T_IceBlock_ROUGH("T_IceBlock_ROUGH", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 1
		_Roughness("Roughness", Range( 0 , 2)) = 1
		_T_IceBlock_AO_02("T_IceBlock_AO_02", 2D) = "white" {}
		_T_IceBlock_AO_01("T_IceBlock_AO_01", 2D) = "white" {}
		_Normal_Tiling("Normal_Tiling", Vector) = (0,0,0,0)
		_Main_Tiling("Main_Tiling", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.5
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldRefl;
			INTERNAL_DATA
		};

		uniform sampler2D _T_IceBlock_N;
		uniform float2 _Main_Tiling;
		uniform float _Normal_Scale;
		uniform sampler2D _Normal_02;
		uniform float2 _Normal_Tiling;
		uniform sampler2D _T_IceBlock_Albedo;
		uniform float _Emission;
		uniform samplerCUBE _Reflection;
		uniform float3 _Reflection_Scale;
		uniform float _Reflection_Intensity;
		uniform sampler2D _T_IceBlock_ROUGH;
		uniform float _Roughness;
		uniform sampler2D _T_IceBlock_AO_02;
		uniform sampler2D _T_IceBlock_AO_01;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord107 = i.uv_texcoord * _Main_Tiling;
			float3 tex2DNode2 = UnpackNormal( tex2D( _T_IceBlock_N, uv_TexCoord107 ) );
			float2 uv_TexCoord173 = i.uv_texcoord * _Normal_Tiling;
			o.Normal = ( tex2DNode2 + UnpackScaleNormal( tex2D( _Normal_02, uv_TexCoord173 ) ,_Normal_Scale ) );
			float3 desaturateInitialColor267 = ( tex2D( _T_IceBlock_Albedo, uv_TexCoord107 ) * _Emission ).rgb;
			float desaturateDot267 = dot( desaturateInitialColor267, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar267 = lerp( desaturateInitialColor267, desaturateDot267.xxx, 0.4 );
			o.Albedo = desaturateVar267;
			o.Emission = ( texCUBE( _Reflection, ( WorldReflectionVector( i , tex2DNode2 ) + _Reflection_Scale ) ) * _Reflection_Intensity ).rgb;
			o.Smoothness = pow( ( ( 1.0 - tex2D( _T_IceBlock_ROUGH, uv_TexCoord107 ).r ) * _Roughness ) , 10.0 );
			o.Occlusion = ( tex2D( _T_IceBlock_AO_02, uv_TexCoord107 ) + tex2D( _T_IceBlock_AO_01, uv_TexCoord107 ) ).r;
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldRefl = -worldViewDir;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
595;67;1411;1004;2110.52;794.009;2.141513;True;True
Node;AmplifyShaderEditor.Vector2Node;279;-2739.093,-249.7284;Float;False;Property;_Main_Tiling;Main_Tiling;14;0;Create;True;0;0;False;0;0,0;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;107;-2514.258,-234.9111;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;278;-1872.784,793.3091;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-2029.358,-231.855;Float;True;Property;_T_IceBlock_N;T_IceBlock_N;1;0;Create;True;0;0;False;0;8744a2895f0bbc340a7a2fabd64af196;8744a2895f0bbc340a7a2fabd64af196;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;163;-1607.204,695.7831;Float;False;Property;_Reflection_Scale;Reflection_Scale;3;0;Create;True;0;0;False;0;0.07,0.1,0.1;0.2,0.2,0.2;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;91;-951.3422,696.2461;Float;True;Property;_T_IceBlock_ROUGH;T_IceBlock_ROUGH;8;0;Create;True;0;0;False;0;69dd37cbe652acf42bedd4ff4f53f742;69dd37cbe652acf42bedd4ff4f53f742;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;280;-1563.386,-116.9557;Float;False;Property;_Normal_Tiling;Normal_Tiling;13;0;Create;True;0;0;False;0;0,0;7,7;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.WorldReflectionVector;162;-1559.204,353.7828;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;164;-1302.981,476.7067;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;175;-1373.985,218.9185;Float;False;Property;_Normal_Scale;Normal_Scale;6;0;Create;True;0;0;False;0;0.09;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;261;-403.4116,-276.0031;Float;False;Property;_Emission;Emission;4;0;Create;True;0;0;False;0;0;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1582.866,-713.7588;Float;True;Property;_T_IceBlock_Albedo;T_IceBlock_Albedo;0;0;Create;True;0;0;False;0;0fbf8134071a7dc4a96425eecc1cc521;0fbf8134071a7dc4a96425eecc1cc521;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;277;-1931.119,1189.172;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;159;-621.9735,868.4216;Float;True;Property;_Roughness;Roughness;10;0;Create;True;0;0;False;0;1;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;173;-1365.525,-130.5323;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;92;-625.9564,617.7429;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;282;-1932.946,870.8076;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;274;-98.37077,1209.696;Float;True;Property;_T_IceBlock_AO_01;T_IceBlock_AO_01;12;0;Create;True;0;0;False;0;8208aa3b87a391e478fd864dfe835246;8208aa3b87a391e478fd864dfe835246;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;167;-668.916,479.746;Float;False;Property;_Reflection_Intensity;Reflection_Intensity;5;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;165;-1022.369,267.2652;Float;True;Property;_Reflection;Reflection;7;0;Create;True;0;0;False;0;0f1ce361444c2834992ab857df569338;0f1ce361444c2834992ab857df569338;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;161;-1049.73,2.872623;Float;True;Property;_Normal_02;Normal_02;2;0;Create;True;0;0;False;0;None;5feb1feff0259ce41a72353575d16e10;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;0.2;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;-190.0494,627.5847;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;260;-82.92882,-445.2834;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;256;-93.30953,863.5644;Float;True;Property;_T_IceBlock_AO_02;T_IceBlock_AO_02;11;0;Create;True;0;0;False;0;None;22d3bf82fc2abc84585d2d570fc84b1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;267;120.7034,-441.328;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.4;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;166;-328.2971,113.2039;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;257;366.3036,1169.975;Float;True;Property;_Opacity;Opacity;9;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;169;17.01717,-201.4394;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;270;46.71333,385.9275;Float;True;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;276;296.7189,801.3231;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;666.7606,-18.2487;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;Effects/SH_Ice_Rock_02;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;107;0;279;0
WireConnection;278;0;107;0
WireConnection;2;1;107;0
WireConnection;91;1;278;0
WireConnection;162;0;2;0
WireConnection;164;0;162;0
WireConnection;164;1;163;0
WireConnection;1;1;107;0
WireConnection;277;0;107;0
WireConnection;173;0;280;0
WireConnection;92;0;91;1
WireConnection;282;0;107;0
WireConnection;274;1;277;0
WireConnection;165;1;164;0
WireConnection;161;1;173;0
WireConnection;161;5;175;0
WireConnection;160;0;92;0
WireConnection;160;1;159;0
WireConnection;260;0;1;0
WireConnection;260;1;261;0
WireConnection;256;1;282;0
WireConnection;267;0;260;0
WireConnection;166;0;165;0
WireConnection;166;1;167;0
WireConnection;169;0;2;0
WireConnection;169;1;161;0
WireConnection;270;0;160;0
WireConnection;276;0;256;0
WireConnection;276;1;274;0
WireConnection;0;0;267;0
WireConnection;0;1;169;0
WireConnection;0;2;166;0
WireConnection;0;4;270;0
WireConnection;0;5;276;0
WireConnection;0;9;257;0
ASEEND*/
//CHKSM=B9230E09BFBA70D5492DDD499063DB01BD7566EE