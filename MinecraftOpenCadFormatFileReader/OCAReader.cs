using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftOpenCadFormatFileReader
{
    internal class OCAReader
    {
        string inFileName = string.Empty;
        byte[] value;
        List<CadPoint>[] A;
        public List<string> cmd = new List<string>();
        string material = string.Empty;
        CadPoint base_point;
        public OCAReader(string inFileName, CadPoint base_point, string material)
        {
            this.inFileName = inFileName;
            this.base_point = base_point;
            this.material = material;

            var list = ParseOCA();

            var max = list.Count;

            int i = 0;

            A = new List<CadPoint>[max];

            while(i< max )
            {
                A[i] = Parse2DPlane(list[i]);
                i++;
            }

            i = 0;


            string str = string.Empty;

            foreach(var s in A)
            {
                i = 0;
                var cnt = s.Count;

                var start = new CadPoint();
                var end = new CadPoint();

                if (cnt == 5)
                {
                    int j = 0;
                    foreach (var t in s)
                    {
                        //Console.WriteLine(t.x.ToString() + "," + t.y.ToString() + "," + t.z.ToString());

                        //奇数の添え字0と偶数の添え字3をそれぞれ　スタート、エンドとする
                        if (j == 0 && (i % 2) == 0)
                        {
                            start.x = t.x;
                            start.y = t.y;
                            start.z = t.z;
                        }
                        else if(j == 3 && (i % 2) == 1)
                        {
                            end.x = t.x;
                            end.y = t.y;
                            end.z = t.z;
                        }

                        j++;
                        i++;
                    }
                }
                else if(cnt == 9)
                {
                    break;

                    foreach (var t in s)
                    {
                        //Console.WriteLine(t.x.ToString() + "," + t.y.ToString() + "," + t.z.ToString());
                    }

                }

                str = string.Format("fill {0} {1} {2} {3} {4} {5} {6}", start.x + base_point.x, start.z + base_point.z, start.y + base_point.y, end.x + base_point.x, end.z + base_point.z, end.y + base_point.y, material);

                cmd.Add(str);

            }
        }

        public List<CadPoint> Parse2DPlane(string str)
        {
            List<CadPoint> result = new List<CadPoint>();


            int i = 0;

            while(i < str.Length) 
            {
                if (str[i] == ' ' && str[i + 1] == 'P')
                {
                    string contents = string.Empty;

                    i += 3;

                    int j = i;

                    while (str[j] != ')')
                    {
                        contents += str[j].ToString();
                        j++;
                    }

                    var arrayPoint = contents.Split(' ');

                    result.Add(new CadPoint((int)float.Parse(arrayPoint[0]), (int)float.Parse(arrayPoint[1]), (int)float.Parse(arrayPoint[2])));
                }

                i++;
            }


            return result;
        }

        public List<string> ParseOCA()
        {
            List<string> result = new List<string>();

            string fileContens = string.Empty;

            FileInfo fi = new FileInfo( inFileName );

            var fileLen = fi.Length;

            using(var fs = new FileStream(this.inFileName, FileMode.Open, FileAccess.Read))
            {
                value = new byte[fileLen];

                fs.Read(value, 0, (int)fileLen);
            }

            int i = 0;

            while(i < fileLen)
            {
                if (i > fileLen - 3)
                    break;

                if (value[i] == 0x0d && value[i+1] == 0x0a && value[i+2] == 'A' )
                {
                    i += 2;

                    int j = 0;
                    byte[] tmp = new byte[5120];

                    while(value[i + j] < fileLen)
                    {
                        if (value[i + j] == 0x0d && value[i + j + 1] == 0x0d && value[i + j + 2] == 0x0a)
                        {
                            break;
                        }
                        else
                        {
                            tmp[j] = (byte)value[i+j];

                            j++;
                        }

                    }

                    fileContens = Encoding.ASCII.GetString(tmp);

                    result.Add(fileContens);
                }

                i++;
            }


            return result;
        }
    }
}
