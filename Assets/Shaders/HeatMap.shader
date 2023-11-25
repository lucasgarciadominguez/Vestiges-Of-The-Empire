Shader "Unlit/HeatMap"
{
    Properties
    {
      _MainTex("Texture", 2D) = "white" {}
      _Color0("Color 0",Color) = (0,0,0,1)
      _Color1("Color 1",Color) = (0,.9,.2,1)
      _Color2("Color 2",Color) = (.9,1,.3,1)
      _Color3("Color 3",Color) = (.9,.7,.1,1)
      _Color4("Color 4",Color) = (1,0,0,1)

      _Range0("Range 0",Range(0,1)) = 0.
      _Range1("Range 1",Range(0,1)) = 0.25
      _Range2("Range 2",Range(0,1)) = 0.5
      _Range3("Range 3",Range(0,1)) = 0.75
      _Range4("Range 4",Range(0,1)) = 1
      _count("Count Pixels",Integer) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color0;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;

            float _Range0;
            float _Range1;
            float _Range2;
            float _Range3;
            float _Range4;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            float4 colors[5];
            float pointranges[5];

            float _Hits[3 * 32];
            int _HitCount = 0;
            int _count;
            void init()
            {
                colors[0] = _Color0;
                colors[1] = _Color1;
                colors[2] = _Color2;
                colors[3] = _Color3;
                colors[4] = _Color4;
                //pointranges[0] = _Range0;
                //pointranges[1] = _Range1;
                //pointranges[2] = _Range2;
                //pointranges[3] = _Range3;
                //pointranges[4] = _Range4;
            }
            float distsq(float2 a, float2 b)    //gets a value between 0 to 1
            {
                float area_of_effect_size = 0.15f;

                float d = pow(max(0.0,1.0 - distance(a, b) / area_of_effect_size),2);
                //if d gets negative it will returns you 0. If its >1, d will return 1. That's the use in this case for max()
                //pow() is only for getting always positive values
                //restricts it for making only not greater than area_of_effect_size percent of the texture
                //if the distance(a,b) returns 1 it would be (0-1) ==> 1 and its the maximum value
                return d;
            }

            float3 getHeatForPixel(float weight)
            {
                if (weight <= pointranges[0])
                {
                    return colors[0];
                }
                if (weight >= pointranges[4])
                {
                    return colors[4];
                }
                for (int u  = 1; u < 5;u++)
                {
                    if (weight < pointranges[u])
                    {
                        float dist_from_lower_point = weight - pointranges[u - 1];
                        float size_of_point_range = pointranges[u] - pointranges[u - 1];
                        float ratio_over_lower_point = dist_from_lower_point / size_of_point_range;

                        float3 color_range = colors[u] - colors[u - 1];
                        float3 color_contribution = color_range * ratio_over_lower_point;

                        float3 new_color = colors[u - 1] + color_contribution;

                        return new_color;
                    }
                }
                return colors[0];
            }
            fixed4 frag(v2f i) : SV_Target
            {
                init();
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 uv = i.uv; 
                float totalWeight = 0;
                for (float count = 0; count < _HitCount; count++)
                {
                    float2 work_points = float2(_Hits[count * 3 + 0], _Hits[count * 3 + 1]);    //gets the values in the array of floats for knowing the x and y coordinates. F Example: being i=1 ==> _Hits[3] and _Hits[4]. Gets whatever texture coordinates in the entire array for the hits
                    float intensity_points = _Hits[count * 3 + 2];  //the same but for getting the intensity value in the array of hits
                    totalWeight += 0.5 * distsq(uv, work_points) * intensity_points;    //gets the weight so you can assign it a range on the pointranges[]
                }//for i
                float3 heat = getHeatForPixel(totalWeight);
                return col + float4(heat,0.5);
            }
            ENDCG
        }
    }
}
