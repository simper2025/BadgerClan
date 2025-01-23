public static class DisplayUtils
{
  public static double CalculateXPercent(int q, int r, int dimension, bool modHack = false)
  {
    double X = modHack
      ? DisplayConstants.Dimension * (DisplayConstants.Root3 * q + DisplayConstants.Root3 / 2.0 * (r % 2))
      : DisplayConstants.Dimension * (DisplayConstants.Root3 * q + DisplayConstants.Root3 / 2.0 * r);
    double xPercent = 100 * X / (double)DisplayConstants.MapWidth;

    double width = 100 * 1 / (double)dimension;
    return xPercent + (width * .75);
  }
  
  public static double CalculateYPercent(int r, int dimension)
  {
    double Y = DisplayConstants.Dimension * (3.0 / 2 * r);
    double yPercent = 100 * Y / (double)DisplayConstants.MapHeight;

    double width = 100 * 1 / (double)dimension;
    return yPercent + (width * .75);
  }
}