// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Ice_Rock"
{
	Properties
	{
		_IceAlbedo("Ice Albedo", 2D) = "white" {}
		_IceNormal("Ice Normal", 2D) = "bump" {}
		_ReflectionTile("Reflection Tile", Vector) = (0.07,0.1,0.1,0)
		_NormalTile("Normal Tile", Vector) = (0.07,0.1,0.1,0)
		_Specular("Specular", Range( 0 , 1)) = 0
		_Ammisive("Ammisive", Range( 0 , 1)) = 0
		_Float0("Float 0", Range( 0 , 1)) = 0
		_Reflections("Reflections", CUBE) = "white" {}
		_T_Ice_D("T_Ice_D", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 1
		_Shiness("Shiness", Range( 0 , 2)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
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

		uniform sampler2D _IceNormal;
		uniform float4 _IceNormal_ST;
		uniform float3 _NormalTile;
		uniform sampler2D _IceAlbedo;
		uniform float4 _IceAlbedo_ST;
		uniform float _Shiness;
		uniform samplerCUBE _Reflections;
		uniform float3 _ReflectionTile;
		uniform float _Ammisive;
		uniform float _Float0;
		uniform sampler2D _T_Ice_D;
		uniform float4 _T_Ice_D_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Specular;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_IceNormal = i.uv_texcoord * _IceNormal_ST.xy + _IceNormal_ST.zw;
			float3 tex2DNode4 = UnpackNormal( tex2D( _IceNormal, uv_IceNormal ) );
			o.Normal = ( tex2DNode4 + _NormalTile );
			float2 uv_IceAlbedo = i.uv_texcoord * _IceAlbedo_ST.xy + _IceAlbedo_ST.zw;
			float4 temp_cast_0 = (_Shiness).xxxx;
			o.Albedo = pow( tex2D( _IceAlbedo, uv_IceAlbedo ) , temp_cast_0 ).rgb;
			o.Emission = ( texCUBE( _Reflections, ( WorldReflectionVector( i , tex2DNode4 ) + _ReflectionTile ) ) * _Ammisive ).rgb;
			o.Metallic = _Float0;
			float2 uv_T_Ice_D = i.uv_texcoord * _T_Ice_D_ST.xy + _T_Ice_D_ST.zw;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 lerpResult77 = lerp( tex2D( _T_Ice_D, uv_T_Ice_D ) , tex2D( _TextureSample0, uv_TextureSample0 ) , 0.0);
			o.Smoothness = ( lerpResult77 * _Specular ).r;
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
12;29;1651;1004;1551.667;790.5971;1.661198;True;True
Node;AmplifyShaderEditor.SamplerNode;4;-2384,-16;Float;True;Property;_IceNormal;Ice Normal;1;0;Create;True;0;0;False;0;None;5feb1feff0259ce41a72353575d16e10;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;31;-2096,768;Float;False;Property;_ReflectionTile;Reflection Tile;3;0;Create;True;0;0;False;0;0.07,0.1,0.1;1,1,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldReflectionVector;33;-2048,432;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;78;-1037.7,455.0015;Float;True;Property;_T_Ice_D;T_Ice_D;9;0;Create;True;0;0;False;0;b6c6155db38fbea439cfb395bc672aa1;b6c6155db38fbea439cfb395bc672aa1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;79;-1054.464,718.6461;Float;True;Property;_TextureSample0;Texture Sample 0;10;0;Create;True;0;0;False;0;None;22d3bf82fc2abc84585d2d570fc84b1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-1791.777,554.9238;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;43;-272.144,51.84301;Float;False;Property;_NormalTile;Normal Tile;4;0;Create;True;0;0;False;0;0.07,0.1,0.1;0.5,0.5,0.5;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;77;-598.1031,467.9186;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-658.9412,722.8984;Float;False;Property;_Specular;Specular;5;0;Create;True;0;0;False;0;0;0.941;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-387.4308,-395.874;Float;False;Property;_Shiness;Shiness;12;0;Create;True;0;0;False;0;0;0.49;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;40;-1511.165,400.6528;Float;True;Property;_Reflections;Reflections;8;0;Create;True;0;0;False;0;0f1ce361444c2834992ab857df569338;0f1ce361444c2834992ab857df569338;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-375.5984,374.1487;Float;False;Property;_Ammisive;Ammisive;6;0;Create;True;0;0;False;0;0;0.587;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-811.4,-722.1001;Float;True;Property;_IceAlbedo;Ice Albedo;0;0;Create;True;0;0;False;0;None;b6c6155db38fbea439cfb395bc672aa1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;86;259.2357,264.2438;Float;False;Property;_Float0;Float 0;7;0;Create;True;0;0;False;0;0;0.9;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;27.87477,-21.21491;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;85;333.9,504.2484;Float;False;Property;_Opacity;Opacity;11;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;-1249.96,-98.47198;Float;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;14;-1766.209,171.8382;Float;True;Property;_SnowNormal;Snow Normal;2;0;Create;True;0;0;False;0;None;035a3a98d5495104d86fcf697b1a57cb;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;8.062536,216.9768;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;80;-9.488777,-494.8324;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.4;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;75.32555,552.2203;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;84;643.4969,62.11356;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Effects/SH_Ice_Rock;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;33;0;4;0
WireConnection;34;0;33;0
WireConnection;34;1;31;0
WireConnection;77;0;78;0
WireConnection;77;1;79;0
WireConnection;40;1;34;0
WireConnection;44;0;4;0
WireConnection;44;1;43;0
WireConnection;15;0;4;0
WireConnection;15;1;14;0
WireConnection;28;0;40;0
WireConnection;28;1;27;0
WireConnection;80;0;1;0
WireConnection;80;1;81;0
WireConnection;48;0;77;0
WireConnection;48;1;49;0
WireConnection;84;0;80;0
WireConnection;84;1;44;0
WireConnection;84;2;28;0
WireConnection;84;3;86;0
WireConnection;84;4;48;0
WireConnection;84;9;85;0
ASEEND*/
//CHKSM=F5ABE3C4048D24C61B8606EBC6F4947AF9CA208B