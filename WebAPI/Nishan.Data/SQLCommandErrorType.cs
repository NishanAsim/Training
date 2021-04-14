namespace Nishan.Data
{
    /// <summary>
    /// Type of parameter error in sql command
    /// </summary>
    public enum SqlCommandErrorType
    {
        /// <summary>
        /// Not known
        /// </summary>
        NotSet,
        /// <summary>
        /// Primary key value is duplicate
        /// </summary>
        DuplicateKey,
        /// <summary>
        ///Forign key violation
        /// </summary>
        InvalidReferenceValue
    }
}