// <copyright file="Matrix.cs" company="Wayne Venables">
//     Copyright (c) 2019 Wayne Venables. All rights reserved.
// </copyright>

using System;

namespace Anki.Vector.Types
{
    /// <summary>
    /// Simplified matrix struct
    /// </summary>
    public struct Matrix : IEquatable<Matrix>
    {
        /// <summary>
        /// The matrix data
        /// </summary>
        private readonly float[,] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix"/> struct.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <param name="cols">The cols.</param>
        public Matrix(int rows, int cols)
        {
            data = new float[rows, cols];
        }

        /// <summary>
        /// Gets the <see cref="System.Single"/> with the specified row.
        /// </summary>
        /// <value>
        /// The <see cref="System.Single"/>.
        /// </value>
        /// <param name="row">The row.</param>
        /// <param name="col">The col.</param>
        /// <returns>The specified matrix value</returns>
        public float this[int row, int col] => data[row, col];

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Matrix ? this.Equals((Matrix)obj) : false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Matrix other)
        {
            if (data.Length != other.data.Length) return false;
            if (data.GetLength(0) != other.data.GetLength(0)) return false;
            if (data.GetLength(1) != other.data.GetLength(1)) return false;
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if (!data[i, j].Equals(other.data[i, j])) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int result = 0;
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    result ^= data[i, j].GetHashCode();
                }
            }
            return result;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Matrix lhs, Matrix rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Matrix lhs, Matrix rhs)
        {
            return !(lhs == rhs);
        }
    }
}
