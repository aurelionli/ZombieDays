
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FPS
{
    public class LyricLine
    {
        public float time; // 时间戳（秒）
        public string text; // 歌词内容

        public LyricLine(float time, string text)
        {
            this.time = time;
            this.text = text;
        }
    }
    public class LrcParser
    {


        public List<LyricLine> lyrics = new List<LyricLine>();


        public void Parse(string lrcText)
        {
            lyrics.Clear();
            string[] lines = lrcText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
          
            foreach (string line in lines)
            {
               
              /*  // 匹配时间戳和歌词内容  
                // /d数字字符 +出现一次或多次 （）捕获为一个组 
                //因此，(\d+) 匹配一个或多个连续的数字字符，并将其作为一个捕获组（例如 "03" 或 "59"）。

                // ：直接匹配冒号

                // \d+ 一到多个数字字符   \.小数点 ，有特殊含义使用\转义
                //\d+ 再次表示一个或多个数字字符（秒数和毫秒数
                //括号 () 将这部分捕获为一个组，以便后续可以引用。
                //(\d+\.\d+) 匹配一个或多个数字后跟一个小数点再跟一个或多个数字（例如 "07.50"）。

                /*(.*)：
                . 表示任意字符（除了换行符）。
                * 表示前面的字符或组出现零次或多次。
                括号 () 将这部分捕获为一个组，以便后续可以引用。
                因此，(.*) 匹配任意数量的任意字符（歌词文本），直到行尾。*/

                //Match 类是震泽的匹配结果
                /*属性
                Success
                类型：bool
                描述：指示是否找到了匹配项。如果找到了匹配项，则为 true；否则为 false。
                Value
                类型：string
                描述：返回整个匹配的子字符串。
                Groups
                类型：GroupCollection
                描述：返回一个包含所有捕获组的集合。每个捕获组都可以通过索引或名称访问。
                Index
                类型：int
                描述：返回匹配项在原始字符串中的起始位置。
                Length
                类型：int
                描述：返回匹配项的长度。
                方法
                NextMatch()
                描述：返回下一个匹配项的 Match 对象。通常与 Regex.Matches 结合使用。
                ToString()
                描述：返回整个匹配的子字符串（等同于 Value 属性）。
                 */
                Match match = Regex.Match(line, @"\[(\d+):(\d+\.\d+)\](.*)");
                if (match.Success)
                {
                    int minutes = int.Parse(match.Groups[1].Value);
                    float seconds = float.Parse(match.Groups[2].Value);
                    string text = match.Groups[3].Value.Trim();//用于移除字符串两端的空白字符。如换行符（\n）、回车符（\r）

                    float time = minutes * 60 + seconds; // 转换为秒
                    lyrics.Add(new LyricLine(time, text));
                }

                // 按时间排序a 和 b：是 Sort 方法在内部自动选取的两个待比较的元素。是比较逻辑，返回一个整数，表示 a 和 b 的相对顺序
                /*如果返回值小于 0，表示 a 应排在 b 之前。
如果返回值等于 0，表示 a 和 b 相等，保持它们的相对位置不变。
如果返回值大于 0，表示 a 应排在 b 之后。*/
                /*lyrics.Sort((a, b) => a.time.CompareTo(b.time)); 是一种常见的固定用法，用于对复杂对象的列表进行排序*/
               // lyrics.Sort((a, b) => a.time.CompareTo(b.time));

            }
        }

        /*实现 IComparable<T> 接口：这是因为 int 类型已经实现了 IComparable<int> 接口，Sort 方法知道如何比较两个整数。
如果你希望在多个地方复用相同的比较逻辑，可以在类中实现 IComparable<T> 接口。
csharp
深色版本
public class Lyric : IComparable<Lyric>
{
    public float time;
    public string lyricText;

    public Lyric(float time, string lyricText)
    {
        this.time = time;
        this.lyricText = lyricText;
    }

    public int CompareTo(Lyric other)
    {
        return this.time.CompareTo(other.time);
    }

    public override string ToString()
    {
        return $"Time: {time}, Lyrics: {lyricText}";
    }
}*/

    }
}
