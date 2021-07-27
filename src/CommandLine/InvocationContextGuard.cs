using System;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;

namespace Ardalis.GuardClauses
{
    /// <summary>
    /// Provides guard clauses for a command line invocation context.
    /// </summary>
    [SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "The guard clause parameters are not used in the methods but are required for usage.")]
    public static class InvocationContextGuard
    {
        /// <summary>
        /// Ensures that an invocation context is not null.
        /// </summary>
        /// <param name="guardClause">The guard clause.</param>
        /// <param name="context">The invocation context.</param>
        public static void MissingInvocationContext(this IGuardClause guardClause, InvocationContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
        }
    }
}
