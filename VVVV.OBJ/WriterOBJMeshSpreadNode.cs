using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VMath;

using System.IO;
using System.Globalization;

namespace VVVV.OBJ
{
    [PluginInfo(Name = "Writer", Category = "OBJ", Version = "Mesh Advanced", Author = "antokhio", AutoEvaluate = true)]
    public class WriterOBJMeshSpreadNode : IPluginEvaluate
    {
        [Input("Vertex")]
        ISpread<Vector3D> FVert;

        [Input("Normal")]
        ISpread<Vector3D> FNormal;

        [Input("Texcoord")]
        ISpread<Vector2D> FUV;

        [Input("Bin Size")]
        ISpread<int> FInBin;

        [Input("FilePath", StringType = StringType.Filename, IsSingle = true)]
        ISpread<string> FPath;

        [Input("Write", IsSingle = true, IsBang = true)]
        ISpread<bool> FWrite;

        [Output("Sucess", IsBang = true)]
        ISpread<bool> FOutSucess;

        //[Output("Debug")]
        //ISpread<string> FOutDebug;

        public void Evaluate(int SpreadMax)
        {
            FOutSucess[0] = false;

            if (FWrite[0])
            {
                FOutSucess[0] = false;

                StringBuilder sb = new StringBuilder();

                int offset = 0;

                sb.AppendLine("# VVVV Wavefront OBJ Exporter by antokhio");
                sb.AppendLine("# File Created: " + DateTime.Now.ToString());
                sb.AppendLine();

                for (int i = 0; i < FInBin.Count; i++)
                {

                    for (int j = offset; j < offset + FInBin[i]; j++)
                    {
                        sb.Append("v ");
                        sb.Append(FVert[j].x.ToString("0.0000", CultureInfo.InvariantCulture) + " ");
                        sb.Append(FVert[j].y.ToString("0.0000", CultureInfo.InvariantCulture) + " ");
                        sb.Append((-FVert[j].z).ToString("0.0000", CultureInfo.InvariantCulture));
                        sb.AppendLine();
                    }

                    sb.AppendLine();

                    for (int j = offset; j < offset + FInBin[i]; j++)
                    {
                        sb.Append("vn ");
                        sb.Append(FNormal[j].x.ToString("0.0000", CultureInfo.InvariantCulture) + " ");
                        sb.Append(FNormal[j].y.ToString("0.0000", CultureInfo.InvariantCulture) + " ");
                        sb.Append((-FNormal[j].z).ToString("0.0000", CultureInfo.InvariantCulture));
                        sb.AppendLine();
                    }

                    sb.AppendLine();

                    for (int j = offset; j < offset + FInBin[i]; j++)
                    {
                        sb.Append("vt ");
                        sb.Append(FUV[j].x.ToString("0.0000", CultureInfo.InvariantCulture) + " ");
                        sb.Append((1 - FUV[j].y).ToString("0.0000", CultureInfo.InvariantCulture));
                        sb.AppendLine();
                    }

                    sb.AppendLine();

                    sb.AppendLine("o Object" + i.ToString());
                    sb.AppendLine("g Object" + i.ToString());

                    
                    for (int j = offset + 1; j < offset + FInBin[i] + 1; j += 3)
                    {

                        sb.Append("f ");

                        sb.Append((j + 2).ToString(CultureInfo.InvariantCulture) + "/");
                        sb.Append((j + 2).ToString(CultureInfo.InvariantCulture) + "/");
                        sb.Append((j + 2).ToString(CultureInfo.InvariantCulture) + " ");

                        sb.Append((j + 1).ToString(CultureInfo.InvariantCulture) + "/");
                        sb.Append((j + 1).ToString(CultureInfo.InvariantCulture) + "/");
                        sb.Append((j + 1).ToString(CultureInfo.InvariantCulture) + " ");

                        sb.Append(j.ToString(CultureInfo.InvariantCulture) + "/");
                        sb.Append(j.ToString(CultureInfo.InvariantCulture) + "/");
                        sb.Append(j.ToString(CultureInfo.InvariantCulture));

                        sb.AppendLine();
                    }

                    sb.AppendLine();

                    offset += FInBin[i];

                }
                
                try
                {
                    StreamWriter sw = new StreamWriter(FPath[0], false, System.Text.Encoding.UTF8);
                    sw.Write(sb);
                    sw.Close();

                    FOutSucess[0] = true;
                    //FOutDebug[0] = sb.ToString();
                }
                catch (Exception e)
                {
                    FOutSucess[0] = false;
                    //FOutDebug[0] = e.Message;
                }
            }
        }
    }
}

