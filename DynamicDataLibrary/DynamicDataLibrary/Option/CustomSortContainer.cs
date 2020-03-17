namespace DynamicDataLibrary.Option
{
    using System;

    public sealed class CustomSortContainer : IComparable<CustomSortContainer>
    {
        public int CompareTo(CustomSortContainer other)
        {
            return 1;
        }
    }
}
