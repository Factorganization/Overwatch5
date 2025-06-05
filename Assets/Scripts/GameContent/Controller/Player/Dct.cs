using System.IO;
using UnityEngine;

namespace GameContent.Controller.Player
{
    public class Dct : MonoBehaviour
    {
        #region methodes

        private void Start()
        {
            Init();
            DctFunc();
            InvertDctFunc();
        }

        private void Init()
        {
            _texMatrice = new short[256, 256];
            _dctMatrice = new short[256, 256];
            _invertMatrice = new short[256, 256];
            var t = texTest.GetRawTextureData<byte>();
            
            var k = 0;
            for (var i = 0; i < 256; i++)
            {
                for (var j = 0; j < 256; j++)
                {
                    _texMatrice[j, i] = t[k];
                    k += 3;
                }
            }
            
            var baseImageArray = texTest.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + "/monke.png", baseImageArray);
            
            for (var i = 0; i < 256; i++)
            {
                for (var j = 0; j < 256; j++)
                {
                    _texMatrice[j, i] -= 128;
                }
            }
        }
        
        private void DctFunc()
        {
            for (var i = 0; i < 256; i += N)
            {
                for (var j = 0; j < 256; j += N)
                {
                    for (var u = i; u < i + N; u++)
                    {
                        for (var v = j; v < j + N; v++)
                        {
                            _dctMatrice[u, v] = (short)ApplyDCTOnChunkAtPos(_texMatrice, N, u, v, i, j);
                            _dctMatrice[u, v] = (short)(_dctMatrice[u, v] / QuantizationTable[u % N, v % N]);
                        }
                    }
                }
            }
                        
            CreatePng(_dctMatrice, "DCTMonke");
        }

        private void InvertDctFunc()
        {
            for (var i = 0; i < 256; i += N)
            {
                for (var j = 0; j < 256; j += N)
                {
                    for (var u = i; u < i + N; u++)
                    {
                        for (var v = j; v < j + N; v++)
                        {
                            _dctMatrice[u, v] = (short)(_dctMatrice[u, v] * QuantizationTable[u % N, v % N]);
                            _invertMatrice[u, v] = (short)ApplyInvertDCTOnChunkAtPos(_dctMatrice, N, u, v, i, j);
                            _invertMatrice[u, v] += 128;
                        }
                    }
                }
            }
            
            CreatePng(_invertMatrice, "InvertDCTMonke");
        }

        private static float PCos(int n, int k, int N) => Mathf.Cos((2 * n + 1) * k * Mathf.PI / (2 * N));

        private static float ApplyDCTOnChunkAtPos(short[,] mat, int n, int u, int v, int i, int j)
        {
            var s = 0f;
            u %= n;
            v %= n;
            
            for (var x = 0; x < n; x++)
            {
                for (var y = 0; y < n; y++)
                {
                    s += mat[x + i, y + j] * PCos(x, u, n) * PCos(y, v, n);
                }
            }
            
            if (u == 0)
                s *= 1 / Mathf.Sqrt(2);
            
            if (v == 0)
                s *= 1 / Mathf.Sqrt(2);
            
            s *= 2 / Mathf.Sqrt(n * n);
            
            return s;
        }

        private static float ApplyInvertDCTOnChunkAtPos(short[,] dctMat, int n, int u, int v, int i, int j)
        {
            var s = 0f;
            u %= n;
            v %= n; 
            
            for (var x = 0; x < n; x++)
            {
                for (var y = 0; y < n; y++)
                {
                    var v1 = 1f;
                    var v2 = 1f;
                    
                    if (x == 0)
                        v1 = 1 / Mathf.Sqrt(2);
                    
                    if (y == 0)
                        v2 = 1 / Mathf.Sqrt(2);
                    
                    s += dctMat[x + i, y + j] * v1 * v2 * PCos(x, u, n) * PCos(y, v, n);
                }
            }

            s *= 2f / n;

            return s;
        }
        
        private static void CreatePng(short[,] mat, string pngName)
        {
            var tex = new Texture2D(256, 256, TextureFormat.R8, false);
            var t = tex.GetRawTextureData<byte>();

            var k = 0;
            for (var i = 0; i < 256; i++)
            {
                for (var j = 0; j < 256; j++)
                {
                    t[k] = (byte)mat[j, i];
                    
                    k++;
                }
            }
            
            tex.Apply();
            File.WriteAllBytes(Application.persistentDataPath + $"/{pngName}.png", tex.EncodeToPNG());
        }
        
        #endregion
        
        #region fields
        
        [SerializeField] private Texture2D texTest;

        private short[,] _texMatrice;
        
        private short[,] _dctMatrice;

        private short[,] _invertMatrice;
        
        private const int N = 8;
        
        private static readonly float[,] QuantizationTable = new float[N, N]
        {
            { 16, 11, 10, 16, 24, 40, 51, 60 },
            { 12, 12, 14, 19, 26, 58, 60, 55 },
            { 14, 13, 16, 24, 40, 57, 69, 56 },
            { 14, 17, 22, 29, 51, 87, 80, 62 },
            { 18, 22, 37, 56, 68, 109, 103, 77 },
            { 24, 35, 55, 64, 81, 104, 113, 92 },
            { 49, 64, 78, 87, 103, 121, 120, 101 },
            { 72, 92, 95, 98, 112, 100, 103, 99 }
        };

        #endregion
    }
}