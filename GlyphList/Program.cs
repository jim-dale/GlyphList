using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;

namespace GlyphList
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Windows\Fonts";
            string searchPattern = "*.ttf";
            SearchOption searchOption = SearchOption.TopDirectoryOnly;

            IGlyphVisitor visitor = new GlyphExportToText();

            var files = Directory.EnumerateFiles(path, searchPattern, searchOption);
            foreach (var file in files)
            {
                ProcessFontFile(file, visitor);
            }
        }

        static void ProcessFontFile(string path, IGlyphVisitor visitor)
        {
            var families = Fonts.GetFontFamilies(path);

            foreach (FontFamily family in families)
            {
                ProcessFontFamily(family, visitor);
            }
        }

        private static void ProcessFontFamily(FontFamily family, IGlyphVisitor visitor)
        {
            string fontFamilyName = family.FamilyNames.FirstOrDefault(a => a.Key.IetfLanguageTag.StartsWith("en")).Value;

            var typefaces = family.GetTypefaces();
            foreach (Typeface typeface in typefaces)
            {
                ProcessTypeface(fontFamilyName, typeface, visitor);
            }
        }

        private static void ProcessTypeface(string fontFamilyName, Typeface typeface, IGlyphVisitor visitor)
        {
            GlyphTypeface glyph = null;
            typeface.TryGetGlyphTypeface(out glyph);
            if (glyph != null)
            {
                visitor.Started(fontFamilyName, typeface.Style.ToString(), typeface.Weight.ToString());

                ProcessGlyphInterface(glyph, visitor);

                visitor.Completed();
            }
        }

        private static void ProcessGlyphInterface(GlyphTypeface glyph, IGlyphVisitor visitor)
        {
            IDictionary<int, ushort> characterMap = glyph.CharacterToGlyphMap;

            StringBuilder builder = new StringBuilder();
            foreach (var kvp in characterMap)
            {
                visitor.GlyphFound(kvp.Key);
            }
        }
    }
}
