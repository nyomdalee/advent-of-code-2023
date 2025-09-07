namespace Twelve.Models;

internal record ThumbprintKey(int[] Thumbprint, bool IsActive, int Length)
{
    public virtual bool Equals(ThumbprintKey? other)
    {
        if (other == null)
        {
            return false;
        }

        if (IsActive != other.IsActive || Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < Thumbprint.Length; i++)
        {
            if (Thumbprint[i] != other.Thumbprint[i])
            {
                return false;
            }
        }
        return true;
    }

    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(IsActive);
        hash.Add(Length);
        foreach (var t in Thumbprint)
        {
            hash.Add(t);
        }
        return hash.ToHashCode();
    }

}
