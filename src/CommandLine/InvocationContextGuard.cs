using System;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;

namespace Ardalis.GuardClauses
{
    [SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "The guard clause parameters are not used in the methods but are required for usage.")]
    public static class InvocationContextGuard
    {
        public static void MissingInvocationContext(this IGuardClause guardClause, InvocationContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
        }
    }
}
