using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;

namespace ABAQUS2AQWA
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string[] args = new string[4];
            args[0] = "D:\\FATOS_v2018_07\\INP\\LC19_hydro(m__ori_center).inp";
            args[1] = "D:\\FATOS_v2018_07";
            args[2] = "2";
            args[3] = "DIFF";*/
            double pa = 0;
            string elset_diff = "";
            string elset_nondiff = "";
            string f_n = "";
            double unit = 0;
            string savepath = "";

            string path = args[0];

            int index = args[0].LastIndexOf("\\");
            int i = index + 1;
            int k = args[0].LastIndexOf(".");
            f_n = args[0].Substring(i, k - i);

            string save = args[1];
            savepath = Path.Combine(save + "\\" + f_n + ".dat");

            unit = Convert.ToDouble(args[2]);

            elset_diff = args[3];

            if (args.Length == 5)
            {
                elset_nondiff = args[4];
            }

            if (unit == 1.0)
            {
                pa = 0.001;
            }
            else if (unit == 2.0)
            {
                pa = 1;
            }
            else if (unit == 3.0)
            {
                pa = 1;
            }
            else if (unit == 4.0)
            {
                pa = 1000;
            }

            int Check_diff = 0;
            int Check_nondiff = 0;

            FileStream num_S_node = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader num_R_node = new StreamReader(num_S_node);

            string rLine;

            int num_node = 0;
            int num_diff_4 = 0;
            int num_diff_3 = 0;
            int num_dry_4 = 0;
            int num_dry_3 = 0;

            while (num_R_node.Peek() != -1)
            {
                rLine = num_R_node.ReadLine();
                if (rLine.Trim().StartsWith("*NODE"))
                {
                    rLine = num_R_node.ReadLine();
                    while (rLine.Trim().StartsWith("*") == false)
                    {
                        num_node = num_node + 1;
                        rLine = num_R_node.ReadLine();
                    }
                }
                else if (rLine.Trim().StartsWith("*ELEMENT,TYPE="))
                {
                    string[] key_ele_type = rLine.Split(',');
                    if (key_ele_type[2].Length == elset_diff.Length + 6)
                    {
                        if (key_ele_type[2].Substring(6, elset_diff.Length).Trim() == elset_diff && key_ele_type[1].Substring(5, 2).Trim() == "S4")
                        {
                            rLine = num_R_node.ReadLine();
                            while (rLine.Trim().StartsWith("*") == false)
                            {
                                num_diff_4 = num_diff_4 + 1;
                                rLine = num_R_node.ReadLine();
                            }
                            Check_diff = 1;
                        }
                        else if (key_ele_type[2].Substring(6, elset_diff.Length).Trim() == elset_diff && key_ele_type[1].Substring(5, 2).Trim() == "S3")
                        {
                            rLine = num_R_node.ReadLine();
                            while (rLine.Trim().StartsWith("*") == false)
                            {
                                num_diff_3 = num_diff_3 + 1;
                                rLine = num_R_node.ReadLine();
                            }
                            Check_diff = 1;
                        }
                    }
                    if (key_ele_type[2].Length == elset_nondiff.Length + 6)
                    {
                        if (key_ele_type[2].Substring(6, elset_nondiff.Length).Trim() == elset_nondiff && key_ele_type[1].Substring(5, 2).Trim() == "S4")
                        {
                            rLine = num_R_node.ReadLine();
                            while (rLine.Trim().StartsWith("*") == false)
                            {
                                num_dry_4 = num_dry_4 + 1;
                                rLine = num_R_node.ReadLine();
                            }
                            Check_nondiff = 1;
                        }
                        else if (key_ele_type[2].Substring(6, elset_nondiff.Length).Trim() == elset_nondiff && key_ele_type[1].Substring(5, 2).Trim() == "S3")
                        {
                            rLine = num_R_node.ReadLine();
                            while (rLine.Trim().StartsWith("*") == false)
                            {
                                num_dry_3 = num_dry_3 + 1;
                                rLine = num_R_node.ReadLine();
                            }
                            Check_nondiff = 1;
                        }
                    }
                }
            }
            num_S_node.Close();
            num_R_node.Close();

            if (Check_diff == 0)
            {
                Console.WriteLine("\nFatigue Assessment Tool for Offshore Structures (FATOS)--ABAQUS2AQWA module\nCopyright (C) 2018 Inha University SOSEL Lab. All rights reserved\n\nWARNING: Check ELSET name for diffraction elements or Distinguish upper/lower cases of ELSET name\n\nPress Enter key to Exit");
                Console.ReadLine();
                return;
            }
            else if (Check_nondiff == 0 && args.Length == 5)
            {
                Console.WriteLine("\nFatigue Assessment Tool for Offshore Structures (FATOS)--ABAQUS2AQWA module\nCopyright (C) 2018 Inha University SOSEL Lab. All rights reserved\n\nWARNING: Check ELSET name for non-diffraction elements or Distinguish upper/lower cases of ELSET name\n\nPress Enter key to Exit");
                Console.ReadLine();
                return;
            }

            double[,] node = new double[num_node, 4];
            double[,] wet_4 = new double[num_diff_4, 5];
            double[,] wet_3 = new double[num_diff_3, 4];
            double[,] dry_4 = new double[num_dry_4, 5];
            double[,] dry_3 = new double[num_dry_3, 4];

            FileStream FS = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader SR = new StreamReader(FS);

            string rLine_e;

            int num_node_e = 0;
            int num_diff_4_e = 0;
            int num_diff_3_e = 0;
            int num_dry_4_e = 0;
            int num_dry_3_e = 0;

            while (SR.Peek() != -1)
            {
                rLine_e = SR.ReadLine();
                if (rLine_e.Trim().StartsWith("*NODE"))
                {
                    rLine_e = SR.ReadLine();
                    while (rLine_e.Trim().StartsWith("*") == false)
                    {
                        num_node_e = num_node_e + 1;
                        string[] eline = rLine_e.Split(',');
                        for (int j = 0; j < 4; j++)
                        {
                            if (j == 0)
                            {
                                node[num_node_e - 1, j] = double.Parse(eline[j]);
                            }
                            else if (j != 0)
                            {
                                node[num_node_e - 1, j] = double.Parse(eline[j]) * pa;
                            }
                        }
                        rLine_e = SR.ReadLine();
                    }
                }
                else if (rLine_e.Trim().StartsWith("*ELEMENT,TYPE="))
                {
                    string[] key_ele_type = rLine_e.Split(',');
                    if (key_ele_type[2].Length == elset_diff.Length + 6)
                    {
                        if (key_ele_type[2].Substring(6, elset_diff.Length).Trim() == elset_diff && key_ele_type[1].Substring(5, 2).Trim() == "S4")
                        {
                            rLine_e = SR.ReadLine();
                            while (rLine_e.Trim().StartsWith("*") == false)
                            {
                                num_diff_4_e = num_diff_4_e + 1;
                                string[] eline = rLine_e.Split(',');
                                for (int j_e = 0; j_e < 5; j_e++)
                                {
                                    wet_4[num_diff_4_e - 1, j_e] = double.Parse(eline[j_e]);
                                }
                                rLine_e = SR.ReadLine();
                            }
                        }
                        else if (key_ele_type[2].Substring(6, elset_diff.Length).Trim() == elset_diff && key_ele_type[1].Substring(5, 2).Trim() == "S3")
                        {
                            rLine_e = SR.ReadLine();
                            while (rLine_e.Trim().StartsWith("*") == false)
                            {
                                num_diff_3_e = num_diff_3_e + 1;
                                string[] eline = rLine_e.Split(',');
                                for (int j_e = 0; j_e < 4; j_e++)
                                {
                                    wet_3[num_diff_3_e - 1, j_e] = double.Parse(eline[j_e]);
                                }
                                rLine_e = SR.ReadLine();
                            }
                        }
                    }
                    if (key_ele_type[2].Length == elset_nondiff.Length + 6)
                    {
                        if (key_ele_type[2].Substring(6, elset_nondiff.Length).Trim() == elset_nondiff && key_ele_type[1].Substring(5, 2).Trim() == "S4")
                        {
                            rLine_e = SR.ReadLine();
                            while (rLine_e.Trim().StartsWith("*") == false)
                            {
                                num_dry_4_e = num_dry_4_e + 1;
                                string[] eline = rLine_e.Split(',');
                                for (int j_e = 0; j_e < 5; j_e++)
                                {
                                    dry_4[num_dry_4_e - 1, j_e] = double.Parse(eline[j_e]);
                                }
                                rLine_e = SR.ReadLine();
                            }
                        }
                        else if (key_ele_type[2].Substring(6, elset_nondiff.Length).Trim() == elset_nondiff && key_ele_type[1].Substring(5, 2).Trim() == "S3")
                        {
                            rLine_e = SR.ReadLine();
                            while (rLine_e.Trim().StartsWith("*") == false)
                            {
                                num_dry_3_e = num_dry_3_e + 1;
                                string[] eline = rLine_e.Split(',');
                                for (int j_e = 0; j_e < 4; j_e++)
                                {
                                    dry_3[num_dry_3_e - 1, j_e] = double.Parse(eline[j_e]);
                                }
                                rLine_e = SR.ReadLine();
                            }
                        }
                    }
                }
            }
            SR.Close();
            FS.Close();

            int aqwa_n = 0;

            FileStream ws = new FileStream(savepath, FileMode.Create, FileAccess.Write, FileShare.Write);
            StreamWriter wr = new StreamWriter(ws);

            wr.WriteLine("***************************************************\n**[ABAQUS Transfer to AQWA Program (ABAQUS2AQWA)]\n**Produced by C.B.Li & J.Choung\n**Ship & Offshore Structure Engineering Lab (SOSEL)\n***************************************************\n");
            wr.WriteLine("********************************************************************************\n*********************************** DECK  {0} ************************************\n********************************************************************************", 0);
            wr.WriteLine("JOB AQWA  LINE\nTITLE\nOPTIONS PRPR REST GOON END\nRESTART  1  3\n********************************************************************************\n*********************************** DECK  1 ************************************\n********************************************************************************\n          COOR\n      NOD5\n      STRC        1");

            for (int i_w = 0; i_w < num_node; i_w++)
            {
                wr.WriteLine("{0,11}{1,19:0.0000e0}{2,10:0.0000e0}{3,10:0.0000e0}", node[i_w, 0], Math.Round(node[i_w, 1], 3), Math.Round(node[i_w, 2], 3), Math.Round(node[i_w, 3], 3));
            }
            wr.WriteLine(" END\n********************************************************************************\n*********************************** DECK  2 ************************************\n********************************************************************************\n          ELM1\n      ZLWL          (           0.00000)");

            for (int j_w = 0; j_w < num_diff_3; j_w++)
            {
                wr.WriteLine("      TPPL DIFF     (1)({0,5})({1,5})({2,5})                       Hyperm Elem No.:{3,5}     Aqwa  Elem No.:{4,5}", wet_3[j_w, 1], wet_3[j_w, 2], wet_3[j_w, 3], wet_3[j_w, 0], aqwa_n + 1);
                aqwa_n = aqwa_n + 1;
            }
            for (int i_w = 0; i_w < num_dry_3; i_w++)
            {
                wr.WriteLine("      TPPL          (1)({0,5})({1,5})({2,5})                       Hyperm Elem No.:{3,5}     Aqwa  Elem No.:{4,5}", dry_3[i_w, 1], dry_3[i_w, 2], dry_3[i_w, 3], dry_3[i_w, 0], aqwa_n + 1);
                aqwa_n = aqwa_n + 1;
            }
            wr.WriteLine("*****");
            for (int j_w = 0; j_w < num_diff_4; j_w++)
            {
                wr.WriteLine("      QPPL DIFF     (1)({0,5})({1,5})({2,5})({3,5})                Hyperm Elem No.:{4,5}     Aqwa  Elem No.:{5,5}", wet_4[j_w, 1], wet_4[j_w, 2], wet_4[j_w, 3], wet_4[j_w, 4], wet_4[j_w, 0], aqwa_n + 1);
                aqwa_n = aqwa_n + 1;
            }
            for (int i_w = 0; i_w < num_dry_4; i_w++)
            {
                wr.WriteLine("      QPPL          (1)({0,5})({1,5})({2,5})({3,5})                Hyperm Elem No.:{4,5}     Aqwa  Elem No.:{5,5}", dry_4[i_w, 1], dry_4[i_w, 2], dry_4[i_w, 3], dry_4[i_w, 4], dry_4[i_w, 0], aqwa_n + 1);
                aqwa_n = aqwa_n + 1;
            }
            wr.WriteLine(" END\n********************************************************************************\n");
            wr.Close();
            ws.Close();

            Console.WriteLine("\nFatigue Assessment Tool for Offshore Structures (FATOS)--ABAQUS2AQWA module\nCopyright (C) 2018 Inha University SOSEL Lab. All rights reserved\n\nProcess completed successfully!!! \n\nPress Enter key to Exit");
            Console.ReadLine();
            return;
        }
    }
}
