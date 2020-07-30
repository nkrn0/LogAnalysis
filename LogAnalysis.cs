using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Resources;

class LogAnalysis
{
    static void Main(string[] args)
    {
        List<string> log_line = new List<string>();
        List<string> add_log = new List<string>();
        string file_path = "";
        string endkey = "";
        string skipkey = "";
        bool errorFlag = false;

        try
        {
            Console.WriteLine("���O�t�@�C���̃p�X����͂��Ă��������B");
            file_path = Console.ReadLine();
            Console.WriteLine();
            log_line = File.ReadAllLines(file_path).ToList();
            do
            {
                endkey = "";
                Console.WriteLine("���O�t�@�C����ǉ�����ꍇ�͒ǉ�����t�@�C���̃p�X����͂��Ă��������B�����ꍇ�́uend�v�Ɠ��͂��Ă��������B");
                endkey = Console.ReadLine();
                Console.WriteLine();
                if (endkey != "end")
                {
                    add_log = File.ReadAllLines(endkey).ToList();
                    log_line.AddRange(add_log);
                }
            }
            while (endkey != "end");

            Console.WriteLine("�W�v�̍ۂ̊��Ԃ̎w�肪����΁A���͂��Ă��������B�w�肵�Ȃ��ꍇ�́uskip�v�Ɠ��͂��Ă��������B");
            Console.WriteLine("�� : 2019�N3��10�� �` 2020�N4��5���̏ꍇ -> 20190310 20200405");
            skipkey = Console.ReadLine();
            Console.WriteLine();
            if (skipkey == "skip")
            {
                Console.WriteLine("�e���ԑі��̃A�N�Z�X����");
                HourLog(log_line);

                Console.WriteLine("�����[�g�z�X�g�ʂ̃A�N�Z�X�����i�~���j");
                SortHost(log_line);
            }
            else
            {
                string[] time = skipkey.Split(' ');
                Console.WriteLine("�e���ԑі��̃A�N�Z�X����");
                HourLog(TimeSort(log_line, int.Parse(time[0]), int.Parse(time[1])));

                Console.WriteLine("�����[�g�z�X�g�ʂ̃A�N�Z�X�����i�~���j");
                SortHost(TimeSort(log_line, int.Parse(time[0]), int.Parse(time[1])));
            }
        }
        catch (FileNotFoundException e)
        {
            errorFlag = true;
            Console.Error.WriteLine("///�G���[���������܂����B�w�肵���t�@�C�����͑��݂��܂���B///");
            Console.Error.WriteLine(e);
        }
        catch (NullReferenceException e)
        {
            errorFlag = true;
            Console.Error.WriteLine("///�G���[���������܂����B�t�@�C���p�X���ԈႦ�Ă���\��������܂��B///");
            Console.Error.WriteLine(e);
        }
        catch (Exception e)
        {
            errorFlag = true;
            Console.Error.WriteLine("///�\�z�O�ȃG���[���������܂����B///");
            Console.Error.WriteLine(e);
            throw;
        }
        finally
        {
            if (errorFlag)
            {
                Console.WriteLine("�A�N�Z�X�����̏W�v�Ɏ��s���܂����B");
            }
            else
            {
                Console.WriteLine("�A�N�Z�X�����̏W�v���������܂����B");
            }
        }

    }

    //�e���ԑтɃA�N�Z�X���ꂽ������\�����郁�\�b�h
    static void HourLog(IEnumerable<string> log)
    {
        Console.WriteLine(" 1��: " + HourCount(log, "01") + "��");
        Console.WriteLine(" 2��: " + HourCount(log, "02") + "��");
        Console.WriteLine(" 3��: " + HourCount(log, "03") + "��");
        Console.WriteLine(" 4��: " + HourCount(log, "04") + "��");
        Console.WriteLine(" 5��: " + HourCount(log, "05") + "��");
        Console.WriteLine(" 6��: " + HourCount(log, "06") + "��");
        Console.WriteLine(" 7��: " + HourCount(log, "07") + "��");
        Console.WriteLine(" 8��: " + HourCount(log, "08") + "��");
        Console.WriteLine(" 9��: " + HourCount(log, "09") + "��");
        Console.WriteLine(" 10��: " + HourCount(log, "10") + "��");
        Console.WriteLine(" 11��: " + HourCount(log, "11") + "��");
        Console.WriteLine(" 12��: " + HourCount(log, "12") + "��");
        Console.WriteLine(" 13��: " + HourCount(log, "13") + "��");
        Console.WriteLine(" 14��: " + HourCount(log, "14") + "��");
        Console.WriteLine(" 15��: " + HourCount(log, "15") + "��");
        Console.WriteLine(" 16��: " + HourCount(log, "16") + "��");
        Console.WriteLine(" 17��: " + HourCount(log, "17") + "��");
        Console.WriteLine(" 18��: " + HourCount(log, "18") + "��");
        Console.WriteLine(" 19��: " + HourCount(log, "19") + "��");
        Console.WriteLine(" 20��: " + HourCount(log, "20") + "��");
        Console.WriteLine(" 21��: " + HourCount(log, "21") + "��");
        Console.WriteLine(" 22��: " + HourCount(log, "22") + "��");
        Console.WriteLine(" 23��: " + HourCount(log, "23") + "��");
        Console.WriteLine(" 24��: " + HourCount(log, "00") + "��");
        Console.WriteLine();
    }

