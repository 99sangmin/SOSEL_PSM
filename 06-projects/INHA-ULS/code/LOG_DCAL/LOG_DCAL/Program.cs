using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using MathNet.Numerics.LinearRegression;

namespace MCAL
{
    class Program
    {
        static void Main(string[] args1)
        {
            string[] args = new string[4];
            args[0] = "D:\\001Works\\001Research\\002Project\\【SOSEL】\\20251115-yaoqiu\\LTs.job";
            args[1] = "D:\\001Works\\001Research\\002Project\\【SOSEL】\\20251115-yaoqiu\\MOM2";
            args[2] = "D:\\001Works\\001Research\\002Project\\【SOSEL】\\20251115-yaoqiu\\LT2";
            args[3] = "0.001";

            string path_main = "";
            string path_inf = "";
            string dir = "";
            int heading_n = 0;
            int wsd_n = 0;
            int pl_n = 0;
            double tol_n = Convert.ToDouble(args[3]);



            path_main = args[0];
            path_inf = args[1];
            dir = args[2];



            DirectoryInfo path_o = new DirectoryInfo(dir);
            if (path_o.Exists == false)
            {
                path_o.Create();
            }

            FileStream es = new FileStream(path_main, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader er = new StreamReader(es);

            string Line_h;

            while (er.Peek() != -1)
            {
                Line_h = er.ReadLine();
                if (Line_h.Trim().StartsWith("*DIR"))
                {
                    Line_h = er.ReadLine();
                    if (Line_h.Trim().StartsWith("*"))
                    {
                        Line_h = er.ReadLine();
                    }
                    while (Line_h.Trim().StartsWith("*") == false)
                    {
                        Line_h = er.ReadLine();
                        heading_n = heading_n + 1;
                    }
                }
                if (Line_h.Trim().StartsWith("*WSD,"))
                {
                    Line_h = er.ReadLine();
                    while (Line_h.Trim().StartsWith("*") == true)
                    {
                        Line_h = er.ReadLine();
                    }
                    while (Line_h.Trim().StartsWith("*") == false)
                    {
                        Line_h = er.ReadLine();
                        wsd_n = wsd_n + 1;
                    }
                }
                if (Line_h.Trim().StartsWith("*LT"))
                {
                    Line_h = er.ReadLine();
                    while (Line_h.Trim().StartsWith("*") == true)
                    {
                        Line_h = er.ReadLine();
                    }
                    string[] p_n = Line_h.Split(',');

                    pl_n = p_n.Length;
                }
                if (Line_h.Trim().StartsWith("*TOL"))
                {
                    Line_h = er.ReadLine();
                    while (Line_h.Trim().StartsWith("*") == true)
                    {
                        Line_h = er.ReadLine();
                    }
                    tol_n = Convert.ToDouble(Line_h);
                }
            }
            er.Close();
            es.Close();

            double[] PL = new double[pl_n];
            string[,] DIRE = new string[heading_n, 3];

            FileStream LS_arr = new FileStream(path_main, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader LR_arr = new StreamReader(LS_arr);

            string Line_arr;
            int d = 0;

            while (LR_arr.Peek() != -1)
            {
                Line_arr = LR_arr.ReadLine();
                if (Line_arr.Trim().StartsWith("*DIR"))
                {
                    Line_arr = LR_arr.ReadLine();
                    while (Line_arr.Trim().StartsWith("*") == true)
                    {
                        Line_arr = LR_arr.ReadLine();
                    }
                    while (Line_arr.Trim().StartsWith("*") == false)
                    {
                        string[] r_d = Line_arr.Split(',');
                        DIRE[d, 0] = Convert.ToString(d + 1);
                        DIRE[d, 1] = r_d[0];
                        DIRE[d, 2] = r_d[1];
                        d = d + 1;
                        Line_arr = LR_arr.ReadLine();
                    }
                }
                if (Line_arr.Trim().StartsWith("*LT"))
                {
                    Line_arr = LR_arr.ReadLine();
                    while (Line_arr.Trim().StartsWith("*") == true)
                    {
                        Line_arr = LR_arr.ReadLine();
                    }
                    string[] p_l = Line_arr.Split(',');

                    for (int i_1 = 0; i_1 < pl_n; i_1++)
                    {
                        PL[i_1] = Convert.ToDouble(p_l[i_1]);
                    }
                }
            }
            LS_arr.Close();
            LR_arr.Close();

            string file_df = Path.Combine(path_inf + "\\SS001" + ".sm");
            int loc_n = 0;
            int f_n = 0;
            string type = "";
            int molen = 0;

            FileStream ps0 = new FileStream(file_df, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader pr0 = new StreamReader(ps0);

            string Line_1;

            while (pr0.Peek() != -1)
            {
                Line_1 = pr0.ReadLine();
                if (Line_1.Trim().StartsWith("*Spectral"))
                {
                    Line_1 = pr0.ReadLine();
                    string[] type_f = Line_1.Split(',');
                    molen = type_f.Length;
                    type = type_f[0];
                    Line_1 = pr0.ReadLine();
                    while (Line_1.Trim().StartsWith("*END") == false)
                    {
                        string[] l_f = new string[3];
                        f_n = 0;
                        string st = "";
                        while (l_f[1] != st)
                        {
                            if (f_n == 0)
                            {
                                l_f = Line_1.Split(',');
                                st = l_f[1];
                            }
                            else if (f_n == 1)
                            {
                                l_f = Line_1.Split(',');
                                loc_n = loc_n + 1;
                            }
                            f_n = f_n + 1;
                            Line_1 = pr0.ReadLine();
                            l_f = Line_1.Split(',');
                            if (l_f[0] == "*END")
                            {
                                if (f_n == 1)
                                {
                                    loc_n = loc_n + 1;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            pr0.Close();
            ps0.Close();

            double[, ,] M_cal = new double[wsd_n, loc_n, molen-2];
            double[,] WSD = new double[wsd_n, 3];
            double[] LOC = new double[loc_n];
            int jk = 0;

            for (int w = 0; w < wsd_n; w++)
            {
                string na = "";

                if (Convert.ToString(w + 1).Length == 1)
                {
                    na = ("SS00" + Convert.ToString(w + 1));
                }
                else if (Convert.ToString(w + 1).Length == 2)
                {
                    na = ("SS0" + Convert.ToString(w + 1));
                }
                else if (Convert.ToString(w + 1).Length == 3)
                {
                    na = ("SS" + Convert.ToString(w + 1));
                }
                string path_open = Path.Combine(path_inf + "\\" + na + ".sm");
                string path_write = Path.Combine(path_o + "\\" + na + ".st");

                FileStream ss = new FileStream(path_write, FileMode.Create, FileAccess.Write, FileShare.Write);
                StreamWriter sr = new StreamWriter(ss);

                    for (int loc = 0; loc < loc_n; loc++)
                    {
                        if (molen == 7 || molen == 4)
                        {
                            jk = 7;
                        }
                        else
                        {
                            jk = 3;
                        }

                        for (int vi = 2; vi < jk; vi++)
                        {
                            if (molen == 4 && vi == 4)
                            {
                                break;
                            }
                            FileStream ps = new FileStream(path_open, FileMode.Open, FileAccess.Read, FileShare.Read);
                            StreamReader pr = new StreamReader(ps);

                            string Line_o;
                            int l_r = 0;

                            while (pr.Peek() != -1)
                            {
                                Line_o = pr.ReadLine();
                                if (loc == 0 && Line_o.Trim().StartsWith("**       Hs"))
                                {
                                    Line_o = pr.ReadLine();
                                    string[] L_t = Line_o.Split(',');
                                    WSD[w, 0] = Convert.ToDouble(L_t[0]);
                                    WSD[w, 1] = Convert.ToDouble(L_t[1]);
                                    WSD[w, 2] = Convert.ToDouble(L_t[2]);
                                    if (molen == 4 && vi == 2)
                                    {
                                        sr.WriteLine("*WSD\n**       Hs(m),       Tz(sec),         Prob.\n{0,14:0.00},{1,14:0.00},{2,14:0.0000e0}\n", WSD[w, 0], WSD[w, 1], WSD[w, 2]);
                                        sr.WriteLine("*Short term moment\n{0},   Shear force,        Moment", type);
                                    }
                                    else if (molen == 7)
                                    {
                                        sr.WriteLine("*WSD\n**       Hs(m),       Tz(sec),         Prob.\n{0,14:0.00},{1,14:0.00},{2,14:0.0000e0}\n", WSD[w, 0], WSD[w, 1], WSD[w, 2]);
                                        sr.WriteLine("*Short term moment\n{0},  Acceleration,        Motion,     F-K force,    DIFF force, FK+DIFF force", type);

                                    }
                                    else
                                    {
                                        sr.WriteLine("*WSD\n**       Hs(m),       Tz(sec),         Prob.\n{0,14:0.00},{1,14:0.00},{2,14:0.0000e0}\n", WSD[w, 0], WSD[w, 1], WSD[w, 2]);
                                        sr.WriteLine("*Short term moment\n{0},            M0", type);
                                    }

                                }

                                if (Line_o.Trim().StartsWith("**") && !Line_o.Trim().StartsWith("**       Hs"))
                                {
                                    Line_o = pr.ReadLine();
                                    while (Line_o.Trim().StartsWith("*END") == false)
                                    {
                                        string[] L_0 = Line_o.Split(',');
                                        LOC[loc] = Convert.ToDouble(L_0[0]);
                                        if (l_r == loc * heading_n)
                                        {
                                            for (int co = 0; co < heading_n; co++)
                                            {
                                                string[] Moment = Line_o.Split(',');

                                                M_cal[w, loc, vi - 2] = M_cal[w, loc, vi - 2] + Convert.ToDouble(Moment[vi]) * Convert.ToDouble(DIRE[co, 2]);
                                                Line_o = pr.ReadLine();
                                            }
                                            break;
                                        }
                                        l_r = l_r + 1;
                                        Line_o = pr.ReadLine();
                                    }

                                    //sr.WriteLine("{0,14},{1,14:0.000000e0}", LOC[loc], M_cal[w, loc, vi - 2]);
                                    break;
                                }
                            }
                            ps.Close();
                            pr.Close();
                        }
                        if (molen == 4)
                        {
                            sr.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0}", LOC[loc], M_cal[w, loc, 0], M_cal[w, loc, 1]);
                        }
                        else if (molen == 7)
                        {
                            sr.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0},{3,14:0.000000e0},{4,14:0.000000e0},{5,14:0.000000e0}", LOC[loc], M_cal[w, loc, 0], M_cal[w, loc, 1], M_cal[w, loc, 2], M_cal[w, loc, 3], M_cal[w, loc, 4]);
                        }
                        else
                        {
                            sr.WriteLine("{0,14},{1,14:0.000000e0}", LOC[loc], M_cal[w, loc, 0]);
                        }
                    }

                sr.WriteLine("*END");
                sr.Close();
                ss.Close();
            }

            string write = Path.Combine(path_o + "\\LTM" + ".lt");

            FileStream ss_1 = new FileStream(write, FileMode.Create, FileAccess.Write, FileShare.Write);
            StreamWriter sr_1 = new StreamWriter(ss_1);
            if (molen == 4)
            {
                sr_1.WriteLine("*LONG TERM PREDICTIONS\n{0},   Prob. level,   Shear force,        Moment", type);
            }
            else if (molen == 7)
            {

            }
            else
            {
                sr_1.WriteLine("*LONG TERM PREDICTIONS\n{0},   Prob. level,      LT value", type);
            }

            double[,,] pl_value = new double[loc_n,pl_n, molen - 2];
            for (int vi = 2; vi < molen; vi++)
            {
                for (int l_r = 0; l_r < loc_n; l_r++)
                {

                    for (int i_pl = 0; i_pl < pl_n; i_pl++)
                    {
                        double min_i = 0;
                        double max_i = Math.Pow(10, 100);

                        int i_i = 0;
                        while (i_i < Math.Pow(10, 1000))
                        {
                            double long_min = 0;
                            double long_max = 0;

                            for (int w_min = 0; w_min < wsd_n; w_min++)
                            {
                                long_min = long_min + Math.Exp(-0.5 * Math.Pow(min_i, 2) / M_cal[w_min, l_r, vi - 2]) * WSD[w_min, 2];
                            }

                            if (Convert.ToString(long_min) == "NaN")
                            {
                                long_min = 100;
                            }

                            for (int w_max = 0; w_max < wsd_n; w_max++)
                            {
                                long_max = long_max + Math.Exp(-0.5 * Math.Pow(max_i, 2) / M_cal[w_max, l_r, vi - 2]) * WSD[w_max, 2];
                            }

                            double long_av = PL[i_pl] - long_min;
                            double long_iv = PL[i_pl] - long_max;
                            double mid = (min_i + max_i) / 2;

                            double long_mid = 0;

                            for (int w_min = 0; w_min < wsd_n; w_min++)
                            {
                                long_mid = long_mid + Math.Exp(-0.5 * Math.Pow(mid, 2) / M_cal[w_min, l_r, vi - 2]) * WSD[w_min, 2];
                            }

                            double long_mv = PL[i_pl] - long_mid;

                            if (long_mv < 0)
                            {
                                min_i = mid;
                            }
                            else if (long_mv > 0)
                            {
                                max_i = mid;
                            }

                            if (Math.Abs(max_i - min_i) / min_i < tol_n)
                            {
                                pl_value[l_r,i_pl, vi - 2] = mid;
                                break;
                            }
                            i_i = i_i + 1;
                        }
                        //sr_1.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0}", LOC[l_r], PL[i_pl], pl_value);
                    }
                }
            }
            for (int l_r = 0; l_r < loc_n; l_r++)
            {
                for (int i_pl = 0; i_pl < pl_n; i_pl++)
                {
                    if (molen == 4)
                    {
                        sr_1.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0},{3,14:0.000000e0}", LOC[l_r], PL[i_pl], pl_value[l_r, i_pl, 0], pl_value[l_r, i_pl, 1]);
                    }
                    else if (molen == 7)
                    {
                        sr_1.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0},{3,14:0.000000e0},{4,14:0.000000e0},{5,14:0.000000e0},{6,14:0.000000e0}", LOC[l_r], PL[i_pl], pl_value[l_r, i_pl, 0], pl_value[l_r, i_pl, 1], pl_value[l_r, i_pl, 2], pl_value[l_r, i_pl, 3], pl_value[l_r, i_pl, 4]);
                    }
                    else
                    {
                        sr_1.WriteLine("{0,14},{1,14:0.000000e0},{2,14:0.000000e0}", LOC[l_r], PL[i_pl], pl_value[l_r, i_pl, 0]);
                    }
                }
            }
            sr_1.WriteLine("*END");
            sr_1.Close();
            ss_1.Close();
        }
    }
}
