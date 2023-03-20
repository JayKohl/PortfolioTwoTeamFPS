// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Swirl_02"
{
	Properties
	{
		_Offset("Offset", Range( 0 , 5)) = 1
		_T_03("T_03", 2D) = "white" {}
		_T_01("T_01", 2D) = "white" {}
		_Panning_Intensity("Panning_Intensity", Vector) = (0.1,-0.21,0,0)
		_Offset_Correction("Offset_Correction", Vector) = (0.02,-0.01,0,0)
		_T_02("T_02", 2D) = "white" {}
		_T_04("T_04", 2D) = "white" {}
		_Main_T("Main_T", 2D) = "white" {}
		_T_05("T_05", 2D) = "white" {}
		_FireIntensity("FireIntensity", Range( 0 , 5)) = 0
		_Opacity("Opacity", Range( 0 , 2)) = 1
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
		uniform sampler2D _T_05;
		uniform float _FireIntensity;
		uniform sampler2D _Main_T;
		uniform float2 _Offset_Correction;
		uniform float _Offset;
		uniform sampler2D _T_01;
		uniform sampler2D _T_02;
		uniform sampler2D _T_03;
		uniform float2 _Panning_Intensity;
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
			float2 panner82 = ( 1.0 * _Time.y * float2( 0.03,0.01 ) + uv_TexCoord79);
			float2 uv_TexCoord75 = i.uv_texcoord * float2( 0.4,0.3 );
			float2 panner81 = ( _Time.x * float2( -0.3,-0.1 ) + uv_TexCoord75);
			o.Emission = ( pow( ( ( tex2D( _T_04, panner82 ) + tex2D( _T_05, panner81 ) ) * _FireIntensity ) , 1.8 ) * i.vertexColor ).rgb;
			float2 uv_TexCoord71 = i.uv_texcoord * float2( 0.7,0.7 ) + float2( 0.15,0.055 );
			float2 uv_TexCoord61 = i.uv_texcoord * float2( 0.2,0.3 );
			float2 panner62 = ( 0.4 * _Time.y * float2( -0.1,-0.2 ) + uv_TexCoord61);
			float2 uv_TexCoord63 = i.uv_texcoord * float2( 4,4 );
			float2 panner67 = ( 0.2 * _Time.y * float2( 1,-0.2 ) + uv_TexCoord63);
			float2 uv_TexCoord64 = i.uv_texcoord * _Panning_Intensity;
			float2 panner66 = ( 1.0 * _Time.y * float2( 0.1,-0.2 ) + uv_TexCoord64);
			float4 lerpResult72 = lerp( pow( tex2D( _T_01, panner62 ) , 3.0 ) , tex2D( _T_02, panner67 ) , tex2D( _T_03, panner66 ).r);
			float2 uv_Gradient = i.uv_texcoord * _Gradient_ST.xy + _Gradient_ST.zw;
			o.Alpha = ( ( i.vertexColor.a * ( tex2D( _Main_T, ( ( _Offset_Correction + uv_TexCoord71 ) + ( _Offset * (lerpResult72).rg ) ) ) * ( 1.0 - tex2D( _Gradient, uv_Gradient ) ) ) ) * _Opacity ).r;
		}

		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
406;29;1411;1004;2054.428;476.2153;1.692172;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;-4076.253,769.8287;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.2,0.3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;60;-3791.072,1680.14;Float;False;Property;_Panning_Intensity;Panning_Intensity;3;0;Create;True;0;0;False;0;0.1,-0.21;0.1,-0.21;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;64;-3554.472,1657.351;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;-3770.411,1288.248;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;4,4;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;62;-3702.486,744.2991;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.1,-0.2;False;1;FLOAT;0.4;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;66;-3274.264,1639.64;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,-0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;65;-3462.486,696.299;Float;True;Property;_T_01;T_01;2;0;Create;True;0;0;False;0;None;22d3bf82fc2abc84585d2d570fc84b1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;67;-3458.411,1271.248;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,-0.2;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;68;-3218.411,1223.248;Float;True;Property;_T_02;T_02;5;0;Create;True;0;0;False;0;None;22d3bf82fc2abc84585d2d570fc84b1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;69;-3065.638,707.1447;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;70;-3057.129,1612.64;Float;True;Property;_T_03;T_03;1;0;Create;True;0;0;False;0;None;c7572165342acd54cb7a26aa313146f2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;72;-2487.131,1339.856;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TimeNode;76;-1832.896,125.6392;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;75;-1904,-35.9408;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.4,0.3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;79;-1899.508,-228.1308;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.2,0.4;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;80;-2065.714,1325.938;Float;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;71;-1943.251,850.1865;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.7,0.7;False;1;FLOAT2;0.15,0.055;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;73;-1918.071,679.7388;Float;False;Property;_Offset_Correction;Offset_Correction;4;0;Create;True;0;0;False;0;0.02,-0.01;-0.01,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;78;-2255.021,1092.73;Float;False;Property;_Offset;Offset;0;0;Create;True;0;0;False;0;1;0.02;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;81;-1530.436,-2.363344;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.3,-0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-1761.514,1232.338;Float;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-1546.313,724.3359;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;82;-1543.582,-233.3892;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.03,0.01;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;88;-1236.412,2.262998;Float;True;Property;_T_05;T_05;8;0;Create;True;0;0;False;0;None;3b583d8b056a89945ba9e0cdbdcee87e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;86;-905.585,1191.273;Float;True;Property;_Gradient;Gradient;11;0;Create;True;0;0;False;0;2b747c4f15a88b541a7a9ad8c1cd6fa0;2b747c4f15a88b541a7a9ad8c1cd6fa0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;85;-1178.518,1157.274;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;87;-1242.459,-265.7625;Float;True;Property;_T_04;T_04;6;0;Create;True;0;0;False;0;None;c7572165342acd54cb7a26aa313146f2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;89;-879.5262,899.3292;Float;True;Property;_Main_T;Main_T;7;0;Create;True;0;0;False;0;012e7799eba970d4297780fb02697e88;f7f49413e28727a409e66a6785495710;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;91;-847.9656,167.7207;Float;False;Property;_FireIntensity;FireIntensity;9;0;Create;True;0;0;False;0;0;2;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;-840.9112,-149.9313;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;92;-574.7075,1128.437;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-508.8268,-15.59686;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;93;-538.1603,440.16;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-358.33,869.9299;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;0.805253,738.2758;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;97;-160.5543,3.719052;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;1.8;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;98;40.60408,972.1934;Float;False;Property;_Opacity;Opacity;10;0;Create;True;0;0;False;0;1;0.92;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;257.6041,568.1934;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;174.7086,86.08711;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;555.2441,163.9529;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Effects/SH_Swirl_02;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;True;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;64;0;60;0
WireConnection;62;0;61;0
WireConnection;66;0;64;0
WireConnection;65;1;62;0
WireConnection;67;0;63;0
WireConnection;68;1;67;0
WireConnection;69;0;65;0
WireConnection;70;1;66;0
WireConnection;72;0;69;0
WireConnection;72;1;68;0
WireConnection;72;2;70;0
WireConnection;80;0;72;0
WireConnection;81;0;75;0
WireConnection;81;1;76;1
WireConnection;84;0;78;0
WireConnection;84;1;80;0
WireConnection;77;0;73;0
WireConnection;77;1;71;0
WireConnection;82;0;79;0
WireConnection;88;1;81;0
WireConnection;85;0;77;0
WireConnection;85;1;84;0
WireConnection;87;1;82;0
WireConnection;89;1;85;0
WireConnection;90;0;87;0
WireConnection;90;1;88;0
WireConnection;92;0;86;0
WireConnection;95;0;90;0
WireConnection;95;1;91;0
WireConnection;94;0;89;0
WireConnection;94;1;92;0
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
//CHKSM=B9361116AB599CFAF23034832ECC735E60DC0B7C