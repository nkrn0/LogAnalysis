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
            Console.WriteLine("ログファイルのパスを入力してください。");
            file_path = Console.ReadLine();
            Console.WriteLine();
            log_line = File.ReadAllLines(file_path).ToList();
            do
            {
                endkey = "";
                Console.WriteLine("ログファイルを追加する場合は追加するファイルのパスを入力してください。無い場合は「end」と入力してください。");
                endkey = Console.ReadLine();
                Console.WriteLine();
                if (endkey != "end")
                {
                    add_log = File.ReadAllLines(endkey).ToList();
                    log_line.AddRange(add_log);
                }
            }
            while (endkey != "end");

            Console.WriteLine("集計の際の期間の指定があれば、入力してください。指定しない場合は「skip」と入力してください。");
            Console.WriteLine("例 : 2019年3月10日 ～ 2020年4月5日の場合 -> 20190310 20200405");
            skipkey = Console.ReadLine();
            Console.WriteLine();
            if (skipkey == "skip")
            {
                Console.WriteLine("各時間帯毎のアクセス件数");
                HourLog(log_line);

                Console.WriteLine("リモートホスト別のアクセス件数（降順）");
                SortHost(log_line);
            }
            else
            {
                string[] time = skipkey.Split(' ');
                Console.WriteLine("各時間帯毎のアクセス件数");
                HourLog(TimeSort(log_line, int.Parse(time[0]), int.Parse(time[1])));

                Console.WriteLine("リモートホスト別のアクセス件数（降順）");
                SortHost(TimeSort(log_line, int.Parse(time[0]), int.Parse(time[1])));
            }
        }
        catch (FileNotFoundException e)
        {
            errorFlag = true;
            Console.Error.WriteLine("///エラーが発生しました。指定したファイル名は存在しません。///");
            Console.Error.WriteLine(e);
        }
        catch (NullReferenceException e)
        {
            errorFlag = true;
            Console.Error.WriteLine("///エラーが発生しました。ファイルパスを間違えている可能性があります。///");
            Console.Error.WriteLine(e);
        }
        catch (Exception e)
        {
            errorFlag = true;
            Console.Error.WriteLine("///予想外なエラーが発生しました。///");
            Console.Error.WriteLine(e);
            throw;
        }
        finally
        {
            if (errorFlag)
            {
                Console.WriteLine("アクセス件数の集計に失敗しました。");
            }
            else
            {
                Console.WriteLine("アクセス件数の集計が完了しました。");
            }
        }

    }

    //各時間帯にアクセスされた件数を表示するメソッド
    static void HourLog(IEnumerable<string> log)
    {
        Console.WriteLine(" 1時: " + HourCount(log, "01") + "件");
        Console.WriteLine(" 2時: " + HourCount(log, "02") + "件");
        Console.WriteLine(" 3時: " + HourCount(log, "03") + "件");
        Console.WriteLine(" 4時: " + HourCount(log, "04") + "件");
        Console.WriteLine(" 5時: " + HourCount(log, "05") + "件");
        Console.WriteLine(" 6時: " + HourCount(log, "06") + "件");
        Console.WriteLine(" 7時: " + HourCount(log, "07") + "件");
        Console.WriteLine(" 8時: " + HourCount(log, "08") + "件");
        Console.WriteLine(" 9時: " + HourCount(log, "09") + "件");
        Console.WriteLine(" 10時: " + HourCount(log, "10") + "件");
        Console.WriteLine(" 11時: " + HourCount(log, "11") + "件");
        Console.WriteLine(" 12時: " + HourCount(log, "12") + "件");
        Console.WriteLine(" 13時: " + HourCount(log, "13") + "件");
        Console.WriteLine(" 14時: " + HourCount(log, "14") + "件");
        Console.WriteLine(" 15時: " + HourCount(log, "15") + "件");
        Console.WriteLine(" 16時: " + HourCount(log, "16") + "件");
        Console.WriteLine(" 17時: " + HourCount(log, "17") + "件");
        Console.WriteLine(" 18時: " + HourCount(log, "18") + "件");
        Console.WriteLine(" 19時: " + HourCount(log, "19") + "件");
        Console.WriteLine(" 20時: " + HourCount(log, "20") + "件");
        Console.WriteLine(" 21時: " + HourCount(log, "21") + "件");
        Console.WriteLine(" 22時: " + HourCount(log, "22") + "件");
        Console.WriteLine(" 23時: " + HourCount(log, "23") + "件");
        Console.WriteLine(" 24時: " + HourCount(log, "00") + "件");
        Console.WriteLine();
    }

    //各時間帯に何件のアクセスがあるかを調べるメソッド
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

    //アクセスの多いリモートホストの順にアクセス件数を表示するメソッド
    static void SortHost(IEnumerable<string> log)
    {
        //ログからリモートホストの一覧を格納する配列を作成(host_list)
        var host_list = new List<string>();
        foreach (string str in log)
        {
            string[] str_array = str.Split(' ');
            host_list.Add(str_array[0]);
        }

        //リモートホストの一覧から重複要素を削除した配列を作成(organized_list)
        //.ToList()メソッドで、IEnumerable型からList型に変換
        //リモートホスト(Key)とホストごとの回数(Value)を格納したDictionaryを作成
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

        //降順にソートして出力
        var sort_list = host_count.OrderByDescending(X => X.Value);
        foreach (KeyValuePair<string, int> str in sort_list)
        {
            Console.WriteLine(str.Key + " : " + str.Value + "回");
        }
        Console.WriteLine();
    }

    //アクセスログから引数で指定した first_time ～ last_time までの
    //時刻の条件を満たしているログを切り取った配列を返すメソッド
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

    //アクセスログの月の英単語を数字に変換するメソッド
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