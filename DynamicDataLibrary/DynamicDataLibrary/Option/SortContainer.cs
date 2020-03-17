using System;
using System.Collections.Generic;
using DynamicDataLibrary.ViewModel;

namespace DynamicDataLibrary.Option
{
    public sealed class SortContainer : IEquatable<SortContainer>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public SortContainer(string description, IComparer<ViewModelBase> comparer)
        {
            Description = description;
            Comparer = comparer;
        }

        public IComparer<ViewModelBase> Comparer { get; }

        public string Description { get; }

        #region Equality members

        public bool Equals(SortContainer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Description, other.Description);
        }

        ////public override bool Equals(object obj)
        ////{
        ////    if (ReferenceEquals(null, obj)) return false;
        ////    if (ReferenceEquals(this, obj)) return true;
        ////    if (obj.GetType() != GetType()) return false;
        ////    return Equals((SortContainer)obj);
        ////}

        ////public override int GetHashCode()
        ////{
        ////    return (Description != null ? Description.GetHashCode() : 0);
        ////}

        ////public static bool operator ==(SortContainer left, SortContainer right)
        ////{
        ////    return Equals(left, right);
        ////}

        ////public static bool operator !=(SortContainer left, SortContainer right)
        ////{
        ////    return !Equals(left, right);
        ////}

        #endregion
    }
}
