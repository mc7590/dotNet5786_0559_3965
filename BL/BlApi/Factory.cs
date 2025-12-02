namespace BlApi;
/// <summary>
/// This factory lets PL, which is above BL, can create objects of type BL without knowing the implementing class BlImplementation, but only IBl.
/// </summary>
public static class Factory
{
    public static IBl Get() => new BlImplementation.Bl();
}