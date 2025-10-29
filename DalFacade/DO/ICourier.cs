namespace DO
{
    public interface ICourier
    {
        int Id { get; init; }

        void Deconstruct(out int Id);
        bool Equals(Courier? other);
        bool Equals(object? obj);
        int GetHashCode();
        string ToString();
    }
}