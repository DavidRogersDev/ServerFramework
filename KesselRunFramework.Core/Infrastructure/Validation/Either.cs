/*
 * This class was taken from this GitHub repo https://github.com/mikhailshilkov/mikhailio-samples
 * under an MIT licence.
 */

using System;

namespace KesselRunFramework.Core.Infrastructure.Validation
{

    /// <summary>
    /// Functional data data to represent a discriminated
    /// union of two possible types.
    /// </summary>
    /// <typeparam name="TL">Type of "Left" item.</typeparam>
    /// <typeparam name="TR">Type of "Right" item.</typeparam>
    public class Either<TL, TR>
    {
        private readonly TL _left;
        private readonly TR _right;
        private readonly bool _isLeft;

        public Either(TL left)
        {
            _left = left;
            _isLeft = true;
        }

        public Either(TR right)
        {
            _right = right;
            _isLeft = false;
        }

        public T Match<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc)
        {
            if (ReferenceEquals(leftFunc, null))
                throw new ArgumentNullException(nameof(leftFunc));

            if (ReferenceEquals(rightFunc, null))
                throw new ArgumentNullException(nameof(rightFunc));

            return _isLeft ? leftFunc(_left) : rightFunc(_right);
        }

        /// <summary>
        /// If right value is assigned, execute an action on it.
        /// </summary>
        /// <param name="rightAction">Action to execute.</param>
        public void DoRight(Action<TR> rightAction)
        {
            if (ReferenceEquals(rightAction, null))
                throw new ArgumentNullException(nameof(rightAction));

            if (!_isLeft)
                rightAction(_right);
        }

        public TL LeftOrDefault() => Match(l => l, r => default(TL));

        public TR RightOrDefault() => Match(l => default(TR), r => r);

        public static implicit operator Either<TL, TR>(TL left) => new Either<TL, TR>(left);

        public static implicit operator Either<TL, TR>(TR right) => new Either<TL, TR>(right);
    }
}
