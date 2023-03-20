// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Swirl_03"
{
	Properties
	{
		_T_04("T_04", 2D) = "white" {}
		_Main_T("Main_T", 2D) = "white" {}
		_FireIntensity("FireIntensity", Range( 0 , 10)) = 5
		_Opacity("Opacity", Range( 0 , 2)) = 0.33
		_Gradient("Gradient", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _T_04;
		uniform sampler2D _Main_T;
		uniform float4 _Main_T_ST;
		uniform float _FireIntensity;
		uniform sampler2D _Gradient;
		uniform float4 _Gradient_ST;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord79 = i.uv_texcoord * float2( 0.2,0.4 );
			float2 panner82 = ( 1.0 * _Time.y * float2( 0.1,0 ) + uv_TexCoord79);
			float2 uv_Main_T = i.uv_texcoord * _Main_T_ST.xy + _Main_T_ST.zw;
			float4 tex2DNode89 = tex2D( _Main_T, uv_Main_T );
			o.Emission = ( pow( ( ( tex2D( _T_04, panner82 ) + tex2DNode89 ) * _FireIntensity ) , 1.8 ) * i.vertexColor ).rgb;
			float2 uv_Gradient = i.uv_texcoord * _Gradient_ST.xy + _Gradient_ST.zw;
			o.Alpha = ( ( i.vertexColor.a * ( tex2DNode89 * ( 1.0 - tex2D( _Gradient, uv_Gradient ) ) ) ) * _Opacity ).r;
		}

		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
406;143;1411;1004;777.4877;142.7174;1.391873;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;79;-1897.68,-228.1308;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.2,0.4;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;82;-1543.582,-233.3892;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;86;-905.585,1191.273;Float;True;Property;_Gradient;Gradient;4;0;Create;True;0;0;False;0;2b747c4f15a88b541a7a9ad8c1cd6fa0;2b747c4f15a88b541a7a9ad8c1cd6fa0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;87;-1240.384,-263.6873;Float;True;Property;_T_04;T_04;0;0;Create;True;0;0;False;0;22d3bf82fc2abc84585d2d570fc84b1f;22d3bf82fc2abc84585d2d570fc84b1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;89;-1346.771,523.3778;Float;True;Property;_Main_T;Main_T;1;0;Create;True;0;0;False;0;012e7799eba970d4297780fb02697e88;012e7799eba970d4297780fb02697e88;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;92;-574.7075,1128.437;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-849.7939,167.7207;Float;False;Property;_FireIntensity;FireIntensity;2;0;Create;True;0;0;False;0;5;2.76;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;-840.9112,-149.9313;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-358.33,869.9299;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-508.8268,-15.59686;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;93;-533.7346,351.6452;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;0.805253,738.2758;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;98;40.60408,972.1934;Float;False;Property;_Opacity;Opacity;3;0;Create;True;0;0;False;0;0.33;0.94;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;97;-175.1808,-191.9101;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;1.8;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;257.6041,568.1934;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;174.7086,86.08711;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;555.2441,163.9529;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Effects/SH_Swirl_03;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;True;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;82;0;79;0
WireConnection;87;1;82;0
WireConnection;92;0;86;0
WireConnection;90;0;87;0
WireConnection;90;1;89;0
WireConnection;94;0;89;0
WireConnection;94;1;92;0
WireConnection;95;0;90;0
WireConnection;95;1;91;0
WireConnection;96;0;93;4
WireConnection;96;1;94;0
WireConnection;97;0;95;0
WireConnection;99;0;96;0
WireConnection;99;1;98;0
WireConnection;100;0;97;0
WireConnection;100;1;93;0
WireConnection;0;2;100;0
WireConnection;0;9;99;0
ASEEND*/
//CHKSM=3809DC4C927569D4D303D976C9EA8B444696F766