    //�e���ԑтɉ����̃A�N�Z�X�����邩�𒲂ׂ郁�\�b�h
    static int HourCount(IEnumerable<string> log, string hour)
    {
        int count = 0;
        foreach(string str in log)
        {
            string[] str_array = str.Split(':');
            if(str_array[1] == hour)
            {
                count++;
            }
        }

        return count;
    }

    //�A�N�Z�X�̑��������[�g�z�X�g�̏��ɃA�N�Z�X������\�����郁�\�b�h
    static void SortHost(IEnumerable<string> log)
    {
        //���O���烊���[�g�z�X�g�̈ꗗ���i�[����z����쐬(host_list)
        var host_list = new List<string>();
        foreach (string str in log)
        {
            string[] str_array = str.Split(' ');
            host_list.Add(str_array[0]);
        }

        //�����[�g�z�X�g�̈ꗗ����d���v�f���폜�����z����쐬(organized_list)
        //.ToList()���\�b�h�ŁAIEnumerable�^����List�^�ɕϊ�
        //�����[�g�z�X�g(Key)�ƃz�X�g���Ƃ̉�(Value)���i�[����Dictionary���쐬
        var organized_list = host_list.Distinct().ToList();
        var host_count = new Dictionary<string, int>();
        int count;
        for(int i=0;i<organized_list.Count();i++)
        {
            count = 0;
            foreach(string str in host_list)
            {
                if(organized_list[i] == str)
                {
                    count++;
                }
            }
            host_count.Add(organized_list[i], count);
        }

        //�~���Ƀ\�[�g���ďo��
        var sort_list = host_count.OrderByDescending(X => X.Value);
        foreach (KeyValuePair<string, int> str in sort_list)
        {
            Console.WriteLine(str.Key + " : " + str.Value + "��");
        }
        Console.WriteLine();
    }

    //�A�N�Z�X���O��������Ŏw�肵�� first_time �` last_time �܂ł�
    //�����̏����𖞂����Ă��郍�O��؂������z���Ԃ����\�b�h
    static List<string> TimeSort(IEnumerable<string> log, int first_time, int last_time)
    {
        string[] str1;
        string[] str2;
        string[] str3;
        int now_time = 0;
        List<string> sort_log = new List<string>();
        foreach (string str in log)
        {
            str1 = str.Split('[');
            str2 = str1[1].Split(':');
            str3 = str2[0].Split('/');
            str3[1] = MonthCheck(str3[1]);
            now_time = int.Parse(str3[2] + str3[1] + str3[0]);
            if(now_time >= first_time && now_time <= last_time)
            {
                sort_log.Add(str);
            }
        }
        return sort_log;
    }

    //�A�N�Z�X���O�̌��̉p�P��𐔎��ɕϊ����郁�\�b�h
    static string MonthCheck(string mon)
    {
        string month = "";
        switch(mon)
        {
            case "Jan":
                month = "01";
                break;
            case "Feb":
                month = "02";
                break;
            case "Mar":
                month = "03";
                break;
            case "Apr":
                month = "04";
                break;
            case "May":
                month = "05";
                break;
            case "Jun":
                month = "06";
                break;
            case "Jul":
                month = "07";
                break;
            case "Aug":
                month = "08";
                break;
            case "Sep":
                month = "09";
                break;
            case "Oct":
                month = "10";
                break;
            case "Nov":
                month = "11";
                break;
            case "Dec":
                month = "12";
                break;
        }
        return month;
    }

}