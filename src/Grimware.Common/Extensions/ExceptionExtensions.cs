using System;
using System.Globalization;
using System.Text;

namespace Grimware.Extensions
{
    public static class ExceptionExtensions
    {
        private const string _AppIdKey = ".appId";
        private const string _VerboseExceptionFormat = "{1}: {2}{0}{3}{4}";
        private const string _VerboseInnerExceptionFormat = "{0} ---> {1}{0}   {2}";
        private const string _EndOfInnerExceptionStackTrace = " End of InnerException Stack Trace";

        public static string ToStringVerbose(this Exception exception)
        {
            if (exception == null)
                return null;

            var builder = new StringBuilder();

            var innerVerbose = exception.InnerException.ToStringVerbose();

            builder.Append(
                AppDomain.CurrentDomain.GetData(_AppIdKey) is string appId
                    ? String.Concat(appId, Environment.NewLine)
                    : String.Empty);

            builder.AppendFormat(
                CultureInfo.InvariantCulture,
                _VerboseExceptionFormat,
                Environment.NewLine,
                exception.GetType().FullName,
                exception.Message,
                innerVerbose == null
                    ? String.Empty
                    : String.Format(
                        CultureInfo.InvariantCulture,
                        _VerboseInnerExceptionFormat,
                        Environment.NewLine,
                        innerVerbose,
                        _EndOfInnerExceptionStackTrace),
                exception.StackTrace);

            return builder.ToString();
        }
    }
}