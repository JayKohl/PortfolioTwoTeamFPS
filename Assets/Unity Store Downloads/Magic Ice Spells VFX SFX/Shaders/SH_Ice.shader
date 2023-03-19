// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Ice"
{
	Properties
	{
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "bump" {}
		_TextureSample4("Texture Sample 4", 2D) = "bump" {}
		_Vector0("Vector 0", Vector) = (0.07,0.1,0.1,0)
		_Float1("Float 1", Range( 0 , 5)) = 0
		_Float3("Float 3", Range( 0 , 1)) = 0
		_Float0("Float 0", Range( 0 , 1)) = 0.09
		_TextureSample5("Texture Sample 5", CUBE) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_Float4("Float 4", Range( 0 , 1)) = 1
		_Float2("Float 2", Range( 0 , 2)) = 1
		_TextureSample6("Texture Sample 6", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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

		uniform sampler2D _TextureSample1;
		uniform float _Float0;
		uniform sampler2D _TextureSample4;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _TextureSample2;
		uniform float _Float1;
		uniform samplerCUBE _TextureSample5;
		uniform float3 _Vector0;
		uniform float _Float3;
		uniform sampler2D _TextureSample3;
		uniform float _Float2;
		uniform sampler2D _TextureSample6;
		uniform float4 _TextureSample6_ST;
		uniform float _Float4;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord106 = i.uv_texcoord * float2( 2,2 );
			float3 tex2DNode108 = UnpackNormal( tex2D( _TextureSample1, uv_TexCoord106 ) );
			float2 uv_TexCoord121 = i.uv_texcoord * float2( 5,5 );
			o.Normal = ( tex2DNode108 + UnpackScaleNormal( tex2D( _TextureSample4, uv_TexCoord121 ) ,_Float0 ) );
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 blendOpSrc112 = tex2D( _TextureSample0, uv_TextureSample0 );
			float4 blendOpDest112 = tex2D( _TextureSample2, uv_TexCoord106 );
			float4 temp_output_112_0 = ( saturate( ( blendOpDest112/ ( 1.0 - blendOpSrc112 ) ) ));
			float3 desaturateInitialColor132 = ( temp_output_112_0 * _Float1 ).rgb;
			float desaturateDot132 = dot( desaturateInitialColor132, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar132 = lerp( desaturateInitialColor132, desaturateDot132.xxx, 0.4 );
			o.Albedo = desaturateVar132;
			o.Emission = ( texCUBE( _TextureSample5, ( WorldReflectionVector( i , tex2DNode108 ) + _Vector0 ) ) * _Float3 ).rgb;
			o.Smoothness = pow( ( ( 1.0 - tex2D( _TextureSample3, uv_TexCoord106 ).r ) * _Float2 ) , 4.0 );
			float2 uv_TextureSample6 = i.uv_texcoord * _TextureSample6_ST.xy + _TextureSample6_ST.zw;
			o.Occlusion = tex2D( _TextureSample6, uv_TextureSample6 ).r;
			o.Alpha = _Float4;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
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
-423;175;1651;1004;1913.844;1186.553;1.694993;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;106;-2551.898,-234.6855;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;108;-2117.075,107.7721;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;8744a2895f0bbc340a7a2fabd64af196;8744a2895f0bbc340a7a2fabd64af196;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;109;-1616.42,-715.5333;Float;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;0fbf8134071a7dc4a96425eecc1cc521;0fbf8134071a7dc4a96425eecc1cc521;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;107;-1488.207,-1045.236;Float;True;Property;_TextureSample0;Texture Sample 0;9;0;Create;True;0;0;False;0;143741622b930e745bea98bc0f0f6e11;143741622b930e745bea98bc0f0f6e11;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;113;-1643.844,694.0086;Float;False;Property;_Vector0;Vector 0;4;0;Create;True;0;0;False;0;0.07,0.1,0.1;0.1,0.1,0.1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;114;-713.8155,771.2455;Float;True;Property;_TextureSample3;Texture Sample 3;10;0;Create;True;0;0;False;0;69dd37cbe652acf42bedd4ff4f53f742;69dd37cbe652acf42bedd4ff4f53f742;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldReflectionVector;110;-1595.844,352.0084;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BlendOpsNode;112;-926.6816,-992.6467;Float;True;ColorDodge;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;118;-1339.621,474.9323;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;119;-440.1597,-277.7775;Float;False;Property;_Float1;Float 1;5;0;Create;True;0;0;False;0;0;1.1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;121;-1514.162,-223.6598;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;120;-521.5828,1002.312;Float;True;Property;_Float2;Float 2;12;0;Create;True;0;0;False;0;1;0.7;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;115;-1410.625,217.1441;Float;False;Property;_Float0;Float 0;7;0;Create;True;0;0;False;0;0.09;0.33;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;117;-417.6165,665.5718;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;-119.5686,-447.0578;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;125;-1078.036,-13.42267;Float;True;Property;_TextureSample4;Texture Sample 4;3;0;Create;True;0;0;False;0;None;5feb1feff0259ce41a72353575d16e10;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;0.2;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;126;-1059.009,319.6613;Float;True;Property;_TextureSample5;Texture Sample 5;8;0;Create;True;0;0;False;0;0f1ce361444c2834992ab857df569338;0f1ce361444c2834992ab857df569338;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;122;-705.5557,477.9716;Float;False;Property;_Float3;Float 3;6;0;Create;True;0;0;False;0;0;0.35;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;-226.6891,625.8102;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;-668.3907,-451.426;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;130;342.3112,684.3638;Float;True;Property;_Float4;Float 4;11;0;Create;True;0;0;False;0;1;0.8884622;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;131;67.40295,-95.46796;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;127;4.578404,380.7225;Float;True;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;29.6774,134.7401;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;132;84.06365,-443.1024;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.4;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;128;8.238346,638.6027;Float;True;Property;_TextureSample6;Texture Sample 6;13;0;Create;True;0;0;False;0;f8f5e464a31e03b49b8dad205a2ee6e2;f8f5e464a31e03b49b8dad205a2ee6e2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;84;643.4969,62.11356;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Effects/SH_Ice;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;2;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;108;1;106;0
WireConnection;109;1;106;0
WireConnection;114;1;106;0
WireConnection;110;0;108;0
WireConnection;112;0;107;0
WireConnection;112;1;109;0
WireConnection;118;0;110;0
WireConnection;118;1;113;0
WireConnection;117;0;114;1
WireConnection;124;0;112;0
WireConnection;124;1;119;0
WireConnection;125;1;121;0
WireConnection;125;5;115;0
WireConnection;126;1;118;0
WireConnection;123;0;117;0
WireConnection;123;1;120;0
WireConnection;116;0;112;0
WireConnection;131;0;108;0
WireConnection;131;1;125;0
WireConnection;127;0;123;0
WireConnection;129;0;126;0
WireConnection;129;1;122;0
WireConnection;132;0;124;0
WireConnection;84;0;132;0
WireConnection;84;1;131;0
WireConnection;84;2;129;0
WireConnection;84;4;127;0
WireConnection;84;5;128;0
WireConnection;84;9;130;0
ASEEND*/
//CHKSM=9C21C4F130F879FBDD23068AD29908DFCF60D638