public static class DisplayUtils
{
  public static double CalculateXPercent(int q, int r, bool modHack = false)
  {

    double X = modHack
      ? DisplayConstants.Dimension * (DisplayConstants.Root3 * q + DisplayConstants.Root3 / 2.0 * (r % 2))
      : DisplayConstants.Dimension * (DisplayConstants.Root3 * q + DisplayConstants.Root3 / 2.0 * r);
    double xPercent = 100 * X / (double)DisplayConstants.MapWidth;
    return xPercent;
  }
  public static double CalculateYPercent(int r)
  {
    double Y = DisplayConstants.Dimension * ((3.0 / 2) * r);
    double yPercent = 100 * Y / (double)DisplayConstants.MapHeight;
    return yPercent;
  }
}