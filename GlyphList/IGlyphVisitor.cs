
namespace GlyphList
{
    public interface IGlyphVisitor
    {
        void Started(string fontFamilyName, string style, string weight);
        void GlyphFound(int code);
        void Completed();
    }
}
