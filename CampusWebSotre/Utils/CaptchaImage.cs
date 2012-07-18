namespace CampusWebStore.Utils
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Amount of random font warping to apply to rendered text
    /// </summary>
    public enum FontWarpFactor
    {
        /// <summary>
        /// 
        /// </summary>
        None, 

        /// <summary>
        /// 
        /// </summary>
        Low, 

        /// <summary>
        /// 
        /// </summary>
        Medium, 

        /// <summary>
        /// 
        /// </summary>
        High, 

        /// <summary>
        /// 
        /// </summary>
        Extreme
    }

    /// <summary>
    /// Amount of background noise to add to rendered image
    /// </summary>
    public enum BackgroundNoiseLevel
    {
        /// <summary>
        /// 
        /// </summary>
        None, 

        /// <summary>
        /// 
        /// </summary>
        Low, 

        /// <summary>
        /// 
        /// </summary>
        Medium, 

        /// <summary>
        /// 
        /// </summary>
        High, 

        /// <summary>
        /// 
        /// </summary>
        Extreme
    }

    /// <summary>
    /// Amount of curved line noise to add to rendered image
    /// </summary>
    public enum LineNoiseLevel
    {
        /// <summary>
        /// 
        /// </summary>
        None, 

        /// <summary>
        /// 
        /// </summary>
        Low, 

        /// <summary>
        /// 
        /// </summary>
        Medium, 

        /// <summary>
        /// 
        /// </summary>
        High, 

        /// <summary>
        /// 
        /// </summary>
        Extreme
    }

    /// <summary>
    /// CAPTCHA Image
    /// </summary>
    /// <seealso href="http://www.codinghorror.com">Original By Jeff Atwood</seealso>
    public class CaptchaImage
    {
        #region Constants and Fields

        /// <summary>
        /// 
        /// </summary>
        private static readonly Color[] RandomColor = { Color.Red, Color.Green, Color.Blue, Color.Black, Color.Purple, Color.Orange };

        /// <summary>
        /// 
        /// </summary>
        private static readonly string[] RandomFontFamily = { "arial", "arial black", "comic sans ms", "courier new", "estrangelo edessa", "franklin gothic medium", "georgia", "lucida console", "lucida sans unicode", "mangal", "microsoft sans serif", "palatino linotype", "sylfaen", "tahoma", "times new roman", "trebuchet ms", "verdana" };

        private readonly Random rand;

        private int height;

        private int width;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes the <see cref="CaptchaImage"/> class.
        /// </summary>
        static CaptchaImage()
        {
            FontWarp = FontWarpFactor.Extreme;
            BackgroundNoise = BackgroundNoiseLevel.High;
            LineNoise = LineNoiseLevel.None;
            TextLength = 5;
            TextChars = "ACDEFGHJKLNPQRTUVXYZ2346789";
            CacheTimeOut = 90D;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaImage"/> class.
        /// </summary>
        public CaptchaImage()
        {
            this.rand = new Random();
            Width = 180;
            Height = 50;
            Text = GenerateRandomText();
            RenderedAt = DateTime.Now;
            UniqueId = Guid.NewGuid().ToString("N");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets amount of background noise to apply to the <see cref="CaptchaImage"/> instance.
        /// </summary>
        /// <value>The background noise.</value>
        public static BackgroundNoiseLevel BackgroundNoise { get; set; }

        /// <summary>
        /// Gets or sets the cache time out.
        /// </summary>
        /// <value>The cache time out.</value>
        public static double CacheTimeOut { get; set; }

        /// <summary>
        /// Gets and sets amount of random warping to apply to the <see cref="CaptchaImage"/> instance.
        /// </summary>
        /// <value>The font warp.</value>
        public static FontWarpFactor FontWarp { get; set; }

        /// <summary>
        /// Gets or sets amount of line noise to apply to the <see cref="CaptchaImage"/> instance.
        /// </summary>
        /// <value>The line noise.</value>
        public static LineNoiseLevel LineNoise { get; set; }

        /// <summary>
        /// Gets or sets a string of available text characters for the generator to use.
        /// </summary>
        /// <value>The text chars.</value>
        public static string TextChars { get; set; }

        /// <summary>
        /// Gets or sets the length of the text.
        /// </summary>
        /// <value>The length of the text.</value>
        public static int TextLength { get; set; }

        /// <summary>
        /// Height of Captcha image to generate, in pixels
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get
            {
                return this.height;
            }

            set
            {
                if (value <= 30)
                {
                    throw new ArgumentOutOfRangeException("value", value, "height must be greater than 30.");
                }

                this.height = value;
            }
        }

        /// <summary>
        /// Returns the date and time this image was last rendered
        /// </summary>
        /// <value>The rendered at.</value>
        public DateTime RenderedAt { get; private set; }

        /// <summary>
        /// Gets the randomly generated Captcha text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Returns a GUID that uniquely identifies this Captcha
        /// </summary>
        /// <value>The unique id.</value>
        public string UniqueId { get; private set; }

        /// <summary>
        /// Width of Captcha image to generate, in pixels
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get
            {
                return this.width;
            }

            set
            {
                if (value <= 60)
                {
                    throw new ArgumentOutOfRangeException("value", value, "width must be greater than 60.");
                }

                this.width = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the cached captcha.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public static CaptchaImage GetCachedCaptcha(string guid)
        {
            if (String.IsNullOrEmpty(guid))
            {
                return null;
            }

            return (CaptchaImage)HttpRuntime.Cache.Get(guid);
        }

        /// <summary>
        /// Forces a new Captcha image to be generated using current property value settings.
        /// </summary>
        /// <returns></returns>
        public Bitmap RenderImage()
        {
            return GenerateImagePrivate();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a GraphicsPath containing the specified string and font
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="f">The f.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        private static GraphicsPath TextPath(string s, Font f, Rectangle r)
        {
            StringFormat sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            GraphicsPath gp = new GraphicsPath();
            gp.AddString(s, f.FontFamily, (int)f.Style, f.Size, r, sf);
            return gp;
        }

        /// <summary>
        /// Add variable level of curved lines to the image
        /// </summary>
        /// <param name="g">The graphics1.</param>
        /// <param name="rect">The rect.</param>
        // ReSharper disable UnusedMember.Local
        private void AddLine(Graphics g, Rectangle rect)
        {
            // ReSharper restore UnusedMember.Local
            int length;
            float f;
            int linecount;

            switch (LineNoise)
            {
                case LineNoiseLevel.None:
                    goto default;
                case LineNoiseLevel.Low:
                    length = 4;
                    f = Convert.ToSingle(this.height / 31.25);
                    linecount = 1;
                    break;
                case LineNoiseLevel.Medium:
                    length = 5;
                    f = Convert.ToSingle(this.height / 27.7777);
                    linecount = 1;
                    break;
                case LineNoiseLevel.High:
                    length = 3;
                    f = Convert.ToSingle(this.height / 25);
                    linecount = 2;
                    break;
                case LineNoiseLevel.Extreme:
                    length = 3;
                    f = Convert.ToSingle(this.height / 22.7272);
                    linecount = 3;
                    break;
                default:
                    return;
            }

            PointF[] pf = new PointF[length + 1];
            using (Pen p = new Pen(GetRandomColor(), f))
            {
                for (int l = 1; l <= linecount; l++)
                {
                    for (int i = 0; i <= length; i++)
                    {
                        pf[i] = RandomPoint(rect);
                    }

                    g.DrawCurve(p, pf, 1.75F);
                }
            }
        }

        /// <summary>
        /// Add a variable level of graphic noise to the image
        /// </summary>
        /// <param name="g">The graphics1.</param>
        /// <param name="rect">The rect.</param>
        // ReSharper disable UnusedMember.Local
        private void AddNoise(Graphics g, Rectangle rect)
        {
            // ReSharper restore UnusedMember.Local
            int density;
            int size;

            switch (BackgroundNoise)
            {
                case BackgroundNoiseLevel.None:
                    goto default;
                case BackgroundNoiseLevel.Low:
                    density = 30;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.Medium:
                    density = 18;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.High:
                    density = 16;
                    size = 39;
                    break;
                case BackgroundNoiseLevel.Extreme:
                    density = 12;
                    size = 38;
                    break;
                default:
                    return;
            }

            SolidBrush br = new SolidBrush(GetRandomColor());
            int max = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size);

            for (int i = 0; i <= Convert.ToInt32((rect.Width * rect.Height) / density); i++)
            {
                g.FillEllipse(br, this.rand.Next(rect.Width), this.rand.Next(rect.Height), this.rand.Next(max), this.rand.Next(max));
            }

            br.Dispose();
        }

        /// <summary>
        /// Renders the CAPTCHA image
        /// </summary>
        /// <returns></returns>
        private Bitmap GenerateImagePrivate()
        {
            Bitmap bmp = new Bitmap(this.width, this.height, PixelFormat.Format24bppRgb);

            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.Clear(ColorTranslator.FromHtml("#A2A2A2"));

                int charOffset = 0;

                // ReSharper disable PossibleLossOfFraction
                double charWidth = this.width / TextLength;

                // ReSharper restore PossibleLossOfFraction

                foreach (char c in Text)
                {
                    // establish font and draw area
                    using (Font fnt = new Font("trebuchet ms", 30, FontStyle.Bold))
                    {
                        using (Brush fontBrush = new SolidBrush(ColorTranslator.FromHtml("#595959")))
                        {
                            Rectangle rectChar = new Rectangle(Convert.ToInt32(charOffset * charWidth), 15, Convert.ToInt32(charWidth), this.height);

                            // warp the character
                            GraphicsPath gp = TextPath(c.ToString(), fnt, rectChar);

                            // 	WarpText(gp, rectChar);

                            // draw the character
                            gr.FillPath(fontBrush, gp);
                            charOffset += 1;
                        }
                    }
                }

                // Rectangle rect = new Rectangle(new Point(0, 0), bmp.Size);
                // AddNoise(gr, rect);
                // AddLine(gr, rect);
            }

            return bmp;
        }

        /// <summary>
        /// generate random text for the CAPTCHA
        /// </summary>
        /// <returns></returns>
        private string GenerateRandomText()
        {
            StringBuilder sb = new StringBuilder(TextLength);
            int maxLength = TextChars.Length;
            for (int n = 0; n <= TextLength - 1; n++)
            {
                sb.Append(TextChars.Substring(this.rand.Next(maxLength), 1));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the CAPTCHA font in an appropriate size
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private Font GetFont()
        {
            // ReSharper restore UnusedMember.Local
            float fsize;
            string fname = GetRandomFontFamily();

            switch (FontWarp)
            {
                case FontWarpFactor.None:
                    goto default;
                case FontWarpFactor.Low:
                    fsize = Convert.ToInt32(this.height * 0.8);
                    break;
                case FontWarpFactor.Medium:
                    fsize = Convert.ToInt32(this.height * 0.85);
                    break;
                case FontWarpFactor.High:
                    fsize = Convert.ToInt32(this.height * 0.9);
                    break;
                case FontWarpFactor.Extreme:
                    fsize = Convert.ToInt32(this.height * 0.95);
                    break;
                default:
                    fsize = Convert.ToInt32(this.height * 0.7);
                    break;
            }

            return new Font(fname, fsize, FontStyle.Bold);
        }

        /// <summary>
        /// Randoms the color.
        /// </summary>
        /// <returns></returns>
        private Color GetRandomColor()
        {
            return RandomColor[this.rand.Next(0, RandomColor.Length)];
        }

        /// <summary>
        /// Returns a random font family from the font whitelist
        /// </summary>
        /// <returns></returns>
        private string GetRandomFontFamily()
        {
            return RandomFontFamily[this.rand.Next(0, RandomFontFamily.Length)];
        }

        /// <summary>
        /// Returns a random point within the specified x and y ranges
        /// </summary>
        /// <param name="xmin">The xmin.</param>
        /// <param name="xmax">The xmax.</param>
        /// <param name="ymin">The ymin.</param>
        /// <param name="ymax">The ymax.</param>
        /// <returns></returns>
        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF(this.rand.Next(xmin, xmax), this.rand.Next(ymin, ymax));
        }

        /// <summary>
        /// Returns a random point within the specified rectangle
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        private PointF RandomPoint(Rectangle rect)
        {
            return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        /// <summary>
        /// Warp the provided text GraphicsPath by a variable amount
        /// </summary>
        /// <param name="textPath">The text path.</param>
        /// <param name="rect">The rect.</param>
        // ReSharper disable UnusedMember.Local
        private void WarpText(GraphicsPath textPath, Rectangle rect)
        {
            // ReSharper restore UnusedMember.Local
            float warpDivisor;
            float rangeModifier;

            switch (FontWarp)
            {
                case FontWarpFactor.None:
                    goto default;
                case FontWarpFactor.Low:
                    warpDivisor = 6F;
                    rangeModifier = 1F;
                    break;
                case FontWarpFactor.Medium:
                    warpDivisor = 5F;
                    rangeModifier = 1.3F;
                    break;
                case FontWarpFactor.High:
                    warpDivisor = 4.5F;
                    rangeModifier = 1.4F;
                    break;
                case FontWarpFactor.Extreme:
                    warpDivisor = 4F;
                    rangeModifier = 1.5F;
                    break;
                default:
                    return;
            }

            RectangleF rectF = new RectangleF(Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height);

            int hrange = Convert.ToInt32(rect.Height / warpDivisor);
            int wrange = Convert.ToInt32(rect.Width / warpDivisor);
            int left = rect.Left - Convert.ToInt32(wrange * rangeModifier);
            int top = rect.Top - Convert.ToInt32(hrange * rangeModifier);
            int xmax = rect.Left + rect.Width + Convert.ToInt32(wrange * rangeModifier);
            int ymax = rect.Top + rect.Height + Convert.ToInt32(hrange * rangeModifier);

            if (left < 0)
            {
                left = 0;
            }

            if (top < 0)
            {
                top = 0;
            }

            if (xmax > this.Width)
            {
                xmax = this.Width;
            }

            if (ymax > this.Height)
            {
                ymax = this.Height;
            }

            PointF leftTop = RandomPoint(left, left + wrange, top, top + hrange);
            PointF rightTop = RandomPoint(xmax - wrange, xmax, top, top + hrange);
            PointF leftBottom = RandomPoint(left, left + wrange, ymax - hrange, ymax);
            PointF rightBottom = RandomPoint(xmax - wrange, xmax, ymax - hrange, ymax);

            PointF[] points = new[] { leftTop, rightTop, leftBottom, rightBottom };
            Matrix m = new Matrix();
            m.Translate(0, 0);
            textPath.Warp(points, rectF, m, WarpMode.Perspective, 0);
        }

        #endregion
    }
}