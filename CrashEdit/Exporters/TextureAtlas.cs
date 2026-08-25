using CrashEdit.CE;
using OpenTK.Mathematics;

namespace CrashEdit.Exporters;

/// <summary>
/// Helper class that simplifies the creation of texture atlas
/// optimized for export
/// </summary>
public class TextureAtlas
{
    public class TextureInfo
    {
        public Bitmap Data;
        public VertexTexInfo Texinfo;
    }
    // TODO: VertexTexInfo MIGHT NOT BE THE BEST TO DISCERN DIFFERENT TEXTURES
    // TODO: MAINLY BECAUSE IT HAS MORE INFORMATION THAN JUST COLOR AND BLEND MODES
    // TODO: AN OPTIMIZATION MIGHT BE ADJUSTING THIS TO TAKE THAT INTO ACCOUNT
    // TODO: AND THUS REDUCING THE SIZE OF THE OUTPUT
    
    private Dictionary <VertexTexInfo, TextureInfo> originals = new Dictionary <VertexTexInfo, TextureInfo> ();

    /// <summary>
    /// Registers the info of a texture for the atlas to be used
    /// </summary>
    /// <param name="info"></param>
    /// <param name="data"></param>
    public void AddTexture (VertexTexInfo info, Bitmap data)
    {
        this.originals [info] = new TextureInfo {Data = data, Texinfo = info};
    }

    /// <summary>
    /// The meat of the class, takes all the UV information and builds an image with all the required information
    /// </summary>
    public Bitmap BuildAtlas (out int width, out int height, out Dictionary <TextureInfo, Vector2> topleft)
    {
        topleft = new Dictionary<TextureInfo, Vector2> ();
        width = 1024 * 4;
        height = 128;
        int currentX = 0;
        
        // width of the atlas will be a multiple of 1024 to fit multiple 1024 textures horizontally
        foreach (KeyValuePair <VertexTexInfo, TextureInfo> pair in originals)
        {
            Vector2 textureSize = TextureUtils.TextureSize (pair.Key.Color);
            topleft.Add (pair.Value, new Vector2 (currentX, height - 128));

            int nextX = currentX + (int) textureSize.X;

            if (nextX >= width)
            {
                currentX = 0;
                height += 128;
            }
            else
            {
                currentX = nextX;
            }
        }

        // the last texture added filled a row, do not create an empty row
        if (currentX == 0)
        {
            height -= 128;
        }

        Bitmap result = new Bitmap (width, height);

        currentX = 0;
        int currentY = 0;
        
        // now apply the same algorithm but copying the data out
        foreach (KeyValuePair <VertexTexInfo, TextureInfo> pair in originals)
        {
            Vector2 textureSize = TextureUtils.TextureSize (pair.Key.Color);
            
            // copy pixels manually
            for (int x = 0; x < textureSize.X; x++)
            {
                for (int y = 0; y < textureSize.Y; y++)
                {
                    result.SetPixel (x + currentX, y + currentY, pair.Value.Data.GetPixel (x, y));
                }
            }
            
            // determine next point to get pixels from
            int nextX = currentX + (int) textureSize.X;

            if (nextX >= width)
            {
                currentX = 0;
                currentY += 128;
            }
            else
            {
                currentX = nextX;
            }
        }

        return result;
    }
}