using System;
using System.Text;
using System.IO;

namespace GlyphList
{
    public class GlyphExportToText : IGlyphVisitor
    {
        private StringBuilder _builder = new StringBuilder();
        private string _fileName;

        public void Started(string fontFamilyName, string style, string weight)
        {
            _builder.Clear();
            _fileName = $"{fontFamilyName}-{style}-{weight}.txt";
        }

        public void GlyphFound(int code)
        {
            if (code <= 0xFFFF)
            {
                char ch = (char)code;
                if (Char.IsControl(ch) == false)
                {
                    _builder.AppendFormat(@"\u{0:x4}", (ushort)code);
                }
            }
            else
            {
                _builder.AppendFormat(@"\U{0:x8}", code);
            }
        }

        public void Completed()
        {
            Console.WriteLine($"Writing \"{_fileName}\"");
            File.WriteAllText(_fileName, _builder.ToString());
        }
    }
}
