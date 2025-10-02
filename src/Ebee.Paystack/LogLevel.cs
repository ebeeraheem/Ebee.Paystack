namespace Ebee.Paystack;

/// <summary>
/// Defines the logging levels for Paystack SDK operations
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// No logging
    /// </summary>
    None,

    /// <summary>
    /// Log only errors
    /// </summary>
    Error,

    /// <summary>
    /// Log warnings and above
    /// </summary>
    Warning,

    /// <summary>
    /// Log informational messages and above
    /// </summary>
    Information,

    /// <summary>
    /// Log debug information and above
    /// </summary>
    Debug,

    /// <summary>
    /// Log all trace information
    /// </summary>
    Trace
}