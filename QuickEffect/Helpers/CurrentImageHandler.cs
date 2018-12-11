using ImageFunctions;

namespace QuickEffect
{
    /// <summary>
    /// Helper class to handle image processing.
    /// Credits to Seleth Prakash on CodeProject.
    /// https://www.codeproject.com/Articles/237226/Image-Processing-is-done-using-WPF
    /// </summary>
    public class CurrentImageHandler : ImageHandler
    {
        public FilterHandler CurrentFilterHandler { get; set; }
        public GrayscaleHandler CurrentGrayscaleHandler { get; set; }
        public ImageFileHandler CurrentFileHandler { get; set; }
        public RotationHandler CurrentRotationHandler { get; set; }
        public SepiaToneHandler CurrentSepiaToneHandler { get; set; }

        public CurrentImageHandler() : base()
        {
            CurrentFilterHandler = new FilterHandler(this);
            CurrentGrayscaleHandler = new GrayscaleHandler(this);
            CurrentFileHandler = new ImageFileHandler(this);
            CurrentRotationHandler = new RotationHandler(this);
            CurrentSepiaToneHandler = new SepiaToneHandler(this);
        }
    }
}
