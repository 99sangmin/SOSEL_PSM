using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;


namespace MRAO
{
    class Program
    {
        static void Main(string[] args1)
        {
            string[] args = new string[2];
            args[0] = "D:\\001Works\\001Research\\002Project\\【SOSEL】\\20251115-yaoqiu\\LT.job";
            args[1] = "D:\\001Works\\001Research\\002Project\\【SOSEL】\\20251115-yaoqiu\\test\\RAO";

            ////args[0] = "D:\\LONGTREM\\EXE\\LTM1.job";
            ////args[1] = "D:\\LONGTREM\\EXE\\TEST1";

            string main = "";
            //string hydro = "";
            string dir = "";
            string abaqus = "";
            int h_n = 0;
            int loc_n = 0;
            string key = "";

            main = args[0];
            dir = args[1];
            double fs_index = 0;

            DirectoryInfo p_rao = new DirectoryInfo(dir);
            if (p_rao.Exists == false)
            {
                p_rao.Create();
            }

            FileStream es = new FileStream(main, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader er = new StreamReader(es);

            string Line_h;

            while (er.Peek() != -1)
            {
                Line_h = er.ReadLine();
                /*if (Line_h.Trim().StartsWith("*AQWA"))
                {
                    Line_h = er.ReadLine();
                    if (Line_h.Trim().StartsWith("*"))
                    {
                        Line_h = er.ReadLine();
                    }
                    hydro = Line_h;
                }*/
                if (Line_h.Trim().StartsWith("*ABAQUS"))
                {
                    Line_h = er.ReadLine();
                    if (Line_h.Trim().StartsWith("*"))
                    {
                        Line_h = er.ReadLine();
                    }
                    abaqus = Line_h;
                }
                if (Line_h.Trim().StartsWith("*DIR"))
                {
                    Line_h = er.ReadLine();
                    if (Line_h.Trim().StartsWith("*"))
                    {
                        Line_h = er.ReadLine();
                    }
                    while (Line_h.Trim().StartsWith("*") == false)
                    {
                        h_n = h_n + 1;
                        Line_h = er.ReadLine();
                    }
                }
                if (Line_h.Trim().StartsWith("*SPD"))
                {
                    Line_h = er.ReadLine();
                    if (Line_h.Trim().StartsWith("*"))
                    {
                        Line_h = er.ReadLine();
                    }
                    fs_index = Convert.ToDouble(Line_h);
                }
                if (Line_h.Trim().StartsWith("*LT"))
                {
                    Line_h = er.ReadLine();
                    while (Line_h.Trim().StartsWith("*") == true)
                    {
                        Line_h = er.ReadLine();
                    }
                    Line_h = er.ReadLine();
                    while (Line_h.Trim().StartsWith("*") == true)
                    {
                        Line_h = er.ReadLine();
                    }
                    string[] loc_s = Line_h.Split(',');
                    key = loc_s[0];
                    loc_n = loc_s.Length - 1;
                }
            }
            er.Close();
            es.Close();

            bool isDat1 = Path.GetExtension(abaqus).Equals(".dat", StringComparison.OrdinalIgnoreCase);

            string[,] DIRE = new string[h_n, 4];
            string[,] FE = new string[h_n, 4];
            double[] LOC = new double[loc_n];

            FileStream LS_arr = new FileStream(main, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader LR_arr = new StreamReader(LS_arr);

            string Line_arr;
            int l = 0;
            double prop = 0;

            while (LR_arr.Peek() != -1)
            {
                Line_arr = LR_arr.ReadLine();
                if (Line_arr.Trim().StartsWith("*DIR"))
                {
                    int d0 = 0;
                    Line_arr = LR_arr.ReadLine();
                    while (Line_arr.Trim().StartsWith("*") == true)
                    {
                        Line_arr = LR_arr.ReadLine();
                    }
                    while (Line_arr.Trim().StartsWith("*") == false)
                    {
                        string[] r_d = Line_arr.Split(',');
                        DIRE[d0, 0] = Convert.ToString(d0 + 1);
                        DIRE[d0, 1] = r_d[0];
                        DIRE[d0, 2] = r_d[1];

                        FE[d0, 0] = Convert.ToString(d0 + 1);
                        FE[d0, 1] = r_d[0];
                        FE[d0, 2] = r_d[1];

                        prop = prop + Convert.ToDouble(DIRE[d0, 2]);

                        /*if ((key != "mom" && key != "MOM") && fs_index == 0)
                        {
                            string lis = hydro.Replace(".dat", ".lis");
                            DIRE[d0, 3] = lis;
                        }*/

                        d0 = d0 + 1;
                        Line_arr = LR_arr.ReadLine();
                    }
                    if (Math.Round(prop,5) - 1.0 != 0)
                    {
                        Console.WriteLine("   Error!!!: The sum of the probabilities of wave headings in JOB file is not equal to 100%");
                        Environment.Exit(0);
                    }
                }
                if ((key != "mom" && key != "MOM"))
                {

                    int d0 = 0;
                    int d1 = 0;
                    //Line_arr = LR_arr.ReadLine();

                    if (Line_arr.Trim().StartsWith("*AQWA"))
                    {
                        while (Line_arr.Trim().StartsWith("*") == true)
                        {
                            Line_arr = LR_arr.ReadLine();
                        }
                        while (Line_arr.Trim().StartsWith("*") == false)
                        {
                            string[] r_d = Line_arr.Split(',');
                            string lis = r_d[0].Replace(".dat", ".lis");
                            DIRE[d0, 3] = lis;

                            d0 = d0 + 1;
                            Line_arr = LR_arr.ReadLine();
                        }
                    }
                    if (Line_arr.Trim().StartsWith("*ABAQUS"))
                    {
                        while (Line_arr.Trim().StartsWith("*") == true)
                        {
                            Line_arr = LR_arr.ReadLine();
                        }
                        while (Line_arr.Trim().StartsWith("*") == false)
                        {
                            FE[d1, 3] = Line_arr;

                            d1 = d1 + 1;
                            Line_arr = LR_arr.ReadLine();
                        }
                    }
                    if (isDat1 == false)
                    {
                        for (int i = 1; i < h_n; i++)
                        {
                            FE[i, 3] = FE[0, 3];
                        }
                    }

                    if (d0 == 1 && fs_index == 0)
                    {
                        for (int i=1; i<h_n;i++)
                        {
                            DIRE[i, 3] = DIRE[0, 3];
                        }
                    }
                    else if (d0 != h_n && fs_index != 0)
                    {
                        Console.WriteLine("   Error!!!: Confirming the required number of AQWA LIS files for W/O ship speed");
                        Environment.Exit(0);
                    }
                    else if (d0 > 1 && fs_index == 0)
                    {
                        Console.WriteLine("   Error!!!: Confirming the required number of AQWA LIS files for W/O ship speed");
                        Environment.Exit(0);
                    }
                }

                if ((key == "mom" || key == "MOM") && Line_arr.Trim().StartsWith("*WFM"))
                {
                    int d0 = 0;
                    Line_arr = LR_arr.ReadLine();
                    while (Line_arr.Trim().StartsWith("*") == true)
                    {
                        Line_arr = LR_arr.ReadLine();
                    }
                    while (Line_arr.Trim().StartsWith("*") == false)
                    {
                        string[] r_d = Line_arr.Split(',');
                        DIRE[d0, 3] = r_d[0];

                        //if ((key != "mom" && key != "MOM") && fs_index == 0)
                        //{
                        //    string lis = hydro.Replace(".dat", ".lis");
                        //    DIRE[d0, 3] = lis;
                        //}

                        d0 = d0 + 1;
                        Line_arr = LR_arr.ReadLine();
                    }
                }
                if (Line_arr.Trim().StartsWith("*LT") && (key == "mom" || key == "MOM"))
                {
                    Line_arr = LR_arr.ReadLine();
                    while (Line_arr.Trim().StartsWith("*") == true)
                    {
                        Line_arr = LR_arr.ReadLine();
                    }
                    Line_arr = LR_arr.ReadLine();
                    while (Line_arr.Trim().StartsWith("*") == true)
                    {
                        Line_arr = LR_arr.ReadLine();
                    }
                    //string[] l_d = System.Text.RegularExpressions.Regex.Split(Line_arr, @"\s+");
                    string[] l_d = Line_arr.Split(',');

                    for (int i = 0; i < loc_n; i++)
                    {
                        if (l_d[i + 1] == "ALL" || l_d[i + 1] == "all")
                        {
                            LOC[l] = 1000000;
                        }
                        else
                        {
                            LOC[l] = Convert.ToDouble(l_d[i + 1]);
                        }
                        l = l + 1;
                    }
                }
            }
            LS_arr.Close();
            LR_arr.Close();

            FileStream LS_f = new FileStream(DIRE[0, 3], FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader LR_f = new StreamReader(LS_f);

            string L_f;
            int fre = 0;
            int fwd = 0;

            while (LR_f.Peek() != -1)
            {
                L_f = LR_f.ReadLine();
                if (L_f.Trim().StartsWith("DECK  6.1"))
                {
                    L_f = LR_f.ReadLine();
                    L_f = LR_f.ReadLine();
                    while (L_f.Trim().StartsWith("END") == false)
                    {
                        L_f = LR_f.ReadLine();
                        string[] r_f = Regex.Split(L_f, @"\s+");
                        if (Regex.Replace(r_f[1], @"\d+", "") == "HRTZ" || Regex.Replace(r_f[1], @"\d+", "") == "FREQ" || Regex.Replace(r_f[1], @"\d+", "") == "PERD")
                        {
                            if (int.Parse(r_f[3]) > fre)
                            {
                                fre = int.Parse(r_f[3]);
                            }
                        }
                        else if (Regex.Replace(r_f[1], @"\d+", "") == "FWDS")
                        {
                            if((Convert.ToDouble(r_f[1])-fs_index)!=0)
                            {
                                Console.WriteLine("   Error!!!: The Ship speed in the JOB file is inconsistent with that in the LIS file.");
                                Environment.Exit(0);
                            }
                            fwd = 1;
                        }
                    }
                    if (fwd == 0 && fs_index != 0)
                    {
                        Console.WriteLine("   Error!!!: The Ship speed in the JOB file is inconsistent with that in the LIS file.");
                        Environment.Exit(0);
                    }
                }
            }
            LS_f.Close();
            LR_f.Close();

            double[] a_f = new double[fre];

            FileStream LS_a = new FileStream(DIRE[0, 3], FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader LR_a = new StreamReader(LS_a);

            string L_a;
            int f = 0;
            int d = 0;

            while (LR_a.Peek() != -1)
            {
                L_a = LR_a.ReadLine();
                if (L_a.Trim().StartsWith("DECK  6.1"))
                {
                    L_a = LR_a.ReadLine();
                    L_a = LR_a.ReadLine();
                    while (L_a.Trim().StartsWith("END") == false)
                    {
                        L_a = LR_a.ReadLine();
                        string[] read_f_d = System.Text.RegularExpressions.Regex.Split(L_a, @"\s+");
                        if (Regex.Replace(read_f_d[1], @"\d+", "") == "HRTZ")
                        {
                            a_f[f] = Convert.ToDouble(read_f_d[4]) * 2 * Math.PI;
                            f = f + 1;
                        }
                        else if (Regex.Replace(read_f_d[1], @"\d+", "") == "FREQ")
                        {
                            a_f[f] = Convert.ToDouble(read_f_d[4]);
                            f = f + 1;
                        }
                        else if (Regex.Replace(read_f_d[1], @"\d+", "") == "PERD")
                        {
                            a_f[f] = (2 * Math.PI) / Convert.ToDouble(read_f_d[4]);
                            f = f + 1;
                        }
                        else if (Regex.Replace(read_f_d[1], @"\d+", "") == "DIRN")
                        {
                            /*double pan = Convert.ToDouble(read_f_d[4]) - Convert.ToDouble(DIRE[d, 1]);
                            if (pan!=0)
                            {
                                Console.WriteLine("   Error!!!: The wave headings in the JOB file is inconsistent with those in the LIS file.");
                                Environment.Exit(0);
                            }*/
                            d = d + 1;
                        }
                    }
                    if (d != h_n)
                    {
                        Console.WriteLine("   Error!!!: The wave headings in the JOB file is inconsistent with those in the LIS file.");
                        Environment.Exit(0);
                    }
                }
            }
            LS_a.Close();
            LR_a.Close();


            if (key != "mom" && key != "MOM")
            {

                if (key == "S" || key == "U" || key == "s" || key == "u")
                {

                    string file_df = "";
                    if (isDat1 == false)
                    {
                        file_df = Path.Combine(abaqus + "\\FE_D001" + ".dat");
                        bool fileExists = File.Exists(file_df);

                        if (fileExists == false)
                        {
                            Console.WriteLine("   Error!!!: No FE_001.dat file.");
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        file_df = abaqus;
                        bool fileExists = File.Exists(file_df);
                        if (fileExists == false)
                        {
                            Console.WriteLine("   Error!!!: No FE_001.dat file.");
                            Environment.Exit(0);
                        }
                    }

                    FileStream PS_e = new FileStream(file_df, FileMode.Open, FileAccess.Read, FileShare.Read);
                    StreamReader PR_e = new StreamReader(PS_e);

                    string Line_en;

                    int node_n = 0;
                    int ele_n = 0;

                    while (PR_e.Peek() != -1)
                    {
                        Line_en = PR_e.ReadLine();
                        if (Line_en.Trim().StartsWith("PT NOTE"))
                        {
                            Line_en = PR_e.ReadLine();
                            Line_en = PR_e.ReadLine();
                            while (Line_en != string.Empty)
                            {
                                ele_n = ele_n + 1;
                                Line_en = PR_e.ReadLine();
                            }
                        }
                        else if (Line_en.Trim().StartsWith("NODE FOOT"))
                        {
                            Line_en = PR_e.ReadLine();
                            Line_en = PR_e.ReadLine();
                            Line_en = PR_e.ReadLine();
                            while (Line_en != string.Empty)
                            {
                                node_n = node_n + 1;
                                Line_en = PR_e.ReadLine();
                            }
                            break;
                        }
                    }
                    PS_e.Close();
                    PR_e.Close();

                    ele_n = ele_n / 4;
                    node_n = node_n / 2;

                    for (int peri_n = 0; peri_n < h_n; peri_n++)
                    {
                        string path_panel = "";
                        string na = "";
                        string na_1 = "";
                        string DIR = Convert.ToString(peri_n + 1);
                        double[,] sel_e = new double[ele_n * fre, 15];


                        if (isDat1 == false)
                        {
                            if (DIR.Length == 1)
                            {
                                na = ("FE_D00" + DIR);
                                na_1 = ("D00" + DIR);
                                path_panel = Path.Combine(abaqus + "\\" + na + ".dat");
                            }
                            else if (DIR.Length == 2)
                            {
                                na = ("FE_D0" + DIR);
                                na_1 = ("D0" + DIR);
                                path_panel = Path.Combine(abaqus + "\\" + na + ".dat");
                            }
                            else if (DIR.Length == 3)
                            {
                                na = ("FE_D" + DIR);
                                na_1 = ("D" + DIR);
                                path_panel = Path.Combine(abaqus + "\\" + na + ".dat");
                            }
                        }
                        else
                        {
                            if (DIR.Length == 1)
                            {
                                na = ("FE_D00" + DIR);
                                na_1 = ("D00" + DIR);
                                path_panel = FE[peri_n, 3];
                            }
                            else if (DIR.Length == 2)
                            {
                                na = ("FE_D0" + DIR);
                                na_1 = ("D0" + DIR);
                                path_panel = FE[peri_n, 3];
                            }
                            else if (DIR.Length == 3)
                            {
                                na = ("FE_D" + DIR);
                                na_1 = ("D" + DIR);
                                path_panel = FE[peri_n, 3];
                            }
                            if (path_panel == null)
                            {
                                Console.WriteLine("   Error!!!: The wave headings in the JOB file is inconsistent with those of ABAQUS file.");
                                Environment.Exit(0);
                            }
                        }

                        bool fileExists = File.Exists(path_panel);
                        if (fileExists == false)
                        {
                            Console.WriteLine("   Error!!!: No File of {0}", na + ".dat");
                            Environment.Exit(0);
                        }

                        FileStream LS = new FileStream(path_panel, FileMode.Open, FileAccess.Read, FileShare.Read);
                        StreamReader LR = new StreamReader(LS);

                        string LLine;

                        if (key == "S" || key == "s")
                        {
                            int se_e = 0;
                            for (int fre_n = 0; fre_n < fre; fre_n++)
                            {
                                LLine = LR.ReadLine();
                                LLine = LR.ReadLine();
                                string[] step = step = System.Text.RegularExpressions.Regex.Split(LLine, @"\s+");
                                while (step[0] != "1")
                                {
                                    if (LLine.Trim().StartsWith("PT NOTE"))
                                    {
                                        LLine = LR.ReadLine();
                                        LLine = LR.ReadLine();
                                        while (LLine != "")
                                        {
                                            string[] Stress = System.Text.RegularExpressions.Regex.Split(LLine, @"\s+");
                                            sel_e[se_e, 0] = Convert.ToDouble(Stress[1]);
                                            if (Stress.Length == 6)
                                            {
                                                sel_e[se_e, 4] = Convert.ToDouble(Stress[3]);
                                                sel_e[se_e, 5] = Convert.ToDouble(Stress[4]);
                                                sel_e[se_e, 6] = Convert.ToDouble(Stress[5]);
                                            }
                                            else if (Stress.Length == 7)
                                            {
                                                sel_e[se_e, 4] = Convert.ToDouble(Stress[4]);
                                                sel_e[se_e, 5] = Convert.ToDouble(Stress[5]);
                                                sel_e[se_e, 6] = Convert.ToDouble(Stress[6]);
                                            }
                                            LLine = LR.ReadLine();
                                            string[] Stress_1 = System.Text.RegularExpressions.Regex.Split(LLine, @"\s+");
                                            sel_e[se_e, 10] = Convert.ToDouble(Stress_1[4]);
                                            sel_e[se_e, 11] = Convert.ToDouble(Stress_1[5]);
                                            sel_e[se_e, 12] = Convert.ToDouble(Stress_1[6]);
                                            LLine = LR.ReadLine();
                                            string[] Stress_2 = System.Text.RegularExpressions.Regex.Split(LLine, @"\s+");
                                            if (Stress_2.Length == 6)
                                            {
                                                sel_e[se_e, 1] = Convert.ToDouble(Stress_2[3]);
                                                sel_e[se_e, 2] = Convert.ToDouble(Stress_2[4]);
                                                sel_e[se_e, 3] = Convert.ToDouble(Stress_2[5]);
                                            }
                                            else if (Stress_2.Length == 7)
                                            {
                                                sel_e[se_e, 1] = Convert.ToDouble(Stress_2[4]);
                                                sel_e[se_e, 2] = Convert.ToDouble(Stress_2[5]);
                                                sel_e[se_e, 3] = Convert.ToDouble(Stress_2[6]);
                                            }
                                            LLine = LR.ReadLine();
                                            string[] Stress_3 = System.Text.RegularExpressions.Regex.Split(LLine, @"\s+");
                                            sel_e[se_e, 7] = Convert.ToDouble(Stress_3[4]);
                                            sel_e[se_e, 8] = Convert.ToDouble(Stress_3[5]);
                                            sel_e[se_e, 9] = Convert.ToDouble(Stress_3[6]);

                                            double s11_t = Math.Sqrt(Math.Pow((Convert.ToDouble(sel_e[se_e, 1])), 2) + Math.Pow((Convert.ToDouble(sel_e[se_e, 7])), 2));
                                            double s22_t = Math.Sqrt(Math.Pow((Convert.ToDouble(sel_e[se_e, 2])), 2) + Math.Pow((Convert.ToDouble(sel_e[se_e, 8])), 2));
                                            double s12_t = Math.Sqrt(Math.Pow((Convert.ToDouble(sel_e[se_e, 3])), 2) + Math.Pow((Convert.ToDouble(sel_e[se_e, 9])), 2));

                                            double s11_b = Math.Sqrt(Math.Pow((Convert.ToDouble(sel_e[se_e, 4])), 2) + Math.Pow((Convert.ToDouble(sel_e[se_e, 10])), 2));
                                            double s22_b = Math.Sqrt(Math.Pow((Convert.ToDouble(sel_e[se_e, 5])), 2) + Math.Pow((Convert.ToDouble(sel_e[se_e, 11])), 2));
                                            double s12_b = Math.Sqrt(Math.Pow((Convert.ToDouble(sel_e[se_e, 6])), 2) + Math.Pow((Convert.ToDouble(sel_e[se_e, 12])), 2));

                                            sel_e[se_e, 13] = Math.Sqrt(Math.Pow(s11_t, 2) - s11_t * s22_t + Math.Pow(s22_t, 2) + 3 * Math.Pow(s12_t, 2));
                                            sel_e[se_e, 14] = Math.Sqrt(Math.Pow(s11_b, 2) - s11_b * s22_b + Math.Pow(s22_b, 2) + 3 * Math.Pow(s12_b, 2));

                                            se_e = se_e + 1;
                                            LLine = LR.ReadLine();
                                        }
                                    }
                                    LLine = LR.ReadLine();
                                    if (LLine.Trim().StartsWith("WALLCLOCK"))
                                    {
                                        LLine = LR.ReadLine();
                                        if (LLine == null)
                                        {
                                            break;
                                        }
                                        else if (LLine.Trim().StartsWith("1"))
                                        {
                                            LLine = LR.ReadLine();
                                        }
                                    }
                                    step = System.Text.RegularExpressions.Regex.Split(LLine, @"\s+");
                                }
                            }

                            string path_save_s = Path.Combine(dir + "\\" + na_1 + ".rao");

                            FileStream ws_s = new FileStream(path_save_s, FileMode.Create, FileAccess.Write, FileShare.Write);
                            StreamWriter wr_s = new StreamWriter(ws_s);

                            wr_s.WriteLine("*Heading (deg)\n{0,14:0.00}\n", DIRE[peri_n, 1]);
                            wr_s.WriteLine("*Stress RAOs\n**      Ele_ID,    FRE(rad/s),    S11-R (Z1),    S22-R (Z1),    S12-R (Z1),    S11-R (Z2),    S22-R (Z2),    S12-R (Z2),    S11-I (Z1),    S22-I (Z1),    S12-I (Z1),    S11-I (Z2),    S22-I (Z2),    S12-I (Z2),Von-Mises (Z1),Von-Mises (Z2)");

                            int a_e = 0;

                            for (int e = 0; e < ele_n; e++)
                            {
                                for (int f0 = 0; f0 < fre; f0++)
                                {
                                    a_e = e + ele_n * f0;

                                    wr_s.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0},{3,14:0.000000e0},{4,14:0.000000e0},{5,14:0.000000e0},{6,14:0.000000e0},{7,14:0.000000e0},{8,14:0.000000e0},{9,14:0.000000e0},{10,14:0.000000e0},{11,14:0.000000e0},{12,14:0.000000e0},{13,14:0.000000e0},{14,14:0.000000e0},{15,14:0.000000e0}", sel_e[a_e, 0], a_f[f0], sel_e[a_e, 1], sel_e[a_e, 2], sel_e[a_e, 3], sel_e[a_e, 4], sel_e[a_e, 5], sel_e[a_e, 6], sel_e[a_e, 7], sel_e[a_e, 8], sel_e[a_e, 9], sel_e[a_e, 10], sel_e[a_e, 11], sel_e[a_e, 12], sel_e[a_e, 13], sel_e[a_e, 14]);
                                }
                            }
                            wr_s.WriteLine("*END\n");
                            wr_s.Close();
                            ws_s.Close();
                        }
                        else if (key == "U" || key == "u")
                        {

                        }
                        LR.Close();
                        LS.Close();
                    }
                }
                //else if (key == "acc" || key == "mot" || key == "ACC" || key == "MOT" || key == "FK" || key == "fk" || key == "DIFF" || key == "diff" || key == "FK+DIFF" || key == "fk+diff")
                else if (key == "HYDRO" || key == "hydro")
                {

                    for (int peri_n = 0; peri_n < h_n; peri_n++)
                    {
                        string savepath = "";
                        string strDIR = Convert.ToString(peri_n + 1);

                        if (strDIR.Length == 1)
                        {
                            savepath = Path.Combine(dir + "\\" + "D00" + strDIR + ".rao");
                        }
                        else if (strDIR.Length == 2)
                        {
                            savepath = Path.Combine(dir + "\\" + "D0" + strDIR + ".rao");
                        }
                        else if (strDIR.Length == 3)
                        {
                            savepath = Path.Combine(dir + "\\" + "D" + strDIR + ".rao");
                        }

                        FileStream ws = new FileStream(savepath, FileMode.Create, FileAccess.Write, FileShare.Write);
                        StreamWriter wr = new StreamWriter(ws);

                        //if (key == "acc" || key == "ACC")
                        //{
                            double[,] ACC = new double[fre, 6];

                            wr.WriteLine("*Heading (deg)\n{0,14:0.00}\n", DIRE[peri_n, 1]);
                            //wr.WriteLine("*Acceleration RAO\n**         DOF,   Freq(rad/s),  Acceleration");
                            wr.WriteLine("*Response Amplitude Operators RAO\n**         DOF,   Freq(rad/s),  Acceleration,        Motion,     F-K force,    DIFF force, FK+DIFF force");

                            FileStream LSacc = new FileStream(DIRE[peri_n, 3], FileMode.Open, FileAccess.Read, FileShare.Read);
                            StreamReader LRacc = new StreamReader(LSacc);

                            string LLine;

                            while (LRacc.Peek() != -1)
                            {
                                LLine = LRacc.ReadLine();
                                if (LLine.Trim().StartsWith("ACC R.A.O.S-VARIATION WITH WAVE PERIOD/FREQUENCY"))
                                {
                                    while (LLine.Trim().StartsWith("1") == false)
                                    {
                                        LLine = LRacc.ReadLine();
                                        if (LLine != "")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            LLine = LRacc.ReadLine();
                                            string a = LLine.Substring(16, 10).Trim();
                                            double b = Convert.ToDouble(a);
                                            double c = Convert.ToDouble(DIRE[peri_n, 1]);
                                            if (b == c)
                                            {
                                                for (int pf = 0; pf < fre; pf++)
                                                {
                                                    if (pf == 0)
                                                    {
                                                        string[] acc = System.Text.RegularExpressions.Regex.Split(LLine, @"\s+");
                                                        for (int i = 0; i < 6; i++)
                                                        {
                                                            ACC[pf, i] = Convert.ToDouble(acc[i * 2 + 4]);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string[] acc = System.Text.RegularExpressions.Regex.Split(LLine, @"\s+");
                                                        for (int i = 0; i < 6; i++)
                                                        {
                                                            ACC[pf, i] = Convert.ToDouble(acc[i * 2 + 3]);
                                                        }
                                                    }
                                                    LLine = LRacc.ReadLine();
                                                }
                                            }
                                            else if (b != c)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            LSacc.Close();
                            LRacc.Close();

                            /*for (int dof = 0; dof < 6; dof++)
                            {
                                for (int fre_n = 0; fre_n < fre; fre_n++)
                                {
                                    wr.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0}", dof + 1, a_f[fre_n], ACC[fre_n, dof]);
                                }
                            }
                            wr.WriteLine("*END\n\n");*/
                        //}
                        //else if (key == "mot" || key == "MOT")
                        //{
                            double[,] Motion = new double[fre, 6];

                            //wr.WriteLine("*Heading (deg)\n{0,14:0.00}\n", DIRE[peri_n, 1]);
                            //wr.WriteLine("*Motion RAO\n**         DOF,   Freq(rad/s),        Motion");

                            FileStream LSmot = new FileStream(DIRE[peri_n, 3], FileMode.Open, FileAccess.Read, FileShare.Read);
                            StreamReader LRmot = new StreamReader(LSmot);

                            string LLinemot;

                            while (LRmot.Peek() != -1)
                            {
                                LLinemot = LRmot.ReadLine();
                                if (LLinemot.Trim().StartsWith("R.A.O.S-VARIATION WITH WAVE PERIOD/FREQUENCY"))
                                {
                                    while (LLinemot.Trim().StartsWith("1") == false)
                                    {
                                        LLinemot = LRmot.ReadLine();
                                        if (LLinemot != "")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            LLinemot = LRmot.ReadLine();
                                            string a = LLinemot.Substring(16, 10).Trim();
                                            double b = Convert.ToDouble(a);
                                            double c = Convert.ToDouble(DIRE[peri_n, 1]);
                                            if (b == c)
                                            {
                                                for (int pf = 0; pf < fre; pf++)
                                                {
                                                    if (pf == 0)
                                                    {
                                                        string[] mot = System.Text.RegularExpressions.Regex.Split(LLinemot, @"\s+");
                                                        for (int i = 0; i < 6; i++)
                                                        {
                                                            Motion[pf, i] = Convert.ToDouble(mot[i * 2 + 4]);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string[] mot = System.Text.RegularExpressions.Regex.Split(LLinemot, @"\s+");
                                                        for (int i = 0; i < 6; i++)
                                                        {
                                                            Motion[pf, i] = Convert.ToDouble(mot[i * 2 + 3]);
                                                        }
                                                    }
                                                    LLinemot = LRmot.ReadLine();
                                                }
                                            }
                                            else if (b != c)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            LSmot.Close();
                            LRmot.Close();

                            /*for (int dof = 0; dof < 6; dof++)
                            {
                                for (int fre_n = 0; fre_n < fre; fre_n++)
                                {
                                    wr.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0}", dof + 1, a_f[fre_n], Motion[fre_n, dof]);
                                }
                            }
                            wr.WriteLine("*END\n\n");*/
                        //}
                        //else if (key == "fk" || key == "FK")
                        //{
                            double[,] FKforce = new double[fre, 6];

                            //wr.WriteLine("*Heading (deg)\n{0,14:0.00}\n", DIRE[peri_n, 1]);
                            //wr.WriteLine("*F-K forces RAO\n**         DOF,   Freq(rad/s),     F-K force");

                            FileStream LSfk = new FileStream(DIRE[peri_n, 3], FileMode.Open, FileAccess.Read, FileShare.Read);
                            StreamReader LRfk = new StreamReader(LSfk);

                            string LLinefk;

                            while (LRfk.Peek() != -1)
                            {
                                LLinefk = LRfk.ReadLine();
                                if (LLinefk.Trim().StartsWith("FROUDE KRYLOV FORCES-VARIATION WITH WAVE PERIOD/FREQUENCY"))
                                {
                                    while (LLinefk.Trim().StartsWith("1") == false)
                                    {
                                        LLinefk = LRfk.ReadLine();
                                        if (LLinefk != "")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            LLinefk = LRfk.ReadLine();
                                            string a = LLinefk.Substring(16, 10).Trim();
                                            double b = Convert.ToDouble(a);
                                            double c = Convert.ToDouble(DIRE[peri_n, 1]);
                                            if (b == c)
                                            {
                                                for (int pf = 0; pf < fre; pf++)
                                                {
                                                    if (pf == 0)
                                                    {
                                                        string[] fk = System.Text.RegularExpressions.Regex.Split(LLinefk, @"\s+");
                                                        for (int i = 0; i < 6; i++)
                                                        {
                                                            FKforce[pf, i] = Convert.ToDouble(fk[i * 2 + 4]);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string[] fk = System.Text.RegularExpressions.Regex.Split(LLinefk, @"\s+");
                                                        for (int i = 0; i < 6; i++)
                                                        {
                                                            FKforce[pf, i] = Convert.ToDouble(fk[i * 2 + 3]);
                                                        }
                                                    }
                                                    LLinefk = LRfk.ReadLine();
                                                }
                                            }
                                            else if (b != c)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            LSfk.Close();
                            LRfk.Close();

                            /*for (int dof = 0; dof < 6; dof++)
                            {
                                for (int fre_n = 0; fre_n < fre; fre_n++)
                                {
                                    wr.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0}", dof + 1, a_f[fre_n], FKforce[fre_n, dof]);
                                }
                            }
                            wr.WriteLine("*END\n\n");*/
                        //}
                        //else if (key == "diff" || key == "DIFF")
                        //{
                            double[,] DIFFforce = new double[fre, 6];

                            //wr.WriteLine("*Heading (deg)\n{0,14:0.00}\n", DIRE[peri_n, 1]);
                            //wr.WriteLine("*Diffraction forces RAO\n**         DOF,   Freq(rad/s),    DIFF force");

                            FileStream LSdiff = new FileStream(DIRE[peri_n, 3], FileMode.Open, FileAccess.Read, FileShare.Read);
                            StreamReader LRdiff = new StreamReader(LSdiff);

                            string LLinediff;

                            while (LRdiff.Peek() != -1)
                            {
                                LLinediff = LRdiff.ReadLine();
                                if (LLinediff.Trim().StartsWith("DIFFRACTION FORCES-VARIATION WITH WAVE PERIOD/FREQUENCY"))
                                {
                                    while (LLinediff.Trim().StartsWith("1") == false)
                                    {
                                        LLinediff = LRdiff.ReadLine();
                                        if (LLinediff != "")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            LLinediff = LRdiff.ReadLine();
                                            string a = LLinediff.Substring(16, 10).Trim();
                                            double b = Convert.ToDouble(a);
                                            double c = Convert.ToDouble(DIRE[peri_n, 1]);
                                            if (b == c)
                                            {
                                                for (int pf = 0; pf < fre; pf++)
                                                {
                                                    if (pf == 0)
                                                    {
                                                        string[] diff = System.Text.RegularExpressions.Regex.Split(LLinediff, @"\s+");
                                                        for (int i = 0; i < 6; i++)
                                                        {
                                                            DIFFforce[pf, i] = Convert.ToDouble(diff[i * 2 + 4]);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string[] diff = System.Text.RegularExpressions.Regex.Split(LLinediff, @"\s+");
                                                        for (int i = 0; i < 6; i++)
                                                        {
                                                            DIFFforce[pf, i] = Convert.ToDouble(diff[i * 2 + 3]);
                                                        }
                                                    }
                                                    LLinediff = LRdiff.ReadLine();
                                                }
                                            }
                                            else if (b != c)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            LSdiff.Close();
                            LRdiff.Close();

                            /*for (int dof = 0; dof < 6; dof++)
                            {
                                for (int fre_n = 0; fre_n < fre; fre_n++)
                                {
                                    wr.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0}", dof + 1, a_f[fre_n], DIFFforce[fre_n, dof]);
                                }
                            }
                            wr.WriteLine("*END\n\n");*/
                        //}
                        //else if (key == "fk+diff" || key == "FK+DIFF")
                        //{
                            double[,] fkDIFFforce = new double[fre, 6];

                            //wr.WriteLine("*Heading (deg)\n{0,14:0.00}\n", DIRE[peri_n, 1]);
                            //wr.WriteLine("*FK+Diffraction forces RAO\n**         DOF,   Freq(rad/s), FK+DIFF force");

                            FileStream LSfkdiff = new FileStream(DIRE[peri_n, 3], FileMode.Open, FileAccess.Read, FileShare.Read);
                            StreamReader LRfkdiff = new StreamReader(LSfkdiff);

                            string LLinefkdiff;

                            while (LRfkdiff.Peek() != -1)
                            {
                                LLinefkdiff = LRfkdiff.ReadLine();
                                if (LLinefkdiff.Trim().StartsWith("FROUDE KRYLOV + DIFFRACTION FORCES-VARIATION WITH WAVE PERIOD/FREQUENCY"))
                                {
                                    while (LLinefkdiff.Trim().StartsWith("1") == false)
                                    {
                                        LLinefkdiff = LRfkdiff.ReadLine();
                                        if (LLinefkdiff != "")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            LLinefkdiff = LRfkdiff.ReadLine();
                                            string a = LLinefkdiff.Substring(16, 10).Trim();
                                            double b = Convert.ToDouble(a);
                                            double c = Convert.ToDouble(DIRE[peri_n, 1]);
                                            if (b == c)
                                            {
                                                for (int pf = 0; pf < fre; pf++)
                                                {
                                                    if (pf == 0)
                                                    {
                                                        string[] diff = System.Text.RegularExpressions.Regex.Split(LLinefkdiff, @"\s+");
                                                        for (int i = 0; i < 6; i++)
                                                        {
                                                            fkDIFFforce[pf, i] = Convert.ToDouble(diff[i * 2 + 4]);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string[] diff = System.Text.RegularExpressions.Regex.Split(LLinefkdiff, @"\s+");
                                                        for (int i = 0; i < 6; i++)
                                                        {
                                                            fkDIFFforce[pf, i] = Convert.ToDouble(diff[i * 2 + 3]);
                                                        }
                                                    }
                                                    LLinefkdiff = LRfkdiff.ReadLine();
                                                }
                                            }
                                            else if (b != c)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            LSfkdiff.Close();
                            LRfkdiff.Close();

                            for (int dof = 0; dof < 6; dof++)
                            {
                                for (int fre_n = 0; fre_n < fre; fre_n++)
                                {
                                    wr.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0},{3,14:0.000000e0},{4,14:0.000000e0},{5,14:0.000000e0},{6,14:0.000000e0}", dof + 1, a_f[fre_n], ACC[fre_n, dof], Motion[fre_n, dof], FKforce[fre_n, dof], DIFFforce[fre_n, dof], fkDIFFforce[fre_n, dof]);
                                }
                            }
                            wr.WriteLine("*END\n\n");
                        //}
                        wr.Close();
                        ws.Close();
                    }
                }
            }
            else if (key == "mom" || key == "MOM")
            {

                int f_n = 0;
                int msd_n = 0;

                FileStream es_f = new FileStream(DIRE[0, 3], FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader er_f = new StreamReader(es_f);

                string Line_f;

                while (er_f.Peek() != -1)
                {
                    Line_f = er_f.ReadLine();
                    if (Line_f.Trim().StartsWith("At"))
                    {
                        string[] l_msd = Line_f.Split('=');
                        msd_n = int.Parse(l_msd[1]);
                    }
                    else if (Line_f.Trim().StartsWith("Shear force"))
                    {
                        Line_f = er_f.ReadLine();
                        string[] l_f = new string[4];
                        string st = "";
                        while (l_f[1] != st)
                        {
                            if (f_n == 0)
                            {
                                l_f = System.Text.RegularExpressions.Regex.Split(Line_f, @"\s+");
                                st = l_f[1];
                            }
                            f_n = f_n + 1;
                            Line_f = er_f.ReadLine();
                            l_f = System.Text.RegularExpressions.Regex.Split(Line_f, @"\s+");
                        }
                    }
                }
                er_f.Close();
                es_f.Close();

                double[,] M_RAO = new double[f_n + 1, msd_n];
                double[,] V_RAO = new double[f_n + 1, msd_n];

                int f_n1 = 1;

                FileStream es_f1 = new FileStream(DIRE[0, 3], FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader er_f1 = new StreamReader(es_f1);

                string Line_f1;

                while (er_f1.Peek() != -1)
                {
                    Line_f1 = er_f1.ReadLine();
                    if (Line_f1.Trim().StartsWith("Shear force"))
                    {
                        Line_f1 = er_f1.ReadLine();
                        string[] l_f1 = new string[4];
                        string st1 = "";
                        while (l_f1[1] != st1)
                        {
                            if (f_n1 == 1)
                            {
                                l_f1 = System.Text.RegularExpressions.Regex.Split(Line_f1, @"\s+");
                                st1 = l_f1[1];
                            }
                            M_RAO[f_n1, 0] = Convert.ToDouble(l_f1[1]);
                            V_RAO[f_n1, 0] = Convert.ToDouble(l_f1[1]);
                            f_n1 = f_n1 + 1;
                            Line_f1 = er_f1.ReadLine();
                            l_f1 = System.Text.RegularExpressions.Regex.Split(Line_f1, @"\s+");
                        }
                    }
                }
                er_f1.Close();
                es_f1.Close();

                for (int dire = 0; dire < h_n; dire++)
                {

                    string na = "";

                    if (Convert.ToString(dire + 1).Length == 1)
                    {
                        na = ("D00" + Convert.ToString(dire + 1));
                    }
                    else if (Convert.ToString(dire + 1).Length == 2)
                    {
                        na = ("D0" + Convert.ToString(dire + 1));
                    }
                    else if (Convert.ToString(dire + 1).Length == 3)
                    {
                        na = ("D" + Convert.ToString(dire + 1));
                    }

                    string path_write = Path.Combine(p_rao + "\\" + na + ".rao");

                    FileStream ps = new FileStream(path_write, FileMode.Create, FileAccess.Write, FileShare.Write);
                    StreamWriter pr = new StreamWriter(ps);

                    pr.WriteLine("*Heading (deg)\n{0,14:0.00}\n", DIRE[dire, 1]);
                    pr.WriteLine("*Wave bending moment RAO\n**     Station,   Freq(rad/s),   Shear force,        Moment");

                    if (!File.Exists(DIRE[dire, 3]))
                    {
                        continue;
                    }

                    for (int loc_1 = 0; loc_1 < loc_n; loc_1++)
                    {
                        int f_n2 = 0;
                        int f_n3 = 0;

                        FileStream PS_rao = new FileStream(DIRE[dire, 3], FileMode.Open, FileAccess.Read, FileShare.Read);
                        StreamReader PR_rao = new StreamReader(PS_rao);

                        string Line_m;

                        while (PR_rao.Peek() != -1)
                        {
                            Line_m = PR_rao.ReadLine();
                            if (Line_m.Trim().StartsWith("Shear force"))
                            {
                                while (Line_m.Length != 0)
                                {
                                    Line_m = PR_rao.ReadLine();
                                    f_n2 = f_n2 + 1;
                                    if (f_n2 == f_n)
                                    {
                                        f_n2 = 0;
                                        f_n3 = f_n3 + 1;
                                    }

                                    if (f_n3 == LOC[loc_1])
                                    {
                                        int f_s = 1;
                                        while (f_s < f_n + 1)
                                        {
                                            Line_m = PR_rao.ReadLine();
                                            string[] f_1 = System.Text.RegularExpressions.Regex.Split(Line_m, @"\s+");
                                            M_RAO[f_s, Convert.ToInt16(LOC[loc_1])] = Convert.ToDouble(f_1[3]);
                                            V_RAO[f_s, Convert.ToInt16(LOC[loc_1])] = Convert.ToDouble(f_1[2]);
                                            f_s = f_s + 1;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        PR_rao.Close();
                        PS_rao.Close();

                        for (int fre_n = 1; fre_n < f_n+1; fre_n++)
                        {
                            pr.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0},{3,14:0.000000e0}", Convert.ToInt16(LOC[loc_1]), M_RAO[fre_n, 0], V_RAO[fre_n, Convert.ToInt16(LOC[loc_1])], M_RAO[fre_n, Convert.ToInt16(LOC[loc_1])]);
                        }
                    }
                    pr.WriteLine("*END");
                    pr.Close();
                    ps.Close();
                }
            }
        }
    }
}
