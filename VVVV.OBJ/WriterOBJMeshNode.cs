//Depricated

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VVVV.PluginInterfaces.V2;
using VVVV.Utils.VMath;

using System.Globalization;


namespace VVVV.OBJ
{
    [PluginInfo(Name = "Writer", Category = "OBJ", Version = "Mesh", Author = "antokhio", AutoEvaluate = true)]
    internal class WriterOBJMeshNode : IPluginEvaluate
    {
        [Input("Vertex")]
        ISpread<Vector3D> FVert;

        [Input("Normal")]
        ISpread<Vector3D> FNormal;

        [Input("Texcoord")]
        ISpread<Vector2D> FUV;

        [Input("FilePath", StringType = StringType.Filename, IsSingle = true)]
        ISpread<string> FPath;

        [Input("Write", IsSingle = true, IsBang = true)]
        ISpread<bool> FWrite;

        [Output("Sucess", IsBang = true)]
        ISpread<bool> FOutSucess;

        [Output("Debug")]
        ISpread<string> FOutDebug;

        public void Evaluate(int SpreadMax)
        { 
            if (FWrite[0] )
            {
                FOutSucess[0] = false;
                
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("o Object1");
                
                foreach (var vert in FVert)
                {

                    sb.Append("v ");
                    sb.Append(vert.x.ToString("0.0000", CultureInfo.InvariantCulture) + " ");
                    sb.Append(vert.y.ToString("0.0000", CultureInfo.InvariantCulture) + " ");
                    sb.Append((-vert.z).ToString("0.0000", CultureInfo.InvariantCulture));
                    sb.AppendLine();
                }

                sb.AppendLine();

                foreach (var norm in FNormal)
                {
                    sb.Append("vn ");
                    sb.Append(norm.x.ToString("0.0000", CultureInfo.InvariantCulture) + " ");
                    sb.Append(norm.y.ToString("0.0000", CultureInfo.InvariantCulture) + " ");
                    sb.Append((-norm.z).ToString("0.0000", CultureInfo.InvariantCulture));
                    sb.AppendLine();
                }

                sb.AppendLine();

                foreach (var uv in FUV)
                {
                    sb.Append("vt ");
                    sb.Append(uv.x.ToString("0.0000", CultureInfo.InvariantCulture) + " ");
                    sb.Append((1-uv.y).ToString("0.0000", CultureInfo.InvariantCulture) + " ");
                    sb.AppendLine();
                }

                sb.AppendLine();

                sb.Append("g Default");
                sb.AppendLine();
                

                

                for (int i = 1; i < FVert.Count() + 1; i+=3)
                {
                   
                    sb.Append("f ");

                    sb.Append((i + 2).ToString(CultureInfo.InvariantCulture) + "/");
                    sb.Append((i + 2).ToString(CultureInfo.InvariantCulture) + "/");
                    sb.Append((i + 2).ToString(CultureInfo.InvariantCulture) + " ");

                    sb.Append((i + 1).ToString(CultureInfo.InvariantCulture) + "/");
                    sb.Append((i + 1).ToString(CultureInfo.InvariantCulture) + "/");
                    sb.Append((i + 1).ToString(CultureInfo.InvariantCulture) + " ");

                    sb.Append(i.ToString(CultureInfo.InvariantCulture) + "/");
                    sb.Append(i.ToString(CultureInfo.InvariantCulture) + "/");
                    sb.Append(i.ToString(CultureInfo.InvariantCulture));

                    sb.AppendLine();
                }

                try
                {
                    StreamWriter sw = new StreamWriter(FPath[0], false, System.Text.Encoding.UTF8);
                    sw.Write(sb);
                    sw.Close();

                    FOutSucess[0] = true;
                    FOutDebug[0] = sb.ToString();
                }
                catch (Exception e)
                {
                    FOutSucess[0] = false;
                    FOutDebug[0] = e.Message;
                }
            }
        }
    }
}
